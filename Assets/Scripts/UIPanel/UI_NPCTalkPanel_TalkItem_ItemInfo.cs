using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NPCTalkPanel_TalkItem_ItemInfo : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private UIText nameText;
    [SerializeField]
    private UIText descText;

    
    
    
    private Item item;
    private Action<Item> TalkOverCallback;
    public void Init(Item item,Action<Item> callback)
    {
        gameObject.SetActive(true);
        this.item = item;
        TalkOverCallback = callback;
        icon.sprite = item.Icon;
        nameText.text = item.Name;
        nameText.Te.color =  Item.GetColor(item.itemColorType);
        descText.text = item.Desc;

    }

    

    public void OnItemClick()
    {
        EventManager.Inst.DistributeEvent(TGANames.BattleTreasureChestItem,item.ID);
        
        BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(item, isOk =>
        {
            if (isOk)
            {
                if (TalkOverCallback != null)
                {
                    TalkOverCallback(item);
                }
                //TODO:这里怎么获取到NPC的ID？
                EventManager.Inst.DistributeEvent(EventName.OnNpcTalkYesOrNoEvent, new NpcTalkYesOrNoEventData(-1,true));
            }
        });
    }
    
}
