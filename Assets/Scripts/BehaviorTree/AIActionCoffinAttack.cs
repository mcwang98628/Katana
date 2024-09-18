using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("棺材生成怪物攻击。")]
public class AIActionCoffinAttack : AIAction
{

    public SharedFloat MaxWaitTime;
    public SharedFloat MinWaitTime;

    public SharedFloat AddTimeOnHaveCreature;
    float waitTime;
    private float timer;
    private EnemyCoffinAttack enemyCoffinAttack;

    public override void OnAwake()
    {
        base.OnAwake();
        timer = Time.time;
        waitTime = Random.Range(MinWaitTime.Value, MaxWaitTime.Value);

    }
    public override TaskStatus OnUpdate()
    {
        if (Time.time > timer + waitTime)
        {
            RoleController.InputAttack();


            int CreatureCount = 0;
            if (enemyCoffinAttack == null)
            {
                enemyCoffinAttack = RoleController.GetComponent<EnemyCoffinAttack>();
            }
            if (enemyCoffinAttack != null)
            {
                CreatureCount = enemyCoffinAttack.GetSpwanCreaturesCount();
            }

            waitTime = Random.Range(MinWaitTime.Value, MaxWaitTime.Value) + AddTimeOnHaveCreature.Value * CreatureCount;
            
            //Debug.LogError(CreatureCount);
            timer = Time.time;

            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }

    }

}