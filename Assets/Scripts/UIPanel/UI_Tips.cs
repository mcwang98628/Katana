using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tips : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public Image Bg;
    public Text Text;
    public AnimationCurve fadeCurve;
    private Tweener fadeTweener;
    private Coroutine coroutine;
    public void ShowText(string str)
    {
        CanvasGroup.alpha = 0;
        Text.text = str;

        if (fadeTweener != null)
        {
            fadeTweener.Kill();
        }
        fadeTweener = CanvasGroup.DOFade(1, 1.5f).SetEase(fadeCurve); 
        
        Bg.color = Color.white;
        Bg.DOColor(new Color(Color.red.r,Color.red.g,Color.red.b,0.5f), 0.5f);
        // if (coroutine != null)
        // {
        //     StopCoroutine(coroutine);
        // }
        // coroutine = StartCoroutine(waitHide());
    }

    // IEnumerator waitHide()
    // {
    //     yield return new WaitForSeconds(1f);
    //     fadeTweener = CanvasGroup.DOFade(0, 0.3f);
    // }
}
