using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//一只可以追踪的箭
public class EliteArcherProjectile : Projectile
{
    public bool CanFlyBack;
    //一阶段飞行时间
    public float FlyTime;
    //二阶段旋转时间
    public float StopForce = 0.2f;
    public float RotateForce = 0.2f;
    public float RotateTime;
    public float RotateFinishWaitTime = 0.3f;
    public float FlySpeed2;



    public ShowArrowLine _showline;
    

    //旋转中的第一阶段
    //public float RotateTime1;
    public override void Start()
    {
        base.Start();
        if(CanFlyBack)
            StartCoroutine(EliteArcherIE());
        //_showline.ShowLine();
    }
    public IEnumerator EliteArcherIE()
    {
        yield return new WaitForSeconds(FlyTime);
        float StartTime = Time.time;
        while(Time.time<StartTime+RotateTime)
        {
            FlySpeed = Mathf.Lerp(FlySpeed,0,StopForce);
            //FlySpeed = 0;
            Vector3 TargetPos = BattleManager.Inst.CurrentPlayer.transform.position;
            TargetPos.y = transform.position.y;
            Quaternion TargetQua= Quaternion.LookRotation(TargetPos-transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation,TargetQua,RotateForce);
            //transform.LookAt(TargetPos);
            yield return null;
        }
        //_showline.ShowLine();
        yield return new WaitForSeconds(RotateFinishWaitTime);
        FlySpeed = FlySpeed2;
        //EndFly
        //LerpRotateToPlayer
    }
   

}
