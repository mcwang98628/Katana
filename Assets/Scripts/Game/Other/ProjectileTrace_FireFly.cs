using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class ProjectileTrace_FireFly : ProjectileTrace,IBattleObject
{

    public BattleObjectType BattleObjectType { get; } = BattleObjectType.FireFly;
    public Guid TemporaryId { get; } = Guid.NewGuid();
    public Transform ObjectTransform { get; private set; }
    
    public int MaxCount = 10;
    public float LifeTime = 3f;
    public override void Start()
    {
        base.Start();
        ObjectTransform = transform;
        BattleManager.Inst.BattleObjectRegistered(this);
        transform.forward = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)).normalized;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        BattleManager.Inst.BattleObjectUnRegistered(this);
    }

    public override void DestroyObj()
    {
        Destroy(gameObject);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (target == null)
        {
            target = owner;
            Destroy(gameObject,LifeTime);
        }
        if (target.IsDie)
        {
            Destroy(gameObject);
        }
    }

}
