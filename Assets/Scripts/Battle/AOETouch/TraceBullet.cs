using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TraceBullet : Bullet
{
    [BoxGroup("追踪",true,true)]
    [SerializeField]
    private float rotSpeed;
    [BoxGroup("追踪",true,true)]
    [SerializeField]
    private float delayTraceTime;
    [BoxGroup("追踪",true,true)]
    [SerializeField]
    private bool fadeDelay = false;
    [BoxGroup("追踪",true,true)]
    [SerializeField]
    private float delayTraceTimer;
    [BoxGroup("追踪",true,true)]
    [SerializeField]
    private TargetType targetChoose;
    
    public enum TargetType
    {
        Nearest,
        Random
    }
    
    private RoleController traceTarget;

    public void Init(RoleController role,RoleController target)
    {
        base.Init(role);
        traceTarget = target;
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();


        if (traceTarget == null)
        {
            if (targetChoose == TargetType.Nearest)
                traceTarget = BattleTool.FindNearestEnemy(Owner);
            else if (targetChoose == TargetType.Random)
                traceTarget = BattleTool.GetRandomEnemy(Owner);
        }
        else
        {
            Vector3 desDir = traceTarget.transform.position - transform.position;
            desDir.y = 0;
            
            float delayPara = (Time.time - delayTraceTimer) / delayTraceTime;
            if (!fadeDelay)
                delayPara = delayPara > 1 ? 1 : 0;
            else
                delayPara = delayPara > 1 ? 1 : delayPara;

            float rotSpeed = this.rotSpeed * Time.fixedDeltaTime*delayPara;

            transform.forward = Vector3.Lerp(transform.forward, desDir.normalized, rotSpeed).normalized;
        }
    }
    
}
