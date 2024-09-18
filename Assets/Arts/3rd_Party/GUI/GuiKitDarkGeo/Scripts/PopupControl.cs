using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class PopupControl : MonoBehaviour {

	public GameObject popupPanel;
	public Image dim;
	public bool _isCloseOn = false;
	public GameObject buttonClose;

	void Awake () {
		dim.DOFade (0f, 0f);
		popupPanel.transform.DOScale (Vector3.zero, 0f);
		if(_isCloseOn)
			buttonClose.transform.DOScale (Vector3.zero, 0f);
		
	}

	IEnumerator Start () {
		dim.DOFade (1f, 0.5f).SetEase (Ease.OutCubic);
		yield return new WaitForSeconds (0.25f);
		popupPanel.transform.DOScale (Vector3.one, 0.25f).SetEase (Ease.OutCubic);
		if (_isCloseOn)
			buttonClose.transform.DOScale (Vector3.one, 0.25f).SetEase (Ease.OutCubic).SetDelay(1f);
	}
}
