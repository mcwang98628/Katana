using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArchiveData_1_4_0 : IArchiveData
{
    public string Version => "1.4.0";
    
    public TemporaryData_1_4_0 TemporaryData;
    public GlobalData_1_4_0 GlobalData;
    public BattleArchiveData_1_4_0 BattleData;
    public StatisticsData_1_4_0 StatisticsData;
    public SettingArchiveData_1_4_0 SettingArchiveData;

    
    public void AddKillEnemyCount(int id,int count)
    {
        if (!StatisticsData.KillEnemys.ContainsKey(id))
        {
            StatisticsData.KillEnemys.Add(id, 0);
        }
        StatisticsData.KillEnemys[id] += count;
    }
    public int GetAtEnemyCount(int id)
    {
        return StatisticsData.KillEnemys[id];
    }


    
    // /// <summary>
    // /// 存档数据升级
    // /// </summary>
    // /// <param name="archiveData">1.3.11存档数据</param>
    // /// <returns>1.4.0存档数据</returns>
    // public static ArchiveData_1_4_0 ArchiveUpgrade(ArchiveData archiveData)
    // {
    //     ArchiveData_1_4_0 archiveData140 = new ArchiveData_1_4_0();
    //     //临时数据
    //     archiveData140.TemporaryData = new TemporaryData_1_4_0();
    //     archiveData140.TemporaryData.UnlockedItems.AddRange(archiveData.TemporaryData.UnlockedItems);
    //     archiveData140.TemporaryData.UnlockItems.AddRange(archiveData.TemporaryData.UnlockItems);
    //     archiveData140.TemporaryData.AddGoldValue = archiveData.TemporaryData.AddGoldValue;
    //     //全局数据
    //     archiveData140.GlobalData = new GlobalData_1_4_0();
    //     archiveData140.GlobalData.Gold = archiveData.Gold;
    //     archiveData140.GlobalData.Fire = 24;
    //     archiveData140.GlobalData.MaxFire = 12;
    //     archiveData140.GlobalData.FireRecoveryTime = -1;
    //     archiveData140.GlobalData.OneFireRecoveryNeedTime = 300;
    //     // foreach (KeyValuePair<int,int> keyValuePair in archiveData.SelectHeroSchoolId)
    //     // {
    //     //     archiveData140.GlobalData.SelectHeroSchoolId.Add(keyValuePair.Key,keyValuePair.Value);
    //     // }
    //     foreach (KeyValuePair<int,int> keyValuePair in archiveData.UnLockHeros)
    //     {
    //         archiveData140.GlobalData.UnLockHeros.Add(keyValuePair.Key);
    //     }
    //     // archiveData140.GlobalData.UnLockHeroSchools.AddRange(archiveData.UnLockHeroSchools);
    //     archiveData140.GlobalData.UnLockItems.AddRange(archiveData.UnLockItems);
    //     archiveData140.GlobalData.LastSelectHeroID = archiveData.LastSelectHeroID;
    //     archiveData140.GlobalData.ThroughTutorial = archiveData.ThroughTutorial;
    //     //战斗数据
    //     if (archiveData.BattleData == null)
    //     {
    //         archiveData140.BattleData = null;
    //     }
    //     else
    //     {
    //         archiveData140.BattleData = new BattleArchiveData_1_4_0();
    //         archiveData140.BattleData.ChapterData = new BattleArchiveChapterData_1_4_0();
    //         archiveData140.BattleData.ChapterData.CurrentCPId = archiveData.BattleData.ChapterData.CurrentCPId;
    //         archiveData140.BattleData.ChapterData.CurrentLevelIndex = archiveData.BattleData.ChapterData.CurrentLevelIndex;
    //         archiveData140.BattleData.ChapterData.CurrentRoomIndex = archiveData.BattleData.ChapterData.CurrentRoomIndex;
    //         archiveData140.BattleData.PlayerData = new BattleArchivePlayerData_1_4_0();
    //         archiveData140.BattleData.PlayerData.Items.AddRange(archiveData.BattleData.PlayerData.Items);
    //         archiveData140.BattleData.PlayerData.CurrentGold = archiveData.BattleData.PlayerData.CurrentGold;
    //         archiveData140.BattleData.PlayerData.CurrentHp = archiveData.BattleData.PlayerData.CurrentHp;
    //         archiveData140.BattleData.PlayerData.HeroId = archiveData.BattleData.PlayerData.HeroData.HeroID;
    //         archiveData140.BattleData.PlayerData.HeroLevel = archiveData.BattleData.PlayerData.HeroData.Level;
    //     }
    //     //统计数据
    //     archiveData140.StatisticsData = new StatisticsData_1_4_0();
    //     foreach (var value in archiveData.StatisticsData.KillEnemys)
    //     {
    //         archiveData140.StatisticsData.KillEnemys.Add(value.Value.Id,value.Value.Count);
    //     }
    //     foreach (KeyValuePair<string,int> keyValuePair in archiveData.TodayStartGameTimes)
    //     {
    //         archiveData140.StatisticsData.TodayStartGameTimes.Add(keyValuePair.Key,keyValuePair.Value);
    //     }
    //     foreach (KeyValuePair<int,ChapterClearanceData> keyValuePair in archiveData.ChapterClearanceDatas)
    //     {
    //         archiveData140.StatisticsData.ChapterClearanceDatas.Add(keyValuePair.Key,keyValuePair.Value);
    //     }
    //     
    //     archiveData140.StatisticsData.AppStartTimes = archiveData.AppStartTimes;
    //     archiveData140.StatisticsData.StartGameTimes = archiveData.StartGameTimes;
    //     archiveData140.StatisticsData.GameTime = archiveData.GameTime;
    //     
    //     return archiveData140;
    // }
    //
    
    public static void NewArchiveData(Action<ArchiveData_1_4_0> callBack)
    {
        
        
        DataManager.Inst.PeekDefaultUnLockItems(delegate(List<int> ints)
        {
            ArchiveData_1_4_0 archiveData140 = new ArchiveData_1_4_0();
            // archiveData140.EquipmentArchiveData = new EquipmentArchiveData();
            //临时数据
            archiveData140.TemporaryData = new TemporaryData_1_4_0();
            archiveData140.TemporaryData.UnlockedItems.Clear();
            archiveData140.TemporaryData.UnlockItems.Clear();
            archiveData140.TemporaryData.AddDiamondValue = 0;
            // archiveData140.TemporaryData.AddSoulValue = 0;
            //全局数据
            archiveData140.GlobalData = new GlobalData_1_4_0();
            archiveData140.GlobalData.Diamond = 0;
            archiveData140.GlobalData.Soul = 0;
            archiveData140.GlobalData.Exp = 0;
            archiveData140.GlobalData.Fire = 12;
            archiveData140.GlobalData.MaxFire = 6;
            archiveData140.GlobalData.FireRecoveryTime = -1;
            // archiveData140.GlobalData.OneFireRecoveryNeedTime = 7200;
            archiveData140.GlobalData.UnLockHeros.Add(1101);
            // ReSharper disable once PossibleInvalidOperationException
            archiveData140.GlobalData.HeroUpgradeDatas.Add(1101,new HeroUpgradeInfo());
            archiveData140.GlobalData.HeroUpgradeDatas[1101].SetHeroUpgradeData(DataManager.Inst.GetHeroUpgradeData(1101,1,0).Value);
            archiveData140.GlobalData.LastSelectHeroID = 1101;
            archiveData140.GlobalData.LastSelectChapterId = 1;
            archiveData140.GlobalData.ThroughTutorial = false;
            // archiveData140.GlobalData.UnLockItems = ints;
            //战斗数据
            archiveData140.BattleData = null;
            //统计数据
            archiveData140.StatisticsData = new StatisticsData_1_4_0();
            archiveData140.StatisticsData.KillEnemys.Clear();
            archiveData140.StatisticsData.TodayStartGameTimes.Clear();
            archiveData140.StatisticsData.AppStartTimes = 0;
            archiveData140.StatisticsData.StartGameTimes = 0;
            archiveData140.StatisticsData.GameTime = 0;
            archiveData140.StatisticsData.ChapterClearanceDatas.Clear();
            // archiveData140.StatisticsData.ChapterClearanceDatas.Add(0,new ChapterClearanceData(){Count = 1}); 跳过新手章节
            archiveData140.SettingArchiveData = new SettingArchiveData_1_4_0();
            archiveData140.SettingArchiveData.Sound = true;
            archiveData140.SettingArchiveData.Bgm = true;
            
            callBack?.Invoke(archiveData140);
        });
    }
}


