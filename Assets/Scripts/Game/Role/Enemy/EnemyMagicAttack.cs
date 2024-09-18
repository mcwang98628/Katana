using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMagicAttack : EnemyAttack
{

    public ParticleSystem[] EffectOnStart;
    public DmgBuffOnTouch Projectile;
    public Transform SpwanPoint;
    public AudioClip StartChargeAudio;
    protected bool isLookAt=false;
    public override void AttackFunc()
    {
        //Debug.Log("Attack");
        if (!IsAcceptInput)
        {
            return;
        }
        
        roleController.InputMove(Vector2.zero);
        roleController.SetIsAttacking(true);

        
        roleController.Animator.SetInteger(AttackStatus, currentAttackStatus);
        roleController.Animator.SetTrigger(Attack);
    }

    protected void ShowChargeVFX()
    {
        if (EffectOnStart.Length>0 && !roleController.IsDie)
        {
            for (int i = 0; i < EffectOnStart.Length; i++)
            {
                EffectOnStart[i].Play();
                EffectOnStart[i].transform.localScale=Vector3.one;
                //EffectOnStart[i].transform.DOScale(Vector3.one,0.8f);
            }
        }
    }



    protected override void Update()
    {
        base.Update();
        // Debug.Log(isLookAt);
        if (roleController.IsDie)
        {
            return;
        }
        if (isLookAt)
        {
            Transform target = UpdateTarget();
            if (target != null)
            {
                roleController.Animator.transform.forward = Vector3.Lerp(roleController.Animator.transform.forward, (target.position - transform.position).normalized, .1f);
            }
        }
    }



    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);

        if (eventName.Contains("FinishCharge"))
        {
            HideChargeVFX();
        }
        else if (eventName.Contains("StartCharge"))
        {
            if(StartChargeAudio!=null)
            {
                AudioManager.Inst.PlaySource(StartChargeAudio,0.5f);
            }

            ShowChargeVFX();
        }        
        else if (eventName.Contains(AnimatorEventName.DmgStart_))
        { 
            SpwanProjectile();
        }
        // else if (eventName.Contains(AnimatorEventName.EndInput))
        // {
        //     isLookAt = false;
        // }
        if (eventName.Contains("StopLookAt"))
        {
            isLookAt = false;
        }
        if (eventName.Contains("StartLookAt"))
        {
            isLookAt = true;
        }

    }
    public void HideChargeVFX()
    {
        if (EffectOnStart.Length > 0)
        {
            for (int i = 0; i < EffectOnStart.Length; i++)
            {
                if (EffectOnStart[i])
                    EffectOnStart[i].Stop();
            }
        }
    }
    public virtual void SpwanProjectile()
    {
        if (Projectile == null)
        {
            return;
        }
        HideChargeVFX();
        DmgBuffOnTouch projectile = Instantiate(Projectile, SpwanPoint.position, SpwanPoint.rotation);
        projectile.Init(roleController,AttackPower);


    }
}
