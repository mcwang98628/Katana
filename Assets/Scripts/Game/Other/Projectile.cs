using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Projectile : DmgBuffOnTouch
{
    [Header("击中后消失")]
    public bool DestroyOnHit = false;

    [Header("子弹飞行")]
    public float FlySpeed = 1;
    public bool UseSpeedCurve=false;
    public AnimationCurve SpeedCurve;

    [Header("子弹大小变化")]
    public bool UseScaleCurve = false;
    public AnimationCurve ScaleCurve;
    private Vector3 iniScale;
    float iniTime;
    float iniRadius;

    [Header("子弹扰动轨迹")]
    public bool UseProjectileNoise;
    public AnimationCurve MoveCurve;
    private Vector3 iniFwd;

    [Header("是否能被盾牌挡住")]
    public bool ShieldAble;

    bool Shielded;
    // Start is called before the first frame update
    public virtual void Start()
    {
        Destroy(gameObject, duration);
        iniScale = transform.localScale;
        iniFwd = transform.forward;
        iniTime = Time.time;
        iniRadius = radius;

    }

    private void Awake()
    {
        BattleManager.Inst.AddBattleThrow(this);
    }

    protected virtual void OnDestroy()
    {
        BattleManager.Inst.RemoveBattleThrow(this);
    }


    [SerializeField][LabelText("碰撞销毁Layer")]
    private LayerMask HitDestroyLayer;
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if(UseSpeedCurve)
        {
             transform.position += transform.forward * FlySpeed*SpeedCurve.Evaluate((Time.time - iniTime)) * Time.fixedDeltaTime;
        }
        else
        {
            transform.position += transform.forward * FlySpeed * Time.fixedDeltaTime;
        }
        if (UseScaleCurve)
        {
            transform.localScale = iniScale * ScaleCurve.Evaluate((Time.time - iniTime));
            radius=iniRadius* ScaleCurve.Evaluate((Time.time - iniTime));
        }
        if(UseProjectileNoise)
            transform.forward= iniFwd+transform.right*MoveCurve.Evaluate((Time.time - iniTime));
        
        bool isHit =  Physics.Raycast(transform.position,transform.forward, FlySpeed * Time.fixedDeltaTime,HitDestroyLayer);
        if (isHit)
        {
            Destroy(gameObject);
        }

    }

    public void Back()
    {
        Shielded = true;
        //owner = BattleManager.Inst.CurrentPlayer;
        SelfDamage = true;
        DamageTarget = DmgTarget.Enemy;
        iniFwd = -iniFwd;
        transform.forward = -transform.forward;
        BattleManager.Inst.RemoveBattleThrow(this);
    }

    protected override void doDamageAndBuff(List<RoleController> targets)
    {
        for(int i=0;i<targets.Count;i++)
        {
            PlayerShieldHealth _shield = targets[i].GetComponent<PlayerShieldHealth>();
            if (!Shielded)
            {
                if (_shield != null)
                {
                    if (Damage != null)
                    {
                        if (_shield.JudgeProjectile(transform.position))
                        {
                            Back();
                        }
                    }
                }
            }
        }

        base.doDamageAndBuff(targets);
        if (DestroyOnHit&& targets.Count>0)
        {
            DestroyObj();
        }
    }
}
