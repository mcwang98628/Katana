using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_NextLevelPanel : PanelBase
{
    [LabelText("已通过颜色")] [SerializeField] [BoxGroup("颜色")]
    private Color passColor;
    [LabelText("当前颜色")] [SerializeField] [BoxGroup("颜色")]
    private Color currentColor;
    [LabelText("没遇到的颜色")] [SerializeField] [BoxGroup("颜色")]
    private Color afterColor;

    [LabelText("刀光")] [SerializeField] [BoxGroup("Audio")]
    private AudioClip bladAudio;
    [LabelText("enemy")] [SerializeField] [BoxGroup("Audio")]
    private AudioClip enemyAudio;
    [LabelText("刀光Volume")] [SerializeField] [BoxGroup("Audio")]
    private float bladAudioVolume= 1;
    [LabelText("enemyVolume")] [SerializeField] [BoxGroup("Audio")]
    private float enemyAudioVolume= 1;
    
    
    private List<Image> iconList = new List<Image>();
    private List<Image> bladList = new List<Image>();

    [SerializeField]
    private GameObject bossPrefab;
    [SerializeField]
    private GameObject enemyPrefab; 
    
    [SerializeField]
    private AnimationCurve bladCurve;
    [SerializeField]
    private AnimationCurve currentIconCurve;
    [SerializeField]
    private float currentIconSize;
    [SerializeField]
    private float currentIconDoTime;
    [SerializeField]
    private Transform panelGroup;

    public override void Show()
    {
        gameObject.SetActive(true);
        IsShow = true;
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        if (_canvasScaler == null)
        {
            _canvasScaler = GetComponent<CanvasScaler>();
        }

        if (_canvasScaler != null)
        {
            float value = (float) Screen.width / (float) Screen.height;
            // _canvasScaler.referenceResolution = new Vector2();
            _canvasScaler.matchWidthOrHeight = value>0.57f ? 1 : 0;
        }

        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        OnPause();
    }

    void Init()
    {
        if (BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData data)
        {
            if (data.LevelStructData is ChapterStructData structData)
            {
                int count = structData.RoomList.Count;
                if (data.LevelStructData.LevelStructType == LevelStructType.Adventure)
                {
                    count = _targetLeveIndex + 1;
                }
                for (int i = 0; i < count; i++)
                {
                    var lastIndex = structData.RoomList[i].Count-1;
                    GameObject go;
                    if (structData.RoomList[i][lastIndex].RoomType == RoomType.BossFightRoom)
                    {
                        go = GameObject.Instantiate(bossPrefab, panelGroup);
                    }
                    else
                    {
                        go = GameObject.Instantiate(enemyPrefab, panelGroup);
                    }
                    iconList.Add(go.GetComponent<Image>());
                    bladList.Add(go.transform.GetChild(0).GetComponent<Image>());
                    go.gameObject.SetActive(true);
                    if (data.LevelStructData.LevelStructType != LevelStructType.Adventure)
                    {
                        go.transform.SetSiblingIndex(0);
                    }
                }
            }
        }
    }

    private int _targetLeveIndex;
    public void OnOpen(int levelindex)
    {
        _targetLeveIndex = levelindex;
        Init();
        int currentLevel = levelindex-1;
        EventManager.Inst.DistributeEvent(EventName.JoyUp);

        for (int i = 0; i < currentLevel; i++)
        {
            iconList[i].color = passColor;
            bladList[i].gameObject.SetActive(true);
            bladList[i].transform.Rotate(new Vector3(0,0,1),Random.Range(-30f,30f));
        }

        for (int i = currentLevel; i < iconList.Count; i++)
        {
            iconList[i].color = afterColor;
        }

        iconList[currentLevel].color = currentColor;
        bladList[currentLevel].gameObject.SetActive(true);
        bladList[currentLevel].color = new Color(1,1,1,0);
        bladList[currentLevel].DOColor(Color.white, 0.2f).SetDelay(0.8f).Delay();
        bladList[currentLevel].transform.localScale = Vector3.one * 6;
        bladList[currentLevel].transform.Rotate(new Vector3(0,0,1),Random.Range(-30f,30f));
        bladList[currentLevel].transform.DOScale(Vector3.one, 0.2f).SetEase(bladCurve).OnComplete(() =>
        {
            iconList[currentLevel].color = passColor;
            panelGroup.DOShakePosition(0.1f, 50,30);
            if (iconList.Count>currentLevel+1)
            {
                iconList[currentLevel + 1].DOColor(currentColor, currentIconDoTime).SetDelay(0.5f).Delay();
                iconList[currentLevel + 1].transform.DOScale(Vector3.one*currentIconSize, currentIconDoTime).SetEase(currentIconCurve).SetDelay(0.5f).Delay();
            }
        }).SetDelay(0.8f).Delay();
        StartCoroutine(WaitClosePanel());
        StartCoroutine(waitPlayAudio());
    }

    IEnumerator waitPlayAudio()
    {
        yield return new WaitForSecondsRealtime(0.9f);
        AudioManager.Inst.PlaySource(bladAudio,bladAudioVolume);
        yield return new WaitForSecondsRealtime(0.7f);
        AudioManager.Inst.PlaySource(enemyAudio,enemyAudioVolume);
    }
    

    IEnumerator WaitClosePanel()
    {
        yield return new WaitForSecondsRealtime(3f);
        UIManager.Inst.Close();
    }
}
