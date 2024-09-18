using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("单位的攻击行为。")]
public class AIActionAttack : AIAction
{
    bool FirstFrame=true;
    public SharedInt AttackState=0;
    public override void OnAwake()
    {
        base.OnAwake();
       
    }
    public override void OnStart()
    {
        base.OnStart();
        RoleController.InputMove(Vector2.zero);
        //Debug.Log(AttackState.Value);
        RoleController.InputAttack(AttackState.Value);
        FirstFrame = true; 
        //Debug.Log(RoleController.IsAttacking);
    }
    

    public override TaskStatus OnUpdate()
    {
        //if(!FirstFrame)
        //if (RoleController.IsAttacking)
        //{
        //    return TaskStatus.Running;
        //}
        //else
        //{
        //        FirstFrame = true;
        //        Debug.Log("EliteZombieFinishAttack");
        //    return TaskStatus.Success;
        //}
        //else
        //{
        //    FirstFrame = false;
        //    return TaskStatus.Running;
        //}
        if (!FirstFrame)
        {
            if (RoleController.IsAttacking)
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
