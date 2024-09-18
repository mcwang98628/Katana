using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.ThornsArmor)]
    [LabelText("反弹伤害比例 - 受伤的伤害比例")]
    public float ThornsMagnification;
}
//荆棘之甲 - 无需触发条件
public class ThornsArmor : ItemEffect
{
    private float Magnification;
    public ThornsArmor(float magnification)
    {
        Magnification = magnification;
    }
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleInjured, OnPlayerInjured);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured, OnPlayerInjured);
    }

    private void OnPlayerInjured(string arg1, object arg2)
    {
        var data = (RoleInjuredInfo)arg2;
        if (data.RoleId != roleController.TemporaryId || data.Dmg.AttackerRole.TemporaryId == roleController.TemporaryId)
        {
            return;
        }
        DamageInfo dmgInfo = new DamageInfo(data.Dmg.AttackerRole.TemporaryId, data.Dmg.DmgValue * Magnification, roleController, roleController.transform.position, DmgType.Other);
        data.Dmg.AttackerRole.HpInjured(dmgInfo);
    }
}

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.Backstab)]
    [LabelText("背刺造成的额外伤害 - 玩家攻击力比例")]
    public float BackstabMagnification;
}
//被刺 - 击中敌人Trigger
public class Backstab : ItemEffect
{
    //攻击力倍数
    private float Magnification;
    public Backstab(float magnification)
    {
        Magnification = magnification;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (value.HasValue && !string.IsNullOrEmpty(value.Value.TargetId))
        {
            Hit(value.Value.TargetId);
        }
    }

     void Hit(string targetId)
    {
        if (BattleManager.Inst.EnemyTeam.ContainsKey(targetId))
        {
            var enemy = BattleManager.Inst.EnemyTeam[targetId];
            var dir = roleController.transform.position - enemy.transform.position;
            if (Mathf.Abs(Vector3.Angle(enemy.Animator.transform.forward, dir)) > 90)
            {
                DamageInfo dmgInfo = new DamageInfo(enemy.TemporaryId, roleController.AttackPower * Magnification, roleController, roleController.transform.position, DmgType.Physical);
                enemy.HpInjured(dmgInfo);
            }
        }
    }
}

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.GetMoneyAddHp)]
    [LabelText("拿一块钱加多少血")]
    public float MoneyAddHpValue;
    [ShowIf("EffectType", EffectType.GetMoneyAddHp)]
    [LabelText("花一块钱减多少血")]
    public float MoneySubHpValue;
}
//拾取金钱 增加血量
public class GetMoneyAddHp : ItemEffect
{
    //多少钱 换算 1点血
    private float MoneyAddHpValue;
    private float MoneySubHpValue;
    public GetMoneyAddHp(float moneyAddHpValue, float moneySubHpValue)
    {
        MoneyAddHpValue = moneyAddHpValue;
        MoneySubHpValue = moneySubHpValue;
    }
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnAddMoney, OnAddMoney);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnAddMoney, OnAddMoney);
    }

    private void OnAddMoney(string arg1, object arg2)
    {
        int money = (int)arg2;
        float hp = money>0?money * MoneyAddHpValue:money *MoneySubHpValue;
        TreatmentData data = new TreatmentData(hp, roleController.TemporaryId);
        roleController.HpTreatment(data);
    }
}

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.LessItemCooling)]
    [LabelText("降低冷却时间 秒")]
    public float LessItemCoolingTime;
}

//减少当前所有物品冷却
public class LessItemCooling : ItemEffect
{
    private float lessTime;
    public LessItemCooling(float lessTime)
    {
        this.lessTime = lessTime;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        
        EventManager.Inst.DistributeEvent(EventName.ItemCoolingLess, lessTime);
    }

}



