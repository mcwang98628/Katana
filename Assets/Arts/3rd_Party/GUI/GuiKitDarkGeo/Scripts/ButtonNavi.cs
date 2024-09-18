using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonNavi : MonoBehaviour, IPointerUpHandler, IPointerExitHandler, IPointerDownHandler, IPointerEnterHandler {

	public bool _isOn = false;
	public bool _isPointerUp = false;

	[HideInInspector]
	private Image myImage;
	private int myNum;
	private NaviControl _NaviControl;
	Text myText;
	
	public Color defultColor;
	public Color activeColor;
	public GameObject myTextBox;

	void Awake () {
		myText = transform.Find ("Text_Panel_Num").gameObject.GetComponent<Text> ();
		myNum = int.Parse (this.gameObject.name);
		myImage = GetComponent<Image> ();
		myImage.DOFade (0f, 0f);
		_NaviControl = GameObject.FindWithTag ("NaviControl").GetComponent<NaviControl> ();
	}

	public void OnPointerUp (PointerEventData data) {
		if (!_isOn && _isPointerUp) { 
			_NaviControl._activeButtons.ButtonReset ();
			_NaviControl._activeButtons = GetComponent<ButtonNavi> ();
			_NaviControl.Reset ();
			ButtonActive ();
		}
	}

	public void OnPointerDown (PointerEventData data) {
		myImage.DOFade (1f, 0f);
	}


	public void OnPointerEnter (PointerEventData data) {
		myTextBox.SetActive (true);
		
		if (!_isOn) {
			_isPointerUp = true;
			myImage.DOFade (0.8f, 0f);
		}
	}

	public void OnPointerExit (PointerEventData data) {
		myTextBox.SetActive (false);
		
		if (!_isOn) {
			_isPointerUp = false;
			myImage.DOFade (0f, 0f);
		}
	}

	public void ButtonActive () {
		_isOn = true;
		_NaviControl.naviNum = myNum;
		_NaviControl.PanelLoad ();
		_NaviControl._activeButtons = GetComponent<ButtonNavi>();
		myImage.DOFade (0.8f, 0f);
		myText.color = new Color (activeColor.r, activeColor.g, activeColor.b, 1f);
		Particle ();
	}

	public void ButtonReset () {
		myImage.DOFade (0f, 0f);
		myText.color = new Color (defultColor.r, defultColor.g, defultColor.b, 1f);
		_isOn = false;
	}

	private void Particle () { 
		GameObject fx = (GameObject) Instantiate (Resources.Load ("Fx_Navi_Effect"), new Vector3 (transform.position.x, transform.position.y, 0f), Quaternion.identity);
		Destroy (fx.gameObject, 1f);
	}
}
