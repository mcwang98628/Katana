using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyNest : MonoBehaviour
{
    [SerializeField]
    private GameObject fireFly;

    [SerializeField]
    private float interval;
    [SerializeField]
    private int minFireFlyCount;
    [SerializeField]
    private int maxFireFlyCount;

    [SerializeField]
    private Transform nestEffect;

    private void Start()
    {
        InvokeRepeating("SpawnFireFly", interval, interval);
    }


    void SpawnFireFly()
    {
                   
        int fireFlyCount = UnityEngine.Random.Range(minFireFlyCount, maxFireFlyCount + 1);

        for (int i = 0; i < fireFlyCount; i++)
        {
            GameObject go = Instantiate(fireFly, nestEffect.transform.position, transform.rotation);
            go.GetComponent<DmgBuffOnTouch>().Init(BattleManager.Inst.CurrentPlayer);
        }
    }
}
