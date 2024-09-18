using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameItem/ItemEffectObj")]
public class ItemEffectObj : ScriptableObject
{
    [LabelText("名称")]//主要用于编辑时候好区分
    public string Name;
    [LabelText("介绍")]//主要用于编辑时候好区分
    public string Desc;
    [LabelText("触发")]
    public ItemEffectTriggerData triggerData;
    [LabelText("效果")]
    public ItemEffectEffectData effectData;
}

[Serializable]
public class ItemEffectTriggerData
{
    [LabelText("触发事件类型")]
    public TriggerEventType TriggerEventType;

    [LabelText("触发方式")]
    [HideIf("TriggerEventType", TriggerEventType.Get)]
    [HideIf("TriggerEventType", TriggerEventType.RoleMove)]
    [HideIf("TriggerEventType", TriggerEventType.RoleRoll)]
    [HideIf("TriggerEventType", TriggerEventType.Active)]
    [HideIf("TriggerEventType", TriggerEventType.NewRoom)]
    public TriggerType TriggerType;

    [HideIf("TriggerEventType", TriggerEventType.NewRoom)]
    [HideIf("TriggerEventType", TriggerEventType.Get)]
    [HideIf("TriggerEventType", TriggerEventType.RoleMove)]
    [HideIf("TriggerEventType", TriggerEventType.RoleRoll)]
    [HideIf("TriggerEventType", TriggerEventType.Active)]
    [ShowIf("TriggerType", TriggerType.Probability)]
    [LabelText("触发概率(0~100)")]
    public int Probability;



    [ShowIf("TriggerType", TriggerType.Times)]
    [ShowIf("TriggerEventType", TriggerEventType.Attack)]
    [LabelText("攻击次数")]
    public int AttackTimes;
    [ShowIf("TriggerType", TriggerType.Times)]
    [ShowIf("TriggerEventType", TriggerEventType.Attack)]
    [LabelText("最小偏移次数")]
    public int AttackMinOffsetTimes;
    [ShowIf("TriggerType", TriggerType.Times)]
    [ShowIf("TriggerEventType", TriggerEventType.Attack)]
    [LabelText("最大偏移次数")]
    public int AttackMaxOffsetTimes;


    [ShowIf("TriggerType", TriggerType.Times)]
    [ShowIf("TriggerEventType", TriggerEventType.Injured)]
    [LabelText("攻击次数")]
    public int InjuredTimes;

    [ShowIf("TriggerType", TriggerType.Times)]
    [ShowIf("TriggerEventType", TriggerEventType.KillEnemy)]
    [LabelText("击杀数量")]
    public int KillEnemyCount;

    [ShowIf("TriggerType", TriggerType.Times)]
    [ShowIf("TriggerEventType", TriggerEventType.Time)]
    [LabelText("触发间隔 float")]
    public float IntervalTime;

    [ShowIf("TriggerEventType", TriggerEventType.RoleMove)]
    [LabelText("触发间隔 float")]
    public float MoveIntervalTime;
    [ShowIf("TriggerEventType", TriggerEventType.RoleRoll)]
    [LabelText("触发间隔 float")]
    public float RollIntervalTime;
    [ShowIf("TriggerEventType", TriggerEventType.PlayerShieldHoldingTrigger)]
    [LabelText("举盾 or 放盾")]
    public bool PlayerShieldHoldingTrigger;
    
    
    [ShowIf("TriggerEventType", TriggerEventType.RoleDeadOfBuff)]
    [LabelText("BuffId")]
    public int RoleDeadBuffId;
    [ShowIf("TriggerEventType", TriggerEventType.Keyboard)]
    [LabelText("Key")]
    public string KeyStr;
    [ShowIf("TriggerEventType", TriggerEventType.AnimatorEventTrigger)]
    [LabelText("事件名")]
    public string EventName;
}

[Serializable]
public partial class  ItemEffectEffectData
{
    [LabelText("效果类型")][PropertyOrder(-1)]
    public EffectType EffectType;

