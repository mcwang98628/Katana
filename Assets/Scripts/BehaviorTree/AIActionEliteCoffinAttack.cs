using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;
using DG.Tweening;
using UnityEngine;


public class AIActionEliteCoffinAttack : AIAction
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



            //if (enemyCoffinAttack == null)
            //{
            //    enemyCoffinAttack = RoleController.GetComponent<EnemyEliteCoffinAttack>();
            //}
            //int CreatureCount = enemyCoffinAttack.GetSpwanCreaturesCount();


            //waitTime = Random.Range(MinWaitTime.Value, MaxWaitTime.Value) + AddTimeOnHaveCreature.Value * CreatureCount;

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
