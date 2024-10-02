using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ArchiveData_1_5_0 : IArchiveData
{
    public string Version => "1.5.0";
    
    public TemporaryData_1_4_0 TemporaryData;
    public GlobalData_1_4_0 GlobalData;
    public BattleArchiveData_1_4_0 BattleData;
    public StatisticsData_1_4_0 StatisticsData;
    public SettingArchiveData_1_4_0 SettingArchiveData;
    public EquipmentArchiveData_1_5_0 equipmentArchiveData150;
    public AttributesArchiveData_1_5_0 attributesArchiveData;
    
    public void AddKillEnemyCount(int id,int count)
    {
        if (!StatisticsData.KillEnemys.ContainsKey(id))
        {
            StatisticsData.KillEnemys.Add(id, 0);
        }
        StatisticsData.KillEnemys[id] += count;
    }
    public int GetAtEnemyCount(int id)
    {
        return StatisticsData.KillEnemys[id];
    }

    
    /// <summary>
    /// 存档数据升级
    /// </summary>
    /// <param name="archiveData">1.3.11存档数据</param>
    /// <returns>1.4.0存档数据</returns>
    public static ArchiveData_1_5_0 ArchiveUpgrade(ArchiveData_1_4_0 archiveData)
    {
        ArchiveData_1_5_0 archiveData150 = new ArchiveData_1_5_0();
        archiveData150.TemporaryData = archiveData.TemporaryData;
        archiveData150.GlobalData = archiveData.GlobalData;
        archiveData150.BattleData = archiveData.BattleData;
        archiveData150.StatisticsData = archiveData.StatisticsData;
        archiveData150.SettingArchiveData = archiveData.SettingArchiveData;
        archiveData150.equipmentArchiveData150 = new EquipmentArchiveData_1_5_0();
        archiveData150.attributesArchiveData = new AttributesArchiveData_1_5_0();
        return archiveData150;
    }
    
    
    public static void NewArchiveData(Action<ArchiveData_1_5_0> callBack)
    {
        DataManager.Inst.PeekDefaultUnLockItems(delegate(List<int> ints)
        {
            ArchiveData_1_5_0 archiveData150 = new ArchiveData_1_5_0();
            archiveData150.equipmentArchiveData150 = new EquipmentArchiveData_1_5_0();
            archiveData150.attributesArchiveData = new AttributesArchiveData_1_5_0();
            //临时数据
            archiveData150.TemporaryData = new TemporaryData_1_4_0();
            archiveData150.TemporaryData.UnlockedItems.Clear();
            archiveData150.TemporaryData.UnlockItems.Clear();
            archiveData150.TemporaryData.AddDiamondValue = 0;
            // archiveData150.TemporaryData.AddSoulValue = 0;
            //全局数据
            archiveData150.GlobalData = new GlobalData_1_4_0();
            archiveData150.GlobalData.Diamond = 0;
            archiveData150.GlobalData.Soul = 0;
            archiveData150.GlobalData.Exp = 0;
            archiveData150.GlobalData.Fire = 12;
            archiveData150.GlobalData.MaxFire = 6;
            archiveData150.GlobalData.FireRecoveryTime = -1;
            // archiveData140.GlobalData.OneFireRecoveryNeedTime = 7200;
#if UNITY_ANDROID|| UNITY_IOS
            archiveData150.GlobalData.UnLockHeros.Add(1101);
            // ReSharper disable once PossibleInvalidOperationException
            archiveData150.GlobalData.HeroUpgradeDatas.Add(1101,new HeroUpgradeInfo());
            archiveData150.GlobalData.HeroUpgradeDatas[1101].SetHeroUpgradeData(DataManager.Inst.GetHeroUpgradeData(1101,1,0).Value);
            archiveData150.GlobalData.LastSelectHeroID = 1101;
#else
            archiveData150.GlobalData.UnLockHeros.Add(1203);
            // ReSharper disable once PossibleInvalidOperationException
            archiveData150.GlobalData.HeroUpgradeDatas.Add(1203,new HeroUpgradeInfo());
            archiveData150.GlobalData.HeroUpgradeDatas[1203].SetHeroUpgradeData(DataManager.Inst.GetHeroUpgradeData(1203,1,0).Value);
            archiveData150.GlobalData.LastSelectHeroID = 1203;
#endif
            archiveData150.GlobalData.LastSelectChapterId = 1;
            archiveData150.GlobalData.ThroughTutorial = false;
            // archiveData140.GlobalData.UnLockItems = ints;
            //战斗数据
            archiveData150.BattleData = null;
            //统计数据
            archiveData150.StatisticsData = new StatisticsData_1_4_0();
            archiveData150.StatisticsData.KillEnemys.Clear();
            archiveData150.StatisticsData.TodayStartGameTimes.Clear();
            archiveData150.StatisticsData.AppStartTimes = 0;
            archiveData150.StatisticsData.StartGameTimes = 0;
            archiveData150.StatisticsData.GameTime = 0;
            archiveData150.StatisticsData.ChapterClearanceDatas.Clear();
            // archiveData140.StatisticsData.ChapterClearanceDatas.Add(0,new ChapterClearanceData(){Count = 1}); 跳过新手章节
            archiveData150.SettingArchiveData = new SettingArchiveData_1_4_0();
            archiveData150.SettingArchiveData.Sound = true;
            archiveData150.SettingArchiveData.Bgm = true;
            
            callBack?.Invoke(archiveData150);
        });
    }
}


[Serializable]
public class EquipmentArchiveData_1_5_0
{
    public Equipment Slot1;
    public Equipment Slot2;
    public Equipment Slot3;

