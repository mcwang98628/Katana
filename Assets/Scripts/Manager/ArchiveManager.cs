using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public partial class ArchiveManager : TSingleton<ArchiveManager>
{
    public const string ArchiveName = "Archive.bin";
    public ArchiveData_1_5_0 ArchiveData => archiveData;
    private ArchiveData_1_5_0 archiveData;
    private bool isInit;
    
    public void Init()
    {
        if (isInit)
            return;
        isInit = true;
        if (GameManager.Inst.IsNewArchive)
        {
            NewArchive();
            return;
        }
        
        LoadArchive(delegate {
            ArchiveData.StatisticsData.AppStartTimes++;
            SaveArchive();
        });
    }

    public void NewArchive(Action callback = null)
    {
        ArchiveData_1_5_0.NewArchiveData(delegate(ArchiveData_1_5_0 data150)
        {
            archiveData = data150;
            PlayerPrefs.SetString("ArchiveVersion","1.5.0");
            SaveArchive();
            callback?.Invoke();
        });
    }
    public void SaveArchive()
    {
        SaveManager.Inst.SaveArchive<ArchiveData_1_5_0>(ArchiveName, archiveData);
    }
    public void SaveBattleArchive()
    {
        if (BattleManager.Inst.CurrentPlayer == null)
        {
            return;
        }

        ChapterRulesRuntimeData runtimeData = (ChapterRulesRuntimeData)BattleManager.Inst.RuntimeData;
        BattleArchiveData_1_4_0 newBattleData = new BattleArchiveData_1_4_0();
        newBattleData.Guid = BattleManager.Inst.BattleGuid;
        newBattleData.ChapterData = new BattleArchiveChapterData_1_4_0();
        newBattleData.ChapterData.CurrentCPId = runtimeData.CurrentChapterId;
        newBattleData.ChapterData.CurrentLevelIndex = runtimeData.CurrentLevelIndex;
        newBattleData.ChapterData.CurrentRoomIndex = runtimeData.CurrentRoomIndex;
        

        newBattleData.PlayerData = new BattleArchivePlayerData_1_4_0();
        newBattleData.PlayerData.CurrentGold = runtimeData.CurrentGold;
        newBattleData.PlayerData.HeroId = runtimeData.CurrentHeroId;
        
        newBattleData.PlayerData.CurrentHp = BattleManager.Inst.CurrentPlayer.CurrentHp;
        newBattleData.PlayerData.Items = BattleManager.Inst.CurrentPlayer.roleItemController.GetItemsId();
        
        ChapterClearance(runtimeData.CurrentChapterId,runtimeData.Progress,runtimeData.CurrentLevelIndex);
        SaveBattleData(newBattleData);
    }
    public void SaveBattleData(BattleArchiveData_1_4_0 battleData)
    {
        if (battleData != null && battleData.ChapterData.CurrentCPId < 0)
        {
            return;
        }
        archiveData.BattleData = battleData;
        SaveArchive();
    }
    public void AddGameTime(float time)
    {
        archiveData.StatisticsData.GameTime = time;
    }

    public void SetSound(bool isOn)
    {
        archiveData.SettingArchiveData.Sound = isOn;
        EventManager.Inst.DistributeEvent(EventName.SetSound);
    }

    public void SetBgm(bool isOn)
    {
        archiveData.SettingArchiveData.Bgm = isOn;
        EventManager.Inst.DistributeEvent(EventName.SetBmg);
    }

    //加载场景
    private void LoadArchive(Action callBack = null)
    {
        if (!SaveManager.Inst.IsHaveArchiveFile(ArchiveName))
        {
            NewArchive(callBack);
            return;
        }
        TemporaryArchiveData(callBack);
    }

    void TemporaryArchiveData(Action callBack)
    {
        try
        {
            var version = PlayerPrefs.GetString("ArchiveVersion");
            switch (version)
            {
                case "1.4.0":
                    var archive140 = SaveManager.Inst.LoadArchive<ArchiveData_1_4_0>(ArchiveName);
                    archiveData = ArchiveData_1_5_0.ArchiveUpgrade(archive140);
                    PlayerPrefs.SetString("ArchiveVersion","1.5.0");
                    break;
                case "1.5.0":
                    archiveData = SaveManager.Inst.LoadArchive<ArchiveData_1_5_0>(ArchiveName);
                    break;
                default:
                    NewArchive(callBack);
                    break;
            }
            callBack?.Invoke();
        }
        catch (Exception e)
        {
            NewArchive(callBack);
        }
    }

    public void StartBattle()
    {
        archiveData.StatisticsData.StartGameTimes++;
        DateTime dt = DateTime.Now;
        string time = dt.ToLongDateString();
        if (!archiveData.StatisticsData.TodayStartGameTimes.ContainsKey(time))
        {
            archiveData.StatisticsData.TodayStartGameTimes.Add(time,0);    
        }

        archiveData.StatisticsData.TodayStartGameTimes[time]++;
        SaveArchive();
    }

    public void UnLockItem(int itemId,bool mute = false)
    {
        // if (archiveData.GlobalData.UnLockItems.Contains(itemId))
        // {
        //     return;
        // }
        // archiveData.GlobalData.UnLockItems.Add(itemId);
        // if (!mute)
        // {
        //     archiveData.TemporaryData.UnlockItems.Add(itemId);
        //     EventManager.Inst.DistributeEvent(EventName.OnUnlockItemEvent,itemId);
        // }
    }

    public void CopyUnLcokItems()
    {
        archiveData.TemporaryData.UnlockedItems.Clear();
        
        // archiveData.TemporaryData.UnlockedItems.AddRange(archiveData.GlobalData.UnLockItems);
        archiveData.TemporaryData.UnlockedItems.AddRange(DataManager.Inst.GetAllItemIds());
    }
    
    public bool CheckItemIsUnLock(int itemId)
    {
        return true;
// #if TEST_MODE
//         if (GameManager.Inst.TestMode)
//             return true;
// #endif
        // return archiveData.TemporaryData.UnlockedItems.Contains(itemId);
    }

    public void ChapterClearance(int cpId,int schedule,int level)
    {
        if (!archiveData.StatisticsData.ChapterClearanceDatas.ContainsKey(cpId))
        {
            archiveData.StatisticsData.ChapterClearanceDatas.Add(cpId,new ChapterClearanceData());
        }

        if (schedule > archiveData.StatisticsData.ChapterClearanceDatas[cpId].Schedule)
        {
            archiveData.StatisticsData.ChapterClearanceDatas[cpId].Schedule = schedule;
        }
        if (schedule >= 99)
        {
            archiveData.StatisticsData.ChapterClearanceDatas[cpId].Count++;
        }

        if (level > archiveData.StatisticsData.ChapterClearanceDatas[cpId].LastLevel)
        {
            archiveData.StatisticsData.ChapterClearanceDatas[cpId].LastLevel = level;
        }
    }

    public ChapterClearanceData GetChapterClearance(int cpId)
    {
        if (archiveData.StatisticsData.ChapterClearanceDatas.ContainsKey(cpId))
        {
            return archiveData.StatisticsData.ChapterClearanceDatas[cpId];
        }

        return null;
    }
    

    //sudo wget wget -cO gitlab-12.3_zh.tar.gz https://gitlab.com/xhang/gitlab/repository/archive.tar.gz?ref=12-3-stable-zh
    public void AddUnlockRuneItem(ExpLevelData runeUpgrade)
    {
        ChangeDiamond(-runeUpgrade.RuneItemPrice,true);
        if (!archiveData.GlobalData.UnlockRuneItem.ContainsKey(runeUpgrade.RuneItemId))
        {
            archiveData.GlobalData.UnlockRuneItem.Add(runeUpgrade.RuneItemId,0);
        }

        archiveData.GlobalData.UnlockRuneItem[runeUpgrade.RuneItemId]++;
        
        SaveArchive();
    }
    
    #region 资源

    
    public void ChangeDiamond(int value,bool mute = false)
    {
        archiveData.GlobalData.Diamond += value;
        // if (!mute)
        // {
        //     if (value>0)
        //     {
        //         archiveData.TemporaryData.AddGoldValue += value;
        //     }
        // }
        EventManager.Inst.DistributeEvent(EventName.OnArchiveDiamondChange,value);
    }
    public void ChangeSoul(int value)
    {
        archiveData.GlobalData.Soul += value;
        EventManager.Inst.DistributeEvent(EventName.OnArchiveSoulChange,value);
    }
    public void ChangeExp(int value)
    {
        archiveData.GlobalData.Exp += value;
        EventManager.Inst.DistributeEvent(EventName.OnArchiveExpChanage,value);
    }
    public void UseFire(int count)
    {
        var time = ArchiveData.GlobalData.FireTimeLeft;
        if (ArchiveData.GlobalData.Fire <= 0)
        {
            return;
        }

        ArchiveData.GlobalData.Fire-=count;
        if (
            ArchiveData.GlobalData.Fire < ArchiveData.GlobalData.MaxFire && 
            ArchiveData.GlobalData.FireRecoveryTime == -1
        )
        {
            ArchiveData.GlobalData.FireRecoveryTime = DateTime.Now.Ticks/10000000;
        }
        EventManager.Inst.DistributeEvent(EventName.OnArchiveFireChange);
        SaveArchive();
    }
    

    #endregion

    public void MissionCompleted(int missionId)
    {
        if (!DataManager.Inst.MissionTabDatas.ContainsKey(missionId))
        {
            return;
        }

        if (archiveData.GlobalData.MissionCompletedIdList.Contains(missionId))
        {
            return;
        }

        var missionData = DataManager.Inst.MissionTabDatas[missionId];
        archiveData.GlobalData.MissionCompletedIdList.Add(missionId);
        switch (missionData.RewardType)
        {
            case MissionRewardType.Diamond:
                ChangeDiamond(missionData.RewardNumber);
                break;
            case MissionRewardType.Soul:
                ChangeSoul(missionData.RewardNumber);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        SaveArchive();
    }

    public void ReceiveChapterFreeSoul(int cpId)
    {
        if (ReceivedChapterFreeSoul(cpId))
        {
            return;
        }
        var soul = DataManager.Inst.ChapterTableDatas[cpId].SoulCount;
        archiveData.GlobalData.ReceiveChapterFreeSoul.Add(cpId);
        ChangeSoul(soul);
        SaveArchive();
    }

    public bool ReceivedChapterFreeSoul(int cpid)
    {
        return archiveData.GlobalData.ReceiveChapterFreeSoul.Contains(cpid);
    }
    
    
    
    public bool EnemyIllustratedCanGetReceive()
    {
        foreach (KeyValuePair<int,int> killEnemyId in archiveData.StatisticsData.KillEnemys)
            if (!archiveData.GlobalData.ReceiveEnemyIllustrated.Contains(killEnemyId.Key))
                return true;

        return false;
    }

    public bool ItemIllustratedCanGetReceive()
    {
        foreach (int itemId in archiveData.StatisticsData.GetItemList)
            if (!archiveData.GlobalData.ReceiveItemIllustrated.Contains(itemId) && DataManager.Inst.GetItemScrObj(itemId).ItemType == ItemType.Artifact)
                return true;

        return false;
    }

    public bool ItemBuildIllustratedCanGetReceive()
    {
        foreach (int itemSchoolId in archiveData.StatisticsData.UnlockItemSchool)
            if (!archiveData.GlobalData.ReceiveItemSchoolIllustrated.Contains(itemSchoolId))
                return true;

        return false;
    }
    
    public int CanReceiveSkillSoul(int heroId,int skillId,bool isReceive = false)
    {
        if (!ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas.ContainsKey(heroId))
            return 0;

        var heroUpgrade = ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas[heroId];        
        var targetSkillDesc = DataManager.Inst.GetHeroTargetAllSkillDesc(
            heroUpgrade.HeroId, 
            heroUpgrade.ColorLevel,
            heroUpgrade.Level);

        
        if (!ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveHeroSkill.ContainsKey(heroId))
        {
            ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveHeroSkill.Add(heroId,new Dictionary<int, List<int>>());
        }
        var heroLevelList = ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveHeroSkill[heroId];
        int receiveTimes = 0;
        for (int i = 0; i < targetSkillDesc.Count; i++)
        {
            if (targetSkillDesc[i].SkillId != skillId)
                continue;
            
            int colorLevel = targetSkillDesc[i].ColorLevel;
            if (!heroLevelList.ContainsKey(colorLevel))
                heroLevelList.Add(colorLevel,new List<int>());
            if (!heroLevelList[colorLevel].Contains(targetSkillDesc[i].Level))
            {
                if (isReceive)
                    heroLevelList[colorLevel].Add(targetSkillDesc[i].Level);
                receiveTimes++;
            }
        }

        return receiveTimes;
    }
    
    
    
    //获取最新的章节id
    public int GetLastNewChapterId()
    {
        int chapterId = 1;
        int maxChapterId = 0;
        var datas = DataManager.Inst.GetAllCpData();
        foreach (var chapterData in datas)
        {
            var data = ArchiveManager.Inst.GetChapterClearance(chapterData.Key);
            if (maxChapterId < chapterData.Key)
            {
                maxChapterId = chapterData.Key;
            }
            if (data == null || data.Count<=0)
            {
                continue;
            }

            int value = chapterData.Key + 1;
            if (chapterId < value)
            {
                chapterId = value;
            }
        }

        if (chapterId > maxChapterId)
        {
            chapterId = maxChapterId;
        }

        return chapterId;
    }

    
}




public enum ArchiveErrorType
{
    NoError,//无Error
    GoldShortage,//金币不足
    DiamondShortage,//钻石不足
    SourShortage,//魂不足
    AlreadyOwned,//已经拥有
    CanNotBuy,//不可购买
    ProductNotFound,//商品不存在
    ErrorNotFound,//未知错误
    MaxLevel,//已经满级
}
[Serializable]
public class ChapterClearanceData
{
    public int Schedule;
    public int Count;
    public int LastLevel;//最后一层到哪里
}