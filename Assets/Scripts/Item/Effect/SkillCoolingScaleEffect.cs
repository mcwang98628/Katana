using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCoolingScaleEffect : ItemEffect
{
    private float value;
    private int triggerTimes;
    public SkillCoolingScaleEffect(float value)
    {
        this.value = value;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value2)
    {
        base.TriggerEffect(value2);
        roleController.MultiplySkillCoolingScale(value);
        triggerTimes++;
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        for (int i = 0; i < triggerTimes; i++)
        {
            roleController.ExceptSkillCoolingScale(value);
        }
    }
}
