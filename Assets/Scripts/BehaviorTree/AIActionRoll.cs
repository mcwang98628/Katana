using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("单位翻滚。")]
public class AIActionRoll : AIAction
{
    bool FirstFrame=true;
    public SharedVector2 Direction;
    public SharedFloat MaxRandomAngleOffset;
    public override void OnAwake()
    {
        base.OnAwake();
       
    }
    public override void OnStart()
    {
        base.OnStart();
        RoleController.InputMove(Vector2.zero);
        
        Vector3 dir3 = (Direction.Value.x * RoleController.Animator.transform.right + Direction.Value.y * RoleController.Animator.transform.forward);
        if (MaxRandomAngleOffset.Value > 0)
        {
            float randomAngle = Random.Range(-MaxRandomAngleOffset.Value, MaxRandomAngleOffset.Value);
            dir3 = Quaternion.AngleAxis(randomAngle, Vector3.up) * dir3;
        }
        Vector2 dir2 = new Vector2(dir3.x, dir3.z).normalized;
        RoleController.InputRoll(dir2);
        FirstFrame = true; 
    }
    

    public override TaskStatus OnUpdate()
    {
       
        if (!FirstFrame)
        {
            if (RoleController.IsRolling)
            {
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Success;
            }
        }
        else
        {
            FirstFrame = false;
        }
        return TaskStatus.Running;
    }

}
