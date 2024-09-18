using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyShield : MonoBehaviour
{
    public float MaxSheild = 100;
    float currentSheild = 0;
    public Vector3 Direction = Vector3.forward;
    public int DefenseAngle = 180;
    public bool UseSheild = true;
    public ParticleSystem SheildEffect;
    public ParticleSystem SheildBreakEffect;
    public GameObject SheildModel;
    public FeedBackObject HitShieldFeedback;
    Vector3 iniScale;
    Tweener scaleTweener;
    protected RoleController roleController;
    private void Start()
    {
        currentSheild = MaxSheild;
        iniScale = SheildModel.transform.localScale;

    }

    protected virtual void Awake()
    {
        roleController = GetComponent<RoleController>();
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
    }

    protected virtual void OnDestroy()
    {
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
    }

    public bool SheildTakeDamage(RoleController roleController, DamageInfo damage)
    {
        if (damage.DmgType != DmgType.Physical)
        {
            return false;
        }
        if (!UseSheild)
        {
            return false;
        }

        if (!SheildModel)
        {
            return false;
        }
        Vector3 shieldDir = roleController.Animator.transform.forward;
        shieldDir.y = 0;
        if (Vector3.Angle(shieldDir, damage.AttackPoint - transform.position) > DefenseAngle / 2)
        {
            return false;
        }
        else
        {
            if (HitShieldFeedback != null)
            {
                FeedbackManager.Inst.UseFeedBack(roleController, HitShieldFeedback);
            }
            if (scaleTweener != null)
            {
                scaleTweener.Kill();
                SheildModel.transform.localScale = iniScale;
            }
            scaleTweener = SheildModel.transform.DOPunchScale(iniScale * 1.1f, 0.2f);

            currentSheild -= damage.DmgValue;
            if (currentSheild > 0)
            {
                if (SheildEffect)
                    SheildEffect.Play();
                return true;
            }
            else
            {
                if (SheildBreakEffect != null)
                {
                    SheildBreakEffect.Play();
                }
                UseSheild = false;
                Destroy(SheildModel);
                return false;
            }
        }

    }

    protected virtual void AnimEvent(GameObject go, string eventName)
    {
        if (roleController == null)
            roleController = GetComponent<RoleController>();
        
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        if (eventName.Contains("ShieldOff"))
        { 
            UseSheild=false;
        }
        else if (eventName.Contains("ShieldOn"))
        {
            UseSheild=true;
        }
    }
}
