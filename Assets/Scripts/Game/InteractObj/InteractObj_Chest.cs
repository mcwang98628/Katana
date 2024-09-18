using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

//这个脚本不能直接用，需要继承
public class InteractObj_Chest : InteractObj_NPC_YesNo
{
    public ItemScriptableObject Item => item;
    protected ItemScriptableObject item;

    [SerializeField][LabelText("能否重复打开")]
    private bool isCanRepeatOpen;
    
    [Header("Chest")]
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
    [Header("AutoOpen")]
    public bool OpenOnNear;
    private Transform player;
    private float autoOpenDistance=2;

    protected override void Init()
    {
        base.Init();
        //箱子创建时面朝玩家
        //Vector3 toPlayerDir = (transform.position-BattleManager.Inst.CurrentPlayer.transform.position).normalized;
        //transform.forward = toPlayerDir;
        //transform.eulerAngles = new Vector3(0, (int)(transform.eulerAngles.y/90)*90, 0);



        Vector3 scale = Chest.transform.localScale;
        Chest.transform.localScale = Vector3.zero;
        var sequence = DOTween.Sequence();
        sequence.Append(Chest.transform.DOScale(scale, 0.2f));
        sequence.Append(Chest.transform.DOShakeScale(0.5f, 0.3f, 10));


        if (InitParti)
            InitParti.Play();
        if (InitAudio)
            AudioManager.Inst.PlaySource(InitAudio);

        player = BattleManager.Inst.CurrentPlayer.transform;

        canInteract = false;

        EventManager.Inst.AddEvent(EventName.OnRoleAddItem, OnGetItem);
        StartCoroutine(DelaySetCanIntact(true, 0.5f));
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        

        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem, OnGetItem);
    }


    protected void OnGetItem(string str, object value)
    {
        var itemdata = (RoleItemEventData)value;
        item = DataManager.Inst.GetItemScrObj(itemdata.Item.ID);
        
        if(this is InteractObj_Chest_ItemPool && item.ItemType==ItemType.Prop)
            return;
        
        OpenChestAnim();
        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem, OnGetItem);
    }

    private void FixedUpdate()
    {
        if (OpenOnNear)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < autoOpenDistance)
            {
                InteractStart();
                //Invoke("InteractStart", 0.5f);
            }
        }
    }
    public override void InteractStart()
    {
        base.InteractStart();
        if (!canInteract)
        {
            return;
        }
        canInteract = false;
        OpenChestAnim();
    }

    //开箱动画
    protected void OpenChestAnim()
    {

        OpenChestEffect();
        InteractEnd();
        //Invoke("InteractEnd", 0.5f);
    }

    public override void InteractEnd()
    {
        if (isCanRepeatOpen)
        {
            base.InteractEnd();
        }
        else
        {
            InteractManager.Inst.RemoveInteractObj(this);
        }
        

        //events
        EventManager.Inst.DistributeEvent(EventName.OnOpenDropItem, item);
    }


    protected void OpenChestEffect()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(Chest.DOShakeScale(0.6f, 0.2f, 15, 90, true));
        sequence.Append(ChestLid.DOLocalRotate(new Vector3(-150, 0, 0), 0.4f).SetEase(Ease.OutBack));

        if (InitParti)
            InitParti.Stop();
        if (OpenAudio)
            AudioManager.Inst.PlaySource(OpenAudio);
      
        BattleManager.Inst.StartCoroutine(DelayOpenEffect());
    }
    IEnumerator DelayOpenEffect()
    {
        yield return new WaitForSeconds(.4f);
        if (IconParti && item)
        {
            IconParti.GetComponent<Renderer>().material.SetTexture("_BaseMap", item.Icon.texture);
            IconParti.Play();
            yield return new WaitForSeconds(.6f);
        }

        if (GetAudio != null)
        {
            GetAudio.Play();
            yield return new WaitForSeconds(1.25f);
        }

        EventManager.Inst.DistributeEvent(EventName.CollectRoomProps);
    }

    
}
