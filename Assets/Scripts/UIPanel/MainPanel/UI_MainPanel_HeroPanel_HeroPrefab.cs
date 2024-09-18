using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_HeroPanel_HeroPrefab : MonoBehaviour
{
    [SerializeField]
    private UI_MainPanel_HeroPanel heroPanel;
    [SerializeField]
    private Transform prefabSizeOffsetObj;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image professionIcon;
    [SerializeField]
    private UIText heroName;
    [SerializeField]
    private GameObject lockIcon;

    [SerializeField]
    private Transform starGroup;
    [SerializeField]
    private Image starGroupBg;
    [SerializeField]
    private Image starPrefab;
    [SerializeField]
    private GameObject radPoint;
    public HeroData HeroData => _data;
    private HeroData _data;

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.HeroUpgradede,OnHeroUpgrade);
        EventManager.Inst.AddEvent(EventName.HeroUpgradedeColorLevel,OnHeroUpgrade);
        EventManager.Inst.AddEvent(EventName.OnBuyHero,OnHeroUpgrade);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.HeroUpgradede,OnHeroUpgrade);
        EventManager.Inst.RemoveEvent(EventName.HeroUpgradedeColorLevel,OnHeroUpgrade);
        EventManager.Inst.RemoveEvent(EventName.OnBuyHero,OnHeroUpgrade);
    }

    private void OnHeroUpgrade(string arg1, object arg2)
    {
        UpdateStarUI();
    }

    public void Init(HeroData data)
    {
        _data = data;
        UpdateUI();
        UpdateStarUI();
        gameObject.SetActive(true);
    }

    private string iconPath = "Assets/Arts/Textures/UISprites/HeroIcon/{0}.png";
    public void UpdateUI()
    {
        heroName.text = _data.HeroName;
        ResourcesManager.Inst.GetAsset<Sprite>(string.Format(iconPath,_data.HeroIcon),
            delegate(Sprite sprite)
            {
                icon.sprite = sprite;
            });
        // professionIcon.sprite = ResourcesManager.Inst.GetAsset<Sprite>("");
        // transform.GetChild(0).GetComponent<Image>().color = HeroData.Color;
        bool isUnlock = ArchiveManager.Inst.ArchiveData.GlobalData.UnLockHeros.Contains(_data.HeroId);
        // GetComponent<Toggle>().interactable = isUnlock;
        lockIcon.SetActive(!isUnlock);
    }

    private void UpdateStarUI()
    {
        
        var data = HeroData;
        for (int i = 1; i < starGroup.transform.childCount; i++)
        {
            Destroy(starGroup.transform.GetChild(i).gameObject);
        }
        if (ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas.ContainsKey(data.HeroId))
        {
            var heroUpgrade = ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas[data.HeroId];
            int colorLevel = heroUpgrade.GetHeroUpgradeData().ColorLevel;
            int levelCount = DataManager.Inst.GetHeroLevelCountByColorLevel(data.HeroId, colorLevel);

            starGroupBg.color = DataManager.Inst.HeroColorLevelDatas[colorLevel].Color;
            
            for (int i = 0; i < levelCount; i++)
            {
                Image starImg = GameObject.Instantiate(starPrefab, starGroup.transform);
                starImg.gameObject.SetActive(true);
                if (heroUpgrade.GetHeroUpgradeData().Level > i)
                {
                    starImg.color =  new Color(1.0f,0.75f,0);
                }
                else
                {
                    //Destroy(starImg.gameObject);
                    starImg.color = new Color(0.2f,0.2f,0.2f);
                }
            }
        }
    }
    
    
    public void OnToggle(bool isOn)
    {
        if (isOn)
        { 
            heroPanel.SelectPrefab(this);
        }
    }

    private float radPointTimer = 1f;
    private void Update()
    {
        radPointTimer += Time.unscaledDeltaTime;
        if (radPointTimer >= 1f)
        {
            radPointTimer -= 1f;
            UpdateRadPoint();
        }
    }

    void UpdateRadPoint()
    {
        bool isCanReceiveSkillSoul = ArchiveManager.Inst.CanReceiveSkillSoul(HeroData.HeroId,1) > 0 ||
                                     ArchiveManager.Inst.CanReceiveSkillSoul(HeroData.HeroId,2) > 0;
        radPoint.SetActive(isCanReceiveSkillSoul);
    }
}
