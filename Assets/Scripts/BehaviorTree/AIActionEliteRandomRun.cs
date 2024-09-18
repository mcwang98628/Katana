using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class AIActionEliteRandomRun : AIAction
{
    //能忍受的最短距离
    public SharedFloat MinTolaDistance;
    //每次射线检测的距离
    //public SharedFloat MaxDistance;
    public float MinMoveTime;
    public float MaxMoveTime;
    public Vector3 Dir;
    float MoveTime;
    float StartTime;
    int RaycastStep=30;
    // Start is called before the first frame update
    public override void OnStart()
    {
        MoveTime = Random.Range(MinMoveTime,MaxMoveTime);
        StartTime = Time.time;
        //射线检测，直到找到一条出路。
        Dir= EnemyWallTester.GetNoWallDir(transform,MinTolaDistance.Value);
        //Debug.Log("Dir"+Dir);
    }

    public override TaskStatus OnUpdate()
    {
        //Debug.Log(Dir);
        if(Time.time<StartTime+MoveTime)
        {
            Debug.Log("InputMove"+ Dir);
            RoleController.InputMove(new Vector2(Dir.x,Dir.z));
            return TaskStatus.Running;
        }
        else
        {
            RoleController.InputMove(Vector2.zero);
            return TaskStatus.Success;
        }
    }
}
