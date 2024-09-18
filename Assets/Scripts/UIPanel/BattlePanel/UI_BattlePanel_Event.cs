using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_BattlePanel_Event : MonoBehaviour
{
    [SerializeField]
    private Transform group;
    [SerializeField]
    private UI_BattlePanel_EventPrefab eventPrefab;
    
    [SerializeField]
    private UI_BattlePanel_ItemTips itemTips;
    [SerializeField]
    private UI_BattlePanel_RoomTips roomTips;
    
    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleAddItem,OnRoleAddItem);
        EventManager.Inst.AddEvent(EventName.OnUnlockItemEvent,OnUnLockItemEvent);
        EventManager.Inst.AddEvent(EventName.EnterEventRoom, OnNewRoom);
        EventManager.Inst.AddEvent(EventName.UI_EnterNextRoom, OnNewRoom);
        EventManager.Inst.AddEvent(EventName.OnRoleAddItem,OnPlayerAddItem);
    }


    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem,OnPlayerAddItem);
        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem,OnRoleAddItem);
        EventManager.Inst.RemoveEvent(EventName.OnUnlockItemEvent,OnUnLockItemEvent);
        EventManager.Inst.RemoveEvent(EventName.EnterEventRoom, OnNewRoom);
        EventManager.Inst.RemoveEvent(EventName.UI_EnterNextRoom, OnNewRoom);
    }

    List<int> shownItemSchoolList = new List<int>();
    private void OnPlayerAddItem(string arg1, object arg2)
    {
        RoleItemEventData data = (RoleItemEventData) arg2;
        if (data.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }

        List<int> itemIds = BattleManager.Inst.CurrentPlayer.roleItemController.GetItemsId();
        var newItemSchool = GetItemSchool(itemIds);
        shownItemSchoolList.AddRange(newItemSchool);
        
        for (int i = 0; i < newItemSchool.Count; i++)
        {
            int schoolId = newItemSchool[i];
            if (!ArchiveManager.Inst.ArchiveData.StatisticsData.UnlockItemSchool.Contains(schoolId))
            {
                ArchiveManager.Inst.ArchiveData.StatisticsData.UnlockItemSchool.Add(schoolId);   
            }

            ResourcesManager.Inst.GetAsset<Sprite>(
                $"Assets/Arts/Textures/UISprites/BuildIcon/{DataManager.Inst.ItemSchool[schoolId].Icon}.png",
                delegate(Sprite sprite)
                {
                    GameObject.Instantiate(eventPrefab,group)
                        .Init(
                            new Color(0.9f,0.6f,0.0f),
                            sprite,
                            LocalizationManger.Inst.GetText("UI_MainPanel_ItemBuild")+" : "+LocalizationManger.Inst.GetText(DataManager.Inst.ItemSchool[schoolId].Name)
                        );
                });
        }
        ArchiveManager.Inst.SaveArchive();
    }

    List<int> GetItemSchool(List<int> itemIds)
    {
        List<int> schoolIds = new List<int>();
        foreach (KeyValuePair<int,ItemSchoolData> itemSchoolData in DataManager.Inst.ItemSchool)
        {
            int itemCount = 0;
            for (int i = 0; i < itemSchoolData.Value.ItemList.Count; i++)
            {
                if (itemIds.Contains(itemSchoolData.Value.ItemList[i]))
                {
                    itemCount++;
                }
            }

            if (!shownItemSchoolList.Contains(itemSchoolData.Key) && itemCount >= itemSchoolData.Value.MinItemCount)
            {
                schoolIds.Add(itemSchoolData.Key);
            }
        }
        return schoolIds;
    }
    
    

    private void OnUnLockItemEvent(string arg1, object itemId)
    {
        return;
        // var id = (int) itemId;
        // var itemData = DataManager.Inst.GetItemScrObj(id);
        // if (itemData.ItemType != ItemType.Artifact)
        // {
        //     return;
        // }
        // GameObject.Instantiate(eventPrefab,group)
        //     .Init(
        //         new Color(0.2f,0.4f,0.8f),
        //         itemData.Icon,
        //         LocalizationManger.Inst.GetText("UnlockItem")+" : "+LocalizationManger.Inst.GetText(itemData.Name)
        //         );
    }


    private void OnNewRoom(string arg1, object arg2)
    {
         var roomData = (RoomData) arg2;
         switch (roomData.RoomType)
         {
             case RoomType.FightRoom:
                 roomTips.Init(LocalizationManger.Inst.GetText("RoomTipsInfo_FightRoom")); 
                 break;
             case RoomType.BossFightRoom:
                 roomTips.Init(LocalizationManger.Inst.GetText("RoomTipsInfo_EliteFightRoom")); 
                 break;
             case RoomType.TreasureRoom:
                 roomTips.Init(LocalizationManger.Inst.GetText("RoomTipsInfo_TreasureRoom")); 
                 break;
             case RoomType.EventRoom:
                 roomTips.Init(LocalizationManger.Inst.GetText("RoomTipsInfo_EventRoom")); 
                 break;
             case RoomType.ShopRoom:
                 roomTips.Init(LocalizationManger.Inst.GetText("RoomTipsInfo_ShopRoom")); 
                 break;
         }
    }
    private void OnRoleAddItem(string arg1, object value)
    {
        var item = (RoleItemEventData) value; 
        if (item.Item.ItemType == ItemType.Artifact ||
            item.Item.ItemType == ItemType.SoulStone ||item.Item.ItemType == ItemType.Prop)
        {
            itemTips.Init(item.Item); 
        }
    }
}