[Serializable]
public class GlobalData_1_4_0
{
    //火种恢复剩余时间
    public long FireTimeLeft
    {
        get
        {
            if (FireRecoveryTime == -1)
            {
                return -1;
            }

            // OneFireRecoveryNeedTime = 5;//TEST
            
            long currentTime = DateTime.Now.Ticks / 10000000;
            long elapsedTime = currentTime - FireRecoveryTime;
            if (elapsedTime>OneFireRecoveryNeedTime)
            {
                var fireCount = Math.Floor((float)elapsedTime/OneFireRecoveryNeedTime);
                Fire += (int)fireCount;
                elapsedTime %= OneFireRecoveryNeedTime;
                if (Fire >= MaxFire)
                {
                    Fire = MaxFire;
                    FireRecoveryTime = -1;
                }
                else
                {
                    FireRecoveryTime = DateTime.Now.Ticks / 10000000;
                }
            }
            return OneFireRecoveryNeedTime - elapsedTime;
        }
    }

    public int CurrentLevel
    {
        get
        {
            int level = 0;
            int currentExp = Exp;
            var expLevelData = DataManager.Inst.ExpLevelData;
            while (expLevelData.ContainsKey(level))
            {
                if (currentExp >= expLevelData[level].UpgradeNeedExp)
                {
                    currentExp -= expLevelData[level].UpgradeNeedExp;
                    level++; 
                }
                else
                {
                    break;
                }
            }
            return level;
        }
    }
    public float CurrentLevelExpPercentage
    {
        get
        {
            float value;
            int level = 0;
            int currentExp = Exp;
            var expLevelData = DataManager.Inst.ExpLevelData;
            while (expLevelData.ContainsKey(level) && currentExp >= expLevelData[level].UpgradeNeedExp)
            {
                currentExp -= expLevelData[level].UpgradeNeedExp;
                level++;
            }

            if (expLevelData.ContainsKey(level))
            {
                value = (float) currentExp / (float) expLevelData[level].UpgradeNeedExp;
            }
            else
            {
                value = 1;
            }

            return value;
        }
    }