    [LabelText("增加攻击力")]
    [ShowIf("EffectType", EffectType.ChangeAttackPower)]
    public int AttackPower;
    [LabelText("增加攻击力百分比 0-1 float")]
    [ShowIf("EffectType", EffectType.ChangeAttackPower)]
    public float AttackPowerPercentage;
    [LabelText("增加攻速")]
    [ShowIf("EffectType", EffectType.ChangeAttackSpeed)]
    public float AttackSpeed;
    // [LabelText("增加溅射伤害 0-1-max float")]
    // [ShowIf("EffectType", EffectType.SplashDamage)]
    // public float SplashDamage;
    [LabelText("增加暴击概率0-1")]
    [ShowIf("EffectType", EffectType.ChangeCriticalProbability)]
    public float CriticalProbability;
    [LabelText("增加暴击倍数")]
    [ShowIf("EffectType", EffectType.ChangeCriticalMultiplier)]
    public float CriticalMultiplier;
    [LabelText("伤害减少比例（负数则增加）")]
    [ShowIf("EffectType", EffectType.ChangeInjuryMultiplier)]
    public float InjuryMultiplier;
    [LabelText("增加血上限")]
    [ShowIf("EffectType", EffectType.ChangeMaxHp)]
    public float MaxHp;
    [LabelText("血上限 百分比0-1f")]
    [ShowIf("EffectType", EffectType.ChangeMaxHp)]
    public float MaxHpPercentage;
    [LabelText("增加治疗倍率")]
    [ShowIf("EffectType", EffectType.ChangeHpTreatMultiplier)]
    public float TreatMultiplier;
    [LabelText("血量")]
    [ShowIf("EffectType", EffectType.ChangeCurrentHp)]
    public float CurrentHp;
    [LabelText("血上限 百分比0-1f")]
    [ShowIf("EffectType", EffectType.ChangeCurrentHp)]
    public float CurrentHpPercentage;
    [LabelText("粒子特效")]
    [ShowIf("EffectType", EffectType.ChangeCurrentHp)]
    public ParticleSystem ChangeCurrentHpParticle;

    
    [ShowIf("EffectType", EffectType.ChangeMoveSpeed)]
    public float MoveSpeed;
    [ShowIf("EffectType", EffectType.ChangeMoveSpeed)]
    public bool SetAcceleration = false;
    [ShowIf("SetAcceleration", true)]
    public bool isUseAccel;
    [ShowIf("SetAcceleration", true)]
    public float startAccel;
    [ShowIf("SetAcceleration", true)]
    public float stopAccel;

    [ShowIf("EffectType", EffectType.ChangeEnemySpeed)]
    public float EnemySpeed;

    // [ShowIf("EffectType", EffectType.AOEAttack)]
    // public SpwanPosType AOECenterType;
    // [LabelText("AOE伤害")]
    // [ShowIf("EffectType", EffectType.AOEAttack)]
    // public int AOEAttackPower;
    // [LabelText("AOE伤害 攻击力百分比 0-1 float")]
    // [ShowIf("EffectType", EffectType.AOEAttack)]
    // public float AOEAttackPowerPercentage;
    // [LabelText("AOE距离")]
    // [ShowIf("EffectType", EffectType.AOEAttack)]
    // public float AOEAttackDistance;
    // [LabelText("是否击退")]
    // [ShowIf("EffectType", EffectType.AOEAttack)]
    // public bool AOEIsUseMove;
    // [LabelText("击退速度")]
    // [ShowIf("AOEIsUseMove")]
    // [ShowIf("EffectType", EffectType.AOEAttack)]
    // public float AOEMoveSpeed;
    // [LabelText("击退时间")]
    // [ShowIf("AOEIsUseMove")]
    // [ShowIf("EffectType", EffectType.AOEAttack)]
    // public float AOEMoveTime;
    //
    // [LabelText("AOE特效")]
    // [ShowIf("EffectType", EffectType.AOEAttack)]
    // public ParticleSystem AOEParticle;
    // [LabelText("特效偏移")]
    // [ShowIf("EffectType", EffectType.AOEAttack)]
    // public Vector3 AOEParticleOffset;

    [LabelText("Buff持续")]
    [ShowIf("EffectType", EffectType.Buff)]
    public BuffLifeCycle BuffLifeCycle;
    [LabelText("Buff效果")]
    [ShowIf("EffectType", EffectType.Buff)]
    public BuffScriptableObject BurnBuffObj;

