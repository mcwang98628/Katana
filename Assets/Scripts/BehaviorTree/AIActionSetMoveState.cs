using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
public class AIActionSetMoveState : AIAction
{
    public SharedFloat moveStatus;
    private static readonly int MoveStatus = Animator.StringToHash("MoveStatus");

    public override TaskStatus OnUpdate()
    {
        RoleController.Animator.SetFloat(MoveStatus, moveStatus.Value);
        return TaskStatus.Success;
    }
}