using AnimatorTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteGoblinFighterAttack : EnemyAttack
{
    public MeleeWeaponTrail trail1;
    public MeleeWeaponTrail trail2;
    public DmgBuffOnTrigger dmg1;
    public DmgBuffOnTrigger dmg2;
    public AudioClip AxeAudio;
    public float AxeMoveSpeed;
    public float AxeMoveTime;
    public DmgBuffOnTrigger KickDmg;
    public float KickMoveSpeed;
    public float KickMoveTime;
    protected override void Start()
    {
        base.Start();
        dmg1.Init(roleController);
        dmg2.Init(roleController);
        KickDmg.Init(roleController);
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
            if (e == "1")
            {
                roleController.FastMove(KickMoveTime, KickMoveSpeed, roleController.Animator.transform.forward, null);
            }
        }
        if(eventName.Contains(AnimatorEventName.DmgStart_))
        {
            string e = eventName.Replace(AnimatorEventName.DmgStart_,"");
            if(e=="0")
            {
                AxeAudio.Play();
                dmg1.ActiveTrigger();
                dmg2.ActiveTrigger();
                trail1.Emit = true;
                trail2.Emit = true;
                roleController.FastMove(AxeMoveTime,AxeMoveSpeed,roleController.Animator.transform.forward,null);
            }
            if(e=="1")
            {
                KickDmg.ActiveTrigger();
            }
        }
        if(eventName.Contains(AnimatorEventName.DmgEnd_))
        {
            string e = eventName.Replace(AnimatorEventName.DmgEnd_, "");
            if(e=="0")
            {
                dmg1.CloseTrigger();
                dmg2.CloseTrigger();
                trail1.Emit = false;
                trail2.Emit = false;
            }
            if(e=="1")
            {
                KickDmg.CloseTrigger();
            }
        }
    }
}
