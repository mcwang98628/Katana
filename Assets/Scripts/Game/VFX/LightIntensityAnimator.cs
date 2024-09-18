using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityAnimator : MonoBehaviour
{
    public AnimationCurve IntensityCurve;
    public float Cycle;
  
    float StartIntensity;
    float StartTime;
    Light light;
    private void Start()
    {
        light = GetComponent<Light>();
        StartIntensity = light.intensity;
        StartTime = Time.time;
    }
    private void Update()
    {
        light.intensity = StartIntensity*IntensityCurve.Evaluate((Time.time - StartTime) % Cycle);
    }


}
