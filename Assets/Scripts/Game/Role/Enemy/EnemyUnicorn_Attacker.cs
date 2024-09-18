using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnicorn_Attacker : EnemyAttack
{
    [SerializeField]
    protected DmgBuffOnTouch dmgTouch;
    
    private static readonly int Sprint = Animator.StringToHash("Sprint");

    protected override void Start()
    {
        base.Start();
        initDmgTouch();
    }

    public void AttackFunc(Vector3 dir,float dis,float attackMoveWarningTime)
    {
        if (roleController.IsDie)
        {
            return;
        }
        base.AttackFunc();
        roleController.Animator.transform.forward = dir;
        var rect = IndicatorManager.Inst.ShowRectangleIndicator();
        rect.Show(roleController,new Vector3(0,0.1f,0), dir,new Vector3(1f,1,0),new Vector3(0.5f,1,dis), attackMoveWarningTime);
    }

    public void StartSprint(float moveTime, float moveSpeed, Vector3 moveDir,Action callback = null)
    {
        if (roleController.IsDie)
        {
            return;
        }
        roleController.Animator.SetBool(Sprint,true);
        dmgTouch.gameObject.SetActive(true);
        roleController.FastMove(moveTime,moveSpeed,moveDir, () =>
        {
            endSprint();
            if (callback!=null)
            {
                callback.Invoke();
            }
        });
    }

    private void endSprint()
    {
        roleController.Animator.SetBool(Sprint,false);
        dmgTouch.gameObject.SetActive(false);
        roleController.SetIsAttacking(true);
    }

    private void initDmgTouch()
    {
        dmgTouch.Init(roleController);
        dmgTouch.gameObject.SetActive(false);
    }
}
