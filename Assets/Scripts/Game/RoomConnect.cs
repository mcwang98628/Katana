using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoomConnect : MonoBehaviour
{
    public float ConnectLength => (start_Length.position - end_Length.position).magnitude;

    [SerializeField]
    private Transform start_Length;
    [SerializeField]
    private Transform end_Length;
    
    [SerializeField]
    private GameObject obstacleBox;
    // [SerializeField]
    // private Transform boxPoint;

    // private void Awake()
    // {
    //     EventManager.Inst.AddEvent(EventName.EnterNextRoom,OnEnterNextRoom);
    // }
    //
    // private void OnDestroy()
    // {
    //     EventManager.Inst.RemoveEvent(EventName.EnterNextRoom,OnEnterNextRoom);
    // }
    //
    // private void OnEnterNextRoom(string arg1, object arg2)
    // {
    //     
    //     if (itemBox!=null)
    //     {
    //         GameObject.Destroy(itemBox.gameObject);
    //     }
    // }

    public void ShowObstacle(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")||
            other.gameObject.layer == LayerMask.NameToLayer("PlayerRoll"))
        {
            obstacleBox.SetActive(true);
        }
    }

    private bool isEnterNextRoomTrigger = false;
    public void EnterNextRoom(Collider other)
    {
        if (isEnterNextRoomTrigger)
        {
            return;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")||
            other.gameObject.layer == LayerMask.NameToLayer("PlayerRoll"))
        {
            isEnterNextRoomTrigger = true;
            EventManager.Inst.DistributeEvent(EventName.OnEnterNextRoom);
            obstacleBox.SetActive(false);
        }
    }

    private bool isHideCurrentRoomTrigger = false;
    public void HideCurrentRoomTrigger(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player") &&
            other.gameObject.layer != LayerMask.NameToLayer("PlayerRoll"))
        {
            return;
        }
        if (isHideCurrentRoomTrigger)
        {
            return;
        }
        EventManager.Inst.DistributeEvent(EventName.HideCurrentRoom);
        isHideCurrentRoomTrigger = true;
    }
    private bool isShowNextRoomTrigger = false;
    public void ShowNextRoomTrigger(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player") &&
            other.gameObject.layer != LayerMask.NameToLayer("PlayerRoll"))
        {
            return;
        }
        if (isShowNextRoomTrigger)
        {
            return;
        }
        EventManager.Inst.DistributeEvent(EventName.ShowNextRoom);
        isShowNextRoomTrigger = true;
    }


    private bool isHide = true;
    private float showTime = 0.5f;
    public void Show()
    {
        isHide = false;
        transform.DOMoveY(0,showTime).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        transform.DOMoveY(-10, .75f).SetEase(Ease.InBack).OnComplete(() =>
        {
            isEnterNextRoomTrigger = false;
            isHideCurrentRoomTrigger = false;
            isShowNextRoomTrigger = false;
            isHide = true;
        });
    }

    private Coroutine _coroutine;
    public void SetPosition(Vector3 targetPos)
    {
        if (_coroutine!=null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _coroutine = StartCoroutine(WaitSetPos(targetPos));
    }

    IEnumerator WaitSetPos(Vector3 targetPos)
    {
        yield return null;
        yield return null;
        
        while (!isHide)
        {
            yield return null;
        }
        transform.position = targetPos;
    }

    // public void SetNextRoomInfo(RoomData roomInfo)
    // {
    //     StartCoroutine(WaitCreateReward(roomInfo));
    // }

    // private GameObject itemBox;
    // IEnumerator WaitCreateReward(RoomData roomInfo)
    // {
    //     yield return new WaitForSecondsRealtime(showTime);
    //
    //     /*
    //     Vector3 position = boxPoint.position;
    //     if (itemBox!=null)
    //     {
    //         GameObject.Destroy(itemBox.gameObject);
    //     }
    //     if (roomInfo.RoomType == RoomType.EliteFightRoom)
    //     {
    //         GameObject dropItemPrefab = ResourcesManager.Inst.GetAsset<GameObject>("Assets/BundleAssets/Prefabs/InterObj_DropItem_Elite.prefab");
    //         itemBox = Instantiate(dropItemPrefab, position, Quaternion.identity);
    //     }
    //     else if (roomInfo.RoomType == RoomType.FightRoom)
    //     {
    //         GameObject dropItemPrefab = ResourcesManager.Inst.GetAsset<GameObject>("Assets/BundleAssets/Prefabs/InterObj_DropItem_Normal.prefab");
    //         itemBox = Instantiate(dropItemPrefab, position, Quaternion.identity);
    //         itemBox.GetComponent<InteractObj_Chest_DropItem>().SetItem(roomInfo.RoomRewardType);
    //     } 
    //     */
    // }
}
