using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;
    [SerializeField] 
    private Transform head;
    [SerializeField] 
    private Color startColor;
    [SerializeField] 
    private Color endColor;
    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField] 
    private GameObject model;

    void Update()
    {
        if (Camera.main == null)
        {
            return;
        }

        if (head == null)
        {
            Debug.LogError(gameObject.name+"的head为空");
            return;
        }
        transform.LookAt(Camera.main.transform);
        transform.position = head.position + offset;
    }

    void DoColor(float time)
    {
        sr.enabled = true;
        sr.color = startColor;
        sr.DOColor(endColor, time).OnComplete(() =>
        {
            sr.enabled = false;
        });
    }
}
