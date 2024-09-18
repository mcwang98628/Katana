TGA
---

Thinking Game Analytics SDK.

## Installation

1. Put all the files in `Assets/TGA` folder
2. Attach `TGAnalytics.cs` to a game object
3. Configure app ids

## Initialization

### Automatically

1. Check `Auto Init` option
2. Use scripting define symbol `RELEASE_BUILD` to switch between debug app id and release app ids

### Manually

1. Uncheck `Auto Init` option
2. Initialize the SDK by your own way, for example:

```c#
#if UNITY_EDITOR || YOUR_OWN_DEFINE
		TGAnalytics.Init(TGAnalytics.Instance.debugAppId);
#elif UNITY_IOS
		TGAnalytics.Init(TGAnalytics.Instance.iOSAppId);
#elif UNITY_ANDROID
		TGAnalytics.Init(TGAnalytics.Instance.androidAppId);
#endif
```

## Tracking

```c#
TGAnalytics.Track(string eventName, Dictionary<string, object> properties = null);
```

## AB Testing

1. Increase the size of AB Tests configuration
2. Put `your_test_name` in `name` field
3. Put the amount of test versions in `amount` field

```c#
// Get test version
int version = TGAnalytics.GetAbVersion(string name);

// Reset test versions
TGAnalytics.ResetAbTests();
```
