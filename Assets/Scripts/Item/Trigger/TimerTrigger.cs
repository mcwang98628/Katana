using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTrigger : ItemEffectTrigger
{
    private float timer = 0;
    private RoleItemController rpe_;
    private bool isOver = false;
    private float timeValue;

    public TimerTrigger(TriggerType type, float time) : base(type)
    {
        timeValue = time;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        rpe_ = rpe;
    }

    public override void Update(RoleItemController rpe)
    {
        if (isOver)
        {
            return;
        }
        base.Update(rpe);
        timer += Time.deltaTime;
        if (timer >= timeValue)
        {
            timer -= timeValue;
            roleItemController.AddCurrentFrameTrigger(()=>{
                Root.itemEffect.TriggerEffect(null);
                
            });
        }
    }

}
