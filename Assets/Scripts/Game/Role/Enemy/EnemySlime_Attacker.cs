using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemySlime_Attacker : EnemyAttack
{
    [SerializeField]
    private DmgBuffOnTouch dmgTouch;

    [SerializeField] private ParticleSystem FX;
    [SerializeField] private FeedBackObject GroundFeedback;

    private static readonly int SlimeAttack = Animator.StringToHash("SlimeAttack");

    protected override void Start()
    {
        base.Start();
        initDmgTouch();
    }

    public void AttackFunc()
    {
        if (roleController.IsDie)
        {
            return;
        }
        base.AttackFunc();
    }

    public void FlyAttack(Vector3 targetPos,float size,float time)
    {
        if (roleController.IsDie)
        {
            return;
        }
        AttackIndeicate_Sector attackIndeicateSector = IndicatorManager.Inst.ShowAttackIndicator();
        attackIndeicateSector.Show(roleController,targetPos,360,size,time,Color.red);
        roleController.Animator.SetBool(SlimeAttack,true);
        transform.DOMoveY(20, 0.4f).SetEase(Ease.Linear);
    }

    public void DownAttack(Vector3 targetPos)
    {
        if (roleController.IsDie)
        {
            return;
        }
        transform.position = new Vector3(targetPos.x,transform.position.y,targetPos.z);
        transform.DOMoveY(0, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (FX != null)
            {
                FX.Play();
            }
            FeedbackManager.Inst.UseFeedBack(roleController,GroundFeedback);
            roleController.Animator.SetBool(SlimeAttack,false);
            dmgTouch.TakeEffect();
        });
    }
    private void initDmgTouch()
    {
        dmgTouch.Init(roleController);
        dmgTouch.gameObject.SetActive(false);
    }
}
