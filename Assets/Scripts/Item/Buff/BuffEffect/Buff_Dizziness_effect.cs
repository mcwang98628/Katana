using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Dizziness_effect : BuffEffect
{
    
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        roleBuff.roleController.SetDizziness(true);
    }

    public override void Destroy()
    {
        base.Destroy();
        roleBuff.roleController.SetDizziness(false);
    }
}
