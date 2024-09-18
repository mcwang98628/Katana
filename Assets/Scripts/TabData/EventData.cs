using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EventData 
{
    public int Id;
    public string ChapterDataPath;
    public string IconName;
    public string EventName;
    public string Desc;

    public int Time;//时间


    public EventData(int id,string chapterDataPath,string icon,string eventName,string desc,int time)
    {
        Id = id;
        ChapterDataPath = chapterDataPath;
        IconName = icon;
        EventName = eventName;
        Desc = desc;
        Time = time;
    }
    
    // public ChapterData LoadChapterData()
    // {
    //     if (!DataManager.Inst.ChapterDatas.ContainsKey(Id))
    //     {
    //         DataManager.Inst.ChapterDatas.Add(Id,ResourcesManager.Inst.GetAsset<ChapterData>("Assets/AssetsPackage/ChapterDatas/" + ChapterDataPath + ".asset"));
    //     }
    //     return DataManager.Inst.ChapterDatas[Id];
    // }
    
    
}
