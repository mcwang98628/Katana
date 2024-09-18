using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;



[Serializable]
public class ItemEffectTrigger
{
    public TriggerType triggerType = TriggerType.Times;
//    public Action Trigger = delegate {  };
    public ItemGroup Root;
    protected RoleItemController roleItemController;
    public ItemEffectTrigger(TriggerType type)
    {
        triggerType = type;
    }

    public virtual void Awake(RoleItemController rpe)
    {
        roleItemController = rpe;
    }
    
    public virtual void Update(RoleItemController rpe){}
    public virtual void Destroy(RoleItemController rpe){}

    private MethodInfo effectOnTriggerFunc;
    public virtual void TriggerEffect(params object[] args)
    {
        if (effectOnTriggerFunc!=null)
        {
            effectOnTriggerFunc.Invoke(Root.itemEffect,args);
        }
        else
        {
            effectOnTriggerFunc = GameTools.CallFunc(Root.itemEffect,"OnTrigger",args);
        }
    }
}

public struct ItemEffectTriggerValue
{
    public string TargetId;
    public DamageInfo DamageInfo;
    public Vector3 TargetPosition;
    public RoleController TargetRole;
}
[Serializable]
public class ItemEffect
{
    public ItemGroup Root;
    protected RoleController roleController;
    //触发
    public virtual void TriggerEffect(ItemEffectTriggerValue? value)
    {
        DistributeTriggerEvent();
    }

    void DistributeTriggerEvent()
    {
        if (Root == null || Root.item == null)
        {
            return;
        }
        EventManager.Inst.DistributeEvent(EventName.ItemTrigger,Root.item);
        EventManager.Inst.DistributeEvent(EventName.ItemEffectTrigger,this.GetType());
    }
    
    public virtual void OnBtnTouch(bool isDown)
    {
        
    }
    public virtual void Awake(RoleItemController rpe)
    {
        roleController = rpe.roleController;
    }
    public virtual void Update(RoleItemController rpe){}
    public virtual void Destroy(RoleItemController rpe){}
}

[Serializable]
public class ItemGroup
{
    public string Name;
    public string Desc;
    public ItemEffectTrigger effectTrigger;
    public ItemEffect itemEffect;
    public Item item;
    private RoleItemController Rpe;
    public virtual void Awake(RoleItemController rpe)
    {
        Rpe = rpe;
        if (itemEffect == null)
        {
            Debug.LogError("ItemError: ItemEffect==null");
        }
        itemEffect.Root = this;
        itemEffect.Awake(rpe);
        effectTrigger.Root = this;
        effectTrigger.Awake(rpe);
    }

    public virtual void Update(RoleItemController rpe)
    {
        effectTrigger.Update(rpe);
        itemEffect.Update(rpe);
    }

    public virtual void Destroy(RoleItemController rpe)
    {
        effectTrigger.Destroy(rpe);
        itemEffect.Destroy(rpe);
        //Rpe.ReMoveItem(item);
    }
}

[Serializable]
public class Item
{
    public int ID;
    public bool isUnique;
    public Sprite Icon;
    public string Name;
    public string Desc;
    public ItemType ItemType;
    public ItemUseType ItemUseType;
    public ArtifactType ArtifactType;
    [FormerlySerializedAs("QualityType")] public ItemColor itemColorType;
    // public string Id;
    public string TemporaryId;
    public VisualObjSlotType VisualObjSlot;
    public GameObject VisualObj;
    public List<ItemGroup> ItemGroups = new List<ItemGroup>();
    public RoleItemController Root;

    public bool IsUiDisplay;//是否在UI显示。

    public Item()
    {
    }


    public void Awake(RoleItemController rpe)
    {
        for (int i = 0; i < ItemGroups.Count; i++)
        {
            ItemGroups[i].Awake(rpe);
        }
        lastUseTime = -99;
        EventManager.Inst.AddEvent(EventName.ItemCoolingLess,OnItemCoolingLess);
      
    }

    public void Destroy(RoleItemController rpe)
    {
        for (int i = 0; i < ItemGroups.Count; i++)
        {
            ItemGroups[i].Destroy(rpe);
        }
        EventManager.Inst.RemoveEvent(EventName.ItemCoolingLess,OnItemCoolingLess);
    }
    public void SetActiveData(int canuseTime,float collingTime)
    {
        this.canUseTimes = canuseTime;
        this.coolingTime = collingTime;
    }
    
