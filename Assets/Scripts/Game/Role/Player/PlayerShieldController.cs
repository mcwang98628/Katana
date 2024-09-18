using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldController : PlayerController
{
    // private static readonly int HoldingShield = Animator.StringToHash("HoldingShield");
    // public bool IsCanHoldingShield = true;
    // public override bool IsCanMove => !((PlayerAttack)roleAttack).IsAccumulateing && base.IsCanMove;
    public override bool IsCanMove => base.IsCanMove && !((PlayerShieldAttack)roleAttack).IsAccumulateing;
    // private bool IsCanHoldingShield => !IsAttacking && !IsDie && !IsRolling;
    //public bool IsHoldingShield => EnemyTarget != null && (!IsAttacking|| _dash.IsShieldDashing) && IsCanHoldingShield ;
    // PlayerShieldDash _dash;
    // protected override void Awake()
    // {
    //     base.Awake();
    //     _dash = GetComponent<PlayerShieldDash>();
    // }
    // public float HoldingShieldStart { get; private set; }


    // public void SetCanHoldingShield(bool isOk)
    // {
    //     IsCanHoldingShield = isOk;
    // }
    //protected override void Update()
    //{
    //    base.Update();
    //    Animator.SetBool(HoldingShield,IsHoldingShield);
    //}

    // protected override void Awake()
    // {
    //     base.Awake();
    //     InputAction.OnInputEvent += ShieldInput;
    // }
    //
    // protected override void OnDestroy()
    // {
    //     base.OnDestroy();
    //     // ReSharper disable once DelegateSubtraction
    //     InputAction.OnInputEvent -= ShieldInput;
    // }
    //
    // private void ShieldInput(JoyStatusData joyStatusData)
    // {
    //     switch (joyStatusData.JoyStatus)
    //     {
    //         case UIJoyStatus.OnHoldStart:
    //         case UIJoyStatus.Holding:
    //         case UIJoyStatus.OnHoldDragStart:
    //         case UIJoyStatus.HoldDraging:
    //             SetHoldingShield(true);
    //             break;
    //         default:
    //             SetHoldingShield(false);
    //             break;
    //     }
    // }
}
