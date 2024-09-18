using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour {
	private void Start() {
		/* Manual initialization */
		// TGAnalytics.Init(TGAnalytics.Instance.debugAppId);
		
		TGAnalytics.Track("event_name", new Dictionary<string, object> {
			{"IsWin", true},
			{"Gold",99},
		});
		TGAnalytics.Track("AppStart");
		Debug.Log("ABTest Version: " + TGAnalytics.GetAbVersion("test"));
		TGAnalytics.ResetAbTests();
		Debug.Log("ABTest Version: " + TGAnalytics.GetAbVersion("test"));
	}
}