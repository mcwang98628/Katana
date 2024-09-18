using AnimatorTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBossAttack : EnemyAttack
{
    public DmgBuffOnTouch SmashDmg;
    public GameObject SmashParticle;
    public Transform SmashParticlePos;
    public DmgBuffOnTrigger KickDmg;
    public AudioClip KickSFX;
    public AudioClip DashSfx;
    public ParticleSystem DashParticle;
    public DmgBuffOnTrigger DashDmg;
    public float DashMoveTime;
    public float DashMoveSpeed;
    public AudioClip SmashSFX;
    public float SmashMoveTime;
    public float SmashMoveSpeed;
    public float FlyKickMoveTime;
    public float FlyKickMoveSpeed;

    private void Start()
    {
        DashDmg.Init(roleController);
        KickDmg.Init(roleController);
        SmashDmg.Init(roleController);
        SmashDmg.WZYInit();
    }
    protected override void AnimEvent(GameObject go, string eventName)
    {
        base.AnimEvent(go, eventName);
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        if(eventName.Contains(AnimatorEventName.StartAttack_))
        {
            string e = eventName.Replace(AnimatorEventName.StartAttack_,"");
            if(e=="2")
            {
                
            }
        }
        if(eventName.Contains("StartFlyKick"))
        {
            roleController.FastMove(FlyKickMoveTime, FlyKickMoveSpeed, roleController.Animator.transform.forward, null);

        }
        if (eventName.Contains(AnimatorEventName.DmgStart_))
        {
            string e = eventName.Replace(AnimatorEventName.DmgStart_,"");
            if(e=="2")
            {
                DashDmg.ActiveTrigger();
                DashSfx.Play();
                DashParticle.Play();
                roleController.FastMove(DashMoveTime,DashMoveSpeed,roleController.Animator.transform.forward,null);
            }
            if(e=="1")
            {
                KickSFX.Play();
                KickDmg.ActiveTrigger();
            }
            if(e=="0")
            {
                SmashParticle.Duplicate().transform.position = SmashParticlePos.position;

                SmashDmg.gameObject.SetActive(true);
                SmashSFX.Play();
                roleController.FastMove(SmashMoveTime,SmashMoveSpeed,roleController.Animator.transform.forward,null);
            }
        }
        if(eventName.Contains(AnimatorEventName.DmgEnd_))
        {
            string e = eventName.Replace(AnimatorEventName.DmgEnd_,"");
            if(e=="2")
            {
                DashDmg.CloseTrigger();
            }
            if(e=="1")
            {
                KickDmg.CloseTrigger();
            }
            if(e=="0")
            {
                SmashDmg.gameObject.SetActive(false);
            }

        }
    }
}
