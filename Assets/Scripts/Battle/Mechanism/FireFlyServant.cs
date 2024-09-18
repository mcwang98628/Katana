using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyServant : MonoBehaviour
{
    public float Interval = 0.1f;
    public GameObject FireFly;
    public Transform EmmitPoint;
    private void Start()
    {
        StartCoroutine(EmmitFireFly());
    }
    public IEnumerator EmmitFireFly()
    {
        while (true)
        {
            GameObject fly = Instantiate(FireFly, EmmitPoint.transform.position, Quaternion.identity);
            fly.GetComponent<ProjectileTrace_FireFly>().Init(BattleManager.Inst.CurrentPlayer,-1);
            fly.GetComponent<ProjectileTrace_FireFly>().SetTarget(BattleTool.FindNearestEnemy(BattleManager.Inst.CurrentPlayer));
            yield return new WaitForSeconds(Interval);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
