using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("Role停止攻击.")]
public class AIRoleStopAttack : Action
{
    public SharedRoleController RoleController;

    public override void OnStart()
    {
            
    }

    public override TaskStatus OnUpdate()
    {
        RoleController.Value.StopAttack();
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
            
    }
}