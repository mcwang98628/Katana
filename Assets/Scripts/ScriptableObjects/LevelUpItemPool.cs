using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameItem/LevelUpItemPool")]
public class LevelUpItemPool : ScriptableObject
{

    public List<ItemScriptableObject> LevelUpItemList;


    public List<ItemScriptableObject> GetItemsAt(int lv)
    {
        lv -= 1;

        List<ItemScriptableObject> Result = new List<ItemScriptableObject>();

        List<int> randList = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, LevelUpItemList.Count);
            while (randList.Contains(rand))
            {
                rand = Random.Range(0, LevelUpItemList.Count);
            }

            randList.Add(rand);
            Result.Add(LevelUpItemList[rand]);
        }


        return Result;

    }
}
