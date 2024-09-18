using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "GameItem/ItemObject")]
public class ItemScriptableObject : ScriptableObject
{
    [LabelText("默认解锁")][GUIColor(1,1,0)][BoxGroup("解锁")]
    public bool DefaultUnLock = true;

    [HideIf("DefaultUnLock")][GUIColor(1,1,0)][BoxGroup("解锁")]
    public int ChapterId;
    [HideIf("DefaultUnLock")][GUIColor(1,1,0)][BoxGroup("解锁")]
    public int LevelId;
    [HideIf("DefaultUnLock")][GUIColor(1,1,0)][BoxGroup("解锁")]
    public int RoomId;
    
    // [Header("物品信息")]
    [HorizontalGroup("info",85)]
    [HideLabel]
    [PreviewField(85)]
    public Sprite Icon;
    
    [VerticalGroup("info/desc")]
    [LabelText("名称")]
    [TextArea(1,1)]
    public string Name;
    
    [VerticalGroup("info/desc")]
    [LabelText("描述")]
    [TextArea]
    public string Describe;

    [LabelText("物品类型")]
    public ItemType ItemType;

    
    [ShowIf("ItemType",ItemType.Artifact)]
    [LabelText("神器类型")]
    public ArtifactType ArtifactType;

    

    [HideIf("ItemType", ItemType.Other)]
    [HideIf("ItemType", ItemType.Artifact)]
    [HideIf("ItemType", ItemType.CharacterSkill)]    
    [HideIf("ItemType", ItemType.SoulStone)]
    [LabelText("可以使用的次数 -1为无限")]
    public int CanUseTimes;
    [HideIf("ItemType", ItemType.Other)]
    [HideIf("ItemType", ItemType.Artifact)]
    [HideIf("ItemType", ItemType.CharacterSkill)]
    [HideIf("ItemType", ItemType.SoulStone)]
    [LabelText("主动物品触发方式")]
    public ItemUseType ItemUseType = ItemUseType.Click;

    [HideIf("ItemType", ItemType.Other)]
    [HideIf("ItemType", ItemType.Artifact)]
    [HideIf("ItemType", ItemType.CharacterSkill)]
    [HideIf("ItemType", ItemType.SoulStone)]
    [LabelText("冷却时间")]
    public float CoolingTime;
    
    
    [FormerlySerializedAs("QualityType")]
    [Space]
    [Space]
    [LabelText("颜色")]
    public ItemColor itemColorType;
    [LabelText("唯一")]
    public bool isUnique;

    [Space]
    [Space]
    [ShowIf("ItemType", ItemType.Artifact)]
    
    [HorizontalGroup("FxObj",85)]
    [Title("视效Object")]
    [PreviewField(85)]
    [HideLabel]
    [ShowIf("ItemType", ItemType.Artifact)]
    public GameObject VisualObj;
    
    [Space] [Space] [Space] [Space] [Space] [Space] [Space] [Space]
    [VerticalGroup("FxObj/Type")]
    [ShowIf("ItemType", ItemType.Artifact)]
    [LabelText("视效位置插槽")]
    public VisualObjSlotType VisualObjSlot;

    
    [Space]
    [Space]
    [LabelText("效果List")]
    [InlineEditor()]
    public List<ItemEffectObj> effects = new List<ItemEffectObj>();

    [Space]
    [Space]
    [ShowIf("ItemType", ItemType.Artifact)]
    [LabelText("升级Item")]
    ItemScriptableObject UpgradeItem;
}

public enum WeaponType
{
    Sword,
    Shield,
    Staff
}
public enum VisualObjSlotType
{
    Head,
    Body,
    Hand,
    Weapon,
    Halo
}
public enum ArtifactType
{
    None=0,
    Armor_Unique=1,//唯一
    Attack_Unique=2,//唯一
    Kill_Unique=3,//唯一
    FootPrint_Unique=4,//唯一
    Halo_Unique=5,//唯一
    Prop_Unique=6,//唯一
    Evade_Unique=7,
    Attribute_Unique=8,
    Summon_Unique=9,
    Special_Unique=10,
    Orb=11
}
public enum ItemColor
{
    White =0,
    Red = 1,
    Orange = 2,
    Yellow =3,
    Green =4,
    Blue=5,
    Cyan=6,
    Purple=7
}


