using System.Collections;
using System.Collections.Generic;
using FlexFramework.Excel;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class SqlDataAnalysis : OdinEditorWindow
{
    [MenuItem("GameTools/SQL 数据分析")]
    private static void OpenWindow()
    {
        GetWindow<SqlDataAnalysis>().Show();
    }

    #region 物品分析

    [LabelText("物品分析")]
    public TextAsset GameOverItemText;

    List<SQL_GameOverItemData> SQL_GameOverItemDatas = new List<SQL_GameOverItemData>();
    Dictionary<bool,Dictionary<int,int>> IsVictoryItemData = new Dictionary<bool, Dictionary<int, int>>();

    [LabelText("胜利道具排行")][ReadOnly]
    public List<ItemShowTimesData> VictoryItems = new List<ItemShowTimesData>();
    [LabelText("失败道具排行")][ReadOnly]
    public List<ItemShowTimesData> FailureItems = new List<ItemShowTimesData>();

    [Button("分析游戏结束 - 物品信息")]
    public void AnalysisGameOverItem()
    {
        var rcDataCSV = Document.Load(GameOverItemText.text);
        SQL_GameOverItemDatas.Clear();
        IsVictoryItemData.Clear();
        VictoryItems.Clear();
        FailureItems.Clear();
        DataManager.Inst.Init();
        LocalizationManger.Inst.Init();
        for (int i = 1; i < rcDataCSV.Count; i++)
        {
            if (rcDataCSV[i].Count<3)
            {
                continue;
            }
            SQL_GameOverItemData gameOverItemData = new SQL_GameOverItemData();
            gameOverItemData.UserId = rcDataCSV[i][1].Convert<string>();
            gameOverItemData.IsVictory = rcDataCSV[i][2].Convert<string>() == "true";
            string items = rcDataCSV[i][3].Convert<string>();
            var itemIdStrs =items.Split('	');
            for (int j = 0; j < itemIdStrs.Length; j++)
            {
                int itemid = 0;
                int.TryParse(itemIdStrs[j],out itemid);
                gameOverItemData.ItemIds.Add(itemid);
            }
            SQL_GameOverItemDatas.Add(gameOverItemData);
            if (!IsVictoryItemData.ContainsKey(gameOverItemData.IsVictory))
            {
                IsVictoryItemData.Add(gameOverItemData.IsVictory,new Dictionary<int, int>());
            }

            foreach (int itemId in gameOverItemData.ItemIds)
            {
                if (!IsVictoryItemData[gameOverItemData.IsVictory].ContainsKey(itemId))
                {
                    IsVictoryItemData[gameOverItemData.IsVictory].Add(itemId,0);
                }
                IsVictoryItemData[gameOverItemData.IsVictory][itemId]++;
            }
        }

        foreach (KeyValuePair<int,int> itemTimes in IsVictoryItemData[true])
        {
            ItemShowTimesData showTimesData = new ItemShowTimesData();
            showTimesData.ItemId = itemTimes.Key;
            showTimesData.Times = itemTimes.Value;
            var itemScrObj = DataManager.Inst.GetItemScrObj(showTimesData.ItemId);
            showTimesData.ItemName = LocalizationManger.Inst.GetText(itemScrObj?itemScrObj.Name:"NULL");
            if (itemScrObj!=null && itemScrObj.ItemType != ItemType.Artifact)
            {
                continue;
            }
            VictoryItems.Add(showTimesData);
        }
        foreach (KeyValuePair<int,int> itemTimes in IsVictoryItemData[false])
        {
            ItemShowTimesData showTimesData = new ItemShowTimesData();
            showTimesData.ItemId = itemTimes.Key;
            showTimesData.Times = itemTimes.Value;
            var itemScrObj = DataManager.Inst.GetItemScrObj(showTimesData.ItemId);
            showTimesData.ItemName = LocalizationManger.Inst.GetText(itemScrObj?itemScrObj.Name:"NULL");
            if (itemScrObj!=null && itemScrObj.ItemType != ItemType.Artifact)
            {
                continue;
            }
            FailureItems.Add(showTimesData);
        }
        
        VictoryItems.Sort((lData, rData) =>
        {
            if (lData.Times > rData.Times)
            {
                return -1;
            }
            else if (lData.Times == rData.Times)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        });
        FailureItems.Sort((lData, rData) =>
        {
            if (lData.Times > rData.Times)
            {
                return -1;
            }
            else if (lData.Times == rData.Times)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        });
        
    }

    #endregion
    
    
    
    
    
}

public class ItemShowTimesData
{
    [LabelText("物品ID")]
    public int ItemId;
    [LabelText("出现次数")]
    public int Times;
    [LabelText("物品名字")]
    public string ItemName;
}
public class SQL_GameOverItemData
{
    public string UserId;
    public bool IsVictory;
    public List<int> ItemIds = new List<int>();
}