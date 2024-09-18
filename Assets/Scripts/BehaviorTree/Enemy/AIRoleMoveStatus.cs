using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
public class AIRoleMoveStatus : Action
{
    public SharedRoleController roleController;
    public SharedFloat moveStatus;
    private static readonly int MoveStatus = Animator.StringToHash("MoveStatus");

    public override void OnStart()
    {
        roleController.Value.Animator.SetFloat(MoveStatus,moveStatus.Value); 
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
            
    }
}