using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractObj_ExorcismPool : InteractObj_NPC_YesNo
{
    public List<ItemScriptableObject> SoulStoneItems;

    [Header("OpenGroup")]
    public AudioClip OpenAudio;
    public ParticleSystem BloodParti;

    protected override void Init()
    {
        base.Init();
        canInteract = false;
        StartCoroutine(DelaySetCanIntact(true, 0.5f));
    }
    protected override void ChooseYes()
    {
        BloodParti.Play();
        AudioManager.Inst.PlaySource(OpenAudio, 1);

        List<Item> removeItem = new List<Item>();
        foreach (Item item in BattleManager.Inst.CurrentPlayer.roleItemController.Items)
        {
            if (item.ItemType == ItemType.Artifact)
            {
                removeItem.Add(item);
            }
        }
        for (int i = 0; i < removeItem.Count; i++)
        {
            BattleManager.Inst.CurrentPlayer.roleItemController.ReMoveItem(removeItem[i]);
            BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(SoulStoneItems[i]),isOk=>{});
        }
        //StartCoroutine(DelaySetCanIntact(true, 0.5f));
    }
    protected override void ChooseNo()
    {
        canInteract = true;
    }



}
