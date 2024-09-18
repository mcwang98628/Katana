using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThinkingAnalytics.Wrapper;
using ThinkingAnalytics.Utils;
using ThinkingAnalytics;
using UnityEngine;

#if ADJUST_ENABLED
using com.adjust.sdk;
#endif

public class TGAnalytics : MonoBehaviour {
	public string iOSAppId = "5129808b12494e1d8180f761353798d6";
	public string androidAppId = "";
	public string debugAppId = "5129808b12494e1d8180f761353798d6";
	public string serverUrl = "https://receiver.habby.mobi";
	public ThinkingAnalyticsAPI.NetworkType networkType = ThinkingAnalyticsAPI.NetworkType.ALL;
	public ThinkingAnalyticsAPI.TAMode mode = ThinkingAnalyticsAPI.TAMode.NORMAL;
	public bool autoInit = true;
	public bool enableLog = true;
	public AbTest[] abTests;

	public static string AppId { get; private set; }
	public static TGAnalytics Instance { get; private set; }

	private const string TimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

	private static ThinkingAnalyticsWrapper _taWrapper;
	private static Dictionary<string, int> _abVersions;
	private static DateTime _installTime;
	private static bool _initialized;
	private static bool _running;

	[Serializable]
	public struct AbTest {
		public string name;
		public int amount;
	}

	public void AwakeInit() {
		Instance = this;
		if (autoInit) {
#if UNITY_EDITOR || !RELEASE_BUILD
			Init(debugAppId);
#elif UNITY_IOS
			Init(iOSAppId);
#elif UNITY_ANDROID
			Init(androidAppId);
#endif
		}
	}

	public static void Init(string appId) {
		if (_initialized) {
			return;
		}

		var token = new ThinkingAnalyticsAPI.Token(appId, Instance.serverUrl, Instance.mode,
			ThinkingAnalyticsAPI.TATimeZone.Local);
		_taWrapper = new ThinkingAnalyticsWrapper(token);
		_taWrapper.SetNetworkType(Instance.networkType);
		_initialized = true;
		AppId = appId;

		if (PlayerPrefs.HasKey("first_active")) {
			try {
				_installTime = DateTime.Parse(PlayerPrefs.GetString("first_active_time"));
			} catch (Exception) {
				_installTime = DateTime.MinValue;
				PlayerPrefs.SetString("first_active_time", _installTime.ToString(TimeFormat));
				PlayerPrefs.Save();
			}
		} else {
			_installTime = DateTime.UtcNow;
			PlayerPrefs.SetInt("first_active", 1);
			PlayerPrefs.SetString("first_active_time", _installTime.ToString(TimeFormat));
			PlayerPrefs.Save();
			Track("first_active");
		}

		Log("Init", appId);
		Track("app_launch");
		TimeEvent("app_quit");

		Instance.OnApplicationFocus(true);

		InitAbTests();
#if ADJUST_ENABLED
        InitAdjust();
#endif
	}

	private static void InitAbTests(bool reset = false) {
		var abTests = Instance.abTests;
		var savedAbVersions = new Dictionary<string, object>();
		_abVersions = new Dictionary<string, int>();

		if (PlayerPrefs.HasKey("ab_versions")) {
			savedAbVersions = TD_MiniJSON.Deserialize(PlayerPrefs.GetString("ab_versions"));
		}

		foreach (var abTest in abTests) {
			var version = -1;
			var abTestName = abTest.name;
			var abTestAmount = abTest.amount;
			if (abTestAmount <= 0) {
				continue;
			}

			if (savedAbVersions.ContainsKey(abTestName)) {
				if (reset) {
					savedAbVersions.Remove(abTestName);
				} else {
					var savedVersion = int.Parse(savedAbVersions[abTestName].ToString());
					if (savedVersion < abTestAmount) {
						version = savedVersion;
					}
				}
			}

			if (version < 0) {
				version = UnityEngine.Random.Range(0, abTest.amount);
				savedAbVersions.Add(abTestName, version);
			}

			_abVersions.Add(abTestName, version);
		}

		if (savedAbVersions.Count > 0) {
			var userAbVersions = savedAbVersions
				.Select(abVersion => FormatField(abVersion.Key + "_" + abVersion.Value))
				.ToList();
			UserSet("ab_versions", userAbVersions);
		}

		PlayerPrefs.SetString("ab_versions", TD_MiniJSON.Serialize(savedAbVersions));
	}

#if ADJUST_ENABLED
    private string _adid;
    private string _networkName;
    private static bool _attributed;

    private static void InitAdjust()
    {
        if (_attributed)
        {
            return;
        }

        Adjust.addSessionCallbackParameter("tga_device_id", _taWrapper.getDeviceId());
        Adjust.addSessionCallbackParameter("tga_distinct_id", _taWrapper.GetDistinctId());
        Instance.Attribute();
    }

