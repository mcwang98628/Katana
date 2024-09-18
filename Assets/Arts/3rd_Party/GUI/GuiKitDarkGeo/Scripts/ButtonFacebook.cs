using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFacebook : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

	public Sprite imageFacebookPress;
	public Sprite imageFacebookNormal;

	public Color pressColor;
	public Color normalColor;

	public Text facebookText;
	public Image facebookIcon;

	public void OnPointerDown (PointerEventData data) {
		facebookIcon.sprite = imageFacebookPress;
		facebookText.color = pressColor;
	}

	public void OnPointerUp (PointerEventData data) {
		facebookIcon.sprite = imageFacebookNormal;
		facebookText.color = normalColor;
	}

	public void OnPointerExit (PointerEventData data) {
		facebookIcon.sprite = imageFacebookNormal;
		facebookText.color = normalColor;
	}
}
