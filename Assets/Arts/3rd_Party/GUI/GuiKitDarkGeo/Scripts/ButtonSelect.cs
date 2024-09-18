using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelect : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler {

	public Sprite bgNormal;
	public Sprite bgPress;

	public Sprite buttonNormal;
	public Sprite buttonPress;

	public Image button;
	public Image bg;

	public void OnPointerDown (PointerEventData data) {
		button.sprite = buttonPress;
		bg.sprite = bgPress;
	}

	public void OnPointerUp (PointerEventData data) {
		button.sprite = buttonNormal;
		bg.sprite = bgNormal;
	}

	public void OnPointerExit (PointerEventData data) {
		button.sprite = buttonNormal;
		bg.sprite = bgNormal;
	}
}
