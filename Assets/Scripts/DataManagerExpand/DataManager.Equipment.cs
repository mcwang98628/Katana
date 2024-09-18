


using System;
using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private string EquipmentInfoPath = "Assets/AssetsPackage/Table/CSV_Equipment.csv";
    private string EquipmentScorePath = "Assets/AssetsPackage/Table/CSV_EquipmentScore.csv";


    public Dictionary<int, EquipmentInfo> EquipmentInfos = new Dictionary<int, EquipmentInfo>();

    public void LoadEquipment()
    {
        LoadEquipmentInfo();
        LoadEquipmentScore();


        
        
    }
    
    private void LoadEquipmentInfo()
    {
        ResourcesManager.Inst.GetAsset<TextAsset>(EquipmentInfoPath, delegate(TextAsset infoText)
        {
            var dataStr = Document.Load(infoText.text);

            for (int i = 1; i < dataStr.Count; i++)
            {
                if (dataStr[i].Count<2)
                    continue;
                
                EquipmentInfo equipmentInfo = new EquipmentInfo();
                equipmentInfo.Id = dataStr[i][0].Convert<int>();
                equipmentInfo.EquipmentType = (EquipmentType)Enum.Parse(typeof(EquipmentType),dataStr[i][1].Convert<string>());
                equipmentInfo.IconStr = dataStr[i][2].Convert<string>();
                equipmentInfo.Name = dataStr[i][3].Convert<string>();
                equipmentInfo.Desc = dataStr[i][4].Convert<string>();
                equipmentInfo.FirstEffect = (EquipmentEffectType)Enum.Parse(typeof(EquipmentEffectType),dataStr[i][5].Convert<string>());
                EquipmentInfos.Add(equipmentInfo.Id, equipmentInfo);
            }
        });
    }

    public Dictionary<EquipmentEffectType, List<EquipmentEffect>> EquipmentEffectList =
        new Dictionary<EquipmentEffectType, List<EquipmentEffect>>(); 

    public Dictionary<EquipmentQuality, List<EquipmentEffect>> EquipmentEffectListByQuality =
        new Dictionary<EquipmentQuality, List<EquipmentEffect>>(); 
        
    public void LoadEquipmentScore()
    {
        ResourcesManager.Inst.GetAsset<TextAsset>(EquipmentScorePath, delegate(TextAsset infoText)
        {
            var dataStr = Document.Load(infoText.text);

            for (int i = 1; i < dataStr.Count; i++)
            {
                if (dataStr[i].Count<2)
                    continue;
                
                for (int j = 2; j < dataStr[0].Count; j++)
                {
                    EquipmentEffect equipmentEffect = new EquipmentEffect();
                    equipmentEffect.EffectType = (EquipmentEffectType) Enum.Parse(typeof(EquipmentEffectType), dataStr[i][0].Convert<string>());
                    equipmentEffect.Score = dataStr[0][j].Convert<int>();
                    equipmentEffect.Value = dataStr[i][j].Convert<float>();
                    equipmentEffect.Quality = (EquipmentQuality)Enum.Parse(typeof(EquipmentQuality), dataStr[i][1].Convert<string>());
                    if (!EquipmentEffectList.ContainsKey(equipmentEffect.EffectType))
                        EquipmentEffectList.Add(equipmentEffect.EffectType,new List<EquipmentEffect>());
                    
                    EquipmentEffectList[equipmentEffect.EffectType].Add(equipmentEffect);

                    if (!EquipmentEffectListByQuality.ContainsKey(equipmentEffect.Quality))
                        EquipmentEffectListByQuality.Add(equipmentEffect.Quality,new List<EquipmentEffect>());
                    EquipmentEffectListByQuality[equipmentEffect.Quality].Add(equipmentEffect);
                }
            }
        });
    }
    
}

public class EquipmentInfo
{
    public int Id;
    public EquipmentType EquipmentType;
    public string IconStr;
    public string Name;
    public string Desc;
    public EquipmentEffectType FirstEffect;
}

public class EquipmentEffect
{
    public EquipmentEffectType EffectType;
    public EquipmentQuality Quality;
    public int Score;
    public float Value;
}

