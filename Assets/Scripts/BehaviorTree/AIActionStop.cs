using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
public class AIActionStop : AIAction
{
    public override TaskStatus OnUpdate()
    {
        RoleController.InputMove(Vector2.zero);
        
        return TaskStatus.Success;
    }
    
}