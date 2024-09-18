using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInjuryMultiplierEffect : ItemEffect
{
    private float Value;
    private List<float> effectValue = new List<float>();
    
    public SetInjuryMultiplierEffect(float value)
    {
        Value = value;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.SetInjuryMultiplier(Value);
        effectValue.Add(Value);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        foreach (float value in effectValue)
        {
            roleController.SetUnInjuryMultiplier(value);
        }
        effectValue.Clear();
    }
 
}
