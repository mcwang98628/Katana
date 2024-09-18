using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class StagePopupControl : MonoBehaviour {

	public Image [] buttons;
	public Sprite activeBg;
	public Sprite normalBg;

	public void Button_01 () {
		ButtonReset ();
		buttons [0].sprite = activeBg;
	}

	public void Button_02 () { 
		ButtonReset ();
		buttons [1].sprite = activeBg;
	}

	public void Button_03 () {
		ButtonReset ();
		buttons [2].sprite = activeBg;
	}

	void ButtonReset () {
		for (int i = 0; i < buttons.Length; i++) {
			buttons [i].sprite = normalBg;
		}
	}
}
