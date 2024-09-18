using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PausePanel_ItemPrefab : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    private Item _item;
    public void Init(Item item)
    {
        _item = item;
        icon.sprite = item.Icon;
        this.gameObject.SetActive(true);
    }

    public void OnBtnClick()
    {
        UIManager.Inst.Open("InfoPanel",false,DataManager.Inst.GetItemScrObj(_item.ID));
    }
}
