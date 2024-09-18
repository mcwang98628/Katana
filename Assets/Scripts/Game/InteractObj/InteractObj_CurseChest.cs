using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractObj_CurseChest : InteractObj_Chest
{
    //public ItemScriptableObject Curse;
    public ItemScriptableObject CurseItem;
    public override void InteractEnd()
    {
        base.InteractEnd();
        BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(CurseItem),isOk=>{});
    }
}
