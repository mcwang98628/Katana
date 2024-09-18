using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    [Serializable]
    public class TriggerEvent:UnityEvent<Collider> { }

    public TriggerEvent OnTriggerEnterEvent;
    public TriggerEvent OnTriggerExitEvent;
    
    public void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent.Invoke(other);
    }
    public void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent.Invoke(other);
    }
}