    // [LabelText("Buff效果")]
    // [ShowIf("EffectType", EffectType.AOEAttack)]
    // public BuffScriptableObject AOEBuffObj;
    // [LabelText("Buff持续")]
    // [ShowIf("EffectType", EffectType.AOEAttack)]
    // public BuffLifeCycle AoeBuffLifecycle;

    [LabelText("强化Tag")]
    [ShowIf("EffectType", EffectType.LightningChain)]
    public string LightningChainTag;
    [LabelText("弹射次数")]
    [ShowIf("EffectType", EffectType.LightningChain)]
    public int CatapultTimes;
    [LabelText("每次弹射距离")]
    [ShowIf("EffectType", EffectType.LightningChain)]
    public int CatapultDistance;
    [LabelText("弹射伤害")]
    [ShowIf("EffectType", EffectType.LightningChain)]
    public int CatapultAttackPower;
    [LabelText("弹射伤害 攻击力百分比")]
    [ShowIf("EffectType", EffectType.LightningChain)]
    public float CatapultAttackPowerPercentage;
    [LabelText("弹射伤害 强化后攻击力百分比")]
    [ShowIf("EffectType", EffectType.LightningChain)]
    public float CatapultAttackPowerPercentage2;
    [LabelText("强化后 颜色")]
    [ShowIf("EffectType", EffectType.LightningChain)]
    [ColorUsage(true,true)]
    public Color LightningChainColor;
    [LabelText("击中Fx")]
    [ShowIf("EffectType", EffectType.LightningChain)]
    public GameObject LightningChainHitFx;


    [LabelText("环绕物")]
    [ShowIf("EffectType", EffectType.Surroud)]
    public Surround_Obj SurroudObj;

    [LabelText("环绕物 攻击力")]
    [ShowIf("EffectType", EffectType.Surroud)]
    public int SurroudObjAttackPower;
    [LabelText("环绕物 攻击力百分比")]
    [ShowIf("EffectType", EffectType.Surroud)]
    public float SurroudObjAttackPowerPercentage;
    
    
    [LabelText("速度")]
    [ShowIf("EffectType", EffectType.SurroudSpeed)]
    public float SurroudSpeedValue;
    
    
    // [LabelText("目标-环绕物")]
    // [ShowIf("EffectType", EffectType.SurroudObjConversion)]
    // public List<Surround_Obj> ConversionSurroudObjs;
    // [LabelText("环绕物 攻击力")]
    // [ShowIf("EffectType", EffectType.SurroudObjConversion)]
    // public int ConversionSurroudObjAttackPower;
    // [LabelText("环绕物 攻击力百分比")]
    // [ShowIf("EffectType", EffectType.SurroudObjConversion)]
    // public float ConversionSurroudObjAttackPowerPercentage;


    [LabelText("路径点效果")]
    [ShowIf("EffectType", EffectType.FootPath)]
    public DmgBuffOnTouch buffOnTouch;
    [LabelText(" 攻击力")]
    [ShowIf("EffectType", EffectType.FootPath)]
    public int FootPathAttackPower;
    [LabelText(" 攻击力百分比")]
    [ShowIf("EffectType", EffectType.FootPath)]
    public float FootPathAttackPowerPercentage;
    
    [LabelText("增加金币数量")]
    [ShowIf("EffectType", EffectType.GoldEffect)]
    public int GoldValue;
    
    [LabelText("增加金币倍率")]
    [ShowIf("EffectType", EffectType.AddGold)]
    public float GoldRatio;
    

    [LabelText("生成物")]
    [ShowIf("EffectType", EffectType.Spawn)]
    public GameObject SpawnObj;
    /*[LabelText("伤害数值")]
    [ShowIf("EffectType", EffectType.Spawn)]
    public int SpwanObjDamageValue;*///没做好全部在item上配之前不放在这
    [LabelText("生成位置")]
    [ShowIf("EffectType", EffectType.Spawn)]
    public SpwanPosType SpwanPosType;
    [ShowIf("EffectType", EffectType.Spawn)]
    public Vector3 Offset;
    [ShowIf("EffectType", EffectType.Spawn)]
    public Vector3 Direction;
    [ShowIf("EffectType", EffectType.Spawn)]
    public bool IsFollowRole;
    [ShowIf("EffectType", EffectType.Spawn)]
    public bool EnemyFollow;
    