    private void Attribute()
    {
        _attributed = true;
        AdjustConfig adjustConfig = new AdjustConfig(Constants.AdjustAppToken, AdjustEnvironment.Production);
        adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
        adjustConfig.setAttributionChangedDelegate(this.AttributionChangedDelegate);
        Adjust.start(adjustConfig);
        StartCoroutine(GetAdid());
    }

    private void AttributionChangedDelegate(AdjustAttribution attribution)
    {
        var superProperties = new Dictionary<string, object>();
        if (attribution.adid != null)
        {
            _adid = attribution.adid;
            superProperties.Add("adjust_adid", attribution.adid);
        }

        if (attribution.network != null)
        {
            _networkName = attribution.network;
            superProperties.Add("adjust_network_name", attribution.network);
        }

        SetSuperProperties(superProperties);
    }

    private IEnumerator GetAdid()
    {
        while (_adid == null)
        {
            yield return new WaitForSeconds(1);
            _adid = Adjust.getAdid();
        }

        var superProperties = new Dictionary<string, object>
        {
            {"adjust_adid", _adid}, {"adjust_network_name", _networkName}
        };
        SetSuperProperties(superProperties);
        yield return null;
    }
#endif

	private static void Log(string action, string info = "") {
		if (Instance.enableLog) {
			Debug.Log("TGA: " + action + "\n" + info);
		}
	}

	private static void Log(string action, ICollection properties) {
		Log(action, properties.Count > 0 ? TD_MiniJSON.Serialize(properties) : "");
	}

	private static void WrapProperties(IDictionary<string, object> properties) {
		var now = DateTime.UtcNow;
		properties.Add("installed_days", (now - _installTime).Days);
		properties.Add("event_time_utc", now.ToString(TimeFormat));
		now = now.AddHours(8);
		properties.Add("event_time_beijing", now.ToString(TimeFormat));
		properties.Add("guid", Guid.NewGuid().ToString());
	}

	public static string FormatField(string name) {
		name = Regex.Replace(name, @"[0-9A-Z]+", match => $"_{match.Value.ToLower()}");
		name = Regex.Replace(name, @"[ _]+", "_");
		name = Regex.Replace(name, @"^_+", "");
		return name;
	}

	public static int GetAbVersion(string name) {
		CheckInitialization();
		if (!_abVersions.ContainsKey(name)) {
			throw new Exception("AB test `" + name + "` does not exists.");
		}

		return _abVersions[name];
	}

	public static void ResetAbTests() {
		CheckInitialization();
		Log("ResetAbTests");
		InitAbTests(true);
	}

	public static void Identify(string uniqueId) {
		CheckInitialization();
		Log("Identify", uniqueId);
		_taWrapper.Identify(uniqueId);
	}

	public static string GetDistinctId() {
		CheckInitialization();
		return _taWrapper.GetDistinctId();
	}

	public static string GetDeviceId() {
		CheckInitialization();
		return _taWrapper.GetDeviceId();
	}

	public static void Login(string account) {
		CheckInitialization();
		Log("Login", account);
		_taWrapper.Login(account);
	}

	public static void Logout() {
		CheckInitialization();
		Log("Logout");
		_taWrapper.Logout();
	}

	public static void Flush() {
		CheckInitialization();
		Log("Flush");
		_taWrapper.Flush();
	}

	public static void Track(string eventName, Dictionary<string, object> properties = null) {
		CheckInitialization();
		if (properties == null) {
			properties = new Dictionary<string, object>();
		}

		Log($"Track ({eventName})", properties);
		WrapProperties(properties);
		_taWrapper.Track(eventName, properties);
	}

	public static void Track(string eventName, Dictionary<string, object> properties, DateTime date) {
		CheckInitialization();
		if (properties == null) {
			properties = new Dictionary<string, object>();
		}

		Log($"Track ({eventName})", properties);
		WrapProperties(properties);
		_taWrapper.Track(eventName, properties, date);
	}

	public static void SetSuperProperties(Dictionary<string, object> superProperties) {
		CheckInitialization();
		Log("SetSuperProperties", superProperties);
		_taWrapper.SetSuperProperties(superProperties);
	}

	public static void UnsetSuperProperty(string property) {
		CheckInitialization();
		Log("UnsetSuperProperty");
		_taWrapper.UnsetSuperProperty(property);
	}

	public static Dictionary<string, object> GetSuperProperties() {
		CheckInitialization();
		return _taWrapper.GetSuperProperties();
	}

	public static void ClearSuperProperties() {
		CheckInitialization();
		Log("ClearSuperProperties");
		_taWrapper.ClearSuperProperty();
	}

	public static void TimeEvent(string eventName) {
		CheckInitialization();
		Log($"TimeEvent ({eventName})");
		_taWrapper.TimeEvent(eventName);
	}

	public static void UserSet(string property, object value) {
		CheckInitialization();
		var properties = new Dictionary<string, object> {
			{property, value}
		};
		UserSet(properties);
	}

