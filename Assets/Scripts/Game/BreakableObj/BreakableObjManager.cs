using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjManager : MonoBehaviour
{
    public static BreakableObjManager Inst { get; private set; }
    FindTarget _findTarget;
    
    public List<BreakableObj> BreakableObjList => breakableObjlist;
    private List<BreakableObj> breakableObjlist;
    [SerializeField]private FeedBackObject OnBreakObjFeedBack;

    public void Init()
    {
        Inst = this;
        breakableObjlist = new List<BreakableObj>();
    }

    public void AddBreakableObj(BreakableObj obj)
    {
        breakableObjlist.Add(obj);
    }
    public void RemoveBreakableObj(BreakableObj obj)
    {
        if (breakableObjlist.Contains(obj))
        {
            breakableObjlist.Remove(obj);
        }
    }
    public BreakableObj FindNearestBreakableObj(float findDis = 999)
    {
        if (BattleManager.Inst.CurrentPlayer == null)
        {
            return null;
        }
        BreakableObj result = null;
        Vector3 playerPos = BattleManager.Inst.CurrentPlayer.transform.position;
        float tempDis = findDis;
        foreach (BreakableObj breakableObj in breakableObjlist)
        {
            if (breakableObj.IsBroken || breakableObj == null)
            {
                continue;
            }
            float distance = Vector3.Distance(playerPos, breakableObj.transform.position);
            if (distance < tempDis)
            {
                tempDis = distance;
                result = breakableObj;
            }
        }
        return result;
    }
    public List<BreakableObj> BreakObjsInRange(RoleController player,float radius,float angle)
    {
        List<BreakableObj> value = new List<BreakableObj>();
        foreach (BreakableObj breakableObj in breakableObjlist)
        {
            if (breakableObj.IsBroken || breakableObj == null)
            {
                continue;
            }
            Vector3 v3 = breakableObj.transform.position - player.transform.position;
            float ang = Vector3.Angle(player.Animator.transform.forward, v3);

            if(v3.magnitude<radius && ang<angle/2)
            {
                value.Add(breakableObj);
            }
        }

        return value;
    }
    public void PlayFeedBack()
    {
        FeedbackManager.Inst.UseFeedBack(BattleManager.Inst.CurrentPlayer,OnBreakObjFeedBack);
    }
}
