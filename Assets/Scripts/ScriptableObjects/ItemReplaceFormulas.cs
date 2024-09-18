using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameItem/ItemReplaceFormulas")]
public class ItemReplaceFormulas : ScriptableObject
{
    [System.Serializable]
    public class ReplaceFormula
    {
        public List<ItemScriptableObject> BaseItems = new List<ItemScriptableObject>();
        public ItemScriptableObject ResultItem = null;
    }

    [SerializeField]
    public List<ReplaceFormula> SynthesisFormulas = new List<ReplaceFormula>();
    public ReplaceFormula GetReplaceFormula()
    {
        List<ReplaceFormula> formulas = new List<ReplaceFormula>();
        foreach (Item item in BattleManager.Inst.CurrentPlayer.roleItemController.Items)
        {
            for (int i = 0; i < SynthesisFormulas.Count; i++)
            {
                for (int j = 0; j < SynthesisFormulas[i].BaseItems.Count; j++)
                {
                    if (item.ID == DataManager.Inst.GetItemId(SynthesisFormulas[i].BaseItems[j]))
                    {
                        formulas.Add(SynthesisFormulas[i]);
                        continue;
                    }
                }
            }
        }

        List<ReplaceFormula> ResultFormulas = new List<ReplaceFormula>();
        for (int i = 0; i < formulas.Count; i++)
        {
            bool misItem = false;
            for (int j = 0; j < formulas[i].BaseItems.Count; j++)
            {
                int itemId = DataManager.Inst.GetItemId(formulas[i].BaseItems[j]);
                if (!BattleManager.Inst.CurrentPlayer.roleItemController.IsHaveItem(itemId))
                {
                    misItem = true;
                }
            }
            if (!misItem)
            {
                ResultFormulas.Add(formulas[i]);
            }
        }

        if (ResultFormulas.Count == 0)
        {
            Debug.Log("没有满足条件的合成公式");
            return null;
        }
        else
        {
            ReplaceFormula result = ResultFormulas[Random.Range(0, ResultFormulas.Count - 1)];

            string str = "";
            for (int i = 0; i < result.BaseItems.Count; i++)
            {
                str += result.BaseItems[i].name;
                if (i < result.BaseItems.Count - 1)
                    str += "+";
            }
            str += "=";
            str += result.ResultItem.name;

            Debug.Log(str);
            return result;
        }
    }
    public List<ReplaceFormula> GetReplaceFormulas()
    {
        List<ReplaceFormula> formulas = new List<ReplaceFormula>();
        foreach (Item item in BattleManager.Inst.CurrentPlayer.roleItemController.Items)
        {
            for (int i = 0; i < SynthesisFormulas.Count; i++)
            {
                for (int j = 0; j < SynthesisFormulas[i].BaseItems.Count; j++)
                {
                    if (item.ID == DataManager.Inst.GetItemId(SynthesisFormulas[i].BaseItems[j]))
                    {
                        formulas.Add(SynthesisFormulas[i]);
                        continue;
                    }
                }
            }
        }

        List<ReplaceFormula> ResultFormulas = new List<ReplaceFormula>();
        for (int i = 0; i < formulas.Count; i++)
        {
            bool misItem = false;
            for (int j = 0; j < formulas[i].BaseItems.Count; j++)
            {
                int itemId = DataManager.Inst.GetItemId(formulas[i].BaseItems[j]);
                if (!BattleManager.Inst.CurrentPlayer.roleItemController.IsHaveItem(itemId))
                {
                    misItem = true;
                }
            }
            if (!misItem)
            {
                ResultFormulas.Add(formulas[i]);
            }
        }

        if (ResultFormulas.Count == 0)
        {
            Debug.Log("没有满足条件的合成公式");
            return null;
        }
        else
        {        
            return ResultFormulas;
        }
    }
}
