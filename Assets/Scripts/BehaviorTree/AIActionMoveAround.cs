using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("Role往边上移动.")]
public class AIActionMoveAround : AIAction 
{
    public SharedFloat MaxMoveTime;
    public SharedFloat MinMoveTime;
    float moveTime=-1;
    float moveTimer;
    Vector3 dir;
    float RotSpeed = -1;

    public override void OnStart()
    {
        base.OnStart();
        if (Target == null)
        {
            return;
        }
         if (RotSpeed < 0)
        {
            RotSpeed = (RoleController as EnemyController).GetRotateSpeed();
        }

        Vector3 fwd =transform.position- Target.transform.position;
        fwd = Quaternion.Euler(new Vector3(0, Random.value>0.5f? 45:-45, 0)) * fwd;
        Vector3 des = Target.transform.position + fwd;
        dir = des - transform.position;
        //float MoveDistance = ;

        //float angle = 0;
        //Vector3 dirTemp = Vector3.zero;
        //RaycastHit hit;
        //int tryCount = 0; 
        //while (MoveDistance < 2 )
        //{
        //    if (tryCount > 10)
        //    {
        //        dirTemp = dir;
        //        break;                
        //    }
        //    angle = (Random.value - 0.5f) >0?90:-90;
        //    dirTemp = Quaternion.Euler(new Vector3(0, angle, 0)) * dir;
        //    if (Physics.Raycast(transform.position, dirTemp, out hit))
        //    {
        //        MoveDistance = Vector3.Distance(transform.position, hit.point);
        //    }
        //    else
        //    {
        //        MoveDistance = 99;
        //    }
        //    tryCount++;
        //}

        //dir = dirTemp;
        moveTime =Random.Range(MinMoveTime.Value,MaxMoveTime.Value);
        moveTimer = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        if (Target == null)
        {
            return TaskStatus.Failure;
        }
       

        if (Time.time > moveTimer + moveTime)
        {
            //Debug.Log("EndMove");
          
            RoleController.InputMove(Vector2.zero);
            return TaskStatus.Success;
        }
        if (!RoleController.IsFreeze)
        {
            Vector3 moveDir = Vector3.Slerp(RoleController.Animator.transform.forward, dir, RotSpeed * Time.deltaTime);
            RoleController.InputMove(new Vector2(moveDir.x, moveDir.z).normalized);
        }
        return TaskStatus.Running;
    }
    
}