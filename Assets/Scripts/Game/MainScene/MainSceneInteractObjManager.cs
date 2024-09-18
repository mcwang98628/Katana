using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MainSceneInteractObjManager : MonoBehaviour
{
    public static MainSceneInteractObjManager Inst { get; private set; }

    public MainScenePlayerMove MainScenePlayer;
    public List<MainSceneInteractObj> MainSceneInteractObjs = new List<MainSceneInteractObj>();
    public MainSceneInteractObj CurrentSelectObj;
    
    private void Awake()
    {
        Inst = this;
        StartCoroutine(LoopFindInteractObj());
    }

    IEnumerator LoopFindInteractObj()
    {
        while (true)
        {
            FindInteractObj();    
            yield return new  WaitForSecondsRealtime(0.1f);
        }
    }
    [SerializeField]
    private float findDis = 1f;
    public void FindInteractObj()
    {
        float mixDis = 99999f;
        MainSceneInteractObj minInteractObj = null;
        foreach (MainSceneInteractObj interactObj in MainSceneInteractObjs)
        {
            if (interactObj == null)
            {
                continue;
            }
            float dis = (interactObj.transform.position - MainScenePlayer.transform.position).magnitude;
            if (dis>findDis)
            {
                continue;
            }
            if (mixDis > dis)
            {
                mixDis = dis;
                minInteractObj = interactObj;
            }
        }

        if (minInteractObj == null)
        {
            if (CurrentSelectObj)
            {
                CurrentSelectObj.OnUnSelect();
                CurrentSelectObj = null;
            }
        }
        else
        if (CurrentSelectObj == null || CurrentSelectObj != minInteractObj)
        {
            CurrentSelectObj = minInteractObj;
            CurrentSelectObj.OnSelect();
        }
    }
    
}
