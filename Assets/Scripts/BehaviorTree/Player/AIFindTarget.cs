using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("找最近的目标.")]
public class AIFindTarget : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("找到的enemy.")]
    public SharedRoleController Target;

    //public SharedVector3 TargetV3;
    public SharedRoleController CurrentRoleController;
    public SharedFloat findDis;

    public override void OnStart()
    {
        if (CurrentRoleController == null)
        {
            CurrentRoleController = transform.GetComponent<RoleController>();
        }
    }
    public override TaskStatus OnUpdate()
    {

        RoleController role = BattleTool.FindNearestEnemy(CurrentRoleController.Value,findDis.Value);

        if (role != null)
        {
            if (Target != null)
            {
                Target.Value = role;
            }
            
            if (CurrentRoleController.Value.IsPlayer)
            {
                //IndicatorManager.Inst.SetTargetIndicator(role.transform);
            }
        }

        if (Target.Value == null)
        {
            return TaskStatus.Failure;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}
