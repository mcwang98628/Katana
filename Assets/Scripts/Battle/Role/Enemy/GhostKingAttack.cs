using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XftWeapon;

public class GhostKingAttack : EnemyAttack
{
    public AudioClip SlashSFX;
    public AudioClip MagicSFX;
    //public MeleeWeaponTrail trail;
    public XWeaponTrail trail;
    public DmgBuffOnTrigger dmg;
    public GameObject Projectile;
    public GameObject ProjectileSpawnPoint;
    public float SlashSpeed;
    public float SlashTime;
    public float JumpSpeed;
    //public GameObject DmgProjectile2;
    //public GameObject SpawnPoint2;

    //public DmgBuffOnTrigger LaserDmg;
    public AudioClip LaserSfx;
    public ParticleSystem LaserStartParticle;
    public GameObject LaserObj;
    public ParticleSystem LaserChargeParticle;
    //public float PrepareEmissionSpeed;
    //public float PrepareEmissionTime;
    //public float EmissionSpeed;
    //public float EmissionTime;
    //Vector3 JumpDir;
    protected override void Start()
    {
        base.Start();
        dmg.Init(roleController);
        //LaserDmg.Init(roleController);
        LaserObj.GetComponent<DmgBuffOnTrigger>().Init(roleController,60);
        LaserObj.SetActive(false);
        trail.Deactivate();
        //dmg.WZYInit();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void AnimEvent(GameObject go, string eventName)
    {
        base.AnimEvent(go, eventName);
        if(go!=roleController.Animator.gameObject)
        {
            return;
        }
        if(eventName.Contains(AnimatorEventName.StartAttack_))
        {
            string e = eventName.Replace(AnimatorEventName.StartAttack_, "");
            //if (e == "0" || e == "1")
            //{
            //    LookAtTarget = true;
            //}
            if (e == "0" )
            {
                
                //roleController.FastMove(PrepareEmissionTime, PrepareEmissionSpeed, roleController.Animator.transform.forward, null);
            }
            if (e=="2")
            {
                //JumpDir = EnemyWallTester.GetNoWallDir(roleController.Animator.transform, 3);
                //roleController.Animator.transform.forward = -JumpDir;
            }
            if(e=="3")
            {
                LaserChargeParticle.Play();
            }
        }
        if(eventName.Contains(AnimatorEventName.DmgStart_))
        {
            string e = eventName.Replace(AnimatorEventName.DmgStart_,"");
            if (e == "0" || e == "1")
            {
                SlashSFX.Play();
                dmg.ActiveTrigger();
                roleController.FastMove(SlashTime,SlashSpeed,roleController.Animator.transform.forward,null);
                //trail.Activate();
            }
            if(e=="1")
            {

                //GameObject CurrentPro = Instantiate(DmgProjectile2,SpawnPoint2.transform.position,roleController.Animator.transform.rotation);
                //CurrentPro.GetComponent<DmgBuffOnTouch>().Init(roleController);
            }
            if(e=="3")
            {
                LaserSfx.Play();
                LaserStartParticle.Play();
                LaserObj.SetActive(true);
                LaserObj.GetComponent<DmgBuffOnTrigger>().ActiveTrigger();
            }

            //if(e=="0")
            //{ 

            //}
            else if (e == "2")
            {
                MagicSFX.Play();
                GameObject CurrentPro = Instantiate(Projectile, ProjectileSpawnPoint.transform.position,roleController.Animator.transform.rotation);
                CurrentPro.GetComponent<DmgBuffOnTouch>().Init(roleController);
                //roleController.Animator.transform.forward = -JumpDir;
                //roleController.FastMove(999,JumpSpeed,JumpDir,null);
            }
        }
        if(eventName.Contains(AnimatorEventName.DmgEnd_))
        {
            string e = eventName.Replace(AnimatorEventName.DmgEnd_, "");
            if (e == "0" || e == "1")
            {
                dmg.CloseTrigger();
                //trail.StopSmoothly(0.3f);
            }
            else if (e == "2")
            {
                roleController.StopFastMove();
            }
            else if(e=="3")
            {
                LaserStartParticle.Stop();
                LaserObj.GetComponent<DmgBuffOnTrigger>().CloseTrigger();
                LaserObj.SetActive(false);
            }
        }
        if(eventName.Contains("StartTrail"))
        {
            trail.Activate();
        }
        if(eventName.Contains("StopTrail"))
        {
            trail.Deactivate();
        }
    }
    public void OnDeath()
    {
        dmg.gameObject.SetActive(false);
    }
}
