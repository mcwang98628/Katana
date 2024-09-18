using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BGMController : MonoBehaviour
{
    public AudioClip BGMFight;
    public AudioClip BGMCalm;

    public float FadeTime;
    float fightBGMVolume = 0;
    public AudioSource BGMFightAS;
    public AudioSource BGMCalmAS;
    public float MaxFightBGMVolume;
    public float MaxCalmBGMVolume;

    Tweener fightBGMTween;
    Tweener calmBGMTween;
    private void Start()
    {
        BGMFightAS.clip = BGMFight;
        BGMFightAS.loop = true;
        BGMFightAS.volume = 0;
        BGMFightAS.Play();


        BGMCalmAS.clip = BGMCalm;
        BGMCalmAS.loop = true;
        BGMCalmAS.volume = 1;
        BGMCalmAS.Play();


        EventManager.Inst.AddEvent(EventName.UI_EnterNextRoom, PlayFightBGM);
        EventManager.Inst.AddEvent(EventName.ClearAllEnemy, PlayCalmBGM);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.UI_EnterNextRoom, PlayFightBGM);
        EventManager.Inst.RemoveEvent(EventName.ClearAllEnemy, PlayCalmBGM);

    }
    void PlayFightBGM(string arg, object roomInfo)
    {
        RoomData roomData = (RoomData)roomInfo;
        if (roomData.RoomType == RoomType.FightRoom ||
        roomData.RoomType == RoomType.BossFightRoom)
        {
            if (fightBGMTween != null)
                fightBGMTween.Kill();
            if (calmBGMTween != null)
                calmBGMTween.Kill();
            fightBGMTween = BGMCalmAS.DOFade(0, FadeTime);
            calmBGMTween = BGMFightAS.DOFade(MaxFightBGMVolume, FadeTime);
        }
    }
    void PlayCalmBGM(string arg, object roomInfo)
    {
        if (fightBGMTween != null)
            fightBGMTween.Kill();
        if (calmBGMTween != null)
            calmBGMTween.Kill();
        BGMCalmAS.DOFade(MaxCalmBGMVolume, FadeTime);
        BGMFightAS.DOFade(0, FadeTime);
    }
    // Update is called once per frame
}
