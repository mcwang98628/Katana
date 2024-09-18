using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_EnterRoom_LifeCycle : RoleBuffLifeCycle
{
    private int times;
    private int timer;
    public Buff_EnterRoom_LifeCycle(int times)
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
        EventManager.Inst.AddEvent(EventName.EnterNextRoom,OnEnterRoom);
    }

    public override void Destroy()
    {
        base.Destroy();
        EventManager.Inst.RemoveEvent(EventName.EnterNextRoom, OnEnterRoom);
    }

    private void OnEnterRoom(string arg1, object value)
    {
        timer++;
        if (timer>=times)
        {
            timer = 0;
            roleBuff.Destroy();
        }
    }
}
