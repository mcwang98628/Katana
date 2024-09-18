using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("Role移动.")]
public class AIRoleMoveTarget : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("移动到这个目标.")]
    public SharedVector3 TargetV3;
    public SharedBool AlwaysFollowTarget;
    public SharedRoleController Target;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("停止距离,距离目标过近就停止移动.")]
    public SharedFloat StopLength;

    public SharedRoleController RoleController;


    private float Timer = 0;
    private float MaxTime = 999f;
    public override void OnStart()
    {
        Timer = 0;
    }

    public override TaskStatus OnUpdate()
    {
        if (TargetV3 == null)
        {
            RoleController.Value.InputMove(Vector2.zero);
            return TaskStatus.Failure;
        }
        
        Vector3 v3 = TargetV3.Value - gameObject.transform.position;

        //朝着目标前进而不是朝着目标之前所在位置前进
        if (AlwaysFollowTarget.Value)
        {
            if(Target.Value!=null)
                v3= Target.Value.transform.position - gameObject.transform.position;
        }

        if (v3.magnitude <= StopLength.Value)//已经到达目标点
        {
            v3 = Vector3.zero;
            return TaskStatus.Success;
        }
        RoleController.Value.InputMove(new Vector2(v3.x,v3.z));
        Timer += Time.deltaTime;
        if (Timer>=MaxTime)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

    public override void OnReset()
    {
        TargetV3 = null;
        StopLength = 1;
    }
}