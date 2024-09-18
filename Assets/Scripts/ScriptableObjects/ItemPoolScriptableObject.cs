using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "GameItem/DropItemPoolObj")]
public class ItemPoolScriptableObject : ScriptableObject
{
    public List<ItemScriptableObject> Items = new List<ItemScriptableObject>();
    
    public List<ItemScriptableObject> GetItemFromPool(int count)
    {
        Dictionary<int,float> probalility = DropManager.Inst.GetItemSchoolItemProbability(GetAllItemId());
        //满足条件，可以随机的物品ID
        List<int> canRandomItemId = new List<int>();
        foreach (var item in probalility)
        {
            var itemScrObj = DataManager.Inst.GetItemScrObj(item.Key);
            if (RandomItemTool.CheckIsCanGetItem(itemScrObj))
            {
                canRandomItemId.Add(item.Key);
            }
        }

        if (canRandomItemId.Count == 0)
        {
            return null;
        }
        
        //累加概率
        Dictionary<int,float> newProbalility = new Dictionary<int, float>();
        float value=0;
        foreach (KeyValuePair<int,float> pair in probalility)
        {
            if (!canRandomItemId.Contains(pair.Key))
            {
                continue;
            }
            value += pair.Value;
            newProbalility.Add(pair.Key,value);
        }
        //最终返回的itemList
        List<ItemScriptableObject> finallyItemObj = new List<ItemScriptableObject>();
        int forCount = count;
        if (canRandomItemId.Count<count)
        {
            forCount = canRandomItemId.Count;
        }

        for (int i = 0; i < forCount; i++)
        {
            int itemId = -1;//物品id
            float minProbal = 999999;
            float randomValue = Random.Range(0f, 1f);
            foreach (KeyValuePair<int,float> pair in newProbalility)
            {
                if (randomValue <= pair.Value && minProbal>pair.Value)
                {
                    itemId = pair.Key;
                    minProbal = pair.Value;
                }
            }

            if (itemId == -1)
            {
                i--;
            }
            else
            {
                finallyItemObj.Add(DataManager.Inst.GetItemScrObj(itemId));
                newProbalility.Remove(itemId);
            }
            if (newProbalility.Count == 0)
            {
                break;
            }
        }
        
        return finallyItemObj;
    }

    List<int> GetAllItemId()
    {
        List<int> itemIds = new List<int>();
        foreach (ItemScriptableObject itemScrObj in Items)
        {
            if (itemScrObj == null)
            {
                Debug.LogError("错误：物品池有空的！！！");
                continue;
            }
            itemIds.Add(DataManager.Inst.GetItemId(itemScrObj));
        }
        return itemIds;
    }
}
