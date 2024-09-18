using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleDeadOfBuffTrigger : ItemEffectTrigger
{
    private int BuffId;

    public RoleDeadOfBuffTrigger(TriggerType type, int buffId) : base(type)
    {
        BuffId = buffId;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleDead,OnRoleDead);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead,OnRoleDead);
    }

    private void OnRoleDead(string arg1, object arg2)
    {
        var data = (RoleDeadEventData) arg2;
        for (int i = 0; i < data.DeadRole.roleBuffController.Buffs.Count; i++)
        {
            if (data.DeadRole.roleBuffController.Buffs[i].ID == BuffId)
            {
                roleItemController.AddCurrentFrameTargetIdTrigger(data.DeadRole.TemporaryId ,()=>{
                    Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue(){TargetId = data.DeadRole.TemporaryId});
                });
                return;
            }
        }
    }
}
