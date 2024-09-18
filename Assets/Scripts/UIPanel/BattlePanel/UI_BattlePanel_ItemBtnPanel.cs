using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UI_BattlePanel_ItemBtnPanel : MonoBehaviour
{
    public UI_BattlePanel_ItemBtn timeBtn;
    public UI_BattlePanel_ItemBtn skillBtn;
    public Transform itemGroup;
    public Transform skillItemGroup;
    
    List<UI_BattlePanel_ItemBtn> skillBtns = new List<UI_BattlePanel_ItemBtn>();
    List<UI_BattlePanel_ItemBtn> itemBtns = new List<UI_BattlePanel_ItemBtn>();
    List<Item> waitItemList = new List<Item>();
    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleReMoveItem,OnRoleReMoveItem);
        EventManager.Inst.AddEvent(EventName.OnAddActiveItem,OnAddActiveItem);
        EventManager.Inst.AddEvent(EventName.SkillGuide,OnSkillGuide);
        EventManager.Inst.AddEvent(EventName.OnUseItem,OnUseItem);
        InitNullItem();
        if (BattleManager.Inst.CurrentPlayer != null)
        {
            foreach (Item item in BattleManager.Inst.CurrentPlayer.roleItemController.Items)
            {
                OnAddActiveItem("",item);
            }
        }
    }

    List<Tweener> _tweeners = new List<Tweener>();
    private void OnSkillGuide(string arg1, object arg2)
    {
        for (int i = 0; i < skillBtns.Count; i++)
        {
            _tweeners.Add(skillBtns[i].transform.DOScale(Vector3.one * 1.5f, 0.12f).SetLoops(-1, LoopType.Yoyo)); 
        }
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleReMoveItem,OnRoleReMoveItem);
        EventManager.Inst.RemoveEvent(EventName.OnAddActiveItem,OnAddActiveItem);
        EventManager.Inst.RemoveEvent(EventName.SkillGuide,OnSkillGuide);
        EventManager.Inst.RemoveEvent(EventName.OnUseItem,OnUseItem);
    }

    private void OnUseItem(string arg1, object arg2)
    {
        if (_tweeners.Count>0)
        {
            for (int i = 0; i < _tweeners.Count; i++)
            {
                _tweeners[i].Kill();
            }
            _tweeners.Clear();
            for (int i = 0; i < skillBtns.Count; i++)
            {
                skillBtns[i].transform.localScale = Vector3.one;
            }
        }
    }

    private void InitNullItem()
    {
        int btnSkillItemCount = BattleManager.Inst.CurrentPlayer.roleItemController.ButonSkillItemCount;
        // int btnItemCount = BattleManager.Inst.CurrentPlayer.roleItemController.ButonItemCount;

        for (int i = 0; i < btnSkillItemCount; i++)
        {
            UI_BattlePanel_ItemBtn itemBtn = Instantiate(skillBtn,skillItemGroup);
            itemBtn.Init(null);
            skillBtns.Add(itemBtn);
        }
        // for (int i = 0; i < btnItemCount; i++)
        // {
        //     UI_BattlePanel_ItemBtn itemBtn = Instantiate(timeBtn,itemGroup);
        //     itemBtn.Init(null);
        //     timeBtns.Add(itemBtn);
        // }
    }

    private int itemBtnCount = 5;
    private void OnAddActiveItem(string eventName, object value)
    {
        Item item = value as Item;
        if (item == null)
        {
            return;
        }

        switch (item.ItemType)
        {
            case ItemType.Prop:
                if (itemBtns.Count >= itemBtnCount)
                {
                    waitItemList.Add(item);
                    return;
                }
                UI_BattlePanel_ItemBtn itemBtn = Instantiate(timeBtn,itemGroup);
                itemBtn.Init(item);
                itemBtns.Add(itemBtn); 
                break;
            case ItemType.ButtonSkill:
                for (int i = 0; i < skillBtns.Count; i++)
                {
                    if (skillBtns[i].ItemData == null)
                    {
                        skillBtns[i].Init(item);
                        return;
                    }
                }
                break;
        }
    }
    
    private void OnRoleReMoveItem(string arg1, object value)
    { 
        var data = (RoleItemEventData) value;
        for (int i = 0; i < itemBtns.Count; i++)
        {
            if (data.Item.TemporaryId == itemBtns[i].ItemData.TemporaryId && 
                data.RoleId == BattleManager.Inst.CurrentPlayer.TemporaryId)
            {
                Destroy(itemBtns[i].gameObject);
                itemBtns.RemoveAt(i);
                break;
            }
        }

        if (itemBtns.Count < itemBtnCount && waitItemList.Count>0)
        {
            int count = itemBtnCount - itemBtns.Count;
            for (int i = 0; i < waitItemList.Count && i < count; i++)
            {
                UI_BattlePanel_ItemBtn itemBtn = Instantiate(timeBtn,itemGroup);
                itemBtn.Init(waitItemList[i]);
                itemBtns.Add(itemBtn); 
                waitItemList.RemoveAt(i);
                i--;
                count--;
            }
        }
    }
    
}
