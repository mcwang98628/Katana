using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct SpawnProp
{
    public GameObject SpwanPrefab;
    public float Possibilty;
}
public class EnemyDropOnDeath : MonoBehaviour
{    
    public List<SpawnProp> SpawnList;

    public void OnDeath()
    {
        StartCoroutine(DelaySpwanObj(0.5f));
    }
    IEnumerator DelaySpwanObj(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        //DropManager.Inst.SpwanDropObj(transform.position, SpawnList);
    }
}
