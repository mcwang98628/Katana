using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinArcherAttack : EnemyShootAttack
{
    public MeleeWeaponTrail SpearTrail;
    public DmgBuffOnTrigger SpearDmgBox;
    public int SpearCount;
    public AudioClip SlashSFX;
    public AudioClip ThrowSFX;
    protected override void Start()
    {
        base.Start();
        SpearDmgBox.Init(roleController);
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
            if (currentAttackStatus == 0)
            {
                if (BowDrawFeedback != null)
                {
                    FeedbackManager.Inst.UseFeedBack(roleController, BowDrawFeedback);
                }
            }

        }
        if (eventName.Contains(AnimatorEventName.DmgStart_))
        {
            string e = eventName.Replace(AnimatorEventName.DmgStart_,"");
            //投掷
            if (e=="0")
            {
                SpwanProjectile();
            }
            else if(e=="1")
            {
                SlashSFX.Play();
                SpearTrail.Emit = true;
                SpearDmgBox.ActiveTrigger();
            }
        }
        if(eventName.Contains(AnimatorEventName.DmgEnd_))
        {
            string e = eventName.Replace(AnimatorEventName.DmgEnd_,"");
            if(e=="1")
            {
                SpearTrail.Emit = false;
                SpearDmgBox.CloseTrigger();
            }
        }
    }
    public override void SpwanProjectile()
    {
        if (Projectile == null||currentAttackStatus!=0)
        {
            return;
        }
        ThrowSFX.Play();
        roleController.FastMove(0.2f,12, -SpwanPoint.transform.forward, null);
        for (int i = 0; i < SpearCount; i++)
        {
            DmgBuffOnTouch projectile = Instantiate(Projectile, SpwanPoint.position, Quaternion.Euler(SpwanPoint.eulerAngles + new Vector3(0,15*(i-(SpearCount+1)/2), 0)));
            projectile.Init(roleController, AttackPower);

            if (AimOnSpwan)
            {
                Vector3 fwd = UpdateTarget().position - projectile.transform.position;
                fwd.y = 0;
                projectile.transform.forward = fwd;
            }
        }
    }
}
