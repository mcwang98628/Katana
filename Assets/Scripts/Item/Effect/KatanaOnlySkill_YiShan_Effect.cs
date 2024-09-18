using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//武士刀角色独有技能，麦克雷瞄准 冲刺斩杀。 
public class KatanaOnlySkill_YiShan_Effect : ItemEffect
{
    Vector3 StartPos;
    public FeedBackObject StartSaFeedback;
    public FeedBackObject SaHitFeedback;
    // List<RoleController> _frozenEnemies=new List<RoleController>();
    public Material TrailMat;
    public TrailRenderer trail;
    public List<LineRenderer> lines = new List<LineRenderer>();


    public ParticleSystem EndParticles;
    public ParticleSystem EndBloodBurst;
    public AudioClip EndSFX;
    public AudioClip BloodSFX;
    
    private float CurrentTrailAlpha;
    public KatanaOnlySkill_YiShan_Effect(FeedBackObject feedback,FeedBackObject saHitFeedback,Material trailMat,ParticleSystem endParticles,ParticleSystem endBloodBurst,AudioClip endSfx,AudioClip bloodSfx)
    {
        this.TrailMat = trailMat;
        
        this.StartSaFeedback = feedback;
        this.SaHitFeedback = saHitFeedback;
        this.EndParticles = endParticles;
        this.EndBloodBurst = endBloodBurst;
        this.EndSFX = endSfx;
        this.BloodSFX = bloodSfx;
        
    }
    

 

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.StartCoroutine(TriggerSkill());
    }

    IEnumerator TriggerSkill()
    {
        OnHoldStart();
        FindEnemy();
        if(enemies.Count>0)
            if (StartSaFeedback != null)
                FeedbackManager.Inst.UseFeedBack(roleController, StartSaFeedback);
        yield return new WaitForSecondsRealtime(0.8f);
        OnRealease();
    }
    
    //按下按钮，准备蓄力
    void OnHoldStart()
    {
        roleController.Animator.SetBool(SkillAccumulate,true);
        TimeManager.Inst.SetTimeScale(0.1f);
        roleController.Animator.speed = 1f/Time.timeScale;
    }
    //按下按钮期间
    void FindEnemy()
    {
        roleController.Animator.SetBool(SkillAccumulate,true);

        float dis = 99;

        enemies.Clear();
        foreach (var enemy in BattleManager.Inst.EnemyTeam)
        {
            if (enemies.Contains(enemy.Value))
            {
                continue;
            }

            if (enemy.Value.IsDie)
            {
                continue;
            }

            if ((enemy.Value.transform.position - roleController.transform.position).magnitude <= dis)
            {
                enemies.Add(enemy.Value);
            }
        }
    }
    //松手时判断，如果有敌人就放技能。
    void OnRealease()
    {
        roleController.Animator.SetBool(SkillAccumulate,false);
        if (enemies.Count > 0)
        {
            roleController.StartCoroutine(skill());
        }
        else
        {
            TimeManager.Inst.SetTimeScale(1f);
            roleController.Animator.speed = 1f/Time.timeScale;
            _indeicate.SetEnable(false);
            // roleController.Animator.Play("Idle");
        }
    }

    private AttackIndeicate_Sector _indeicate;

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
     
        _indeicate = IndicatorManager.Inst.ShowAttackIndicator();
        _indeicate.SetEnable(false);
        EventManager.Inst.AddEvent("DoBlinkDamage",DoBlinkDamage);
    }

    void DoBlinkDamage(string str,object dmgValue)
    {
        for (int j = 0; j < enemies.Count; j++)
        {
            if (enemies[j] == null || enemies[j].transform == null)
            {
                continue;
            }
            GameObject BloodObj = GameObject.Instantiate(EndBloodBurst.gameObject, enemies[j].transform.position + 1 * Vector3.up, Quaternion.identity);
            BloodObj.transform.localScale = Vector3.one * 1.3f;
            float value = (float) dmgValue;
            DamageInfo dmg = new DamageInfo(enemies[j].TemporaryId,(float)dmgValue, roleController, roleController.transform.position, DmgType.Other);
            enemies[j].HpInjured(dmg);
            
            EventManager.Inst.DistributeEvent(EventName.OnRoleAttack,roleController.TemporaryId);
            EventManager.Inst.DistributeEvent(EventName.OnPlayerAttackHitEnemy,dmg);
            
            BloodSFX.Play();
            if (!enemies[j].IsDie)
            {
                enemies[j].Animator.Play("Hit");
            }
        }
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        if (_indeicate!=null)
        {
            _indeicate.Recover();
        }
        EventManager.Inst.RemoveEvent("DoBlinkDamage",DoBlinkDamage);
    }


    List<RoleController> enemies = new List<RoleController>();
    private static readonly int SkillAccumulate = Animator.StringToHash("SkillAccumulate");
    private static readonly int YiShan = Animator.StringToHash("YiShan");

    public override void Update(RoleItemController rpe)
    {
        CurrentTrailAlpha -= Time.deltaTime*0.5f ;
        //if (trail != null)
        //{
        //    trail.startColor = new Color(255, 255, 255, CurrentTrailAlpha);
        //    trail.endColor = new Color(255, 255, 255, CurrentTrailAlpha);
        //}

        for(int i=0;i<lines.Count;i++)
        {
            if (lines[i] != null)
            {
                lines[i].startColor = new Color(255, 255, 255, CurrentTrailAlpha);
                lines[i].endColor = new Color(255, 255, 255, CurrentTrailAlpha);
            }
        }
        base.Update(rpe);
    }

    public void FreezeEnemies()
    {
        for(int i=0;i<enemies.Count;i++)
        {
            if (enemies[i]==null || enemies[i].IsDie)
            {
                continue;
            }
            enemies[i].SetFreeze(true);
            enemies[i].StopFastMove();
            enemies[i].Rigidbody.velocity = Vector3.zero;
        }
    }
    public void ClearFreezeEnemies()
    {
        for(int i=0;i<enemies.Count;i++)
        {
            if (enemies[i]==null || enemies[i].IsDie)
            {
                continue;
            }
            enemies[i].SetFreeze(false);
            enemies[i].StopFastMove();
        }
    }
    //松开键后开始乱穿
 
    public void ExitPrepare()
    {
        _indeicate.SetEnable(false);
        TimeManager.Inst.SetTimeScale(1f);
    }
    IEnumerator skill()
    {
        roleController.SetAcceptInput(false);
        EventManager.Inst.DistributeAnimatorEvent(roleController.Animator.gameObject,AnimatorEventName.CantMoveSkillStart);
        if(enemies.Count==0)
        {
            yield break;
        }
        FreezeEnemies();
        

        // for (int i = 0; i < lines.Count; i++)
        // {
        //     GameObject.Destroy(lines[i].gameObject);
        // }
        // lines.Clear();
        lines = new List<LineRenderer>();

        if (trail!=null)
        GameObject.Destroy(trail.gameObject);


        //CreateTrail();
        if (BattleManager.Inst.CurrentRoom is FightRoom fightRoom)
        {
            float minDis = 9999999;
            int childIndex = 0;
            for (int i = 0; i < fightRoom.Enemypoints.childCount; i++)
            {
                var dir = fightRoom.Enemypoints.GetChild(i).position - BattleManager.Inst.CurrentPlayer.transform.position;
                if (Vector3.SqrMagnitude(dir) < minDis)
                {
                    minDis = dir.magnitude;
                    childIndex = i;
                }
            }
            StartPos = fightRoom.Enemypoints.GetChild(childIndex).position;
        }
        else
        {
            StartPos = roleController.transform.position;
        }
        
        
        ExitPrepare();
 
        //trail.emitting = true;
        roleController.SetGod(true);
        roleController.GetComponent<Collider>().enabled = false;
        
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null || enemies[i].IsDie)
            {
                continue;
            }
            CurrentTrailAlpha = 1;
            // roleController.Animator.CrossFade("Sa",0.02f);
            roleController.Animator.SetTrigger(YiShan);
            var v3 = enemies[i].transform.position - roleController.transform.position;
            //转向
            roleController.Animator.transform.forward = v3.normalized;
            float DashTime = 0.1f;
            float DashSpeed;
            //至少得冲出一段距离
            if(v3.magnitude<3f)
            {
                DashSpeed = 3f / 0.1f;
            }
            else
            DashSpeed = (v3.magnitude) / 0.1f;
            //模型瞬移到身后
            //roleController.Animator.transform.position = enemies[i].transform.position - 1.2f * enemies[i].Animator.transform.forward;
            //DamageInfo dmg = new DamageInfo(roleController.AttackPower * 2, roleController, roleController.transform.position, DmgType.Physical);
            //enemies[i].HpInjured(dmg);
            FeedbackManager.Inst.UseFeedBack(enemies[i],SaHitFeedback); 
            var enemy = enemies[i];



            GameObject LineVFXObj = new GameObject();
            LineRenderer renderer = LineVFXObj.AddComponent<LineRenderer>();
            renderer.positionCount = 2;
            renderer.SetPosition(0, enemies[i].transform.position+ v3 * 500 + Vector3.up);
            renderer.SetPosition(1, roleController.transform.position - v3 * 500 + Vector3.up);
            renderer.alignment = LineAlignment.TransformZ;
            renderer.transform.rotation = Quaternion.Euler(90, 0, 0);
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.material = TrailMat;
            renderer.widthMultiplier = 0.1f;
            lines.Add(renderer);
           
            

            roleController.FastMove( DashTime,DashSpeed,v3.normalized, () =>
            {
                //DamageInfo dmg = new DamageInfo(roleController.AttackPower * 2, roleController, roleController.transform.position, DmgType.Other);
                //enemy.HpInjured(dmg);
                // _frozenEnemies.Add(enemy);
            
            },null);
            yield return new WaitForSeconds(0.1f);
            roleController.transform.forward = v3;
        }
        //roleController.Animator.transform.localPosition = Vector3.zero;
        roleController.Animator.SetTrigger(YiShan);
        roleController.Animator.transform.forward = (StartPos - roleController.transform.position).normalized;

        roleController.transform.position = StartPos;
       
        //结尾。
        yield return new WaitForSeconds(0.75f);
        for(int i=0;i<1;i++)
        {
            yield return new WaitForSeconds(0.1f);
            for(int j=0;j<enemies.Count;j++)
            {
                if (enemies[j] == null || enemies[j].transform == null)
                {
                    continue;
                }
                GameObject EndParticleObj = GameObject.Instantiate(EndParticles.gameObject, enemies[j].transform.position + 1 * Vector3.up, Quaternion.identity);
                EndParticleObj.transform.localScale = Vector3.one * 1.3f;
                //DamageInfo dmg = new DamageInfo(roleController.AttackPower * 0.3f, roleController, roleController.transform.position, DmgType.Other);
                //enemies[j].HpInjured(dmg);
            }
                EndSFX.Play();
        }
        EventManager.Inst.DistributeEvent(EventName.OnEnvironmentBack);
        yield return new WaitForSeconds(0.5f);
        
        ResetPlayer();
        DoBlinkDamage("", roleController.AttackPower * 2.5f);
        FinishSkill();
        if (roleController.GetComponent<PlayerSkill>().RoleSkillLevel >= 3)
        {
            yield return new WaitForSeconds(0.4f);
            DoBlinkDamage("", roleController.AttackPower * 0.5f);
            yield return new WaitForSeconds(1f);
            DoBlinkDamage("", roleController.AttackPower * 1);
        }


        enemies.Clear();
        EventManager.Inst.DistributeEvent(EventName.OnSkillOver);
    }
    public void FinishSkill()
    {
        ClearFreezeEnemies();
        TimeManager.Inst.SetTimeScale(1f);
        
        //trail.emitting = false;
        FeedbackManager.Inst.UseFeedBack(roleController, SaHitFeedback);
        
        for(int i=0;i<lines.Count;i++)
        {
            if (lines[i] != null)
            {
                GameObject.Destroy(lines[i].gameObject);
            }
        }
        
        lines.Clear();
    }

    void ResetPlayer()
    {
        roleController.GetComponent<Collider>().enabled = true;
        roleController.SetGod(false);
        roleController.SetAcceptInput(true);
        EventManager.Inst.DistributeAnimatorEvent(roleController.Animator.gameObject,AnimatorEventName.CantMoveSkillEnd);

    }


}