	public static void UserSet(Dictionary<string, object> properties) {
		CheckInitialization();
		Log("UserSet", properties);
		_taWrapper.UserSet(properties);
	}

	public static void UserSet(Dictionary<string, object> properties, DateTime dateTime) {
		CheckInitialization();
		Log("UserSet", properties);
		_taWrapper.UserSet(properties, dateTime);
	}

	public static void UserUnset(string property) {
		CheckInitialization();
		var properties = new List<string> {property};
		UserUnset(properties);
	}

	public static void UserUnset(List<string> properties) {
		CheckInitialization();
		Log("UserUnset", properties);
		_taWrapper.UserUnset(properties);
	}

	public void UserUnset(List<string> properties, DateTime dateTime) {
		CheckInitialization();
		Log("UserUnset", properties);
		_taWrapper.UserUnset(properties, dateTime);
	}

	public static void UserSetOnce(Dictionary<string, object> properties) {
		CheckInitialization();
		Log("UserSetOnce", properties);
		_taWrapper.UserSetOnce(properties);
	}

	public void UserSetOnce(Dictionary<string, object> properties, DateTime dateTime) {
		CheckInitialization();
		Log("UserSetOnce", properties);
		_taWrapper.UserSetOnce(properties, dateTime);
	}

	public static void UserAdd(string property, object value) {
		CheckInitialization();
		var properties = new Dictionary<string, object> {
			{property, value}
		};
		UserAdd(properties);
	}

	public static void UserAdd(Dictionary<string, object> properties) {
		CheckInitialization();
		Log("UserAdd", properties);
		_taWrapper.UserAdd(properties);
	}

	public void UserAdd(Dictionary<string, object> properties, DateTime dateTime) {
		CheckInitialization();
		Log("UserAdd", properties);
		_taWrapper.UserAdd(properties, dateTime);
	}

	public static void UserAppend(string property, object value) {
		CheckInitialization();
		var properties = new Dictionary<string, object> {
			{property, value}
		};
		UserAppend(properties);
	}

	public static void UserAppend(Dictionary<string, object> properties) {
		CheckInitialization();
		Log("UserAppend", properties);
		_taWrapper.UserAppend(properties);
	}


	public void UserAppend(Dictionary<string, object> properties, DateTime dateTime) {
		CheckInitialization();
		Log("UserAppend", properties);
		_taWrapper.UserAppend(properties, dateTime);
	}

	public static void UserDelete() {
		CheckInitialization();
		Log("UserDelete");
		_taWrapper.UserDelete();
	}

	public void UserDelete(DateTime dateTime) {
		CheckInitialization();
		Log("UserDelete");
		_taWrapper.UserDelete(dateTime);
	}

	public static void SetNetworkType(ThinkingAnalyticsAPI.NetworkType networkType) {
		CheckInitialization();
		Log("SetNetworkType", networkType.ToString());
		_taWrapper.SetNetworkType(networkType);
	}

	public static void SetDynamicSuperProperties(IDynamicSuperProperties dynamicSuperProperties) {
		CheckInitialization();
		Log("SetDynamicSuperProperties", dynamicSuperProperties.GetDynamicSuperProperties());
		_taWrapper.SetDynamicSuperProperties(dynamicSuperProperties);
	}

	public static void OptOutTracking() {
		CheckInitialization();
		Log("OptOutTracking");
		_taWrapper.OptOutTracking();
	}

	public static void OptOutTrackingAndDeleteUser() {
		CheckInitialization();
		Log("OptOutTrackingAndDeleteUser");
		_taWrapper.OptOutTrackingAndDeleteUser();
	}

	public static void OptInTracking() {
		CheckInitialization();
		Log("OptInTracking");
		_taWrapper.OptInTracking();
	}

	public static void EnableTracking(bool enabled) {
		CheckInitialization();
		Log("EnableTracking", enabled.ToString());
		_taWrapper.EnableTracking(enabled);
	}

	public static void CalibrateTime(long timestamp) {
		ThinkingAnalyticsWrapper.CalibrateTime(timestamp);
	}

	public static void CalibrateTimeWithNtp(string ntpServer) {
		ThinkingAnalyticsWrapper.CalibrateTimeWithNtp(ntpServer);
	}

	private static void CheckInitialization() {
		if (!_initialized) {
			throw new Exception("TGA SDK not initialized!");
		}
	}

	private void OnApplicationFocus(bool focused) {
		if (!_initialized) {
			return;
		}

		if (focused && !_running) {
			_running = true;
			Track("ta_app_start");
			TimeEvent("ta_app_end");
			Track("app_start");
			TimeEvent("app_end");
		} else if (!focused && _running) {
			_running = false;
			Track("ta_app_end");
			Track("app_end");
		}
	}

	private void OnApplicationQuit() {
		if (!_initialized) {
			return;
		}

		OnApplicationFocus(false);
		Track("app_quit");
	}
}