using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_InjuryMultiplier_Effect : BuffEffect
{
    private float InjuryMultiplierValue;


    private List<float> effectValue = new List<float>();
    public Buff_InjuryMultiplier_Effect(float value)
    {
        InjuryMultiplierValue = value;
    }
    
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        roleBuff.roleController.SetInjuryMultiplier(InjuryMultiplierValue);
        effectValue.Add(InjuryMultiplierValue);
    }

    public override void Destroy()
    {
        base.Destroy();
        foreach (float value in effectValue)
        {
            roleBuff.roleController.SetUnInjuryMultiplier(value);
        }
        effectValue.Clear();
    }
    
}
