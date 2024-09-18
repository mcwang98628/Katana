using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BreakableObjSpawnOnDestroy : MonoBehaviour
{
    public List<SpawnProp> SpawnList;
    [System.Serializable]
    public struct SpawnProp
    {
        public GameObject SpwanPrefab;
        public float Possibilty;
    }
    //private void OnDestroy()
    //{
    //    Spawn();
    //}
    public void Spawn()
    {
        for (int i = 0; i < SpawnList.Count; i++)
        {
            if (Random.value < SpawnList[i].Possibilty)
            {
                GameObject CurrentSpawnObj = Instantiate(SpawnList[i].SpwanPrefab, transform.position, Quaternion.identity);                            
                return;
            }
        }
    }
}
