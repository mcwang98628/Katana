using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    private PlayerController roleController;
    [ShowInInspector]
    public RoleController EnemyTarget => enemyTarget;
    private RoleController enemyTarget;
    [ShowInInspector]
    public InteractObj InteractTarget => interactTarget;
    private InteractObj interactTarget;
    [ShowInInspector]
    public BreakableObj BreakableTarget => breakableTarget;
    private BreakableObj breakableTarget;
    [SerializeField]
    public float FindEnemyDis = 6;
    public float FindInteractDis = 2;
    private void Awake()
    {
        roleController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (roleController && !roleController.IsAttacking ||
            roleController.EnemyTarget == null ||
            roleController.EnemyTarget.IsDie)
        {
            UpdateTarget();
        }
    }

    void UpdateTarget()
    {
        //更新目标
        enemyTarget = BattleTool.FindNearestEnemy(roleController, FindEnemyDis);
        if(enemyTarget!=null)
        {
            interactTarget=null;
            breakableTarget=null;
            return;
        }
        InteractObj newInterOnj = InteractManager.Inst.FindNearestInteractObj(FindInteractDis);
        if (newInterOnj != null)
        {
            if (interactTarget == null)
            {
                newInterOnj.SelectStart();
            }
            else if (interactTarget != newInterOnj)
            {
                interactTarget.SelectEnd();
                newInterOnj.SelectStart();
                
            }
            breakableTarget=null;
        }
        interactTarget = newInterOnj;
        breakableTarget= BreakableObjManager.Inst.FindNearestBreakableObj(FindInteractDis);

    }

}
