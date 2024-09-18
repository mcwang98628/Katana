using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("BehaviorExpansion/MyAI")]
public class AIActionRunAway : AIAction
{
    bool IsRunningAway = true;
    public SharedFloat WaitTime;
    public SharedFloat MinTolaDistance;
    public override void OnStart()
    {
        base.OnStart();
        IsRunningAway = true;
        GetComponent<EnemyFlashAway>().dir = EnemyWallTester.GetNoWallDir(transform, MinTolaDistance.Value);
        GetComponent<EnemyFlashAway>().Dodge();
        StartCoroutine(waitRunaway());
    }
    public IEnumerator waitRunaway()
    {
        yield return new WaitForSeconds(WaitTime.Value);
        IsRunningAway = false;
    }
    public override TaskStatus OnUpdate()
    {
        if(!IsRunningAway)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }
    }


}
public class EnemyWallTester
{
    public static Vector3 GetNoWallDir(Transform trans,float MinTolaDistance)
    {
        CapsuleCollider _collider = trans.GetComponent<CapsuleCollider>();
        float ColliderRadius=0;
        if (trans.GetComponent<CapsuleCollider>() != null)
        {
            ColliderRadius = trans.GetComponent<CapsuleCollider>().radius;
        }
        int RaycastStep = 30;
        Vector3 CurrentDir;
        //RaycastHit[] hit;
        RaycastHit hit;
        //List<RaycastHit> hits = new List<RaycastHit>();
        List<Vector3> Dirs = new List<Vector3>();
        float eachStep = 360f / (float)RaycastStep;
        //四周射线检测。再从中随机
        for (int i = 0; i < RaycastStep; i++)
        {
            CurrentDir = new Vector3(Mathf.Cos(eachStep * i), 0, Mathf.Sin(eachStep * i));
            //hit = Physics.RaycastAll(trans.position + CurrentDir*(ColliderRadius+0.01f)+ new Vector3(0, 0.1f, 0), CurrentDir, MinTolaDistance);
            //foreach (RaycastHit _hit in hit)
            //{
            //    if (_hit.collider == null)
            //    {
            //        Dirs.Add(CurrentDir);
            //    }
            //}
            Physics.Raycast(trans.position + CurrentDir * (ColliderRadius + 0.01f) + new Vector3(0, 0.1f, 0), CurrentDir, out hit, MinTolaDistance);
            if (hit.collider == null)
            {
                Dirs.Add(CurrentDir.normalized);
            }
        }
        if(Dirs.Count==0)
        {
            Debug.Log("RaycastFail");
            return -trans.GetComponent<RoleController>().Animator.transform.forward;
        }
        else
        return Dirs[Random.Range(0, Dirs.Count)];
    }
}
