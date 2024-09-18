using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class KillEnemyTrigger : ItemEffectTrigger
{
    private int times;//计数

    private int Value;
    public KillEnemyTrigger(TriggerType type, int value) : base(type)
    {
        Value = value;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleDead,OnEnemyKill);
    }
    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead,OnEnemyKill);
    }
    private void OnEnemyKill(string arg1, object roleDeadEventData)
    {
        RoleDeadEventData eventData = (RoleDeadEventData) roleDeadEventData;
        if (BattleManager.Inst.CurrentPlayer.TemporaryId == (string)eventData.DeadRole.TemporaryId)
        {
            return;
        }

        switch (triggerType)
        {
            case TriggerType.Probability:
                int value = Random.Range(0, 100);
                if (value <= Value)
                {
                    // roleItemController.AddCurrentFrameTargetIdTrigger(eventData.DeadRole.TemporaryId,()=>{Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue(){TargetId = eventData.DeadRole.TemporaryId});});
                    Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue()
                    {
                        TargetId = eventData.DeadRole.TemporaryId,
                        TargetPosition = eventData.DeadRole.transform.position,
                        TargetRole = eventData.DeadRole
                    });
                }
                break;
            case TriggerType.Times:
                times++;
                if (times>=Value)
                {
                    times -= Value;
                    // roleItemController.AddCurrentFrameTargetIdTrigger(eventData.DeadRole.TemporaryId,()=>{Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue(){TargetId = eventData.DeadRole.TemporaryId});});
                    Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue()
                    {
                        TargetId = eventData.DeadRole.TemporaryId,
                        TargetPosition = eventData.DeadRole.transform.position,
                        TargetRole = eventData.DeadRole
                    });
                }
                break;
        }
    }
}
