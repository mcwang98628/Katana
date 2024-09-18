using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleMoveTrigger : ItemEffectTrigger
{
    private float moveTime;
    public RoleMoveTrigger(TriggerType type,float moveTime) : base(type)
    {
        this.moveTime = moveTime;
    }

    private float timer;
    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        if (!roleItemController.roleController.IsMoving)
        {
            return;
        }
        
        timer += Time.deltaTime;
        if (timer >= moveTime)
        {
            timer -= moveTime;
            roleItemController.AddCurrentFrameTrigger(()=>{
                Root.itemEffect.TriggerEffect(null);
                
            });
        }
    }
}
