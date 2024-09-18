using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Gold_JumpText : MonoBehaviour
{
    public float Offset;
    Text _text;
    //Transform Target;
    public float textStayTime = 2f;
    //累加的时间
    public float textSumTime = 0.8f;
    float LastReceiveTimeStamp = -999;
    float CurrentMoneyCount;
    bool IsSumming;
    private RoleController roleController;
    private void Awake()
    {
        _text = GetComponent<Text>();
        //_text.enabled
        EventManager.Inst.AddEvent(EventName.OnRoleRegistered, OnRoleRegis);
        EventManager.Inst.AddEvent(EventName.OnAddMoney, OnAddGold);
    }
    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnAddMoney, OnAddGold);
        EventManager.Inst.RemoveEvent(EventName.OnRoleRegistered, OnRoleRegis);
    }
    private void OnRoleRegis(string eventName, object role_)
    {
        SetTarget();
    }
    public void SetTarget()
    {
        //Target = BattleManager.Inst.CurrentPlayer.GetComponent<RoleNode>().Head;
        roleController = BattleManager.Inst.CurrentPlayer.GetComponent<RoleController>();
    }

    public void OnAddGold(string eventName, object MoneyCount)
    {
        //Debug.Log("ReceiveAddGoldEvent");
        //临时做法，不然花钱会出错
        //if ((int)MoneyCount < 0)
        //    return;
            //检测到得钱，那么就会重置
        if (((int)MoneyCount > 0 && CurrentMoneyCount < 0) || ((int)MoneyCount < 0 && CurrentMoneyCount > 0))
        {
            CurrentMoneyCount = (int)MoneyCount;
        }
        else
        {
            CurrentMoneyCount += (int)MoneyCount;
        }


        if (CurrentMoneyCount < 0)
        {
            _text.text =  CurrentMoneyCount.ToString() + "G";
        }
        else
        {
            _text.text = "+" + CurrentMoneyCount.ToString() + "G";
        }
        LastReceiveTimeStamp = Time.time;
    }
    private void LateUpdate()
    {
        if (roleController != null)
        {
            //字体停留
            if (Time.time > LastReceiveTimeStamp + textStayTime)
            {
                _text.enabled = false;
                //CurrentMoneyCount = 0;
            }
            else
            {
                _text.enabled = true;
            }
            //金钱重置
            if(Time.time>LastReceiveTimeStamp+textSumTime)
            {
                CurrentMoneyCount = 0;
            }


            //设置位置
            if (Camera.main != null)
            {
                Vector3 targetPos;
                if (roleController.roleNode.Head != null)
                {
                    targetPos = roleController.roleNode.Head.position;
                }
                else
                {
                    targetPos = roleController.transform.position;
                }
                transform.position = Camera.main.WorldToScreenPoint(targetPos) + new Vector3(0, Offset, 0);
            }
        }
    }
}
