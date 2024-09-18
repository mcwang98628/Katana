using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Inst { get; private set; }
    [SerializeField]
    private GameObject heroScene;
    [SerializeField]
    private GameObject mainScene;
    [SerializeField]
    private ParticleSystem upgradeFx;
    [SerializeField]
    private List<Animator> heroAnimators;
    [SerializeField]
    private Transform heroSceneCamera;
    private void Awake()
    {
        Inst = this;
        OpenMainScene();
    }

    public void OpenHeroScene()
    {
        mainScene.SetActive(false);
        heroScene.SetActive(true);
    }

    public void OpenMainScene()
    {
        mainScene.SetActive(true);
        heroScene.SetActive(false);
    }

    
    public void HeroUpgrade()
    {
        foreach (Animator heroAnimator in heroAnimators)
        {
            heroAnimator.SetTrigger("UpGrade");
        }
        upgradeFx.Play();
    }

    public void SelectHero()
    {
        foreach (Animator heroAnimator in heroAnimators)
        {
            heroAnimator.SetTrigger("EnterGame");
        }
    }

    public void MoveHeroSceneCamera(float x,float time,Action callback=null)
    {
        heroSceneCamera.DOLocalMoveX(x, time).OnComplete(() => { callback?.Invoke(); });
    }
    
}
