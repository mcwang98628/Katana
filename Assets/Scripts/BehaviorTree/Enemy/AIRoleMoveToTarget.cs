using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("Role移动到.")]
public class AIRoleMoveToTarget : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("移动到这个目标.")]
    public SharedVector3 TargetV3;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("停止距离,距离目标过近就停止移动，返回True")]
    public SharedFloat StopLength;

    public SharedRoleController RoleController;
    
        

    public override void OnStart()
    {
            
    }

    public override TaskStatus OnUpdate()
    {
        if (TargetV3 == null)
        {
            RoleController.Value.InputMove(Vector2.zero);
            return TaskStatus.Failure;
        }
        
        Vector3 v3 = TargetV3.Value - gameObject.transform.position;
        if (v3.magnitude <= StopLength.Value)//已经到达目标点
        {
            v3 = Vector3.zero;
            return TaskStatus.Success;
        }
        RoleController.Value.InputMove(new Vector2(v3.x,v3.z));
        
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        TargetV3 = null;
        StopLength = 1;
    }
}