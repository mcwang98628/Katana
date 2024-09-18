using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSurroundSpeedEffect : ItemEffect
{
    private float value;

    public AddSurroundSpeedEffect(float value)
    {
        this.value = value;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value2)
    {
        base.TriggerEffect(value2);
        roleController.roleSurroundController.AddSurroundSpeed(value);
    }
}
