using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class SwitchOnOff : MonoBehaviour {

	public Sprite sprite_OffButton;
	public Sprite sprite_OnButton;
	public Color color_On;
	public Color color_Off;

	public Text onoffText;
	public Image buttonImage;

	public bool _isOn = false;
	public void ButtonClick () {
		if (_isOn) {
			_isOn = false;
			onoffText.color = color_Off;
			buttonImage.sprite = sprite_OffButton;
			buttonImage.transform.DOKill ();
			buttonImage.transform.DOLocalMoveX (-62, 0.1f).SetEase (Ease.InOutCubic);

			onoffText.transform.DOKill ();
			onoffText.transform.DOLocalMoveX (24.9f, 0.1f).SetEase (Ease.InOutCubic);
			onoffText.text = "OFF";

		} else {
			_isOn = true;
			onoffText.color = color_On;
			buttonImage.sprite = sprite_OnButton;
			buttonImage.transform.DOKill ();
			buttonImage.transform.DOLocalMoveX (62, 0.1f).SetEase (Ease.InOutCubic);

			onoffText.transform.DOKill ();
			onoffText.transform.DOLocalMoveX (-24.9f, 0.1f).SetEase (Ease.InOutCubic);
			onoffText.text = "ON";
		}
	}

}
