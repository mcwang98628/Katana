using System;
using System.Collections.Generic;
using UnityEngine;


public enum ItemPoolType
{
    Lv1,
    Lv2,
    Lv3,
    Shop,
}

[CreateAssetMenu(fileName = "NewItemPool", menuName = "Item/ItemPool", order = 0)]
public class ItemPool : ScriptableObject
{
    public List<ItemPoolData> Lv1 = new List<ItemPoolData>();
    public List<ItemPoolData> Lv2 = new List<ItemPoolData>();
    public List<ItemPoolData> Lv3 = new List<ItemPoolData>();
    public List<ItemPoolData> Shop = new List<ItemPoolData>();
}

[Serializable]
public class ItemPoolData
{
    public ItemScriptableObject Item;
    public int ProbabilityWeight;
    public int Price;
}
