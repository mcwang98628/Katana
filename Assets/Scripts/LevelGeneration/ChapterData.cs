using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Chapter Data Asset")]
public class ChapterData : ScriptableObject
{

    public void Init()
    {
        EnemyData.Init();
        RoomData.Init();
    }

    [Header("基本数据")]
    [LabelText("关卡环境")]
    public List<EnvironmentItemScript> EnvironmentData;
    [LabelText("关卡类型")]
    public LevelStructType levelStructType;
    [ShowIf("levelStructType", LevelStructType.Chapter)]
    public bool IsTutorial = false;



    [Header("关卡结构")]

    [ShowIf("levelStructType", LevelStructType.Chapter)]
    [SerializeField]
    [LabelText("关卡结构")] [GUIColor(0.6f, 0.8f, 1f)] public List<LevelStructConfig_Floor> ChapterStruct;
    [System.Serializable]
    public struct LevelStructConfig_Floor
    {
        public List<RoomInfoConfig> Rooms;
    }

    [Header("无尽关卡房间")]
    [HideLabel]
    [ShowIf("levelStructType", LevelStructType.OneRoomEndless)]
    public LevelStructConfig_OneRoomEndless EndlessLevelData;



    [Header("关卡数据")]


    [LabelText("房间数据")] [GUIColor(0.4f, 1f, 0.5f)] public RoomDataGroup RoomData;
    [System.Serializable]
    public class RoomDataGroup
    {
        [GUIColor(0.4f, 1f, 0.5f)] [SerializeField] private List<RoomController> StartRooms;
        List<RoomController> startRooms;
        [GUIColor(0.4f, 1f, 0.5f)] [SerializeField] private List<FightRoom> FightRooms;
        List<FightRoom> fightRooms;
        [GUIColor(0.4f, 1f, 0.5f)] [SerializeField] private List<FightRoom> BossFightRooms;
        List<FightRoom> bossFightRooms;
        [SerializeField] private RoomController EmptyShopRoom;
        [SerializeField] private RoomController EmptyTreasureRoom;
        public void Init()
        {
            startRooms = new List<RoomController>(StartRooms.ToArray());
            fightRooms = new List<FightRoom>(FightRooms.ToArray());
            bossFightRooms = new List<FightRoom>(BossFightRooms.ToArray());
        }
        public RoomController GetRoom(RoomType roomType, bool isTutorial)
        {
            RoomController result = null;
            switch (roomType)
            {
                case RoomType.StartRoom:
                    result = startRooms[0];
                    if (result == null)
                    {
                        Debug.LogError(startRooms.Count);
                    }
                    startRooms.RemoveAt(0);
                    break;
                case RoomType.FightRoom:
                    int index = isTutorial ? 0 : Random.Range(0, fightRooms.Count);
                    result = fightRooms[index];
                    fightRooms.RemoveAt(index);
                    break;
                case RoomType.BossFightRoom:
                    result = bossFightRooms[0];
                    bossFightRooms.RemoveAt(0);
                    break;
                case RoomType.ShopRoom:
                    result = EmptyShopRoom;
                    break;
                case RoomType.TreasureRoom:
                    result = EmptyTreasureRoom;
                    break;
                case RoomType.EventRoom:
                    result = EmptyTreasureRoom;
                    break;
            }
            return result;
        }
    }


    [LabelText("敌人数据")] [GUIColor(1f, 0.7f, 0.7f)] public EnemyDataGroup EnemyData;
    [System.Serializable]
    public class EnemyDataGroup
    {
        [SerializeField] private List<RoleController> EnemyList_Lv1;
        [SerializeField] private List<RoleController> EnemyList_Lv2;
        [SerializeField] private List<RoleController> EnemyList_Lv3;
        [SerializeField] private List<RoleController> EnemyList_Boss;
        List<RoleController> enemyList_Boss;
        public void Init()
        {
            enemyList_Boss = new List<RoleController>(EnemyList_Boss);
        }
        public EnemyInfoData GetEnemy(int enemyLevel)
        {
            RoleController result = null;
            switch (enemyLevel)
            {
                case 1:
                    if (EnemyList_Lv1 == null||EnemyList_Lv1.Count<1)
                        return null;
                    result = EnemyList_Lv1[Random.Range(0, EnemyList_Lv1.Count)];
                    break;
                case 2:
                    if (EnemyList_Lv2 == null||EnemyList_Lv2.Count<1)
                        return null;
                    result = EnemyList_Lv2[Random.Range(0, EnemyList_Lv2.Count)];
                    break;
                case 3:
                    if (EnemyList_Lv3 == null||EnemyList_Lv3.Count<1)
                        return null;
                    result = EnemyList_Lv3[Random.Range(0, EnemyList_Lv3.Count)];
                    break;
                case 4:
                    if (enemyList_Boss == null||enemyList_Boss.Count<1)
                        return null;
                    result = enemyList_Boss[0];
                    enemyList_Boss.RemoveAt(0);
                    break;
            }

            if (result == null)
                return null;
            return DataManager.Inst.GetEnemyInfo(result.gameObject.name);
        }
    }



