using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomDoor : MonoBehaviour
{
    public LevelLogo levelLogo;
    [HideInInspector]
    public RoomType RoomType;

    private Transform player;

    [SerializeField]
    private GameObject doorObject;
    [SerializeField]
    private GameObject logoPoint;
    
    [SerializeField] [LabelText("障碍 碰撞")]
    private BoxCollider BlockCollision;
    [SerializeField] [LabelText("过关传送 碰撞")]
    private BoxCollider NextLevelCollision;

    [HideInInspector]
    public bool IsInit;

    public bool DoorIsOpen { get; protected set; } = false;
    //本层最后房间
    public bool LastRoomDoor { get; protected set; } = false;


    public AudioClip OpenDoorSFX;
    public AudioClip EnterNextRoomSFX;


    private RoomData data;
    public void Init(RoomData roominfo,bool isLastRoom)
    {
        IsInit = true;
        data = roominfo;
        this.RoomType = roominfo.RoomType;
        LastRoomDoor = isLastRoom;
        NextLevelCollision.enabled = false;
        doorObject.SetActive(true);
    }
    public void OpenDoor()
    {
        if (!IsInit)
        {
            return;
        }

        DoorIsOpen = true;
        // NextLevelCollision.enabled = isGameOverDoor?true:LastRoomDoor;
        NextLevelCollision.enabled = true;
        BlockCollision.enabled = false;
        GetComponentInChildren<OpenDoorFeedbacks>().Open();
        if (OpenDoorSFX != null)
        {
            StartCoroutine(DelayPlayDoorOpen());
        }
        if (!isGameOverDoor)
        {
            switch (data.RoomType)
            {
                case RoomType.None:
                    break;
                case RoomType.StartRoom:
                    if (levelLogo.StartRoom != null)
                        Instantiate(levelLogo.StartRoom, logoPoint.transform);
                    break;
                case RoomType.FightRoom:                    
                    //这一段没用？
                    if (levelLogo.FightRoom != null)
                        Instantiate(levelLogo.FightRoom, logoPoint.transform);
                    break;
                    
                // case RoomType.AwardRoom:
                //     switch (data.RoomRewardType)
                //     {
                //         case RoomRewardType.Attribute_Attack:
                //             if (levelLogo.Attribute_Attack != null)
                //                 Instantiate(levelLogo.Attribute_Attack, logoPoint.transform);
                //             break;
                //         case RoomRewardType.Attribute_MaxHp:
                //             if (levelLogo.Attribute_MaxHp != null)
                //                 Instantiate(levelLogo.Attribute_MaxHp, logoPoint.transform);
                //             break;
                //         case RoomRewardType.Attribute_Speed:
                //             if (levelLogo.Attribute_Speed != null)
                //                 Instantiate(levelLogo.Attribute_Speed, logoPoint.transform);
                //             break;
                //         case RoomRewardType.Money:
                //             if (levelLogo.Money != null)
                //                 Instantiate(levelLogo.Money, logoPoint.transform);
                //             break;
                //         case RoomRewardType.Potion:
                //             if (levelLogo.Potion != null)
                //                 Instantiate(levelLogo.Potion, logoPoint.transform);
                //             break;
                //     }
                    //if (levelLogo.FightRoom != null)
                    //     Instantiate(levelLogo.FightRoom, logoPoint.transform);
                //     break;
                // case RoomType.EliteFightRoom:
                //     if (levelLogo.EliteFightRoom != null)
                //         Instantiate(levelLogo.EliteFightRoom, logoPoint.transform);
                //     break;
                // case RoomType.SpecialFightRoom:
                //     if (levelLogo.SpecialFightRoom != null)
                //         Instantiate(levelLogo.SpecialFightRoom, logoPoint.transform);
                //     break;
                case RoomType.BossFightRoom:
                    if (levelLogo.BossFightRoom != null)
                        Instantiate(levelLogo.BossFightRoom, logoPoint.transform);
                    break;
                case RoomType.TreasureRoom:
                    if (levelLogo.AwardRoom != null)
                        Instantiate(levelLogo.AwardRoom, logoPoint.transform);
                    break;
                case RoomType.EventRoom:
                    if (levelLogo.EventRoom != null)
                        Instantiate(levelLogo.EventRoom, logoPoint.transform);
                    break;
                case RoomType.ShopRoom:
                    if (levelLogo.ShopRoom != null)
                        Instantiate(levelLogo.ShopRoom, logoPoint.transform);
                    break;
            }
        }
        else
        {
            if (levelLogo.StartRoom != null)
                Instantiate(levelLogo.StartRoom, logoPoint.transform);
        }
        
        
    }
    public IEnumerator DelayPlayDoorOpen()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        AudioManager.Inst.PlaySource(OpenDoorSFX);
    }


    private bool isGameOverDoor;
    public void InitGameOverDoor()
    {
        IsInit = true;
        isGameOverDoor = true;
        doorObject.SetActive(true);
    }
    // public void EnterNextRoom()
    // {
    //     if (EnterNextRoomSFX != null)
    //     {
    //         AudioManager.Inst.PlaySource(EnterNextRoomSFX);
    //     }
    //
    //     if (isGameOverDoor)
    //     {
    //         EventManager.Inst.DistributeEvent(EventName.EnterLastDoor);
    //         return;
    //     }
    //     
    //     UIManager.Inst.ShowMask(() =>
    //     {
    //         // BattleManager.Inst.NextRoomCounter();
    //         // BattleManager.Inst.StartRoom();
    //         UIManager.Inst.HideMask(null);
    //     });
    //     
    //     
    // }

    private bool isShowConnect = false;
    public void OnEnterDoorTrigger(Collider collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Player") && collider.gameObject.layer != LayerMask.NameToLayer("PlayerRoll"))
        {
            return;
        }
        
        if (LastRoomDoor)
        {
            EventManager.Inst.DistributeEvent(EventName.EnterLastDoor);
            return;
        }

        if (isGameOverDoor)
        {
            EventManager.Inst.DistributeEvent(EventName.EnterGameOverDoor);
            return;
        }
        if (
            !IsInit ||
            !DoorIsOpen 
            ||
            isShowConnect)
        {
            return;
        }

        isShowConnect = true;

        EventManager.Inst.DistributeEvent(EventName.EnterDoor,new EnterDoorData{Position = transform.position,RoomData = data});
    }

}

public struct EnterDoorData
{
    public Vector3 Position;
    public RoomData RoomData;
}