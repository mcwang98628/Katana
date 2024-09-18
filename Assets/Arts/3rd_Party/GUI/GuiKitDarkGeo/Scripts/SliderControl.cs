using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour {

	public bool _isLoadingBar = false;
	public Text perText;
	Slider mySlider;
	void Start () {
		mySlider = GetComponent<Slider> ();
		StartCoroutine (SliderAnim ());
	}

	IEnumerator SliderAnim () {
		while (true) {
			float num = 2f;
			mySlider.DOValue (1f, num).SetEase (Ease.Linear);
			yield return new WaitForSeconds (num + 0.2f);
			mySlider.DOValue (0f, num).SetEase (Ease.Linear);
			yield return new WaitForSeconds (num);
		}
	}

	void Update () {
		if (_isLoadingBar) {
			float num = mySlider.value * 100;
			int result = (int) num;
			perText.text = result.ToString() + "%";
		}
	}

}
