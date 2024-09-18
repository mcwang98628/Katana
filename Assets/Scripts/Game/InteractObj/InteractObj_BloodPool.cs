using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractObj_BloodPool : InteractObj
{
    public ItemScriptableObject Item;

    [Header("OpenGroup")]
    public AudioClip OpenAudio;
    public ParticleSystem BloodParti;

    protected override void Init()
    {
        base.Init();
        canInteract = false;
        StartCoroutine(DelaySetCanIntact(true, 0.5f));
    }
    public override void InteractStart()
    {
        if (!canInteract)
        {
            return;
        }
        canInteract = false;
        BloodParti.Play();
        AudioManager.Inst.PlaySource(OpenAudio, 1);       

        Invoke("InteractEnd", 0.3f);

    }
    public override void InteractEnd()
    {
        BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(Item),isOk=>{});
        StartCoroutine(DelaySetCanIntact(true, 0.5f));
       
    }


}
