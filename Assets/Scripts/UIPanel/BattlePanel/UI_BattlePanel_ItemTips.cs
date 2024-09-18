using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePanel_ItemTips : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private UIText itemName;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Transform itemPrefabList;
    [SerializeField]
    private Transform itemBtnGroup;

    
    private bool isPlaying = false;
    public List<Item> waitList = new List<Item>();
    public void Init(Item item)
    {
        if (isPlaying)
        {
            waitList.Add(item);
            return;
        }
        isPlaying = true;
        icon.sprite = item.Icon;
        itemName.text = item.Name;
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.3f).SetUpdate(false);
        canvasGroup.DOFade(0, 0.3f).SetUpdate(false).SetDelay(2f).Delay();
        var newIcon = GameObject.Instantiate(icon.transform.parent, transform.parent);
        var newPos = item.ItemType == ItemType.Prop ? itemBtnGroup.position : itemPrefabList.position;//+ new Vector3(52f, 0, 0); itemBtnGroup
        newIcon.transform.position = icon.transform.position;
        newIcon.transform.DOMove(newPos, 0.5f).SetUpdate(false).SetDelay(2f).Delay();
        newIcon.transform.DOScale(Vector3.zero, 0.4f).OnComplete(() =>
        {
            GameObject.Destroy(newIcon.gameObject);
            EventManager.Inst.DistributeEvent(EventName.ShowBattlePanelItem,item);
            isPlaying = false;
            PlayOver();
        }).SetUpdate(true).SetDelay(2f).Delay();
    }

    void PlayOver()
    {
        if (waitList.Count>0)
        {
            var item = waitList[0];
            waitList.RemoveAt(0);
            Init(item);
        }
    }

}
