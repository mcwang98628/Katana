using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NPCTalkPanel_TalkText : MonoBehaviour
{
    [SerializeField]
    private UIText text;

    private Action onHideCallBack = null;
    
    public void ShowText(string str,Action onHide)
    {
        text.text = str;
        onHideCallBack = onHide;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        if (onHideCallBack!=null)
        {
            onHideCallBack();
        }
    }
}
