using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
public class AIRoleIsHaveTarget : Action
{
    public SharedRoleController Target;
    public SharedBool Reverse;
    public override void OnStart()
    {
            
    }

    public override TaskStatus OnUpdate()
    {
        if (Target.Value == null)
        {
            if (Reverse.Value)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
        else
        {
            if (Reverse.Value)
            {
                return TaskStatus.Failure;
            }
            else
            {
                return TaskStatus.Success;
            }
        }
    }

    public override void OnReset()
    {
            
    }
}