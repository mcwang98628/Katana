using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Attack_LifeCycle : RoleBuffLifeCycle
{
    private int times;
    private int timer;
    public Buff_Attack_LifeCycle(int times)
    {
        this.times = times;
        timer = 0;
    }

    public override void ReSet(RoleBuffLifeCycle buffLifeCycle)
    {
        timer = 0;
    }

    public override void Append()
    {
        times += times;
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

        timer++;
        if (timer>=times)
        {
            timer = 0;
            roleBuff.Destroy();
        }
    }
}
