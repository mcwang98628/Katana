using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeScreen : MonoBehaviour
{
    public Color HurtColor;
    public float LerpMul;
    public Image image;
    private void Start()
    {
        image = GetComponentInChildren<Image>();
    }
    //由不正常变回正常
    Tweener imgTweener;
    public void StartFadeIn(Color hurtColor,float Mul)
    {
        //StartCoroutine(FadeInIE(hurtColor,Mul));
        image.color = hurtColor;
        if (imgTweener != null)
        {
            imgTweener.Kill(true);
        }
        imgTweener = image.DOColor(new Color(hurtColor.r, hurtColor.g, hurtColor.b, 0), Mul);
    }
    //public IEnumerator FadeInIE(Color hurtColor,float mul)
    //{
    //    HurtColor = hurtColor;
    //    Color CurrentColor = hurtColor;
    //    while (CurrentColor.a>0)
    //    {
    //        //Debug.Log(CurrentColor.a-NormalColor.a);
    //        CurrentColor.a = Mathf.Lerp(CurrentColor.a,0,mul);
    //        image.color = CurrentColor;
    //        yield return null;
    //    }
    //    CurrentColor.a = 0;
    //}
}