public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.Dizziness)]
    [LabelText("眩晕时间 秒")]
    public float DizzinessTime;
}
public class DizzinessEffect : ItemEffect
{
    private float DizzTime;
    public DizzinessEffect(float dizzTime)
    {
        DizzTime = dizzTime;
    }


    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (value.HasValue && !string.IsNullOrEmpty(value.Value.TargetId))
        {
            roleController.StartCoroutine(SetDizz(value.Value.TargetId));
        }
    }

    IEnumerator SetDizz(string targetId)
    {
        if (BattleManager.Inst.EnemyTeam.ContainsKey(targetId))
            BattleManager.Inst.EnemyTeam[targetId].SetDizziness(true);
        
        yield return new WaitForSeconds(DizzTime);
        
        if (BattleManager.Inst.EnemyTeam.ContainsKey(targetId))
            BattleManager.Inst.EnemyTeam[targetId].SetDizziness(false);
    }
}

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.AttributeEffect)]
    [LabelText("数值")]
    public float AttributeEffectValue;
    [ShowIf("EffectType", EffectType.AttributeEffect)]
    [LabelText("属性类型")]
    public AttributeType AttributeEffectType;
}
public class AttributeEffect : ItemEffect
{
    private float AddValue;
    AttributeBonus attributeBonus;
    public AttributeEffect(float value, AttributeType attributeType)
    {
        AddValue = value;
        if (AddValue <= 0)
            Debug.LogError("#Err# 物品配置错误！");
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = attributeType;
        attributeBonus.Value = 0;
    }
    
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        roleController.AddAttributeBonus(attributeBonus);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.RemoveAttributeBonus(attributeBonus);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        attributeBonus.Value += AddValue;
    }
}


public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.ItemCountAddAttribute)]
    [LabelText("每个物品增加的攻击力")]
    public float ItemCountAddAttackPowerValue;
    [ShowIf("EffectType", EffectType.ItemCountAddAttribute)]
    [LabelText("属性类型")]
    public AttributeType ItemCountAddAttributeType;
}

public class ItemCountAddAttribute : ItemEffect
{
    private float EveryItemAddValue;//每一个物品增加的攻击力
    AttributeBonus attributeBonus;
    public ItemCountAddAttribute(float value, AttributeType attributeType)
    {
        EveryItemAddValue = value;
        if (EveryItemAddValue <= 0)
        {
            Debug.LogError("#Err# 物品配置错误！");
        }
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = attributeType;
        attributeBonus.Value = 0;
    }

    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        if (attributeBonus.Type == AttributeType.AttackPower)
        {
            attributeBonus.Value = roleController.OriginalAttackPower * (roleController.roleItemController.Items.Count * EveryItemAddValue);
        }
        else if (attributeBonus.Type == AttributeType.MaxHp)
        {
            attributeBonus.Value = roleController.OriginalMaxHp * (roleController.roleItemController.Items.Count * EveryItemAddValue);
        }
        else
        {
            attributeBonus.Value = roleController.roleItemController.Items.Count * EveryItemAddValue;
        }
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        roleController.AddAttributeBonus(attributeBonus);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.RemoveAttributeBonus(attributeBonus);
    }

}


public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.MoneyAddAttribute)]
    [LabelText("多少Money增加1%攻击")]
    public float MoneyAddAttackPowerValue;
    [ShowIf("EffectType", EffectType.MoneyAddAttribute)]
    [LabelText("攻击上限%，每1为100%")]
    public float MaxAddAttackPowerValue;
    [ShowIf("EffectType", EffectType.MoneyAddAttribute)]
    [LabelText("属性类型")]
    public AttributeType MoneyAddAttackAttributeType;
}
public class MoneyAddAttackAttribute : ItemEffect
{
    private float Value;//多少钱能增加一点攻击力
    private float MaxValue;//上限值 5
    AttributeBonus attributeBonus;
    private float ratioAtt => 0.01f;
    public MoneyAddAttackAttribute(float value, float MaxAddAttackPowerValue, AttributeType attributeType)
    {
        Value = value;
        MaxValue = MaxAddAttackPowerValue;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = attributeType;
        attributeBonus.Value = 0;
    }

    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        float addValue = BattleManager.Inst.CurrentGold / Value*ratioAtt;
        addValue = Mathf.Clamp(addValue, 0, MaxValue);
        attributeBonus.Value = (int)(roleController.OriginalAttackPower*addValue);
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        roleController.AddAttributeBonus(attributeBonus);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.RemoveAttributeBonus(attributeBonus);
    }

}

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.HpProportionAttribute)]
    [LabelText("完全空血 增加多少百分比 0-1f （移速除外")]
    public float HpProportionValue;
    [ShowIf("EffectType", EffectType.HpProportionAttribute)]
    [LabelText("属性类型")]
    public AttributeType HpProportionAttributeType;

}
//血比例 属性
public class HpProportionAttribute : ItemEffect
{
    private float Value;
    AttributeBonus attributeBonus;

