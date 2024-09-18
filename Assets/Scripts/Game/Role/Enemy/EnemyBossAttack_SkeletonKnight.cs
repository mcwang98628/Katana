using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XftWeapon;
public class EnemyBossAttack_SkeletonKnight : EnemyAttack
{
    [Header("Sword Effect")]
    public XWeaponTrail ComboTrail;
    public ParticleSystem WeaponParticle;
    public ParticleSystem ChargeParticle;
    public AudioClip ChargeSound;

    [Header("JumpAttack")]
    public AudioClip JumpAttackSound;
    public ParticleSystem JumpAttackParticle;
    public DmgBuffOnTrigger JumpAttackDamageArea;

    [Header("GroundAttack")]
    public AudioClip GroundAttackSound;
    public ParticleSystem GroundAttackParticle;
    public DmgBuffOnTrigger GroundAttackDamageArea;

    protected override void Awake()
    {
        base.Awake();
        
        StartCoroutine(DelayInitDmgArea());
    }
    protected override void Start()
    {
        base.Start();
        ComboTrail.Deactivate();
    }
    IEnumerator DelayInitDmgArea()
    {
        yield return new WaitForEndOfFrame();
        GroundAttackDamageArea.Init(roleController);
        GroundAttackDamageArea.gameObject.SetActive(false);
        JumpAttackDamageArea.Init(roleController);
        JumpAttackDamageArea.gameObject.SetActive(false);
    }
    void PlayParticle(string particleName)
    {
        if (particleName == "ChargeParticle")
            ChargeParticle.Play();
        else if(particleName == "GroundAttackParticle")
            JumpAttackParticle.Play();
    }
    
    //index==0,1,普通攻击
    //index=2，跳跃攻击
    //index=3，施法攻击
    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);
        if (eventName.Contains(AnimatorEventName.StartAttack_))
        {

            int index = int.Parse(eventName.Replace(AnimatorEventName.StartAttack_, ""));
            if (currentAttackStatus == 0 || currentAttackStatus == 1)
            {
                //ComboTrail.Emit = true;
            }

        }
        else if (eventName.Contains(AnimatorEventName.DmgStart_))
        {
            //int index = int.Parse(eventName.Replace(AnimatorEventName.DmgStart_, ""));
            //Debug.Log("CurrentAttackStatus:"+currentAttackStatus);
            if (currentAttackStatus == 1 || currentAttackStatus == 0)
            {
            }
            //跳跃斩击
            else if (currentAttackStatus == 2)
            {
                if (JumpAttackSound)
                    AudioManager.Inst.PlaySource(JumpAttackSound);
                //if (JumpAttackParticle)
                //    JumpAttackParticle.Play();

                JumpAttackDamageArea.gameObject.SetActive(true);
                JumpAttackDamageArea.ActiveTrigger();
            }
            //劈地板
            else if (currentAttackStatus == 3)
            {
                if (GroundAttackSound)
                    AudioManager.Inst.PlaySource(GroundAttackSound);
                //if (GroundAttackParticle)
                //    GroundAttackParticle.Play();

                GroundAttackDamageArea.gameObject.SetActive(true);
                GroundAttackDamageArea.ActiveTrigger();
            }

            if (WeaponParticle)
                WeaponParticle.Play();
        }
        else if (eventName.Contains(AnimatorEventName.DmgEnd_))
        {
            if (WeaponParticle)
                WeaponParticle.Stop();

            JumpAttackDamageArea.CloseTrigger();
            GroundAttackDamageArea.CloseTrigger();
            GroundAttackDamageArea.gameObject.SetActive(false);
            JumpAttackDamageArea.gameObject.SetActive(false);
        }
        else if (eventName.Contains("JumpStart"))
        {
            StartDash();
        }
        else if (eventName.Contains("JumpEnd"))
        {
        }
        else if (eventName.Contains("EndTrail"))
        {
            ComboTrail.StopSmoothly(0.3f);
        }
        else if (eventName.Contains("StartTrail"))
        {
            ComboTrail.Activate();
        }
        else if (eventName.Contains("StartCharge")) 
        {
            AudioManager.Inst.PlaySource(ChargeSound);
            ChargeParticle.Play();
        }
        else if (eventName.Contains("EndCharge")) 
        {
            ChargeParticle.Stop();
        }


    }

    void StartDash()
    {
       
        GetComponent<Collider>().isTrigger = true;

        Vector3 des = BattleManager.Inst.CurrentPlayer.transform.position;
        Vector3 dashDir = (des - transform.position).normalized;
        float speed = (Vector3.Distance(des , transform.position))/1.2f;
        speed = Mathf.Clamp(speed, 1, 5);
        roleController.FastMove(1.2f, speed, dashDir,null);

        //IndicatorManager.Inst.ShowAttackIndicator().Show(roleController, des + dashDir * 1, 360, 2f, 1.5f);
    }
}
