using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopPanel_ItemCurrency : MonoBehaviour
{
    public Image Icon;
    public Text CountText;

    private int itemGuid;
    public void Init(int itemguid)
    {
        itemGuid = itemguid;
        gameObject.SetActive(true);
        updateIcon();
        updateCount();
    }

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleReMoveItem,OnRoleItemUpdate);
        EventManager.Inst.AddEvent(EventName.OnRoleAddItem,OnRoleItemUpdate);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleReMoveItem,OnRoleItemUpdate);
        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem,OnRoleItemUpdate);
    }
    
    private void OnRoleItemUpdate(string arg1, object arg2)
    {
        if (((RoleItemEventData)arg2).Item.ID != itemGuid)
        {
            return;
        }
        updateCount();
    }

    void updateCount()
    {
        int count = BattleManager.Inst.CurrentPlayer.roleItemController.GetItemCount(itemGuid);
        CountText.text = count.ToString();
    }

    void updateIcon()
    {
        Icon.sprite = DataManager.Inst.GetItemScrObj(itemGuid).Icon;
    }
}
