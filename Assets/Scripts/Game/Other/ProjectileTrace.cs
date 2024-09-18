using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrace : Projectile
{
    [Header("追踪相关")]
    public float RotSpeed;
    public float DelayTraceTime;
    public bool FadeDelay = false;
    float delayTraceTimer;
    public enum TargetType
    {
        Nearest,
        Random
    }
    public TargetType TargetChoose;
    protected RoleController target;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        delayTraceTimer = Time.time;
    }
    public void SetTarget(RoleController role)
    {
        target = role;
    }
    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();


        if (target == null)
        {
            if (TargetChoose == TargetType.Nearest)
                target = BattleTool.FindNearestEnemy(owner);
            else if (TargetChoose == TargetType.Random)
                target = BattleTool.GetRandomEnemy(owner);
        }
        else
        {
            Vector3 desDir = target.transform.position - transform.position;
            desDir.y = 0;
            
            float delayPara = (Time.time - delayTraceTimer) / DelayTraceTime;
            if (!FadeDelay)
                delayPara = delayPara > 1 ? 1 : 0;
            else
                delayPara = delayPara > 1 ? 1 : delayPara;

            float rotSpeed = RotSpeed * Time.fixedDeltaTime*delayPara;

            transform.forward = Vector3.Lerp(transform.forward, desDir.normalized, rotSpeed).normalized;
        }
    }

}
