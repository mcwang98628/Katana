
public class TGANames
{
    //事件名字
    public const string AppStart = "AppStart";
    public const string GameStart = "GameStart";
    public const string GameOver = "GameOver";
    public const string EnterRoom = "EnterRoom";
    public const string PlayerInjured = "PlayerInjured";
    public const string PlayerDodgeInjured = "PlayerDodgeInjured";
    
    public const string MainPanelOpenChapterPanel = "MainPanelOpenChapterPanel";
    public const string MainPanelPeekChapterItem = "MainPanelPeekChapterItem";
    public const string MainPanelPeekChapterInfo = "MainPanelPeekChapterInfo";
    public const string MainPanelGetChapterInfoSoul = "MainPanelGetChapterInfoSoul";
    public const string MainPanelBuyRune = "MainPanelBuyRune";
    public const string MainPanelBuyHero = "MainPanelBuyHero";
    public const string HeroLevelUpgrade = "HeroLevelUpgrade";
    public const string MainPanelSelectHero = "MainPanelSelectHero";
    public const string MainPanelPeekHeroSkillInfo = "MainPanelPeekHeroSkillInfo";
    public const string MainPanelOpenArtifactPanel = "MainPanelOpenArtifactPanel";
    public const string MainPanelOpenMonsterArtifactPanel = "MainPanelOpenMonsterArtifactPanel";
    public const string MainPanelOpenItemArtifactPanel = "MainPanelOpenItemArtifactPanel";
    public const string MainPanelOpenItemBuildArtifactPanel = "MainPanelOpenItemBuildArtifactPanel";
    public const string MainPanelShop = "MainPanelShop";
    public const string EquipmentPanel = "EquipmentPanel";
    
    public const string WearEquipment = "WearEquipment";
    public const string UnloadEquipment = "UnloadEquipment";
    public const string BuyEquipment = "BuyEquipment";//线上版本埋点名字错了，叫UnloadEquipment
    
    public const string BattleTreasureChestItem = "BattleTreasureChestItem";//从宝箱获取道具
    public const string BattleTreasureChestSkip = "BattleTreasureChestSkip";//宝箱 跳过
    public const string BattleShopBuyItem = "BattleShopBuyItem";//商店买道具 value itemId
    
    
    //参数名字
    public const string CurrentLanguage = "CurrentLanguage";//语言
    public const string AppStartTimes = "AppStartTimes"; //App启动次数
    public const string StartGameTimes = "StartGameTimes"; //开始战斗次数
    public const string TodayStartGameTimes = "TodayStartGameTimes"; //今天开始战斗次数
    public const string IsHaveBattleArchive = "IsHaveBattleArchive";//是否有战斗存档
    public const string IsArchiveContinueBattle = "IsArchiveContinueBattle";//是否是从存档读取战斗 继续上一局游戏
    public const string IsVictory = "IsVictory";//胜利
    public const string CurrentHp = "CurrentHp";//血
    public const string Gold = "Gold";//钱
    public const string KillEnemyNumber = "KillEnemyNumber";//小怪数量
    public const string KillEliteEnemyNumber = "KillEliteEnemyNumber";//精英怪数量
    public const string BattleProgress = "BattleProgress";//战斗进度    
    public const string PassingRoomTypes = "PassingRoomTypes";//经过的房间 
    public const string BattleTime = "BattleTime";//局内游戏时长
    public const string EnemyId = "EnemyId";//EnemyId
    public const string GameTime = "GameTime";//总时长
    
    public const string GlobalGold = "GlobalGold";//外围游戏币 钻石。。因为字段数量限制 尽量避免新名字
    public const string GlobalSoul = "GlobalSoul";//外围魂
    public const string GlobalFire = "GlobalFire";//外围火种
    public const string GlobalLevel = "GlobalLevel";//外围等级
    
    
    
    public const string HeroSchoolId = "HeroSchoolId";//角色流派ID 废弃。。。
    
    public const string BattleGuid = "BattleGuid";//战斗的唯一ID，用于标记是否是同一场战斗
    
    public const string IsChapterMode = "IsChapterMode";//
    
    public const string HeroId = "HeroId";//角色ID
    public const string HeroLevel = "HeroLevel";//角色等级
    public const string ChapterID = "ChapterID";//章节
    public const string LevelID = "LevelID";//关卡 = 层
    public const string RoomID = "RoomID";//房间
    public const string Items = "Items";//物品's
    public const string ItemId = "ItemId";//物品
    
}