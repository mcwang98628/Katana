using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideManager : MonoBehaviour
{
    public static GuideManager Inst { get; private set; }

    
    public List<GameObject> Rooms = new List<GameObject>();
    public List<OpenDoorFeedbacks> Doors = new List<OpenDoorFeedbacks>(); 
    
    void Awake()
    {
        Inst = this;
    }

    private bool isCloseDoor;
    private void Update()
    {
        if (BattleManager.Inst.EnemyTeam.Count == 0)
        {
            if (isCloseDoor)
            {
                isCloseDoor = false;
                OpenDoor();
            }
        }
        else
        {
            isCloseDoor = true;
        }
    }

    private int currentRoomIndex;
    private void Start()
    {
        currentRoomIndex = 0;
        Rooms[currentRoomIndex].SetActive(true);
    }

    public void OnExitDoor()
    {
        UIManager.Inst.ShowMask(() =>
        {
            EventManager.Inst.DistributeEvent(EventName.JoyUp);
            currentRoomIndex++;
            if (currentRoomIndex < Rooms.Count)
            {
                Rooms[currentRoomIndex-1].SetActive(false);
                Rooms[currentRoomIndex].SetActive(true);
                CloseDoor();
                GameObject startPoint = GameObject.Find("Entrance_Z");
                BattleManager.Inst.CurrentPlayer.transform.position = startPoint.transform.position;
            }
            else
            {
                // BattleManager.Inst.EndGuide();
            }
            UIManager.Inst.HideMask(null);
        });
    }


    public void OpenDoor()
    {
        Doors[currentRoomIndex].Open();
    }

    private void CloseDoor()
    {
        Doors[currentRoomIndex].Close();
    }
    
}
