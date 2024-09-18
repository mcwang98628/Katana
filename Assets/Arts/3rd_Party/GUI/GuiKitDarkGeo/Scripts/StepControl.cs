using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class StepControl : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {

	public Transform pos_01;
	public Transform pos_02;
	public Transform pos_03;

	public Transform easyPos;
	public Transform normalPos;
	public Transform hardPos;
	public Transform bossPos;
	private	bool _touchOn = false;
	private Vector3 pos;

	private Image myImage;

	public Text textEasy;
	public Text textNormal;
	public Text textHard;
	public Text textBoss;

	public Color textNormalColor;
	public Color textActiveColor;

	void Start () {
		myImage = GetComponent<Image> ();
	}

	public void OnDrag (PointerEventData data) {
		pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		if (pos.x < pos_01.position.x) {
			Easy ();
		} else if (pos.x >= pos_01.position.x && pos.x < pos_02.position.x) {
			Normal ();
		} else if (pos.x >= pos_02.position.x && pos.x < pos_03.position.x) {
			Hard ();
		} else if (pos.x >= pos_03.position.x) {
			Boss ();
		}
	}

	public void Easy () { 
		transform.DOMoveX (easyPos.position.x, 0);
		textEasy.color = textActiveColor;
		textNormal.color = textNormalColor;
		textHard.color = textNormalColor;
		textBoss.color = textNormalColor;
	}

	public void Normal () { 
		transform.DOMoveX (normalPos.position.x, 0);
		textEasy.color = textNormalColor;
		textNormal.color = textActiveColor;
		textHard.color = textNormalColor;
		textBoss.color = textNormalColor;
	}

	public void Hard () { 
		transform.DOMoveX (hardPos.position.x, 0);
		textEasy.color = textNormalColor;
		textNormal.color = textNormalColor;
		textHard.color = textActiveColor;
		textBoss.color = textNormalColor;
	}

	public void Boss () { 
		transform.DOMoveX (bossPos.position.x, 0);
		textEasy.color = textNormalColor;
		textNormal.color = textNormalColor;
		textHard.color = textNormalColor;
		textBoss.color = textActiveColor;
	}







	public void OnPointerDown (PointerEventData data) {
		myImage.DOKill ();
		myImage.DOColor (new Color (0.8f, 0.8f, 0.8f, 1f), 0.15f);
	}

	public void OnPointerUp (PointerEventData data) {
		myImage.DOKill ();
		myImage.DOColor (new Color (1f, 1f, 1f, 1f), 0.15f);
	}
}
