

public partial class AnimatorEventName
{
    //攻击状态标记 用
    public const string StartAttack_ = "StartAttack_";
    public const string EndAttack_ = "EndAttack_";
    public const string ReAttack = "ReAttack";
    public const string ReAttackA = "ReAttackA";
    public const string ReAttackB = "ReAttackB";
    
    public const string RollStart = "RollStart";
    public const string RollEnd = "RollEnd";
    
    public const string RollAttackStart = "RollAttackStart";
    public const string RollAttackEnd = "RollAttackEnd";
    
    public const string RollNormalAttackStart = "RollNormalAttackStart";
    public const string RollNormalAttackEnd = "RollNormalAttackEnd";
    
    public const string FeverWave = "FeverWave";

    public const string BigSkillStart = "BigSkillStart";
    public const string BigSkillEnd = "BigSkillEnd";
    
    //伤害判定 用
    public const string DmgStart_ = "DmgStart_";
    public const string DmgEnd_ = "DmgEnd_";
    //动作移动 用
    public const string MoveAction_ = "MoveAction_";
    // public const string EndInput = "EndInput";
    // public const string StartInput = "StartInput";
    
    public const string CanMoveSkillStart = "CanMoveSkillStart";
    public const string CanMoveSkillEnd = "CanMoveSkillEnd";
    public const string CantMoveSkillStart = "CantMoveSkillStart";
    public const string CantMoveSkillEnd = "CantMoveSkillEnd";
    
    public const string AcceptInterruptionStart = "AcceptInterruptionStart";
    public const string AcceptInterruptionEnd = "AcceptInterruptionEnd";
    public const string StopEmmitParticles_ = "StopEmmitParticles_";

    public const string OnlyRollStart = "OnlyRoll_Start";
    public const string OnlyRollEnd = "OnlyRoll_End";
    
    // //Enemy攻击警告 _value = 秒 float
    // public const string EnemyShowWarning_ = "EnemyShowWarning_";
    // //Enemy攻击范围警告 
    // public const string EnemyShowAttackWarning_ = "EnemyShowAttackWarning_";

    //攻击提示器
    public const string ShowRectIndicate_ = "ShowRectIndicate_";//参数length_width_time
    public const string ShowSectorIndicate_ = "ShowSectorIndicate_";//参数radius_angle_time
    public const string ShowDamageAreaIndicate_ = "ShowDamageAreaIndicate_";//参数radius_time

    //攻击FeedBack
    public const string ShowFeedBack = "ShowFeedBack";

    //动画时间缩放
    public const string AnimTimeScaleStart_ = "AnimTimeScaleStart_";
    public const string AnimTimeScaleEnd = "AnimTimeScaleEnd";
    //无敌
    public const string StartGod = "StartGod";
    public const string EndGod = "EndGod";
    
    public const string Foot = "Foot";

    //展示连招的字体
    public const string ShowComboText_ = "ShowComboText_";

    //面朝目标
    public const string ChangeRotSpeed_ = "ChangeRotSpeed_";
    public const string ResetRotSpeed = "ResetRotSpeed";
    public const string LookAtTargetStart = "LookAtTargetStart";
    public const string LookAtTargetEnd = "LookAtTargetEnd";

    //播放粒子
    public const string StartParticles_ = "StartParticles_";
    public const string CreateParticles_ = "CreateParticles_";
    public const string StopParticles_ = "StopParticles_";
    public const string StartFeedbacks_ = "StartFeedbacks_";
    public const string EmmitPhantom = "EmmitPhantom";
    public const string StartPhantom = "StartPhantom";
    public const string StopPhantom = "StopPhantom";


    public const string StartSFX_ = "StartSFX_";
    
    
    //盾反开始
    public const string ShieldAgainstStart = "ShieldAgainstStart";
    public const string ShieldAgainstEnd = "ShieldAgainstEnd";
    
}

public partial class EventName
{
    public const string OnAppStart = "OnAppStart"; //value = null
    public const string OnUpdateLanguage = "OnLanguage"; //value = null

    public const string OnChapterBattleStart = "OnChapterBattleStart";
    public const string OnEndlessBattleStart = "OnEndlessBattleStart";
    public const string OnArchiveBattleStart = "OnArchiveBattleStart";
    public const string OnBattleOver = "OnBattleOver";//gameOverData
    
    public const string NeedPower = "PowerBarShake"; //value = null
    public const string RollCool = "RollCool"; //value = null
    
