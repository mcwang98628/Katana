using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private const string expLevelTablePath = "Assets/AssetsPackage/Table/CSV_ExpLevel.csv";

    public Dictionary<int,ExpLevelData> ExpLevelData { get; private set; }

    void InitExpLevelTable()
    {
        initNumber++;
        ExpLevelData = new Dictionary<int, ExpLevelData>();
        
        ResourcesManager.Inst.GetAsset<TextAsset>(expLevelTablePath, delegate(TextAsset infoText)
        {
            var infoDatas = Document.Load(infoText.text);
            for (int i = 1; i < infoDatas.Count; i++)
            {
                int id = infoDatas[i][0].Convert<int>();
                ExpLevelData.Add(id, new ExpLevelData(
                    id,
                    infoDatas[i][1].Convert<int>(),
                    infoDatas[i][2].Convert<int>(),
                    infoDatas[i][3].Convert<int>()
                ));
            }

            initNumber--;
        });
        
    }
    
    
}