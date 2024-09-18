using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelfExplodeAttack : EnemyAttack
{
    public float DashSpeed = 15;
    public float DashTime = 0.5f;
    public AnimationCurve DashCurve;
    public GameObject SpwanOnStart;
    public GameObject Explosion;
    public Vector2 RandomOffset;
    bool startAttack = false;
    public ThrowProjectile Projectile;
    bool isDashing = false;
    public ParticleSystem FireParti;


    float endDashTime;
    AttributeBonus attriDefenseLevel;
    FeedBackObject expFeedBack;

    protected override void Awake()
    {
        base.Awake();
        EventManager.Inst.AddEvent(EventName.OnRoleDead, OnDead);

        attriDefenseLevel = new AttributeBonus();
        attriDefenseLevel.Type = AttributeType.DefenseLevel;
        attriDefenseLevel.Value = 99;

        expFeedBack = GetComponent<Enemy_Common_FeedBack>().StartFeedback;
    }

    public override void AttackFunc()
    {
        if (!IsAcceptInput)
        {
            return;
        }
        if (startAttack)
        {
            //return;
        }


        roleController.InputMove(Vector2.zero);
        roleController.SetIsAttacking(true);


        roleController.Animator.SetInteger(AttackStatus, currentAttackStatus);
        roleController.Animator.SetTrigger(Attack);


        startAttack = true;
    }

    void StartDash()
    {
        if (SpwanOnStart)
        {
            Instantiate(SpwanOnStart, roleController.Animator.transform.position, roleController.Animator.transform.rotation).transform.SetParent(roleController.Animator.transform);
        }
        roleController.gameObject.layer = LayerMask.NameToLayer("PlayerRoll");
        //GetComponent<Collider>().isTrigger = true;

        roleController.AddAttributeBonus(attriDefenseLevel);

        Vector3 des = Vector3.zero;
        Transform player = BattleManager.Inst.CurrentPlayer.transform;




        des = transform.position + roleController.Animator.transform.forward * DashSpeed * DashTime;

        des += new Vector3(RandomOffset.x * (Random.value - .5f), 0, RandomOffset.y * (Random.value - .5f));
        float dashSpeed = Vector3.Distance(transform.position, des) / DashTime;
        Vector3 dashDir = (des - transform.position).normalized;

        RaycastHit hitinfo;
        if (Physics.Raycast(new Ray(new Vector3(transform.transform.position.x,0.2f,transform.transform.position.z), dashDir), out hitinfo, dashSpeed * DashTime,1 << LayerMask.NameToLayer("Default")))
        {
            des = hitinfo.point;
            des.y = 0;
            dashSpeed = Vector3.Distance(transform.position, des) / DashTime;
        }

        endDashTime = Time.time + DashTime;
        roleController.FastMove(DashTime, dashSpeed, dashDir, EndDash, DashCurve);
        IndicatorManager.Inst.ShowAttackIndicator().Show(roleController, des, 360, 2f, DashTime, Color.red, true);

        //IndicatorManager.Inst.ShowAttackIndicator().SetEnable(true).SetAngle(360).SetSize(1.6f).SetTime(1f).SetPosition(des).SetRole(roleController);

        startAttack = false;
        isDashing = true;
    }

    void EndDash()
    {
        if (Time.time + 0.1f > endDashTime)
        {

            StartCoroutine(Explode(0.1f));
        }
        roleController.gameObject.layer = LayerMask.NameToLayer("Enemy");
        roleController.RemoveAttributeBonus(attriDefenseLevel);
        isDashing = false;
    }


    IEnumerator Explode(float Time)
    {
        yield return new WaitForSeconds(Time);
        if (Explosion != null)
        {
            //Instantiate(SpwanOnEnd, roleController.Animator.transform.position + Vector3.up*0.5f, roleController.Animator.transform.rotation);
            var dmg = Instantiate(Explosion);
            dmg.gameObject.transform.position = transform.position;
            dmg.gameObject.SetActive(true);
            //dmg.SetRole(roleController).SetDmg(attackPower);
            dmg.GetComponent<DmgBuffOnTouch>().Init(roleController);
            FeedbackManager.Inst.UseFeedBack(roleController, expFeedBack);
            roleController.SetIsAttacking(false);
        }
    }
    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);
        if (eventName.Contains("StartJump"))
        {
            StartDash();
        }
    }

    private void OnDead(string arg1, object roleDeadEventData)
    {
        RoleDeadEventData eventData = (RoleDeadEventData)roleDeadEventData;
        if (roleController.TemporaryId != (string)eventData.DeadRole.TemporaryId)
        {
            return;
        }

        if (isDashing)
            return;
        
        if(FireParti)
            FireParti.Stop();


        if (eventData.AttackerRole == BattleManager.Inst.CurrentPlayer)
        {
            ThrowBomb();
        }

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead, OnDead);
    }
    void ThrowBomb()
    {
        GameObject projectile = Instantiate(Projectile, transform.position, Quaternion.identity).gameObject;
        projectile.GetComponent<ThrowProjectile>().Init(transform.position, 1.2f, 4.0f, Explosion, 2, AttackPower, roleController);
    }
}
