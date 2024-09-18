using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 OpenRotation;
    public Vector3 CloseRotation;
    public float RotateSpeed = 0.3f;
    public float RotateTime = 1f;

    private void Start()
    {
        //CloseDoor();
    }

    //关门
    public void CloseDoor()
    {
        //Debug.Log("TryCloseDoor");
        GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(CloseDoorIE());
    }
    //开门
    public void OpenDoor()
    {
        //Debug.Log("TryOpenDoor");
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(OpenDoorIE());
    }
    public IEnumerator CloseDoorIE()
    {
        float StartTime = Time.time;
        while(Time.time < StartTime + RotateTime)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(CloseRotation), RotateSpeed);

            //transform.localRotation = Quaternion.Euler(CloseRotation);
            yield return null;
        }
    }
    public IEnumerator OpenDoorIE()
    {
        float StartTime = Time.time;
        while (Time.time<StartTime + RotateTime )
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(OpenRotation), RotateSpeed);
            //transform.localRotation = Quaternion.Euler(OpenRotation);
            yield return null;
        }
    }

}
