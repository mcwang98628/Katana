using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : RoleHealth
{
    public override int DefenseLevel => base.DefenseLevel;

    public enum RotationAxis {X,Y,Z}
    protected EnemyDropOnDeath enemyDropOnDeath;
    protected EnemyBehavior enemyBehavior;


    public ParticleSystem DeathParticles;
    //public GameObject DynamicHitReactionJoint;
    bool IsReactingHit;
    public float Onhit_SpineRotationAngle = 70f;
    public float Onhit_HeadRotationAngle=20f;
    float SpineOnHit_RotationAngle = 50f;
    float HeadOnHit_RotationAngle = 20f;
    //1或者-1，受伤后骨骼旋转方向。
    public int OnHitRotationDirection=1;
    public RotationAxis OnHitRotationAxis = RotationAxis.Z;
    [HideInInspector]
    public int KillerID=-1;
    public override void Init(float _maxHp)
    {
        maxHp = _maxHp;
        currentHp =maxHp;
        enemyDropOnDeath = GetComponent<EnemyDropOnDeath>();
        enemyBehavior = GetComponent<EnemyBehavior>();

    }
    public void InterruptRepel(float repelTime, float moveSpeed, Vector3 moveDir)
    {
        roleController.Animator.SetTrigger(Hit);
        enemyBehavior.ResetBehaviorTree();
        roleController.FastMove(repelTime, moveSpeed, moveDir, null);
    }
    public override void Injured(DamageInfo dmg)
    {
        void InterruptionFunction()
        {
            enemyBehavior.ResetBehaviorTree();
            if (roleController.IsAttacking)
            {
                roleController.StopAttack();
            }
            else
            {
                roleController.StopAttack();
            }
        }
        
        if(dmg.Interruption)
        {
            ResetOnHitAnimationPara();
        }

        if (IsAcceptInterruption)
        {
            if (dmg.Interruption||IsCanHitInterrupt) {
                InterruptionFunction();
            }
        }
        
        base.Injured(dmg);
    }
    //开始播放动画
    public void ResetOnHitAnimationPara()
    {
        //开始播放一个小动画
        IsReactingHit = true;
        SpineOnHit_RotationAngle = Onhit_SpineRotationAngle;
        HeadOnHit_RotationAngle = Onhit_HeadRotationAngle;

    }
    private void LateUpdate()
    {
        //动画的修改不放在LateUpdate里面无法运行。这导致我不知道怎么使用携程来完成这个功能。
        PlayOnHitAnimation();
    }
    public void PlayOnHitAnimation()
    {
        if (!roleController.IsDie)
        {
            if (IsReactingHit)
            {
                if (roleController.roleNode.Body != null && roleController.roleNode.Head != null)
                {
                    //Y不太可能
                    if (OnHitRotationAxis == RotationAxis.Z)
                    {
                        roleController.roleNode.Body.transform.Rotate(0, 0, OnHitRotationDirection * -SpineOnHit_RotationAngle);
                        roleController.roleNode.Head.transform.Rotate(0, 0, OnHitRotationDirection * -HeadOnHit_RotationAngle);
                    }
                    if (OnHitRotationAxis == RotationAxis.X)
                    {
                        roleController.roleNode.Body.transform.Rotate(OnHitRotationDirection * -SpineOnHit_RotationAngle, 0, 0);
                        roleController.roleNode.Head.transform.Rotate(OnHitRotationDirection * -HeadOnHit_RotationAngle, 0, 0);
                    }
                    SpineOnHit_RotationAngle -= Time.deltaTime * 100f;
                    HeadOnHit_RotationAngle -= Time.deltaTime * 40f;
                }
                if (SpineOnHit_RotationAngle <= 0 || HeadOnHit_RotationAngle <= 0)
                {
                    IsReactingHit = false;
                }
            }

        }    
    }
    public override void OnDeath(int AttackerID,RoleController AttackerRole)
    {
        if (enemyDropOnDeath != null)
            enemyDropOnDeath.OnDeath();

        if (DeathParticles != null)
        {
            DeathParticles.Play();
        }
        base.OnDeath(AttackerID, AttackerRole);
    }
}
