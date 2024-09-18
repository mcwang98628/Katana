using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private string AllItemPath = "Assets/AssetsPackage/ScriptObject_Item/AllItem.asset";
    private string ItemCSVPath = "Assets/AssetsPackage/Table/CSV_Items.csv";
    private string ItemSchoolCSVPath = "Assets/AssetsPackage/Table/CSV_ItemSchool.csv";
    private string UnLockItemDataPath = "Assets/AssetsPackage/Table/CSV_UnLockItem.csv";

    private string ItemPoolPath = "Assets/AssetsPackage/ScriptObject_Item/ItemPool.asset";
    // private string ChapterItemUnLockDataPath = "Assets/AssetsPackage/Table/CSV_ChapterItemUnLock.csv";
    
    private Dictionary<int, Item> ItemDatas = new Dictionary<int, Item>();
    //Key：这个配置文件的唯一ID 。  Value:配置文件
    private Dictionary<ItemScriptableObject, int> itemScrObj_ValueKey = new Dictionary<ItemScriptableObject, int>();
    //Key：这个配置文件的唯一ID 。  Value:配置文件
    private Dictionary<int, ItemScriptableObject> itemScrObj_KeyValue = new Dictionary<int, ItemScriptableObject>();
    public Dictionary<int, ItemScriptableObject> AllItemObj => itemScrObj_KeyValue;
    Dictionary<int,ItemSchoolData> itemSchool = new Dictionary<int, ItemSchoolData>();
    public Dictionary<int, ItemSchoolData> ItemSchool => itemSchool;
    private AllItemObj allItem;
    public ItemPool ItemPool => _itemPool;
    private ItemPool _itemPool;
    // public List<ChapterItemUnLockData> ChapterItemUnLocks { get; } = new List<ChapterItemUnLockData>();
    public void LoadItemPool()
    {
        ResourcesManager.Inst.GetAsset<ItemPool>(ItemPoolPath, delegate(ItemPool pool)
        {
            _itemPool = pool;
        });
    }
    public void PeekDefaultUnLockItems(Action<List<int>> callback)
    {
        ResourcesManager.Inst.GetAsset<TextAsset>(UnLockItemDataPath, delegate(TextAsset asset)
        {
            List<int> items = new List<int>();
            var itemCsv = Document.Load(asset.text);
            for (int i = 0; i < itemCsv.Count; i++)
            {
                if (itemCsv[i].Count<1 || string.IsNullOrEmpty(itemCsv[i][0]))
                {
                    continue;
                }

                try
                {
                    items.Add(itemCsv[i][0].Convert<int>());
                }
                catch (Exception e)
                {
                    Debug.LogError(itemCsv[i][0]);
                }
            }
            callback?.Invoke(items);
        });
    }


    void InitItemInfo()
    {
        ResourcesManager.Inst.GetAsset<AllItemObj>(AllItemPath, delegate(AllItemObj obj)
        {
            allItem = obj;
            initNumber++;
            
            ResourcesManager.Inst.GetAsset<TextAsset>(ItemCSVPath, delegate(TextAsset infoText)
            {
                var itemCsv = Document.Load(infoText.text);
                for (int i = 1; i < itemCsv.Count; i++)
                {
                    if (itemCsv[i].Count<2)
                    {
                        continue;
                    }
                    var id = itemCsv[i][0].Convert<int>();
                    var itemSObj = getAllitemObjItem(itemCsv[i][1]);
                    if (itemSObj == null)
                    {
                        Debug.LogError($"Err:{itemCsv[i][1]}不存在");
                    }

                    try
                    {
                        itemScrObj_ValueKey.Add(itemSObj, id);
                        itemScrObj_KeyValue.Add(id, itemSObj);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                
                initNumber--;
            });
            
            
            ResourcesManager.Inst.GetAsset<TextAsset>(ItemSchoolCSVPath, delegate(TextAsset itemSchoolText)
            {
                var itemSchoolCsv = Document.Load(itemSchoolText.text);
                for (int i = 1; i < itemSchoolCsv.Count; i++)
                {
                    if (itemSchoolCsv[i].Count<2)
                    {
                        continue;
                    }

                    int id = itemSchoolCsv[i][0].Convert<int>();
                    List<int> itemIds =new List<int>();
                    foreach (string str in itemSchoolCsv[i][1].Convert<string>().Split('-'))
                    {
                        int itemId = -1;
                        if (int.TryParse(str, out itemId))
                        {
                            itemIds.Add(itemId);
                        }
                    }
                    ItemSchool.Add(id,new ItemSchoolData()
                        {
                            Id = id,
                            ItemList = itemIds,
                            Name = itemSchoolCsv[i][2],
                            Desc = itemSchoolCsv[i][3],
                            Icon = itemSchoolCsv[i][4],
                            MinItemCount = itemSchoolCsv[i][5].Convert<int>()
                        }
                    );
                }
            });
        });
        
    }

    ItemScriptableObject getAllitemObjItem(string itemFileName)
    {
        for (int i = 0; i < allItem.objs.Count; i++)
        {
            if (allItem.objs[i].name == itemFileName)
            {
                return allItem.objs[i];
            }
        }
        Debug.LogError($"Err:Item表和Item文件不匹配  {itemFileName}");
        return null;
    }

    public Item GetItem(int id)
    {
        if (ItemDatas.ContainsKey(id))
        {
            return ItemDatas[id];
        }

        return null;
    }

    public List<int> GetAllItemIds()
    {
        List<int> itemIds = new List<int>();
        foreach (var itemObj in itemScrObj_ValueKey)
        {
            itemIds.Add(itemObj.Value);
        }
        return itemIds;
    }
    public int GetItemId(ItemScriptableObject item)
    {
        return itemScrObj_ValueKey[item];
    }
    public ItemScriptableObject GetItemScrObj(int id)
    {
        if (itemScrObj_KeyValue.ContainsKey(id))
        {
            return itemScrObj_KeyValue[id];
        }
        Debug.LogError($"没找到对应Item配置文件，ItemId：{id}");
        return null;
    }
    //解析Item
    public Item ParsingItemObj(ItemScriptableObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("Null");
            return null;
        }
        Item item = new Item();
        item.ID = itemScrObj_ValueKey[obj];
        item.isUnique = obj.isUnique;
        item.Name = obj.Name;
        item.Desc = obj.Describe;
        item.Icon = obj.Icon;
        item.itemColorType = obj.itemColorType;
        item.ItemType = obj.ItemType;
        item.ArtifactType = obj.ArtifactType;
        // item.Id = guid;
        item.VisualObjSlot = obj.VisualObjSlot;
        item.VisualObj = obj.VisualObj;


        if (obj.ItemType == ItemType.Prop ||
            obj.ItemType == ItemType.ButtonSkill)
        {
            item.SetActiveData(obj.CanUseTimes, obj.CoolingTime);
            item.ItemUseType = obj.ItemUseType;
        }
        List<ItemEffectObj> effects = new List<ItemEffectObj>();
        if (obj.effects.Count > 0)
        {
            foreach (ItemEffectObj objEffect in obj.effects)
            {
                if (objEffect == null)
                {
                    continue;
                }
                effects.Add(objEffect);
            }
        }

        for (int i = 0; i < effects.Count; i++)
        {
            ItemEffectObj ieo = effects[i];
            ItemGroup itemGroup = ParsingEffectObj(ieo);
            itemGroup.item = item;
            item.ItemGroups.Add(itemGroup);
        }

        if (!ItemDatas.ContainsKey(item.ID))
        {
            ItemDatas.Add(item.ID, item);
        }
        return item;
    }

    public ItemGroup ParsingEffectObj(ItemEffectObj effectObj)
    {
        ItemGroup itemGroup = new ItemGroup();
        itemGroup.Name = effectObj.Name;
        itemGroup.Desc = effectObj.Desc;
        switch (effectObj.triggerData.TriggerEventType)
        {
            case TriggerEventType.Active:
                itemGroup.effectTrigger = new ActiveTrigger(effectObj.triggerData.TriggerType);
                break;
            case TriggerEventType.Get:
                itemGroup.effectTrigger = new ItemGetTrigger(effectObj.triggerData.TriggerType);
                break;
            case TriggerEventType.Attack:
                switch (effectObj.triggerData.TriggerType)
                {
                    case TriggerType.Probability:
                        itemGroup.effectTrigger = new AttackTrigger(effectObj.triggerData.TriggerType, effectObj.triggerData.Probability);
                        break;
                    case TriggerType.Times:
                        itemGroup.effectTrigger = new AttackTrigger(effectObj.triggerData.TriggerType, effectObj.triggerData.AttackTimes,effectObj.triggerData.AttackMinOffsetTimes,effectObj.triggerData.AttackMaxOffsetTimes);
                        break;
                }
                break;
            case TriggerEventType.Injured:
                switch (effectObj.triggerData.TriggerType)
                {
                    case TriggerType.Probability:
                        itemGroup.effectTrigger = new InjuredTrigger(effectObj.triggerData.TriggerType, effectObj.triggerData.Probability);
                        break;
                    case TriggerType.Times:
                        itemGroup.effectTrigger = new InjuredTrigger(effectObj.triggerData.TriggerType, effectObj.triggerData.InjuredTimes);
                        break;
                }
                break;
            case TriggerEventType.KillEnemy:
                switch (effectObj.triggerData.TriggerType)
                {
                    case TriggerType.Probability:
                        itemGroup.effectTrigger = new KillEnemyTrigger(effectObj.triggerData.TriggerType, effectObj.triggerData.Probability);
                        break;
                    case TriggerType.Times:
                        itemGroup.effectTrigger = new KillEnemyTrigger(effectObj.triggerData.TriggerType, effectObj.triggerData.KillEnemyCount);
                        break;
                }
                break;
            case TriggerEventType.Time:
                itemGroup.effectTrigger = new TimerTrigger(effectObj.triggerData.TriggerType, effectObj.triggerData.IntervalTime);
                break;
            case TriggerEventType.HitEnmey:
                switch (effectObj.triggerData.TriggerType)
                {
                    case TriggerType.Probability:
                        itemGroup.effectTrigger = new AttackHitEnemyTrigger(effectObj.triggerData.TriggerType, effectObj.triggerData.Probability);
                        break;
                    case TriggerType.Times:
                        itemGroup.effectTrigger = new AttackHitEnemyTrigger(effectObj.triggerData.TriggerType, 1);
                        break;
                }
                break;
            case TriggerEventType.NewRoom:
                itemGroup.effectTrigger = new NewRoomTrigger(effectObj.triggerData.TriggerType);
                //TODO 
                break;
            case TriggerEventType.Dodge:
                itemGroup.effectTrigger = new DodgeTrigger(effectObj.triggerData.TriggerType);
                break;
            case TriggerEventType.EnterFightRoom:
                itemGroup.effectTrigger = new EnterFightRoomTrigger(effectObj.triggerData.TriggerType);
                //TODO 
                break;
            case TriggerEventType.RoleMove:
                itemGroup.effectTrigger = new RoleMoveTrigger(effectObj.triggerData.TriggerType, effectObj.triggerData.MoveIntervalTime);
                break;
            case TriggerEventType.RoleRoll:
                itemGroup.effectTrigger = new RollTrigger(effectObj.triggerData.TriggerType, effectObj.triggerData.RollIntervalTime);
                break;
            case TriggerEventType.PlayerShieldHoldingTrigger:
                itemGroup.effectTrigger = new PlayerShieldHoldingTrigger(effectObj.triggerData.TriggerType, effectObj.triggerData.PlayerShieldHoldingTrigger);
                break;
            case TriggerEventType.PlayerAttackCrit:
                itemGroup.effectTrigger = new AttackCritTrigger(effectObj.triggerData.TriggerType);
                break;
            case TriggerEventType.LightningChainHitRole:
                itemGroup.effectTrigger = new LightningChainHitRoleTrigger(effectObj.triggerData.TriggerType);
                break;
            case TriggerEventType.RoleDeadOfBuff:
                itemGroup.effectTrigger = new RoleDeadOfBuffTrigger(effectObj.triggerData.TriggerType,effectObj.triggerData.RoleDeadBuffId);
                break;
            case TriggerEventType.RoleDeadTrigger:
                itemGroup.effectTrigger = new RoleDeadTrigger(effectObj.triggerData.TriggerType);
                break;
            case TriggerEventType.DmgBuffOnTouchHitEnemyTrigger:
                itemGroup.effectTrigger = new DmgBuffOnTouchHitEnemyTrigger(effectObj.triggerData.TriggerType,effectObj.triggerData.Probability);
                break;
            case TriggerEventType.ReviveTrigger:
                itemGroup.effectTrigger = new ReviveTrigger(effectObj.triggerData.TriggerType);
                break;
            case TriggerEventType.Keyboard:
                itemGroup.effectTrigger = new KeyboardTrigger(effectObj.triggerData.TriggerType,effectObj.triggerData.KeyStr);
                break;
            case TriggerEventType.BattleObjectDestroy:
                itemGroup.effectTrigger = new BattleObjectDestroy(effectObj.triggerData.TriggerType);
                break;
            case TriggerEventType.RollAttack:
                itemGroup.effectTrigger = new RollAttackTrigger(effectObj.triggerData.TriggerType);
                break;
            case TriggerEventType.OnCreateEnemy:
                itemGroup.effectTrigger = new CreateEnemyTrigger(effectObj.triggerData.TriggerType);
                break;
            case TriggerEventType.KillRewardEnemyDead:
                itemGroup.effectTrigger = new KillRewardEnemyDeadTrigger(effectObj.triggerData.TriggerType);
                break;
            case TriggerEventType.AnimatorEventTrigger:
                itemGroup.effectTrigger = new AnimatorEventTrigger(effectObj.triggerData.TriggerType,effectObj.triggerData.EventName);
                break;
            case TriggerEventType.RollHitEnemy:
                itemGroup.effectTrigger = new RollHitEnemyTrigger(effectObj.triggerData.TriggerType);
                break;
        }

        switch (effectObj.effectData.EffectType)
        {
            case EffectType.None:
                Debug.LogError("没配置效果！！！！！！  Item:" + effectObj.Name);
                //NULL
                break;
            case EffectType.ChangeAttackPower:
                itemGroup.itemEffect = new SetAttackPowerEffect(effectObj.effectData.AttackPower,effectObj.effectData.AttackPowerPercentage);
                break;
            case EffectType.ChangeAttackSpeed:
                itemGroup.itemEffect = new SetAttackSpeedEffect(effectObj.effectData.AttackSpeed);
                break;
            case EffectType.ChangeCriticalProbability:
                itemGroup.itemEffect = new SetCriticalProbabilityEffect(effectObj.effectData.CriticalProbability);
                break;
            case EffectType.ChangeCriticalMultiplier:
                itemGroup.itemEffect = new SetCriticalMultiplierEffect(effectObj.effectData.CriticalMultiplier);
                break;
            case EffectType.ChangeInjuryMultiplier:
                itemGroup.itemEffect = new SetInjuryMultiplierEffect(effectObj.effectData.InjuryMultiplier);
                break;
            case EffectType.ChangeMaxHp:
                itemGroup.itemEffect = new SetMaxHpEffect(effectObj.effectData.MaxHp,effectObj.effectData.MaxHpPercentage);
                break;
            case EffectType.ChangeHpTreatMultiplier:
                itemGroup.itemEffect = new SetTreatParaEffect(effectObj.effectData.TreatMultiplier);
                break;
            case EffectType.ChangeCurrentHp:
                itemGroup.itemEffect = new SetHpEffect(effectObj.effectData.CurrentHp,effectObj.effectData.CurrentHpPercentage, effectObj.effectData.ChangeCurrentHpParticle);
                break;
            case EffectType.ChangeMoveSpeed:
                if (effectObj.effectData.SetAcceleration)
                {
                    itemGroup.itemEffect = new SetMoveSpeedEffect(effectObj.effectData.MoveSpeed, effectObj.effectData.isUseAccel, effectObj.effectData.startAccel, effectObj.effectData.stopAccel);
                }
                else
                {
                    itemGroup.itemEffect = new SetMoveSpeedEffect(effectObj.effectData.MoveSpeed);
                }
                break;
            case EffectType.ChangeEnemySpeed:
                itemGroup.itemEffect = new SetEnemySpeedEffect(effectObj.effectData.EnemySpeed);
                break;
            
            // case EffectType.AOEAttack:
            //     itemGroup.itemEffect = new AOEAttackEffect(
            //          effectObj.effectData.AOECenterType,
            //         effectObj.effectData.AOEAttackDistance,
            //         effectObj.effectData.AOEAttackPower,
            //          effectObj.effectData.AOEAttackPowerPercentage,
            //         effectObj.effectData.AOEIsUseMove,
            //         effectObj.effectData.AOEMoveSpeed,
            //         effectObj.effectData.AOEMoveTime,
            //         effectObj.effectData.AOEParticle,
            //         effectObj.effectData.AOEParticleOffset,
            //         effectObj.effectData.AOEBuffObj,
            //         effectObj.effectData.AoeBuffLifecycle
            //         );
            //     break;
            // case EffectType.SplashDamage:
            //     itemGroup.itemEffect = new SetSplashDamageEffect(effectObj.effectData.SplashDamage);
            //     break;
            case EffectType.Buff:
                // effectObj.effectData.BurnBuffObj.LifeCycle = effectObj.effectData.BuffLifeCycle;
                itemGroup.itemEffect = new ItemBuffEffect(effectObj.effectData.BurnBuffObj,effectObj.effectData.BuffLifeCycle);
                break;
            case EffectType.LightningChain:
                itemGroup.itemEffect = new LightningChainEffect(
                    effectObj.effectData.LightningChainTag,
                    effectObj.effectData.CatapultTimes, 
                    effectObj.effectData.CatapultDistance, 
                    effectObj.effectData.CatapultAttackPower, 
                    effectObj.effectData.CatapultAttackPowerPercentage,
                    effectObj.effectData.CatapultAttackPowerPercentage2,
                    effectObj.effectData.LightningChainColor,
                    effectObj.effectData.LightningChainHitFx
                    );
                break;
            case EffectType.Surroud:
                itemGroup.itemEffect = new SurroundEffect(
                    effectObj.effectData.SurroudObj,
                    effectObj.effectData.SurroudObjAttackPower,
                    effectObj.effectData.SurroudObjAttackPowerPercentage);
                break;
            case EffectType.SurroudSpeed:
                itemGroup.itemEffect = new AddSurroundSpeedEffect(effectObj.effectData.SurroudSpeedValue);
                break;
            // case EffectType.SurroudSplit:
            //     itemGroup.itemEffect = new SurroundObjectSplitEffect();
            //     break;
            // case EffectType.SurroudObjConversion:
            //     itemGroup.itemEffect = new SurroundObjectConversionEffect(effectObj.effectData.ConversionSurroudObjs,effectObj.effectData.ConversionSurroudObjAttackPower,effectObj.effectData.ConversionSurroudObjAttackPowerPercentage);
            //     break;
            case EffectType.FootPath:
                itemGroup.itemEffect = new FootPathEffect(effectObj.effectData.buffOnTouch,effectObj.effectData.FootPathAttackPower,effectObj.effectData.FootPathAttackPowerPercentage);
                break;
            case EffectType.Spawn:
                itemGroup.itemEffect = new SpawnEffect(effectObj.effectData.SpawnObj, effectObj.effectData.SpwanPosType, effectObj.effectData.Offset, effectObj.effectData.Direction,effectObj.effectData.IsFollowRole,effectObj.effectData.EnemyFollow);
                break;
            case EffectType.GoldEffect:
                itemGroup.itemEffect = new GoldEffect(effectObj.effectData.GoldValue);
                break;
            case EffectType.AnimatorTrigger:
                itemGroup.itemEffect = new AnimatorTriggerEffect(effectObj.effectData.AnimatorParaName, effectObj.effectData.AnimatorParaTime);
                break;
            case EffectType.SkillCoolingScale:
                itemGroup.itemEffect = new SkillCoolingScaleEffect(effectObj.effectData.SkillCoolingScale);
                break;
            // case EffectType.Phoenix:
            //     itemGroup.itemEffect = new PhoenixEffect(effectObj.effectData.PhoenixPrefab, effectObj.effectData.PhoenixEmmitVFX, effectObj.effectData.PhoenixAttackPower, effectObj.effectData.PhoenixAttackPowerPercentage);
            //     break;
            case EffectType.EmmitThunder:
                itemGroup.itemEffect = new EmmitThunderEffect(effectObj.effectData.ThunderPrefab,effectObj.effectData.ThunderAttackPower,effectObj.effectData.ThunderAttackPowerPercentage, effectObj.effectData.MaxThunderCount, effectObj.effectData.MinThunderCount);
                break;
            case EffectType.Revive:
                itemGroup.itemEffect = new ReviveEffect(effectObj.effectData.ReviveHp);
                break;
            case EffectType.ModifyLuck:
                itemGroup.itemEffect = new SetLuckEffect(effectObj.effectData.ModifyLuck);
                break;
            // case EffectType.ModifyDifficult:
            //     itemGroup.itemEffect = new SetDifficultEffect(effectObj.effectData.ModifyDifficult);
            //     break;
            // case EffectType.AutoAttack:
            //     itemGroup.itemEffect = new AutoAttackEffect(true);
            //     break;
            // case EffectType.Lottery:
            //     itemGroup.itemEffect = new LotteryEffect(effectObj.effectData.AwardGolds);
            //     break;
            case EffectType.AttackDistance:
                itemGroup.itemEffect = new SetAttackDistanceEffect(effectObj.effectData.AttackDistance);
                break;
            case EffectType.MaxPower:
                itemGroup.itemEffect = new SetMaxPowerEffect(effectObj.effectData.MaxPower);
                break;
            // case EffectType.AttackPowerRecovery:
            //     itemGroup.itemEffect = new SetAttackPowerRecoveryEffect(effectObj.effectData.AttackPowerRecovery);
            //     break;
            case EffectType.PowerRecovery:
                itemGroup.itemEffect = new SetPowerRecoveryEffect(effectObj.effectData.PowerRecovery);
                break;
            case EffectType.Bomb:
                itemGroup.itemEffect = new BombEffect(
                    effectObj.effectData.BombCount
                    ,effectObj.effectData.BombExplosion
                    ,effectObj.effectData.BombThrowProjectile
                    ,effectObj.effectData.BombMoveTime
                    ,effectObj.effectData.BombHeight
                    ,effectObj.effectData.BombPosOffset
                    ,effectObj.effectData.BombSpwanPosType
                    ,effectObj.effectData.ThrowBombAudio);
                break;
            case EffectType.TrackingArrow:
                itemGroup.itemEffect = new TrackingArrowEffect(effectObj.effectData.ArrowAudio,effectObj.effectData.Arrow);
                break;
            case EffectType.ArrowLightningChain:
                itemGroup.itemEffect = new ArrowLightningChain();
                break;
            case EffectType.ArrowAOE:
                itemGroup.itemEffect = new ArrowAOE();
                break;
            case EffectType.KatanaOnlySkill:
                itemGroup.itemEffect = new KatanaOnlySkill_YiShan_Effect(effectObj.effectData.PrepareFeedback,effectObj.effectData.SaHitFeedback,effectObj.effectData.SaTrailMat,effectObj.effectData.EndParticles,effectObj.effectData.EndBloodBurst,effectObj.effectData.EndSFX,effectObj.effectData.EndBloodSFX);
                break;
            case EffectType.PlayerAttackType:
                itemGroup.itemEffect = new PlayerAttackTypeEffect(effectObj.effectData.PlayerAttackType);
                break;
            case EffectType.PlayerAccumulateAttackType:
                itemGroup.itemEffect = new PlayerAccumulateAttackTypeEffect(effectObj.effectData.PlayerAccumulateAttackType);
                break;
            case EffectType.PlayerRollType:
                itemGroup.itemEffect = new PlayerRollTypeEffect(effectObj.effectData.PlayerRollType);
                break;
            case EffectType.ShieldAgainstBullet:
                itemGroup.itemEffect = new ShieldAgainstBullet();
                break;
            case EffectType.PowerShieldAgainst:
                itemGroup.itemEffect = new PowerShieldAgainst(effectObj.effectData.PowerShieldAgainstAOE,effectObj.effectData.PowerShieldAgainstAOEOffset);
                break;
            case EffectType.ShieldAgainst:
                itemGroup.itemEffect = new ShieldAgainst(effectObj.effectData.ShieldAgainstFeedBack,effectObj.effectData.ShieldDistance);
                break;
            case EffectType.SmallShieldAgainst:
                itemGroup.itemEffect = new SmallShieldAgainst(effectObj.effectData.SmallShieldAgainstValue);
                break;
            case EffectType.ShieldAttributes:
                itemGroup.itemEffect = new ShieldAttributes(effectObj.effectData.ShieldAttributesAng,effectObj.effectData.ShieldAttributesMoveSpeed,effectObj.effectData.ShieldMesh1,effectObj.effectData.ShieldMesh2);
                break;
            case EffectType.AccumulateTime:
                itemGroup.itemEffect = new AccumulateTimeEffect(effectObj.effectData.AccumulateTime);
                break;
            case EffectType.ShieldSkill_Against:
                itemGroup.itemEffect = new ShieldSkill_Against(effectObj.effectData.ShieldSkill_Against_Times,effectObj.effectData.ShieldSkill_Against_Interval);
                break;
            case EffectType.ShieldRollLastAttack:
                itemGroup.itemEffect = new ShieldRollAttack();
                break;
            
            case EffectType.ShieldDash_SkipPreAction:
                itemGroup.itemEffect = new ShieldDash_SkipPreAction();
                break;
            case EffectType.ShieldDash_AfterAttack:
                itemGroup.itemEffect = new ShieldDash_AfterAttack();
                break;
            case EffectType.ShieldDash_God:
                itemGroup.itemEffect = new ShieldDash_God();
                break;
            case EffectType.ThornsArmor:
                itemGroup.itemEffect = new ThornsArmor(effectObj.effectData.ThornsMagnification);
                break;
            case EffectType.Backstab:
                itemGroup.itemEffect = new Backstab(effectObj.effectData.BackstabMagnification);
                break;
            case EffectType.GetMoneyAddHp:
                itemGroup.itemEffect = new GetMoneyAddHp(effectObj.effectData.MoneyAddHpValue,effectObj.effectData.MoneySubHpValue);
                break;
            case EffectType.LessItemCooling:
                itemGroup.itemEffect = new LessItemCooling(effectObj.effectData.LessItemCoolingTime);
                break;
            case EffectType.Dizziness:
                itemGroup.itemEffect = new DizzinessEffect(effectObj.effectData.DizzinessTime);
                break;
            case EffectType.ItemCountAddAttribute:
                itemGroup.itemEffect = new ItemCountAddAttribute(effectObj.effectData.ItemCountAddAttackPowerValue,effectObj.effectData.ItemCountAddAttributeType);
                break;
            case EffectType.MoneyAddAttribute:
                itemGroup.itemEffect = new MoneyAddAttackAttribute(effectObj.effectData.MoneyAddAttackPowerValue,effectObj.effectData.MaxAddAttackPowerValue,effectObj.effectData.MoneyAddAttackAttributeType);
                break;
            case EffectType.HpProportionAttribute:
                itemGroup.itemEffect = new HpProportionAttribute(effectObj.effectData.HpProportionValue,effectObj.effectData.HpProportionAttributeType);
                break;
            case EffectType.AttackPowerProportionHp:
                itemGroup.itemEffect = new AttackPowerProportionHp(effectObj.effectData.dmgProportionHpValue);
                break;
            case EffectType.NotAcceptInterruption:
                itemGroup.itemEffect = new NotAcceptInterruption();
                break;
            case EffectType.CrazyAttribute:
                itemGroup.itemEffect = new CrazyAttribute(effectObj.effectData.CrazyAttributeValue,effectObj.effectData.CrazyAttributeType);
                break;
            case EffectType.AccumulationObject:
                itemGroup.itemEffect = new AccumulationObject(effectObj.effectData.SecondAccumulationObject,effectObj.effectData.AccumulationObjectMaxCount,effectObj.effectData.AccumulationObject);
                break;
            case EffectType.TreatmentGetMoney:
                itemGroup.itemEffect = new TreatmentGetMoney(effectObj.effectData.TreatmentGetMoneyProportion);
                break;
            case EffectType.ReduceMonsterHP:
                itemGroup.itemEffect = new ReduceMonsterHP(
                    effectObj.effectData.ReduceMonsterHPStrengthenTag,
                    effectObj.effectData.ReduceMonsterHPPercentage,
                    effectObj.effectData.ReduceMonsterHPPercentage2,
                    effectObj.effectData.ReduceMonsterHPFx);
                break;
            case EffectType.DiceAttribute:
                itemGroup.itemEffect = new DiceAttribute(effectObj.effectData.DiceAttributeList);
                break;
            case EffectType.TargetMaxHpCritical:
                itemGroup.itemEffect = new TargetMaxHpCritical();
                break;
            case EffectType.GoldMagnification:
                itemGroup.itemEffect = new GoldMagnification(effectObj.effectData.GoldMagnification);
                break;
            case EffectType.DmgAoeCrit:
                itemGroup.itemEffect = new AOECritEffect();
                break;
            case EffectType.TriggerHitEnemy:
                itemGroup.itemEffect = new TriggerHitEnemy();
                break;
            case EffectType.SetCantHoldingShield:
                itemGroup.itemEffect = new SetCantHoldingShield();
                break;
            case EffectType.Audio:
                itemGroup.itemEffect = new AudioEffect(effectObj.effectData.AudioClip,effectObj.effectData.AudioVolume,effectObj.effectData.AudioDelay);
                break;
            case EffectType.ShieldSkill:
                itemGroup.itemEffect = new ShieldSkillEffect(effectObj.effectData.ShieldSkillDuration,effectObj.effectData.ShieldSkillAng,effectObj.effectData.ShieldSkillMoveSpeed);
                break;
            case EffectType.MagicPower:
                itemGroup.itemEffect = new MagicPowerEffect(effectObj.effectData.MagicPower);
                break;
            case EffectType.NewKatanaSkill:
                itemGroup.itemEffect = new NewKatanaSkillEffect(
                    effectObj.effectData.KatanaSkillKillRecoveryPower,
                    effectObj.effectData.KatanaSkillAttackDis,
                    effectObj.effectData.KatanaSkillAttackAng,
                    effectObj.effectData.KatanaSkillAttackPowerPercentage,
                    effectObj.effectData.KatanaSkillAttackWaitTime,
                    effectObj.effectData.KatanaSkillFx,
                    effectObj.effectData.KatanaSkillAudio
                    );
                break;
            case EffectType.DoubleAttack:
                itemGroup.itemEffect = new DoubleAttack();
                break;
            case EffectType.TagEffect:
                itemGroup.itemEffect = new TagEffect(effectObj.effectData.TagStr);
                break;
            case EffectType.Scattering:
                itemGroup.itemEffect = new ScatteringEffect(
                    effectObj.effectData.ScatteringObject,
                    effectObj.effectData.ScatteringObjectCount,
                    effectObj.effectData.ScatteringOffset);
                break;
            case EffectType.BattleObjectLightning:
                itemGroup.itemEffect = new BattleObjectLightning();
                break;
            case EffectType.KnifeWing:
                itemGroup.itemEffect = new KnifeWing(
                    effectObj.effectData.KnifeWingObject,
                    effectObj.effectData.KnifeWingCount,
                    effectObj.effectData.KnifeWingOffset
                    );
                break;
            case EffectType.AddItem:
                itemGroup.itemEffect = new AddItemEffect(effectObj.effectData.ItemObj);
                break;
            case EffectType.AddDice:
                itemGroup.itemEffect = new AddDiceEffect(effectObj.effectData.DiceCount);
                break;
            case EffectType.AddGold:
                itemGroup.itemEffect = new AddGold(effectObj.effectData.GoldRatio);
                break;
            case EffectType.ShowEnemyHpText:
                itemGroup.itemEffect = new ShowEnemyHpTextEffect();
                break;
            case EffectType.AttributeEffect:
                itemGroup.itemEffect = new AttributeEffect(
                    effectObj.effectData.AttributeEffectValue,
                    effectObj.effectData.AttributeEffectType);
                break;
            
            case EffectType.FullHpEffect:
                itemGroup.itemEffect = new FullHpEffect(
                    effectObj.effectData.spawnCd,
                    effectObj.effectData.spawn_FullHp,
                    effectObj.effectData.surroundObj_FullHp,
                    effectObj.effectData.offset_FullHp,
                    effectObj.effectData.direction_FullHp,
                    effectObj.effectData.isFollow_FullHp);
                break;
            case EffectType.KillRewardTagEffect:
                itemGroup.itemEffect = new KillRewardTagEffect(effectObj.effectData.KillRewardTagObject);
                break;
            case EffectType.CrownEffect:
                itemGroup.itemEffect = new CrownEffect(
                    effectObj.effectData.maxCrown,
                    effectObj.effectData.perCrownAttack,
                    effectObj.effectData.perDamageReduce,
                    effectObj.effectData.crown,
                    effectObj.effectData.startPoint,
                    effectObj.effectData.offsetY);
                break;
            case EffectType.RoleShadowEffect:
                itemGroup.itemEffect = new RoleShadowEffect(effectObj.effectData.ShadowMaterial);
                break;
            case EffectType.GoldManuscript:
                itemGroup.itemEffect = new GoldManuscript(effectObj.effectData.GoldManuscriptPrefab,effectObj.effectData.GoldManuscriptProbability);
                break;
            
            
            
            
            // case EffectType.FeverEffect:
            //     itemGroup.itemEffect = new FeverEffect(
            //         effectObj.effectData.FeverMaxPower,
            //         effectObj.effectData.FeverRecoveryPower,
            //         effectObj.effectData.FeverConsumePower);
            //     
            //     break;
        }

        return itemGroup;
    }

}
