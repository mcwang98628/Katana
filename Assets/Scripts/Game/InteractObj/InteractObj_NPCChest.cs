using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractObj_NPCChest : InteractObj_NPC
{
    ItemScriptableObject item;
    public Transform ChestLid;
    public Transform Chest;

    [Header("InitGroup")]
    public ParticleSystem InitParti;
    public AudioClip InitAudio;

    [Header("OpenGroup")]
    public AudioClip OpenAudio;
    public ParticleSystem IconParti;
    public AudioClip GetAudio;
    public float GetAudioDelay;
    protected override void Init()
    {
        base.Init();
        Chest.transform.DOShakeScale(0.8f, 0.3f, 10);


        if (InitParti)
            InitParti.Play();
        if (InitAudio)
            AudioManager.Inst.PlaySource(InitAudio);


        canInteract = false;
        StartCoroutine(DelaySetCanIntact(true, 0.8f));

        EventManager.Inst.AddEvent(EventName.OnRoleAddItem, GetItem);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem, GetItem);
    }

    public override void InteractStart()
    {
        if (!canInteract)
        {
            return;
        }
        canInteract = false;
        ChestLid.DOLocalRotate(new Vector3(-150, 0, 0), 0.4f).SetEase(Ease.OutBack);

        AudioManager.Inst.PlaySource(OpenAudio, 1);

        //item = ItemPool.GetItemFromPool();
        UIManager.Inst.Open("NPCTalkPanel",true, DialogData, this);



    }
    void GetItem(string arg1, object value)
    {
        var itemdata = (RoleItemEventData)value;
        item = DataManager.Inst.GetItemScrObj(itemdata.Item.ID);

        if (InitParti)
            InitParti.Stop();
        if (IconParti)
        {
            IconParti.GetComponent<Renderer>().material.SetTexture("_BaseMap", DataManager.Inst.ParsingItemObj(item).Icon.texture);
            IconParti.Play();
        }


        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem, GetItem);
        InteractEnd();
    }
    public override void InteractEnd()
    {
        //BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(item);
        InteractManager.Inst.RemoveInteractObj(this);
        EventManager.Inst.DistributeEvent(EventName.CollectRoomProps);
        EventManager.Inst.DistributeEvent(EventName.OnOpenDropItem, item);
        //events       
        StartCoroutine(TriggerGetAudio());
    }
    public IEnumerator TriggerGetAudio()
    {
        yield return new WaitForSeconds(GetAudioDelay);
        if (GetAudio != null)
            GetAudio.Play();

    }

}
