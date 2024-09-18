using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Buff_Time_Trigger : BuffTrigger
{
    private float intervalTime;//间隔时间

    //计时器
    private float durationTimer;
    private float intervalTimeTimer;
    /// <summary>
    /// 时间Trigger
    /// </summary>
    /// <param name="intervalTime">触发间隔</param>
    public Buff_Time_Trigger(float intervalTime)
    {
        this.intervalTime = intervalTime;
    }

    public override void Awake()
    {
        base.Awake();
        durationTimer = 0;
        intervalTimeTimer = 0;
        isTrigger = false;
    }

    private bool isTrigger = false;//是否触发过
    public override void Update()
    {
        base.Update();

        if (intervalTime == -1)
        {
            if (!isTrigger)
            {
                isTrigger = true;
                roleBuff.TriggerEffect();
            }
        }
        else
        {
            durationTimer += Time.deltaTime;
            intervalTimeTimer += Time.deltaTime;
            //触发
            if (intervalTimeTimer >= intervalTime)
            {
                intervalTimeTimer -= intervalTime;
                roleBuff.TriggerEffect();
            }
        }
    }
}
