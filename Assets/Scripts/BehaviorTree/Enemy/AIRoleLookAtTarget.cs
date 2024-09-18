using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("Role朝向目標.")]
public class AIRoleLookAtTarget : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("移动到这个目标.")]
    public SharedRoleController Target;
    public SharedBool SmoothLookAt;
    public SharedFloat RotSmooth;

    public SharedRoleController RoleController;


    private float Timer = 0;
    private float MaxTime = 2f;
    public override void OnStart()
    {
        Timer = 0;
    }

    public override TaskStatus OnUpdate()
    {
        if (Target == null)
        {
            RoleController.Value.InputMove(Vector2.zero);
            return TaskStatus.Failure;
        }
        
        Vector3 dir =Target.Value.transform.position - transform.position;
        dir.y = 0;
        dir = dir.normalized;

        if (!RoleController.Value.IsAttacking)
        {
            //平滑旋转
            if (SmoothLookAt.Value)
            {
                RoleController.Value.Animator.transform.forward = Vector3.Lerp(RoleController.Value.Animator.transform.forward, dir, RotSmooth.Value);
                //防侧倾
                Vector3 euler= RoleController.Value.Animator.transform.eulerAngles;
                euler.x = 0; euler.z=0;
                RoleController.Value.Animator.transform.eulerAngles = euler;
            }
            else
            {
                RoleController.Value.Animator.transform.forward = dir;
            }
        }
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        Target.Value = null;
    }
}