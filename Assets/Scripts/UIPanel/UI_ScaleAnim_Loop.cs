using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScaleAnim_Loop : MonoBehaviour
{
    [SerializeField] private float cycleTime = 1f;
    [SerializeField] private AnimationCurve animCurve;
    private Vector3 initScale;

    private void Start()
    {
        initScale = transform.localScale;
    }

    private void Update()
    {
        transform.localScale = initScale * animCurve.Evaluate(Time.time);
    }
}
