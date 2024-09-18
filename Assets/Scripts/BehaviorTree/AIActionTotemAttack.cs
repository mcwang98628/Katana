using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("图腾攻击。")]
public class AIActionTotemAttack : AIAction
{
    public override void OnStart()
    {
        base.OnStart();
        var uAttacker = RoleController.roleAttack as EnemyToTem_Attacker;
        if (uAttacker)
        {
            uAttacker.AttackFunc();
        }
    }
    

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }

}