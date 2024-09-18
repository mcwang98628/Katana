using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("判断target存活.")]
public class AIJudgingTargetIsAlive : Action
{

    public SharedRoleController roleController;
        

    public override void OnStart()
    {
            
    }

    public override TaskStatus OnUpdate()
    {
        if (roleController.Value.IsDie)
        {
            return TaskStatus.Failure;
        }
        else
        {
            return TaskStatus.Success;
        }
    }

    public override void OnReset()
    {
            
    }
}