using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
public class PlayerShieldHealth : RoleHealth
{
    [LabelText("盾防御角度")][SerializeField]
    private float defenseAng=120;
    //反弹子弹
    private bool againstBullet;
    private bool IsHoldingShield => ((PlayerAttack)roleController.roleAttack).IsAccumulateing; 
    
    public ParticleSystem ShieldOnHitParticles;
    public float ShieldOnHitBackTime;
    public float ShieldOnHitBackSpeed;
    //完美弹反
    
    //盾牌防御Enemy攻击 相关次数
    public float E_ShieldOnHitBackTime;
    public float E_ShieldOnHitBackSpeed;
    public ParticleSystem E_ShieldOnHitParticles;
    public BuffScriptableObject E_ShieldOnHitBuff;
    public float E_ShieldOnHitRemainTime;
    bool IsHoldingShieldLastFrame;
    public GameObject ShieldModel;
    Material mat;
    public FeedBackObject OnHitShieldFeedback;

    protected override void Awake()
    {
        base.Awake();
        //mat = Instantiate(ShieldModel.GetComponent<Renderer>().material);
        mat = ShieldModel.GetComponent<Renderer>().material;
        roleController.AddSkillPower(9999);
    }


    //
    // private void Update()
    // {
    //     // // 朝向最近的敌人
    //     // if (IsHoldingShield && BattleTool.FindNearestEnemy(roleController) != null && !roleController.IsMoving)
    //     // {
    //     //     roleController.Animator.transform.forward = BattleTool.FindNearestEnemy(roleController.transform).transform.position - roleController.Animator.transform.position;
    //     // }
    //     if(IsHoldingShield)
    //     {
    //        
    //         mat.SetColor("_CoverColor",Color.white);
    //         mat.SetFloat("_CoverColorAlpha",0.15f);
    //     }
    //     else
    //     {
    //         mat.SetColor("_CoverColor",Color.black);
    //         mat.SetFloat ("_CoverColorAlpha", 0.15f);
    //     }
    //
    //
    // }

    public void SetDefenseAng(float ang)
    {
        this.defenseAng = ang;
    }
    public void SetIsAgainstBullet(bool isagainstBullet)
    {
        againstBullet = isagainstBullet;
    }
    public bool JudgeProjectile(Vector3 attacPoint)
    {
        if (!againstBullet)
        {
            return false;
        }
        
        ////成功反弹
        //if ((IsHoldingShield) &&
        //          Vector3.Angle(roleController.Animator.transform.forward, (attacPoint - transform.position)) < defenseAng/2f)
        //{
        //    return true;
        //}
        return false;
    }
    public override void Injured(DamageInfo dmg)
    {
         // //完美弹反支持所有范围。
         // //刚按下过一会也是能弹反的。
         // // if (IsHoldingShield && Time.time - HoldingShieldStart <= 10.2f)//盾反 判定  暂定0.2f完美弹反时间
         // if (Time.time - HoldingShieldStart <= 0.5f)//盾反 判定  暂定0.2f完美弹反时间
         // {
         //     roleController.Animator.SetTrigger(ShieldAgainst);
         //     ShieldReflectParticle.Play();
         //     roleController.FastMove(0.05f, 10f, roleController.Animator.transform.forward, null);
         //     FeedbackManager.Inst.UseFeedBack(roleController, PerfectShieldFeedback);
         //
         //     //弹伤敌人
         //     if (dmg.AttackerRole != null && dmg.AttackerRole != roleController)
         //     {
         //         DamageInfo damage = new DamageInfo(roleController.AttackPower * 4, roleController, transform.position);
         //         dmg.AttackerRole.HpInjured(damage);
         //     }
         //
         //     //震退周围的敌人
         //     List<RoleController> NearEnemies = BattleManager.Inst.GetEnemiesInDistance(roleController, 2.5f);
         //     for (int i = 0; i < NearEnemies.Count; i++)
         //     {
         //         NearEnemies[i].DelayFastMove(Time.deltaTime, 0.15f, 8f, (NearEnemies[i].Animator.transform.position - roleController.Animator.transform.position).normalized, null);
         //     }
         //     
         //     //完美弹反之后重制时间
         //     HoldingShieldStart = Time.time;
         // }
         // //受击判定
         // else 

        var ang = Vector3.Angle(roleController.Animator.transform.forward, (dmg.AttackPoint - transform.position));
        if ((dmg.DmgType == DmgType.Physical  || dmg.DmgType == DmgType.ArrowHit) 
            && 
            IsHoldingShield && ang < defenseAng/2f && roleController.CurrentSkillPower>0)
        {
            //受击后有个后退，暂时关掉。
            //roleController.FastMove(ShieldOnHitBackTime, ShieldOnHitBackSpeed, -roleController.Animator.transform.forward, null);
            if (dmg.AttackerRole != null && dmg.AttackerRole != roleController)
            {
                //防御后获取的收益
                //if (dmg.AttackerRole.IsAcceptInterruption)
                //{
                if (dmg.DmgType == DmgType.Physical)
                {
                    //StartCoroutine(EnemyHitShield(dmg.AttackerRole));
                    if(Vector3.Distance(transform.position, dmg.AttackerRole.transform.position)<2)
                        (dmg.AttackerRole.roleHealth as EnemyHealth).InterruptRepel(E_ShieldOnHitBackTime, E_ShieldOnHitBackSpeed, roleController.Animator.transform.forward);
                }
                //}
            }

            FeedbackManager.Inst.UseFeedBack(roleController, OnHitShieldFeedback);
            ShieldOnHitParticles.Play();
            
            EventManager.Inst.DistributeEvent(EventName.OnShieldDefense,dmg);
        }
        else
        {
            base.Injured(dmg);
        }
    }
    public IEnumerator EnemyHitShield(RoleController enemy)
    {
        yield return new WaitForEndOfFrame();
        GameObject Pars = Instantiate(E_ShieldOnHitParticles.gameObject);
        Pars.transform.position = enemy.roleNode.Head.transform.position;
        Pars.transform.SetParent(enemy.Animator.transform);

        /*
        enemy.FastMove(E_ShieldOnHitBackTime, E_ShieldOnHitBackSpeed, roleController.Animator.transform.forward, null);
        //enemy.StopFastMove();
        enemy.DelayFastMove(Time.deltaTime, E_ShieldOnHitBackTime, E_ShieldOnHitBackSpeed, roleController.Animator.transform.forward, null);
        //enemy.SetDizziness(true);
        enemy.Animator.Play("Hit");
        BehaviorTree bTree;
        bTree = enemy.GetComponent<BehaviorTree>();
        if (bTree != null)
        {
            bTree.enabled = false;
        }
        yield return new WaitForSeconds(E_ShieldOnHitRemainTime);
        if(bTree!=null)
        {
            bTree.enabled = true;
        }
        */
        Pars.GetComponent<ParticleSystem>().Clear();
        Destroy(Pars);
        //enemy.SetDizziness(false);
    }

}
