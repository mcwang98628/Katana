using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private string BuffInfoPath = "Assets/AssetsPackage/ScriptObject_Buff/AllBuff.asset";
    
    //Key：这个配置文件的唯一ID 。  Value:配置文件
    private Dictionary<BuffScriptableObject, int> buffScrObj_ValueKey = new Dictionary<BuffScriptableObject, int>();
    //Key：这个配置文件的唯一ID 。  Value:配置文件
    private Dictionary<int, BuffScriptableObject> buffScrObj_KeyValue = new Dictionary<int, BuffScriptableObject>();
    //id,buff
    public Dictionary<int, RoleBuff> BuffDatas = new Dictionary<int, RoleBuff>();

    void InitBuffInfo()
    {
        initNumber++;
        ResourcesManager.Inst.GetAsset<AllBuffObj>(BuffInfoPath, delegate(AllBuffObj aio)
        {
            for (int i = 0; i < aio.objs.Count; i++)
            {
                ParsingBuff(aio.objs[i],null);
            }
        });
        initNumber--;
    }

    public BuffScriptableObject GetBuffScrObj(int id)
    {
        if (buffScrObj_KeyValue.ContainsKey(id))
        {
            return buffScrObj_KeyValue[id];
        }

        return null;
    }
    //解析Buff
    public RoleBuff ParsingBuff(BuffScriptableObject buffObj,BuffLifeCycle lifeCycle)
    {
        
        if (buffObj == null)
        {
            Debug.LogError("Error 没传buff！");
            return null;
        }
        
        // string guid;
        if (!buffScrObj_ValueKey.ContainsKey(buffObj) && !buffScrObj_KeyValue.ContainsKey(buffObj.ID))
        {
            buffScrObj_ValueKey.Add(buffObj, buffObj.ID);
            buffScrObj_KeyValue.Add(buffObj.ID, buffObj);
        }

        if (lifeCycle == null)
        {
            return null;
        }

        RoleBuff roleBuff = new RoleBuff();
        roleBuff.ID = buffObj.ID;
        RoleBuffLifeCycle buffLifeCycle = null;
        BuffTrigger buffTrigger = null;
        BuffEffect buffEffect = null;
        BuffOverlayEffect buffOverlayEffect = null;
        switch (lifeCycle.LifeCycleType)
        {
            case BuffLifeCycleType.Time:
                buffLifeCycle = new Buff_Time_LifeCycle(lifeCycle.Duration);
                break;
            case BuffLifeCycleType.EnterRoom:
                buffLifeCycle = new Buff_EnterRoom_LifeCycle(lifeCycle.EnterRoomTimes);
                break;
            case BuffLifeCycleType.EnterFightRoom:
                buffLifeCycle = new Buff_EnterFightRoom_LifeCycle(lifeCycle.EnterFightRoomTimes);
                break;
            case BuffLifeCycleType.Attack:
                buffLifeCycle = new Buff_Attack_LifeCycle(lifeCycle.AttackTimes);
                break;
            case BuffLifeCycleType.Injured:
                buffLifeCycle = new Buff_Injured_LifeCycle(lifeCycle.InjuredTimes);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (buffObj.TriggerData.TriggerType)
        {
            case BuffTriggerType.Time:
                buffTrigger = new Buff_Time_Trigger(buffObj.TriggerData.IntervalTime);
                break;
            case BuffTriggerType.EnterRoom:
                buffTrigger = new Buff_EnterRoom_Trigger();
                break;
            case BuffTriggerType.EnterFightRoom:
                buffTrigger = new Buff_EnterFightRoom_Trigger();
                break;
            case BuffTriggerType.Attack:
                buffTrigger = new Buff_Attack_Trigger();
                break;
            case BuffTriggerType.Injured:
                buffTrigger = new Buff_Injured_Trigger();
                break;
        }
        switch (buffObj.EffectData.EffectType)
        {
            case BuffEffectType.MaxHp:
                buffEffect = new Buff_MaxHp_Effect(buffObj.EffectData.MaxHpValue);
                break;
            case BuffEffectType.Hp:
                buffEffect = new Buff_Hp_Effect(buffObj.EffectData.HpValue);
                break;
            case BuffEffectType.AttackPower:
                buffEffect = new Buff_AttackPower_Effect(buffObj.EffectData.AttackPower);
                break;
            case BuffEffectType.AttackSpeed:
                buffEffect = new Buff_AttackSpeed_Effect(buffObj.EffectData.AttackSpeed);
                break;
            case BuffEffectType.MoveSpeed:
                buffEffect = new Buff_MoveSpeed_Effect(buffObj.EffectData.MoveSpeed);
                break;
            case BuffEffectType.EnemyCold:
                buffEffect = new Buff_EnemyCold_Effect(buffObj.EffectData.AnimSpeed, buffObj.EffectData.EnemyMoveSpeed);
                break;
            case BuffEffectType.Frozen:
                buffEffect = new Buff_Frozen_effect();
                break;
            case BuffEffectType.Dizziness:
                buffEffect = new Buff_Dizziness_effect();
                break;
            case BuffEffectType.IsGod:
                buffEffect = new Buff_God_Effect();
                break;
            case BuffEffectType.InjureMultiplier:
                buffEffect = new Buff_InjuryMultiplier_Effect(buffObj.EffectData.InjureMultiplier);
                break;
            case BuffEffectType.HpTreatMultiplier:
                buffEffect = new Buff_HpTreatMultiplier_Effect(buffObj.EffectData.HpTreatMultiplier);
                break;
            case BuffEffectType.SuckBlood:
                buffEffect = new Buff_SuckBlood_Effect(buffObj.EffectData.SuckBloodValue);
                break;
        }
        buffEffect.SetColor(buffObj.EffectData.BuffColor,buffObj.EffectData.BuffAimColor,buffObj.EffectData.BuffColorType );

        switch (buffObj.OverlayEffectData.EffectType)
        {
            case BuffOverlayEffectType.AOEDmgBuff:
                buffOverlayEffect = new BuffOverlay_AOE(
                    buffObj.OverlayEffectData.Probability,
                    buffObj.OverlayEffectData.AOEAttackDistance,
                    buffObj.OverlayEffectData.AOEAttackPower,
                    buffObj.OverlayEffectData.AOEIsUseMove,
                    buffObj.OverlayEffectData.AOEMoveSpeed,
                    buffObj.OverlayEffectData.AOEMoveTime,
                    buffObj.OverlayEffectData.AOEParticle,
                    buffObj.OverlayEffectData.ParticleOffset,
                    buffObj.OverlayEffectData.AOEBuffObj,
                    buffObj.OverlayEffectData.AOEBuffLife);
                break;
        }
        roleBuff.Init(buffLifeCycle,buffTrigger, buffEffect, buffOverlayEffect, buffObj.EffectData.particleEffect, buffObj.Icon, buffObj.Name, buffObj.Describe,buffObj.BuffOverlayType);

        if (!BuffDatas.ContainsKey(roleBuff.ID))
        {
            BuffDatas.Add(roleBuff.ID, roleBuff);
        }

        return roleBuff;
    }
}
