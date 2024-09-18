using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using UnityEngine;
using DG.Tweening;

public class EnemyVampireAttack : EnemyMagicAttack
{
    //吸血 特效线
    public LineRenderer Line;
    [SerializeField]
    private float hpTreatmentValue = 1;
    private Vector3 lineOffset = new Vector3(0, 1, 0);
    private bool isVampireing;
    private int attackCount = 0;
    private RoleController target;

    public ParticleSystem HealParticle;
    bool canRetuenHp;
    private bool behaviorEnabled
    {
        get
        {
            if (_tree == null)
            {
                return false;
            }

            return _tree.enabled;
        }
    }
    private BehaviorTree _tree;
    protected override void Awake()
    {
        base.Awake();
        canRetuenHp = true;
        _tree = GetComponent<BehaviorTree>();
    }

    /// <summary>
    /// 吸血攻击
    /// </summary>
    /// <param name="target">目标(可以无视队伍)</param>
    /// <param name="attackValue">一共 吸血的数值</param>
    /// <param name="time">一共吸血多久</param>
    /// <param name="distance">吸血最远多少距离,超过就打断</param>
    /// <param name="callBack">吸血完成/中断 时调用</param>
    public void VampireAttack(RoleController target, float intervaltime, float distance, Action callBack = null)
    {
        if (isVampireing)
        {
            return;
        }
        base.AttackFunc();
        callBack += () =>
        {
            roleController.SetIsAttacking(false);
        };
        this.target = target;
        StartCoroutine(vampireAttack(target, intervaltime, distance, callBack));
    }

    private float vampireTimer;
    IEnumerator vampireAttack(RoleController target, float intervaltime, float distance, Action callBack)
    {
        isVampireing = true;
        Line.gameObject.SetActive(true);
        vampireTimer = 0;
        var dir = target.transform.position - roleController.transform.position;
        float dis = dir.magnitude;
        while (dis <= distance && !target.IsDie && !roleController.IsDie && behaviorEnabled)
        {
            dir = target.transform.position - roleController.transform.position;
            dis = dir.magnitude;
            //Line
            if (Line != null)
            {
                Line.positionCount = 2;
                //Line.SetPosition(0,roleController.transform.position+lineOffset);
                Line.SetPosition(0, roleController.roleNode.Head.transform.position);
                Line.SetPosition(1, target.transform.position + lineOffset);
            }

            roleController.Animator.transform.forward = dir;
            //
            vampireTimer += Time.deltaTime;
            if (vampireTimer >= intervaltime)
            {
                vampireTimer -= vampireTimer;
                var value = AttackPower;
                DamageInfo dmginfo = new DamageInfo(target.TemporaryId, value, roleController, roleController.transform.position, DmgType.Vampire);
                target.HpInjured(dmginfo);
                TreatmentData treatmentData = new TreatmentData(hpTreatmentValue, roleController.TemporaryId);
                roleController.HpTreatment(treatmentData);
                attackCount++;
            }
            yield return null;
        }

        if (Line != null)
        {
            Line.positionCount = 0;
            Line.gameObject.SetActive(false);
        }
        if (callBack != null)
        {
            callBack();
        }

        isVampireing = false;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        if(HealParticle!=null)
        {
            Destroy(HealParticle.gameObject);
        }
    }

    protected override void Update()
    {
        base.Update();


        if (roleController.IsDie && canRetuenHp && HealParticle != null)
        {
            if (target == null)
            {
                target = BattleManager.Inst.CurrentPlayer;
            }

            HealParticle.transform.SetParent(transform.parent);

            Vector3 des = target.transform.position + Vector3.up;
            Vector3 dir = des - HealParticle.transform.position;
            dir.y = 0;
            HealParticle.transform.position += dir.normalized * Time.deltaTime * 3;


            Vector3 dis= HealParticle.transform.position - des;
            dis.y=0;

            if (dis.magnitude< 0.2f)
            {
                TreatmentData treatmentData = new TreatmentData(attackCount * attackPower * 0.8f+1, target.TemporaryId);
                target.HpTreatment(treatmentData);
                canRetuenHp = false;
                if (HealParticle != null)
                {
                    HealParticle.Stop();
                    Destroy(HealParticle.gameObject, 0.3f);
                }
            }
        }
    }
}
