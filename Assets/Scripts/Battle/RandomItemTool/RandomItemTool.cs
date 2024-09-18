using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomItemTool
{
    /// <summary>
    /// 获取玩家身上Item的流派信息
    /// </summary>
    /// <returns>Item流派信息</returns>
    public static PlayerItemSchoolInfo GetPlayerItemSchoolInfo()
    {
        var items = BattleManager.Inst.CurrentPlayer.roleItemController.Items;
        List<int> itemIds = new List<int>();
        foreach (Item item in items)
        {
            itemIds.Add(item.ID);
        }

        //流派中缺少的Item
        Dictionary<int,List<int>> itemSchoolLackItem = new Dictionary<int, List<int>>();
        //不包含的流派
        List<int> notContainItemSchool = new List<int>();
        //完整包含道具的流派
        List<int> containItemSchool = new List<int>();
        foreach (var itemSchool in DataManager.Inst.ItemSchool)
        {
            //是否包含这个流派的Item，如果不包含 也不随机这个流派的道具。
            bool isHaveSchoolItem = false;
            bool isHaveAllSchoolItem = true;
            foreach (int itemId in itemSchool.Value.ItemList)
            {
                if (itemIds.Contains(itemId))
                {
                    isHaveSchoolItem = true;
                }
                else
                {
                    isHaveAllSchoolItem = false;
                    if (!itemSchoolLackItem.ContainsKey(itemSchool.Key))
                    {
                        itemSchoolLackItem.Add(itemSchool.Key,new List<int>());
                    }
                    itemSchoolLackItem[itemSchool.Key].Add(itemId);
                }
            }

            if (!isHaveSchoolItem)
            {
                notContainItemSchool.Add(itemSchool.Key);
                itemSchoolLackItem.Remove(itemSchool.Key);
            }

            if (isHaveAllSchoolItem)
            {
                containItemSchool.Add(itemSchool.Key);
            }
        }
        
        return new PlayerItemSchoolInfo(itemSchoolLackItem,notContainItemSchool,containItemSchool);
    }

    /// <summary>
    /// 随机一个玩家未能组成的流派中的一个缺少的ItemId
    /// </summary>
    /// <returns></returns>
    public static int GetOneItemIdByPlayerItemSchoolInfo()
    {
        List<int> items = GetPlayerItemSchoolInfo().GetAllLackItem();
        for (int i = 0; i < items.Count; i++)
        {
            if (!CheckIsCanGetItem(DataManager.Inst.GetItemScrObj(items[i])))
            {
                items.RemoveAt(i);
                i--;
            }
        }
        
        if (items.Count == 0)
        {
            return -1;
        }
        int index = Random.Range(0, items.Count);
        return items[index];
    }
    
    /// <summary>
    /// 检查是否可以获取这个Item（唯一 已拥有一个，未解锁什么的)
    /// </summary>
    /// <returns>是否可获取</returns>
    public static bool CheckIsCanGetItem(ItemScriptableObject itemScriptableObject)
    {
        int itemId = DataManager.Inst.GetItemId(itemScriptableObject);
        if ((BattleManager.Inst.CurrentPlayer.roleItemController.IsHaveItem(itemId) && itemScriptableObject.isUnique) //已拥有唯一的
            ||
            !ArchiveManager.Inst.CheckItemIsUnLock(itemId))//未解锁
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 获取当前玩家可获取的ItemList
    /// </summary>
    /// <param name="itemPool"></param>
    /// <returns></returns>
    public static List<ItemScriptableObject> GetCanGetItemPool(List<ItemScriptableObject> itemPool)
    {
        List<ItemScriptableObject> itemList = new List<ItemScriptableObject>();
        foreach (ItemScriptableObject item in itemPool)
        {
            if (!CheckIsCanGetItem(item) || itemList.Contains(item))
            {
                continue;
            }
            itemList.Add(item);
        }
        return itemList;
    }
    
    
    /// <summary>
    /// 随机商店商品
    /// </summary>
    /// <param name="itemPool">ItemList</param>
    /// <param name="originalPrice">原价格</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static List<ShopItem> RandomShopGod(int count)
    {
        List<ShopItem> shopItemList = new List<ShopItem>();
        List<ItemPoolData> itemPool = new List<ItemPoolData>(DataManager.Inst.ItemPool.Shop);
        List<ItemPoolData> _showItemList = new List<ItemPoolData>();
        for (int i = 0; i < count; i++)
        {
            int randomMaxValue = 0;
            foreach (var poolData in itemPool)
                randomMaxValue += poolData.ProbabilityWeight;
            int randomValue = Random.Range(0, randomMaxValue+1);
            int weight = 0;
            foreach (var poolData in itemPool)
            {
                if (!CheckIsCanGetItem(poolData.Item))
                    continue;
                
                weight += poolData.ProbabilityWeight;
                if (randomValue <= weight)
                {
                    _showItemList.Add(poolData);
                    itemPool.Remove(poolData);
                    break;
                }
            }
        }

        int probability = BattleManager.Inst.CurrentPlayer.GetTagCount(RoleTagName.FreeGoods) * 5;
        foreach (ItemPoolData poolData in _showItemList)
        {
            int price = poolData.Price;
            int randomValue = Random.Range(0, 100);
            if (randomValue < probability)
                price = 0;
            
            shopItemList.Add(new ShopItem(poolData.Item, price,1));
        }
        
        if (shopItemList.Count == 0)
        {
            Debug.LogError("没有符合要求的道具");
            return null;
        }

        // int schoolItemid = 0;
        // //强制流派，药水商店不执行,如果玩家通过第二章 不执行
        // if (!ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas.ContainsKey(2) ||
        //     ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas[2].Count <= 0 )
        // {
        //     schoolItemid = GetOneItemIdByPlayerItemSchoolInfo();
        //     if (schoolItemid>0)
        //     {
        //         bool isCanForceBuild = false;
        //         foreach (ShopItem shopItem in shopItemList)//判断池里是否有。
        //         {
        //             if (DataManager.Inst.GetItemId(shopItem.Item) == schoolItemid)
        //             {
        //                 isCanForceBuild = true;
        //                 break;
        //             }
        //         }
        //
        //         if (isCanForceBuild)
        //         {
        //             count--;
        //         }
        //         else
        //         {
        //             schoolItemid = 0;
        //         }
        //     }
        // }
        //
        // if (schoolItemid > 0)
        // {
        //     foreach (ShopItem shopItem in shopItemList)
        //     {
        //         int itemId = DataManager.Inst.GetItemId(shopItem.Item);
        //         if (schoolItemid == itemId)
        //         {
        //             shopItemList.Remove(shopItem);
        //             break;
        //         }
        //     }
        // }
        //
        // while (shopItemList.Count > count)
        // {
        //     var item = shopItemList[UnityEngine.Random.Range(0, shopItemList.Count)];
        //     shopItemList.Remove(item);
        // }
        //
        // if (schoolItemid > 0)
        // {
        //     shopItemList.Add(new ShopItem(
        //         DataManager.Inst.GetItemScrObj(schoolItemid),
        //         originalPrice,
        //         1));
        // }
        
        return shopItemList;
    }

    public static List<ItemScriptableObject> RandomItems(List<ItemScriptableObject> itemPool,int count)
    {
        var itemList = GetCanGetItemPool(itemPool);
        if (itemList.Count == 0)
        {
            Debug.LogError("没有可以获取的道具。");
            return null;
        }
        List<ItemScriptableObject> result=new List<ItemScriptableObject>();

        //道具池中添加一个BUID道具
        int schoolItemid = GetOneItemIdByPlayerItemSchoolInfo();
        if (schoolItemid>0)
        {
            result.Add(DataManager.Inst.GetItemScrObj(schoolItemid));
            count--;
        }
        for (int i = 0; i < count; i++)
        {
            if (itemList.Count == 0)
                break;
            
            var item = itemList[UnityEngine.Random.Range(0, itemList.Count)];
            itemList.Remove(item);
            if (result.Contains(item))
            {
                i--;
                continue;
            }
            result.Add(item);
        }
        return result;
    }
    
}
