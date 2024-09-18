using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractObj_LifeSpring : InteractObj
{
    public ItemScriptableObject Item;
    public Transform Water;
    public ParticleSystem FliesParti;
    public AudioClip TriggerAudio;


    protected override void Init()
    {
        base.Init();
    }
    public override void InteractStart()
    {
        canInteract = false;
        FliesParti.Stop();
        AudioManager.Inst.PlaySource(TriggerAudio, 1);
        Water.DOMoveY(0f, 2f);
        InteractEnd();

    }
    public override void InteractEnd()
    {        
        BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(Item), isOk =>
        {
            if (isOk)
            {
                InteractManager.Inst.RemoveInteractObj(this);
            }
        });
    }

}
