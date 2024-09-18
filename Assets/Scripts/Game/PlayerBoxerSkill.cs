using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoxerSkill : PlayerSkill
{
    public float AddBombDistance = 2 ;

    public GameObject BombUltimateVFX;
    public AudioClip UltimateSFX;
    public GameObject NukeUltimateVFX;
    public Transform ExpPosition;
    //拳风
    public ParticleSystem PunchWave;
    public int AttackCounter = 0;
    public GameObject TimeBomb;
    public List<RoleController> EnemiesWithBomb;
    public Transform PunchWaveStartPosition;



    private void Awake()
    {
        EnemiesWithBomb = new List<RoleController>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.Inst.AddEvent(EventName.OnPlayerAttackCrit,EmmitPunchWave);
    }
    protected virtual void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Inst.RemoveEvent(EventName.OnPlayerAttackCrit, EmmitPunchWave);
    }
    private void Update()
    {
        if(roleColorLevel>=2)
        {
            //安装炸弹 暂时关闭处理
            //AddBomb();
        }
    }
    public void AddBomb()
    {
        if (roleController != null)
            if (roleController.IsRolling)
            {
                List<RoleController> NearEnemies = BattleTool.GetEnemysInDistance(roleController, AddBombDistance);
                for (int i = 0; i < NearEnemies.Count; i++)
                {
                    bool IsAdded = false;
                    for (int j = 0; j < EnemiesWithBomb.Count; j++)
                    {
                        if (NearEnemies[i] == EnemiesWithBomb[j])
                        {
                            IsAdded = true;
                        }
                    }
                    if (!IsAdded)
                    {
                        //NearEnemies[i].
                        GameObject timeBomb = Instantiate(TimeBomb, NearEnemies[i].transform.position, Quaternion.identity);
                        timeBomb.GetComponent<TimeBomb>().Target = NearEnemies[i];
                        timeBomb.GetComponent<ObjFollower>().SetFollowingObj(NearEnemies[i].gameObject);
                        EnemiesWithBomb.Add(NearEnemies[i]);
                    }
                }
            }
            else
            {

            }
    }

    public void EmmitPunchWave(string arg1,object arg2)
    {
        PunchWave.Play();
    }
    protected override void AnimEvent(GameObject go, string eventName)
    {
        base.AnimEvent(go, eventName);
        if(go!=roleController.Animator.gameObject)
        {
            return;
        }
        //终极大招爆炸
        if(eventName == "UltimateExp")
        {
            {
                if (roleColorLevel == 1)
                {
                    GameObject expVFX = Instantiate(NukeUltimateVFX, ExpPosition.transform.position, Quaternion.identity);
                }
                else
                {
                    //GameObject expVFX = Instantiate(BombUltimateVFX, ExpPosition.transform.position, Quaternion.identity);
                    BombUltimateVFX.GetComponent<ParticleSystem>().Play();
                    UltimateSFX.Play();
                }
            }
        }
        if(roleColorLevel >=2)
        if(eventName == "NormalAttack")
        {
            AttackCounter += 1;
            if (AttackCounter == 3)
            {
                GameObject pWave = Instantiate(PunchWave.gameObject, PunchWaveStartPosition.position, PunchWaveStartPosition.rotation);
                pWave.GetComponentInChildren<DmgBuffOnTouch>().Init(roleController,-1f);
                PunchWave.Play();
                AttackCounter = 0;
            }
        }


    }
}
