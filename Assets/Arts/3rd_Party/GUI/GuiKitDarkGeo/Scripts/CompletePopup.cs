using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class CompletePopup : MonoBehaviour {

	public GameObject popupPanel;
	public Image dim;


	public Text textComplete;
	public Text textScore;
	public Text textScoreNum;

	public GameObject [] stars;
	public GameObject [] buttons;

	public Transform pos_01;
	public Transform pos_02;
	public Transform pos_03;

	void Awake () { 
		popupPanel.transform.DOScale (Vector3.zero, 0);
		textComplete.DOFade (0f, 0f);
		textScore.DOFade (0f, 0f);
		textScoreNum.DOFade (0f, 0f);
		


		dim.DOFade (0f, 0f);
	}

	IEnumerator Start () {
		yield return new WaitForSeconds (0.2f);
		dim.DOFade (1f, 0.5f).SetEase (Ease.InOutCubic);
		yield return new WaitForSeconds (0.3f);

		popupPanel.transform.DOScale (Vector3.one, 0.35f).SetEase (Ease.OutBack);
		yield return new WaitForSeconds (0.3f);

		StartCoroutine (ButtonScale ());
		textComplete.DOFade (1f, 0.5f).SetEase (Ease.Linear);
		yield return new WaitForSeconds (0.5f);
		StartCoroutine (Score ());

	}

	IEnumerator Score () {
		textScore.DOFade (1f, 0.5f).SetEase (Ease.Linear);
		textScoreNum.DOFade (1f, 0.5f).SetEase (Ease.Linear);
		
		for (int i = 0; i < 678600; i++) {
			i += 11111;
			textScore.text = ChangeScore (i);
			yield return null;
		}

		StartCoroutine (InfoNum ());
		
	}

	IEnumerator StarScale () {
		stars [0].transform.DOScale (Vector3.one, 0.2f).SetEase (Ease.OutBack);
		GameObject fx_01 = (GameObject) Instantiate (Resources.Load ("FX_Complete_star_1"), new Vector3 (pos_01.position.x, pos_01.position.y, pos_01.position.z), Quaternion.identity);
		yield return new WaitForSeconds (0.35f);

		stars [1].transform.DOScale (Vector3.one, 0.2f).SetEase (Ease.OutBack);
		GameObject fx_02 = (GameObject) Instantiate (Resources.Load ("FX_Complete_star_2"), new Vector3 (pos_02.position.x, pos_02.position.y, pos_02.position.z), Quaternion.identity);
		yield return new WaitForSeconds (0.35f);

		stars [2].transform.DOScale (Vector3.one, 0.2f).SetEase (Ease.OutBack);
		GameObject fx_03 = (GameObject) Instantiate (Resources.Load ("FX_Complete_star_3"), new Vector3 (pos_03.position.x, pos_03.position.y, pos_03.position.z), Quaternion.identity);
		yield return new WaitForSeconds (1f);
		Destroy (fx_01.gameObject);
		Destroy (fx_02.gameObject);
		Destroy (fx_03.gameObject);
	}

	IEnumerator ButtonScale () { 
		for (int i = 0; i < buttons.Length; i++) {
			buttons [i].transform.DOScale (Vector3.one, 0.15f).SetEase (Ease.OutBack);
			yield return null;
		}
	}

	public Text death;
	public Text length;
	public Text money;

	IEnumerator InfoNum () { 
		for (int d = 0; d < 94; d++) {
			d += 11;
			death.text = d.ToString();
			yield return null;
		}
		for (float l = 0; l < 1.6f; l++) {
			l += 0.1f;
			length.text = l.ToString () + "km";
			yield return null;
		}
		for (int m = 0; m < 100; m++) {
			m += 11;
			money.text = m.ToString ();
			yield return null;
		}
		StartCoroutine (StarScale ());
	}


	static string ChangeScore (int myScore) {
		return string.Format ("{0:n0}", myScore);
	}
}
