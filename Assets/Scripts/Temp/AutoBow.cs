using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBow : MonoBehaviour
{
    public GameObject ArrowPrefabs;
    public GameObject ArrowStartPoint;
    RoleController target;
    public float Distance;
    public float LerpLookAtMul;
    public Animator animator;
    public int AttackPower = 5;
    public float AttackInterval = 1;
    float timer=0;
    bool isAttacking = false;


    protected virtual void Awake()
    {
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
    }
    private void OnDestroy()
    {
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
    }
    private void FixedUpdate()
    {
        
        if (target == null)
        {
                target = BattleTool.FindNearestEnemy(transform, Distance);
        }
        else
        {
            if (target != null)
            {
                if (!target.IsDie)
                {
                    transform.LerpLookAt(target.transform, LerpLookAtMul);
                    if (Time.time - timer > AttackInterval)
                    {
                        timer = Time.time;
                        animator.SetTrigger("Shoot");
                    }
                }
                else
                {
                    target = BattleTool.FindNearestEnemy(transform, Distance);
                }
            }
        }
      
    }

    public void AnimEvent(GameObject go, string eventName)
    {
        if (go != animator.gameObject)
        {
            return;
        }

        if (eventName == "Shoot")
        {
            if (target != null)
            {
                ProjectileTrace CurrentArrow = Instantiate(ArrowPrefabs, ArrowStartPoint.transform.position, ArrowStartPoint.transform.rotation).GetComponent<ProjectileTrace>();
                CurrentArrow.transform.forward = target.transform.position - ArrowStartPoint.transform.position;
                CurrentArrow.Init(BattleManager.Inst.CurrentPlayer, AttackPower);
                CurrentArrow.SetTarget(target);
            }
        }
    }
}
