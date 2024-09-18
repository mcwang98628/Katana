using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLuckEffect : ItemEffect
{
    private float Value;
    public SetLuckEffect(float value)
    {
        Value = value;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        DropManager.Inst.Luck += Value;
    }

    public override void Destroy(RoleItemController rpe)
    {
        DropManager.Inst.Luck -= Value;
        base.Destroy(rpe);
    }
 
}
