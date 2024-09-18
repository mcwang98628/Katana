using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameBuff/BuffObj")]
public class BuffScriptableObject : ScriptableObject
{
    public int ID;
    
    [Title("Buff信息")]
    [PreviewField(85)]
    [HideLabel]
    [HorizontalGroup("Info", 85)]
    public Sprite Icon;
    [VerticalGroup("Info/Desc")]
    [LabelText("名称")]
    [TextArea(1, 1)]
    public string Name;
    [VerticalGroup("Info/Desc")]
    [LabelText("描述")]
    [TextArea]
    public string Describe;

    [Space]
    [Space]

    // [LabelText("叠加数量")]
    // public int OverlayNumber;
    [Space]
    [Space]
    [LabelText("Buff叠加类型")]
    public BuffOverlayType BuffOverlayType;
    
    // [LabelText("BUFF持续")]
    // [HideInInspector]
    // public BuffLifeCycle LifeCycle;
    [LabelText("BUFF触发")]
    public BuffTriggerData TriggerData;
    [LabelText("Buff效果")]
    public BuffEffectData EffectData;
    [LabelText("叠加触发效果")]
    public BuffOverlayEffectData OverlayEffectData;
}

public enum BuffOverlayType
{
    [LabelText("普通叠加 单纯加buff")]
    Normal,
    [LabelText("叠加buff生命周期")]
    AppendLife,
    [LabelText("刷新buff生命周期")]
    ReSetLife,
    [LabelText("不添加")]
    NoAdd,
}
public enum BuffLifeCycleType
{
    [LabelText("计时")]
    Time,
    [LabelText("进入房间")]
    EnterRoom,
    [LabelText("进入战斗房间")]
    EnterFightRoom,
    [LabelText("攻击")]
    Attack,
    [LabelText("受伤")]
    Injured,
}
public enum BuffTriggerType
{
    [LabelText("计时")]
    Time,
    [LabelText("进入房间")]
    EnterRoom,
    [LabelText("进入战斗房间")]
    EnterFightRoom,
    [LabelText("攻击")]
    Attack,
    [LabelText("受伤")]
    Injured,
}

[Serializable]
public class BuffLifeCycle
{
    [LabelText("类型")]
    public BuffLifeCycleType LifeCycleType;

    [LabelText("持续时间")]
    [ShowIf("LifeCycleType", BuffLifeCycleType.Time)]
    public float Duration;

    [LabelText("持续的房间次数")]
    [ShowIf("LifeCycleType", BuffLifeCycleType.EnterRoom)]
    public int EnterRoomTimes;

    [LabelText("持续的战斗房间次数")]
    [ShowIf("LifeCycleType", BuffLifeCycleType.EnterFightRoom)]
    public int EnterFightRoomTimes;

    [LabelText("攻击次数")]
    [ShowIf("LifeCycleType", BuffLifeCycleType.Attack)]
    public int AttackTimes;

    [LabelText("受伤次数")]
    [ShowIf("LifeCycleType", BuffLifeCycleType.Injured)]
    public int InjuredTimes;
}

[Serializable]
public class BuffTriggerData
{
    [LabelText("触发类型")]
    public BuffTriggerType TriggerType;

    [LabelText("间隔时间 （-1为立刻且只触发1次）")]
    [ShowIf("TriggerType", BuffTriggerType.Time)]
    public float IntervalTime;
}

public enum BuffEffectType
{
    [LabelText("血上限")]
    MaxHp,
    [LabelText("血量")]
    Hp,
    [LabelText("攻击力")]
    AttackPower,
    [LabelText("攻击速度")]
    AttackSpeed,
    [LabelText("移动速度")]
    MoveSpeed,
    [LabelText("敌人寒冷-减速")]
    EnemyCold,
    [LabelText("冰冻")]
    Frozen,
    [LabelText("眩晕")]
    Dizziness,
    [LabelText("无敌")]
    IsGod,
    [LabelText("受伤比率")]
    InjureMultiplier,
    [LabelText("治疗恢复率")]
    HpTreatMultiplier,
    [LabelText("吸血")]
    SuckBlood

}

