using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class BaseRules : IBattleRules
{
    public bool IsLoadOver { get; protected set; }
    public abstract LevelStructType BattleRulesType { get; }
    public bool IsArchiveBattle { get; protected set; }
    public abstract RoomController CurrentRoom { get; }
    public abstract void StartGame();

    public abstract void LoadBattle(IBattleRulesData rulesData);

    public abstract void LoadBattle(IArchiveBattleData archiveBattleData);

    public abstract void EndBattle();

    public abstract void Update();

    
    protected BattleRuntimeBaseData RuntimeBaseData => BattleManager.Inst.RuntimeData;
    protected PlayerController CurrentPlayer => BattleManager.Inst.CurrentPlayer;
    protected CameraController CurrentCamera => BattleManager.Inst.CurrentCamera;
    protected HeroData CurrentHeroData => RuntimeBaseData.CurrentHeroData;
    
    //更新HeroData
    public virtual void UpdateHeroData()
    {
        
    }

    protected void CreateCamera(Action callBack)
    {
        if (CurrentCamera!=null)
        {
            GameObject.Destroy(CurrentCamera.gameObject);
        }
        BattleTool.CreatePlayerCamera(delegate(CameraController controller)
        {
            controller.SetTarget(CurrentPlayer.transform);
            //controller.Distance = 40f;
            controller.Distance = 20f;
            BattleManager.Inst.SetCamera(controller);
            callBack?.Invoke();
        });
    }
    protected void CreatePlayer(Action callback)
    {
        if (CurrentPlayer!=null)
        {
            GameObject.Destroy(CurrentPlayer.gameObject);
        }
        
        BattleTool.CreatePlayer(RuntimeBaseData.CurrentHeroData.HeroPrefabName,
            delegate(PlayerController controller)
            {
                GameObject startPoint = GameObject.Find("Entrance_Z");
                if (startPoint != null)
                {
                    controller.transform.position = startPoint.transform.position;// + new Vector3(0, 0, 0.5f);
                    controller.transform.rotation = Quaternion.identity;
                    
                }
                BattleManager.Inst.SetPlayer(controller);
                callback?.Invoke();
                
                controller.SetUniqueID(CurrentHeroData.HeroId);
                controller.GetComponent<PlayerSkill>()?.Init();
                controller.GetComponent<PlayerAppearance>()?.Init();
                
                //------------------------------------------------
                controller.Animator.transform.localPosition = new Vector3(0,15,0);
                ResourcesManager.Inst.GetAsset<GameObject>("Assets/AssetsPackage/CommonFX/VFX_PlayerLand.prefab", 
                    delegate(GameObject go)
                    {
                        controller.Animator.transform.DOLocalMove(Vector3.zero, 0.2f).OnComplete(() =>
                        {
                            var fxGo = GameObject.Instantiate(go);
                            fxGo.transform.position = controller.transform.position;
                            fxGo.gameObject.SetActive(true);
                            
                            
                            ResourcesManager.Inst.GetAsset<FeedBackObject>("Assets/AssetsPackage/ScriptObject_Combat/FeedBack/Feedback_Player_OnPlayerGround.asset", 
                                delegate(FeedBackObject data)
                                {
                                    FeedbackManager.Inst.UseFeedBack(controller,data);
                                });
                        }).SetDelay(0.3f).Delay();
                    });
                //------------------------------------------------
                
                EnvironmentManager.SetCurrentCharacterLight();
            });
        
        // GameManager.Inst.SetFog(RuntimeBaseData.CurrentChapterId);
        
    }

    protected void CreatePlayerAndCamera()
    {
        CreatePlayer(delegate
        {
            CreateCamera(delegate { IsLoadOver = true; });
        });
    }
    protected void PlayerAddEquipment()
    {
        var equipData = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150;
        AddEquipment(equipData.Slot1);
        AddEquipment(equipData.Slot2);
        AddEquipment(equipData.Slot3);
        
        
        CurrentPlayer.SetCurrentHp(CurrentPlayer.MaxHp);
    }
    private void AddEquipment(Equipment equipment)
    {
        if (equipment == null)
            return;
        
        for (int i = 0; i < equipment.EffectList.Count; i++)
        {
            EquipmentQuality quality = EquipmentQuality.Lv1;
            switch (i)
            {
                case 0:
                    quality = EquipmentQuality.Lv1;
                    break;
                case 1:
                    quality = EquipmentQuality.Lv2;
                    break;
                case 2:
                    quality = EquipmentQuality.Lv3;
                    break;
                case 3:
                    quality = EquipmentQuality.Lv4;
                    break;
            }
            float effectValue = equipment.GetEffectValue(equipment.EffectList[i], quality);
            AttributeBonus attributeBonus = new AttributeBonus();
            attributeBonus.Value = effectValue;
            attributeBonus.SourceType = SourceType.Periphery;
            switch (equipment.EffectList[i])
            {
                case EquipmentEffectType.AttackPower:
                    attributeBonus.Type = AttributeType.AttackPower;
                    break;
                case EquipmentEffectType.AttackSpeed:
                    attributeBonus.Type = AttributeType.AttackSpeed;
                    break;
                case EquipmentEffectType.MoveSpeed:
                    attributeBonus.Type = AttributeType.MoveSpeed;
                    break;
                case EquipmentEffectType.MaxHp:
                    attributeBonus.Type = AttributeType.MaxHp;
                    break;
                case EquipmentEffectType.CriticalProbability:
                    attributeBonus.Type = AttributeType.CriticalProbability;
                    break;
                case EquipmentEffectType.CriticalMultiplier:
                    attributeBonus.Type = AttributeType.CriticalMultiplier;
                    break;
                case EquipmentEffectType.DodgeProbability:
                    attributeBonus.Type = AttributeType.DodgeProbability;
                    break;
                case EquipmentEffectType.Resurrection:
                    Item resurrectionItem = new Item();
                    resurrectionItem.ID = -1;
                    resurrectionItem.ItemType = ItemType.Other;
                    var effectGroup = new ItemGroup()
                    {
                        effectTrigger = new RoleDeadTrigger(TriggerType.Times),
                        itemEffect = new ReviveEffect(1)
                    };
                    effectGroup.item = resurrectionItem;
                    effectGroup.effectTrigger.Root = effectGroup;
                    effectGroup.itemEffect.Root = effectGroup;
                    resurrectionItem.ItemGroups.Add(effectGroup);
                    CurrentPlayer.roleItemController.AddItem(resurrectionItem,null);
                    break;
                case EquipmentEffectType.Vampire:
                    Item vampireItem = new Item();
                    vampireItem.ID = -1;
                    vampireItem.ItemType = ItemType.Other;
                    var vampireEffectGroup = new ItemGroup()
                    {
                        effectTrigger = new AttackHitEnemyTrigger(TriggerType.Times,1),
                        itemEffect = new AttackPowerProportionHp(effectValue)
                    };
                    vampireEffectGroup.item = vampireItem;
                    vampireEffectGroup.effectTrigger.Root = vampireEffectGroup;
                    vampireEffectGroup.itemEffect.Root = vampireEffectGroup;
                    vampireItem.ItemGroups.Add(vampireEffectGroup);
                    CurrentPlayer.roleItemController.AddItem(vampireItem,null);
                    break;
                case EquipmentEffectType.ThornsArmor:
                    
                    Item thornsArmorItem = new Item();
                    thornsArmorItem.ID = -1;
                    thornsArmorItem.ItemType = ItemType.Other;
                    var thornsArmorEffectGroup = new ItemGroup()
                    {
                        effectTrigger = new ItemGetTrigger(TriggerType.Times),
                        itemEffect = new ThornsArmor(effectValue)
                    };
                    thornsArmorEffectGroup.item = thornsArmorItem;
                    thornsArmorEffectGroup.effectTrigger.Root = thornsArmorEffectGroup;
                    thornsArmorEffectGroup.itemEffect.Root = thornsArmorEffectGroup;
                    thornsArmorItem.ItemGroups.Add(thornsArmorEffectGroup);
                    CurrentPlayer.roleItemController.AddItem(thornsArmorItem,null);
                    break;
                case EquipmentEffectType.Gold:
                    RuntimeBaseData.AddGold((int) effectValue);
                    break;
                case EquipmentEffectType.HpTreatMultiplier:
                    attributeBonus.Type = AttributeType.HpTreatMultiplier;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            switch (equipment.EffectList[i])
            {
                case EquipmentEffectType.AttackPower:
                case EquipmentEffectType.AttackSpeed:
                case EquipmentEffectType.MoveSpeed:
                case EquipmentEffectType.MaxHp:
                case EquipmentEffectType.CriticalProbability:
                case EquipmentEffectType.CriticalMultiplier:
                case EquipmentEffectType.DodgeProbability:
                case EquipmentEffectType.HpTreatMultiplier:
                    CurrentPlayer.AddAttributeBonus(attributeBonus);
                    break;
            }
            
        }
    }

    protected void AddAttributes()
    {
        //属性加点功能 暂时关闭
        // var atk = ArchiveManager.Inst.ArchiveData.attributesArchiveData.GetCurrentAttributeData(AttributeType.AttackPower);
        // var hp = ArchiveManager.Inst.ArchiveData.attributesArchiveData.GetCurrentAttributeData(AttributeType.MaxHp);
        // var crit = ArchiveManager.Inst.ArchiveData.attributesArchiveData.GetCurrentAttributeData(AttributeType.CriticalProbability);
        //
        // AttributeBonus atkAttr = new AttributeBonus();
        // atkAttr.Type = AttributeType.AttackPower;
        // atkAttr.Value = atk.Value;
        // CurrentPlayer.AddAttributeBonus(atkAttr);
        //
        // AttributeBonus hpAttr = new AttributeBonus();
        // hpAttr.Type = AttributeType.MaxHp;
        // hpAttr.Value = hp.Value;
        // CurrentPlayer.AddAttributeBonus(hpAttr);
        //
        // AttributeBonus critAttr = new AttributeBonus();
        // critAttr.Type = AttributeType.CriticalProbability;
        // critAttr.Value = crit.Value;
        // CurrentPlayer.AddAttributeBonus(critAttr);

    }
    protected void PlayerAddGlobalRuneItem()
    {
        List<int> itemIds = new List<int>();
        foreach (var item in ArchiveManager.Inst.ArchiveData.GlobalData.UnlockRuneItem)
        {
            for (int i = 0; i < item.Value; i++)
            {
                itemIds.Add(item.Key);
            }
        }

        PlayerAddItem(itemIds);
        
    }
    
    protected void PlayerAddItem(List<int> itemIds)
    {
        foreach (int itemId in itemIds)
        {
            CurrentPlayer.roleItemController.AddItem(
                DataManager.Inst.ParsingItemObj(DataManager.Inst.GetItemScrObj(itemId)), isOk => { });
        }
    }

    protected void InitPlayerAttributes()
    {
        var heroUpgradeData = ArchiveManager.Inst.GetHeroUpgradeData(CurrentHeroData.HeroId).GetHeroUpgradeData();
        CurrentPlayer.InitAttacker(heroUpgradeData.AttackPower,heroUpgradeData.AoeMagnification);
        CurrentPlayer.InitMaxHp(heroUpgradeData.MaxHp);
    }

    protected void AddPlayerItem()
    {
        var heroUpgradeData = ArchiveManager.Inst.GetHeroUpgradeData(CurrentHeroData.HeroId).GetHeroUpgradeData();
        PlayerAddItem(heroUpgradeData.ItemIds);
    }
}
