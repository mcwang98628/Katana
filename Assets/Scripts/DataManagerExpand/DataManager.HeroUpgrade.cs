using System;
using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    Dictionary<int,Dictionary<int,Dictionary<int,HeroUpgradeData>>> HeroUpgradeDatas = new Dictionary<int, Dictionary<int, Dictionary<int, HeroUpgradeData>>>();
    public Dictionary<int,HeroColorLevelData> HeroColorLevelDatas = new Dictionary<int, HeroColorLevelData>();
    private string HeroUpgradePath = "Assets/AssetsPackage/Table/CSV_HeroUpgrade.csv";
    private string HeroUpgradeColorPath = "Assets/AssetsPackage/Table/CSV_HeroUpgradeColor.csv";

    public int GetHeroLevelCountByColorLevel(int heroId,int colorLevel)
    {
        if (HeroUpgradeDatas.ContainsKey(heroId) &&
            HeroUpgradeDatas[heroId].ContainsKey(colorLevel))
        {
            return HeroUpgradeDatas[heroId][colorLevel].Count-1;
        }

        return -1;
    }
    
    public HeroUpgradeData? GetHeroUpgradeData(int heroId,int colorLevel,int level)
    {
        if (HeroUpgradeDatas.ContainsKey(heroId) &&
            HeroUpgradeDatas[heroId].ContainsKey(colorLevel) &&
            HeroUpgradeDatas[heroId][colorLevel].ContainsKey(level))
        {
            return HeroUpgradeDatas[heroId][colorLevel][level];
        }

        return null;
    }

    /// <summary>
    /// 获取指定等级的skill介绍
    /// </summary>
    public HeroUpgradeData? GetHeroSkillDesc(int heroId,int colorLevel,int level)
    {
        var heroUpgradeData = GetHeroUpgradeData(heroId, colorLevel, level);
        if (heroUpgradeData != null && heroUpgradeData.Value.SkillId > 0)
            return heroUpgradeData.Value;

        return null;
    }

    /// <summary>
    /// 获取当前等级和所有之前的等级的skill介绍
    /// </summary>
    public List<HeroUpgradeData> GetHeroTargetAllSkillDesc(int heroId,int colorLevel,int level)
    {
        List<HeroUpgradeData> heroUpgradeDatas = new List<HeroUpgradeData>();

        for (int i = colorLevel; i > 0; i--)
        {
            if (i<colorLevel)
                level = HeroUpgradeDatas[heroId][colorLevel].Count - 1;
            
            for (int j = level; j >= 0; j--)
            {
                var heroUpgradeData = GetHeroSkillDesc(heroId, i, j);
                if (heroUpgradeData != null)
                {
                    heroUpgradeDatas.Add(heroUpgradeData.Value);
                }
            }
        }
        
        return heroUpgradeDatas;
    }

    void InitHeroUpgradeData()
    {
        initNumber++;
        ResourcesManager.Inst.GetAsset<TextAsset>(HeroUpgradePath, delegate(TextAsset asset)
        {
            TextAsset infoText = asset;
            
            var infoDatas = Document.Load(infoText.text);
            for (int i = 1; i < infoDatas.Count; i++)
            {
                if (infoDatas[i].Count <= 2)
                    continue;
                
                var itemIdStrs = infoDatas[i][6].ToString().Split('-');
                List<int> itemIds = new List<int>();
                foreach (string itemIdStr in itemIdStrs)
                {
                    itemIds.Add(int.Parse(itemIdStr));
                }

                int skillId;
                string idStr = infoDatas[i][8].Convert<string>();
                int.TryParse(idStr,out skillId);
                HeroUpgradeData heroUpgradeData = new HeroUpgradeData()
                {
                    HeroId = infoDatas[i][0].Convert<int>(),
                    ColorLevel = infoDatas[i][1].Convert<int>(),
                    Level = infoDatas[i][2].Convert<int>(),
                    NeedExp = infoDatas[i][3].Convert<int>(),
                    MaxHp = infoDatas[i][4].Convert<int>(),
                    AttackPower = infoDatas[i][5].Convert<int>(),
                    ItemIds = itemIds,
                    AoeMagnification = infoDatas[i][7].Convert<float>(),
                    SkillId = skillId,
                    SkillDesc = infoDatas[i][9].Convert<string>(),
                };
                AddHeroUpgradeData(ref heroUpgradeData);
            }

            initNumber--;
        });
        
    }
    void InitHeroUpgradeColorData()
    {
        initNumber++;
        ResourcesManager.Inst.GetAsset<TextAsset>(HeroUpgradeColorPath, delegate(TextAsset asset)
        {
            TextAsset infoText = asset;
            
            var infoDatas = Document.Load(infoText.text);
            for (int i = 1; i < infoDatas.Count; i++)
            {
                if (infoDatas[i].Count < 2)
                    continue;

                int colorLevel = infoDatas[i][0].Convert<int>();
                var colorStrs = infoDatas[i][1].Convert<string>().Split('-');
                var desc = infoDatas[i][2].Convert<string>();
                float r = float.Parse(colorStrs[0]);
                float g = float.Parse(colorStrs[1]);
                float b = float.Parse(colorStrs[2]);
                Color color = new Color(r,g,b);
                HeroColorLevelDatas.Add(colorLevel,new HeroColorLevelData()
                {
                    ColorLevel = colorLevel,
                    Color = color,
                    Desc = desc
                });
            }

            initNumber--;
        });
        
    }

    void AddHeroUpgradeData(ref HeroUpgradeData heroUpgradeData)
    {
        if (!HeroUpgradeDatas.ContainsKey(heroUpgradeData.HeroId))
            HeroUpgradeDatas.Add(heroUpgradeData.HeroId,new Dictionary<int, Dictionary<int, HeroUpgradeData>>());
        if (!HeroUpgradeDatas[heroUpgradeData.HeroId].ContainsKey(heroUpgradeData.ColorLevel))
            HeroUpgradeDatas[heroUpgradeData.HeroId].Add(heroUpgradeData.ColorLevel,new Dictionary<int, HeroUpgradeData>());
        if (!HeroUpgradeDatas[heroUpgradeData.HeroId][heroUpgradeData.ColorLevel].ContainsKey(heroUpgradeData.Level))
            HeroUpgradeDatas[heroUpgradeData.HeroId][heroUpgradeData.ColorLevel].Add(heroUpgradeData.Level,heroUpgradeData);
    }
    
}

[Serializable]
public struct HeroColorLevelData
{
    public int ColorLevel;
    public Color Color;
    public string Desc;
}
[Serializable]
public struct HeroUpgradeData
{
    public int HeroId;
    public int ColorLevel;//颜色，品质等级
    public int Level;//星等级
    public int NeedExp;//升级到下一级所需到经验
    public int MaxHp;
    public int AttackPower;
    public List<int> ItemIds;
    public float AoeMagnification;//AOE法术增益
    public int SkillId;
    public string SkillDesc;
}