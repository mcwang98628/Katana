using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentColor
{
    public static Color Lv1Color = Color.white;
    public static Color Lv2Color = Color.green;
    public static Color Lv3Color = Color.red;
    public static Color Lv4Color = Color.yellow;
}
public class UI_MainPanel_EquipmentPanel_EquipSlot : MonoBehaviour
{
  

    [SerializeField]
    private UI_MainPanel_EquipmentPanel equipmentPanel;
    [SerializeField]
    private Image bg;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private GameObject lvTextBg;
    [SerializeField]
    private Text lvText;
    private Equipment _equipment;

    public void Init(Equipment equipment)
    {
        _equipment = equipment;
        if (_equipment == null)
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
            bg.color = Color.gray;
            lvText.text = "";
            lvTextBg.gameObject.SetActive(false);
            return;
        }
        lvTextBg.gameObject.SetActive(true);
        lvText.text = (_equipment.Score / 10).ToString();
        _equipment.LoadIcon(delegate(Sprite sprite)
        {
            icon.sprite = sprite;
            icon.color = Color.white;
        });
        
        switch (equipment.Quality)
        {
            case EquipmentQuality.Lv1:
                bg.color = EquipmentColor.Lv1Color;
                break;
            case EquipmentQuality.Lv2:
                bg.color = EquipmentColor.Lv2Color;
                break;
            case EquipmentQuality.Lv3:
                bg.color = EquipmentColor.Lv3Color;
                break;
            case EquipmentQuality.Lv4:
                bg.color = EquipmentColor.Lv4Color;
                break;
        }
    }
    
    public void OnBtnClick()
    {
        equipmentPanel.SetCurrentEquip(_equipment);
    }

    public void OnToggle(bool isOn)
    {
        if (isOn)
        {
            OnBtnClick();
        }
    }
}
