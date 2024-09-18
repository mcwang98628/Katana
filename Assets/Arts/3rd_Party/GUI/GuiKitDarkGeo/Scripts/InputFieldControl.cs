using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputFieldControl : MonoBehaviour {

	public GameObject inputText_01;
	public GameObject inputText_02;

	public Color enterColor;
	public Color exitColor;

	public void Input_01_Change () {
		inputText_01.SetActive (false);
		inputText_01.GetComponent<Text> ().color = enterColor;
	}

	public void Input_02_Change () {
		inputText_02.SetActive (false);
		inputText_02.GetComponent<Text> ().color = enterColor;
	}


	public void Input_01_End () {
		inputText_01.SetActive (true);
		inputText_01.GetComponent<Text> ().color = exitColor;
	}

	public void Input_02_End () {
		inputText_02.SetActive (true);
		inputText_02.GetComponent<Text> ().color = exitColor;
	}
}
