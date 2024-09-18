using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxerNuke : MonoBehaviour
{
    public DmgBuffOnTouch Radiation;
    private void Start()
    {
        Radiation.Init(BattleManager.Inst.CurrentPlayer, 20f);
    }
}
