using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowAttack : EnemyAttack
{

    public GameObject Projectile;
    public GameObject Explosion;
    public Transform SpwanPoint;

    public float ThrowMoveTime;
    public float ThrowHeight;
    public float RandomOffst;
    public float BombRange = 1.6f;
    

    public override void AttackFunc()
    {
        if (!IsAcceptInput)
        {
            return;
        }

        //新加：
        //攻击时自动停下（从行为树里面摘过来的）
        roleController.InputMove(Vector2.zero);
        roleController.SetIsAttacking(true);

        roleController.Animator.SetInteger(AttackStatus, currentAttackStatus);
        roleController.Animator.SetTrigger(Attack);
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
        // if (eventName.Contains(AnimatorEventName.EnemyShowAttackWarning_))
        // {
        //     eventName = eventName.Replace(AnimatorEventName.EnemyShowAttackWarning_, "");
        //     AttackInfo ai = DataManager.Inst.GetAttackInfo(eventName);
        //     //ShowAttackerDebug(ai.AttackType, ai.AttackRadius, ai.AttackAngle);
        // }
        // else 
        if (eventName.Contains(AnimatorEventName.DmgStart_))
        {
            //Debug.Log("StartSpawn");
            //HideAttackerDebug();
            SpwanProjectile();
        }
    }

    protected virtual void SpwanProjectile()
    {
        Transform Target = UpdateTarget();
        if (!Target)
            return;

        Vector3 desPos = Target.position + new Vector3(RandomOffst * (Random.value - 0.5f), 0, RandomOffst * (Random.value - 0.5f));
        GameObject projectile = Instantiate(Projectile, SpwanPoint.position, Quaternion.identity);
        projectile.GetComponent<ThrowProjectile>().Init(desPos, ThrowMoveTime,ThrowHeight,Explosion, BombRange, AttackPower, roleController);
        // IndicatorManager.Inst.ShowAttackIndicator().Show(roleController,desPos,360,BombRange,ThrowMoveTime+0.1f, Color.red);
        // IndicatorManager.Inst.ShowAttackIndicator().SetEnable(true).SetAngle(360).SetSize(BombRange).SetTime(ThrowMoveTime+ 0.1f).SetPosition(desPos);
    }
    
   
    
}
