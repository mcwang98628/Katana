using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("Role移动.")]
public class AIActionMoveForward : AIAction 
{
    public override TaskStatus OnUpdate()
    {
        Vector3 fwd = RoleController.Animator.transform.forward;
        RoleController.InputMove(new Vector2(fwd.x, fwd.z).normalized);

        return TaskStatus.Success;
    }
    
}