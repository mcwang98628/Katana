using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("史莱姆的攻击。")]
public class AIActionSlimeAttack : AIAction
{
    public override void OnStart()
    {
        base.OnStart();
        var uAttacker = RoleController.roleAttack as EnemySlime_Attacker;
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