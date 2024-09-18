using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public class InteractObj_SlotMachine : InteractObj
{
    public ItemPoolScriptableObject AwardItemPool;
    public ItemPoolScriptableObject BreakAwardItemPool;
    [Header("参数")]
    public float InitPossiblity;
    public float AddPossibility;
    int drawCount;
    public int Price;
    public int MinItemCount = 5;
    public int MaxItemCount = 10;
    int breakCount = 0;
    int getItemCount = 0;
    [SerializeField]
    public PriceTypeEnum PriceType;
    public enum PriceTypeEnum
    {
        Money,
        Health
    }
    [Header("声音")]
    public AudioClip StartAudio;
    public AudioClip BadStartAudio;
    public AudioClip WinAudio;
    public AudioClip LostAudio;
    public AudioClip BreakAudio;
    [Header("图标")]
    public MeshRenderer Icon;
    public Sprite LostIcon;
    public Sprite EmptyIcon;
    [Header("其它表现")]
    public Animator Animator;
    public ParticleSystem FinishParti;
    [Header("Break")]
    public GameObject BreakModel;
    public ParticleSystem BreakParti;
    

    protected override void Init()
    {
        base.Init();
        canInteract = false;
        Icon.material.SetTexture("_BaseMap", EmptyIcon.texture);
        StartCoroutine(DelaySetCanIntact(true, 0.5f));
        breakCount = Random.Range(MinItemCount, MaxItemCount);
    }
    public override void InteractStart()
    {
        if (!canInteract)
        {
            return;
        }
        canInteract = false;

        if (BattleManager.Inst.CurrentGold >= Price && PriceType == PriceTypeEnum.Money)
        {
            Icon.material.SetTexture("_BaseMap", EmptyIcon.texture);
            BattleManager.Inst.AddGold(-Price);
            AudioManager.Inst.PlaySource(StartAudio, 1);
            Animator.SetTrigger("Play");
            Invoke("InteractEnd", 1f);
        }
        else if (/*BattleManager.Inst.CurrentPlayer.CurrentHp > Price && */
        PriceType == PriceTypeEnum.Health)
        {
            Icon.material.SetTexture("_BaseMap", EmptyIcon.texture);
            float dmgValue=Price;
            if (ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas.ContainsKey(BattleManager.Inst.CurrentPlayer
                .UniqueID))
            {
                dmgValue =
                Price *(0.5f+ ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas[BattleManager.Inst.CurrentPlayer.UniqueID].ColorLevel*0.5f); 
            }

            
            DamageInfo damage = new DamageInfo(BattleManager.Inst.CurrentPlayer.TemporaryId,dmgValue,BattleManager.Inst.CurrentPlayer,transform.position,DmgType.Unavoidable);
            BattleManager.Inst.CurrentPlayer.HpInjured(damage);
            AudioManager.Inst.PlaySource(StartAudio, 1);
            Animator.SetTrigger("Play");
            Invoke("InteractEnd", 1f);
        }
        else
        {
            AudioManager.Inst.PlaySource(BadStartAudio, 1);
            Animator.SetTrigger("BadPlay");
            StartCoroutine(DelaySetCanIntact(true, 0.5f));
        }


    }

    public override void InteractEnd()
    {
        if (Random.value < InitPossiblity+drawCount*AddPossibility)
        {
            //中奖
            AudioManager.Inst.PlaySource(WinAudio, 1);
            var items = AwardItemPool.GetItemFromPool(1);
            Icon.material.SetTexture("_BaseMap", items[0].Icon.texture);
            BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(items[0]),
                isOk =>
                {
                    if (isOk)
                    {
                        getItemCount++;            
                        drawCount=0;

                        if (getItemCount >= breakCount)
                        {
                            Break();
                            return;
                        }
                    }
                });
        }
        else
        {
            drawCount++;
            //没中
            AudioManager.Inst.PlaySource(LostAudio, 1);
            Icon.material.SetTexture("_BaseMap", LostIcon.texture);
        }

        FinishParti.Play();
        StartCoroutine(DelaySetCanIntact(true, 0.5f));

    }

    void Break()
    {
        InteractManager.Inst.RemoveInteractObj(this);
        canInteract = false;
        AudioManager.Inst.PlaySource(BreakAudio, 1);

        Animator.speed = 0;
        StartCoroutine(DelaySwitchModel(.05f));
        BreakParti.Play();

        BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(BreakAwardItemPool.GetItemFromPool(1)[0]),isOk=>{});
    }

    IEnumerator DelaySwitchModel(float time)
    {
        yield return new WaitForSeconds(time);

        BreakModel.SetActive(true);
        Animator.gameObject.SetActive(false);
    }


}
