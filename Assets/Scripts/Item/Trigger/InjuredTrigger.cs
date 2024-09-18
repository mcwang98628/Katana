using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class InjuredTrigger : ItemEffectTrigger
{
    private int times;//计数
    private int Value;

    public InjuredTrigger(TriggerType type, int value) : base(type)
    {
        Value = value;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnRoleInjured);
        // EventManager.Inst.AddEvent(EventName.OnRoleGodInjured,OnRoleInjured);
    }
    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnRoleInjured);
        // EventManager.Inst.RemoveEvent(EventName.OnRoleGodInjured,OnRoleInjured);
    }
    private void OnRoleInjured(string arg1, object id)
    {
        if (BattleManager.Inst.CurrentPlayer.TemporaryId != ((RoleInjuredInfo)id).RoleId)
        {
            return;
        }

        if (((RoleInjuredInfo)id).Dmg.DmgValue < BattleManager.Inst.CurrentPlayer.MaxHp*0.05f)
        {
            return;
        }

        switch (triggerType)
        {
            case TriggerType.Probability:
                int value = Random.Range(0, 100);
                if (value <= Value)
                {
                    roleItemController.AddCurrentFrameTrigger(()=>{Root.itemEffect.TriggerEffect(null);});
                }
                break;
            case TriggerType.Times:
                times++;
                if (times>=Value)
                {
                    times -= Value;
                    roleItemController.AddCurrentFrameTrigger(()=>{Root.itemEffect.TriggerEffect(null);});
                }
                break;
        }
    }
}


public class RoleDeadTrigger : ItemEffectTrigger
{
    public RoleDeadTrigger(TriggerType type) : base(type)
    {
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleDead,OnRoleDead);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead,OnRoleDead);
    }

    private void OnRoleDead(string arg1, object arg2)
    {
        RoleDeadEventData data = (RoleDeadEventData) arg2;
        Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue(){TargetId = data.DeadRole.TemporaryId});
    }
}