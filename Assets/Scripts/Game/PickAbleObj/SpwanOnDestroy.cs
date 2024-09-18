using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpwanOnDestroy : MonoBehaviour
{
    public List<SpawnProp> SpawnList;
    RoleHealth roleHealth;
    [System.Serializable]
    public struct SpawnProp
    {
        public GameObject SpwanPrefab;
        public float Possibilty;
    }
    private void Start() {
        roleHealth = GetComponent<RoleHealth>();
        roleHealth.OnDead.AddListener(Spawn);
    }
    public void Spawn()
    {
        for(int i=0;i<SpawnList.Count;i++)
        {
            if(Random.value<SpawnList[i].Possibilty)
                Instantiate(SpawnList[i].SpwanPrefab,transform.position,Quaternion.identity);
        }
    }
    private void OnDestroy() {
        roleHealth.OnDead.RemoveListener(Spawn);
    }
}
