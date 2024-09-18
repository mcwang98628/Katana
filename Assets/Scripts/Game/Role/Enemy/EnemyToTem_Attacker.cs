using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToTem_Attacker : EnemyAttack
{
    [SerializeField]
    private DmgBuffOnTouch dmgTouch;
    
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
        dmgTouch.TakeEffect();
        if (StartAttackParticle!=null)
        {
            StartAttackParticle.Play();
        }
    }

    private void initDmgTouch()
    {
        dmgTouch.Init(roleController);
        dmgTouch.gameObject.SetActive(false);
    }
}
