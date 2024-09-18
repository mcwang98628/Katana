using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDmgBuffOnTouch : DmgBuffOnTouch
{
    public float Speed;
    //public float FindDistance;
    public GameObject DeadParticles;
    bool IsInited;
    public RoleController _TargetController;
    public void SetTarget(RoleController Target)
    {
        _TargetController = Target;
        
    }
    protected override void Update()
    {
        base.Update();
        if (isInit)
        {
            if (_TargetController != null)
            {
                if(_TargetController.IsDie)
                {
                    Destroy(gameObject);
                }
                transform.LerpLookAt(_TargetController.transform, 0.8f);
                transform.Translate(Vector3.forward*Speed*Time.deltaTime);
            }
        }
    }
    public override void OtherEffect(RoleController Target)
    {
        base.OtherEffect(Target);
        transform.SetParent(Target.transform);
        Destroy(gameObject);
        //Destroy(this);
        if (DeadParticles != null)
        {
            DeadParticles.DuplicateAtPosition(transform.position);
        }
    }
}