public enum TriggerEventType
{
    [LabelText("主动")]
    Active = 1,//主动
    [LabelText("拾取")]
    Get = 2,//拾取
    [LabelText("攻击")]
    Attack = 3,//攻击
    [LabelText("受伤")]
    Injured = 4,//受伤
    [LabelText("击杀敌人")]
    KillEnemy = 5,//击杀敌人
    [LabelText("时间")]
    Time = 6,//时间
    [LabelText("击中敌人")]
    HitEnmey = 7,
    [LabelText("角色移动")]
    RoleMove = 8,
    [LabelText("角色翻滚")]
    RoleRoll = 9,
    [LabelText("进入新房间")]
    NewRoom = 10,//新房间
    [LabelText("闪避")]
    Dodge = 11,//
    [LabelText("进入战斗房间")]
    EnterFightRoom = 23,
    [LabelText("举盾，放盾")]
    PlayerShieldHoldingTrigger = 24,
    [LabelText("暴击")]
    PlayerAttackCrit = 25,
    [LabelText("闪电链击中角色")]
    LightningChainHitRole = 26,
    [LabelText("角色死亡时身上有Buff")]
    RoleDeadOfBuff = 27,
    [LabelText("角色死亡时")]
    RoleDeadTrigger = 28,
    [LabelText("法术-DmgBuffOnTouchHitEnemyTrigger")]
    DmgBuffOnTouchHitEnemyTrigger = 29,
    [LabelText("玩家复活")]
    ReviveTrigger = 30,
    [LabelText("键盘测试")]
    Keyboard = 31,
    [LabelText("BattleObjectDestroy")]
    BattleObjectDestroy = 32,
    [LabelText("翻滚刀")]
    RollAttack = 33,
    [LabelText("当创建Enemy时")]
    OnCreateEnemy = 34,
    [LabelText("悬赏怪死亡时")]
    KillRewardEnemyDead = 35,
    [LabelText("动画事件Trigger")]
    AnimatorEventTrigger = 36,
    [LabelText("翻滚刀击中敌人")]
    RollHitEnemy=37,
}

public enum EffectType
{
    None = 0,
    //属性
    [LabelText("攻击力")]
    ChangeAttackPower = 1,//攻击力
    [LabelText("攻速")]
    ChangeAttackSpeed = 2,//攻速
    // [LabelText("溅射")]
    // SplashDamage = 3,//溅射
    [LabelText("暴击概率")]
    ChangeCriticalProbability = 4,//暴击概率
    [LabelText("暴击倍率")]
    ChangeCriticalMultiplier = 5,//暴击倍率
    [LabelText("受伤倍数")]
    ChangeInjuryMultiplier = 6,//受伤倍数
    [LabelText("血上限")]
    ChangeMaxHp = 7,//血上限
    [LabelText("当前血")]
    ChangeCurrentHp = 8,//改血
    [LabelText("治疗倍率")]
    ChangeHpTreatMultiplier=25,
    [LabelText("移速")]
    ChangeMoveSpeed = 9,//移速
    [LabelText("降低所有敌人移速")]
    ChangeEnemySpeed=10,
    [LabelText("闪电链")]
    LightningChain = 11,
    // [LabelText("AOE 攻击")]
    // AOEAttack = 12,
    [LabelText("Buff")]
    Buff = 13,
    [LabelText("环绕物")]
    Surroud = 14,
    [LabelText("环绕物 速度")]
    SurroudSpeed = 15,
    // [LabelText("环绕物 分裂")]
    // SurroudSplit = 16,
    // [LabelText("环绕物 转换")]
    // SurroudObjConversion = 17,
    
    [LabelText("路径")]
    FootPath = 18,

    [LabelText("创建召唤物")]
    Spawn = 19,
    
