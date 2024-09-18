using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagEnabler : MonoBehaviour
{
    public string Tag;
    public GameObject TargetObj;

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleTagChange,OnTagChange);
    }

    private void Start()
    {
        OnTagChange(null,null);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleTagChange,OnTagChange);
    }

    private void OnTagChange(string arg1, object arg2)
    {
        
        if(BattleManager.Inst.CurrentPlayer != null && 
           BattleManager.Inst.CurrentPlayer.GetTagCount(Tag)>0)
        {
            TargetObj.SetActive(true);
            DmgBuffOnTouch dmgBuffOnTouch = TargetObj.GetComponent<DmgBuffOnTouch>();
            if (dmgBuffOnTouch != null)
            {
                dmgBuffOnTouch.Init(BattleManager.Inst.CurrentPlayer,BattleManager.Inst.CurrentPlayer.AttackPower*0.3f);
            }
        }
        else
        {
            TargetObj.SetActive(false);
        }
    }
}
