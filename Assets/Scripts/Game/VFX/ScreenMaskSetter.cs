using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenMaskSetter : MonoBehaviour
{
    //public GameObject ScreenMask;
    public Material _ScreenMaskMat;
    public Material _maskIns;
    public float FadeMul=0.05f;
    bool Fading;
    private void Start()
    {
        _ScreenMaskMat = GetComponent<Image>().material;
        _maskIns = Instantiate(_ScreenMaskMat);
        GetComponent<Image>().material = _maskIns;
        _maskIns.SetFloat("_Percentage", 0);
        //FadeIn();
    }
    public void SetPercentage(float Percentage)
    {
        _maskIns.SetFloat("_Percentage",Percentage);
    }
    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInIE());
    }
    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutIE());
    }


    public void  StopFading()
    {
        Fading = false;
    }

    public IEnumerator FadeInIE()
    {
        Fading = true;
        _maskIns.SetFloat("_Percentage",1);
        while (_maskIns.GetFloat("_Percentage")>-0.02f&&Fading)
        {
            _maskIns.SetFloat("_Percentage", Mathf.Lerp(_maskIns.GetFloat("_Percentage"),-0.1f,FadeMul));
            yield return null;
        }
        Fading = false;
    }
    public IEnumerator FadeOutIE()
    {
        Fading = true;
        //_maskIns.SetFloat("_Percentage", 1);
        while (_maskIns.GetFloat("_Percentage") < 1f && Fading)
        {
            _maskIns.SetFloat("_Percentage", Mathf.Lerp(_maskIns.GetFloat("_Percentage"), 1f, FadeMul));
            yield return null;
        }
        Fading = false;
    }
}
