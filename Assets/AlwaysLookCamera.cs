using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookCamera : MonoBehaviour
{
    void Update()
    {
        //transform.LookAt(Camera.main.transform.position, Vector3.up);
        transform.forward = -Camera.main.transform.forward;
    }
}
