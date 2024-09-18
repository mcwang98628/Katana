using System;
using System.Collections.Generic;
using UnityEngine;

public class Player_Fever : MonoBehaviour
{
    private RoleController roleController;
    [SerializeField] private List<ParticleSystem> FeverParticles;
    [SerializeField] private Projectile SwordTrail;
    [SerializeField] private Transform StartPoint;
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject feverWeapon;
    [SerializeField] private DmgBuffOnTouch dmgBuffOnTouch;

    [SerializeField] private ParticleSystem EnterFeverParticles;
    [SerializeField] private AudioClip EnterFeverSFX;

    [SerializeField] private List<ParticleSystem> commonFxList = new List<ParticleSystem>();
    [SerializeField] private List<AudioClip> commonAudioList = new List<AudioClip>();

    [SerializeField] private List<ParticleSystem> feverFxList = new List<ParticleSystem>();
    [SerializeField] private List<AudioClip> feverAudioList = new List<AudioClip>();

    [SerializeField] private float SwirlWindDmg = 0.2f;
    [SerializeField] private float SwordBeamDmg = 0.4f;


    public float MaxPower => maxPower;
    public float CurrentPower => _currentPower;

    public bool Fevering { get; private set; }
    public Action<bool> FeverChangeEvent;


    [SerializeField]
    private float maxPower = 100;
    [SerializeField]
    private float recoveryPower = 10;
    [SerializeField]
    private float bossPower = 10;
    [SerializeField]
    private float feverConsumePower = 10; //每秒消耗

    private float _currentPower;
    private bool _fevering;

    public void Init(RoleController roleController)
    {
        this.roleController = roleController;
        _fevering = false;
        _currentPower = 0;
    }


    public void SetFever(bool fever)
    {
        if (Fevering == fever)
            return;
        Fevering = fever;
        roleController.Animator.SetBool("Fever", fever);
        if (roleController.Animator2 != null)
        {
            roleController.Animator2.SetBool("Fever", fever);
        }

        FeverChangeEvent?.Invoke(Fevering);
    }

    private void Start()
    {
        FeverSwitch(false);
        FeverChangeEvent += FeverSwitch;
    }

    private void OnDestroy()
    {
        FeverChangeEvent -= FeverSwitch;
    }

    void FeverSwitch(bool isfever)
    {
        if (isfever)
        {
            EnterFeverParticles.Play();
            EnterFeverSFX.Play();
        }

        foreach (var particleSystem in FeverParticles)
        {
            if (isfever)
                particleSystem.Play();
            else
                particleSystem.Stop();
        }

        feverWeapon.SetActive(isfever);
        weapon.SetActive(!isfever);
    }

    private void OnEnable()
    {
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
        EventManager.Inst.AddEvent(EventName.OnPlayerAttackHitEnemy, OnHitEnemy);
        // EventManager.Inst.AddEvent(EventName.OnPlayerAttackCrit,OnAttackCrit);
    }

    private void OnDisable()
    {
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
        EventManager.Inst.RemoveEvent(EventName.OnPlayerAttackHitEnemy, OnHitEnemy);
        // EventManager.Inst.RemoveEvent(EventName.OnPlayerAttackCrit,OnAttackCrit);
    }

    protected void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
            return;
        if (eventName.Contains(AnimatorEventName.FeverWave))
        {
            if (Fevering)
            {
                Projectile swordBeam = Instantiate(SwordTrail, StartPoint.transform.position, Quaternion.identity);
                swordBeam.transform.forward = roleController.Animator.transform.forward;
                swordBeam.Init(roleController, SwordBeamDmg * roleController.AttackPower);
            }
        }
        else if (eventName.Contains("FeverFx_"))
        {
            int index = 0;
            int.TryParse(eventName.Replace("FeverFx_", ""), out index);
            if (Fevering)
            {
                feverFxList[index].Play();
            }
            else
            {
                commonFxList[index].Play();
            }
        }
        else if (eventName.Contains("FeverAudio_"))
        {
            int index = 0;
            int.TryParse(eventName.Replace("FeverAudio_", ""), out index);
            if (Fevering)
            {
                feverAudioList[index].Play();
            }
            else
            {
                commonAudioList[index].Play();
            }
        }
    }


    private void OnAttackCrit(string arg1, object arg2)
    {
        //去除Fever模式的前置条件
        //if (!Fevering)
        //    return;

        if (roleController.GetTagCount(RoleTagName.WindBlade) <= 0)
            return;


        DamageInfo dmgInfo = (DamageInfo) arg2;
        Vector3 fxPos = dmgInfo.AttackPoint;
        RoleController target = null;
        if (BattleManager.Inst.EnemyTeam.ContainsKey(dmgInfo.HitRoleId))
        {
            target = BattleManager.Inst.EnemyTeam[dmgInfo.HitRoleId];
            fxPos = target.transform.position;
        }

        DmgBuffOnTouch dmgOnTouch = Instantiate(dmgBuffOnTouch, fxPos, Quaternion.identity);
        dmgOnTouch.Init(roleController, roleController.AttackPower * SwirlWindDmg);
        dmgOnTouch.transform.SetParent(target.transform);
    }

    //
    private void OnHitEnemy(string arg1, object arg2)
    {
        DamageInfo dmgInfo = (DamageInfo) arg2;
        if (BattleManager.Inst.EnemyTeam.ContainsKey(dmgInfo.HitRoleId) &&
            (BattleManager.Inst.EnemyTeam[dmgInfo.HitRoleId].roleTeamType == RoleTeamType.EliteEnemy ||
             BattleManager.Inst.EnemyTeam[dmgInfo.HitRoleId].roleTeamType == RoleTeamType.Enemy_Boss))
        {
            AddPower(recoveryPower + bossPower);
        }
        else
        {
            AddPower();
        }
        
        // if (dmgInfo.AttackType == DamageAttackType.RollAttack)
        // {
        //     OnAttackCrit("", dmgInfo);
        // }
    }

    void AddPower()
    {
        AddPower(recoveryPower);
    }

    public void AddPower(float value)
    {
        if (Fevering)
            return;

        _currentPower += value;
        if (_currentPower >= maxPower)
        {
            _fevering = true;
            _currentPower = maxPower;
            SetFever(true);
        }
    }

    void Update()
    {
        if (_fevering)
        {
            bool enemyAllDead = true;
            foreach (var enemy in BattleManager.Inst.EnemyTeam)
            {
                if (!enemy.Value.IsDie)
                {
                    enemyAllDead = false;
                    break;
                }
            }

            if (enemyAllDead)
            {
                return;
            }

            _currentPower -= feverConsumePower * Time.deltaTime;
            if (_currentPower <= 0)
            {
                _fevering = false;
                _currentPower = 0;
                SetFever(false);
            }
        }
    }
}