    public const string GodStateChange = "GodStateChange"; //value = string TemporaryId
    public const string GodStart = "GodStart";
    public const string GodStop = "GodStop";
    public const string RollGodStateChange = "RollGodStateChange"; //value = string TemporaryId
    
    
    public const string ItemCoolingLess = "ItemCoolingLess"; //value = float time
    
    public const string ItemTrigger = "ItemTrigger"; //value = Item临时ID
    public const string ItemEffectTrigger = "ItemEffectTrigger"; //Effect Type
    
    
    public const string MainScene_HeroLevelUpgrade = "MainScene_HeroLevelUpgrade"; //UnlockHeroLevelData


    public const string OnDmgBuffOnTouchHitRole = "OnDmgBuffOnTouchHitRole";//value = DamageInfo 
    
    public const string OnUnlockItemEvent = "OnUnlockItemEvent";//value = itemId

    //
    public const string OnRoleAttack = "OnRoleAttack";//value = roleId
    public const string OnRoleInjured = "OnRoleInjured";//value = RoleInjuredInfo
    public const string OnRolePowerChange = "OnRolePowerChange";//value = TemporaryId;
    public const string OnRoleSkillPowerChange = "OnRoleSkillPowerChange";//value = TemporaryId;
    public const string OnRoleGodInjured = "OnRoleGodInjured";//value = RoleInjuredInfo
    public const string OnRoleDodgeInjured = "OnRoleDodgeInjured";//value = RoleInjuredInfo
    public const string OnRoleTreatment = "OnRoleTreatment";//value = TreatmentData
    public const string OnRoleDead = "OnRoleDead";//value = RoleDeadEventData
    public const string OnRoleRoll = "OnRoleRoll";//value = null
    public const string OnBreakableObjBreake = "OnBreakableObjBreake";//value = ObjPosition
    //Player
    public const string OnPlayerAttackHitEnemy = "OnRoleAttackHitEnemy";//value = DamageInfo 攻击每命中一个敌人就会发送事件
    public const string OnPlayerAttackCrit = "OnPlayerAttackCrit";//暴击 value = DamageInfo
    
    //盾反成功
    public const string OnShieldAgainst = "OnShieldAgainst";
    
    //role 添加、移除 物品
    public const string OnRoleAddItem = "OnRoleAddItem";//value = RoleItemEventData;
    public const string OnRoleReMoveItem = "OnRoleReMoveItem";//value = RoleItemEventData;
    
    public const string OnReplaceItem = "OnReplaceItem";
    
    public const string OnAddArtifactItemCount = "OnAddArtifactItemCount";
    public const string OnAddButtonSkillItemCount = "OnAddButtonSkillItemCount";
    public const string OnAddButtonItemCount = "OnAddButtonItemCount";
    
    //role 添加、移除 buff   
    public const string OnRoleAddBuff = "OnRoleAddBuff";//value = RoleBuffEventData;
    public const string OnRoleRemoveBuff = "OnRoleRemoveBuff";//value = RoleBuffEventData;

    public const string OnRoleGetWeapon = "OnRoleGetWeapon";//Value = DropItem
    
    //盾角色专属事件
    public const string OnShieldDefense = "OnShieldDefense";//Value = DamageInfo
    // public const string ShieldHolding = "ShieldHolding";//Value = bool
    
    //Katana
    public const string KatanaAccumulateEnd = "KatanaAccumulateEnd";//
    public const string KatanaAccumulateAttack = "KatanaAccumulateAttack";
    //摇杆
    // public const string JoyTouch = "JoyTouch";//value = bool: Down = true,Up = false
    // public const string JoyValue = "JoyValue";//value = Vector2;
    // public const string JoySlide = "JoySlide";//value = Vector2;
    // public const string JoyClick = "JoyClick";
    // public const string JoyLongPress = "JoyLongPress";//没有
    public const string JoyStatus = "JoyStatus";//JoyStatusData
    
    //事件主动抬起摇杆 - 程序命令joy抬起
    public const string JoyUp = "JoyUp";//没有
    
    public const string OnRoleRegistered = "OnRoleRegistered";// value = RoleController
    //InteractObj
    public const string OnInteractStart = "OnInteractStart";//value = InteractObj
    public const string OnInteractEnd = "OnInteractEnd";//value = InteractObj
    public const string OnInteractObjSelectStart = "OnInteractObjSelectStart";//value = InteractObj
    public const string OnInteractObjSelectEnd = "OnInteractObjSelectEnd";//value = InteractObj
    //Item
    public const string OnAddActiveItem = "OnAddActiveItem";//value = Item
    public const string OnItemDataUpdate = "OnItemDataUpdate";//value = Item
    public const string OnUseItem = "OnUseItem";//value = Item
    public const string OnLightningChainHitRole = "OnLightningChainHitRole";//value = RoleId  string
    //    //buff叠加
    //    public const string OnBuffOverlay = "OnBuffOverlay";//value = BuffOverlayData
    
