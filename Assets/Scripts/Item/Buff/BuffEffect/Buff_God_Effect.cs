using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_God_Effect : BuffEffect
{
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        roleBuff.roleController.SetGod(true);
        // Debug.LogError("God");
    }

    public override void Destroy()
    {
        base.Destroy();
        roleBuff.roleController.SetGod(false);
        // Debug.LogError("God_NO");
    }
}
