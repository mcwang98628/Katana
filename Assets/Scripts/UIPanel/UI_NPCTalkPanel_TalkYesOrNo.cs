// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class UI_NPCTalkPanel_TalkYesOrNo : MonoBehaviour
// {
//     [SerializeField]
//     private UIText text;
//
//     private Action<bool> callBack = null;
//     
//     public void ShowText(string str,Action<bool> onclick)
//     {
//         text.text = str;
//         callBack = onclick;
//         gameObject.SetActive(true);
//     }
//
//     public void OnBtnClick(bool isOk)
//     {
//         gameObject.SetActive(false);
//         if (callBack==null)
//         {
//             return;
//         }
//         callBack(isOk);
//     }
// }
