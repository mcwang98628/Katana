using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class EnemyDodge : MonoBehaviour
{
    public float DodgeSpeed = 5f;
    public float DodgeTime = 0.1f;

    public bool ResetBehavior = false;

    public FeedBackObject DodgeFeedbacks;
    //public AudioClip DodgeSFX;
    public ParticleSystem Effect;
    public float TriggerDistance = 1.5f;
    protected RoleController roleController;
    BehaviorTree behaviorTree;
    public PhantomReleaser phantom;
    public bool DashForward = false;
    public bool CanDodgeOnAttack = false;
    public float CoolDown = 5;
    float timer = 0;

    public bool RandomDirection;
    private void Start()
    {
        roleController = GetComponent<RoleController>();
        behaviorTree = GetComponent<BehaviorTree>();
    }
    public void OnDead()
    {
        Destroy(this);
    }
    private void FixedUpdate()
    {
        if (BattleManager.Inst.CurrentPlayer == null)
            return;
        if (!roleController.IsCanMove)
            return;
        if ((!CanDodgeOnAttack) && roleController.IsAttacking)
            return;
        if (Vector3.Distance(transform.position, BattleManager.Inst.CurrentPlayer.transform.position) < TriggerDistance)
        {
            if (timer < Time.time)
            {
                timer = Time.time + CoolDown;
                Dodge();
            }
        }
    }
    public virtual void Dodge()
    {
        //可能有些躲避带动画
        Vector3 dir = (BattleManager.Inst.CurrentPlayer.transform.position - transform.position).normalized;
        if (RandomDirection)
            dir = new Vector3(Random.value - .05f, 0, Random.value - 0.5f).normalized;
        if (DashForward)
        {
            roleController.FastMove(DodgeTime, DodgeSpeed, dir, null);
        }
        else
        {
            roleController.FastMove(DodgeTime, DodgeSpeed, -dir, null);
        }

        if (Effect)
        {
            Effect.transform.forward = dir;
            Effect.transform.position = transform.position + dir.normalized + Vector3.up;
            Effect.Play();
        }
        //if(DodgeSFX!=null)
        //{
        //    AudioManager.Inst.PlaySource(DodgeSFX,1);
        //}
        if (DodgeFeedbacks != null)
        {
            FeedbackManager.Inst.UseFeedBack(roleController, DodgeFeedbacks);
        }

    }


}
