using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenRoom : RoomController
{
    public float DelayTime=0;
    void Start()
    {
        //直接开门
        // Invoke("OpenTheDoor",DelayTime);
        OpenTheDoor();
    }
    
    
}
