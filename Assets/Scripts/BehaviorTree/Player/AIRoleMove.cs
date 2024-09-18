using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("Role移动.")]
public class AIRoleMove : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("移动到这个目标.")]
    public SharedRoleController TargetGameObject;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("停止距离,距离目标过近就停止移动.")]
    public SharedFloat StopLength;

    public SharedRoleController RoleController;
    
        

    public override void OnStart()
    {
            
    }

    public override TaskStatus OnUpdate()
    {
        if (RoleController.Value.IsAttacking)
        {
            return TaskStatus.Failure;
        }
        if (TargetGameObject.Value == null)
        {
            RoleController.Value.InputMove(Vector2.zero);
//            RoleView.Value.SetMoveDirection(Vector2.zero);
            return TaskStatus.Failure;
        }
        
        Vector3 v3 = TargetGameObject.Value.transform.position - gameObject.transform.position;
        if (v3.magnitude <= StopLength.Value)//已经到达目标点
        {
            v3 = Vector3.zero;
        }

        var input = new Vector2(v3.x, v3.z).normalized;
        RoleController.Value.InputMove(input);
//        RoleView.Value.SetMoveDirection(new Vector2(v3.x,v3.z));
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        TargetGameObject = null;
        StopLength = 1;
    }
}