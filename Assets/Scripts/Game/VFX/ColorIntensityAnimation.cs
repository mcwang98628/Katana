using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorIntensityAnimation : MonoBehaviour
{
    public AnimationCurve animationCurve;
    Material _mat;
    ParticleSystem _particleSystem;
    Color StartColor;
    float StartTime;
    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _mat = _particleSystem.GetComponent<Renderer>().material;
        StartTime = Time.time;
        StartColor = _mat.GetColor("_EmissionColor");
    }
    private void Update()
    {
        _mat.SetColor("_EmissionColor",StartColor* animationCurve.Evaluate(Time.time - StartTime));
    }

}
