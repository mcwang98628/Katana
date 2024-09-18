using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerKatanaAttack : PlayerAttack
{
    [SerializeField][LabelText("不使用移动蓄力")]
    private bool _notUseMoveAccumulate;
    [SerializeField][LabelText("蓄力自动释放")]
    private bool _autoAccumulateAttack = false;
    
    protected override void Awake()
    {
        base.Awake();
        EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnRoleInjured);
        EventManager.Inst.AddEvent(EventName.KatanaAccumulateEnd,OnKatanaAccumulateEnd);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnRoleInjured);
        EventManager.Inst.RemoveEvent(EventName.KatanaAccumulateEnd,OnKatanaAccumulateEnd);
    }

    private void OnKatanaAccumulateEnd(string arg1, object arg2)
    {
        if (_autoAccumulateAttack)
        {
            AccumlateAttack();
            isDisEvent = false;
            isDisMoveEvent = false;
        }
    }
    private void OnRoleInjured(string arg1, object arg2)
    {
        RoleInjuredInfo info = (RoleInjuredInfo) arg2;
        if (info.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }

        if (info.Dmg.Interruption)
        {
            isDisEvent = false;
            isDisMoveEvent = false;
            // SetAccumulateing(false);
        }
    }
    
    // public override void BackMoveAnim()
    // {
    //     if (((PlayerController)roleController).CanMoveSkillUsingCount <= 0 && ((PlayerController)roleController).CantMoveSkillUsingCount <= 0 )
    //     {
    //         base.BackMoveAnim();
    //     }
    // }

    
    [LabelText("蓄力时间")] [SerializeField]
    public float holdTime = 0.8f;
    
    public float HoldTime => holdTime + roleController.GetAttributeBonusValue(AttributeType.AccumulateTime);
    
    
    public bool IsAccumulateComplete => IsAccumulateing && Time.time - AccumulateingStartTime > HoldTime;
    // private float _holdStartTime;
    // private bool _isHolding = false;

    // private static readonly int AccumulateAttacking = Animator.StringToHash("AccumulateAttacking");

    // // protected static readonly int HoldAttackStatus = Animator.StringToHash("HoldAttackStatus");
    
    
    private bool isDisEvent;
    private bool isDisMoveEvent = false;
    protected override void Update()
    {
        base.Update();
    
        if (roleController.IsRolling)
        {
            SetAccumulateing(false);
        }
        
        if (IsAccumulateing && Time.time - AccumulateingStartTime > HoldTime && !isDisEvent && !isDisMoveEvent)
        {
            isDisEvent = true;
            EventManager.Inst.DistributeEvent(EventName.KatanaAccumulateEnd);
        }
        
        if (IsCanDash && !isDisEvent && !isDisMoveEvent && !_notUseMoveAccumulate)
        {
            isDisMoveEvent = true;
            EventManager.Inst.DistributeEvent(EventName.KatanaAccumulateEnd);
        }
    }
    
    protected override void OnInput(JoyStatusData statusData)
    {
        if (roleController.IsDie)
        {
            SetAccumulateing(false);
            roleController.Animator.SetBool(Accumulate,false);
            return;
        }

        if (statusData.JoyStatus == UIJoyStatus.OnPressUp && !IsAccumulateing)
        {
            roleController.InputAttack();
            return;
        }
        
        if (statusData.JoyStatus == UIJoyStatus.OnHoldStart||
            statusData.JoyStatus == UIJoyStatus.Holding)
        {
            if (IsAccumulateing)
            {
                return;
            }
            if (roleController.EnemyTarget != null)
            {
                roleController.Animator.transform.forward =
                    (roleController.EnemyTarget.transform.position - roleController.transform.position).normalized;
            }
            SetAccumulateing(true);
        }
        else if (statusData.JoyStatus == UIJoyStatus.OnHoldEnd)
        {
            if (Time.time - AccumulateingStartTime > HoldTime)
            {
                AccumlateAttack();
            }
            SetAccumulateing(false);
            isDisEvent = false;
            isDisMoveEvent = false;
        }
        else if (statusData.JoyStatus == UIJoyStatus.OnHoldDragStart)
        {
            if (IsAccumulateComplete)
            {
                ((PlayerMove)roleController.roleMove).SetMoveSpeedMax();
            }
            SetAccumulateing(false);
        }        
        else if (statusData.JoyStatus == UIJoyStatus.OnHoldDragEnd
                 || statusData.JoyStatus == UIJoyStatus.HoldDraging
                 || statusData.JoyStatus == UIJoyStatus.Idle)
        {
            SetAccumulateing(false);
        }

        if (statusData.JoyStatus == UIJoyStatus.Idle)
        {
            isDisEvent = false;
            isDisMoveEvent = false;
        }
        
        roleController.Animator.SetBool(Accumulate,IsAccumulateing);

        MoveAccumulateAttack(statusData);
    }

    public void AccumlateAttack()
    {
        Vector3 dir;
        if (roleController.EnemyTarget == null)
        {
            dir = roleController.Animator.transform.forward;
        }
        else
        {
            dir = roleController.EnemyTarget.transform.position - roleController.transform.position;
        }
        StartAccumulateAttack(dir);
        StartCoroutine(CloseAnimBool());
    }
    public bool IsCanDash
    {
        get
        {
            float value2 = roleController.MaxMoveSpeed - roleController.MinMoveSpeed;
            float value = value2 - (roleController.MaxMoveSpeed - roleController.CurrentMoveSpeed);
            return ((roleController.IsMoving && value / value2 >= 0.75f) || ((PlayerKatanaAttack)roleController.roleAttack).IsAccumulateComplete ) && !roleController.IsRolling && !roleController.IsDie;
        }
    }
    void MoveAccumulateAttack(JoyStatusData data)
    {
        if (_notUseMoveAccumulate)
            return;
        
        
        if (data.JoyStatus == UIJoyStatus.OnDragEnd
            || data.JoyStatus == UIJoyStatus.OnHoldDragEnd)
        {
            if (!IsCanDash)
            {
                return;
            }
        
            Vector3 dir;
            if (roleController.EnemyTarget==null)
            {
                dir = roleController.Animator.transform.forward;
            }
            else
            {
                dir = roleController.EnemyTarget.transform.position - roleController.transform.position;
            }
            StartRunAccumulateAttack(dir);
            
            
            isDisEvent = false;
            isDisMoveEvent = false;
            
        }
    }

    IEnumerator CloseAnimBool()
    {
        yield return null;
        SetAccumulateing(false);
    }


    public void StartAccumulateAttack(Vector3 dir)
    {
        roleController.Animator.transform.forward = dir;
        // roleController.Animator.SetInteger(AccumulateAttackType,currentAccumulateAttackType);
        roleController.Animator.SetTrigger(AccumulateAttack);
        EventManager.Inst.DistributeEvent(EventName.KatanaAccumulateAttack);
    }

    public void StartRunAccumulateAttack(Vector3 dir)
    {
        roleController.Animator.transform.forward = dir;
        // roleController.Animator.SetInteger(AccumulateAttackType,currentAccumulateAttackType);
        roleController.Animator.SetTrigger(RunAccumulateAttack);
        EventManager.Inst.DistributeEvent(EventName.KatanaAccumulateAttack);
    }
}
