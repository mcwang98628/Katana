using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePanel_ItemPrefab : MonoBehaviour
{

    [SerializeField]
    private Image Bg;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text numberText;
    private Item itemData;
    public Item ItemData => itemData;


    [SerializeField]
    private AnimationCurve _curve;
    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.ItemTrigger,OnItemTrigger);
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.3f);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.ItemTrigger,OnItemTrigger);
    }

    private Tweener iconTweener; 
    private void OnItemTrigger(string arg1, object arg2)
    {
        if (itemData==null)
        {
            return;
        }
        string id = ((Item) arg2).TemporaryId;
        if (id != itemData.TemporaryId)
        {
            return;
        }

        if (iconTweener!=null)
        {
            iconTweener.Kill(true);
        }
        iconTweener = transform.DOScale(Vector3.one * 1.5f, 0.2f).SetEase(_curve);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="ItemId">唯一ID</param>
    public void Init(Item item)
    {
        gameObject.SetActive(true);
        
        icon.gameObject.SetActive(item!=null);
        
        if (item==null)
        {
            itemData = null;
            return;
        }

        itemData = item;
        icon.sprite = itemData.Icon;
        Bg.enabled = (item.ItemType == ItemType.Artifact);
    }
    
    
    public void ShowInfo()
    {
        if (itemData==null)
        {
            return;
        }
        var item = DataManager.Inst.GetItemScrObj(itemData.ID);
        UIManager.Inst.Open("InfoPanel",false,item);
    }

    public void UpdateNumber()
    {
        if (itemData==null)
        {
            return;
        }
        int count = BattleManager.Inst.CurrentPlayer.roleItemController.GetItemCount(ItemData.ID);
        if (count>1)
        {
            numberText.text = count.ToString();
        }
        else
        {
            numberText.text = "";
        }
    }
}
