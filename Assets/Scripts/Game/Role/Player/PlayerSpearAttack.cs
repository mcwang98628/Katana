using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerSpearAttack : PlayerAttack
{
    [SerializeField]
    private Transform Weapon;

    [SerializeField]
    private Vector3 attackPos;
    [SerializeField]
    private Vector3 notAttackPos;

    private Tweener _tweener;
    private bool isAccepting;
    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go,eventName);
        if (eventName.Contains(AnimatorEventName.StartAttack_))
        {
            isClose = false;
            if (_tweener!=null)
            {
                _tweener.Kill(false);
            }
            _tweener = Weapon.DOLocalMove(attackPos, 0.2f);
        }
        else if (eventName.Contains("CloseWeapon"))
        {
            closeWeapon();
        }
        else if (eventName.Contains("StartAccept"))
        {
            isAccepting = true;
        }
        else if (eventName.Contains("EndAccept"))
        {
            isAccepting = false;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        EventManager.Inst.AddEvent(EventName.OnRoleDodgeInjured,OnGodInjured);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleDodgeInjured,OnGodInjured);
    }

    private void OnGodInjured(string arg1, object arg2)
    {
        var data = (RoleInjuredInfo) arg2;
        if (data.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }
        if (isAccepting)
        {
            roleController.Animator.SetTrigger("AccepTrigger");
        }
    }

    // public override void Attack_B_Func()
    // {
    //     if (roleController.MaxSkillPower != roleController.CurrentSkillPower)
    //     {
    //         EventManager.Inst.DistributeEvent(EventName.NeedPower);
    //         return;
    //     }
    //     roleController.AddSkillPower(-roleController.MaxSkillPower);
    //     roleController.Animator.SetTrigger(Attack_B);
    // }
    // protected override void OnRoleInjured(string arg1, object arg2)
    // {
    //     base.OnRoleInjured(arg1, arg2);
    //     
    //     var data = (RoleInjuredInfo) arg2;
    //     if (data.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
    //     {
    //         return;
    //     }
    //
    // }

    private bool isClose = false;
    void closeWeapon()
    {
        if (isClose)
        {
            return;
        }
        
        isClose = true;
        if (_tweener!=null)
        {
            _tweener.Kill(false);
        }
        _tweener = Weapon.DOLocalMove(notAttackPos, 0.2f);
    }
}
