using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ShowAttackCircle : MonoBehaviour
{
    public string attackDisName="AttackDistance";
    public Material mat;

    private void Start()
    {
        
        CircleInser.DrawCircle(gameObject, (float)GetComponentInParent<BehaviorTree>().GetVariable(attackDisName).GetValue(), 0.03f);
        if (mat != null)
        {
            GetComponent<LineRenderer>().material = mat;
        }
    }
}
