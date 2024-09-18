using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class RoleAuto : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree behaviorTree;

    private RoleController roleController;

    public GameObject Prompt;
    public float PromptDis;
    private void Awake()
    {
        // roleController = GetComponent<RoleController>();
        // Prompt.transform.localScale = Vector3.one * PromptDis;
    }

    
    private void Update()
    {
        return;
        // if (!BattleManager.Inst.AutoGame)
        // {
        //     behaviorTree.enabled = false;
        //     Prompt.SetActive(false);
        //     return;
        // }

        bool isShow = roleController.EnemyTarget != null;
        if (isShow)
        {
            isShow = (roleController.EnemyTarget.transform.position - roleController.transform.position).magnitude <=
                     PromptDis;
        }
        
        Prompt.SetActive(isShow);


        if (roleController.IsAuto)
        {
            if (!behaviorTree.enabled)
            {
                behaviorTree.enabled = true;
            }
        }
        else
        {
            if (behaviorTree.enabled)
            {
                behaviorTree.enabled = false;
            }
        }
    }
}
