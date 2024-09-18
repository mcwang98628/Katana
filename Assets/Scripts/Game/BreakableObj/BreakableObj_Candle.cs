using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class BreakableObj_Candle : BreakableObj
{
     [Header("物理模拟")]
    public float HitStrength;
    public float HitHeight;

    public override void BreakObj()
    {
        base.BreakObj();
        PhysicSimulate();
    }
    //这里之后改成手动计算
    void PhysicSimulate()
    {
        shards = new List<GameObject>();
        if (Shard != null)
        {
            for (int i = 0; i < Shard.transform.childCount; i++)
            {
                shards.Add(Shard.transform.GetChild(i).gameObject);
            }
        }

        //计算击飞的力道
        Vector3 Force = (transform.position - BattleManager.Inst.CurrentPlayer.transform.position);
        Force.y = HitHeight;
        Force *= HitStrength;
        for (int i = 0; i < shards.Count; i++)
        {
            Vector3 CurrentForce = Force * Random.Range(0.6f, 1f);
            if (shards[i].GetComponent<Rigidbody>() != null)
            {
                shards[i].GetComponent<Rigidbody>().AddForce(CurrentForce, ForceMode.Impulse);
                StartCoroutine(DelayDisablePhysics());
            }
        }
    }
    public IEnumerator DelayDisablePhysics()
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < shards.Count; i++)
        {
            shards[i].SetActive(false);
        }
    }

}
