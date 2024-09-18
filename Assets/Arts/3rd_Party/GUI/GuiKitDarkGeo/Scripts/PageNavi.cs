using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PageNavi : MonoBehaviour {

	public GameObject naviIcon;

	public Transform navi_01;
	public Transform navi_02;
	public Transform navi_03;

	public void PageNavi_01 () {
		naviIcon.transform.DOLocalMoveX (navi_01.localPosition.x, 0f);
	}

	public void PageNavi_02 () {
		naviIcon.transform.DOLocalMoveX (navi_02.localPosition.x, 0f);
	}

	public void PageNavi_03 () {
		naviIcon.transform.DOLocalMoveX (navi_03.localPosition.x, 0f);
	}
}
