using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_EquipmentPanel : MonoBehaviour
{
    // [SerializeField]
    // private VerticalLayoutGroup panelLayout;
    [SerializeField]
    private UI_MainPanel_EquipmentPanel_EquipInfo currentEquipInfo;
    // [SerializeField]
    // private UI_MainPanel_EquipmentPanel_EquipInfo selectEquipInfo;
    [SerializeField]
    private UI_MainPanel_EquipmentPanel_EquipSlot slot1;
    [SerializeField]
    private UI_MainPanel_EquipmentPanel_EquipSlot slot2;
    [SerializeField]
    private UI_MainPanel_EquipmentPanel_EquipSlot slot3;
    
    [SerializeField]
    private UI_MainPanel_EquipmentPanel_EquipPrefab equipPrefab;
    [SerializeField]
    private Transform group;

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnLoadEquip,InitPanel);
        EventManager.Inst.AddEvent(EventName.OnUnLoadEquip,InitPanel);
        // panelLayout.padding.top = (int)Screen.safeArea.min.y;
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnLoadEquip,InitPanel);
        EventManager.Inst.RemoveEvent(EventName.OnUnLoadEquip,InitPanel);
    }

    private void OnEnable()
    {
        InitPanel();
        
        EventManager.Inst.DistributeEvent(TGANames.EquipmentPanel);
    }


    private void InitPanel(string arg1, object arg2)
    {
        InitPanel();
    }

    public void InitPanel()
    {
        for (int i = 0; i < group.childCount; i++)
            GameObject.Destroy(group.GetChild(i).gameObject);
        var equipArchive = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150;
        var list = new List<Equipment>(equipArchive.EquipmentPacket);
        list.Sort((x,y)=>y.Score.CompareTo(x.Score));
        for (int i = 0; i < list.Count; i++)
        {
            var prefabGo = GameObject.Instantiate(equipPrefab, group);
            prefabGo.Init(list[i]);
            prefabGo.gameObject.name = $"Equip{i}";
        } 
        slot1.Init(equipArchive.Slot1);
        slot2.Init(equipArchive.Slot2);
        slot3.Init(equipArchive.Slot3);
        
    }

    public void SetCurrentEquip(Equipment equipment)
    {
        currentEquipInfo.Init(equipment);
    }

    public void SetSelectEquip(Equipment equipment)
    {
        currentEquipInfo.Init(equipment);
        // selectEquipInfo.Init(equipment);
    }

    public void LoadEquip(Equipment equipment)
    {
        if (equipment == null)
            return;
        
        EquipmentArchiveData_1_5_0 equipmentData150 = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150;
        equipmentData150.LoadEquip(equipment);
        
        EventManager.Inst.DistributeEvent(TGANames.WearEquipment,equipment);
    }

    public void UnLoadEquip(Equipment equipment)
    {
        if (equipment == null)
            return;

        EquipmentArchiveData_1_5_0 equipmentData150 = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150;
        equipmentData150.UnLoadEquip(equipment);
        
        EventManager.Inst.DistributeEvent(TGANames.UnloadEquipment,equipment);
    }
    
    
    
}
