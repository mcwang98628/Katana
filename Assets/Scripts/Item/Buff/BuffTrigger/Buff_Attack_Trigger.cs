using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Attack_Trigger : BuffTrigger
{
    public Buff_Attack_Trigger()
    {
    }

    public override void Awake()
    {
        base.Awake();
        EventManager.Inst.AddEvent(EventName.OnRoleAttack,OnRoleAttack);
    }

    public override void Destroy()
    {
        base.Destroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleAttack,OnRoleAttack);
    }

    private void OnRoleAttack(string arg1, object id)
    {
        if (roleBuff.roleController.TemporaryId != (string)id)
        {
            return;
        }
        roleBuff.TriggerEffect();
    }
}
