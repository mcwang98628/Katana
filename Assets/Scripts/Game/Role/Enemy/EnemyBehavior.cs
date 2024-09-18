using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public bool ActiveOnSpawn = true;
    private RoleController roleController;
    private BehaviorTree behaviorTree;
    private void Awake()
    {
        roleController = GetComponent<RoleController>();
        behaviorTree = GetComponent<BehaviorTree>();

        EventManager.Inst.AddEvent(EventName.OnRoleDead, OnRoleDead);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead, OnRoleDead);
    }

    private void OnRoleDead(string arg1, object arg2)
    {
        RoleDeadEventData data = (RoleDeadEventData)arg2;
        if (data.DeadRole.TemporaryId == roleController.TemporaryId)
        {
            if (behaviorTree != null)
            {
                // Destroy(behaviorTree);
                behaviorTree.DisableBehavior();
            }
        }
    }

    private void Start()
    {
        // behaviorTree.enabled = false;
        behaviorTree.DisableBehavior(true);
        if (ActiveOnSpawn)
        {
            if (roleController.roleTeamType == RoleTeamType.Enemy_Boss || roleController.roleTeamType == RoleTeamType.EliteEnemy)
            {
                EnableBeahavior(2f);
            }
            else
            {
                EnableBeahavior(0.8f);
            }
        }

    }
    public void EnableBeahavior(float time = 0)
    {
        StartCoroutine(AwakeBeahaviorTree(time));
    }
    //public void DisableBehavior()
    //{
    //    if (!roleController.IsDie)// && !roleController.IsDizziness)
    //    {
    //        behaviorTree.enabled = false;
    //    }
    //}
    IEnumerator AwakeBeahaviorTree(float time)
    {
        yield return new WaitForSeconds(time);
        if (!roleController.IsDie)// && !roleController.IsDizziness)
        {
            // behaviorTree.enabled = true;
            behaviorTree.EnableBehavior();
        }
    }
    //public void OnDead()
    //{
    //    Destroy(behaviorTree);
    //}



    IEnumerator ResetBehaviorTreeIE(float delayTime)
    {
        if (behaviorTree == null)
        {
            yield break;
        }
        // behaviorTree.enabled = false;
        behaviorTree.DisableBehavior(true);
        yield return new WaitForSeconds(delayTime);
        if (behaviorTree == null)
        {
            yield break;
        }
        if (!roleController.IsDie)
        {
            // behaviorTree.enabled = true;
            behaviorTree.EnableBehavior();
        }
    }
    Coroutine coroutine;
    public void ResetBehaviorTree(float delayTime = 0.6f)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            // if (behaviorTree != null)
                // behaviorTree.enabled = true;
                 behaviorTree.EnableBehavior();
        }
        coroutine = StartCoroutine(ResetBehaviorTreeIE(delayTime));

    }

}
