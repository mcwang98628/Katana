using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChapterTabData
{
    public int Id;
    public string ChapterDataPath;
    public string IconName;
    public string ChapterName;
    public string Desc;
    
    public int LevelCount;//一共多少层，仅UI展示需要
    public int SoulCount;//免费给多少Soul

    public List<int> UnlockItemIds;
    public Color TitleBg;

    public ChapterTabData(int id,string cpDataPath,string icon,string cpName,string desc,int levelCount,int soulCount,string itemIds,string colorStr)
    {
        Id = id;
        ChapterDataPath = cpDataPath;
        IconName = icon;
        ChapterName = cpName;
        Desc = desc;
        LevelCount = levelCount;
        SoulCount = soulCount;
        UnlockItemIds = new List<int>();
        var idStr = itemIds.Split('-');
        foreach (string itemIdStr in idStr)
        {
            if (string.IsNullOrEmpty(itemIdStr))
                continue;
            
            int itemId = int.Parse(itemIdStr);
            UnlockItemIds.Add(itemId);
        }

        var colorStrArrar = colorStr.Split('-');
        TitleBg = new Color(
            float.Parse(colorStrArrar[0]),
            float.Parse(colorStrArrar[1]),
            float.Parse(colorStrArrar[2]),
            1);
    }

    public void LoadChapterData(Action<ChapterData> callback)
    {
        if (!DataManager.Inst.ChapterDatas.ContainsKey(Id))
        {
            ChapterTabData tmpThis = this;
            ResourcesManager.Inst.GetAsset<ChapterData>("Assets/AssetsPackage/ChapterDatas/" + tmpThis.ChapterDataPath + ".asset",
                delegate(ChapterData data)
                {
                    DataManager.Inst.ChapterDatas.Add(tmpThis.Id,data);
                    callback?.Invoke(DataManager.Inst.ChapterDatas[tmpThis.Id]);
                });
        }
        else
        {
            callback?.Invoke(DataManager.Inst.ChapterDatas[Id]);
        }
    }
}
