using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerEnterDetector : MonoBehaviour
{
    public UnityEvent eve;
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag=="Player")
        {
            //Debug.Log("PlayerEnter");
            eve.Invoke();
        }
    }
}
