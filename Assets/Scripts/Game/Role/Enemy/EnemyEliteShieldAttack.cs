using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XftWeapon;

public class EnemyEliteShieldAttack : EnemyAttack
{
    //public MeleeWeaponTrail Combotrail;
    //public MeleeWeaponTrail HeavyAttackTrail;
    public XWeaponTrail trail;
    //三连击
    public GameObject RotateAttPreParticles;
    public GameObject RotateAttParticles;

    //一下的重攻击
    public GameObject HeavyAttackParticles;

    //蓄力的粒子
    //public GameObject ChargeParticlesPosition;
    //public GameObject HeavyAttackSword;
    protected override void Start()
    {
        base.Start();
        trail.Deactivate();
    }
    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);
        if (eventName.Contains(AnimatorEventName.StartAttack_))
        {

            int index = int.Parse(eventName.Replace(AnimatorEventName.StartAttack_, ""));
            if (index == 0 || index == 1)
            {
                //GameObject CurrentParticles=HeavyAttackParticles.Duplicate();
                //CurrentParticles.SetActive(true);
                //CurrentParticles.AddComponent<SelfDestruct>();
                trail.Activate();
            }
            //连续打击
            //if (index == 1)
            //{
            //    Debug.Log("TryEmmitParticles");
            //    GameObject CurrentParticles = RotateAttPreParticles.Duplicate();
            //    CurrentParticles.SetActive(true);
            //    CurrentParticles.AddComponent<SelfDestruct>();
            //}
        } 
        if(eventName.Contains(AnimatorEventName.DmgStart_))
        {
            //int index = int.Parse(eventName.Replace(AnimatorEventName.DmgStart_, ""));
            //Debug.Log("CurrentAttackStatus:"+currentAttackStatus);
            if (currentAttackStatus==1)
            {
                GameObject CurrentParticles = RotateAttParticles.DuplicateRoleParticles();
                //Debug.Log(CurrentParticles);
            }
        }
        else if (eventName.Contains("EndTrail"))
        {
            ////一下的重度打击
            //if (currentAttackStatus == 0)
            //{
            //    //HeavyAttackSword.SetActive(false);
            //    HeavyAttackTrail.Emit = false;
            //}
            ////连续打击
            //if (currentAttackStatus == 1)
            //{
            //    Combotrail.Emit = false;
            //}
            trail.StopSmoothly(0.5f);
        }
      
       
    } 
}
