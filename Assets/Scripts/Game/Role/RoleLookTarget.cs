using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoleLookTarget : MonoBehaviour
{
    private RoleController roleController;
    private bool isUse;
    [ShowInInspector]
    private Transform Target => target;
//    [SerializeField]
    private Transform target;

    [SerializeField]
    private float targetDis;

    [SerializeField]
    private bool debug_useTarget;
    
    private void Awake()
    {
        roleController = GetComponent<RoleController>();
    }
    
    
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

//    private void Update()
//    {
//        //TODO: Debug
//        if (!debug_useTarget)
//        {
//            roleController.SetIsHaveLookTarget(false);
//            return;
//        }
//        //TODO: Debug
//        
//        roleController.SetIsHaveLookTarget(target != null);
//        UpdateTarget();
//                    
//        if (target == null)
//        {
//            return;
//        }
//
//        if (roleController.IsCanMove)
//        {
//            var position = (target.position - transform.position).normalized;
//            roleController.Animator.transform.forward = new Vector3(position.x,0,position.z);
//        }
//
//    }

    void UpdateTarget()
    {
        Vector3 playerPos = BattleManager.Inst.CurrentPlayer.transform.position;
        if (roleController.IsPlayer)
        {
            float minDis = 9999999;
            RoleController minEnemy = null;
            foreach (KeyValuePair<string,RoleController> enemy in BattleManager.Inst.EnemyTeam)
            {
                if (enemy.Value.IsDie)
                {
                    continue;
                }
                Vector3 v3 = enemy.Value.transform.position - playerPos;
                if (v3.magnitude <= targetDis && v3.magnitude <= minDis)
                {
                    minEnemy = enemy.Value;
                    minDis = v3.magnitude;
                }
            }

            if (minEnemy != null)
            {
                SetTarget(minEnemy.transform);
            }
            else
            {
                SetTarget(null);
            }
        }
        else
        {
            Vector3 v3 = playerPos - roleController.transform.position;
            if (v3.magnitude <= targetDis)
            {
                SetTarget(BattleManager.Inst.CurrentPlayer.transform);
            }
        }
    }
}
