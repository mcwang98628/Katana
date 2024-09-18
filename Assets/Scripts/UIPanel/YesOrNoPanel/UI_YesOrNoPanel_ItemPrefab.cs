using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_YesOrNoPanel_ItemPrefab : MonoBehaviour
{
    [SerializeField]
    private Image Icon;
    [SerializeField]
    private UIText Name;
    [SerializeField]
    private UIText Desc;

    

    // private ItemScriptableObject Item;
    
    public void Init(ItemScriptableObject item)
    {
        // Item = item;
        Icon.sprite = item.Icon;
        Name.text = item.Name;
        Desc.text = item.Describe;
        
    }
}
