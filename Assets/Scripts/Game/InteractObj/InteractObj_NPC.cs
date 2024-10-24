using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class InteractObj_NPC : InteractObj
{

    public NPCTalkObject DialogData;
    public bool CanRepeat = true;
    public bool Talked => talked;
    protected bool talked = false;

    
    public AudioClip FinishAudio;
    public ParticleSystem FinishParticle;

    protected override void Init()
    {
        base.Init();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void InteractStart()
    {
        if (DialogData == null)
        {
            return;
        }

        if (!canInteract || talked)
        {
            return;
        }
        canInteract = false;
        
        UIManager.Inst.Open("NPCTalkPanel",true, DialogData, this);



    }
    public override void InteractEnd()
    {
        if (FinishParticle)
            FinishParticle.Play();
        if (FinishAudio)
            AudioManager.Inst.PlaySource(FinishAudio, 1);
        
        StartCoroutine(DelaySetCanIntact(true, .4f));
        //canInteract = true;
        talked = true;
    }


    public List<ShopItem> ItemGods { get; private set; }
    public bool IsRefresh = false;//刷新过
    public Action RefreshFunc;

    public void SetStoreGoods(List<ShopItem> gods)
    {
        ItemGods = new List<ShopItem>();
        
        if (gods == null)
            return;
        
        foreach (var item in gods)
        {
            ItemGods.Add(new ShopItem(item.Item,item.OriginalPrice,item.Number));
        }
    }
    public void DeleteGoods(ShopItem item)
    {
        if (ItemGods.Contains(item) && item.Number > 0)
        {
            item.ChangeNumber(-1);
            if (item.Number <= 0)
            {
                ItemGods.Remove(item);   
            }
        }
    }
    public ItemReplaceFormulas.ReplaceFormula ReplaceFormula { get; private set; }
    public void SetSynthetic(ItemReplaceFormulas.ReplaceFormula rep)
    {
        ReplaceFormula = rep;
    }

    public ItemScriptableObject GiveItem { get; private set; }
    public void SetGiveItem(ItemScriptableObject obj)
    {
        GiveItem = obj;
    }
}
