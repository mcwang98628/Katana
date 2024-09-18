using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("判断target是否在范围内，若是reverse则判断是否在范围外.")]
public class AIJudgingTargetDistance : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("攻击这个目标.")]
    public SharedRoleController Target;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("距离.")]
    public SharedFloat Distance;

    
    public SharedBool Reverse;
        

    public override void OnStart()
    {
            
    }

    public override TaskStatus OnUpdate()
    {
        if (Target.Value == null)
        {
            return TaskStatus.Failure;
        }
        Vector3 v3 = Target.Value.transform.position - transform.position;
        if (v3.magnitude > Distance.Value)
        {
            
            return Reverse.Value ? TaskStatus.Success: TaskStatus.Failure;
        }
        else
        {
            return (!Reverse.Value )? TaskStatus.Success : TaskStatus.Failure;
        }
    }

    public override void OnReset()
    {
            
    }
}