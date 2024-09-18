// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class EnemyPool : MonoBehaviour
// {
//     public bool IsDebug;
//     public List<GameObject> EnemyWave;
//     public int CurrentIndex;
//     public float SpawnDelay;
//     public Door[] Doors;
//     bool IsTriggered;
//     public string RoomName;
//     public ItemPoolScriptableObject PropPool;
//
//     public ParticleSystem AppearParticle;
//
//     public void AutoSearchDoor()
//     {
//         //根据名字找门
//         //Debug.Log("TrySearchDoor");
//         GameObject Room = GameObject.Find(RoomName);
//         if(Room!=null)
//         Doors =Room.GetComponentsInChildren<Door>();
//     }
//     
//     public void GetEnemyWave()
//     {
//         EnemyWave = new List<GameObject>();
//         for(int i=0;i<transform.childCount;i++)
//         {
//             if(transform.GetChild(i).name.Contains("Wave"))
//             {
//                 transform.GetChild(i).name = "Wave" + (EnemyWave.Count+1);
//                 EnemyWave.Add(transform.GetChild(i).gameObject);
//             }
//         }
//         //foreach(Transform child in transform)
//         //{
//         //    if(child.gameObject.name.Contains("Wave"))
//         //    {
//         //        EnemyWave.Add(child.gameObject);
//         //    }
//         //}
//     }
//     
//     //把怪物放进池子
//     public void PutEnemyInWave(GameObject Obj,int wave)
//     {
//         Obj.transform.SetParent(EnemyWave[wave].transform);
//     }
//
//     public void ShowWaveInEditor(int wave)
//     {
//         DisableAll();
//         if (EnemyWave.Count > wave)
//             EnemyWave[wave].SetActive(true);
//     }
//     //显示某池子
//     public void ShowWave(int wave)
//     {
//         StartCoroutine(ShowWaveIE(wave));
//     }
//     IEnumerator ShowWaveIE(int wave)
//     {
//         DisableAll();
//         //直接显示一个特效
//         //CurrentIndex += 1;
//         for (int i = 0; i < EnemyWave[wave].transform.childCount; i++)
//         {
//             if(AppearParticle!=null)
//             Instantiate(AppearParticle, EnemyWave[wave].transform.GetChild(i).transform.position, Quaternion.identity);
//         }
//         yield return new WaitForSeconds(1f);
//         if (EnemyWave.Count > wave)
//             EnemyWave[wave].SetActive(true);
//
//     }
//     public void DisableAll()
//     {
//         if (EnemyWave.Count > 0)
//         {
//             foreach (GameObject Obj in EnemyWave)
//             {
//                 Obj.SetActive(false);
//             }
//         }
//     }
//    
//     private void Start()
//     {
//
//         if (IsDebug)
//         {
//             //AutoSearchDoor();
//             GetEnemyWave();
//             DisableAll();
//             CurrentIndex = 0;
//
//
//             StartSpawnEnemies();
//         }
//     }
//     public void StartSpawnEnemies()
//     {
//         if (!IsTriggered)
//         {
//             StartCoroutine(SpawnEnemiesIE());
//             IsTriggered = true;
//         }
//     }
//     //逐波释放敌人
//     public IEnumerator SpawnEnemiesIE()
//     {
//         if (EnemyWave.Count > 0)
//         {
//             CloseRoom();
//             yield return new WaitForSeconds(SpawnDelay);
//             ShowWave(0);
//             yield return new WaitForSeconds(2);
//             while (CurrentIndex < EnemyWave.Count)
//             {
//                 if (CheckEnemiesAllDied())
//                 {
//                     //Debug.Log("AllDied");
//                     CurrentIndex += 1;
//                     yield return new WaitForSeconds(SpawnDelay);
//                     ShowNextWave();
//
//                 }
//                 yield return null;
//             }
//             OpenRoom();
//             yield return new WaitForSeconds(SpawnDelay);
//             SpawnTreasure();
//         }
//     }
//     //释放宝箱
//     public void SpawnTreasure()
//     {
//         ItemScriptableObject testItem = PropPool.Items[UnityEngine.Random.Range(0, PropPool.Items.Count)];
//         BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(testItem));
//     }
//     //打开房间
//     public void OpenRoom()
//     {
//         if(Doors!=null)
//         {
//             for(int i=0;i<Doors.Length;i++)
//             {
//                 //Debug.Log("TryOpenDoors");
//                 Doors[i].OpenDoor();
//             }
//         }
//     }
//     //进入房间时，关闭房间
//     public void CloseRoom()
//     {
//         if (Doors != null)
//         {
//             for (int i = 0; i < Doors.Length; i++)
//             {
//                 //Debug.Log("TryCloseDoors");
//                 Doors[i].CloseDoor();
//             }
//         }
//     }
//
//     public void ShowNextWave()
//     {
//         //CurrentIndex += 1;
//         //CurrentIndex += 1;
//         if(CurrentIndex<EnemyWave.Count)
//         ShowWave(CurrentIndex);
//     }
//
//     //有很大的改进空间
//     public bool CheckEnemiesAllDied()
//     {
//         for(int i=0;i<EnemyWave[CurrentIndex].transform.childCount;i++)
//         {
//             //Debug.Log("CurrentWave:"+CurrentIndex);
//             //Debug.Log(EnemyWave[CurrentIndex].transform.GetChild(i));
//             if (EnemyWave[CurrentIndex].transform.GetChild(i) != null)
//             {
//                 if (!EnemyWave[CurrentIndex].transform.GetChild(i).GetComponent<RoleController>().IsDie)
//                 {
//                     return false;
//                 }
//             }
//         }
//         return true;
//     }
//
//     
// }
