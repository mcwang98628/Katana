using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
public class EnemyEliteCoffinAttack : EnemyAttack
{
    [SerializeField]
    private Transform createPoint;
    [SerializeField]
    private GameObject ghostSkull;

    [SerializeField]
    private List<int> EnemyIds = new List<int>();
    private List<EnemyController> SpwanCreatures;
    private Transform enemySpwanPoints;
    private Vector3 targetPoint;

    public int CurrentSpawnIndex;
    public int SpawnCount=6;
    /*
    public override void AttackFunc()
    {
        roleController.Animator.SetTrigger(Attack);
        Debug.LogError("Attack");
    }
    */

    void RefreshCreatureList()
    {
        List<EnemyController> removeList = new List<EnemyController>();
        foreach (EnemyController creature in SpwanCreatures)
        {
            if (creature != null)
            {
                if (creature.IsDie)
                {
                    removeList.Add(creature);
                }
            }
            else
            {
                removeList.Add(creature);
            }
        }

        foreach (EnemyController removeCreature in removeList)
        {
            SpwanCreatures.Remove(removeCreature);
        }
    }
    public int GetSpwanCreaturesCount()
    {
        if (SpwanCreatures != null)
        {
            RefreshCreatureList();
            return SpwanCreatures.Count;
        }
        else
        {
            return 0;
        }
    }
    protected override void Start()
    {
        base.Start();

        SpwanCreatures = new List<EnemyController>();
        enemySpwanPoints = GameObject.Find("EnemySpawnPoints").transform;

    }

    //Vector3 GetNearBySpwanPoint()
    //{
    //    List<Vector3> poinsList = new List<Vector3>();
    //    for (int i = 0; i < enemySpwanPoints.childCount; i++)
    //    {
    //        float dis = Vector3.Distance(enemySpwanPoints.GetChild(i).position, transform.position);
    //        if (dis > 1f && dis < 4)
    //            poinsList.Add(enemySpwanPoints.GetChild(i).position);
    //    }

    //    if (poinsList.Count > 0)
    //    {
    //        float minDis = 999;
    //        Vector3 result = poinsList[0];
    //        for (int i = 0; i < poinsList.Count; i++)
    //        {
    //            float dis = Vector3.Distance(poinsList[i], BattleManager.Inst.CurrentPlayer.transform.position);
    //            if (dis < minDis)
    //            {
    //                minDis = dis;
    //                result = poinsList[i];
    //            }
    //        }

    //        return result;
    //    }
    //    else
    //        return transform.position - Vector3.up * 99;
    //}

    void SpwanGhostSkull()
    {
        GameObject skull = Instantiate(ghostSkull, createPoint.position, createPoint.rotation);
        skull.SetActive(true);

        var tweenSquence = DOTween.Sequence();
        tweenSquence.Append(skull.transform.DOMoveX(targetPoint.x, 0.8f).SetEase(Ease.Linear));
        tweenSquence.Insert(0, skull.transform.DOMoveZ(targetPoint.z, 0.8f).SetEase(Ease.Linear));
        tweenSquence.Insert(0, skull.transform.DOMoveY(targetPoint.y + 1.2f, 0.3f).SetEase(Ease.OutSine));
        tweenSquence.Insert(0.5f, skull.transform.DOMoveY(targetPoint.y + 0.5f, 0.5f).SetEase(Ease.InSine));
        tweenSquence.Append(skull.transform.DOScale(Vector3.zero, 0.3f)).OnComplete(() => Destroy(skull));

        StartCoroutine(SpawnEnemy(.5f, targetPoint));
    }

    IEnumerator SpawnEnemy(float delayTime, Vector3 position)
    {
        yield return new WaitForSeconds(delayTime);
        if (roleController.IsDie)
        {
            yield break;
        }

        BattleTool.CreateEnemy(CurrentSpawnIndex, delegate(EnemyController controller)
        {
            if (controller == null)
                return;

            controller.transform.position = position;
            //怪物出来会有个小动画。
            controller.Animator.transform.localScale = new Vector3(0, 0, 0);
            controller.Animator.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            if (SpwanCreatures != null)
            {
                SpwanCreatures.Add(controller);
            }
        });
    }
    
    //
    // void SpawnEnemy()
    // {
    //     ShowStartAttackParticle();
    //     PlayAttackAudio();
    //
    //     var enemyGo = BattleTool.CreateEnemy(EnemyIds[Random.Range(0, EnemyIds.Count)]);
    //
    //     if (enemyGo == null)
    //     {
    //         return;
    //     }
    //
    //     enemyGo.transform.position = createPoint.position;
    //     //怪物出来会有个小动画。
    //     enemyGo.Animator.transform.localScale = new Vector3(0, 0, 0);
    //     enemyGo.Animator.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
    //     if (SpwanCreatures != null)
    //     {
    //         SpwanCreatures.Add(enemyGo);
    //     }
    // }
    //
    //
    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);
        if (eventName == "CreateEnemy")
        {
            //List<Transform> spawnPoints = new List<Transform>();
            //for (int i = 0; i < enemySpwanPoints.childCount; i++)
            //{
            //    spawnPoints.Add(enemySpwanPoints.transform.GetChild(i));
            //    spawnPoints.
            //        }
            //一下放N个敌人
            CurrentSpawnIndex = EnemyIds[Random.Range(0,EnemyIds.Count)];
            List<int> spawnPointsIndex = new List<int>();
            for (int i = 0; i < SpawnCount; i++)
            {
                int randomNum;
                do
                {
                    randomNum = Random.Range(0, enemySpwanPoints.childCount);
                }
                while (spawnPointsIndex.Contains(randomNum));
                spawnPointsIndex.Add(randomNum);

                targetPoint = enemySpwanPoints.GetChild(randomNum).position;
                SpwanGhostSkull();
            }
        }
        else if (eventName.Contains(AnimatorEventName.StartAttack_))
        {

            //targetPoint = GetNearBySpwanPoint();
            //roleController.Animator.transform.DOLookAt(new Vector3(targetPoint.x, transform.position.y, targetPoint.z), 0.3f);
        }
    }

    public void OnDead()
    {
        foreach (EnemyController creature in SpwanCreatures)
        {
            if (creature != null)
            {
                if (!creature.IsDie)
                {
                    creature.HpInjured(new DamageInfo(creature.TemporaryId, 999, roleController, transform.position));
                    GameObject skull = Instantiate(ghostSkull, creature.transform.position, Quaternion.identity);
                    skull.SetActive(true);
                    Vector3 fwd = transform.position - creature.transform.position;
                    fwd.y = 0;
                    skull.transform.forward = fwd.normalized;
                    skull.transform.DOMove(createPoint.position, 1f).SetEase(Ease.InSine).OnComplete(() =>
                    {
                        skull.transform.DOScale(Vector3.zero, .2f);
                    });
                    Destroy(skull, 1.2f);
                }
            }
        }
    }
}
