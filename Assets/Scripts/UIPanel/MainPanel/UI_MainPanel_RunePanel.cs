using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_RunePanel : MonoBehaviour
{
    [SerializeField]
    private UI_MainPanel_Tips _tips;
    [SerializeField]
    private Image currentIcon;
    [SerializeField]
    private Text randomBtnDiamondText;
    [SerializeField]
    private Text remainingTimesText;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    List<UI_MainPanel_RunePanel_RuneSlot> runeSlots = new List<UI_MainPanel_RunePanel_RuneSlot>();
    [SerializeField]
    Transform MainSlot;
    [SerializeField]
    AnimationCurve ScaleAnimCurve;


    [SerializeField]
    private AudioClip RuneDialSound;
    [SerializeField]
    private AudioClip RuneGetSound;


    private int RemainingTimes
    {
        get
        {
            return ArchiveManager.Inst.ArchiveData.GlobalData.CurrentLevel - ArchiveManager.Inst.ArchiveData.GlobalData.UnlockRuneCount;
        }
    }

    private ExpLevelData? CurrentRuneUpgradeData
    {
        get
        {
            if (DataManager.Inst.ExpLevelData.ContainsKey(ArchiveManager.Inst.ArchiveData.GlobalData.UnlockRuneCount + 1))
            {
                return DataManager.Inst.ExpLevelData[ArchiveManager.Inst.ArchiveData.GlobalData.UnlockRuneCount + 1];
            }
            else
            {
                return null;
            }
        }
    }


    private int CurrentDiamond => ArchiveManager.Inst.ArchiveData.GlobalData.Diamond;


    private void Awake()
    {
        Init();
        UpdateUI();
    }

    public void Init()
    {
        for (int i = 0; i < runeSlots.Count; i++)
        {
            runeSlots[i].Init(DataManager.Inst.RuneSlotDatas[i].ItemId);
        }
    }

    public void OnBtnClick()
    {
        if (RemainingTimes < 1)
        {
            _tips.Show("UI_Tips_NoTimes");
            return;
        }

        if (!CurrentRuneUpgradeData.HasValue)
        {
            _tips.Show("Err:没有剩余次数! 没配置当面等级应该获得什么");
            return;
        }

        if (CurrentDiamond < CurrentRuneUpgradeData.Value.RuneItemPrice)
        {
            _tips.Show(UI_Asset.AssetType.Diamond);
            return;
        }

        List<UI_MainPanel_RunePanel_RuneSlot> runeList = new List<UI_MainPanel_RunePanel_RuneSlot>();
        runeList.AddRange(runeSlots);
        runeList.AddRange(runeSlots);
        for (int i = 0; i < runeList.Count; i++)
        {
            runeList.Add(runeList[i]);
            if (runeList[i].ItemId == CurrentRuneUpgradeData.Value.RuneItemId)
            {
                break;
            }
        }
        StartCoroutine(Tween(runeList));


        ArchiveManager.Inst.AddUnlockRuneItem(CurrentRuneUpgradeData.Value);
        EventManager.Inst.DistributeEvent(TGANames.MainPanelBuyRune);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < runeSlots.Count; i++)
        {
            runeSlots[i].UpdateUI();
        }

        if (CurrentRuneUpgradeData.HasValue)
        {
            randomBtnDiamondText.text = CurrentRuneUpgradeData.Value.RuneItemPrice.ToString();
        }
        remainingTimesText.text = LocalizationManger.Inst.GetText("UI_MainPanel_RemainingTimes") + RemainingTimes;
    }


    IEnumerator Tween(List<UI_MainPanel_RunePanel_RuneSlot> runeList)
    {
        canvasGroup.interactable = false;
        yield return null;
        currentIcon.gameObject.SetActive(true);
        float interval = 0;
        for (int i = 0; i < runeList.Count; i++)
        {
            currentIcon.sprite = runeList[i].itemIcon.sprite;
            foreach (var runeSlot in runeList)
            {
                runeSlot.OnSelected(false);
            }
            runeList[i].OnSelected(true);
            AudioManager.Inst.PlaySource(RuneDialSound);
            interval = Mathf.Lerp(0.1f, 0.25f, (float)i / runeList.Count);
            MainSlot.DOScale(Vector3.one*1.1f,interval).SetEase(ScaleAnimCurve);
            yield return new WaitForSecondsRealtime(interval);
        }

        yield return new WaitForSecondsRealtime(1f);
        AudioManager.Inst.PlaySource(RuneGetSound);
        var newIcon = GameObject.Instantiate(currentIcon, currentIcon.transform.parent.parent);
        newIcon.transform.position = currentIcon.transform.position;
        newIcon.transform.DOScale(Vector3.one * 1.5f, 0.3f);
        yield return null;
        currentIcon.gameObject.SetActive(false);

        var tweener = newIcon.transform.DOMove(runeList[runeList.Count - 1].transform.position, 0.3f);
        tweener.SetDelay(0.5f).Delay();
        newIcon.transform.DOScale(Vector3.one * 0.65f, 0.3f).SetDelay(0.5f).Delay();
        yield return new WaitForSecondsRealtime(1f);
        GameObject.Destroy(newIcon.gameObject);

        UpdateUI();

        yield return null;

        foreach (var runeSlot in runeList)
        {
            runeSlot.OnSelected(false);
        }

        canvasGroup.interactable = true;
    }
}
