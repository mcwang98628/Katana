using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomItemBox : MonoBehaviour
{
    [SerializeField]
    private int lv1;
    [SerializeField]
    private int lv2;
    [SerializeField]
    private int lv3;
    [SerializeField]
    private GameObject lv1Box;
    [SerializeField]
    private GameObject lv2Box;
    [SerializeField]
    private GameObject lv3Box;

    private void Awake()
    {
        if (BattleManager.Inst.CurrentPlayer.GetTagCount(RoleTagName.StrengthenReward) > 0)
        {
            lv3Box.SetActive(true);
            return;
        }
        int randomValue = Random.Range(0, 100);
        if (randomValue <= lv1)
        {
            lv1Box.SetActive(true);
        }
        else if (randomValue <= lv2+lv1)
        {
            lv2Box.SetActive(true);
        }
        else if (randomValue <= lv3+lv2+lv1)
        {
            lv3Box.SetActive(true);
        }
        
    }
}
