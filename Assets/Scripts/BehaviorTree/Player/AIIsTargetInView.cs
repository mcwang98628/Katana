using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("判断target是否在范围内，若是reverse则判断是否在范围外.")]
public class AIIsTargetInView : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("攻击这个目标.")]
    public SharedRoleController Target;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("距离.")]
    public SharedFloat Distance;
    public SharedInt Angle;
    public SharedRoleController RoleController;



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



        
        //贴身误判保护
        if (v3.magnitude < 0.5f)
        {
            return TaskStatus.Success;
        }

        if (v3.magnitude < Distance.Value && Vector3.Angle(v3, RoleController.Value.Animator.transform.forward) <Angle.Value/2)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    public override void OnReset()
    {
            
    }
}