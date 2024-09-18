using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Checkbox : MonoBehaviour {

	public GameObject onPoint;
	public GameObject offPoint;

	bool _isOn = false;

	void Awake () {
		if (_isOn) {
			_isOn = false;
			onPoint.transform.DOScale (Vector3.zero, 0.1f).SetEase (Ease.InBack);
			offPoint.transform.DOScale (Vector3.one, 0.15f).SetEase (Ease.OutBack);
		} else { 
			_isOn = true;
			onPoint.transform.DOScale (Vector3.one, 0.15f).SetEase (Ease.OutBack);
			offPoint.transform.DOScale (Vector3.zero, 0.1f).SetEase (Ease.InBack);
		}
	}

	public void ClickOff () {
		if (_isOn) {
			_isOn = false;
			onPoint.transform.DOKill ();
			offPoint.transform.DOKill ();

			onPoint.transform.DOScale (Vector3.zero, 0.1f).SetEase (Ease.InBack);
			offPoint.transform.DOScale (Vector3.one, 0.15f).SetEase (Ease.OutBack);
		}
	}

	public void ClickOn () {
		if (!_isOn) { 
			_isOn = true;
			onPoint.transform.DOKill ();
			offPoint.transform.DOKill ();

			onPoint.transform.DOScale (Vector3.one, 0.15f).SetEase (Ease.OutBack);
			offPoint.transform.DOScale (Vector3.zero, 0.1f).SetEase (Ease.InBack);
		}
	}
}
