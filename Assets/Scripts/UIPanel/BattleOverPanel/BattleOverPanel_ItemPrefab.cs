using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleOverPanel_ItemPrefab : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text numberText;
    [SerializeField]
    private Sprite diamIcon;
    private Item itemData;
    
    public void Init(Item item)
    {
        itemData = item;
        icon.sprite = item.Icon;
        transform.localScale = Vector3.zero;
        this.gameObject.SetActive(true);
    }

    public void Init(Equipment equipment)
    {
        equipment.LoadIcon(delegate(Sprite sprite)
        {
            icon.sprite = sprite;
        });
        transform.localScale = Vector3.zero;
        this.gameObject.SetActive(true);
    }

    public void Init(int goldCount)
    {
        numberText.text = goldCount.ToString();
        icon.sprite = diamIcon;
        transform.localScale = Vector3.zero;
        numberText.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
    }

    public void Show(float value)
    {
        transform.DOScale(Vector3.one, 0.3f).SetDelay(value).Delay();
    }
    
    public void OnBtnClick()
    {
        return;
        // if (itemData == null)
        // {
        //     return;
        // }
        // var item = DataManager.Inst.GetItemScrObj(itemData.ID);
        // UIManager.Inst.Open("InfoPanel",false,item);
    }
}
