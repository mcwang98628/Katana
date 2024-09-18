using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_ExpBar : MonoBehaviour
{
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Image expBar;

    [SerializeField]
    private Text addNumberText;
    
    
    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnArchiveExpChanage,OnExpChanage);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnArchiveExpChanage,OnExpChanage);
    }

    private void Update()
    {
        UpdateText();
    }

    private void OnExpChanage(string arg1, object arg2)
    {
        UpdateText();
        ShowNumberTips((int)arg2);
    }

    void UpdateText()
    {
        if (ArchiveManager.Inst.ArchiveData == null)
        {
            return;
        }
        
        levelText.text = ArchiveManager.Inst.ArchiveData.GlobalData.CurrentLevel.ToString();
        expBar.fillAmount = ArchiveManager.Inst.ArchiveData.GlobalData.CurrentLevelExpPercentage;
    }
    
    void ShowNumberTips(int num)
    {
        if (num<=0)
            return;
        
        var numText = GameObject.Instantiate(addNumberText, transform);
        numText.gameObject.SetActive(true);
        numText.text = "+ " + num;
        numText.transform.localPosition = new Vector3(20, -70, 0);
        numText.transform.DOLocalMoveY(0, 0.5f).SetDelay(3f).Delay();
        numText.DOColor(new Color(1, 1, 1, 0), 0.5f).OnComplete(() =>
        {
            GameObject.Destroy(numText.gameObject);
        }).SetDelay(1.5f).Delay();
    }
}
