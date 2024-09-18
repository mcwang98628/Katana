using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public struct BuffOverlayData
{
    //叠加的Buff层数
    public int OverlayNumber;
    //被添加的Buff
    public RoleController Role;
    //添加的Buff唯一ID
    public string Id;
}
public class RoleBuffController : MonoBehaviour
{
    private RoleController roleController;

    public List<RoleBuff> Buffs {
        get
        {
            List<RoleBuff> buffs = new List<RoleBuff>();
            foreach (KeyValuePair<string,RoleBuff> buff in buffDic)
            {
                buffs.Add(buff.Value);
            }

            return buffs;
        }
    }

    //临时ID
    Dictionary<string,RoleBuff> buffDic = new Dictionary<string, RoleBuff>();
    
    // //唯一Buff List : 唯一ID，临时ID
    // Dictionary<int,string> onlyBuffIdList_KeyValue = new Dictionary<int,string>();
    // //唯一Buff List : 临时ID，唯一ID
    // Dictionary<string,int> onlyBuffIdList_ValueKey = new Dictionary<string,int>();
    
    //到期删除的Buff，临时ID
    List<string> removeList = new List<string>();

    public int GetBuffCount(int buffId)
    {
        int count = 0;
        foreach (var buff in buffDic)
        {
            if (buff.Value.ID == buffId)
            {
                count++;
            }
        }
        return count;
    }
    
    public List<int> GetBuffsId()
    {
        List<int> itemIds = new List<int>();
        for (int i = 0; i < Buffs.Count; i++)
        {
            itemIds.Add(Buffs[i].ID);
        }
        return itemIds;
    }
    RoleBuff findRoleHaveBuff(int buffID)
    {
        foreach (var roleBuff in buffDic)
        {
            if (roleBuff.Value.ID == buffID)
            {
                return roleBuff.Value;
            }
        }
        return null;
    }
    public void AddBuff(RoleBuff buff,RoleController Adder)
    {
        if (roleController.IsDie)
        {
            return;
        }
        
        bool ShouldIniNewEffect=true;
        //唯一的Buff，重复添加就移除之前的。
        var oldBuff = findRoleHaveBuff(buff.ID);
        if (oldBuff!=null)
        {
            switch (oldBuff.BuffOverlayType)
            {
                case BuffOverlayType.NoAdd:
                    return;
                case BuffOverlayType.Normal:
                    break;
                case BuffOverlayType.AppendLife:
                    oldBuff.lifeCycle.Append();
                    return;
                    break;
                case BuffOverlayType.ReSetLife:
                    oldBuff.lifeCycle.ReSet(buff.lifeCycle);
                    return;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
            // oldBuff.trigger.Destroy();
            //
            //     oldBuff.effect.Destroy();
            // buffDic.Remove(tId);
            // onlyBuffIdList_KeyValue.Remove(buff.ID);
            // onlyBuffIdList_ValueKey.Remove(tId);
            // EventManager.Inst.DistributeEvent(EventName.OnRoleRemoveBuff,new RoleBuffEventData()
            // {
            //     BuffId = oldBuff.ID,BuffTemporaryId = oldBuff.TemporaryId,RoleId = roleController.TemporaryId
            // });
        }

        string guid = GuidTools.GetGUID();
        buffDic.Add(guid,buff);
        // if (!buff.CanOverlay)
        // {
        //     onlyBuffIdList_KeyValue.Add(buff.ID,guid);
        //     onlyBuffIdList_ValueKey.Add(guid,buff.ID);
        // }
        buff.Awake(guid, roleController, Adder, this);
        //是否应该有新的视效
        if(ShouldIniNewEffect)
        {
            buff.IniEffect();
        }

        EventManager.Inst.DistributeEvent(EventName.OnRoleAddBuff,new RoleBuffEventData(){BuffId = buff.ID,BuffTemporaryId = buff.TemporaryId,RoleId = roleController.TemporaryId});

        //叠加Buff事件----End
        
        if ( buff.overlayEffect!=null)
        {
            int overlayNumber = 0;
            foreach (KeyValuePair<string,RoleBuff> roleBuff in buffDic)
            {
                if (roleBuff.Value.ID == buff.ID)
                {
                    overlayNumber++;
                }
            }
            //减去这次添加的数量
            overlayNumber--;
            int probability;//0-100
            if (buff.overlayEffect.Probability.Count > overlayNumber)
            {
                probability = buff.overlayEffect.Probability[overlayNumber];
            }
            else
            {
                probability = buff.overlayEffect.Probability[buff.overlayEffect.Probability.Count-1];
            }

            int randomValue =Random.Range(0, 100);
            if (randomValue <= probability)
            {
                buff.TriggerOverlayEffect(overlayNumber);
            }
        }
    }
    
    

    public void RemoveBuff(RoleBuff buff)
    {
        if (!buffDic.ContainsKey(buff.TemporaryId))
        {
            return;
        }
        removeList.Add(buff.TemporaryId);
        EventManager.Inst.DistributeEvent(EventName.OnRoleRemoveBuff,new RoleBuffEventData(){BuffId = buff.ID,BuffTemporaryId = buff.TemporaryId,RoleId = roleController.TemporaryId});
    }

    private void Awake()
    {
        roleController = GetComponent<RoleController>();
    }

    private void Update()
    {
        
        
        for (int i = 0; i < removeList.Count; i++)
        {
            //临时id
            var tid = removeList[i];
            // if (onlyBuffIdList_ValueKey.ContainsKey(tid))//唯一Buff 删除
            // {
            //     //唯一id
            //     var oid = onlyBuffIdList_ValueKey[tid];
            //     onlyBuffIdList_KeyValue.Remove(oid);
            //     onlyBuffIdList_ValueKey.Remove(tid);
            // }
            buffDic.Remove(tid);
        }
        removeList.Clear();
        
        if (roleController.IsDie)
        {
            foreach (KeyValuePair<string,RoleBuff> buff in buffDic)
            {
                buff.Value.Destroy();
            }
            return;
        }
        foreach (var buff in buffDic)
        {
            if (buff.Value == null)
            {
                Debug.LogError("Error: 空");
                continue;
            }

            if (buff.Value.IsDestroy)
            {
                Debug.LogError("aaaa");
                continue;
            }
            buff.Value.Update();
        }
    }

}
