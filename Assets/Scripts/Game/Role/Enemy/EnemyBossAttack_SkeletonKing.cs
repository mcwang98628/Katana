using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBossAttack_SkeletonKing : EnemyAttack
{
    public DmgBuffOnTrigger DamgeBoxAttackVertiacal;
    public DmgBuffOnTrigger DamgeBoxAttackHorizental;
    public DmgBuffOnTrigger DamgeBoxAttackJump;

    protected Transform MetrosSpawnPoints;
    public GameObject SkillSpwanObject;
    public float SkillSpwanInterval = 0.3f;
    public ParticleSystem SkillEffect;
    float timer;
    List<Vector3> SkillSpwanPoints;
    bool isUseSkill = false;
    void Start()
    {
        StartCoroutine(WaitForInit());
    }
    IEnumerator WaitForInit()
    {
        yield return new WaitForEndOfFrame();
        MetrosSpawnPoints = GameObject.Find("MeteorSpawnPoints").transform;
        DamgeBoxAttackVertiacal.Init(roleController, (int)(roleController.AttackPower* 1f));
        DamgeBoxAttackHorizental.Init(roleController,(int)(roleController.AttackPower* 1f));
        DamgeBoxAttackJump.Init(roleController, (int)(roleController.AttackPower* 1f));
    }

    float randTime=0; 
    override protected void Update()
    {
        base.Update();
        if (isUseSkill && !roleController.IsDie)
        {
            if (Time.time - timer > SkillSpwanInterval+randTime)
            {
                if (SkillSpwanPoints.Count > 0)
                {
                    int rand = Random.Range(0, SkillSpwanPoints.Count);
                    while(Vector3.SqrMagnitude(SkillSpwanPoints[rand]-transform.position)>64)
                    {
                        rand = Random.Range(0, SkillSpwanPoints.Count);
                    }
                    Instantiate(SkillSpwanObject, SkillSpwanPoints[rand] + Vector3.up * 0.1f, Quaternion.identity)
                    .GetComponent<MeteorFalling>().Init(3.3f, AttackPower, roleController);
                    randTime=(Random.value-0.5f)*0.2f;
                    timer = Time.time;
                }
            }
        }
    }

    public FeedBackObject PreFeedback;
    public override void AttackFunc()
    {
        if (!IsAcceptInput)
        {
            return;
        }
        if (PreFeedback != null)
        {
            FeedbackManager.Inst.UseFeedBack(roleController, PreFeedback);
        }

        roleController.InputMove(Vector2.zero);
        roleController.SetIsAttacking(true);

        roleController.Animator.SetInteger(AttackStatus, currentAttackStatus);
        roleController.Animator.SetTrigger(Attack);

        roleController.Animator.transform.DOLookAt(UpdateTarget().position, 0.3f, AxisConstraint.Y);

    }


    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);
        if (eventName.Contains(AnimatorEventName.DmgStart_))
        { 
            switch (eventName)
            {
                case "DmgStart_SlashVertical":
                    DamageStart(DamgeBoxAttackVertiacal);
                    break;
                case "DmgStart_SlashHorizental":
                    DamageStart(DamgeBoxAttackHorizental);
                    break;
                case "DmgStart_JumpGroud":
                    DamageStart(DamgeBoxAttackJump);
                    break;
                case "DmgStart_UseSkill":
                    BossUseSkillStart();
                    break;
            }
        }
        else if (eventName.Contains(AnimatorEventName.DmgEnd_))
        {
            switch (eventName)
            {
                case "DmgEnd_SlashVertical":
                    DamageEnd(DamgeBoxAttackVertiacal);
                    break;
                case "DmgEnd_SlashHorizental":
                    DamageEnd(DamgeBoxAttackHorizental);
                    break;
                case "DmgEnd_JumpGroud":
                    DamageEnd(DamgeBoxAttackJump);
                    break;
                case "DmgEnd_UseSkill":
                    BossUseSkillEnd();
                    break;
            }
        }
        else if (eventName.Contains("JumpStart"))
        {
            roleController.FastMove(0.5f, 3, roleController.Animator.transform.forward, null);
            GetComponent<Collider>().isTrigger = true;
        }
        else if (eventName.Contains("JumpEnd"))
        {

            GetComponent<Collider>().isTrigger = false;
        }
    }

    void DamageStart(DmgBuffOnTrigger damageBox)
    {
        damageBox.ActiveTrigger();
        if (damageBox.gameObject.name != "skeleton_kingJump")
        {
            roleController.FastMove(0.15f, 20, roleController.Animator.transform.forward, null);
        }
        ParticleSystem[] effects = damageBox.transform.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].Play();
        }
    }
    void DamageEnd(DmgBuffOnTrigger damageBox)
    {
        //damageBox.GetComponent<Collider>().enabled = false;
        damageBox.CloseTrigger();
    }
    void BossUseSkillStart()
    {
        SkillSpwanPoints = new List<Vector3>();
        for (int i = 0; i < MetrosSpawnPoints.childCount; i++)
        {
            SkillSpwanPoints.Add(MetrosSpawnPoints.GetChild(i).position);
        }
        SkillEffect.Play();
        timer = -1;
        isUseSkill = true;
    }

    void BossUseSkillEnd()
    {
        SkillEffect.Stop();
        isUseSkill = false;
    }

}

