using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStartTimeSetter : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Renderer>().material.SetFloat("_StartTime", Time.time);
    }
}
