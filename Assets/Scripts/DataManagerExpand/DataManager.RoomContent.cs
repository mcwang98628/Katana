using System;
using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private string RoomContentCSVPath = "Assets/AssetsPackage/Table/CSV_RoomContent.csv";
        
    Dictionary<int,ConditionInfo> ConditionInfos = new Dictionary<int, ConditionInfo>();
    Dictionary<RoomType,List<ConditionInfo>> ConditionInfosOfRoomType = new Dictionary<RoomType, List<ConditionInfo>>();
    void InitRoomContentData()
    {
        initNumber++;
        ResourcesManager.Inst.GetAsset<TextAsset>(RoomContentCSVPath, delegate(TextAsset infoText)
        {
            var rcDataCSV = Document.Load(infoText.text);
            for (int i = 1; i < rcDataCSV.Count; i++)
            {
                int id = rcDataCSV[i][0].Convert<int>();
                string roomName = rcDataCSV[i][1].ToString();
                RoomType roomType = (RoomType)Enum.Parse(typeof(RoomType),rcDataCSV[i][2].ToString(),true);
                string contentFileName = rcDataCSV[i][3];
                ConditionType conditionType = (ConditionType)Enum.Parse(typeof(ConditionType),rcDataCSV[i][4].ToString(),true);
                string value =  rcDataCSV[i][5];
                bool isonly = rcDataCSV[i][6].Convert<bool>();
                ConditionInfo conditionInfo = new ConditionInfo();
                conditionInfo.Id = id;
                conditionInfo.RoomName = roomName;
                conditionInfo.RoomType = roomType;
                conditionInfo.ContentFileName = contentFileName;
                conditionInfo.ConditionType = conditionType;
                conditionInfo.ConditionValue = value;
                conditionInfo.IsOnly = isonly;
                ConditionInfos.Add(id,conditionInfo);
                if (!ConditionInfosOfRoomType.ContainsKey(roomType))
                {
                    ConditionInfosOfRoomType.Add(roomType,new List<ConditionInfo>());
                }
                ConditionInfosOfRoomType[roomType].Add(conditionInfo);
            }

            initNumber--;
        });
    }

    public List<ConditionInfo> GetConditionInfos(RoomType type)
    {
        if (!ConditionInfosOfRoomType.ContainsKey(type))
        {
            Debug.LogError("Err!");
            return null;
        }
        return ConditionInfosOfRoomType[type];
    }
    public ConditionInfo GetConditionInfo(int id)
    {
        return ConditionInfos[id];
    }
    
}