using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardServant : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==BattleManager.Inst.CurrentPlayer.gameObject)
        {
            BattleManager.Inst.CurrentPlayer.SetGod(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == BattleManager.Inst.CurrentPlayer.gameObject)
        {
            BattleManager.Inst.CurrentPlayer.SetGod(false);
        }
    }
}