    [LabelText("变量")]
    [ShowIf("EffectType", EffectType.AnimatorTrigger)]
    public string AnimatorParaName;
    [LabelText("时间,为0则是设置trigger")]
    [ShowIf("EffectType", EffectType.AnimatorTrigger)]
    public float AnimatorParaTime;

    [LabelText("技能冷却缩放")]
    [ShowIf("EffectType", EffectType.SkillCoolingScale)]
    public float SkillCoolingScale;


    // [LabelText("凤凰剑气")]
    // [ShowIf("EffectType", EffectType.Phoenix)]
    // public GameObject PhoenixPrefab;
    // [LabelText("剑气特效")]
    // [ShowIf("EffectType", EffectType.Phoenix)]
    // public GameObject PhoenixEmmitVFX;
    // [LabelText("剑气攻击力")]
    // [ShowIf("EffectType", EffectType.Phoenix)]
    // public int PhoenixAttackPower;
    // [LabelText("剑气攻击力百分比")]
    // [ShowIf("EffectType", EffectType.Phoenix)]
    // public float PhoenixAttackPowerPercentage;

    [ShowIf("EffectType", EffectType.EmmitThunder)]
    public GameObject ThunderPrefab;
    [LabelText("最多雷电数")]
    [ShowIf("EffectType", EffectType.EmmitThunder)]
    public int MaxThunderCount;
    [LabelText("最少雷电数")]
    [ShowIf("EffectType", EffectType.EmmitThunder)]
    public int MinThunderCount;
    [LabelText("攻击力")]
    [ShowIf("EffectType", EffectType.EmmitThunder)]
    public int ThunderAttackPower;
    [LabelText("攻击力百分比")]
    [ShowIf("EffectType", EffectType.EmmitThunder)]
    public float ThunderAttackPowerPercentage;

    [LabelText("复活剩余生命")]
    [ShowIf("EffectType", EffectType.Revive)]
    public float ReviveHp;
    [ShowIf("EffectType", EffectType.ModifyLuck)]
    public float ModifyLuck;
    // [ShowIf("EffectType", EffectType.ModifyDifficult)]
    // public float ModifyDifficult;
    // [ShowIf("EffectType", EffectType.Lottery)]
    // public List<int> AwardGolds;
    [ShowIf("EffectType", EffectType.AttackDistance)]
    public float AttackDistance;
    [ShowIf("EffectType", EffectType.MaxPower)]
    public int MaxPower;
    // [ShowIf("EffectType", EffectType.AttackPowerRecovery)]
    // public int AttackPowerRecovery;
    [ShowIf("EffectType", EffectType.PowerRecovery)]
    public int PowerRecovery;
    
    
    [ShowIf("EffectType", EffectType.Bomb)] [InlineEditor()]
    public DmgBuffOnTouch BombExplosion;
    [ShowIf("EffectType", EffectType.Bomb)] 
    public int BombCount;
    [ShowIf("EffectType", EffectType.Bomb)] 
    public ThrowProjectile BombThrowProjectile;
    [ShowIf("EffectType", EffectType.Bomb)]
    public float BombMoveTime;
    [ShowIf("EffectType", EffectType.Bomb)]
    public float BombHeight;
    [ShowIf("EffectType", EffectType.Bomb)]
    public float BombPosOffset;
    [ShowIf("EffectType", EffectType.Bomb)]
    public SpwanPosType BombSpwanPosType;
     [ShowIf("EffectType", EffectType.Bomb)]
    public AudioClip ThrowBombAudio;



