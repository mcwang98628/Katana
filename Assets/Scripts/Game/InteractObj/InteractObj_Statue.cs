using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractObj_Statue : InteractObj_NPC_YesNo
{
    public ParticleSystem TriggerParti;
    public AudioClip TriggerAudio;
    //public GameObject OtherStatue;



    protected override void ChooseYes()
    {
        //OtherStatue.GetComponent<InteractObj_Statue>().SetCanInteract(false);
        TriggerParti.Play();
        AudioManager.Inst.PlaySource(TriggerAudio, 1);
        transform.GetChild(0).GetComponent<Renderer>().material.DOColor(Color.white, "_BaseColor", 1);
        //OtherStatue.transform.GetChild(0).GetComponent<Renderer>().material.DOColor(Color.white, "_BaseColor", 1);

        canInteract = false;
    }
    protected override void ChooseNo()
    {
        canInteract = true;
    }

}
