using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_MainPanel_RunePanel_RuneSlot : MonoBehaviour
{
    [SerializeField]
    public Image itemIcon;
    [SerializeField]
    private UIText itemName;
    [SerializeField]
    private Text itemCount;
    [SerializeField]
    private Image bg;

    public int ItemId;
    public void Init(int itemId)
    {
        itemIcon.material = GameObject.Instantiate(itemIcon.material);
        ItemId = itemId;
        UpdateUI();
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OnBtnClick);
    }

    public void UpdateUI()
    {
        var itemScrObj = DataManager.Inst.GetItemScrObj(ItemId);
        itemIcon.sprite = itemScrObj.Icon;
        itemName.text = itemScrObj.Name;
        if (ArchiveManager.Inst.ArchiveData.GlobalData.UnlockRuneItem.ContainsKey(ItemId))
        {
            int count = ArchiveManager.Inst.ArchiveData.GlobalData.UnlockRuneItem[ItemId];
            itemCount.text = count.ToString();
            itemIcon.material.DisableKeyword("_GRAYENABLED_ON");
        }
        else
        {
            itemCount.text = "";
            itemIcon.material.EnableKeyword("_GRAYENABLED_ON");
        }

    }

    Tweener scaleTween;
    public void OnSelected(bool isSelected)
    {
        if (isSelected)
        {
            scaleTween.Kill();
            scaleTween = bg.transform.DOScale(Vector3.one * 1.2f, 0.1f);
            bg.color = Color.yellow;
        }
        else
        {
            scaleTween.Kill();
            scaleTween = bg.transform.DOScale(Vector3.one, 0.1f);
            bg.color = Color.white;
        }
    }

    public void OnBtnClick()
    {
        UIManager.Inst.Open("InfoPanel",false,DataManager.Inst.GetItemScrObj(ItemId));
    }

}
