using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiveObject : MonoBehaviour
{
    public ItemPoolScriptableObject ItemPool;
    [HideInInspector]
    public ItemScriptableObject item;
    void Start()
    {
        item = ItemPool.Items[Random.Range(0,ItemPool.Items.Count)];
    }

    
}