public enum BuffColorType
{
    Trigger,
    Continued
}
[Serializable]
public class BuffEffectData
{
    [BoxGroup("颜色")]
    [LabelText("Buff颜色")]
    public Color BuffColor;
    [BoxGroup("颜色")]
    [LabelText("BuffAim颜色")]
    public Color BuffAimColor;
    [BoxGroup("颜色")]
    [LabelText("Buff颜色显示类型")]
    public BuffColorType BuffColorType;
    
    [LabelText("效果类型")]
    public BuffEffectType EffectType;

    [LabelText("粒子特效")]
    public ParticleSystem particleEffect;

    [LabelText("增加血上限")]
    [ShowIf("EffectType", BuffEffectType.MaxHp)]
    public float MaxHpValue;
    [LabelText("增加血量")]
    [ShowIf("EffectType", BuffEffectType.Hp)]
    public float HpValue;
    [LabelText("增加攻击力")]
    [ShowIf("EffectType", BuffEffectType.AttackPower)]
    public int AttackPower;
    [LabelText("增加攻击速度")]
    [ShowIf("EffectType", BuffEffectType.AttackSpeed)]
    public float AttackSpeed;
    [LabelText("增加移动速度")]
    [ShowIf("EffectType", BuffEffectType.MoveSpeed)]
    public float MoveSpeed;

    [LabelText("动画速度缩放比例")]
    [ShowIf("EffectType", BuffEffectType.EnemyCold)]
    public float AnimSpeed;
    [LabelText("移动速度缩放比例")]
    [ShowIf("EffectType", BuffEffectType.EnemyCold)]
    public float EnemyMoveSpeed;

    [LabelText("伤害减少比例（负数则增加）")]
    [ShowIf("EffectType", BuffEffectType.InjureMultiplier)]
    public float InjureMultiplier;

    [LabelText("增加回复率")]
    [ShowIf("EffectType", BuffEffectType.HpTreatMultiplier)]
    public float HpTreatMultiplier;
    [LabelText("吸血:攻击 比例 0-1f")]
    [ShowIf("EffectType", BuffEffectType.SuckBlood)]
    public float SuckBloodValue;

}

public enum BuffOverlayEffectType
{
    [LabelText("效果叠加")]
    NULL,
    [LabelText("AOE伤害Buff")]
    AOEDmgBuff,

}
[Serializable]
public class BuffOverlayEffectData
{
    [LabelText("效果类型")]
    public BuffOverlayEffectType EffectType;

    [LabelText("概率s")]
    [HideIf("EffectType", BuffOverlayEffectType.NULL)]
    public List<int> Probability = new List<int>();

    #region AOE

    [LabelText("AOE伤害")]
    [ShowIf("EffectType", BuffOverlayEffectType.AOEDmgBuff)]
    public int AOEAttackPower;

    [LabelText("AOE距离")]
    [ShowIf("EffectType", BuffOverlayEffectType.AOEDmgBuff)]
    public float AOEAttackDistance;

    [LabelText("是否击退")]
    [ShowIf("EffectType", BuffOverlayEffectType.AOEDmgBuff)]
    public bool AOEIsUseMove;

    [LabelText("击退速度")]
    [ShowIf("AOEIsUseMove")]
    [ShowIf("EffectType", BuffOverlayEffectType.AOEDmgBuff)]
    public float AOEMoveSpeed;

    [LabelText("击退时间")]
    [ShowIf("AOEIsUseMove")]
    [ShowIf("EffectType", BuffOverlayEffectType.AOEDmgBuff)]
    public float AOEMoveTime;

    [LabelText("AOE特效")]
    [ShowIf("EffectType", BuffOverlayEffectType.AOEDmgBuff)]
    public ParticleSystem AOEParticle;
    [LabelText("AOE特效-偏移")]
    [ShowIf("EffectType", BuffOverlayEffectType.AOEDmgBuff)]
    public Vector3 ParticleOffset;
    [LabelText("Buff效果")]
    [ShowIf("EffectType", BuffOverlayEffectType.AOEDmgBuff)]
    public BuffScriptableObject AOEBuffObj;

    [LabelText("Buff持续时间")]
    [ShowIf("EffectType", BuffOverlayEffectType.AOEDmgBuff)]
    public BuffLifeCycle AOEBuffLife;


    #endregion


}