    //NPC对话结束
    public const string OnNpcTalkStart = "OnNpcTalkStart";//value = NPCId
    public const string OnNpcTalkOver = "OnNpcTalkOver";//value = NPCId
    public const string OnSelectNPC = "OnSelectNPC";//value = GameNpcBase
    
    //地图机制事件
    public const string ClearAllEnemy = "ClearAllEnemy";
    public const string OpenRoomDoor = "OpenRoomDoor";
    public const string EnterEventRoom = "EnterEventRoom";//RoomData
    public const string EnterNextRoom = "EnterNextRoom";//RoomData
    public const string ExitRoom = "ExitRoom";
    public const string EnterGameOverDoor = "EnterGameOverDoor";//进入游戏最后一个门了
    public const string EnterLastDoor = "EnterLastDoor";//进入本层最后一个门了
    public const string EnterDoor = "EnterDoor";//进入门了 EnterDoorData
    public const string UI_EnterNextRoom = "UI_EnterNextRoom";//RoomData  UITips专用
    public const string EnterFightRoom = "EnterFightRoom";//RoomData
    public const string EnterNextLevel = "EnterNextLevel";//
    public const string CollectRoomProps="CollectRoomProps";
    public const string OnOpenDropItem="OnOpenDropItem";//当拾起宝箱。

    //ZY's 开门
    public const string OpenDoor = "OpenDoor";
    //ZY's 获取金钱
    public const string OnAddMoney = "OnAddMoney";//value=int(money)

    public const string OnNpcTalkYesOrNoEvent = "OnNpcTalkYesOrNoEvent";//value = NpcTalkYesOrNoEventData
    

    //经验
    public const string OnGetExp = "OnGetExperience";//Value = (bool)IsLevelUp
    public const string UpdateExp = "UpdateExperience";

    //奇遇值满了
    public const string FortuneFull="FortuneFull";
    public const string FortuneClear = "FortuneClear";
    
    public const string PlayerRevive = "PlayerRevive";
    
    
    
    public const string SkillGuide = "SkillGuide";
    
    
    public static string HideCurrentRoom  = "HideCurrentRoom";
    public static string ShowNextRoom  = "ShowNextRoom";
    public static string OnEnterNextRoom  = "OnEnterNextRoom";
    
    
    public static string EndlessWaveTips  = "EndlessWaveTips";//EndlessWaveType
    public static string EndlessGameOver  = "EndlessGameOver";//int = wave index

    public static string SetSound="SetSound";
    public static string SetBmg = "SetBmg";
    public static string LoadChapter = "LoadChapter";//CPID
    public static string SelectHero = "SelectHero";//Hero Id
    
    
    public static string GuideDialog = "GuideDialog";//BattleGuideSequenceData
    public static string OnMoveCamera = "OnMoveCamera";//bool ,true开始，false结束
    public static string HideJoy = "HideJoy";//bool
    
    public static string ShowBattlePanelItem = "ShowBattlePanelItem";//item class
    
    public static string ShowSkillBtn = "ShowSkillBtn";
    public static string FightRoomWaveTips  = "FightRoomWaveTips";//战斗房间波数 string
    public static string FightRoomResetEnemyWave  = "FightRoomResetEnemyWave";
    
    
    public static string OnBuyHero  = "OnBuyHero";
    public static string HeroUpgradede  = "HeroUpgradede";
    public static string HeroUpgradedeColorLevel  = "HeroUpgradedeColorLevel";

    public static string OnSkillOver = "OnSkillOver";
    
    public static string OnEnvironmentBack = "OnEnvironmentBack";

    public static string OnBattleObjectDestroy = "OnBattleObjectDestroy";//Vector3 Position
    
    
    public static string OnRoleTagChange = "OnRoleTagChange";//

    public static string OnBossDead = "OnBossDead";
}


public partial class SDKEventName
{
    public const string OpenChapterPanel = "OpenChapterPanel";
}


public struct RoleItemEventData
{
    public Item Item;
    public string RoleId;
}

public struct RoleBuffEventData
{
    public int BuffId;
    public string BuffTemporaryId;
    public string RoleId;
}