    private int canUseTimes;//可以使用次数
    private float coolingTime;//冷却需要的时间
    public float CoolingTime
    {
        get
        {
            float time = coolingTime;
            if (ItemType == ItemType.ButtonSkill)
            {
                time = coolingTime * Root.roleController.SkillCoolingScale;
            }
            return time;
        }
    }
    private float lastUseTime=0f;//最后一次使用的时间 Time.time
    private int useTimes;//已经使用的次数
    
    public float CoolingPercentage//冷却进度 0-1
    {
        get
        {
            // if (ItemType == ItemType.Skill)
            // {
            //     if (BattleManager.Inst.CurrentPlayer == null)
            //     {
            //         return 0;
            //     }
            //     return BattleManager.Inst.CurrentPlayer.CurrentSkillPower / (float)BattleManager.Inst.CurrentPlayer.MaxSkillPower;
            // }
            // else
            {
                float value = (Time.time - lastUseTime) / CoolingTime;
                if (value>1)
                {
                    value = 1;
                }
                return value;
            }
        }
    }
    public int RemainingTimes//剩余次数
    {
        get
        {
            if (canUseTimes == -1)
            {
                return 999;
            }
            return canUseTimes - useTimes;
        }
    }

    public void RefreshCool()
    {
        lastUseTime = Time.time - CoolingTime;
    }

    public void AddUseTimes(int times)
    {
        canUseTimes += times;
        EventManager.Inst.DistributeEvent(EventName.OnItemDataUpdate, this);
    }

    public void UseActive()
    {
        if (ItemUseType != ItemUseType.Click )
        {
            return;
        }

        var player = BattleManager.Inst.CurrentPlayer;
        if (!player.IsAcceptInput ||player.IsFreeze || player.IsDie || player.IsDizziness)// || player.IsRolling)
        {
            return;
        }
        if (CoolingPercentage == 1f && RemainingTimes>0)
        {
            // var player = BattleManager.Inst.CurrentPlayer;
            // player.AddSkillPower(-player.MaxSkillPower);
            lastUseTime = Time.time;
            useTimes++;
            //触发效果
            for (int i = 0; i < ItemGroups.Count; i++)
            {
                ActiveTrigger activeTrigger = ItemGroups[i].effectTrigger as ActiveTrigger;
                if (activeTrigger == null)
                {
                    continue;
                }
                activeTrigger.Active();
            }
            if (RemainingTimes<=0)
            {
                Root.ReMoveItem(this);
            }

            EventManager.Inst.DistributeEvent(EventName.OnItemDataUpdate, this);
            EventManager.Inst.DistributeEvent(EventName.OnUseItem, this);
        }
    }

    public void OnBtnTouch(bool isDown)
    {
        if (ItemUseType != ItemUseType.Hold)
        {
            return;
        }
        if (!BattleManager.Inst.CurrentPlayer.IsAcceptInput && isDown)
        {
            return;
        }

        if (CoolingPercentage == 1f && RemainingTimes > 0)
        {
            if (!isDown)
            {
                var player = BattleManager.Inst.CurrentPlayer;
                player.AddSkillPower(-player.MaxSkillPower);
                lastUseTime = Time.time;
                useTimes++;
            }
            
            for (int i = 0; i < ItemGroups.Count; i++)
            {
                ActiveTrigger activeTrigger = ItemGroups[i].effectTrigger as ActiveTrigger;
                if (activeTrigger == null)
                {
                    continue;
                }
                activeTrigger.OnBtnTouch(isDown);
            }
            if (RemainingTimes<=0)
            {
                Root.ReMoveItem(this);
            }

        }
    }

    public void OnDrag(bool isDown,Vector2 dir)
    {
        
    }



    private void OnItemCoolingLess(string arg1, object arg2)
    {
        float time = (float) arg2;
        LessCoolingTime(time);
    }
    public void LessCoolingTime(float timeValue)
    {
        lastUseTime -= timeValue;
    }

    public static Color GetColor(ItemColor colorType)
    {
        switch (colorType)
        {
            case ItemColor.White:
                return Color.white;
                break;
            case ItemColor.Red:
                return new Color(1.0f,0.2f,0.1f);
                break;
            case ItemColor.Orange:
                return new Color(1.0f,0.4f,0.0f);
                break;
            case ItemColor.Yellow:
                return new Color(1.0f,0.8f,0.0f);
                break;
            case ItemColor.Green:
                return new Color(0.1f,0.6f,0.0f);
                break;
            case ItemColor.Blue:
                return new Color(0.4f,0.6f,1.0f);
                break;
            case ItemColor.Cyan:
                return new Color(0.3f,0.9f,0.8f);
                break;
            case ItemColor.Purple:
                return new Color(0.6f,0.0f,0.8f);
                break;
            default:
                return Color.white;
        }
    }
}





