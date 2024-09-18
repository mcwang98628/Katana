using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("角色朝向玩家.")]
public class AIActionMoveToTarget : AIAction
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("朝向玩家.")]

    float RotSpeed = 0;
    public SharedFloat StopDistance = 1;
    public override void OnAwake()
    {
        base.OnAwake();
        RotSpeed = (RoleController as EnemyController).GetRotateSpeed();
    }

    private Vector3 dir;

    public override void OnStart()
    {
        base.OnStart();
        dir = Target.transform.position - gameObject.transform.position;
        dir.y = 0;
    }
    public override TaskStatus OnUpdate()
    {
        if (Target == null)
        {
            Target = BattleManager.Inst.CurrentPlayer;
            return TaskStatus.Failure;
        }
        if (RotSpeed < 0)
        {
            RotSpeed = (RoleController as EnemyController).GetRotateSpeed();
        }
        
        dir = Target.transform.position - gameObject.transform.position;
        dir.y = 0;
        
        if (Vector3.Distance(transform.position, Target.transform.position)< StopDistance.Value)
        {
            RoleController.InputMove(new Vector2(0, 0));
            return TaskStatus.Success;
        }

        else
        {
            if (!RoleController.IsFreeze)
            {
                //获取角度
                float angle = Vector3.Angle(dir, RoleController.Animator.transform.forward);
                //大部分情况下，旋转的角度小于夹角
                float rotSpeed = (RotSpeed < angle )? RotSpeed : Mathf.Abs(angle);


                int rotDir=Vector3.Cross(RoleController.Animator.transform.forward, dir).y>0?1:-1;
                
                RoleController.Animator.transform.forward = Quaternion.Euler(0,rotDir*rotSpeed,0)*RoleController.Animator.transform.forward;
                Vector3 fwd = RoleController.Animator.transform.forward;
                RoleController.InputMove(new Vector2(fwd.x, fwd.z).normalized);
            }
            return TaskStatus.Running;
        }


       

    }

}