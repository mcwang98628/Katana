using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorFeedbacks : MonoBehaviour
{
    public GameObject OpenDoorParticles;
    public AudioClip OpenDoorSFX;
    public GameObject Collision;

    private void OnEnable()
    {
        EventManager.Inst.AddEvent(EventName.OpenDoor, OpentheDoor);
    }
    private void OnDisable()
    {
        EventManager.Inst.RemoveEvent(EventName.OpenDoor, OpentheDoor);
    }
    public void OpentheDoor(string a,object b)
    {
        Open();
    }
    public void Open()
    {
        //Debug.Log("OpenTheDoor");
        if (OpenDoorSFX != null)
        {
            AudioManager.Inst.PlaySource(OpenDoorSFX,0);
        }
        if (OpenDoorParticles != null)
        {
            OpenDoorParticles.SetActive(true);
        }
        GetComponent<Animator>().SetBool("IsOpen", true);
        if (Collision!=null)
        {
            Collision.SetActive(false);
        }

    }

    public void Close()
    {
        if (OpenDoorParticles != null)
        {
            OpenDoorParticles.SetActive(false);
        }
        GetComponent<Animator>().SetBool("IsOpen", false);
    }
}
