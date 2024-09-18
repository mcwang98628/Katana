using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAbleObj_Item : PickAbleObj
{
    public ItemScriptableObject Item;


    protected override void TakeEffect()
    {
        BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(
            DataManager.Inst.ParsingItemObj(Item),null);

        base.TakeEffect();
    }
}
