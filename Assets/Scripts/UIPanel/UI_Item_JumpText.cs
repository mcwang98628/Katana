using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Item_JumpText : MonoBehaviour
{
    public float Offset;
    Text _text;
    //Transform Target;
    public float textStayTime;
    float LastReceiveTimeStamp = -999;
    float CurrentMoneyCount;
    private RoleController roleController;
    private void Awake()
    {
        _text = GetComponent<Text>();
        //_text.enabled
        EventManager.Inst.AddEvent(EventName.OnRoleRegistered, OnRoleRegis);
        EventManager.Inst.AddEvent(EventName.OnRoleAddItem, OnGetItem);
    }
    private void OnDestroy()
    {

        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem, OnGetItem);
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
    public void OnGetItem(string eventName, object itemEventData)
    {
        Debug.Log("GetItem");
        LastReceiveTimeStamp = Time.time;
        RoleItemEventData ried = (RoleItemEventData)itemEventData;
        var item = DataManager.Inst.GetItemScrObj(ried.Item.ID);
        
        if (ried.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }
        else
        {
            _text.text = "获得 "+ item.name ;
        }
    }
    private void LateUpdate()
    {
        if (roleController != null)
        {
            //改变颜色
            if (Time.time > LastReceiveTimeStamp + textStayTime)
            {
                _text.enabled = false;
                CurrentMoneyCount = 0;
            }
            else
            {
                _text.enabled = true;
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
