using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EliteMasterAttack : EnemyMagicAttack
{
    /*
    [Header("第一种攻击方式，魔法球之剑")]
    public int EmmitCount;
    public float HorizontalOrbDistance;
    */
    [Header("技能相关")]
    public float SpawnRadiusMin = 6;
    public float SpawnRadiusMax = 16;


    public int SpwanCount = 8;
    public GameObject SpawnerObj;
    public DmgBuffOnTouch SmallOrbProjectile;
    public float ShootInterval;
    public AudioClip ShootSound;
    public override void SpwanProjectile()
    {
        if (currentAttackStatus == 0)
        {
            base.SpwanProjectile();
            //StartCoroutine(MagicSwordAttack());

        }
        else if (currentAttackStatus == 1)
        {
            RoundSmallOrb();

        }
    }
    /*
    public IEnumerator MagicSwordAttack()
    {
        for (int i = 0; i < EffectOnStart.Length; i++)
        {
            if (EffectOnStart[i])
                EffectOnStart[i].SetActive(false);
        }

        isLookAt = false;
        for (int i = 0; i < EmmitCount; i++)
        {
            DmgBuffOnTouch projectile = Instantiate(Projectile, SpwanPoint.position, SpwanPoint.rotation);
            projectile.GetComponent<EliteMasterProjectile>().StartPos = SpwanPoint.position + roleController.Animator.transform.forward * i * HorizontalOrbDistance;
            projectile.GetComponent<EliteMasterProjectile>().BehaviorWay = 1;
            projectile.transform.SetParent(roleController.Animator.transform);
            projectile.Init(roleController, AttackPower);
        }
        yield return new WaitForSeconds(1f);
        float CurrentRotateAngle = 0;
        float StartTime = Time.time;
        while (Time.time < StartTime + 5)
        {
            transform.Translate(transform.forward * RotateSpeedCurve.Evaluate(Time.time - StartTime) * Time.deltaTime);
            //Vector3.Lerp(transform.position, BattleManager.Inst.CurrentPlayer.transform.position,0.1f*Time.deltaTime);
            transform.Rotate(new Vector3(0, RotateSpeedCurve.Evaluate(Time.time - StartTime), 0));
            yield return null;
        }
    }
    */
    public void RandomProjectile()
    {
        //int RandomSeed = Mathf.FloorToInt(Random.Range(0f, 2f));

        List<GameObject> SpwanerList = new List<GameObject>();

        float interval = 0;
        for (int i = 0; i < SpwanCount; i++)
        {
            GameObject go = Instantiate(SpawnerObj, SpwanPoint.position, SpwanPoint.rotation);
            go.SetActive(true);
            Vector3 pos = SpwanPoint.position + new Vector3((Random.Range(0f, 1f) > 0.5 ? 1 : -1) * Random.Range(SpawnRadiusMin, SpawnRadiusMax), 0, (Random.Range(0f, 1f) > 0.5 ? 1 : -1) * Random.Range(SpawnRadiusMin, SpawnRadiusMax));
            go.transform.DOMove(pos, 1f);
            go.transform.localScale = Vector3.one;
            interval += ShootInterval;

            go.transform.DOScale(Vector3.one * 3, interval - 0.3f);
            StartCoroutine(SpwanSmallOrb(interval, go));
        }
    }
    public void RoundSmallOrb()
    {
        List<GameObject> SpwanerList = new List<GameObject>();

        float interval = 0;
        for (int i = 0; i < SpwanCount; i++)
        {
            DmgBuffOnTouch projectile = Instantiate(SmallOrbProjectile, SpwanPoint.position, SpwanPoint.rotation);
            projectile.Init(roleController, AttackPower / 4);
            float angle = 360 / SpwanCount;
            Vector3 pos = SpwanPoint.position + Quaternion.Euler(0, angle * i, 0) * transform.forward * 8;

            interval += ShootInterval;


            projectile.transform.DOMove(pos, 2.4f).OnComplete(() =>
             {
                 projectile.transform.DOMove(new Vector3(transform.position.x,2,transform.position.z), 2.4f).SetEase((Ease.Linear)).OnComplete(() =>
             {
                 Destroy(projectile.gameObject);
             });
             });

            //StartCoroutine(SpwanSmallOrb(interval, go));
        }
    }

    IEnumerator SpwanSmallOrb(float time, GameObject spwaner)
    {
        //球移动之前的固定等待时间
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(time);
        if (ShootSound)
            AudioManager.Inst.PlaySource(ShootSound);
        DmgBuffOnTouch projectile = Instantiate(SmallOrbProjectile, spwaner.transform.position, Quaternion.identity);
        projectile.Init(roleController, AttackPower / 4);
        Vector3 fwd = BattleTool.FindNearestEnemy(roleController).transform.position - spwaner.transform.position;
        fwd.y = 0;
        projectile.transform.forward = fwd;
        Destroy(spwaner);

    }
}
