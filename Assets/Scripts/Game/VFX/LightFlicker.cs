using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light light;
    public float FlickMinTime;
    public float FlickMaxTime;
    private void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(flick());
    }
    public IEnumerator flick()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(FlickMinTime,FlickMaxTime));
            light.enabled = false;
            yield return null;
            light.enabled = true;
        }
    }
}
