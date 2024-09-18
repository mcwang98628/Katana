using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceItemPanel_ItemPrefab : MonoBehaviour
{
    [SerializeField]
    private ReplaceItemPanel rootPanel;
    
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private UIText itemName;
    [SerializeField]
    private UIText itemDesc;
    // [SerializeField]
    // private UIText btnText;
    // [SerializeFieldr;

    private Item item;
    
    public void Init(Item item,bool showBtn)
    {
        this.item = item;
        itemIcon.sprite = item.Icon;
        itemName.text = item.Name;
        itemDesc.text = item.Desc;
        // Btn.gameObject.SetActive(showBtn);
        // if (showBtn)
        // {
        //     btnText.text = "替换";
        //     Btn.onClick.AddListener(Replace);
        // }
        // else
        // {
        //     btnText.text = "舍弃";
        //     //TODO 
        // }
        gameObject.SetActive(true);
    }

    public void Replace()
    {
        BattleManager.Inst.CurrentPlayer.roleItemController.ReMoveItem(item);
        rootPanel.AddItem();
    }
}
