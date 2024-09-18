using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionPanel : PanelBase
{
    public void OnBtnClick()
    {
        UIManager.Inst.Close();
        BattleManager.Inst.Resurrection();
    }
}
