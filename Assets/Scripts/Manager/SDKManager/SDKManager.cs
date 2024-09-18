using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public partial class SDKManager : MonoBehaviour
{
    public TGAnalytics tGAnalytics;
    private bool isInit;
    public void Init()
    {
        if (isInit)
        {
            return;
        }
        isInit = true;
        tGAnalytics.AwakeInit();    
        
        
        if (!FB.IsInitialized) {
            FB.Init(InitCallback, OnHideUnity);
        } else {
            FB.ActivateApp();
        }
        
        
        Debug.Log("#SDK# SDKManagerInit");
    }
    
    
    private void InitCallback ()
    {
        if (FB.IsInitialized) {
            FB.ActivateApp();
        } else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity (bool isGameShown)
    {
        Debug.Log("isGameShown: "+isGameShown);
    }
    
    
    private void Awake()
    {
        // Debug.LogError("SDK埋点需要重新设计，此Log只用于上线前提醒。");
        EventManager.Inst.AddEvent(EventName.OnAppStart,OnAppStart);
        AddBattleEvent();
        AddMainPanelEvent();
        Init();
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnAppStart,OnAppStart);
        RemoveBattleEvent();
        RemoveMainPanelEvent();
    }
    
    private void OnAppStart(string arg1, object arg2)
    {
        TGAnalytics.Track(TGANames.AppStart,GetCommonData());
    }
}
