using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayTraceProjectile : Projectile
{
    [Header("追踪参数")]
    public float DelayTime=1;
    public float TraceRotSpeed=2;
    public AnimationCurve TraceInfluence;
    float timer;
    public override void Start()
    {
       base.Start();
       timer=Time.time;
    }

    private void Update() {
        Vector3 dir=BattleManager.Inst.CurrentPlayer.transform.position-transform.position;
        dir.y=0;
        transform.forward=Vector3.Lerp(transform.forward,dir.normalized,TraceRotSpeed*Time.deltaTime*(TraceInfluence.Evaluate(Time.time-timer)));
    }
}
