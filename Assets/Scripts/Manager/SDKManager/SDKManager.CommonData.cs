using System;
using System.Collections.Generic;


public partial class SDKManager
{
    private Dictionary<string, object> GetCommonData()
    {
        string time = DateTime.Now.ToLongDateString();
        int todayStartGameTimes = 0;
        if (ArchiveManager.Inst.ArchiveData.StatisticsData.TodayStartGameTimes.ContainsKey(time))
        {
            todayStartGameTimes = ArchiveManager.Inst.ArchiveData.StatisticsData.TodayStartGameTimes[time];
        }
        Dictionary<string, object> objects = new Dictionary<string, object>()
        {
            {TGANames.CurrentLanguage,LocalizationManger.Inst.CurrentLanguageType.ToString()},
            {TGANames.AppStartTimes,ArchiveManager.Inst.ArchiveData.StatisticsData.AppStartTimes},
            {TGANames.StartGameTimes,ArchiveManager.Inst.ArchiveData.StatisticsData.StartGameTimes},
            {TGANames.TodayStartGameTimes,todayStartGameTimes},
            {TGANames.IsHaveBattleArchive,ArchiveManager.Inst.ArchiveData.BattleData!=null},
            {TGANames.GameTime,ArchiveManager.Inst.ArchiveData.StatisticsData.GameTime},
            {TGANames.GlobalGold,ArchiveManager.Inst.ArchiveData.GlobalData.Diamond},
            {TGANames.GlobalSoul,ArchiveManager.Inst.ArchiveData.GlobalData.Soul},
            {TGANames.GlobalFire,ArchiveManager.Inst.ArchiveData.GlobalData.Fire},
            {TGANames.GlobalLevel,ArchiveManager.Inst.ArchiveData.GlobalData.CurrentLevel},
        };
        return objects;
    }
}