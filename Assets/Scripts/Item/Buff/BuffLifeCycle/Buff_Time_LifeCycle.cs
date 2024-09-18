using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Time_LifeCycle : RoleBuffLifeCycle
{
    public float time;
    public float oTime;//原Time
    private float timer;
    public Buff_Time_LifeCycle(float time)
    {
        this.time = time;
        oTime = time;
        timer = 0;
    }
    public override void ReSet(RoleBuffLifeCycle buffLifeCycle)
    {
        if (buffLifeCycle is Buff_Time_LifeCycle timeLife)
        {
            this.time = timeLife.time;
            oTime = time;
            timer = 0;
        }
    }
    public override void Append()
    {
        time += oTime;
    }

    public override void Awake()
    {
        base.Awake();
        if (roleBuff.roleController!=null && 
            (roleBuff.roleController.roleTeamType == RoleTeamType.EliteEnemy || roleBuff.roleController.roleTeamType == RoleTeamType.Enemy_Boss))
        {
            time *= 0.5f;
        }
    }

    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if (timer>=time)
        {
            timer -= time;
            roleBuff.Destroy();
        }
    }
}
