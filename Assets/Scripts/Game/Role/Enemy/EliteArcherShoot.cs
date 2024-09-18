using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteArcherShoot : EnemyShootAttack
{
    public int ShootCount;
    public float Angle;
    public float SurroundShotRotateSpeed=360;
    public float SurroundShotInterval;
    public ParticleSystem PowerUpParticle;
    //public FeedBackObject BowDrawFeedback;
    public override void AttackFunc()
    {
        base.AttackFunc();
        if(currentAttackStatus== 1)
            aim = false;
        //if (BowDrawFeedback!=null)
        //{
        //    FeedbackManager.Inst.UseFeedBack(roleController,BowDrawFeedback);
        //}
    }

    protected override void AnimEvent(GameObject go, string eventName)
    {
        base.AnimEvent(go, eventName);
        if (eventName.Contains(AnimatorEventName.StartAttack_))
        {
            if (currentAttackStatus == 1 && PowerUpParticle) 
            {
                PowerUpParticle.Play();
            }

        }
    }
    public override void SpwanProjectile()
    {
        //直接射脸法
        if(currentAttackStatus==0)
        {
            SpwanProjectile(false);
            //FastStraightShoot();
        }

        //散射开花
        else if (currentAttackStatus == 1)
        {
            StartCoroutine(MutiShoot());
            //StartCoroutine(TracingShoot());
        }
        //旋转发射
        else
        {
            StartCoroutine(SurroundShot());
        }
    }

    //跟踪射击，必须远离怪物以躲避
    public IEnumerator TracingShoot()
    {
        for (int i = 0; i < ShootCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            roleController.FastMove(0.05f, 10, -SpwanPoint.transform.forward, null);
            roleController.Animator.transform.LerpLookAt(BattleManager.Inst.CurrentPlayer.transform,0.6f);
            DmgBuffOnTouch projectile = Instantiate(Projectile, SpwanPoint.position,SpwanPoint.transform.rotation);
            projectile.Init(roleController , AttackPower);
        }
    }
    IEnumerator MutiShoot()
    {
        int OffsetCount = (ShootCount - 1) / 2;
        for (int i = 0; i < ShootCount; i++)
        {
            yield return new WaitForSeconds(0.05f);
            roleController.FastMove(0.05f, 10, -SpwanPoint.transform.forward, null);
            DmgBuffOnTouch projectile = Instantiate(Projectile, SpwanPoint.position, Quaternion.Euler(SpwanPoint.eulerAngles + new Vector3(0, (-OffsetCount + i) * Angle, 0)));
            projectile.Init(roleController, AttackPower);
           
            (projectile as EliteArcherProjectile).CanFlyBack=true;
        }
    }

    public IEnumerator SurroundShot()
    {
        float StartRotation = transform.rotation.eulerAngles.y;
        float CurrentRotateAngles = 0;
        //计算上次射击时间
        float ShotTimeTamp = Time.time;
        while(CurrentRotateAngles<360)
        {
            if(Time.time-ShotTimeTamp>SurroundShotInterval)
            {
               
                SpwanProjectile(true);
                ShotTimeTamp = Time.time;
            }
            CurrentRotateAngles += Time.deltaTime * SurroundShotRotateSpeed;
            transform.rotation = Quaternion.Euler(0,StartRotation+CurrentRotateAngles,0);
            yield return null;
        }
        
    }

      public void SpwanProjectile(bool canflyBack)
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
        (projectile as EliteArcherProjectile).CanFlyBack=canflyBack;
    }

}
