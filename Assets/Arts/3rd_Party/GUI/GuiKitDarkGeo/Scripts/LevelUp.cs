using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour {

	public bool _isCloseOn = false;
	public GameObject buttonClose;

	public Image imageLevel;
	public Image imageUp;
	public Image imageLeaf_Left;
	public Image imageLeaf_Right;

	public Image imageFrame_01;
	public Image imageFrame_02;
	public Text levelNum;
	public Image backLight;

	public Text textUnLock;
	public Image imageDeco_Left;
	public Image imageDeco_Right;

	public GameObject buttonTime;
	public GameObject buttonShield;
	public GameObject buttonKey;

	void Awake () {
		//Level
		imageLevel.transform.DOScale (Vector3.zero, 0f);
		imageLevel.DOFade (0f, 0f);

		//Up
		imageUp.transform.DOScale (Vector3.zero, 0f);
		imageUp.DOFade (0f, 0f);

		//Leaf
		imageLeaf_Left.transform.DOScale (Vector3.zero, 0f);
		imageLeaf_Right.transform.DOScale (Vector3.zero, 0f);

		//CloseButton
		buttonClose.transform.DOScale (Vector3.zero, 0f);


		//Frame & LevelNum
		imageFrame_01.DOFade (0f, 0f);
		imageFrame_01.transform.DOScale (Vector3.one * 3f, 0f);
		imageFrame_02.DOFade (0f, 0f);
		imageFrame_02.transform.DOScale (Vector3.one * 3f, 0f);
		levelNum.transform.DOScale (Vector3.one * 3f, 0f);
		levelNum.DOFade (0f, 0f);

		//unlocked & deco
		textUnLock.DOFade (0f, 0f).SetEase (Ease.OutCubic);
		imageDeco_Left.transform.DOScale (0f, 0f);
		imageDeco_Right.transform.DOScale (0f, 0f);

		//buttonItem
		buttonTime.transform.DOScale (Vector3.zero, 0f);
		buttonShield.transform.DOScale (Vector3.zero, 0f);
		buttonKey.transform.DOScale (Vector3.zero, 0f);


		//backlight
		backLight.DOFade (0f, 0f);

	}

	IEnumerator Start () {
		yield return new WaitForSeconds (0.3f);
		imageLevel.transform.DOScale (Vector3.one, 0.2f).SetEase(Ease.OutBack);
		imageLevel.DOFade (1f, 0.1f).SetEase(Ease.OutCubic);

		//Up
		imageUp.transform.DOScale (Vector3.one, 0.2f).SetEase(Ease.OutBack).SetDelay(0.1f);
		imageUp.DOFade (1f, 0.1f).SetEase (Ease.OutCubic).SetDelay (0.1f);

		//Leaf
		imageLeaf_Left.transform.DOScale (Vector3.one, 0.15f).SetDelay(0.15f).SetEase(Ease.OutBack);
		imageLeaf_Right.transform.DOScale (Vector3.one, 0.15f).SetDelay (0.15f).SetEase (Ease.OutBack);

		//Frame & LevelNum
		levelNum.transform.DOScale (Vector3.one, 0.15f).SetEase (Ease.OutExpo);
		levelNum.DOFade (1f, 0.1f);

		yield return new WaitForSeconds (0.15f);
		imageFrame_01.DOFade (1f, 0.1f);
		imageFrame_01.transform.DOScale (Vector3.one, 0.18f).SetEase (Ease.OutExpo);
		
		imageFrame_02.DOFade (1f, 0.1f);
		imageFrame_02.transform.DOScale (Vector3.one, 0.18f).SetEase(Ease.OutExpo);

		backLight.DOFade (1f, 1f);

		yield return new WaitForSeconds (0.15f);
		//unlocked & deco
		textUnLock.DOFade (1f, 0.1f).SetEase (Ease.OutCubic);
		imageDeco_Left.transform.DOScale (1f, 0.1f).SetEase (Ease.OutBack).SetDelay(0.1f);
		imageDeco_Right .transform.DOScale (1f, 0.1f).SetEase (Ease.OutBack).SetDelay (0.1f);
		yield return new WaitForSeconds (0.15f);

		//buttonItem
		buttonTime.transform.DOScale (Vector3.one, 0.2f).SetEase (Ease.OutBack).SetDelay(0.1f);
		buttonShield.transform.DOScale (Vector3.one, 0.2f).SetEase (Ease.OutBack).SetDelay (0.2f);
		buttonKey.transform.DOScale (Vector3.one, 0.2f).SetEase (Ease.OutBack).SetDelay (0.3f);


		yield return new WaitForSeconds (1f);
		buttonClose.transform.DOScale (Vector3.one, 0.25f).SetEase (Ease.OutCubic);



	}

	void Update () {
		backLight.transform.Rotate (Vector3.back * 60f * Time.deltaTime);
	}

}
