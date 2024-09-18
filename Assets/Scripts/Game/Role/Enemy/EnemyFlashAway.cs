using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlashAway : EnemyDodge
{
    public Vector3 dir; 
    public PhantomReleaser FlashPhantom;
    public FeedBackObject FlashFeedbacks;
    public GameObject FlashProjectile;
    public float FlashProInterval;
    
    public override void Dodge()
    {
        base.Dodge();
        StopAllCoroutines();
        StartCoroutine(dodgeIE());
    }
    public IEnumerator dodgeIE()
    {
        //EnemyWallTester.GetNoWallDir(transform,);




        roleController.InputMove(Vector2.zero);
        roleController.Animator.SetTrigger("Roll");
        roleController.Animator.transform.forward = dir;
        if(FlashPhantom!=null)
        FlashPhantom.StartEmmitPhantom();
        if(FlashFeedbacks!=null)
        FeedbackManager.Inst.UseFeedBack(roleController,FlashFeedbacks);
        //可能有些躲避带动画
        if (DashForward)
        {
            roleController.FastMove(DodgeTime, DodgeSpeed, dir.normalized, null);
        }
        else
        {
            roleController.FastMove(DodgeTime, DodgeSpeed, -dir.normalized, null);
        }

        if (Effect)
        {
            Effect.transform.forward = dir;
            Effect.transform.position = transform.position + dir.normalized + Vector3.up;
            Effect.Play();
        }
        //释放冲浪效果的粒子
        //float StartTime = Time.time;
        //float FlashProTimeTamp = Time.time;
        //while(Time.time<StartTime+DodgeTime)
        //{
        //    if(Time.time-FlashProTimeTamp>FlashProInterval)
        //    {
        //        //TODO:待优化
        //        FlashProTimeTamp = Time.time;
        //        //Emmit
        //        GameObject CurrentFlashPro = FlashProjectile.Duplicate();
        //        CurrentFlashPro.transform.position = GetComponent<EliteMasterAttack>().SpwanPoint.position;
        //        CurrentFlashPro.transform.forward = -roleController.Animator.transform.right;
        //        //CurrentFlashPro.GetComponent<EliteMasterProjectile>().Init(roleController, GetComponent<EliteMasterAttack>().AttackPower);
        //        CurrentFlashPro.GetComponent<EliteMasterProjectile>().BehaviorWay = 0;
        //        CurrentFlashPro.GetComponent<EliteMasterProjectile>().StartLerpTime = 0;
        //        CurrentFlashPro.GetComponent<EliteMasterProjectile>().StartPos = CurrentFlashPro.transform.position;

        //        GameObject CurrentFlashPro2 = CurrentFlashPro.Duplicate();
        //        CurrentFlashPro2.transform.forward = roleController.Animator.transform.right;
        //        //CurrentFlashPro2.GetComponent<EliteMasterProjectile>().Init(roleController, GetComponent<EliteMasterAttack>().AttackPower);
        //    }
        yield return null;
        //}
        if (FlashPhantom != null)
            FlashPhantom.EndEmmitPhantom();
    }

}
