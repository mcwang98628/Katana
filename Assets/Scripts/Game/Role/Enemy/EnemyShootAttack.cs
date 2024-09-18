using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootAttack : EnemyAttack
{

    public DmgBuffOnTouch Projectile;
    public Transform SpwanPoint;
    public bool AimOnSpwan;
    public bool AimBeforeShoot = false;
    protected bool aim=false;
    public float RandomAngleOffst;
    public FeedBackObject BowDrawFeedback;
    float RotSpeed = -1;
    public override void AttackFunc()
    {
        if (!IsAcceptInput)
        {
            return;
        }
        //if (BowDrawFeedback != null)
        //{
        //    FeedbackManager.Inst.UseFeedBack(roleController, BowDrawFeedback);
        //}
        if (RotSpeed < 0)
        {
            RotSpeed = (roleController as EnemyController).GetRotateSpeed();
        }

        roleController.InputMove(Vector2.zero);
        roleController.SetIsAttacking(true);

        roleController.Animator.SetInteger(AttackStatus, currentAttackStatus);
        roleController.Animator.SetTrigger(Attack);

        if(AimBeforeShoot)
        {
            aim=true;
        }
    }
    protected override void Update()
    {
        if (BattleManager.Inst.CurrentPlayer == null)
            return;
        if (!roleController.IsCanMove)
            return;
        if (aim && !roleController.IsFreeze)
        {
            Vector3 dir =UpdateTarget().position - gameObject.transform.position;
            roleController.Animator.transform.forward = Vector3.Slerp(roleController.Animator.transform.forward, dir, RotSpeed * Time.deltaTime);
        }
    }

    //protected override void DamageCalculation(AttackInfo info)
    //{
    //    Vector3 v3;
    //    switch (info.AttackType)
    //    {
    //        case AttackType.Round:
    //            v3 = BattleManager.Inst.CurrentPlayer.transform.position - roleController.transform.position;
    //            if (v3.magnitude <= info.AttackRadius)
    //            {
    //                TargetDmg(BattleManager.Inst.CurrentPlayer, info);
    //            }
    //            break;
    //        case AttackType.Sector:
    //            v3 = BattleManager.Inst.CurrentPlayer.transform.position - roleController.transform.position;
    //            if (v3.magnitude <= info.AttackRadius)
    //            {
    //                float angle = Vector3.Angle(roleController.Animator.transform.forward, v3);
    //                if (angle <= info.AttackAngle * 0.5f)
    //                {
    //                    TargetDmg(BattleManager.Inst.CurrentPlayer, info);
    //                }
    //            }
    //            break;
    //    }
    //}



    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);
        if(eventName.Contains(AnimatorEventName.StartAttack_))
        {
            if (BowDrawFeedback != null)
            {
                FeedbackManager.Inst.UseFeedBack(roleController, BowDrawFeedback);
            }
        }
        if (eventName.Contains(AnimatorEventName.EndAttack_))
        {
            eventName = eventName.Replace(AnimatorEventName.EndAttack_, "");
            int index = int.Parse(eventName);
            if (index == currentAttackIndex)
            {
                roleController.SetIsAttacking(false);
            }
        } 
        if (eventName.Contains(AnimatorEventName.DmgStart_))
        {
            SpwanProjectile();
        }
    }

    public virtual void SpwanProjectile()
    {
        if (Projectile == null)
        {
            return;
        }
        roleController.FastMove(0.12f, 4, -SpwanPoint.transform.forward, null);
        DmgBuffOnTouch projectile = Instantiate(Projectile, SpwanPoint.position, Quaternion.Euler(SpwanPoint.eulerAngles + new Vector3(0, RandomAngleOffst * (Random.value - .5f), 0)));
        projectile.Init(roleController,AttackPower);

        if (AimOnSpwan)
        {
            Vector3 fwd = UpdateTarget().position - projectile.transform.position;
            fwd.y = 0;
            projectile.transform.forward = fwd;
        }
    }


}
