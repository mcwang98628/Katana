using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UI_MainPanel_BackgroundColor : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {
    //     SetColor(new Color(0.75f,0,0),0);
    // }
    public Image Halo;
    public void SetColor(Color bgColor,float duration=0.2f)
    {
        if (duration <= 0)
        {
            Shader.SetGlobalColor("_BackgroundColor",bgColor);
            Shader.SetGlobalColor("_IconColor",bgColor*0.9f);
        }

        DOTween.To(() => Shader.GetGlobalColor("_BackgroundColor"), value =>
        {
            Shader.SetGlobalColor("_BackgroundColor", value);
        },bgColor,duration);
        DOTween.To(() => Shader.GetGlobalColor("_IconColor"), value =>
        {
            Shader.SetGlobalColor("_IconColor", value);
        },bgColor*0.9f,duration);

        if (Halo != null)
        {
            Color haloColor = Color.white;
            float maxColorChannel = Mathf.Max(bgColor.r, bgColor.g, bgColor.b);
            if ((maxColorChannel - bgColor.r) * (maxColorChannel - bgColor.r) < 0.0001f)
            {
                haloColor = new Color(1.0f, 1.0f, 0f);
            }
            else if ((maxColorChannel - bgColor.g) * (maxColorChannel - bgColor.g) < 0.0001f)
            {
                haloColor = new Color(0.6f, 1.0f, 0.1f);
            }
            else
            {
                haloColor = new Color(0.5f, 1.0f, 1.0f);
            }
            
            haloColor.a = Halo.color.a;
            Halo.color = haloColor;
        }
    }



}
