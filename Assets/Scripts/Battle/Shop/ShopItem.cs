using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem
{
    private ItemScriptableObject _item;
    // private float _priceIncrease;
    private int _price;
    private int _number;

    public ShopItem(
        ItemScriptableObject item,
        // float priceIncrease,
        int price,
        int number
        )
    {
        _item = item;
        // _priceIncrease = priceIncrease;
        _price = price;
        _number = number;
    }
    
    public int Price
    {
        get
        {
            int _magnification = 1;
            var runtimeData = BattleManager.Inst.RuntimeData;
            if (runtimeData is ChapterRulesRuntimeData chapterRulesRuntimeData)
            {
                _magnification = chapterRulesRuntimeData.CurrentLevelIndex;
            }
            else if (runtimeData is EndlessRulesRuntimeData endlessRulesRuntimeData)
            {
                _magnification = 0;
            }
            else
            {
                _magnification = 0;
            }

            return (int) (_price);// + (_priceIncrease * _magnification));
        }
    }

    public int OriginalPrice => _price;
    // public float PriceIncrease => _priceIncrease;
    public int Number => _number;
    public ItemScriptableObject Item => _item;

    public void ChangeNumber(int number)
    {
        _number += number;
    }
}
