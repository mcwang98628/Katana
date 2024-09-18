using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class PlayerPaladinSkill : PlayerSkill
{
    RoleHealth _health;
    //public DmgBuffOnTouch Dmg;
    //public PaladinLifeHeal heal;
    
    public PaladinSuckBlood SuckBlood;

    [LabelText("法球出生点")]
    public Transform SpawnPoint;
    [LabelText("法球本体")]
    public GameObject Projectiles;
    //圣光次数
    public int HolyLightTimesLow;
    //圣光升级后次数
    public int HolyLightTimesHigh;

    //范围
    public float HolyLightRange = 8;
    //频率
    public float HolyLightInterval = 0.1f;
    //蓄力无敌的时间
    public float InvincibleTime = 7f;
    //圣光的Prefab
    public GameObject HolyLightSmall;
    public GameObject HolyLightLarge;
    public float HolyLightSmallDmgMul;
    public float HolyLightLargeDmgMul;

    public ParticleSystem ShieldParticles;
    public BuffScriptableObject buff;
    public BuffLifeCycle life;

    public Transform HitGroundIniPos;


    //private void OnGUI()
    //{
    //    //使当前角色成为1级
    //    if(GUI.Button(new Rect(300,0,100,50),"Level1"))
    //    {
    //        CurrentLevel = 1;
    //    }
    //    if (GUI.Button(new Rect(300, 50, 100, 50), "Level2"))
    //    {
    //        CurrentLevel = 2;
    //    }
    //    if (GUI.Button(new Rect(300, 100, 100, 50), "Level3"))
    //    {
    //        CurrentLevel = 3;
    //    }
    //}
    //public ParticleSystem HealParticles;
    protected override void OnEnable()
    {
        base.OnEnable();
        //EventManager.Inst.AddAnimatorEvent(AnimEvent);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        //EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
    }

    public override void Init()
    {
        base.Init();
        //_controller = GetComponent<RoleController>();
        _health = GetComponent<RoleHealth>();
        //heal._controller = roleController;
        //heal._health = _health;

        //Dmg.Init(roleController);
    }
    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        //if (eventName.Contains("HolyLight"))
        //{
        //    StartSpawnHolyLight();
        //}
        if(eventName.Contains("SuckBlood"))
        {
            SuckBlood.SuckBlood(roleColorLevel);
        }
        if(eventName.Contains("SpawnPros"))
        {
            SpawnPros();
        }
        if (eventName.Contains("HolyLight"))
        {
            StartSpawnHolyLight();
        }
        if (eventName.Contains("HitGround"))
        {
            StartSpawnHitGroundParticles();
        }
    }
    //public void StartSpawnHolyLight()
    //{
    //    GameObject Heal = Instantiate(heal.gameObject,_controller.transform.position,Quaternion.identity);
    //    Heal.SetActive(true);

    //}

    //平A时释放法球
    public void SpawnPros()
    {
        GameObject CurrentPro = Instantiate(Projectiles, SpawnPoint.position, Quaternion.LookRotation( roleController.Animator.transform.forward, roleController.Animator.transform.up ));
        CurrentPro.GetComponent<DmgBuffOnTouch>().Init(roleController,1);
        CurrentPro.GetComponent<PaladinAttackOrb>().CurrentLevel = roleColorLevel;
    }
    public void StartSpawnHitGroundParticles()
    {
        if (roleColorLevel > 1)
        {
            GameObject CurrentPro = Instantiate(HolyLightLarge, HitGroundIniPos.transform.position, Quaternion.LookRotation(roleController.Animator.transform.forward, roleController.Animator.transform.up));
        }
        //CurrentPro.GetComponent<DmgBuffOnTouch>().Init(roleController, 1);
        //CurrentPro.GetComponent<PaladinAttackOrb>().CurrentLevel = roleColorLevel;
    }
    public void StartSpawnHolyLight()
    {
        StartCoroutine(StartSpawnHolyLightIE());
    }
    //public IEnumerator StartSpawnRoundHolyLightIE()
    //{
    //    Vector3 fw = roleController .Animator.transform.forward;
    //    for (int i = 0; i < 3; i++)
    //    {
    //        GameObject holyLight = Instantiate(HolyLight, transform.position + 2 * fw * (i + 1), Quaternion.identity);
    //        holyLight.GetComponent<DmgBuffOnTouch>().Init(roleController, 60);
    //        yield return new WaitForSeconds(HolyLightInterval);
    //    }
    //    for (int i = 3; i < 6; i++)
    //    {
    //        GameObject holyLight = Instantiate(HolyLight, transform.position + 2 * fw * (6 - i), Quaternion.identity);
    //        holyLight.GetComponent<DmgBuffOnTouch>().Init(roleController, 60);
    //        yield return new WaitForSeconds(HolyLightInterval);
    //    }
    //}
    public IEnumerator StartSpawnHolyLightIE()
    {
        for (int i = 0; i < ((roleColorLevel>1)?HolyLightTimesHigh:HolyLightTimesLow); i++)
        {
            GameObject holyLight = Instantiate(roleColorLevel <=2 ?  HolyLightSmall:HolyLightLarge, 
                transform.position + (roleColorLevel<=2?0.5f:1f) * new Vector3(Random.Range(-HolyLightRange, HolyLightRange), 0, Random.Range(-HolyLightRange, HolyLightRange)),
                Quaternion.identity);
            holyLight.GetComponent<DmgBuffOnTouch>().Init(roleController, roleController.roleAttack.AttackPower * (roleColorLevel<=2? HolyLightSmallDmgMul:HolyLightLargeDmgMul));
            yield return new WaitForSeconds(HolyLightInterval);
        }
    }
}
