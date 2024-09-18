using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractObj_Chest_DropItem : InteractObj_Chest
{
    
    public override void InteractEnd()
    {
        base.InteractEnd();
        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem, OnGetItem);
        if ( DropManager.Inst.CheckIfCanSpwanCoin())
        {
            StartCoroutine(DelayDropItem());
        }
        else
        {
            StartCoroutine(DelayGiveItem());
        }
    }
     IEnumerator DelayDropItem()
    {
        yield return new WaitForSeconds(0.5f);
        DropManager.Inst.CreateChestDropItem(transform.position);
        yield return new WaitForSeconds(2f);
        DestroyInteractObj();
    }
     IEnumerator DelayGiveItem()
     {
         yield return new WaitForSeconds(.4f);
         int chapterid = BattleManager.Inst.RuntimeData.CurrentChapterId;
         item = DataManager.Inst.ChapterDatas[chapterid].ItemData.PropPool.GetItemFromPool(1)[0];
         BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(item),null);
         if (IconParti)
         {
             IconParti.GetComponent<Renderer>().material.SetTexture("_BaseMap", item.Icon.texture);
             IconParti.Play();
             yield return new WaitForSeconds(.6f);
         }

         if (GetAudio != null)
         {
             GetAudio.Play();
         }
         
         yield return new WaitForSeconds(2f);
         DestroyInteractObj();
     }
}
