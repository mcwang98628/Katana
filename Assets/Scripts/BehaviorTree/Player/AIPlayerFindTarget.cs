using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("找最近的目标.")]
public class AIPlayerFindTarget : Action
{
    public SharedRoleController Target;

    public SharedRoleController CurrentRoleController;

    public override void OnStart()
    {
    }
    public override TaskStatus OnUpdate()
    {
        if (CurrentRoleController != null && CurrentRoleController.Value != null)
        {
            Target.Value = CurrentRoleController.Value.EnemyTarget;
        }
        return TaskStatus.Success;
    }
}
