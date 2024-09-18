using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonScale : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

	public Transform move;
	public void OnPointerDown (PointerEventData data) {
		transform.DOScale (Vector3.one * 1.03f, 0.1f);
	}

	public void OnPointerUp (PointerEventData data) {
		transform.DOScale (Vector3.one, 0.1f);
		move.DOMoveX(transform.position.x,0.3f);
	}

}
