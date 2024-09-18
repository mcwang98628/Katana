using System;
using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private string EnemyInfoPath = "Assets/AssetsPackage/Table/CSV_EnemyInfo.csv";
    private string EnemyDiffPath = "Assets/AssetsPackage/Table/EnemyDatas/{0}.csv";
    public Dictionary<int, EnemyInfoData> EnemyDatas { get; private set; }
    void InitEnemyInfo()
    {
        initNumber++;
        EnemyDatas = new Dictionary<int, EnemyInfoData>();
        ResourcesManager.Inst.GetAsset<TextAsset>(EnemyInfoPath, delegate(TextAsset infoText)
        {
            var enemyData = Document.Load(infoText.text);
            for (int i = 1; i < enemyData.Count; i++)
            {
                EnemyInfoData infoData = new EnemyInfoData();
                infoData.EnemyID = enemyData[i][0].Convert<int>();
                infoData.EnemyName = enemyData[i][1].String;
                infoData.EnemyIcon = enemyData[i][2].String;
                infoData.EnemyPrefabName = enemyData[i][3].String;
                infoData.EnemyTalk = enemyData[i][4].String;
                infoData.EnemyType = (EnemyType)Enum.Parse(typeof(EnemyType),enemyData[i][5].String);
                infoData.EnemyWeight = enemyData[i][6].Convert<int>();
                infoData.ShowInAtlas = enemyData[i][7].Convert<string>().ToLower() == "true";
                EnemyDatas.Add(infoData.EnemyID, infoData);

                LoadEnemyDiff(infoData.EnemyName,infoData.EnemyID);
            }

            initNumber--;
        });
    }
    public EnemyInfoData GetEnemyInfo(int id)
    {
        if (!EnemyDatas.ContainsKey(id))
        { 
            Debug.LogError("表里找不到怪物：ID="+id);
            return null;
        }

        return EnemyDatas[id];
    }
    public EnemyInfoData GetEnemyInfo(string prefabName)
    {
        foreach (var enemy in EnemyDatas)
        {
            if(enemy.Value.EnemyPrefabName == prefabName)
            return enemy.Value;
        }
        Debug.LogError("表里找不到怪物："+prefabName);
        return null;
    }


    private Dictionary<int, Dictionary<int, Dictionary<int, EnemyDiffData>>> enemyDiffDatas =
        new Dictionary<int, Dictionary<int, Dictionary<int, EnemyDiffData>>>();
    void LoadEnemyDiff(string enemyName,int enemyId)
    {
        initNumber++;
        ResourcesManager.Inst.GetAsset<TextAsset>(string.Format(EnemyDiffPath,enemyName), delegate(TextAsset infoText)
        {
            if (infoText == null || string.IsNullOrEmpty(infoText.text))
            {
                initNumber--;
                Debug.LogError($"没有找到 {enemyName} 表");
                return;
            }
            var enemyDiffCsv = Document.Load(infoText.text);
            for (int i = 1; i < enemyDiffCsv.Count; i++)
            {
                if (enemyDiffCsv[i].Count < 2)
                    continue;
                
                EnemyDiffData enemyDiffData = new EnemyDiffData();
                enemyDiffData.EnemyId = enemyId;
                enemyDiffData.EnemyName = enemyName;
                enemyDiffData.ChapterId = enemyDiffCsv[i][0].Convert<int>();
                enemyDiffData.LevelId = enemyDiffCsv[i][1].Convert<int>();
                enemyDiffData.MaxHp = enemyDiffCsv[i][2].Convert<int>();
                enemyDiffData.AttackPower = enemyDiffCsv[i][3].Convert<int>();

                if (!enemyDiffDatas.ContainsKey(enemyDiffData.EnemyId))
                    enemyDiffDatas.Add(enemyDiffData.EnemyId,new Dictionary<int, Dictionary<int, EnemyDiffData>>());
                if (!enemyDiffDatas[enemyDiffData.EnemyId].ContainsKey(enemyDiffData.ChapterId))
                    enemyDiffDatas[enemyDiffData.EnemyId]
                        .Add(enemyDiffData.ChapterId, new Dictionary<int, EnemyDiffData>());
                enemyDiffDatas[enemyDiffData.EnemyId][enemyDiffData.ChapterId].Add(enemyDiffData.LevelId,enemyDiffData);
            }

            initNumber--;
        });
    }

    public EnemyDiffData GetEnemyDiffData(int enemyId,int cpId,int levelId)
    {
        if (!enemyDiffDatas.ContainsKey(enemyId))
            return null;
        if (!enemyDiffDatas[enemyId].ContainsKey(cpId))
            return null;
        if (!enemyDiffDatas[enemyId][cpId].ContainsKey(levelId))
            return null;
        
        
        return enemyDiffDatas[enemyId][cpId][levelId];
    }
    
}

public class EnemyDiffData
{
    public int EnemyId;
    public string EnemyName;
    public int ChapterId;
    public int LevelId;
    public int MaxHp;
    public int AttackPower;
}