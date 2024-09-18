using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideDoorEvent : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player") &&
            other.gameObject.layer != LayerMask.NameToLayer("PlayerRoll"))
        {
            return;
        }
        
        GuideManager.Inst.OnExitDoor();
    }
}