    public HpProportionAttribute(float value, AttributeType attributeType)
    {
        Value = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = attributeType;
        attributeBonus.Value = 0;
    }

    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        float targetValue = 0;
        switch (attributeBonus.Type)
        {
            case AttributeType.AttackPower:
                targetValue = roleController.OriginalAttackPower * Value;
                break;
            case AttributeType.AttackSpeed:
                targetValue = roleController.OriginalAttackSpeed * Value;
                break;
            case AttributeType.MaxHp:
                targetValue = roleController.OriginalMaxHp * Value;
                break;
            case AttributeType.MoveSpeed:
                targetValue = Value;
                break;
            case AttributeType.HpTreatMultiplier:
                targetValue = roleController.TreatMultiplier * Value;
                break;
            case AttributeType.InjuryMultiplier:
                targetValue = roleController.InjuryMultiplier * Value;
                break;
            case AttributeType.CriticalProbability:
                targetValue = ((PlayerAttack)roleController.roleAttack).CriticalProbability * Value;
                break;
        }
        float proportion = 1 - (roleController.CurrentHp / roleController.MaxHp);
        attributeBonus.Value = proportion * targetValue;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        roleController.AddAttributeBonus(attributeBonus);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.RemoveAttributeBonus(attributeBonus);
    }
}



public partial class ItemEffectEffectData
{
    [FormerlySerializedAs("AttackPowerProportionHpValue")]
    [ShowIf("EffectType", EffectType.AttackPowerProportionHp)]
    [LabelText("Dmg Value百分比")]
    public float dmgProportionHpValue;
}

//攻击力百分比回血 （ 吸血 ） 
public class AttackPowerProportionHp : ItemEffect
{
    private float dmgProportion;
    public AttackPowerProportionHp(float attackPowerProportion)
    {
        dmgProportion = attackPowerProportion;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (!value.HasValue || value.Value.DamageInfo == null)
        {
            return;
        }
        TreatmentData data = new TreatmentData(value.Value.DamageInfo.DmgValue * dmgProportion, roleController.TemporaryId);
        if (data.TreatmentValue<1)
            data.TreatmentValue = 1;
        roleController.HpTreatment(data);
    }

    
}

//不接受打断
public class NotAcceptInterruption : ItemEffect
{
    private int setTimes;
    
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.roleHealth.SetIsAcceptInterruption(false);
        setTimes++;
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        for (int i = 0; i < setTimes; i++)
        {
            roleController.roleHealth.SetIsAcceptInterruption(true);
        }
    }
}



public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.CrazyAttribute)]
    [LabelText("属性百分比")]
    public float CrazyAttributeValue;
    [ShowIf("EffectType", EffectType.CrazyAttribute)]
    [LabelText("属性类型")]
    public AttributeType CrazyAttributeType;
}

//疯狂属性，杀敌涨属性，受伤 or 滑动失去加成
public class CrazyAttribute : ItemEffect
{
    private float Value;
    private AttributeBonus attributeBonus;
    public CrazyAttribute(float value, AttributeType attributeType)
    {
        Value = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = attributeType;
        attributeBonus.Value = 0;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleDead, OnRoleDead);
        EventManager.Inst.AddEvent(EventName.JoyStatus, OnJoyStatus);
        EventManager.Inst.AddEvent(EventName.OnRoleInjured, OnRoleInjured);
        roleController.AddAttributeBonus(attributeBonus);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead, OnRoleDead);
        EventManager.Inst.RemoveEvent(EventName.JoyStatus, OnJoyStatus);
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured, OnRoleInjured);
        roleController.RemoveAttributeBonus(attributeBonus);
    }

    private void OnRoleInjured(string arg1, object arg2)
    {
        RoleInjuredInfo data = (RoleInjuredInfo)arg2;
        if (data.RoleId == roleController.TemporaryId)
        {
            return;
        }
        attributeBonus.Value = 0;
    }

    private void OnJoyStatus(string arg1, object arg2)
    {
        JoyStatusData data = (JoyStatusData)arg2;
        if (data.JoyStatus != UIJoyStatus.OnSlide && data.JoyStatus != UIJoyStatus.OnHoldSlide)
        {
            return;
        }
        attributeBonus.Value = 0;
    }

    private void OnRoleDead(string arg1, object arg2)
    {
        attributeBonus.Value += Value;
    }
}


