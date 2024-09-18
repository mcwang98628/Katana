// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
//
// [Serializable]
// public class ArchiveData
// {
//     public string Version;
//     public bool ThroughTutorial;//通过教程
//     
//     public float GameTime;
//     
//     //战斗数据，可以为Null
//     public BattleArchiveData BattleData;
// //临时数据
//     public TemporaryData TemporaryData;
//     
//     //统计数据，多用于任务和成就。
//     public StatisticsData StatisticsData;
//
//     //完成的任务List
//     public List<int> OverMissionIds = new List<int>();
//     //显示提示过任务完成的Mission List
//     public List<int> TipsOverMissionIds = new List<int>();
//     //最后一次选择的英雄ID
//     public int LastSelectHeroID ;
//
//     //英雄的流派ID， HeroId-SchoolId
//     public Dictionary<int, int> SelectHeroSchoolId = new Dictionary<int, int>();
//     //解锁的英雄，key：id，value：level
//     public Dictionary<int,int> UnLockHeros = new Dictionary<int, int>();
//     //解锁的流派
//     public List<int> UnLockHeroSchools = new List<int>();
//     //已经解锁的物品
//     public List<int> UnLockItems = new List<int>();
//     
//     //章节通关信息
//     public Dictionary<int,ChapterClearanceData> ChapterClearanceDatas = new Dictionary<int, ChapterClearanceData>();
//     //一次性的临时物品，在城镇商店处购买，进入战斗时给玩家添加到身上 然后清空这个List。
//     public List<int> TemporaryItems = new List<int>();
//     
//     
//     
//     //局外金币
//     public int Gold;
//     //BGM
//     public float BgmVolume = 1f;
//     //音效
//     public float SoundVolume = 1f;
//     //语言
//     public SystemLanguage Language;
//     //设置过语言
//     public bool IsSetLanguage;
//
//     //App启动次数
//     public int AppStartTimes;
//     //开始游戏次数
//     public int StartGameTimes;
//     public Dictionary<string,int> TodayStartGameTimes = new Dictionary<string, int>();
//     
//     
//     
// }
//
//
//
// [Serializable]
// public class ChapterClearanceData
// {
//     public int Schedule;
//     public int Count;
// }
//
//
//
//
// [Serializable]
// public class TemporaryData
// {
//     public List<int> UnlockItems = new List<int>();
//     public int AddGoldValue;//添加的Gold
//     
//     //已经解锁的物品
//     public List<int> UnlockedItems = new List<int>();
// }
//
// [Serializable]
// public class KillEnemyData
// {
//     public int Id;
//     public int Count;
// }
// //统计数据
// [Serializable]
// public class StatisticsData
// {
//     /// <summary>
//     /// 击杀多怪物数量，Key：ID，Value：Count
//     /// </summary>
//     public Dictionary<int,KillEnemyData> KillEnemys = new Dictionary<int, KillEnemyData>();
//         
//     // public List<KillEnemyData> KillEnemys = new List<KillEnemyData>();
//     
//     /// <summary>
//     /// 击杀的怪物数量总和
//     /// </summary>
//     public int AllKillEnemyCount
//     {
//         get
//         {
//             int count = 0;
//             foreach (var enemyCount in KillEnemys)
//             {
//                 count += enemyCount.Value.Count;
//             }
//             return count;
//         }
//     }
//     
//     //TODO 其他数据
//
//
//     public void AddKillEnemyCount(int id,int count)
//     {
//         if (!KillEnemys.ContainsKey(id))
//         {
//             KillEnemys.Add(id,new KillEnemyData(){Id = id,Count = 0});
//         }
//
//         KillEnemys[id].Count += count;
//     }
//
//     public int GetAtEnemyCount(int id)
//     {
//         int count = 0;
//         for (int i = 0; i < KillEnemys.Count; i++)
//         {
//             if (KillEnemys[i].Id == id)
//             {
//                 count = KillEnemys[i].Count;
//             }
//         }
//
//         return count;
//     }
//     
// }
//
// #region 战斗中存档
//
//
// /// <summary>
// /// 战斗中存档
// /// </summary>
// [Serializable]
// public class BattleArchiveData
// {
//     public BattleArchiveChapterData ChapterData;
//     public BattleArchivePlayerData PlayerData;
// }
//
// public class BattleArchiveChapterData
// {
//     public int CurrentCPId;
//     public int CurrentLevelIndex;
//     public int CurrentRoomIndex;
//     public ChapterStructData ChapterStructData;
// }
// public class BattleArchivePlayerData
// {
//     //剩余金钱
//     public int CurrentGold;
//     //英雄数据
//     public HeroUpgradeData HeroData;
//     //玩家身上的buff item 和当前血量
//     public float CurrentHp;
//     public List<int> Items = new List<int>();
//     // public List<int> Buffs = new List<int>();
// }
//
// #endregion
