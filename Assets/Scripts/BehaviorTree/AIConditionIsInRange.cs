using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("判断target是否在范围内.")]
public class AIConditionIsInRange : AICondition
{
    public SharedFloat Distance;

    public override TaskStatus OnUpdate()
    {

            Target = BattleManager.Inst.CurrentPlayer;
        if (Target == null)
        {
            return TaskStatus.Failure;
        }

        Vector3 dir = Target.transform.position - transform.position;
        

        if (dir.magnitude < Distance.Value)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
    
}