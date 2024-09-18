using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgBuffOnTouchHitEnemyTrigger : ItemEffectTrigger
{
    private int Value;

    public DmgBuffOnTouchHitEnemyTrigger(TriggerType type, int value) : base(type)
    {
        Value = value;
        
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnDmgBuffOnTouchHitRole,OnHitRole);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnDmgBuffOnTouchHitRole,OnHitRole);
    }

    private void OnHitRole(string arg1, object dmg)
    {
        if (triggerType == TriggerType.Probability)
        {
            int value = Random.Range(0, 100);
            if (value > Value)
            {
                return;
            }
        }

        var dmgdata = (DamageInfo) dmg;
        Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue(){TargetId = dmgdata.HitRoleId});
    }
}
