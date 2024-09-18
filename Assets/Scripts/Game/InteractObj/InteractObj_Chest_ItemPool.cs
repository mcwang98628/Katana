using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class InteractObj_Chest_ItemPool : InteractObj_Chest
{

    // [LabelText("新手引导-道具池")][GUIColor(1,1,0)]
    // public ItemPoolScriptableObject GuideItemPool;
    // [Header("ItemPool")]
    // public ItemPoolScriptableObject itemPool;

    public override void InteractStart()
    {
        if (!canInteract)
        {
            return;
        }
        canInteract = false;
        
        UIManager.Inst.Open("ChooseItemPanel");  
        
        EventManager.Inst.AddEvent(EventName.OnRoleAddItem, OnGetItem);
        //EventManager.Inst.AddEvent(EventName.OnReplaceItem, Open);   
    }

    public override void InteractEnd()
    {
        base.InteractEnd();
        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem, OnGetItem);
        //EventManager.Inst.RemoveEvent(EventName.OnReplaceItem, Open);   
    }

    void Open(string str,object value)
    {
        OpenChestEffect();
    }

    void OnGetItem(string str,object value)
    {
        var itemdata = (RoleItemEventData)value;
        item = DataManager.Inst.GetItemScrObj(itemdata.Item.ID);
        //临时解决宝箱被药水激活的情况
        if(item.ItemType==ItemType.Prop)
            return;
        
        OpenChestEffect();

        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem, OnGetItem);
        Invoke("InteractEnd", 0.5f);
    }

}
