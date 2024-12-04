using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_NavigationToggle : MonoBehaviour
{
    public enum ToggleType
    {
        Rune,
        Event,
        Hero,
        Chapter,
        Equipment,
        Attributes,
        Shop,
    }
    [SerializeField]
    private LayoutElement layoutElement;
    [SerializeField]
    private Image bg;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image lockedIcon;
    [SerializeField]
    private Text text;
    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    private ToggleType currentToggleType;

    private Toggle _toggle;
    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        if (_toggle)
        {
            _toggle.onValueChanged.AddListener(OnToggle);
        }
        
    }

    public void OnToggle(bool isOn)
    {
        if (isOn)
        {
            DOTween.To(value => layoutElement.preferredWidth = value,
                layoutElement.preferredWidth, 360, 0.15f).SetEase(Ease.Linear);
            icon.transform.DOScale(Vector3.one * 1.55f, 0.3f).SetEase(curve);
            text.transform.DOScale(Vector3.one * 1.2f, 0.3f);
            text.color = Color.yellow;
            // icon.color = Color.white;
        }
        else
        {
            DOTween.To(value => layoutElement.preferredWidth = value,
                layoutElement.preferredWidth, 260, 0.15f).SetEase(Ease.Linear);
            icon.transform.DOScale(Vector3.one, 0.3f);
            text.transform.DOScale(Vector3.one, 0.3f);
            text.color = Color.white;
            // icon.color = Color.gray;
        }

        var color = bg.color;
        color=new Color(color.r,color.g,color.b,isOn?1:0);
        bg.color = color;
    }

    private void Update()
    {
        if (isInit)
        {
            return;
        }

        if (UIManager.Inst.MaskIsShowing)
        {
            return;
        }
        Init();
    }

    private bool isInit = false;
    void Init()
    {
        isInit = true;    
        bool isUnlock = IsUnLock(currentToggleType);
        _toggle.interactable = isUnlock;
        icon.gameObject.SetActive(isUnlock);
        var unlockAnimations = ArchiveManager.Inst.ArchiveData.GlobalData.PlayedUnlockSystemAnimaton;
        if (isUnlock)
        {
            bool isPlayAnimation = false;
            switch (currentToggleType)
            {
                case ToggleType.Equipment:
                    if (  !unlockAnimations.Contains(UnlockSystemType.Equipment))
                    {
                        isPlayAnimation = true;
                        unlockAnimations.Add(UnlockSystemType.Equipment);
                        FocusGuide.Inst.StartGuide(FocusGuide.GuideType.Equipment);
                    }
                    break;
                // case ToggleType.Hero:
                //     if (!unlockAnimations.Contains(UnlockSystemType.Hero))
                //     {
                //         isPlayAnimation = true;
                //         unlockAnimations.Add(UnlockSystemType.Hero);
                //         FocusGuide.Inst.StartGuide(FocusGuide.GuideType.Hero);
                //     }
                    break;
            }
            ArchiveManager.Inst.SaveArchive();
            if (isPlayAnimation)
            {
                lockedIcon.gameObject.SetActive(true);
                icon.gameObject.SetActive(true);
                icon.color = new Color(1,1,1,0);
                lockedIcon.transform.DOScale(Vector3.one * 1.5f, 0.3f).SetDelay(2f).Delay();
                lockedIcon.DOColor(new Color(1, 1, 1, 0), 0.3f).SetDelay(2f).Delay();
                icon.DOColor(Color.white, 0.3f).SetDelay(2.3f).Delay();
            }
            else
            {
                lockedIcon.gameObject.SetActive(false);
                icon.gameObject.SetActive(true);
            }
        }
    }

    bool IsUnLock(ToggleType toggleType)
    {
        var clearData = ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas;
        switch (toggleType)
        {
            case ToggleType.Chapter:
                return true;
                break;
            case ToggleType.Shop:
                return clearData.ContainsKey(1) && clearData[1].Count > 0;
                break;
            case ToggleType.Hero:
                return clearData.ContainsKey(2) && clearData[2].Count > 0;
                break;
            case ToggleType.Equipment:
                return clearData.ContainsKey(1) && clearData[1].Count > 0;
                break;
        }

        return false;
    }
}
