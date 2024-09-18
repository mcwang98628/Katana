using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollTrigger : ItemEffectTrigger
{
    private float rollTime;
    public RollTrigger(TriggerType type,float time_) : base(type){
        this.rollTime = time_;
    }

    //计时器
    private float timer;
    private bool onEnterRoll = false;
    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        if (!roleItemController.roleController.IsRolling)
        {
            onEnterRoll = false;
            return;
        }

        if (!onEnterRoll)
        {
            onEnterRoll = true;
            timer = rollTime;
        }
        
        timer += Time.deltaTime;
        if (timer >= rollTime)
        {
            timer -= rollTime;
            roleItemController.AddCurrentFrameTrigger(()=>{
                Root.itemEffect.TriggerEffect(null);
                
            });
        }
    }
}
