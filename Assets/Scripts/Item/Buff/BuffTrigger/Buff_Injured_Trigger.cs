using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Injured_Trigger : BuffTrigger
{
    public Buff_Injured_Trigger()
    {
    }

    public override void Awake()
    {
        base.Awake();
        EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnRoleInjured);
    }

    public override void Destroy()
    {
        base.Destroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnRoleInjured);
    }

    private void OnRoleInjured(string arg1, object id)
    {
        if (roleBuff.roleController.TemporaryId != ((RoleInjuredInfo)id).RoleId)
        {
            return;
        }

        roleBuff.TriggerEffect();
    }
}
