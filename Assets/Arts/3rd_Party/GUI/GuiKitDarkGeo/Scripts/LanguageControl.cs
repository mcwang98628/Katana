using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LanguageControl : MonoBehaviour {

	GameObject activeLanguage;

	public GameObject [] selectLangs;
	public GameObject [] notselectLangs;
	public GameObject langPanel;
	public Text langText;

	bool _isPanleOn = false;
	public GameObject blockPanel;

	void Start () {
		ClosePanle ();
	}

	public void LangPanelOpen () {
		if (!_isPanleOn) {
			_isPanleOn = true;
			langPanel.SetActive (true);
			blockPanel.SetActive (true);
		} else {
			ClosePanle ();
			blockPanel.SetActive (false);
		}
	}

	public void Button_FRA () {
		ClosePanle ();
		ResetNotSelectLangs ();
		notselectLangs [0].SetActive (false);
		ResetSelectLangs ();
		selectLangs [0].SetActive (true);
		langText.text = "FRA";
	}

	public void Button_KOR () { 
		ClosePanle ();
		ResetNotSelectLangs ();
		notselectLangs [1].SetActive (false);
		ResetSelectLangs ();
		selectLangs [1].SetActive (true);
		langText.text = "KOR";
	}

	public void Button_CHN () { 
		ClosePanle ();
		ResetNotSelectLangs ();
		notselectLangs [2].SetActive (false);
		ResetSelectLangs ();
		selectLangs [2].SetActive (true);
		langText.text = "CHN";
	}

	public void Button_JPN () { 
		ClosePanle ();
		ResetNotSelectLangs ();
		notselectLangs [3].SetActive (false);
		ResetSelectLangs ();
		selectLangs [3].SetActive (true);
		langText.text = "JPN";
	}

	public void Button_USA () {
		ClosePanle ();
		ResetNotSelectLangs ();
		notselectLangs [4].SetActive (false);
		ResetSelectLangs ();
		selectLangs [4].SetActive (true);
		langText.text = "USA";
	}

	void ClosePanle () {
		langPanel.SetActive (false);
		_isPanleOn = false;
		blockPanel.SetActive (false);
	}

	void ResetNotSelectLangs () { 
		for (int i = 0; i < notselectLangs.Length; i++) {
			notselectLangs [i].SetActive (true);
		}
	}

	void ResetSelectLangs () {
		for (int i = 0; i < selectLangs.Length; i++) {
			selectLangs [i].SetActive (false);
		}
	}

	public void BlockPanelClick () {
		ClosePanle ();
		blockPanel.SetActive (false);
	}

}