    [ShowIf("EffectType", EffectType.TrackingArrow)]
    public AudioClip ArrowAudio;
    [ShowIf("EffectType", EffectType.TrackingArrow)]
    public GameObject Arrow;

    
    [ShowIf("EffectType", EffectType.KatanaOnlySkill)]
    public FeedBackObject PrepareFeedback;
    [ShowIf("EffectType", EffectType.KatanaOnlySkill)]
    public FeedBackObject SaHitFeedback;
    [ShowIf("EffectType", EffectType.KatanaOnlySkill)]
    public Material SaTrailMat;
    [ShowIf("EffectType", EffectType.KatanaOnlySkill)]
    public ParticleSystem EndParticles;
    [ShowIf("EffectType", EffectType.KatanaOnlySkill)]
    public ParticleSystem EndBloodBurst;
    [ShowIf("EffectType", EffectType.KatanaOnlySkill)]
    public AudioClip EndSFX;
    [ShowIf("EffectType", EffectType.KatanaOnlySkill)]
    public AudioClip EndBloodSFX;
    
    [ShowIf("EffectType", EffectType.PlayerAttackType)]
    public int PlayerAttackType;
    [ShowIf("EffectType", EffectType.PlayerAccumulateAttackType)]
    public int PlayerAccumulateAttackType;
    [ShowIf("EffectType", EffectType.PlayerRollType)]
    public int PlayerRollType;
    [ShowIf("EffectType", EffectType.PowerShieldAgainst)]
    public DmgBuffOnTouch PowerShieldAgainstAOE;
    [ShowIf("EffectType", EffectType.PowerShieldAgainst)]
    public Vector3 PowerShieldAgainstAOEOffset;
    

    [ShowIf("EffectType", EffectType.ShieldAgainst)]
    public FeedBackObject ShieldAgainstFeedBack;
    [ShowIf("EffectType", EffectType.ShieldAgainst)]
    [LabelText("可弹回投射物的距离")]
    public float ShieldDistance;
    
    [ShowIf("EffectType", EffectType.SmallShieldAgainst)]
    [LabelText("弹反伤害比例")]
    public float SmallShieldAgainstValue;
    
    [ShowIf("EffectType", EffectType.ShieldAttributes)]
    public float ShieldAttributesAng;
    [ShowIf("EffectType", EffectType.ShieldAttributes)]
    public float ShieldAttributesMoveSpeed;
    [ShowIf("EffectType", EffectType.ShieldAttributes)]
    public Mesh ShieldMesh1;
    [ShowIf("EffectType", EffectType.ShieldAttributes)]
    public Mesh ShieldMesh2;
    
    
    [ShowIf("EffectType", EffectType.AccumulateTime)]
    public float AccumulateTime;
    
    
    [ShowIf("EffectType", EffectType.ShieldSkill_Against)]
    public int ShieldSkill_Against_Times;
    [ShowIf("EffectType", EffectType.ShieldSkill_Against)]
    public float ShieldSkill_Against_Interval;

    [ShowIf("EffectType", EffectType.FullHpEffect)]
    public Surround_Obj surroundObj_FullHp;
    [ShowIf("EffectType", EffectType.FullHpEffect)]
    public GameObject spawn_FullHp;
    [ShowIf("EffectType", EffectType.FullHpEffect)]
    public float spawnCd;
    [ShowIf("EffectType", EffectType.FullHpEffect)]
    public Vector3 offset_FullHp;
    [ShowIf("EffectType", EffectType.FullHpEffect)]
    public Vector3 direction_FullHp;
    [ShowIf("EffectType", EffectType.FullHpEffect)]
    public bool isFollow_FullHp;


    [ShowIf("EffectType", EffectType.CrownEffect)]
    [LabelText("皇冠最大上限")]
    public int maxCrown;
    [ShowIf("EffectType", EffectType.CrownEffect)]
    [LabelText("每点皇冠增加的攻击力%"),Range(0,1)]
    public float perCrownAttack;
    [ShowIf("EffectType", EffectType.CrownEffect)]
    [LabelText("头顶物品")]
    public GameObject crown;
    [ShowIf("EffectType", EffectType.CrownEffect)]
    [LabelText("第一个皇冠位置")]
    public Vector3 startPoint;
    [ShowIf("EffectType", EffectType.CrownEffect)]
    [LabelText("每个皇冠偏移位置")]
    public float offsetY;
    [ShowIf("EffectType", EffectType.CrownEffect)]
    [LabelText("每次受伤掉几个皇冠")]
    public int perDamageReduce;
}

