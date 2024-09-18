using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("Role攻击.")]
public class AIRoleAttack : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("攻击这个目标.")]
    public SharedRoleController TargetGameObject;

    public SharedRoleController RoleController;
    public SharedBool SmoothRot = true;
    public SharedBool KeepAttacking = false;
    private bool isok=false;
    public override void OnStart()
    {
        if (RoleController == null)
        {
            RoleController = transform.GetComponent<RoleController>();
        }
        RoleController.Value.InputMove(Vector2.zero);
        isok = true;
        RoleController.Value.InputAttack();
        // if (RoleController.Value.IsAcceptAttackInput)
        // {
        //     // Vector3 v3 = TargetGameObject.Value.transform.position - gameObject.transform.position;
        //     // if (SmoothRot.Value)
        //     // {
        //     //     RoleController.Value.Animator.transform.DORotateQuaternion(Quaternion.LookRotation(new Vector3(v3.x, 0, v3.z)), 0.3f).OnComplete(() =>
        //     //     {
        //     //         isok = true;
        //     //         RoleController.Value.InputAttack();
        //     //     }
        //     //    ).SetEase(Ease.Linear);
        //     // }
        //     // else
        //     // {
        //     //     RoleController.Value.Animator.transform.forward = new Vector3(v3.x, 0, v3.z);
        //     //     isok = true;
        //     //     RoleController.Value.InputAttack();
        //     // }
        //     
        // }
        
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
        if (KeepAttacking.Value)
        {
            if (RoleController.Value.IsAttacking || !isok)
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
            if (!isok)
            {
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Success;
            }
        }
        
    }

    public override void OnReset()
    {
            
    }
}