    //经验值
    public int Exp;
    //魂
    public int Soul;
    //钻石
    public int Diamond;
    
    //火种
    public int Fire;
    //最大火种
    public int MaxFire;
    //火种开始恢复时间,-1为满了没有在恢复
    public long FireRecoveryTime;
    //一个火种恢复所需要的时间 - 秒
    public int OneFireRecoveryNeedTime => 1;
    
    //解锁的英雄
    public List<int> UnLockHeros = new List<int>();
    //已经解锁的物品
    // public List<int> UnLockItems = new List<int>();
    
    // key = heroId
    public Dictionary<int,HeroUpgradeInfo> HeroUpgradeDatas = new Dictionary<int, HeroUpgradeInfo>();
    
    //最后一次选择的英雄ID
    public int LastSelectHeroID ;
    public int LastSelectChapterId;
    public bool ThroughTutorial;



    public int UnlockRuneCount
    {
        get
        {
            int count = 0;
            foreach (var runeItem in UnlockRuneItem)
            {
                count+=runeItem.Value;
            }
            return count;
        }
    }

    //Key = itemId,Value = Count
    public Dictionary<int,int> UnlockRuneItem = new Dictionary<int, int>();
    
    public List<int> MissionCompletedIdList = new List<int>();

    public List<int> ReceiveChapterFreeSoul = new List<int>();
    
