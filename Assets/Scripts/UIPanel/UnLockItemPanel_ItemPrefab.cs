using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnLockItemPanel_ItemPrefab : MonoBehaviour
{
    [SerializeField]
    private Image Icon;

    private ItemScriptableObject _itemScriptableObject;
    
    public void Init(ItemScriptableObject itemData)
    {
        this.gameObject.SetActive(true);
        _itemScriptableObject = itemData;
        Icon.sprite = itemData.Icon;
    }

    public void OnBtnClick()
    {
        if (_itemScriptableObject == null)
        {
            return;
        }
        UIManager.Inst.Open("InfoPanel",false,_itemScriptableObject);
    }
}