    public List<Equipment> EquipmentPacket = new List<Equipment>();

    //guid
    public List<string> NeedPeekEquipList = new List<string>();
    
    public int GetScore()
    {
        int maxScore = 0;
        if (Slot1 != null)
            if (maxScore < Slot1.Score)
                maxScore = Slot1.Score;
        if (Slot2 != null)
            if (maxScore < Slot2.Score)
                maxScore = Slot2.Score;
        if (Slot3 != null)
            if (maxScore < Slot3.Score)
                maxScore = Slot3.Score;

        return maxScore;
    }

    public void LoadEquip(Equipment equipment)
    {
        if (!EquipmentPacket.Contains(equipment))
            return;
        if (Slot1 == null)
        {
            Slot1 = equipment;
        }
        else if (Slot2 == null)
        {
            Slot2 = equipment;
        }
        else if (Slot3 == null)
        {
            Slot3 = equipment;
        }
        else
        {
            UIManager.Inst.Tips.ShowText(LocalizationManger.Inst.GetText("NoSlots"));
            // Debug.LogError("没有空槽");
            return;
        }
        EquipmentPacket.Remove(equipment);
        EventManager.Inst.DistributeEvent(EventName.OnLoadEquip);
        ArchiveManager.Inst.SaveArchive();
    }
    public void UnLoadEquip(Equipment equipment)
    {
        if (Slot1 != null && Slot1.Guid == equipment.Guid)
        {
            Slot1 = null;
        }
        else if (Slot2 != null && Slot2.Guid == equipment.Guid)
        {
            Slot2 = null;
        }
        else if (Slot3 != null && Slot3.Guid == equipment.Guid)
        {
            Slot3 = null;
        }
        else
        {
            Debug.LogError("没有找到对应槽位");
            return;
        }
        EquipmentPacket.Add(equipment);
        EventManager.Inst.DistributeEvent(EventName.OnUnLoadEquip);
        ArchiveManager.Inst.SaveArchive();
    }

    public void DeleteEquip(Equipment equipment)
    {
        if (!EquipmentPacket.Contains(equipment))
            return;
        EquipmentPacket.Remove(equipment);
        ArchiveManager.Inst.ChangeDiamond(25);
        ArchiveManager.Inst.SaveArchive();
    }

    public void AddEquip(Equipment equipment)
    {
        EquipmentPacket.Add(equipment);
        if (!NeedPeekEquipList.Contains(equipment.Guid))
            NeedPeekEquipList.Add(equipment.Guid);
        ArchiveManager.Inst.SaveArchive();
    }

    public void RemoveEquipRedPoint(string guid)
    {
        if (NeedPeekEquipList.Contains(guid))
            NeedPeekEquipList.Remove(guid);
        ArchiveManager.Inst.SaveArchive();
    }
}

[Serializable]
public class AttributesArchiveData_1_5_0
{
    public int AttackPowerLv = 0;
    public int MaxHpLv = 0;
    public int CritLv = 0;

    public ArchiveErrorType UpgradeAttribute(AttributeType attributeType)
    {
        TableAttributesData tableAttributesData = null;
        switch (attributeType)
        {
            case AttributeType.AttackPower:
                tableAttributesData = DataManager.Inst.GetTableAttributesData(attributeType,AttackPowerLv+1);
                break;
            case AttributeType.MaxHp:
                tableAttributesData = DataManager.Inst.GetTableAttributesData(attributeType,MaxHpLv+1);
                break;
            case AttributeType.CriticalProbability:
                tableAttributesData = DataManager.Inst.GetTableAttributesData(attributeType,CritLv+1);
                break;
        }

        //满级
        if (tableAttributesData == null)
            return ArchiveErrorType.MaxLevel;

        //没钱
        if (tableAttributesData.Price > ArchiveManager.Inst.ArchiveData.GlobalData.Diamond)
            return ArchiveErrorType.DiamondShortage;
        
        ArchiveManager.Inst.ChangeDiamond(-tableAttributesData.Price);

        switch (attributeType)
        {
            case AttributeType.AttackPower:
                AttackPowerLv++;
                break;
            case AttributeType.CriticalProbability:
                CritLv++;
                break;
            case AttributeType.MaxHp:
                MaxHpLv++;
                break;
        }

        return ArchiveErrorType.NoError;
    }

    public TableAttributesData GetCurrentAttributeData(AttributeType attributeType)
    {
        TableAttributesData tableAttributesData = null;
        switch (attributeType)
        {
            case AttributeType.AttackPower:
                tableAttributesData = DataManager.Inst.GetTableAttributesData(attributeType,AttackPowerLv);
                break;
            case AttributeType.CriticalProbability:
                tableAttributesData = DataManager.Inst.GetTableAttributesData(attributeType,CritLv);
                break;
            case AttributeType.MaxHp:
                tableAttributesData = DataManager.Inst.GetTableAttributesData(attributeType,MaxHpLv);
                break;
        }

        return tableAttributesData;
    }
    public TableAttributesData GetNextLvAttributeData(AttributeType attributeType)
    {
        TableAttributesData tableAttributesData = null;
        switch (attributeType)
        {
            case AttributeType.AttackPower:
                tableAttributesData = DataManager.Inst.GetTableAttributesData(attributeType,AttackPowerLv+1);
                break;
            case AttributeType.CriticalProbability:
                tableAttributesData = DataManager.Inst.GetTableAttributesData(attributeType,CritLv+1);
                break;
            case AttributeType.MaxHp:
                tableAttributesData = DataManager.Inst.GetTableAttributesData(attributeType,MaxHpLv+1);
                break;
        }

        return tableAttributesData;
    }
    
    
    
}