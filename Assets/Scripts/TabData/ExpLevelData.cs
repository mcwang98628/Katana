using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ExpLevelData
{
    public int Level;
    public int UpgradeNeedExp;
    public int RuneItemId;
    public int RuneItemPrice;

    public ExpLevelData(int level,int exp,int runeItem,int runeItemPrice)
    {
        Level = level;
        UpgradeNeedExp = exp;
        RuneItemId = runeItem;
        RuneItemPrice = runeItemPrice;
    }
}
