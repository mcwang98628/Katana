using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("判断target是否在视野内.")]

public class AIConditionIsInView : AICondition
{
    public SharedFloat Distance;
    public SharedFloat Angle;

    public override TaskStatus OnUpdate()
    {
       
        Target = BattleManager.Inst.CurrentPlayer;
        if (Target == null)
        {
            return TaskStatus.Failure;
        }

        Vector3 dir = Target.transform.position - transform.position;
        
        //贴身误判保护，太近的话也会认为在范围内
        if (dir.magnitude < 0.5f)
        {
            return TaskStatus.Success;
        }


        if (dir.magnitude < Distance.Value  && Vector3.Angle(dir, RoleController.Animator.transform.forward) <Angle.Value/2)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
    
}