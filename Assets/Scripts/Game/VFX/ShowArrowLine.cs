using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowArrowLine : MonoBehaviour
{
    public LineRenderer _renderer;
    Material _mat;
    Material Mat;
    public float FadeSpeed;
    public void ShowLine()
    {
        if(_mat==null)
        {
            _mat = GetComponent<LineRenderer>().material;
            Mat = _mat;
            GetComponent<LineRenderer>().material = Mat;
        }
        StartCoroutine(ShowLineIE());
    }
    public IEnumerator ShowLineIE()
    {
        //float StartTime = Time.time;
        //_mat.SetFloat("_Transparency", 1);
        //while (_mat.GetFloat("_Transparency")>0.01f)
        //{
        //    _mat.SetFloat("_Transparency",Mathf.Lerp(_mat.GetFloat("_Transparency"),0,FadeSpeed));
        //    yield return null;
        //}
        float StartTime = Time.time;
        Mat.SetFloat("_Transparency", 1);
        while (Mat.GetFloat("_Transparency") > 0.01f)
        {
            Mat.SetFloat("_Transparency", Mathf.Lerp(Mat.GetFloat("_Transparency"), 0, FadeSpeed));
            yield return null;
        }
    }
}
