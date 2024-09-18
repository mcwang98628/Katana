using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public static InteractManager Inst { get; private set; }
    FindTarget _findTarget;

    public List<InteractObj> InteractObjList => interactObjList;
    List<InteractObj> interactObjList;

    public void Init()
    {
        Inst = this;
        interactObjList = new List<InteractObj>();
    }

    public void AddInteractObj(InteractObj obj)
    {
        InteractObjList.Add(obj);
    }
    public void RemoveInteractObj(InteractObj obj)
    {
        InteractObjList.Remove(obj);
    }
    public InteractObj FindNearestInteractObj(float findDis = 999)
    {
        if (BattleManager.Inst.CurrentPlayer == null)
        {
            return null;
        }
        InteractObj result = null;
        Vector3 playerPos = BattleManager.Inst.CurrentPlayer.transform.position;
        float tempDis = findDis;
        foreach (InteractObj interactObj in InteractObjList)
        {
            if (!interactObj.CanInteract || interactObj == null)
            {
                continue;
            }
            float distance = Vector3.Distance(playerPos, interactObj.transform.position);
            if (distance < tempDis)
            {
                tempDis = distance;
                result = interactObj;
            }
        }
        return result;
    }

    public bool IsCanInteract()
    {
        if (_findTarget == null)
        {
            if (BattleManager.Inst.CurrentPlayer == null)
            {
                return false;
            }
            else
            {
                _findTarget = BattleManager.Inst.CurrentPlayer.FindEnemyTarget;
            }
        }
        if (_findTarget == null)
        {
            return false;
        }
        if (_findTarget.InteractTarget == null)
        {
            return false;
        } 
        return true;
    }
    public bool TryInteract()
    {
         if (_findTarget == null)
        {
            if (BattleManager.Inst.CurrentPlayer == null)
            {
                return false;
            }
            else
            {
                _findTarget = BattleManager.Inst.CurrentPlayer.FindEnemyTarget;
            }
        }
        if (_findTarget == null)
        {
            return false;
        }
        if (_findTarget.InteractTarget == null)
        {
            return false;
        }
        _findTarget.InteractTarget.InteractStart();
        return true;
    }
    public void Reset()
    {
        interactObjList = new List<InteractObj>();
    }



}
