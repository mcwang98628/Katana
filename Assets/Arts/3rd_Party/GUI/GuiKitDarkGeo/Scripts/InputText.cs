using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Color exitColor;
	public Color enterColor;

	private Text myText;

	void Start () {
		myText = GetComponent<Text> ();
	}

	public void OnPointerEnter (PointerEventData data) {
		myText.color = enterColor;
	}


	public void OnPointerExit (PointerEventData data) { 
		myText.color = exitColor;
	}

}
