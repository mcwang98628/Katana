using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家目前的Item流派信息
/// </summary>
public struct PlayerItemSchoolInfo 
{
    //流派中缺少的Item
    public Dictionary<int,List<int>> ItemSchoolLackItem;
    //不包含的流派
    public List<int> NotContainItemSchool;
    //已经完整拥有的流派
    public List<int> ContainItemSchool;

    public PlayerItemSchoolInfo(Dictionary<int,List<int>> lackItem,List<int> notContainItemSchool,List<int> containItemSchool)
    {
        ItemSchoolLackItem = lackItem;
        NotContainItemSchool = notContainItemSchool;
        ContainItemSchool = containItemSchool;
    }

    /// <summary>
    /// 获取所有缺少的流派Item
    /// </summary>
    /// <returns></returns>
    public List<int> GetAllLackItem()
    {
        List<int> itemList = new List<int>();
        foreach (var items in ItemSchoolLackItem)
        {
            itemList.AddRange(items.Value);
        }
        return itemList;
    }
}
