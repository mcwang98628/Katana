using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class RolePower : MonoBehaviour
{

    bool freezeSkillPower;
    public float MaxPower => maxPower+RoleController.GetAttributeBonusValue(AttributeType.MaxPower);
    [SerializeField] [LabelText("最大翻滚体力")] [BoxGroup("翻滚")]
    private float maxPower = 320;
    [HideInInspector]//当前体力
    public float CurrentPower { get; private set; }
    public float PowerRecovery => powerRecovery+RoleController.GetAttributeBonusValue(AttributeType.PowerRecovery);
    [SerializeField] [LabelText("翻滚体力恢复 /s")] [BoxGroup("翻滚")]
    private float powerRecovery=50;
    [SerializeField] [LabelText("翻滚所需体力")] [BoxGroup("翻滚")]
    private float rollNeedPower = 10;
    public bool IsCanRoll => CurrentPower >= rollNeedPower && !RoleController.IsDizziness;
    public float RollNeedPower => rollNeedPower;
    
    [SerializeField][LabelText("攻击恢复Skill能量")][BoxGroup("技能")]
    protected int attackPowerRecover = 0;

    
    //SkillPower
    public float MaxSkillPower { get; private set; } = 100;
    public float CurrentSkillPower { get; private set; }
    
    
    

    private RoleController roleController;

    protected RoleController RoleController
    {
        get
        {
            if (roleController == null)
            {
                roleController = this.gameObject.GetComponent<RoleController>();
            }
            return roleController;
        }
    }
    protected virtual void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnPlayerAttackHitEnemy,OnPlayerAttack);
        EventManager.Inst.AddEvent(EventName.OnRoleRoll,OnRoleRoll);
        Init();
    }

    protected virtual void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnPlayerAttackHitEnemy,OnPlayerAttack);
        EventManager.Inst.RemoveEvent(EventName.OnRoleRoll,OnRoleRoll);
    }
    
    public virtual void Init()
    {
        CurrentPower = 0f;
    }
    
    protected virtual void Update()
    {
        AddPower(PowerRecovery * Time.deltaTime);
    }
    
    protected virtual void OnPlayerAttack(string arg1, object arg2)
    {
        AddSkillPower(attackPowerRecover);
    }
    
    

    private void OnRoleRoll(string arg1, object arg2)
    {
        AddPower(-rollNeedPower);
    }

    public virtual void AddSkillPower(float value)
    {
        if (!freezeSkillPower)
        {
            CurrentSkillPower += value;
            if (CurrentSkillPower<0)
            {
                CurrentSkillPower = 0;
            }else if (CurrentSkillPower>MaxSkillPower)
            {
                CurrentSkillPower = MaxSkillPower;
            }
            EventManager.Inst.DistributeEvent(EventName.OnRoleSkillPowerChange, RoleController.TemporaryId);
        }
    }
    public void AddPower(float value)
    {
        CurrentPower += value;
        if (CurrentPower >= MaxPower)
        {
            CurrentPower = MaxPower;
        }
        else if (CurrentPower < 0)
        {
            CurrentPower = 0;
        }
        EventManager.Inst.DistributeEvent(EventName.OnRolePowerChange, RoleController.TemporaryId);
    }
    public void FreezeSkillPower()
    {
        freezeSkillPower = true;
    }
    public void UnFreezeSkillPower()
    {
        freezeSkillPower = false;
    }
}
