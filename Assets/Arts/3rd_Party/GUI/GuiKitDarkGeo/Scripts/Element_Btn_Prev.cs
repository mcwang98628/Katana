using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Element_Btn_Prev : MonoBehaviour, IPointerClickHandler{

	NaviControl _NaviControl;

	void Awake () {
		_NaviControl = GameObject.FindWithTag ("NaviControl").gameObject.GetComponent<NaviControl> ();
	}

	public void OnPointerClick (PointerEventData data) {
		_NaviControl.ButtonLeftClick ();
	}

}
