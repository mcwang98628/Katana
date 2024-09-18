using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackEffect : ItemEffect
{
    private bool OnUse;
    public AutoAttackEffect(bool value)
    {
        OnUse = value;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.GetComponent<RoleAuto>().enabled=OnUse;
    }

    public override void Destroy(RoleItemController rpe)
    {
        roleController.GetComponent<RoleAuto>().enabled=!OnUse;
        base.Destroy(rpe);
    }

}
