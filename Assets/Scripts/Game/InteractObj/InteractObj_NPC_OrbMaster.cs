using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractObj_NPC_OrbMaster : InteractObj_NPC_YesNo
{
    [Header("给Orb能力")]
    public NPCTalkObject UpgradeOrbItemDialog;
    [Header("替换item为orb")]
    public NPCTalkObject ReplaceItemWithOrbDialog;
    public ItemReplaceFormulas ReplaceFormula;

    protected override void Init()
    {
        base.Init();
        if (BattleManager.Inst.CurrentPlayer.roleSurroundController.SurroundObjs.Count > 3)
        {
            DialogData = UpgradeOrbItemDialog;
        }
        else
        {
            DialogData = ReplaceItemWithOrbDialog;
        }
    }


    protected override void ChooseYes()
    {
        if (DialogData == ReplaceItemWithOrbDialog)
        {
            List<ItemReplaceFormulas.ReplaceFormula> replaceFormulas = ReplaceFormula.GetReplaceFormulas();
            foreach (ItemReplaceFormulas.ReplaceFormula formula in replaceFormulas)
            {
                for (int i = 0; i < formula.BaseItems.Count; i++)
                {
                    BattleManager.Inst.CurrentPlayer.roleItemController.ReMoveItemByID(DataManager.Inst.GetItemId(formula.BaseItems[i]), 1);
                }
                BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(formula.ResultItem),isOk=>{});
            }
        }

        base.ChooseYes();
    }
    protected override void ChooseNo()
    {
        base.ChooseNo();
    }

}