public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.AccumulationObject)]
    [LabelText("几秒积累1个")]
    public float SecondAccumulationObject;
    [ShowIf("EffectType", EffectType.AccumulationObject)]
    [LabelText("积累上限")]
    public int AccumulationObjectMaxCount;
    [ShowIf("EffectType", EffectType.AccumulationObject)]
    [LabelText("释放的Object")]
    public GameObject AccumulationObject;
}
public class AccumulationObject : ItemEffect
{
    private float SecondAccumulationObject;
    private int AccumulationObjectMaxCount;
    private GameObject Object;
    public AccumulationObject(float secondAccumulationObject, int accumulationObjectMaxCount, GameObject gameobject)
    {
        SecondAccumulationObject = secondAccumulationObject;
        AccumulationObjectMaxCount = accumulationObjectMaxCount;
        Object = gameobject;
    }

    private float timer;
    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        if (!roleController.IsAttacking)
        {
            timer += Time.time;
        }
    }


    public override void TriggerEffect(ItemEffectTriggerValue? valuee)
    {
        base.TriggerEffect(valuee);
        
        int value = (int)(timer / SecondAccumulationObject);
        if (value > AccumulationObjectMaxCount)
        {
            value = AccumulationObjectMaxCount;
        }

        for (int i = 0; i < value; i++)
        {
            var obj = GameObject.Instantiate(Object);
            obj.transform.position = roleController.transform.position;
        }
        timer = 0;
    }
}



public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.TreatmentGetMoney)]
    [LabelText("多少血换1金币")]
    public float TreatmentGetMoneyProportion;
}
public class TreatmentGetMoney : ItemEffect
{
    private float TreatmentGetMoneyProportion;//多少血换1金币
    public TreatmentGetMoney(float Proportion)
    {
        this.TreatmentGetMoneyProportion = Proportion;
    }
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleTreatment, OnRoleTreatment);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnRoleTreatment, OnRoleTreatment);
    }

    private void OnRoleTreatment(string arg1, object arg2)
    {
        TreatmentData data = (TreatmentData)arg2;
        if (data.RoleId != roleController.TemporaryId)
        {
            return;
        }

        float goldValue = data.TreatmentValue / TreatmentGetMoneyProportion;
        BattleManager.Inst.AddGold((int)goldValue);
    }
}



public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.ReduceMonsterHP)]
    [LabelText("强化Tag")]
    public string ReduceMonsterHPStrengthenTag;
    [ShowIf("EffectType", EffectType.ReduceMonsterHP)]
    [LabelText("怪物出生随时血量百分比")]
    public float ReduceMonsterHPPercentage;
    [ShowIf("EffectType", EffectType.ReduceMonsterHP)]
    [LabelText("怪物出生随时血量百分比2")]
    public float ReduceMonsterHPPercentage2;
    [ShowIf("EffectType", EffectType.ReduceMonsterHP)]
    [LabelText("特效")]
    public GameObject ReduceMonsterHPFx;
}
//削减怪物血量
public class ReduceMonsterHP : ItemEffect
{
    private string StrengthenTag;
    private float Percentage;
    private float Percentage2;//强化后
    GameObject ReduceMonsterHPFx;
    public ReduceMonsterHP(string tag,float percentage,float percentage2,GameObject fx)
    {
        StrengthenTag = tag;
        Percentage = percentage;
        Percentage2 = percentage2;
        ReduceMonsterHPFx = fx;
    }
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleRegistered, OnRoleRegistered);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnRoleRegistered, OnRoleRegistered);
    }

    private void OnRoleRegistered(string arg1, object arg2)
    {
        RoleController role = (RoleController)arg2;
        if (role == roleController)
        {
            return;
        }

        int tagCount = roleController.GetTagCount(StrengthenTag);

        float value = role.MaxHp *  (tagCount <= 0 ? Percentage:Percentage2);
        DamageInfo damageInfo = new DamageInfo(role.TemporaryId, value, roleController, role.transform.position, DmgType.Other);
        role.HpInjured(damageInfo);

        var objFx = GameObject.Instantiate(ReduceMonsterHPFx);
        objFx.transform.position = role.transform.position;
        objFx.SetActive(true);
    }
}


