using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("角色朝向玩家.")]
public class AIActionLookAt : AIAction
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("朝向玩家.")]
    public SharedFloat StopAngle = 10;
    float RotSpeed = -1;

    public override void OnAwake()
    {
        base.OnAwake();
        RotSpeed = (RoleController as EnemyController).GetRotateSpeed();
        if(RoleController!=null)
        {
            RoleController.InputMove(Vector2.zero);
            //RoleController.StopFastMove();
        }
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
        //Debug.Log(Target);
        if (Target == null)
        {
            Target = BattleManager.Inst.CurrentPlayer;
            if (Target == null)
            {
                Debug.Log("没找到玩家");
                return TaskStatus.Failure;
            }
        }
        if (RotSpeed < 0)
        {
            RotSpeed = (RoleController as EnemyController).GetRotateSpeed();
        }
        
        dir = Target.transform.position - gameObject.transform.position;
        dir.y = 0;
       
        if(RoleController.IsFreeze)
        {
            //Debug.Log(RoleController.IsFreeze);
        }
        else
        {
           
            int rotDir=Vector3.Cross(RoleController.Animator.transform.forward, dir).y>0?1:-1;
            RoleController.Animator.transform.forward = Quaternion.Euler(0,rotDir*RotSpeed,0)*RoleController.Animator.transform.forward;
        }
        float angle = Vector3.Angle(dir, RoleController.Animator.transform.forward);


        if (angle < StopAngle.Value||(StopAngle.Value<RotSpeed/2&&angle < RotSpeed/2))
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
            //return TaskStatus.Failure;
        }
    }

}