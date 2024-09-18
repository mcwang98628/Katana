using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideRoomEvent : MonoBehaviour
{
    public ToturialGuideType CurrentType;
    public float GuideWaitTime;
    public void OnEnable()
    {
        if (CurrentType == ToturialGuideType.Move)
        {
            GameManager.Inst.StartCoroutine(waitOpenPanel(GuideWaitTime));   
        }
    }

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.EnterNextRoom,OnEnterNextRoom);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.EnterNextRoom,OnEnterNextRoom);
    }

    private void OnEnterNextRoom(string arg1, object arg2)
    {
        if (CurrentType == ToturialGuideType.Click && BattleManager.Inst.IsShowToturialClick)
        {
            return;
        }

        if (CurrentType == ToturialGuideType.Click && !BattleManager.Inst.IsShowToturialClick)
        {
            BattleManager.Inst.SetShowToturialClick();
        }
        
        if (CurrentType != ToturialGuideType.Move)
        {
             GameManager.Inst.StartCoroutine(waitOpenPanel(GuideWaitTime));   
        }
        EventManager.Inst.RemoveEvent(EventName.EnterNextRoom,OnEnterNextRoom);
    }

    IEnumerator waitOpenPanel(float waitTime)
    {
        while (!BattleManager.Inst.GameIsRuning)
        {
            yield return null;
        }
        yield return null;
        yield return new WaitForSeconds(waitTime);
        if (CurrentType == ToturialGuideType.Skill)
        {
            EventManager.Inst.DistributeEvent(EventName.SkillGuide);
        }
        else
        {
            UIManager.Inst.Open("ToturialPanel",false,CurrentType);
        }
    }
}
