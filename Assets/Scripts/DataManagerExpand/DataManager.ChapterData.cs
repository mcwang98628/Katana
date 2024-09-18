using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private string ChapterDataCSVPath = "Assets/AssetsPackage/Table/CSV_ChapterDatas.csv";
    private string EventDataCSVPath = "Assets/AssetsPackage/Table/CSV_EventDatas.csv";
        
    public Dictionary<int,ChapterTabData> ChapterTableDatas = new Dictionary<int, ChapterTabData>();
    public Dictionary<int,ChapterData> ChapterDatas = new Dictionary<int, ChapterData>();
    
    public Dictionary<int,EventData> EventDatas = new Dictionary<int, EventData>();
    
    void InitChapterData()
    {
        initNumber++;
        ResourcesManager.Inst.GetAsset<TextAsset>(ChapterDataCSVPath, delegate(TextAsset infoText)
        {
            var cpDataCSV = Document.Load(infoText.text);
            for (int i = 1; i < cpDataCSV.Count; i++)
            {
                if (cpDataCSV[i].Count < 5)
                {
                    continue;
                }
                int id = cpDataCSV[i][0].Convert<int>(); 
            
                ChapterTabData tabData = new ChapterTabData(id,
                    cpDataCSV[i][1].Convert<string>(),
                    cpDataCSV[i][2].Convert<string>(),
                    cpDataCSV[i][3].Convert<string>(),
                    cpDataCSV[i][4].Convert<string>(),
                    cpDataCSV[i][5].Convert<int>(),
                    cpDataCSV[i][6].Convert<int>(),
                    cpDataCSV[i][7].Convert<string>(),
                    cpDataCSV[i][8].Convert<string>()
                );
            
                ChapterTableDatas.Add(id,tabData);
            }

            initNumber--;
        });
    }

    public ChapterTabData GetCpData(int cpId)
    {
        return ChapterTableDatas[cpId];
    }

    public Dictionary<int, ChapterTabData> GetAllCpData()
    {
        return ChapterTableDatas;
    }

    public void InitEventData()
    {
        initNumber++;
        ResourcesManager.Inst.GetAsset<TextAsset>(EventDataCSVPath, delegate(TextAsset infoText)
        {
            if (infoText == null)
            {
                return;
            }
            var cpDataCSV = Document.Load(infoText.text);
            for (int i = 1; i < cpDataCSV.Count; i++)
            {
                if (cpDataCSV[i].Count < 5)
                {
                    continue;
                }
                int id = cpDataCSV[i][0].Convert<int>();
                EventData tabData = new EventData(
                    id,
                    cpDataCSV[i][1].Convert<string>(),
                    cpDataCSV[i][2].Convert<string>(),
                    cpDataCSV[i][3].Convert<string>(),
                    cpDataCSV[i][4].Convert<string>(),
                    cpDataCSV[i][5].Convert<int>()
                );
            
                EventDatas.Add(id,tabData);
            }

            initNumber--;
        });
    }

}