using System;
using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private readonly string _missionDataPath = "Assets/AssetsPackage/Table/CSV_MissionData.csv";

    public Dictionary<int, MissionTabData> MissionTabDatas = new Dictionary<int, MissionTabData>();
    
    private void InitMissionData()
    {
        initNumber++;
        ResourcesManager.Inst.GetAsset<TextAsset>(_missionDataPath, delegate(TextAsset infoText)
        {
            var doc = Document.Load(infoText.text);

            for (int i = 1; i < doc.Count; i++)
            {
                MissionTabData tabData = new MissionTabData(
                    doc[i][0].Convert<int>(),
                    doc[i][1],
                    (MissionType)Enum.Parse(typeof(MissionType),doc[i][2]),
                    (MissionTargetType)Enum.Parse(typeof(MissionTargetType),doc[i][3]),
                    doc[i][4].Convert<int>(),
                    doc[i][5].Convert<int>(),
                    (MissionRewardType)Enum.Parse(typeof(MissionRewardType),doc[i][6]),
                    doc[i][7].Convert<int>()
                );
            
                MissionTabDatas.Add(tabData.Id,tabData);
            }

            initNumber--;
        });
        
    }
    
}