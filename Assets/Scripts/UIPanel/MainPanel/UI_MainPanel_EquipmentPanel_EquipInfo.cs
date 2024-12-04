using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_EquipmentPanel_EquipInfo : MonoBehaviour
{
    [SerializeField]
    private UI_MainPanel_EquipmentPanel equipmentPanel;
    [SerializeField]
    private Image iconFrame;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text equipName;
    [SerializeField]
    private Text desc;
    [SerializeField]
    private Text quality;
    [SerializeField]
    private Text score;
    [SerializeField]
    private Text effect1;
    [SerializeField]
    private Text effect2;
    [SerializeField]
    private Text effect3;
    [SerializeField]
    private Text effect4;

    [SerializeField]
    private GameObject equipmentBtn;
    [SerializeField]
    private GameObject unEquipmentBtn;
    [SerializeField]
    private GameObject deleteBtn;
    

    private Equipment _equipment;

    private bool IsInPackage
    {
        get
        {
            if (_equipment == null)
                return false;
            var equipArchive = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150;
            if (equipArchive.Slot1 != null && equipArchive.Slot1.Guid == _equipment.Guid)
                return false;
            if (equipArchive.Slot2 != null && equipArchive.Slot2.Guid == _equipment.Guid)
                return false;
            if (equipArchive.Slot3 != null && equipArchive.Slot3.Guid == _equipment.Guid)
                return false;

            return true;
        }
    }

    public void Init(Equipment equipment)
    {
        SetNullEquip();
        if (equipment == null)
            return;
        equipment.LoadIcon(delegate(Sprite sprite)
        {
            iconFrame.gameObject.SetActive(true);
            icon.sprite = sprite;
            icon.color = Color.white;
        });
        _equipment = equipment;
        equipName.text = LocalizationManger.Inst.GetText(equipment.Name) ;
        desc.text = equipment.Desc;
        //score.text = "强度: " + equipment.Score;
        //score.text = equipment.Score .ToString();
        score.text = "Lv." + equipment.Score / 10;
        // quality.text = $"品质:{equipment.Quality}";
        switch (equipment.Quality)
        {
            case EquipmentQuality.Lv1:
                iconFrame.color = EquipmentColor.Lv1Color ;
                quality.color = EquipmentColor.Lv1Color ;
                break;
            case EquipmentQuality.Lv2:
                iconFrame.color = EquipmentColor.Lv2Color ;
                quality.color = EquipmentColor.Lv2Color ;
                break;
            case EquipmentQuality.Lv3:
                iconFrame.color = EquipmentColor.Lv3Color ;
                quality.color = EquipmentColor.Lv3Color ;
                break;
            case EquipmentQuality.Lv4:
                iconFrame.color = EquipmentColor.Lv4Color ;
                quality.color = EquipmentColor.Lv4Color ;
                break;
        }

        for (int i = 0; i < equipment.EffectList.Count; i++)
        {
            var effectType = equipment.EffectList[i];
            switch (i)
            {
                case 0:
                    effect1.text = "• " + equipment.GetEffectStr(effectType,EquipmentQuality.Lv1);
                    break;
                case 1:
                    effect2.text = "+(" + equipment.GetEffectStr(effectType,EquipmentQuality.Lv2) + ")";
                    break;
                case 2:
                    effect3.text = "+(" + equipment.GetEffectStr(effectType,EquipmentQuality.Lv3) + ")";
                    break;
                case 3:
                    effect4.text = "+(" + equipment.GetEffectStr(effectType,EquipmentQuality.Lv4) + ")";
                    break;
            }
        }
        
        // deleteBtn.SetActive(IsInPackage);
        equipmentBtn.SetActive(IsInPackage);
        unEquipmentBtn.SetActive(!IsInPackage);
    }



    public void LoadEquip()
    {
        if (!IsInPackage)
            return;
        equipmentPanel.LoadEquip(_equipment);
        SetNullEquip();
    }

    public void UnLoadEquip()
    {
        if (IsInPackage)
            return;
        equipmentPanel.UnLoadEquip(_equipment);
        SetNullEquip();
    }

    public void OnDeleteBtnClick()
    {
        ArchiveManager.Inst.ArchiveData.equipmentArchiveData150.DeleteEquip(_equipment);
        equipmentPanel.InitPanel();
        Init(null);
    }

    private void SetNullEquip()
    {
        _equipment = null;
        iconFrame.gameObject.SetActive(false);
        iconFrame.color = Color.grey;
        icon.sprite = null;
        equipName.text = "";
        desc.text = "";
        score.text = "";
        effect1.text = "";
        effect2.text = "";
        effect3.text = "";
        effect4.text = "";
        quality.text = "";
        icon.color = new Color(1, 1, 1, 0);
        equipmentBtn.SetActive(false);
        unEquipmentBtn.SetActive(false);
        deleteBtn.SetActive(false);
    }
    
}



