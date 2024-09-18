using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedField : MonoBehaviour
{
    public float SpeedMul;
    bool IsModified;
    //float SaveSpeed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"&&!IsModified)
        {
            BattleManager.Inst.CurrentPlayer.GetComponent<RoleMove>().SetMultiplyMoveSpeed(SpeedMul);
            IsModified = true;
        }
        //Debug.Log("PlayerIsIn");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player"&&IsModified)
        {
            BattleManager.Inst.CurrentPlayer.GetComponent<RoleMove>().SetMultiplyMoveSpeed(1 / SpeedMul);
            IsModified = false;
        }
        //Debug.Log("PlayerIsIn");
    }
    private void OnDestroy()
    {
        if(IsModified)
        {
            if(BattleManager.Inst.CurrentPlayer!=null)
                BattleManager.Inst.CurrentPlayer.GetComponent<RoleMove>().SetMultiplyMoveSpeed(1 / SpeedMul);
        }
    }
}
