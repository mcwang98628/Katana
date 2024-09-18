using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Injured_LifeCycle : RoleBuffLifeCycle
{
    private int times;
    private int timer;
    public Buff_Injured_LifeCycle(int times)
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
        EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnRoleInjured);
        EventManager.Inst.AddEvent(EventName.OnRoleGodInjured,OnRoleInjured);
    }

    public override void Destroy()
    {
        base.Destroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnRoleInjured);
        EventManager.Inst.RemoveEvent(EventName.OnRoleGodInjured,OnRoleInjured);
    }

    private void OnRoleInjured(string arg1, object id)
    {
        if (roleBuff.roleController.TemporaryId != ((RoleInjuredInfo)id).RoleId)
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
