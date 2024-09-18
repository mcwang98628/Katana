using System.Collections.Generic;
using UnityEngine;

public class CrownEffect : ItemEffect
{
    private int maxCrown;
    private float attack;
    private int preDamgeReduce;
    private GameObject effect;
    private Vector3 startPoint;
    private float offsetY;


    private Dictionary<int, GameObject> crownObjs;
    private int currentCrown;
    private AttributeBonus attb;

    public CrownEffect(int maxCrown, float attack, int pre, GameObject e, Vector3 sP, float o)
    {
        this.maxCrown = maxCrown;
        this.attack = attack;
        preDamgeReduce = pre;
        effect = e;
        startPoint = sP;
        offsetY = o;
        attb = new AttributeBonus {Type = AttributeType.AttackPower, Value = 0};
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (currentCrown >= maxCrown) return;
        if (effect) CreateCorwn(currentCrown);
        currentCrown++;
        float ratio = currentCrown * attack;
        attb.Value = (int)(roleController.OriginalAttackPower*ratio);
    }

    void CreateCorwn(int index)
    {
        if (crownObjs.ContainsKey(index))
        {
            crownObjs[index].SetActive(true);
        }
        else
        {
            var crown = Object.Instantiate(effect);
            var crownOffset = startPoint;
            crownOffset.y += offsetY * index;
            crown.transform.position = crownOffset;
            crownObjs.Add(index, crown);
        }
    }

    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        var tempPos = BattleManager.Inst.CurrentPlayer.transform.position;
        foreach (var t in crownObjs.Values)
        {
            var position = t.transform.position;
            position = new Vector3(startPoint.x + tempPos.x, position.y, startPoint.z + tempPos.z);
            t.transform.position = position;
        }
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        roleController.AddAttributeBonus(attb);
        EventManager.Inst.AddEvent(EventName.OnRoleInjured, OnPlayerInjured);
        crownObjs = new Dictionary<int, GameObject>();
    }

    private void OnPlayerInjured(string arg1, object arg2)
    {
        RoleInjuredInfo data = (RoleInjuredInfo) arg2;
        if (data.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }

        currentCrown -= preDamgeReduce;
        if (currentCrown < 0)
        {
            currentCrown = 0;
        }

        float ratio = currentCrown * attack;
        attb.Value = (int)(roleController.OriginalAttackPower*ratio);
        
        for (int i = 0; i < crownObjs.Count; i++)
        {
            if (i < currentCrown)
            {
                continue;
            }

            crownObjs[i].SetActive(false);
        }
    }

    void DestroyCrown()
    {
        for (int i = crownObjs.Count - 1; i >= 0; i--)
        {
            Object.Destroy(crownObjs[i]);
        }

        crownObjs.Clear();
    }

    public override void Destroy(RoleItemController rpe)
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured, OnPlayerInjured);
        roleController.RemoveAttributeBonus(attb);
        DestroyCrown();
        base.Destroy(rpe);
    }
}