using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
public class AIRoleStopMove : Action
{
    public SharedRoleController RoleController;
    
        

    public override void OnStart()
    {
            
    }

    public override TaskStatus OnUpdate()
    {
        RoleController.Value.InputMove(Vector2.zero);
        
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }
}