using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDieOnOwner : MonoBehaviour
{
    RoleController _controller;
    private void OnEnable()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleDead, CloseParticles);
        _controller = GetComponentInParent<RoleController>();
        if (_controller==null)
            Destroy(this);
    }
    private void OnDisable()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead, CloseParticles);
    }
    public void CloseParticles(string a1,object a2)
    {
        if(_controller.IsDie)
        {
            Destroy(gameObject);
        }
    }

}
