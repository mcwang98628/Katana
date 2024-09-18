using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractObj_NPC_YesNo : InteractObj_NPC
{
    protected bool result = false;
    protected override void Init()
    {
        base.Init();
        EventManager.Inst.AddEvent(EventName.OnNpcTalkYesOrNoEvent, OnChoose);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Inst.RemoveEvent(EventName.OnNpcTalkYesOrNoEvent, OnChoose);
    }

    public override void InteractEnd()
    {
        if (result)
        {
            ChooseYes();
        }
        else
        {
            ChooseNo();
        }
    }
    protected virtual void ChooseYes()
    {
        if (FinishParticle)
            FinishParticle.Play();
        if (FinishAudio)
            AudioManager.Inst.PlaySource(FinishAudio, 1.5f);
            
        if(CanRepeat)
            StartCoroutine(DelaySetCanIntact(true, .4f));
        talked = true;
    }
    protected virtual void ChooseNo()
    {
        StartCoroutine(DelaySetCanIntact(true, .4f));
        talked = false;
    }
    void OnChoose(string str, object value)
    {
        var eventData = (NpcTalkYesOrNoEventData)value;
        result = eventData.IsOk;
    }
}
