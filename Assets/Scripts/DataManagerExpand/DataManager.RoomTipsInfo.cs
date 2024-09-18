using System;
using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private string RoomTipsInfoPath = "Assets/AssetsPackage/Table/CSV_RoomTipsInfo.csv";
        

    public Dictionary<RoomType,List<RoomTipsInfo>> RoomTipsInfos = new Dictionary<RoomType, List<RoomTipsInfo>>();
    void InitRoomTipsInfoData()
    {
        initNumber++;
        RoomTipsInfos.Clear();
        ResourcesManager.Inst.GetAsset<TextAsset>(RoomTipsInfoPath, delegate(TextAsset infoText)
        {
            var rcDataCSV = Document.Load(infoText.text);
            for (int i = 1; i < rcDataCSV.Count; i++)
            {
                RoomTipsInfo rti = new RoomTipsInfo();
                string roomType = rcDataCSV[i][0].Convert<string>();
                string str = rcDataCSV[i][1].Convert<string>();
                int levelIndex = rcDataCSV[i][2].Convert<int>();
            
                rti.CurrentRoomType = (RoomType)Enum.Parse(typeof(RoomType), roomType, true);
                rti.Text = str;
                rti.LevelIndex = levelIndex;
                if (!RoomTipsInfos.ContainsKey(rti.CurrentRoomType))
                {
                    RoomTipsInfos.Add(rti.CurrentRoomType,new List<RoomTipsInfo>());
                }
                RoomTipsInfos[rti.CurrentRoomType].Add(rti);
            }

            initNumber--;
        });
    }


}