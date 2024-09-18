using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NewItemBox : InteractObj
{
    [SerializeField]
    private ItemPoolType itemPoolType;
    [SerializeField]
    private int itemCount;
    [SerializeField]
    private Transform cap;
    [SerializeField]
    private ParticleSystem openFx;

    public override void InteractStart()
    {
        base.InteractStart();
        
        UIManager.Inst.Open("ChooseItemPanel",true,
            DataManager.Inst.ItemPool,
            itemPoolType,
            itemCount,
            new Action<Item>(delegate(Item item)
            {
                _getItem = item;
                OpenAnim();
            }));  
    }

    private Item _getItem;
    private void OpenAnim()
    {
        cap.DOLocalRotate(new Vector3(-100, 0, 0), 0.6f);
        if (_getItem != null)
        {
            openFx.GetComponent<Renderer>().material.SetTexture("_BaseMap", _getItem.Icon.texture);
            openFx.Play();
        }
    }
    
}
