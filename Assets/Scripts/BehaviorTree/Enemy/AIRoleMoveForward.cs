using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("Role移动.")]
public class AIRoleMoveForward : Action
{
    public SharedRoleController RoleController;


    private float Timer = 0;
    private float MaxTime = 2f;
    public override void OnStart()
    {
        Timer = 0;
    }

    public override TaskStatus OnUpdate()
    {
        Vector3 fwd = RoleController.Value.Animator.transform.forward;
        RoleController.Value.InputMove(new Vector2(fwd.x, fwd.z).normalized);

        return TaskStatus.Success;
    }
    
}