    [LabelText("物品数据")] [GUIColor(0.4f, 1f, 0.5f)] public ItemDataGroup ItemData;
    [System.Serializable]
    public struct ItemDataGroup
    {
        [LabelText("神器池")] public ItemPoolScriptableObject ArtifactPool;
        [LabelText("神器原价")] public int ArtifactShopPrice;
        [LabelText("神器涨幅")] public float ArtifactShopPriceIncrease;
        [LabelText("道具池")] public ItemPoolScriptableObject PropPool;
        [LabelText("道具原价")] public int PropShopPrice;
        [LabelText("道具涨幅")] public float PropShopPriceIncrease;
        [LabelText("商店刷新价格")] 
        public int ShopRefreshPrice;
        [LabelText("刷新价格递增数量")] 
        public int ShopRefreshPriceStep;
    }


}

//需要加回血房
//需要加奇迹房

[Flags]
public enum RoomType
{
    None = 0,
    StartRoom = 1,
    FightRoom = 2,     //战斗房间
    BossFightRoom = 3, //Boss房间
    TreasureRoom = 4,  //宝藏房间
    EventRoom = 5,     //事件房间
    ShopRoom = 6,      //商店房间
    RecoverRoom = 7,   //回血房
}

public enum RoomRewardType
{
    None = 0,//无
    Item = 1,
    Money = 2,
    Hp = 3,
    GlobalDiamond=4,//钻石
}

[System.Serializable]
public class RoomInfoConfig
{
    public RoomType RoomType;

    [ShowIf("RoomType", RoomType.FightRoom)]
    public int EnemyWaveCount;
    [ShowIf("RoomType", RoomType.FightRoom)]
    public int MaxEnemyLv;
    [ShowIf("RoomType", RoomType.FightRoom)]
    public int EnemyWaveDifficult;

    [LabelText("奖励类型")]
    [HideIf("RoomType", RoomType.None)]
    [HideIf("RoomType", RoomType.StartRoom)]
    [HideIf("RoomType", RoomType.TreasureRoom)]
    [HideIf("RoomType", RoomType.EventRoom)]
    [HideIf("RoomType", RoomType.ShopRoom)]
    public RoomRewardType RoomRewardType;

    [ShowIf("RoomRewardType", RoomRewardType.Hp)]
    public int HpCount = 2;

    [ShowIf("RoomRewardType", RoomRewardType.Money)]
    public int RewardMoney;
    [ShowIf("RoomRewardType", RoomRewardType.GlobalDiamond)]
    public int GlobalDiamondCount;

    public RoomInfoConfig(RoomType roomType,int enemyWaveDifficult=0,int maxEnemyLv=0,int enemyWaveCount=0)
    {
        RoomType = roomType;
        EnemyWaveCount = enemyWaveCount;
        EnemyWaveDifficult = enemyWaveDifficult;
        MaxEnemyLv = maxEnemyLv;
    }
}
//章节模式关卡结构
[System.Serializable]


//无尽模式关卡
public class LevelStructConfig_OneRoomEndless
{
    [GUIColor(0.5f, 1f, 0.5f)]
    [BoxGroup("环境数据", false)]
    public EnvironmentItemScript EnvironmentData;

    [BoxGroup("无尽模式")]
    public EndlessRoom Room;

    [BoxGroup("无尽模式")]
    public List<int> EnemyIdPool = new List<int>();
}

[System.Serializable]
public class LevelStructConfig_ChapterTutorial
{
    public List<TutorialRoomData> Rooms;
}

[System.Serializable]
public class TutorialRoomData
{
    public RoomType RoomType;
    // public RoomRewardType RoomRewardType;
    public RoomController Room;

    [HideIf("RoomType", RoomType.None)]
    [HideIf("RoomType", RoomType.StartRoom)]
    [HideIf("RoomType", RoomType.TreasureRoom)]
    [HideIf("RoomType", RoomType.EventRoom)]
    [HideIf("RoomType", RoomType.ShopRoom)]
    public List<int> EnemyIds = new List<int>();
}