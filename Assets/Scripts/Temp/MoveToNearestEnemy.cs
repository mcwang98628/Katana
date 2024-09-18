using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNearestEnemy : MonoBehaviour
{
    RoleController Target;
    public float Speed;
    //多少距离可以判断为很近并开始停止
    public float NearDistance;
    public float RotateMul = 0.2f;
    public float SpeedLerpMul;
    float CurrentSpeed;
    RoleController target;
    public enum IdleActionEnum
    { 
        Stop,
        FollowPlayer

    }
    public IdleActionEnum IdleActionType;
    private void Update()
    {
        //如果检测到敌人
        target = BattleTool.FindNearestEnemy(transform);
        if (target != null)
        {

            if (VectorToolkit.CompareDistance(target.transform.position, transform.position, NearDistance) == 1)
            {
                transform.LerpLookAt(target.transform, RotateMul* Time.deltaTime);
                CurrentSpeed.LerpTo(Speed, SpeedLerpMul* Time.deltaTime);
            }
            else
            {
                transform.LerpLookAt(target.transform, RotateMul * Time.deltaTime);
                CurrentSpeed.LerpTo(0, SpeedLerpMul* Time.deltaTime);
            }
        }
        else if (target == null)
        {
            switch (IdleActionType)
            {
                case IdleActionEnum.FollowPlayer:
                    transform.LerpLookAt(BattleManager.Inst.CurrentPlayer.transform, RotateMul * Time.deltaTime);
                    CurrentSpeed.LerpTo(Speed, SpeedLerpMul * Time.deltaTime);
                    break;
                case IdleActionEnum.Stop:
                    CurrentSpeed.LerpTo(0, SpeedLerpMul * Time.deltaTime);
                    break;

            }

        }
        transform.Translate(Vector3.forward * CurrentSpeed * Time.deltaTime);
    }



}
