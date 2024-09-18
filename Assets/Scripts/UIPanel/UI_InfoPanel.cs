using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_InfoPanel : PanelBase
{
    [SerializeField]
    private Transform infoPanel;
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private UIText itemName;
    [SerializeField]
    private UIText typeName;
    [SerializeField]
    private UIText itemText;

    [SerializeField]
    private Transform itemBuildItemPrefab;
    [SerializeField]
    private Transform itemBuildGroup;
    [SerializeField]
    private Image lockIcon;
    [SerializeField]
    private AnimationCurve curve;

    private Action closePanelCallBack;

    private Tweener scaleTweener;
    private void ShowAnim()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.DOFade(1, 0.2f).SetEase(Ease.Linear);
        if (scaleTweener!=null)
        {
            scaleTweener.Kill(true);
        }
        scaleTweener = infoPanel.DOScale(Vector3.one*1.5f, 0.2f).SetEase(curve);
    }

    public void OnOpen(ItemSchoolData schoolData)
    {
        if (string.IsNullOrEmpty(schoolData.Desc))
        {
            typeName.text = "Unlock";
        }
        else
        {
            typeName.text = "UI_MainPanel_ItemBuild";   
        }
        itemIcon.transform.parent.gameObject.SetActive(false);
        itemName.text = schoolData.Name;
        itemText.text = schoolData.Desc;
        foreach (int itemId in schoolData.ItemList)
        {
            var go = GameObject.Instantiate(itemBuildItemPrefab, itemBuildGroup);
            go.GetChild(0).GetComponent<Image>().sprite = DataManager.Inst.GetItemScrObj(itemId).Icon;
            go.gameObject.SetActive(true);
        }
        
    }
    public void OnOpen(ItemScriptableObject itemObj)
    {
        Init(itemObj);
        typeName.text = "Artifact";
        ShowAnim();
    }

    public void OnOpen(Equipment equipment)
    {
        equipment.LoadIcon(delegate(Sprite sprite)
        {
            itemIcon.sprite = sprite;
            itemName.text = equipment.Name;
            typeName.text = "";


            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < equipment.EffectList.Count; i++)
            {
                var effectType = equipment.EffectList[i];
                switch (i)
                {
                    case 0:
                        sb.Append("• " + equipment.GetEffectStr(effectType, EquipmentQuality.Lv1) + "\n");
                        break;
                    case 1:
                        sb.Append("+(" + equipment.GetEffectStr(effectType,EquipmentQuality.Lv2) + ")\n");
                        break;
                    case 2:
                        sb.Append("+(" + equipment.GetEffectStr(effectType,EquipmentQuality.Lv3) + ")\n");
                        break;
                    case 3:
                        sb.Append("+(" + equipment.GetEffectStr(effectType,EquipmentQuality.Lv4) + ")\n");
                        break;
                }
            }

            itemText.Translate = false;
            itemText.text = sb.ToString();
        });
    }
    

    List<int> itemIds = new List<int>();
    private int idIndex = 0;
    public void OnOpen(List<int> itemIds,string titleText,Action closeCallBack)
    {
        typeName.text = titleText;
        closePanelCallBack = closeCallBack;
        this.itemIds.Clear();
        this.itemIds.AddRange(itemIds);
        idIndex = 0;
        ShowItemInItemIds();
    }

    void ShowItemInItemIds()
    {
        if (idIndex >= itemIds.Count)
        {
            return;
        }
        Init(DataManager.Inst.GetItemScrObj(itemIds[idIndex]));
        idIndex++;
        ShowAnim();
    }

    private string enemyIconPath = "Assets/Arts/Textures/UISprites/EnemyIcon/{0}.png";
    public void OnOpen(EnemyInfoData enemyInfoData)
    {
        itemName.text = enemyInfoData.EnemyName;
        itemText.text = enemyInfoData.EnemyTalk;
        typeName.text = "Monster";
        switch ( enemyInfoData.EnemyType)
        {
            case EnemyType.Lv1:
                itemName.Te.color = new Color(0.3f,0.8f,0.3f);
                break;
            case EnemyType.Lv2:
                itemName.Te.color = new Color(0.2f,0.5f,1.0f);
                break;
            case EnemyType.Lv3:
                itemName.Te.color = new Color(0.5f,0.0f,1.0f);
                break;
            case EnemyType.Elite:
                itemName.Te.color = new Color(1.0f,0.3f,0.0f);
                break;
            case EnemyType.Boss:
                itemName.Te.color = new Color(0.9f,0.1f,0.0f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        ResourcesManager.Inst.GetAsset<Sprite>(string.Format(enemyIconPath,enemyInfoData.EnemyIcon),
            delegate(Sprite iconSprite)
            {
                itemIcon.sprite = iconSprite;
            });
    }

    

    void Init(ItemScriptableObject itemObj)
    {
        itemIcon.sprite = itemObj.Icon;
        itemName.text = itemObj.Name;
        if (itemObj.itemColorType != ItemColor.White)
            itemName.Te.color = Item.GetColor(itemObj.itemColorType);
        itemText.text = itemObj.Describe;
    }

    public void OnBtnClick()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.DOFade(0, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (itemIds.Count > 0 && idIndex < itemIds.Count)
            {
                ShowItemInItemIds();
                return;
            }
            UIManager.Inst.Close("InfoPanel");
            closePanelCallBack?.Invoke();
        }).SetDelay(0.1f).Delay();
        if (scaleTweener!=null)
        {
            scaleTweener.Kill(true);
        }
        scaleTweener = infoPanel.DOScale(Vector3.one * 1.5f, 0.2f).SetEase(curve);
    }
}
