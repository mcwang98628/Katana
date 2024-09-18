using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour {

	public Image textPause;

	public GameObject button_01;
	public GameObject button_02;
	public GameObject button_03;

	public Text text_button_01;
	public Text text_button_02;
	public Text text_button_03;

	public Image dim;

	void Awake () {
		dim.DOFade (0f, 0f);
		textPause.DOFade (0, 0f);
		button_01.transform.DOScale (Vector3.zero, 0f);
		button_02.transform.DOScale (Vector3.zero, 0f);
		button_03.transform.DOScale (Vector3.zero, 0f);
		text_button_01.DOFade (0f, 0f);
		text_button_02.DOFade (0f, 0f);
		text_button_03.DOFade (0f, 0f);
	}

	IEnumerator Start () {
		dim.DOFade (1f, 0.5f).SetEase (Ease.OutCubic);
		textPause.DOFade (1f, 0.2f).SetEase (Ease.OutCubic).SetDelay (0.2f);
		yield return new WaitForSeconds (0.25f);
		button_01.transform.DOScale (Vector3.one, 0.25f).SetEase (Ease.OutBack);
		button_02.transform.DOScale (Vector3.one, 0.25f).SetEase (Ease.OutBack).SetDelay(0.15f);
		button_03.transform.DOScale (Vector3.one, 0.25f).SetEase (Ease.OutBack).SetDelay(0.3f);

		text_button_01.DOFade (1f, 0.2f).SetEase (Ease.OutCubic).SetDelay(0.15f);
		text_button_02.DOFade (1f, 0.2f).SetEase (Ease.OutCubic).SetDelay(0.3f);
		text_button_03.DOFade (1f, 0.2f).SetEase (Ease.OutCubic).SetDelay(0.45f);
	}
}
