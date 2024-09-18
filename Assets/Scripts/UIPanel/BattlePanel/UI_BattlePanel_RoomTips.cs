using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePanel_RoomTips : MonoBehaviour
{    
    [SerializeField]
    private Text tipsText;
    [SerializeField]
    private CanvasGroup canvasGroup;

    private bool isPlaying = false;
    List<string> waitList = new List<string>();
    [SerializeField]private AudioClip soundOnShow;

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.FightRoomWaveTips,FightRoomWaveTips);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.FightRoomWaveTips,FightRoomWaveTips);
    }

    private void FightRoomWaveTips(string arg1, object arg2)
    {
        Init((string)arg2);
        PlaySound();
    }

    private void PlaySound()
    {
        if(soundOnShow)
        {
            AudioManager.Inst.PlaySource(soundOnShow);
        }
    }
    public void Init(string tips)
    {
        if (isPlaying)
        {
            waitList.Add(tips);
        }

        isPlaying = true;
        tipsText.text = tips;
        canvasGroup.alpha = 0;
        
        canvasGroup.DOFade(1, 0.2f);
        transform.localScale = Vector3.one * 5f;
        transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
        {
            // transform.DOScale(Vector3.one * 3, 0.3f).SetDelay(2f).Delay();
            canvasGroup.DOFade(0, 0.2f).OnComplete(() =>
            {
                isPlaying = false;
                PlayOver();
            }).SetDelay(3f).Delay();
        });
    }

    void PlayOver()
    {
        if (waitList.Count>0)
        {
            string tips = waitList[0];
            waitList.RemoveAt(0);
            Init(tips);
        }
    }
}
