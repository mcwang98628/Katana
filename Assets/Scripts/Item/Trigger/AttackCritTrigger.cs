using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritTrigger : ItemEffectTrigger
{
    public AttackCritTrigger(TriggerType type) : base(type)
    {
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnPlayerAttackCrit,OnPlayerCrit);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnPlayerAttackCrit,OnPlayerCrit);
    }

    private void OnPlayerCrit(string arg1, object arg2)
    {
        var dmg = (DamageInfo) arg2;
        if (!dmg.IsCrit)
        {
            return;
        }
        roleItemController.AddCurrentFrameTrigger(() =>
        {
            Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue(){DamageInfo = dmg});
        });
    }
}
