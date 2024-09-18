using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleContinueDamageArea : MonoBehaviour
{
    RoleController roleController;
    public float damage;
    public DmgBuffOnTouch DmgArea;
    public bool DestroyOnDead;
    void Start()
    {
        roleController=GetComponent<RoleController>();
        DmgArea.Init(roleController,damage);
        EventManager.Inst.AddEvent(EventName.OnRoleDead,OnEnemyDead);
    }
    private void OnDestroy() {
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead,OnEnemyDead);
    }

    private void OnEnemyDead(string arg1, object arg2)
    {
        RoleDeadEventData data = (RoleDeadEventData)arg2;
        if(data.DeadRole == roleController){
            DmgArea.enabled=false;
            ParticleSystem[] particles=GetComponentsInChildren<ParticleSystem>();
            foreach(var particle in particles)
            {
                particle.Stop();
            }
        }
    }
}
