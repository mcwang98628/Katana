using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private string DifficultyCSVPath = "Assets/AssetsPackage/Table/CSV_Difficulty.csv";
    
    
    Dictionary<int,DifficultyData> _difficultyDatas = new Dictionary<int, DifficultyData>();
    private int _maxDifficultyLevel;
    
    
    void InitDifficultyCSV()
    {
        initNumber++;
        _difficultyDatas.Clear();
        _maxDifficultyLevel = 0;
        ResourcesManager.Inst.GetAsset<TextAsset>(DifficultyCSVPath, delegate(TextAsset infoText)
        {
            var infoDatas = Document.Load(infoText.text);
            for (int i = 1; i < infoDatas.Count; i++)
            {
                if (infoDatas[i].Count < 2)
                {
                    continue;
                }
                DifficultyData difficultyData = new DifficultyData()
                {
                    Level = infoDatas[i][0].Convert<int>(),
                    HpPercentage = infoDatas[i][1].Convert<float>(),
                    AttackPowerPercentage = infoDatas[i][2].Convert<float>(),
                    EnemyCount = infoDatas[i][3].Convert<float>(),
                    DefenseLevel = infoDatas[i][4].Convert<int>(),
                    EnhancedEnemyProbability = infoDatas[i][5].Convert<int>(),
                    Desc = infoDatas[i][6].Convert<string>()
                };
                _difficultyDatas.Add(difficultyData.Level,difficultyData);
                if (difficultyData.Level > _maxDifficultyLevel)
                {
                    _maxDifficultyLevel = difficultyData.Level;
                }
            }

            initNumber--;
        });
    }

    public DifficultyData GetDifficultyDataByLevel(int level)
    {
        if (level > _maxDifficultyLevel)
            level = _maxDifficultyLevel;
        return _difficultyDatas[level];
    }


}