    public List<int> ReceiveEnemyIllustrated = new List<int>();
    public List<int> ReceiveItemIllustrated = new List<int>();
    public List<int> ReceiveItemSchoolIllustrated = new List<int>();
    //HeroId,LevelList
    public Dictionary<int,Dictionary<int,List<int>>> ReceiveHeroSkill = new Dictionary<int, Dictionary<int,List<int>>>();
    
    public List<UnlockSystemType> PlayedUnlockSystemAnimaton = new List<UnlockSystemType>();
}

[Serializable]
public class HeroUpgradeInfo
{
    public int UseingExp;
    public int HeroId;
    public int ColorLevel;
    public int Level;

    public void SetHeroUpgradeData(HeroUpgradeData value)
    {
        HeroId = value.HeroId;
        ColorLevel = value.ColorLevel;
        Level = value.Level;
    }
    public HeroUpgradeData GetHeroUpgradeData()
    {
        return DataManager.Inst.GetHeroUpgradeData(HeroId, ColorLevel, Level).Value;
    }
}

public enum UnlockSystemType
{
    Rune,
    Event,
    Hero,
    Equipment
}

[Serializable]
public class TemporaryData_1_4_0
{
    //上一场战斗中解锁的Item
    public List<int> UnlockItems = new List<int>();
    //上一场战斗中添加的
    public int AddDiamondValue;
    //上一场战斗中添加的
    public int AddSoulValue;
    //上一场战斗中添加的
    public int AddExpValue;
    //已经解锁的物品 - 每次战斗之前缓存一份，防止随机出战斗过程中解锁的Item
    public List<int> UnlockedItems = new List<int>();

    public List<Equipment> EquipmentList = new List<Equipment>();
}


//统计数据
[Serializable]
public class StatisticsData_1_4_0
{
    //App启动次数
    public int AppStartTimes;
    //开始游戏次数
    public int StartGameTimes;
    public float GameTime;
    public Dictionary<string,int> TodayStartGameTimes = new Dictionary<string, int>();
    /// <summary>
    /// 击杀多怪物数量，Key：ID，Value：Count
    /// </summary>
    public Dictionary<int, int> KillEnemys = new Dictionary<int, int>();
    //章节通关信息
    public Dictionary<int,ChapterClearanceData> ChapterClearanceDatas = new Dictionary<int, ChapterClearanceData>();
    //获得过的道具
    public List<int> GetItemList = new List<int>();
    public List<int> UnlockItemSchool= new List<int>();
    /// <summary>
    /// 击杀的怪物数量总和
    /// </summary>
    public int AllKillEnemyCount
    {
        get
        {
            int count = 0;
            foreach (var enemyCount in KillEnemys)
            {
                count += enemyCount.Value;
            }
            return count;
        }
    }
}

public class SettingArchiveData_1_4_0
{
    public bool Sound;
    public bool Bgm;
}


#region BattleArchiveData

[Serializable]
public class BattleArchiveData_1_4_0:IArchiveBattleData
{
    public Guid Guid;
    public BattleArchiveChapterData_1_4_0 ChapterData;
    public BattleArchivePlayerData_1_4_0 PlayerData;
}
[Serializable]
public class BattleArchiveChapterData_1_4_0
{
    public int CurrentCPId;
    public int CurrentLevelIndex;
    public int CurrentRoomIndex;
}
[Serializable]
public class BattleArchivePlayerData_1_4_0
{
    public int CurrentGold;
    public int HeroId;
    public float CurrentHp;
    public List<int> Items = new List<int>();
}

#endregion