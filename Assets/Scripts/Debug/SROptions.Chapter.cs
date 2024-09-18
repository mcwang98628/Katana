using System.ComponentModel;
using SRDebugger;
using SRDebugger.Internal;
using SRF.Service;
using UnityEngine;

public partial class SROptions
{

    [Category("章节相关")]
    public void UnlockAllChapter()
    {
        
        if (!ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas.ContainsKey(0))
        {
            ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas.Add(0,new ChapterClearanceData(){Schedule = 100,Count = 1});
        }
        else
        {
            ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas[0] = (new ChapterClearanceData(){Schedule = 100,Count = 1});
        }
        
        foreach (var cpData in DataManager.Inst.ChapterTableDatas)
        {
            if (!ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas.ContainsKey(cpData.Key))
            {
                ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas.Add(cpData.Key,new ChapterClearanceData(){Schedule = 100,Count = 1});
            }
            else
            {
                ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas[cpData.Key] = (new ChapterClearanceData(){Schedule = 100,Count = 1});
            }
        }
        ArchiveManager.Inst.SaveArchive();
        Debug.Log($"全部章节已解锁");
    }
}
