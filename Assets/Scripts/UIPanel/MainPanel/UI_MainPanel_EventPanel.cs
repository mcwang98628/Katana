using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainPanel_EventPanel : MonoBehaviour
{

    [SerializeField]
    private UI_MainPanel_EventPanel_EventPrefab eventPrefab;
    [SerializeField]
    private Transform eventGroup;

    private List<UI_MainPanel_EventPanel_EventPrefab> _eventPrefabs = new List<UI_MainPanel_EventPanel_EventPrefab>();

    private string _prefabPath = "Assets/AssetsPackage/MainScene/AdventureMainScene.prefab";
    private AdventureMainScene _prefabGo;
    private EnvironmentItemScript _newEnvironmentData;
    private EnvironmentItemScript _oldEnvironmentData;
    private void OnEnable()
    {
        LoadAdventurePrefab();
    }

    private void OnDisable()
    {
        HideAdventurePrefab();
    }

    void LoadAdventurePrefab()
    {
        if (_prefabGo == null)
        {
            ResourcesManager.Inst.GetAsset<GameObject>(_prefabPath, delegate(GameObject prefab)
            {
                _prefabGo = GameObject.Instantiate(prefab).GetComponent<AdventureMainScene>();
                _prefabGo.transform.position = new Vector3(-100,0,-100);
                ShowAdventurePrefab();
            });
            
            ResourcesManager.Inst.GetAsset<EnvironmentItemScript>("Assets/AssetsPackage/ChapterDatas/EnvironmentData/Environment_AdventureEntrance.asset",
                delegate(EnvironmentItemScript environment)
                {
                    _newEnvironmentData = environment;
                    _oldEnvironmentData = EnvironmentManager.Inst.CurrentEnvironment;
                    EnterEnvironment();
                });
        }
        else
        {
            ShowAdventurePrefab();
        }
    }

    void EnterEnvironment()
    {
        if (_newEnvironmentData == null)
            return;
        EnvironmentManager.Inst.SetEnvironment(_newEnvironmentData);
    }

    void ExitEnvironment()
    {
        if (_oldEnvironmentData == null)
            return;
        EnvironmentManager.Inst.SetEnvironment(_oldEnvironmentData);
    }

    private bool _isShowing = false;
    void ShowAdventurePrefab()
    {
        if (_isShowing)
        {
            return;
        }

        _isShowing = true;
        _prefabGo.gameObject.SetActive(true);
        BattleManager.Inst.CurrentCamera.gameObject.SetActive(false);
        // var transform1 = BattleManager.Inst.CurrentPlayer.transform;
        // _oldPos = transform1.position;
        // _oldDir = transform1.forward;
        // transform1.position = _prefabGo.PlayerPosition.position;
        // transform1.forward = _prefabGo.PlayerPosition.forward;
        EnterEnvironment();
    }

    private Vector3 _oldPos;
    private Vector3 _oldDir;
    void HideAdventurePrefab()
    {
        if (!_isShowing || BattleManager.Inst.CurrentPlayer==null)
        {
            return;
        }
        _isShowing = false;
        // var transform1 = BattleManager.Inst.CurrentPlayer.transform;
        // transform1.position = _oldPos;
        // transform1.forward = _oldDir;
        
        BattleManager.Inst.CurrentCamera.gameObject.SetActive(true);
        if (_prefabGo != null)
        {
            _prefabGo.gameObject.SetActive(false);
        }

        ExitEnvironment();
    }

    
    
    
}
