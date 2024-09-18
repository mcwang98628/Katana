using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractObj_Chest_NPCTalk : InteractObj_Chest
{
    protected override void Init()
    {
        base.Init();
        EventManager.Inst.AddEvent(EventName.OnNpcTalkOver,OnTalkOver);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Inst.RemoveEvent(EventName.OnNpcTalkOver,OnTalkOver);
    }

    public override void InteractStart()
    {
        if (result)
        {
            return;
        }
        base.InteractStart();
    }

    public override void InteractEnd()
    {
        if (result)
        {
            return;
        }
        base.InteractEnd();
    }

    private void OnTalkOver(string arg1, object arg2)
    {
        if (result)
        {
            OpenChestAnim();
        }
    }

}
