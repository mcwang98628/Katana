using System;
using System.Linq;
using Random = UnityEngine.Random;

public class EquipmentTool
{
    public static Equipment RandomScoreEquipment()
    {
        int score = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150.GetScore();
        int scoreOffset = Random.Range(-5, 50);
        score += scoreOffset;
        switch (BattleManager.Inst.RuntimeData.CurrentChapterId)
        {
            case 0:
                score = 20;
                break;
            case 1:
                if (score<20)
                    score = 20;
                if (score>100)
                    score = 100;
                break;
            case 2:
                if (score<100)
                    score = 100;
                if (score>200)
                    score = 200;
                break;
            case 3:
                if (score<200)
                    score = 200;
                if (score>300)
                    score = 300;
                break;
            case 4:
                if (score<250)
                    score = 250;
                if (score>400)
                    score = 400;
                break;
            case 5:
                if (score<350)
                    score = 350;
                if (score>500)
                    score = 500;
                break;
            case 6:
                if (score<500)
                    score = 500;
                if (score>600)
                    score = 600;
                break;
        }
        var qualityArr = Enum.GetNames(typeof(EquipmentQuality));
        EquipmentQuality equipmentQuality = (EquipmentQuality) Enum.Parse(typeof(EquipmentQuality), qualityArr[Random.Range(0, qualityArr.Length)]);
        Equipment equipment = RandomEquipment(score, equipmentQuality);
        return equipment;
    }
    public static Equipment RandomScoreEquipmentById(int id)
    {
        int score = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150.GetScore();
        int scoreOffset = Random.Range(-5, 50);
        score += scoreOffset;
        switch (BattleManager.Inst.RuntimeData.CurrentChapterId)
        {
            case 0:
                score = 20;
                break;
            case 1:
                if (score<20)
                    score = 20;
                if (score>100)
                    score = 100;
                break;
            case 2:
                if (score<100)
                    score = 100;
                if (score>200)
                    score = 200;
                break;
            case 3:
                if (score<200)
                    score = 200;
                if (score>300)
                    score = 300;
                break;
            case 4:
                if (score<250)
                    score = 250;
                if (score>400)
                    score = 400;
                break;
            case 5:
                if (score<350)
                    score = 350;
                if (score>500)
                    score = 500;
                break;
            case 6:
                if (score<500)
                    score = 500;
                if (score>600)
                    score = 600;
                break;
        }
        var qualityArr = Enum.GetNames(typeof(EquipmentQuality));
        EquipmentQuality equipmentQuality = (EquipmentQuality) Enum.Parse(typeof(EquipmentQuality), qualityArr[Random.Range(0, qualityArr.Length)]);
        Equipment equipment = RandomEquipment(score, equipmentQuality,id);
        return equipment;
    }
    
    
    public static Equipment RandomEquipment(int score,EquipmentQuality quality,int id = -1)
    {
        if (score < 20)
            score = 20;
        
        Equipment newEquipment = new Equipment();
        newEquipment.Score = score;
        newEquipment.Quality = quality;
        newEquipment.Guid = GuidTools.GetGUID();

        EquipmentInfo equip = null;
        if (id < 0)
        {
            var equipList = DataManager.Inst.EquipmentInfos.Values.ToList();
            equip = equipList[Random.Range(0, equipList.Count)];
        }
        else
        {
            equip = DataManager.Inst.EquipmentInfos[id];
        }
        newEquipment.Id = equip.Id;
        newEquipment.Name = equip.Name;
        newEquipment.Desc = equip.Desc;
        newEquipment.Icon = equip.IconStr;
        newEquipment.EquipmentType = equip.EquipmentType;
        newEquipment.EffectList.Add(equip.FirstEffect);

        var lv2EffectList = DataManager.Inst.EquipmentEffectListByQuality[EquipmentQuality.Lv2];
        var lv3EffectList = DataManager.Inst.EquipmentEffectListByQuality[EquipmentQuality.Lv3];
        var lv4EffectList = DataManager.Inst.EquipmentEffectListByQuality[EquipmentQuality.Lv4];
        EquipmentEffectType lv2Effect = lv2EffectList[Random.Range(0, lv2EffectList.Count)].EffectType;
        EquipmentEffectType lv3Effect = lv3EffectList[Random.Range(0, lv3EffectList.Count)].EffectType;
        EquipmentEffectType lv4Effect = lv4EffectList[Random.Range(0, lv4EffectList.Count)].EffectType;
        
        switch (quality)
        {
            case EquipmentQuality.Lv1:
                break;
            case EquipmentQuality.Lv2:
                newEquipment.EffectList.Add(lv2Effect);
                break;
            case EquipmentQuality.Lv3:
                newEquipment.EffectList.Add(lv2Effect);
                newEquipment.EffectList.Add(lv3Effect);
                break;
            case EquipmentQuality.Lv4:
                newEquipment.EffectList.Add(lv2Effect);
                newEquipment.EffectList.Add(lv3Effect);
                newEquipment.EffectList.Add(lv4Effect);
                break;
        }
        
        return newEquipment;
    }
}