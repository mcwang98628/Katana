using System;
using System.Collections.Generic;
using UnityEngine;


//装备品质
public enum EquipmentQuality
{
    Lv1 = 1,
    Lv2 = 2,
    Lv3 = 3,
    Lv4 = 4,
}

//装备类型
public enum EquipmentType
{
    Ring = 1,//戒指
    Crown = 2,//头冠
    Belt = 3,//腰带
}
public enum EquipmentEffectType
{
    AttackPower = 1,
    AttackSpeed = 2,
    MoveSpeed = 3,
    MaxHp = 4,
    CriticalProbability = 5,//暴击概率
    CriticalMultiplier = 6,//暴击倍率
    DodgeProbability = 7,//闪避概率
    Gold = 8,//开局金币
    HpTreatMultiplier = 9,//治疗倍率
    Resurrection = 10,//复活
    Vampire = 11,//吸血
    ThornsArmor =12,//反伤甲
}

[Serializable]
public class Equipment
{
    public int Id;
    public string Guid;
    public string Icon;
    public string Name;
    public string Desc;
    public int Score;
    public EquipmentQuality Quality;
    public EquipmentType EquipmentType;
    public List<EquipmentEffectType> EffectList = new List<EquipmentEffectType>();

    public void LoadIcon(Action<Sprite> callBack)
    {
        // Debug.LogError($"Assets/AssetsPackage/EquipIcon/{Icon}");
        ResourcesManager.Inst.GetAsset<Sprite>($"Assets/AssetsPackage/EquipIcon/{Icon}", delegate(Sprite sprite)
        {
            callBack?.Invoke(sprite);
        });
    }

    public float GetEffectValue(EquipmentEffectType effectType,EquipmentQuality qualityLv)
    {
        float value = 0;
        if (!DataManager.Inst.EquipmentEffectList.ContainsKey(effectType))
        {
            Debug.LogError("Error" + effectType);
            return 0;
        }

        EquipmentEffect effect = null;
        EquipmentEffect currentEffect = null;
        foreach (var scoreData in DataManager.Inst.EquipmentEffectList[effectType])
        {
            if (scoreData.Quality != qualityLv)
                continue;
            
            if (scoreData.Score < Score)
            {
                effect = scoreData;
            }
            else
            {
                currentEffect = scoreData;
                break;
            }
        }

        if (effect == null)
            effect = currentEffect;

        int diff = Score - effect.Score;
        if (diff < 0)
            diff = 0;
        
        int diff2 = currentEffect.Score - effect.Score;
        value = Mathf.Lerp(effect.Value, currentEffect.Value, diff==diff2?1:(float)diff / (float)diff2);
        
        return value;
    }
    
    public 
    string GetEffectStr(EquipmentEffectType effectType,EquipmentQuality qualityLv)
    {
        var effectValue = GetEffectValue(effectType, qualityLv);
        switch (effectType)
        {
            case EquipmentEffectType.AttackPower:
                return $"{LocalizationManger.Inst.GetText("Attack")}  {(int)effectValue}";
                break;
            case EquipmentEffectType.AttackSpeed:
                effectValue *= 100;
                return $"{LocalizationManger.Inst.GetText("AttackSpeed")} {(int)effectValue}";
                break;
            case EquipmentEffectType.MoveSpeed:
                return $"{LocalizationManger.Inst.GetText("MoveSpeed")} {effectValue.ToString("0.0")}";
                break;
            case EquipmentEffectType.MaxHp:
                return $"{LocalizationManger.Inst.GetText("MaxHp")} {(int)effectValue}";
                break;
            case EquipmentEffectType.CriticalProbability:
                effectValue *= 100;
                return $"{LocalizationManger.Inst.GetText("CritRate")} {(int)effectValue}%";
                break;
            case EquipmentEffectType.CriticalMultiplier:
                return $"{LocalizationManger.Inst.GetText("CritMul")} {effectValue.ToString("F")}";
                break;
            case EquipmentEffectType.DodgeProbability:
                effectValue *= 100;
                return $"{LocalizationManger.Inst.GetText("AvoidRate")} {(int)effectValue}%";
                break;
            case EquipmentEffectType.Resurrection:
                return $"{LocalizationManger.Inst.GetText("Resurrection")} {(int)effectValue}";
                break;
            case EquipmentEffectType.Gold:
                return $"{LocalizationManger.Inst.GetText("AddGoldAtStart")} {(int)effectValue} ";
                break;
            case EquipmentEffectType.HpTreatMultiplier:
                return $"{LocalizationManger.Inst.GetText("AddHpTreat")}{(float) effectValue}";
                break;
            case EquipmentEffectType.ThornsArmor:
                return $"{LocalizationManger.Inst.GetText("ThornsArmor")}{(float)effectValue}";
                break;
            case EquipmentEffectType.Vampire:
                return $"{LocalizationManger.Inst.GetText("Vampire")}{(float)effectValue}";
                break;
            default:
                return effectType.ToString();
                break;
        }

        return "";
    }

}




