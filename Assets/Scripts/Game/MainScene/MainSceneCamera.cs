using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainSceneCamera : MonoBehaviour
{
    public static MainSceneCamera Inst { get; private set; }
    
    public Vector3 DefaultPos;
    public Vector3 DefaultRot;
    public Vector3 HeroPos;
    public Vector3 HeroRot;

    private void Awake()
    {
        Inst = this;
    }

    public void MoveDefault()
    {
        transform.DOMove(DefaultPos, 0.5f);
        transform.DORotate(DefaultRot, 0.5f);
    }
    
    public void MoveHero()
    {
        transform.DOMove(HeroPos, 0.5f);
        transform.DORotate(HeroRot, 0.5f);
    }
    
}
