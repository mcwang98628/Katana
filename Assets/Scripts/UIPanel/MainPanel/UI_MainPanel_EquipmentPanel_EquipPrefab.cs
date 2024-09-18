using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_EquipmentPanel_EquipPrefab : MonoBehaviour
{
    
    [SerializeField]
    private UI_MainPanel_EquipmentPanel equipmentPanel;
    [SerializeField]
    private Image bg;
    [SerializeField]
    private Image icon;
    // [SerializeField]
    // private Image select;

    [SerializeField]
    private Text lvText;
    [SerializeField]
    private GameObject redPoint;
    
    private Equipment _equipment;

    public Color Lv1Color;
    public Color Lv2Color;
    public Color Lv3Color;
    public Color Lv4Color;


    public void Init(Equipment equipment)
    {
        _equipment = equipment;
        _equipment.LoadIcon(delegate(Sprite sprite)
        {
            icon.sprite = sprite;
        });
        lvText.text = (_equipment.Score / 10).ToString();

        switch (equipment.Quality)
        {
            case EquipmentQuality.Lv1:
                bg.color = EquipmentColor.Lv1Color;
                break;
            case EquipmentQuality.Lv2:
                bg.color = EquipmentColor.Lv2Color ;
                break;
            case EquipmentQuality.Lv3:
                bg.color = EquipmentColor.Lv3Color ;
                break;
            case EquipmentQuality.Lv4:
                bg.color = EquipmentColor.Lv4Color ;
                break;
            
        }

        bool isNeedPeek = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150.NeedPeekEquipList.Contains(equipment.Guid);
        redPoint.SetActive(isNeedPeek);
        this.gameObject.SetActive(true);
    }
    
    public void OnBtnClick()
    {
        redPoint.SetActive(false);
        equipmentPanel.SetSelectEquip(_equipment);
        ArchiveManager.Inst.ArchiveData.equipmentArchiveData150.RemoveEquipRedPoint(_equipment.Guid);
    }

    public void OnToggle(bool isOn)
    {
        if (isOn)
        {
            OnBtnClick();
        }
    }
}
