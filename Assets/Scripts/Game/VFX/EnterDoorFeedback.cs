using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoorFeedback : MonoBehaviour
{
    bool PlayerIsIn;
    public Vector3 StartPos;
    public float CloseDoorMul;
    public GameObject RotationRef;
    // public bool IsEnteringNextRoom;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            PlayerIsIn = true;
            StartPos = BattleManager.Inst.CurrentPlayer.transform.position;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Player")
        {
            PlayerIsIn = false;
            //UIManager.Inst.SetPercentageMask(0);
            //UIManager.Inst.SetPercentageMask(0);
        }
    }

    private bool isOver = false;
    private void Update()
    {
        if (isOver)
        {
            return;
        }
        if(PlayerIsIn)
        {
            // Vector3 PlayerPos = BattleManager.Inst.CurrentPlayer.transform.position;
            
            // float Dis = -Vector3.Dot((PlayerPos - StartPos), RotationRef.transform.TransformDirection(Vector3.forward)) * CloseDoorMul;
            
            // if(Dis>1f&&!IsEnteringNextRoom)
            // if(!IsEnteringNextRoom)
            // {
            //     IsEnteringNextRoom = true;
            // }
            // isOver = true;
            // GetComponentInParent<RoomDoor>().EnterNextRoom();
        }
    }
    

}
