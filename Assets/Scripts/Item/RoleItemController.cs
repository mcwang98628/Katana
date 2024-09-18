using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleItemController : MonoBehaviour
{
    public RoleController roleController;
    Dictionary<string, List<GameObject>> itemVisualObjDic = new Dictionary<string, List<GameObject>>();
    List<Item> items = new List<Item>();
    public List<Item> Items => items;
    private Item consumable;


    // private int artifactItemCount = 5;
    private int buttonSKillItemCount = 1;
    private int buttonItemCount = 2;
    //最多拥有多少神器
    // public int ArtifactItemCount => artifactItemCount;
    //最多拥有多少主动技能
    public int ButonSkillItemCount => buttonSKillItemCount;
    //最多拥有多少主动道具
    public int ButonItemCount => buttonItemCount;

    //当前帧 item 触发次数， EffectType，Times
    private Dictionary<int, int> currentFrameTriggerTimes = new Dictionary<int, int>();
    //效果
    private Dictionary<Type, int> currentFrameEffectTriggerTimes = new Dictionary<Type, int>();

    private void Awake()
    {
        roleController = GetComponent<RoleController>();
        
        EventManager.Inst.AddEvent(EventName.ItemTrigger,OnItemTrigger);
        EventManager.Inst.AddEvent(EventName.ItemEffectTrigger,OnItemEffectTrigger);
    }

    private void OnDestroy()
    {
        CloseAllItem();
        EventManager.Inst.RemoveEvent(EventName.ItemTrigger,OnItemTrigger);
        EventManager.Inst.RemoveEvent(EventName.ItemEffectTrigger,OnItemEffectTrigger);
    }

    private void OnItemEffectTrigger(string arg1, object arg2)
    {
        Type type = (Type) arg2;
        if (!currentFrameEffectTriggerTimes.ContainsKey(type))
            currentFrameEffectTriggerTimes.Add(type,0);
        currentFrameEffectTriggerTimes[type]++;
    }


    private void OnItemTrigger(string arg1, object arg2)
    {
        Item item = (Item) arg2;
        if (!currentFrameTriggerTimes.ContainsKey(item.ID))
            currentFrameTriggerTimes.Add(item.ID,0);
        currentFrameTriggerTimes[item.ID]++;
    }

    public int GetCurrentFrameItemTriggerTimes(int itemId)
    {
        if (!currentFrameTriggerTimes.ContainsKey(itemId))
            return 0;
        return currentFrameTriggerTimes[itemId];
    }

    public int GetCurrentFrameItemEffectTriggerTimes(Type effectType)
    {
        if (!currentFrameEffectTriggerTimes.ContainsKey(effectType))
            return 0;
        return currentFrameEffectTriggerTimes[effectType];
    }

    void CloseAllItem()
    {
        for (int i = 0; i < items.Count; i++)
        {
            foreach (ItemGroup itemGroup in items[i].ItemGroups)
            {
                itemGroup.Destroy(this);
            }
        }
        items.Clear();

    }

    public List<int> GetItemsId()
    {
        List<int> itemIds = new List<int>();
        for (int i = 0; i < Items.Count; i++)
        {
            itemIds.Add(Items[i].ID);
        }
        return itemIds;
    }

    public List<T> GetItemEffectByEffectType<T>() where T : ItemEffect
    {
        List<T> effects = new List<T>();
        foreach (Item item in items)
            foreach (ItemGroup itemGroup in item.ItemGroups)
                if (itemGroup.itemEffect is T)
                    effects.Add(itemGroup.itemEffect as T);
        return effects;
    }

    public List<Item> GetItemsForArtifactType(ArtifactType type)
    {
        List<Item> items = new List<Item>();

        foreach (Item itemValue in Items)
        {
            if (itemValue.ArtifactType == type)
            {
                items.Add(itemValue);
            }
        }
        return items;
    }
    public List<Item> GetItemsForItemType(ItemType type)
    {
        List<Item> items = new List<Item>();

        foreach (Item itemValue in Items)
        {
            if (itemValue.ItemType == type)
            {
                items.Add(itemValue);
            }
        }
        return items;
    }
    public Item GetItemForUniqueArtifactType(ArtifactType type)
    {

        if (type == ArtifactType.Armor_Unique ||
            type == ArtifactType.Attack_Unique ||
            type == ArtifactType.Kill_Unique ||
            type == ArtifactType.FootPrint_Unique ||
            type == ArtifactType.Special_Unique ||
            type == ArtifactType.Halo_Unique ||
            type == ArtifactType.Prop_Unique ||
            type == ArtifactType.Evade_Unique ||
            type == ArtifactType.Attribute_Unique ||
            type == ArtifactType.Summon_Unique)
        {
            foreach (Item itemValue in Items)
            {
                if (itemValue.ArtifactType == type)
                {
                    return itemValue;
                }
            }
        }

        return null;
    }

    public void ClearAllItem()
    {
        var itemIds = GetItemsId();
        for (int i = 0; i < itemIds.Count; i++)
        {
            ReMoveItemByID(itemIds[i]);
        }
    }
    public void AddItem(Item item, Action<bool> callback)
    {
        if (item == null)
        {
            Debug.LogError("Item = Null");
            callback?.Invoke(false);
            return;
        }

        item.TemporaryId = GuidTools.GetGUID();
        
        if (item.ItemType == ItemType.ButtonSkill)
        {
            int haveCount = GetItemsForItemType(ItemType.ButtonSkill).Count;
            if (haveCount >= ButonSkillItemCount)
            {
                UIManager.Inst.Open("ReplaceItemPanel", true, item, callback);
                // callback?.Invoke(false);
                return;
            }
        }

        item.Root = this;
        switch (item.ItemType)
        {
            case ItemType.Prop:
                Item haveItem = null;
                foreach (var value in items)
                {
                    if (value.ID == item.ID)
                    {
                        haveItem = value;
                        break;
                    }
                }
                if (haveItem != null)
                {
                    haveItem.AddUseTimes(item.RemainingTimes);
                }
                else
                {
                    items.Add(item);
                    item.Awake(this);
                    EventManager.Inst.DistributeEvent(EventName.OnAddActiveItem, item);
                }
                break;
            case ItemType.ButtonSkill:
                items.Add(item);
                item.Awake(this);
                EventManager.Inst.DistributeEvent(EventName.OnAddActiveItem, item);
                break;
            default:
                // case ItemType.Artifact:
                // case ItemType.CharacterSkill:
                // case ItemType.SoulStone:
                // case ItemType.Other:
                items.Add(item);
                item.Awake(this);
                break;
        }

        if (item.VisualObj != null)
        {
            GameObject itemVisualObj = null;
            switch (item.VisualObjSlot)
            {
                case VisualObjSlotType.Head:
                    itemVisualObj = GameObject.Instantiate(item.VisualObj, roleController.roleNode.Head, true);
                    itemVisualObj.transform.localPosition = Vector3.zero;
                    itemVisualObjDic.Add(item.TemporaryId, new List<GameObject>(){itemVisualObj});
                    break;
                case VisualObjSlotType.Body:
                    itemVisualObj = GameObject.Instantiate(item.VisualObj, roleController.roleNode.Body, true);
                    itemVisualObj.transform.localPosition = Vector3.zero;
                    itemVisualObjDic.Add(item.TemporaryId,  new List<GameObject>(){itemVisualObj});
                    break;
                case VisualObjSlotType.Weapon:
                    for (int i = 0; i < roleController.roleNode.Weapon.Count; i++)
                    {
                        itemVisualObj = GameObject.Instantiate(item.VisualObj, roleController.roleNode.Weapon[i], true);
                        itemVisualObj.transform.localPosition = Vector3.zero;
                        if (itemVisualObjDic.ContainsKey(item.TemporaryId))
                        {
                            itemVisualObjDic[item.TemporaryId].Add(itemVisualObj);
                        }
                        else
                        {
                            itemVisualObjDic.Add(item.TemporaryId,  new List<GameObject>(){itemVisualObj});
                        }
                    }
                    break;
                case VisualObjSlotType.Halo:
                    itemVisualObj = GameObject.Instantiate(item.VisualObj, roleController.roleNode.Halo, true);
                    itemVisualObj.transform.localPosition = Vector3.zero;
                    itemVisualObjDic.Add(item.TemporaryId,  new List<GameObject>(){itemVisualObj});
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (roleController.IsPlayer)
        {
            ArchiveManager.Inst.ArchiveData.StatisticsData.GetItemList.Add(item.ID);
        }
        
        EventManager.Inst.DistributeEvent(EventName.OnRoleAddItem, new RoleItemEventData() { Item = item, RoleId = roleController.TemporaryId });
        callback?.Invoke(true);
        return;
    }

    public void ReMoveItem(Item item)
    {
        item.Destroy(this);
        if (itemVisualObjDic.ContainsKey(item.TemporaryId))
        {
            foreach (GameObject obj in itemVisualObjDic[item.TemporaryId])
            {
                GameObject.Destroy(obj);
            }
        }
        items.Remove(item);
        itemVisualObjDic.Remove(item.TemporaryId);
        EventManager.Inst.DistributeEvent(EventName.OnRoleReMoveItem, new RoleItemEventData() { Item = item, RoleId = roleController.TemporaryId });
    }

    public bool ReMoveItemByID(int guid, int maxNumber = 1)
    {
        int num = 0;
        List<Item> removeitem = new List<Item>();
        foreach (var item in items)
        {
            if (item.ID == guid)
            {
                removeitem.Add(item);
                num++;
                if (num >= maxNumber)
                {
                    break;
                }
            }
        }
        foreach (var item in removeitem)
        {
            ReMoveItem(item);
        }

        return removeitem.Count > 0;
    }

    public void RemoveAllItem()
    {
        for (int i = Items.Count - 1; i >= 0; i--)
        {
            ReMoveItem(Items[i]);
        }
    }
    public int GetItemCount(int guid)
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item.ID == guid)
            {
                count++;
            }
        }
        return count;
    }

    private void Update()
    {
        foreach (Item item in items)
        {
            for (int i = 0; i < item.ItemGroups.Count; i++)
            {
                item.ItemGroups[i].Update(this);
            }
        }

        if (consumable != null)
        {
            for (int i = 0; i < consumable.ItemGroups.Count; i++)
            {
                consumable.ItemGroups[i].Update(this);
            }
        }

    }

    public bool IsHaveItem(int itemGuId)
    {
        foreach (Item item in items)
        {
            if (item.ID == itemGuId)
            {
                return true;
            }
        }
        return false;
    }


    List<Action> currentFrameTrigger = new List<Action>();
    //添加当前帧需要触发的Item
    public void AddCurrentFrameTrigger(Action triggerCallBack)
    {
        currentFrameTrigger.Add(triggerCallBack);
    }

    Dictionary<string, List<Action>> currentFrameTargetIdTriggers = new Dictionary<string, List<Action>>();
    public void AddCurrentFrameTargetIdTrigger(string targetId, Action callBack)
    {
        callBack?.Invoke();
        return;//关闭了道具延迟触发
        if (!currentFrameTargetIdTriggers.ContainsKey(targetId))
        {
            currentFrameTargetIdTriggers.Add(targetId, new List<Action>());
        }
        currentFrameTargetIdTriggers[targetId].Add(callBack);
    }

    private void LateUpdate()
    {
        DelayedTrigger();
        currentFrameTriggerTimes.Clear();
        currentFrameEffectTriggerTimes.Clear();
    }

    void DelayedTrigger()
    {
        if (currentFrameTrigger.Count > 0)
        {
            List<Action> newList = new List<Action>();
            foreach (Action action in currentFrameTrigger)
            {
                newList.Add(action);
            }
            currentFrameTrigger.Clear();
            StartCoroutine(delayedTrigger(newList));
        }

        if (currentFrameTargetIdTriggers.Count > 0)
        {
            StartCoroutine(delayedTargetIdTriggers());
        }
    }

    IEnumerator delayedTrigger(List<Action> actions)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].Invoke();
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator delayedTargetIdTriggers()
    {
        Dictionary<string, List<Action>> targetTriggers = new Dictionary<string, List<Action>>();
        foreach (var triggers in currentFrameTargetIdTriggers)
        {
            targetTriggers.Add(triggers.Key, triggers.Value);
        }
        currentFrameTargetIdTriggers.Clear();
        List<List<Action>> currentTriggers = new List<List<Action>>();
        while (targetTriggers.Count > 0)
        {
            currentTriggers.Add(new List<Action>());
            List<string> removeTriggers = new List<string>();
            foreach (var targetTrigger in targetTriggers)
            {
                if (targetTrigger.Value.Count <= 0)
                {
                    removeTriggers.Add(targetTrigger.Key);
                    continue;
                }
                currentTriggers[currentTriggers.Count - 1].Add(targetTrigger.Value[0]);
                targetTrigger.Value.RemoveAt(0);
            }

            for (int i = 0; i < removeTriggers.Count; i++)
            {
                targetTriggers.Remove(removeTriggers[i]);
            }
        }
        for (int i = 0; i < currentTriggers.Count; i++)
        {
            for (int j = 0; j < currentTriggers[i].Count; j++)
            {
                currentTriggers[i][j].Invoke();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }


    public DifficultyData GetDifficultyData()
    {
        int level = GetItemsForItemType(ItemType.Artifact).Count - 5;
        if (level < 0)
            level = 0;
        return DataManager.Inst.GetDifficultyDataByLevel(level);
    }
}
