using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

public class SummonedCreatureController : MonoBehaviour
{
    public enum CreatureTypeEnum
    {
        DestroyOnRoomClear,
        DestroyAfterTime,
        DestroyOnEnterNewRoom,
        NeverDestroy
    }
    public CreatureTypeEnum CreatureType;
    [ShowIf("CreatureType", CreatureTypeEnum.DestroyAfterTime)]
    public float LifeTime = 0;

    void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f);
        switch (CreatureType)
        {
            case CreatureTypeEnum.DestroyAfterTime:
                Invoke("DestroyCreature", LifeTime);
                EventManager.Inst.AddEvent(EventName.EnterNextRoom, DestroyCreature);
                break;
            case CreatureTypeEnum.DestroyOnEnterNewRoom:
                EventManager.Inst.AddEvent(EventName.EnterNextRoom, DestroyCreature);
                break;
            case CreatureTypeEnum.DestroyOnRoomClear:
                EventManager.Inst.AddEvent(EventName.CollectRoomProps, DestroyCreature);
                break;
            case CreatureTypeEnum.NeverDestroy:
                EventManager.Inst.AddEvent(EventName.EnterNextRoom, ResetPosition);
                break;
        }

    }

    void ResetPosition(string arg1, object arg2)
    {
        StartCoroutine(ResetPositionIE());
    }
    IEnumerator ResetPositionIE()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = BattleManager.Inst.CurrentPlayer.transform.position + new Vector3(Random.value * 0.3f, 0, Random.value * 0.3f);
    }
    // Update is called once per frame
    void DestroyCreature(string arg1, object arg2)
    {
        DestroyCreature();
    }
    void DestroyCreature()
    {
        transform.DOScale(Vector3.zero, 0.5f);
        Destroy(gameObject, 0.5f);
    }
    private void OnDestroy()
    {
        switch (CreatureType)
        {
            case CreatureTypeEnum.DestroyAfterTime:
                EventManager.Inst.RemoveEvent(EventName.EnterNextRoom, DestroyCreature);
                break;
            case CreatureTypeEnum.DestroyOnEnterNewRoom:
                EventManager.Inst.RemoveEvent(EventName.EnterNextRoom, DestroyCreature);
                break;
            case CreatureTypeEnum.DestroyOnRoomClear:
                EventManager.Inst.RemoveEvent(EventName.CollectRoomProps, DestroyCreature);
                break;
            case CreatureTypeEnum.NeverDestroy:
                EventManager.Inst.RemoveEvent(EventName.EnterNextRoom, ResetPosition);
                break;
        }
    }
}
