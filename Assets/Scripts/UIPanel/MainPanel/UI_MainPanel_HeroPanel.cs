using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_HeroPanel : MonoBehaviour
{
    [SerializeField]
    private UI_MainPanel_BackgroundColor _backgroundColor;
    [SerializeField]
    private UI_MainPanel_Tips _tips;
    [SerializeField]
    private UI_MainPanel_HeroPanel_HeroPrefab heroPrefab;
    
    
    [SerializeField]
    private Transform heroPrefabGroup;

    [SerializeField]
    [BoxGroup("预览")]
    private UIText heroName;
    [SerializeField]
    [BoxGroup("预览")]
    private UIText heroProfessionName;
    [SerializeField]
    [BoxGroup("预览")]
    private UIText heroDesc;
    [SerializeField]
    [BoxGroup("预览")]
    private Image heroSkill1Icon;
    [SerializeField]
    [BoxGroup("预览")]
    private Image heroSkill2Icon;
    [SerializeField]
    [BoxGroup("预览")]
    private UIText heroSkill1Name;
    [SerializeField]
    [BoxGroup("预览")]
    private UIText heroSkill2Name;
    [SerializeField]
    [BoxGroup("预览")]
    private Text heroSkill1Desc;
    [SerializeField]
    [BoxGroup("预览")]
    private Text heroSkill2Desc;

    [SerializeField]
    [BoxGroup("Button")]
    private GameObject selectBtn;
    [SerializeField]
    [BoxGroup("Button")]
    private UIText selectBtnText;
    [SerializeField]
    [BoxGroup("Button")]
    private GameObject buyBtn;
    [SerializeField]
    [BoxGroup("Button")]
    private Text buyNumberText;

    [SerializeField]
    private Toggle chapterToggle;

    [SerializeField]
    private Transform cameraTrans;

    [SerializeField]
    private Image starPrefab;
    [SerializeField]
    private Transform starGroup;
    [SerializeField]
    private Slider upgradeBtnSlider;
    // [SerializeField]
    // private UI_MainPanel_HeroPanel_UpgradeBtn upgradeBtn;
    // [SerializeField]
    // private GameObject upgradeColorLevelBtn;
    [SerializeField]
    private Text upgradeExpText1;
    [SerializeField]
    private Text upgradeExpText2;
    
    [SerializeField]
    private GameObject skill1RadPoint;
    [SerializeField]
    private GameObject skill2RadPoint;

    private List<UI_MainPanel_HeroPanel_HeroPrefab> _prefabs = new List<UI_MainPanel_HeroPanel_HeroPrefab>();
    private UI_MainPanel_HeroPanel_HeroPrefab _currentSelectHeroPrefab;

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.HeroUpgradede,OnHeroUpgrade);
        EventManager.Inst.AddEvent(EventName.HeroUpgradede,ShowHeroUpgradePanel);
        EventManager.Inst.AddEvent(EventName.HeroUpgradedeColorLevel,OnHeroUpgrade);
        EventManager.Inst.AddEvent(EventName.HeroUpgradedeColorLevel,ShowPromotePanel);
        EventManager.Inst.AddEvent(EventName.OnBuyHero,OnHeroUpgrade);
    }


    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.HeroUpgradede,OnHeroUpgrade);
        EventManager.Inst.RemoveEvent(EventName.HeroUpgradede,ShowHeroUpgradePanel);
        EventManager.Inst.RemoveEvent(EventName.HeroUpgradedeColorLevel,OnHeroUpgrade);
        EventManager.Inst.RemoveEvent(EventName.HeroUpgradedeColorLevel,ShowPromotePanel);
        EventManager.Inst.RemoveEvent(EventName.OnBuyHero,OnHeroUpgrade);
    }
    
    private void ShowPromotePanel(string arg1, object arg2)
    {
        UIManager.Inst.Open("PromotePanel", false, arg2);
    }

    private void ShowHeroUpgradePanel(string arg1, object arg2)
    {
        UIManager.Inst.Open("LevelUpPanel",false);
    }

    private void OnHeroUpgrade(string arg1, object arg2)
    {
        if (arg2 != null)
        {
            int heroId = (int) arg2;
            UIManager.Inst.Open("UnLockHeroPanel",false,heroId);
        }
        UpdateHeroColorLevel();
        UpdateHeroText();
        UpdateStar();
        UpdateUpgradeBtnSlider();
        UpdateUpgradeColorLevelBtn();
        if (currentPlayerGo != null)
            currentPlayerGo.transform.GetComponentInChildren<Animator>()?.SetTrigger("UnLock");
    }

    private void OnEnable()
    {
        if (_isInit)
        {
            UpdateAllPrefab();
            UpdateBtnText();
        }
        else
        {
            Init();
        }
    }

    private bool _isInit = false;
    public void Init()
    {
        int index = 0;
        foreach (var heroData in DataManager.Inst.HeroDatas)
        {
            var go = GameObject.Instantiate(heroPrefab, heroPrefabGroup);
            go.Init(heroData.Value);
            go.name = "HeroPrefab" + index;
            
            // bool isUnlock = ArchiveManager.Inst.ArchiveData.GlobalData.UnLockHeros.Contains(heroData.Value.HeroId);
            // if (isUnlock)
            // {
            //     go.transform.SetSiblingIndex(0);
            // }
            
            _prefabs.Add(go);

            if (heroData.Value.HeroId == ArchiveManager.Inst.ArchiveData.GlobalData.LastSelectHeroID)
            {
                go.GetComponent<Toggle>().isOn = true;
            }

            index++;
        }
        _isInit = true;
    }

    public void SelectPrefab(UI_MainPanel_HeroPanel_HeroPrefab prefab)
    {
        if (_currentSelectHeroPrefab == prefab)
            return;
        _currentSelectHeroPrefab = prefab;
        UpdatePerviewHero();
        UpdateBtnText();
        UpdateRadPoint();
    }

    private string playerModelPath = "Assets/AssetsPackage/PlayerPrefabs/UI_{0}.prefab";
    private string skillIconPath = "Assets/Arts/Textures/UISprites/SkillIcons/{0}.png";
    private GameObject currentPlayerGo;
    public void UpdatePerviewHero()
    {
        var data = _currentSelectHeroPrefab.HeroData;
        _backgroundColor.SetColor(data.BgColor);
        UpdateHeroText();
        if (currentPlayerGo != null)
        {
            GameObject.Destroy(currentPlayerGo);
        }
        ResourcesManager.Inst.GetAsset<GameObject>(string.Format(playerModelPath, data.HeroPrefabName),
            delegate (GameObject prefab)
            {
                currentPlayerGo = GameObject.Instantiate(prefab, cameraTrans);
                currentPlayerGo.transform.localPosition = new Vector3(0.45f, -0.4f, 6f);
                currentPlayerGo.transform.eulerAngles = new Vector3(0, 180, 0f);

                UpdateHeroColorLevel();
            });
        
        
        //Star
        UpdateStar();
        UpdateUpgradeColorLevelBtn();
        UpdateUpgradeBtnSlider();
    }

    private void UpdateHeroColorLevel()
    {
        var data = _currentSelectHeroPrefab.HeroData;
        PlayerAppearance playerAppearance = currentPlayerGo.GetComponentInChildren<PlayerAppearance>();
        if (ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas.ContainsKey(data.HeroId))
        {
            // ReSharper disable once Unity.NoNullPropagation
            playerAppearance?.Init(ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas[data.HeroId]
                .ColorLevel);
        }
        else
            // ReSharper disable once Unity.NoNullPropagation
            playerAppearance?.Init(1);
    }

    private void UpdateHeroText()
    {
        
        var data = _currentSelectHeroPrefab.HeroData;
        string colorDesc = "";
        StringBuilder skill1Desc = new StringBuilder();
        StringBuilder skill2Desc = new StringBuilder();
        heroName.Te.color = Color.white;
        heroProfessionName.Te.color =  Color.white;
        var archiveHeroUpgrade = ArchiveManager.Inst.GetHeroUpgradeData(data.HeroId);
        if (archiveHeroUpgrade != null)
        {
            var colorLevel = archiveHeroUpgrade.ColorLevel;
            var heroColorLevelData = DataManager.Inst.HeroColorLevelDatas[colorLevel];
            heroName.Te.color = heroColorLevelData.Color;
            heroProfessionName.Te.color = heroColorLevelData.Color;
            colorDesc = heroColorLevelData.Desc;
            var targetSkillDesc = DataManager.Inst.GetHeroTargetAllSkillDesc(archiveHeroUpgrade.HeroId, archiveHeroUpgrade.ColorLevel,
                archiveHeroUpgrade.Level);
            foreach (HeroUpgradeData upgradeData in targetSkillDesc)
            {
                string skillStr = $"<color=#{ColorUtility.ToHtmlStringRGBA(DataManager.Inst.HeroColorLevelDatas[upgradeData.ColorLevel].Color)}>"
                                  + LocalizationManger.Inst.GetText(upgradeData.SkillDesc) + "</color>";
                switch (upgradeData.SkillId)
                {
                    case 1:
                        skill1Desc.Append("\n" + skillStr);
                        break;
                    case 2:
                        skill2Desc.Append("\n" + skillStr);
                        break;
                }
            }
        }
        heroName.text = data.HeroName;
        heroDesc.text = data.HeroDesc;
        heroProfessionName.text = colorDesc;// + "-" + data.ProfessionName;

        heroSkill1Name.text = data.SKill1Name;
        heroSkill1Desc.text = LocalizationManger.Inst.GetText(data.SKill1Desc) + skill1Desc.ToString();
        heroSkill2Name.text = data.SKill2Name;
        heroSkill2Desc.text = LocalizationManger.Inst.GetText(data.SKill2Desc) + skill2Desc.ToString();

        heroSkill1Icon.transform.parent.parent.parent.gameObject.SetActive(data.SKill1Icon!=" ");
        heroSkill2Icon.transform.parent.parent.parent.gameObject.SetActive(data.SKill2Icon!=" ");

        heroSkill1Icon.transform.parent.GetComponent<Image>().color = data.Color;
        heroSkill2Icon.transform.parent.GetComponent<Image>().color = data.Color;
        ResourcesManager.Inst.GetAsset<Sprite>(string.Format(skillIconPath, data.SKill1Icon),
            delegate (Sprite sprite)
            {
                heroSkill1Icon.sprite = sprite;
            });
        ResourcesManager.Inst.GetAsset<Sprite>(string.Format(skillIconPath, data.SKill2Icon),
            delegate (Sprite sprite)
            {
                heroSkill2Icon.sprite = sprite;
            });
    }

    private void UpdateAllPrefab()
    {
        foreach (var prefab in _prefabs)
        {
            prefab.UpdateUI();
        }
    }
    
    public void OnBuyBtnClick()
    {
        var errType = ArchiveManager.Inst.BuyHero(_currentSelectHeroPrefab.HeroData.HeroId);
        switch (errType)
        {
            case ArchiveErrorType.NoError:
                break;
            case ArchiveErrorType.DiamondShortage:
                _tips.Show(UI_Asset.AssetType.Diamond);
                break;
            case ArchiveErrorType.AlreadyOwned:
                _tips.Show("Error！");
                break;
        }
        UpdateBtnText();
        UpdateAllPrefab();
        EventManager.Inst.DistributeEvent(TGANames.MainPanelBuyHero,_currentSelectHeroPrefab.HeroData.HeroId);
    }

    private float timer = 0;
    public void OnUpgrade(int exp)
    {
        if (exp > ArchiveManager.Inst.ArchiveData.GlobalData.Soul)
        {
            exp = ArchiveManager.Inst.ArchiveData.GlobalData.Soul;
        }

        if (exp == 0)
        {
            if (Time.time - timer > 1)
            {
                timer = Time.time;
                _tips.Show(UI_Asset.AssetType.Soul);
            }
            return;
        }
        
        // upgradeBtn.UpgradeBtnEffect();
        var heroId = _currentSelectHeroPrefab.HeroData.HeroId;
        ArchiveManager.Inst.HeroUpgrade(heroId, exp);
        EventManager.Inst.DistributeEvent(EventName.OnAssetTips,new AssetTipsData()
        {
            Type = UI_Asset.AssetType.Soul,
            TargetPoint = upgradeBtnSlider.transform.position,
            AssetCount = exp
        });
        UpdateUpgradeBtnSlider();
    }

    [SerializeField] private Sprite FullStar;
    [SerializeField] private Sprite EmptyStar;
    private void UpdateStar()
    {
        
        var data = _currentSelectHeroPrefab.HeroData;
        for (int i = 0; i < starGroup.childCount; i++)
        {
            Destroy(starGroup.GetChild(i).gameObject);
        }
        if (ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas.ContainsKey(data.HeroId))
        {
            var heroUpgrade = ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas[data.HeroId];
            int colorLevel = heroUpgrade.GetHeroUpgradeData().ColorLevel;
            int levelCount = DataManager.Inst.GetHeroLevelCountByColorLevel(data.HeroId, colorLevel);
            for (int i = 0; i < levelCount; i++)
            {
                Image starImg = GameObject.Instantiate(starPrefab, starGroup);
                starImg.gameObject.SetActive(true);
                if (heroUpgrade.GetHeroUpgradeData().Level > i)
                {
                    starImg.sprite = FullStar;
                }
                else
                {
                    starImg.sprite = EmptyStar;
                }
            }
        }
    }
    private void UpdateUpgradeBtnSlider()
    {
        var heroId = _currentSelectHeroPrefab.HeroData.HeroId;
        var currentHeroUpgrade = ArchiveManager.Inst.GetHeroUpgradeData(heroId);
        bool isCanUpgradeLevel = currentHeroUpgrade != null && ArchiveManager.Inst.IsCanUpgradeLevel(currentHeroUpgrade.GetHeroUpgradeData());
        // upgradeBtn.gameObject.SetActive(isCanUpgradeLevel);
        
        if (!isCanUpgradeLevel)
            return;
        
        var heroUpgrade = ArchiveManager.Inst.GetHeroUpgradeData(heroId);
        upgradeBtnSlider.value = heroUpgrade.UseingExp / (float)heroUpgrade.GetHeroUpgradeData().NeedExp;
        upgradeExpText1.text = $"{heroUpgrade.UseingExp}/{heroUpgrade.GetHeroUpgradeData().NeedExp}";
    }

    private void UpdateUpgradeColorLevelBtn()
    {
        var heroId = _currentSelectHeroPrefab.HeroData.HeroId;
        var currentHeroUpgrade = ArchiveManager.Inst.GetHeroUpgradeData(heroId);
        bool isCanUpgradeLevel = currentHeroUpgrade != null && ArchiveManager.Inst.IsCanUpgradeLevel(currentHeroUpgrade.GetHeroUpgradeData());
        bool isCanUpgradeColorLevel = !isCanUpgradeLevel && currentHeroUpgrade != null && ArchiveManager.Inst.IsCanUpgradeColorLevel(currentHeroUpgrade.GetHeroUpgradeData());
        // upgradeColorLevelBtn.gameObject.SetActive(isCanUpgradeColorLevel);
        
        if (currentHeroUpgrade != null) 
            upgradeExpText2.text = currentHeroUpgrade.GetHeroUpgradeData().NeedExp.ToString();
    }

    public void OnUpgradeColorLevelBtnClick()
    {
        var heroId = _currentSelectHeroPrefab.HeroData.HeroId;
        var err = ArchiveManager.Inst.HeroUpgradeColorLevel(heroId);
        if (err == ArchiveErrorType.SourShortage)
        {
            _tips.Show(UI_Asset.AssetType.Soul);
        }
    }
    public void OnSelectBtnClick()
    {
        ArchiveManager.Inst.SetLastSelectHero(_currentSelectHeroPrefab.HeroData.HeroId);
        UpdateBtnText();
        chapterToggle.isOn = true;
        
        EventManager.Inst.DistributeEvent(TGANames.MainPanelSelectHero,_currentSelectHeroPrefab.HeroData.HeroId);
    }

    private void UpdateBtnText()
    {
        bool isCurrentSelectHero = ArchiveManager.Inst.ArchiveData.GlobalData.LastSelectHeroID == _currentSelectHeroPrefab.HeroData.HeroId;
        if (isCurrentSelectHero)
        {
            selectBtnText.text = "UI_Selected";
        }
        else
        {
            selectBtnText.text = "UI_Select";
        }
        buyNumberText.text = _currentSelectHeroPrefab.HeroData.Price.ToString();


        bool isUnlock = ArchiveManager.Inst.ArchiveData.GlobalData.UnLockHeros.Contains(_currentSelectHeroPrefab.HeroData.HeroId);
        if (isUnlock)
        {
            selectBtn.SetActive(true);
            buyBtn.SetActive(false);
        }
        else
        {
            selectBtn.SetActive(false);
            buyBtn.SetActive(true);
        }

    }

    public void OnHeroSkill1Click()
    {
        // var heroId = _currentSelectHeroPrefab.HeroData.HeroId;
        // int receiveTimes = ArchiveManager.Inst.CanReceiveSkillSoul(heroId,1,true);
        // if (receiveTimes > 0)
        // {
        //     ArchiveManager.Inst.ChangeSoul(receiveTimes * 20);
        //     ArchiveManager.Inst.SaveArchive();
        // }
    }
    public void OnHeroSkill2Click()
    {
        // var heroId = _currentSelectHeroPrefab.HeroData.HeroId;
        // int receiveTimes = ArchiveManager.Inst.CanReceiveSkillSoul(heroId,2,true);
        // if (receiveTimes > 0)
        // {
        //     ArchiveManager.Inst.ChangeSoul(receiveTimes * 20);
        //     ArchiveManager.Inst.SaveArchive();
        // }
    }


    private float radTimer = 0.5f;
    private void Update()
    {
        radTimer += Time.unscaledDeltaTime;
        if (radTimer >= 0.5f)
        {
            radTimer -= 0.5f;
            UpdateRadPoint();
        }
    }

    void UpdateRadPoint()
    {
        var heroId = _currentSelectHeroPrefab.HeroData.HeroId;
        skill1RadPoint.SetActive(ArchiveManager.Inst.CanReceiveSkillSoul(heroId,1) > 0);
        skill2RadPoint.SetActive(ArchiveManager.Inst.CanReceiveSkillSoul(heroId,2) > 0);
    }
}
