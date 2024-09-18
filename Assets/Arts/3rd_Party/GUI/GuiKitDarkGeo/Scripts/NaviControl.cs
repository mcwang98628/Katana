using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class NaviControl : MonoBehaviour {

	public int naviNum = 0;
	public ButtonNavi _activeButtons;
	public List<ButtonNavi> buttons = new List<ButtonNavi> ();
	public string [] panelName;
	public Transform panelTransform;
	public GameObject activePanels;

	public Color defultColor;
	public Color activeColor;

	void Start () {
		foreach (Transform t in transform.Find ("Buttons").transform){
			buttons.Add (t.GetComponent<ButtonNavi>());
		}
		_activeButtons = buttons [naviNum];
		PanelLoad ();
		Reset ();
		buttons [naviNum]._isOn = true;
		buttons [naviNum].ButtonActive ();
	}

	public void PanelLoad () {

		if (activePanels != null)
			Destroy (activePanels);
		
		GameObject panels = (GameObject) Instantiate (Resources.Load ("Panels/" + panelName [naviNum]), new Vector3 (0f, 0f, 0f), Quaternion.identity);
		activePanels = panels;
		panels.transform.SetParent (panelTransform, false);
		panels.name = panelName [naviNum];

	}

	public void ButtonLeftClick () {
		if (naviNum > 0) {
			Reset ();
			naviNum -= 1;
			buttons [naviNum].ButtonActive ();
			PanelLoad ();
		}
	}

	public void ButtonRightClick () {
		if (naviNum < buttons.Count - 1) {
			Reset ();
			naviNum += 1;
			PanelLoad ();
			buttons [naviNum].ButtonActive ();
		}
	}

	public void Reset () {
		for (int i = 0; i < buttons.Count; i++) {
			buttons [i].ButtonReset ();
		}
	}
}
