using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel : PanelBase
{
    public Canvas Canvas;
    public Camera CanvasCamera;

    [SerializeField]
    private UI_MainPanel_Tips _tips;
    [SerializeField]
    private UI_MainPanel_BackgroundColor _background;
    [SerializeField]
    private RectTransform navigationBar;
    [SerializeField] [LabelText("章节Toggle")]
    private Toggle chapterToggle;
    [SerializeField]
    private GameObject ChapterPanel;
    [SerializeField]
    private GameObject HeroPanel;
    [SerializeField]
    private GameObject EventPanel;
    [SerializeField]
    private GameObject RunePanel;
    [SerializeField]
    private GameObject IllustrationsPanel;
    
    [SerializeField]
    private GameObject EquipPanel;
    
    [SerializeField]
    private GameObject AttributesPanel;

    [SerializeField] 
    private GameObject ShopPanel;
    
    

    // [SerializeField] 
    // private GameObject IllustrationsRadPoint;
    [SerializeField] 
    private GameObject HeroRadPoint;
    [SerializeField] 
    private GameObject equipRadPoint;
    
    public void OnOpen()
    {
        chapterToggle.isOn = true;

        // var unlockItems = ArchiveManager.Inst.ArchiveData.TemporaryData.UnlockItems;
        // if (unlockItems != null && unlockItems.Count > 0 )
        // {
        //     UIManager.Inst.Open("UnLockItemPanel",false,unlockItems,new Action(() => { }));
        //     ArchiveManager.Inst.ArchiveData.TemporaryData.UnlockItems.Clear();
        //     ArchiveManager.Inst.SaveArchive();
        // }
        
        navigationBar.sizeDelta = new Vector2(navigationBar.sizeDelta.x,navigationBar.sizeDelta.y-(Screen.safeArea.height - Screen.safeArea.max.y));

        AddAssets();

        SetBg();


        // for (int i = 0; i < 10; i++)
        // {
        //     var equip = EquipmentTool.RandomEquipment(10, EquipmentQuality.Lv4);
        //     Debug.LogError(equip.Name);
        //     foreach (var effect in equip.EffectList)
        //         Debug.LogError(effect.ToString());
        //     Debug.LogError("-------");
        // }
    }

    public override void Show()
    {
        base.Show();
        UIManager.Inst.ShowAssetsBar(true);
        
    }

    private void AddAssets()
    {
        GameManager.Inst.StartCoroutine(waitAddAssets());
    }

    IEnumerator waitAddAssets()
    {
        yield return null;
        yield return new WaitForSecondsRealtime(0.5f);
        
        if (ArchiveManager.Inst.ArchiveData.TemporaryData.AddDiamondValue>0)
        {
            ArchiveManager.Inst.ChangeDiamond(ArchiveManager.Inst.ArchiveData.TemporaryData.AddDiamondValue);
            ArchiveManager.Inst.ArchiveData.TemporaryData.AddDiamondValue = 0;
        }
        
        // if (ArchiveManager.Inst.ArchiveData.TemporaryData.AddSoulValue>0)
        // {
        //     ArchiveManager.Inst.ChangeSoul(ArchiveManager.Inst.ArchiveData.TemporaryData.AddSoulValue);
        //     ArchiveManager.Inst.ArchiveData.TemporaryData.AddSoulValue = 0;
        // }
        // if (ArchiveManager.Inst.ArchiveData.TemporaryData.AddExpValue>0)
        // {
        //     ArchiveManager.Inst.ChangeExp(ArchiveManager.Inst.ArchiveData.TemporaryData.AddExpValue);
        //     ArchiveManager.Inst.ArchiveData.TemporaryData.AddExpValue = 0;
        // }

        foreach (Equipment equipment in ArchiveManager.Inst.ArchiveData.TemporaryData.EquipmentList)
        {
            Debug.LogError(equipment.Name);
        }

        foreach (Equipment equipment in ArchiveManager.Inst.ArchiveData.TemporaryData.EquipmentList)
        {
            ArchiveManager.Inst.ArchiveData.equipmentArchiveData150.AddEquip(equipment);
        }
        ArchiveManager.Inst.ArchiveData.TemporaryData.EquipmentList.Clear();
        
        ArchiveManager.Inst.SaveArchive();
    }

    public void OnEnterAdventureBtnClick()
    {
        if (ArchiveManager.Inst.ArchiveData.GlobalData.Fire < 1)
        {
            _tips.Show(UI_Asset.AssetType.Fire);
            return;
        }
        UIManager.Inst.ShowMask(() =>
        {
            GameManager.Inst.StartCoroutine(EnterAdventure());
        });
    }

    IEnumerator EnterAdventure()
    {
        UIManager.Inst.Close("MainPanel");
        BattleManager.Inst.LoadAdventureBattle();
        yield return null;
        BattleManager.Inst.StartGame();
        yield return null;
        yield return null;
        UIManager.Inst.HideMask(null);
    }

    public override void Hide(Action callBack)
    {
        base.Hide(callBack);
        UIManager.Inst.ShowAssetsBar(false);
    }
 
    protected override void OnPause()
    {
    }

    protected override void OnUnPause()
    {
    }

    public void OnChapterToggle(bool isOn)
    {
        ChapterPanel.SetActive(isOn);
        SetCameraRender(!isOn);
    }

    public void OnHeroToggle(bool isOn)
    {
        HeroPanel.SetActive(isOn);
    }

    public void OnEventPanel(bool isOn)
    {
        EventPanel.SetActive(isOn);
    }

    public void OnRuneToggle(bool isOn)
    {
        RunePanel.SetActive(isOn);
    }

    public void OnIllustrationsToggle(bool isOn)
    {
        IllustrationsPanel.SetActive(isOn);
    }
    public void OnEquipPanelToggle(bool isOn)
    {
        EquipPanel.SetActive(isOn);
    }
    public void OnAttributesPanelToggle(bool isOn)
    {
        AttributesPanel.SetActive(isOn);
    }

    public void OnShopPanelToggle(bool isOn)
    {
        ShopPanel.SetActive(isOn);
    }

    public void SetCameraRender(bool isCamera)
    {
        return;
        if (isCamera)
        {
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            CanvasCamera.gameObject.SetActive(true);
        }
        else
        {
            Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasCamera.gameObject.SetActive(false);
        }
    }

    private float timer = 1;
    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer >= 1f)
        {
            timer -= 1f;
            UpdateRadPoint();
        }
    }

    void UpdateRadPoint()
    {
        // bool illustratedRad = ArchiveManager.Inst.EnemyIllustratedCanGetReceive() ||
        //                       ArchiveManager.Inst.ItemIllustratedCanGetReceive() ||
        //                       ArchiveManager.Inst.ItemBuildIllustratedCanGetReceive();
        // IllustrationsRadPoint.SetActive(illustratedRad);

        bool isCanReceiveSkillSoul = false;
        foreach (KeyValuePair<int,HeroUpgradeInfo> heroUpgradeData in ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas)
        {
            if (ArchiveManager.Inst.CanReceiveSkillSoul(heroUpgradeData.Value.HeroId,1) > 0 ||
                ArchiveManager.Inst.CanReceiveSkillSoul(heroUpgradeData.Value.HeroId,2) > 0)
            {
                isCanReceiveSkillSoul = true;
                break;
            }
        }
        HeroRadPoint.SetActive(isCanReceiveSkillSoul);
        bool isNeedPeek = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150.NeedPeekEquipList.Count > 0;
        equipRadPoint.SetActive(isNeedPeek);
    }

    void SetBg()
    {
        HeroData heroData = DataManager.Inst.HeroDatas[ArchiveManager.Inst.ArchiveData.GlobalData.LastSelectHeroID];
        _background.SetColor(heroData.BgColor);
    }
}