public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.DiceAttribute)]
    [LabelText("属性列表")]
    public List<DiceAttribute.DiceAttributeData> DiceAttributeList;
}
//属性骰子
public class DiceAttribute : ItemEffect
{
    [Serializable]
    public class DiceAttributeData
    {
        public AttributeType AttributeType;
        public float Percentage;
    }

    private AttributeBonus attributeBonus;
    private List<DiceAttribute.DiceAttributeData> DiceAttributeList;
    public DiceAttribute(List<DiceAttribute.DiceAttributeData> list)
    {
        DiceAttributeList = list;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        attributeBonus = new AttributeBonus();
        roleController.AddAttributeBonus(attributeBonus);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.RemoveAttributeBonus(attributeBonus);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        var attribute = DiceAttributeList[Random.Range(0, DiceAttributeList.Count)];
        attributeBonus.Type = attribute.AttributeType;
        float attValue = 0;
        switch (attributeBonus.Type)
        {
            case AttributeType.AttackPower:
                attValue = roleController.AttackPower * attribute.Percentage;
                break;
            case AttributeType.AttackSpeed:
                attValue = roleController.AttackSpeed * attribute.Percentage;
                break;
            case AttributeType.MaxHp:
                attValue = roleController.MaxHp * attribute.Percentage;
                break;
            case AttributeType.MoveSpeed:
                attValue = roleController.MaxMoveSpeed * attribute.Percentage;
                break;
            case AttributeType.HpTreatMultiplier:
                attValue = roleController.TreatMultiplier * attribute.Percentage;
                break;
            case AttributeType.InjuryMultiplier:
                attValue = roleController.InjuryMultiplier * attribute.Percentage;
                break;
            case AttributeType.CriticalProbability:
                attValue = ((PlayerAttack)roleController.roleAttack).CriticalProbability * attribute.Percentage;
                break;
        }
        attributeBonus.Value = attValue;

    } 
}


//满血暴击
public class TargetMaxHpCritical : ItemEffect
{
    private AttributeBonus attributeBonus;
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.TargetFullHpCriticalProbability;
        roleController.AddAttributeBonus(attributeBonus);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.RemoveAttributeBonus(attributeBonus);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        attributeBonus.Value = 1;
    }
}



public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.GoldMagnification)]
    [LabelText("增加的百分比 0-1f")]
    public float GoldMagnification;
}
//获取金钱比例
public class GoldMagnification : ItemEffect
{
    private AttributeBonus attributeBonus;

    private float proportion;
    public GoldMagnification(float proportion)
    {
        proportion = proportion <= -1 ? -0.9f : proportion;//防止除0
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.AddGoldMagnification;
        attributeBonus.Value = proportion;
        this.proportion = proportion;
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        // DropManager.Inst.ChestCoinMaxCount = Mathf.CeilToInt(DropManager.Inst.ChestCoinMaxCount / (1 + proportion));
        // DropManager.Inst.EnemyDeadCoinMaxCount = Mathf.CeilToInt(DropManager.Inst.EnemyDeadCoinMaxCount / (1 + proportion));
        roleController.RemoveAttributeBonus(attributeBonus);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.AddAttributeBonus(attributeBonus);
        // DropManager.Inst.ChestCoinMaxCount = Mathf.CeilToInt(DropManager.Inst.ChestCoinMaxCount * (1 + proportion));
        // DropManager.Inst.EnemyDeadCoinMaxCount = Mathf.CeilToInt(DropManager.Inst.EnemyDeadCoinMaxCount * (1 + proportion));
    }
}

//AOE可以暴击
public class AOECritEffect : ItemEffect
{
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        BattleManager.Inst.CurrentPlayer.AOECrit = true;
    }
}

public class TriggerHitEnemy : ItemEffect
{

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (!value.HasValue || value.Value.DamageInfo == null )
        {
            return;
        }
        
        EventManager.Inst.DistributeEvent(EventName.OnPlayerAttackHitEnemy, value.Value.DamageInfo);
        EventManager.Inst.DistributeEvent(EventName.OnRoleAttack, roleController.TemporaryId);
    }
}
public class SetCantHoldingShield : ItemEffect
{ 
    // public override void TriggerEffect(ItemEffectTriggerValue? value)
    // {
    //     base.TriggerEffect(value);
    //     if (BattleManager.Inst.CurrentPlayer is PlayerShieldController shieldController)
    //     {
    //         shieldController.SetCanHoldingShield(false);
    //     }
    // }
}