    [LabelText("金币-局内游戏币")]
    GoldEffect = 20,
    [LabelText("触发动画")]
    AnimatorTrigger = 21,
    [LabelText("技能冷却缩放")]
    SkillCoolingScale = 22,
    // [LabelText("凤凰剑气")]
    // Phoenix = 23,
    [LabelText("雷电攻击")]
    EmmitThunder = 24,
    [LabelText("复活")]
    Revive = 26,
     [LabelText("增加幸运")]
    ModifyLuck = 27,
    // [LabelText("降低难度")]
    // ModifyDifficult = 28,
    // [LabelText("开启自动攻击")]
    // AutoAttack = 29,
    // [LabelText("彩票")]
    // Lottery=30,
    [LabelText("攻击距离")]
    AttackDistance = 31,
    [LabelText("最大体力")]
    MaxPower = 32,
    // [LabelText("攻击体力恢复速度")]
    // AttackPowerRecovery = 33,
    [LabelText("体力恢复速度")]
    PowerRecovery = 34,
    [LabelText("炸弹")]
    Bomb = 35,
    [LabelText("追踪箭")]
    TrackingArrow = 36,
    [LabelText("闪电箭")]
    ArrowLightningChain = 37,
    [LabelText("箭AOE")]
    ArrowAOE = 38,
    [LabelText("Katana/YiShan")]
    KatanaOnlySkill = 39,
    [LabelText("Player/普通攻击类型")]
    PlayerAttackType = 40,
    [LabelText("Player/蓄力攻击类型")]
    PlayerAccumulateAttackType = 41,
    [LabelText("Player/翻滚类型")]
    PlayerRollType = 42,
    [LabelText("盾/盾反弹子弹")]
    ShieldAgainstBullet = 43,
    [LabelText("盾/能量盾反AOE")]
    PowerShieldAgainst = 44,
    [LabelText("盾/盾反")]
    ShieldAgainst = 45,
    [LabelText("盾/小盾反")]
    SmallShieldAgainst = 46,
    [LabelText("盾/盾 角度 移速")]
    ShieldAttributes = 47,
    [LabelText("Player/蓄力时间")]
    AccumulateTime = 48,
    [LabelText("盾/盾 反击技能")]
    ShieldSkill_Against = 49,
    [LabelText("盾/盾冲 最后有攻击")]
    ShieldRollLastAttack = 50,
    [LabelText("盾/盾冲 无前摇")]
    ShieldDash_SkipPreAction = 52,
    [LabelText("盾/盾冲 最后攻击")]
    ShieldDash_AfterAttack = 53,
    [LabelText("盾/盾冲 无敌")]
    ShieldDash_God = 54,
    
    [LabelText("荆棘之甲")]
    ThornsArmor = 55,
    [LabelText("背刺")]
    Backstab = 56,
    [LabelText("捡钱同时回血")]
    GetMoneyAddHp = 57,
    [LabelText("减少物品冷却时间 一次性")]
    LessItemCooling = 58,
    [LabelText("眩晕")]
    Dizziness = 59,
    [LabelText("物品数量Add属性")]
    ItemCountAddAttribute = 61,
    [LabelText("Money数量Add属性")]
    MoneyAddAttribute = 62,
    [LabelText("血比例Add属性")]
    HpProportionAttribute = 63,
    [LabelText("攻击力百分比吸血")]
    AttackPowerProportionHp = 64,
    [LabelText("不会被打断")]
    NotAcceptInterruption = 65,
    [LabelText("疯狂属性")]
    CrazyAttribute = 66,
    [LabelText("积累物体")]
    AccumulationObject = 67,
    [LabelText("治疗加钱")]
    TreatmentGetMoney = 68,
    [LabelText("怪物出生损失血量")]
    ReduceMonsterHP = 69,
    [LabelText("属性骰子")]
    DiceAttribute = 70,
    [LabelText("目标满血必暴击")]
    TargetMaxHpCritical = 71,
    [LabelText("金币增加比例")]
    GoldMagnification = 72,
    [LabelText("DmgBuffOnTouch暴击")]
    DmgAoeCrit = 73,
    [LabelText("触发攻击")]
    TriggerHitEnemy = 74,
    [LabelText("设置不能举盾")]
    SetCantHoldingShield = 75,
    [LabelText("音效")]
    Audio = 76,
    [LabelText("盾技能")]
    ShieldSkill = 77,
    [LabelText("法术强度")]
    MagicPower = 78,
    // [LabelText("FeverEffect")]
    // FeverEffect = 79,
    [LabelText("katana戳 斩杀技")]
    NewKatanaSkill = 80,
    [LabelText("双重攻击")]
    DoubleAttack = 81,
    [LabelText("Tag")]
    TagEffect = 82,
    [LabelText("散射")]
    Scattering = 83,
    [LabelText("召唤物闪电链")]
    BattleObjectLightning = 84,
    [LabelText("刀翼")]
    KnifeWing = 85,
    [LabelText("添加道具")]
    AddItem = 86,
    [LabelText("添加骰子")]
    AddDice = 87,
    [LabelText("添加金币")]
    AddGold = 88,
    [LabelText("显示敌人生命")]
    ShowEnemyHpText = 89,
    [LabelText("属性加成")]
    AttributeEffect = 90,
    [LabelText("满血效果")]
    FullHpEffect=91,
    [LabelText("赏金标记")]
    KillRewardTagEffect=92,
    [LabelText("皇冠")]
    CrownEffect=93,
    [LabelText("影分身")]
    RoleShadowEffect=94,
    [LabelText("金铲子")]
    GoldManuscript=95,
    
}
