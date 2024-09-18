using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Frozen_effect : BuffEffect
{
    
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        if (!roleBuff.roleController.IsDie)
        {
            // roleBuff.roleController.Animator.speed = 0;
            roleBuff.roleController.InputMove(new Vector2(0, 0));
            roleBuff.roleController.SetFreeze(true);
        }
    }

    public override void Update()
    {
        base.Update();
        
        // roleBuff.roleController.Animator.speed = 0;
        roleBuff.roleController.InputMove(new Vector2(0, 0));
    }

    public override void Destroy()
    {
        base.Destroy();
        if (!roleBuff.roleController.IsDie)
            roleBuff.roleController.SetFreeze(false);
    }
}
