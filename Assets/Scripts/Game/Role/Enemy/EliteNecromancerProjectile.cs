using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteNecromancerProjectile : DmgBuffOnTouch
{
    public float Speed;
    public float RotateSpeed;
    protected override void Update()
    {
        base.Update();
        transform.Translate(Vector3.forward*Time.deltaTime*Speed,Space.Self);
        transform.LerpLookAt(BattleManager.Inst.CurrentPlayer.transform,RotateSpeed*Time.deltaTime);
    }
}
