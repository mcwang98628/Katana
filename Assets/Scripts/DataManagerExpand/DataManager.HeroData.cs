using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private string HeroDataPath = "Assets/AssetsPackage/Table/CSV_HeroData.csv";
    public Dictionary<int, HeroData> HeroDatas => _heroDatas;
    Dictionary<int,HeroData> _heroDatas = new Dictionary<int, HeroData>();

    void InitHeroData()
    {
        initNumber++;
        ResourcesManager.Inst.GetAsset<TextAsset>(HeroDataPath, delegate(TextAsset asset)
        {
            TextAsset infoText = asset;
            
            var infoDatas = Document.Load(infoText.text);
            for (int i = 1; i < infoDatas.Count; i++)
            {
                if ( infoDatas[i].Count < 3)
                {
                    continue;
                }
                int id = infoDatas[i][0].Convert<int>();
                string itemIdsStr = infoDatas[i][3].Convert<string>();
                List<int> itemIdsList = new List<int>();
                string[] itemIdsStrArrar = itemIdsStr.Split('-');
                for (int j = 0; j < itemIdsStrArrar.Length; j++)
                {
                    if (string.IsNullOrEmpty(itemIdsStrArrar[j]))
                    {
                        continue;
                    }
                    itemIdsList.Add(int.Parse(itemIdsStrArrar[j]));
                }

                var colorStrs = infoDatas[i][17].Convert<string>().Split('-');
                var color = new Color(float.Parse(colorStrs[0]),float.Parse(colorStrs[1]),float.Parse(colorStrs[2]));
                var bgcolorStrs = infoDatas[i][18].Convert<string>().Split('-');
                var bgcolor = new Color(float.Parse(bgcolorStrs[0]),float.Parse(bgcolorStrs[1]),float.Parse(bgcolorStrs[2]));
            
                _heroDatas.Add(id,new HeroData()
                {
                    HeroId = id,
                    // MaxHp = infoDatas[i][1].Convert<int>(),
                    // AttackPower = infoDatas[i][2].Convert<int>(),
                    // Items = itemIdsList,
                    Price = infoDatas[i][4].Convert<int>(),
                    HeroPrefabName = infoDatas[i][5].Convert<string>(),
                    FeedBackName = infoDatas[i][6].Convert<string>(),
                    HeroName = infoDatas[i][7].Convert<string>(),
                    HeroDesc = infoDatas[i][8].Convert<string>(),
                    HeroIcon = infoDatas[i][9].Convert<string>(),
                    ProfessionName = infoDatas[i][10].Convert<string>(),
                    SKill1Icon = infoDatas[i][11].Convert<string>(),
                    SKill1Name = infoDatas[i][12].Convert<string>(),
                    SKill1Desc = infoDatas[i][13].Convert<string>(),
                    SKill2Icon = infoDatas[i][14].Convert<string>(),
                    SKill2Name = infoDatas[i][15].Convert<string>(),
                    SKill2Desc = infoDatas[i][16].Convert<string>(),
                    Color = color,
                    BgColor = bgcolor,
                });
            }

            initNumber--;
        });
        
    }
    
    public HeroData GetHeroData(int heroId)
    {
        return _heroDatas[heroId];
    }


}