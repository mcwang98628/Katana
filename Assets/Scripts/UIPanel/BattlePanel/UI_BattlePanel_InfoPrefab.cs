using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePanel_InfoPrefab : MonoBehaviour
{
    public enum InfoType
    {
        [LabelText("金币")] Gold,
        [LabelText("难度")] Difficulty,
        [LabelText("进度 百分比")] Progress,
    }

    [SerializeField] private InfoType infoType;
    [SerializeField] private Text text;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField]
    private UIText DifficultyText;
    [SerializeField]
    private GameObject DifficultyIconPrefab;
    
    private int value = -1;

    private void Update()
    { 
        switch (infoType)
        {
            case InfoType.Gold:
                if (value == BattleManager.Inst.CurrentGold)
                    return;
                text.text = $"{BattleManager.Inst.CurrentGold}";
                value = BattleManager.Inst.CurrentGold;
                DoInfoPanel();
                break;
            case InfoType.Difficulty:
                // int level = BattleManager.Inst.GetPlayerItemDifficultyData().Level;
                // if (value == level)
                //     return;
                // _canvasGroup.alpha = level > 0 ? 1 : 0;
                // value = level;
                // if (level > 0)
                //     StartCoroutine(WaitDoDifficulty(level));
                break;
            case InfoType.Progress:
                if (BattleManager.Inst.RuntimeData.LevelStructData.LevelStructType != LevelStructType.Chapter)
                {
                    gameObject.SetActive(false);
                    return;
                }

                if (BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData runtimeData)
                {
                    int current = runtimeData.ClearRoomNumber + 2;
                    if (value == current)
                        return;
                    ChapterStructData cpData = runtimeData.LevelStructData as ChapterStructData;
                    int roomvalue = 0;
                    for (int i = 0; i < runtimeData.CurrentLevelIndex; i++)
                    {
                        roomvalue += cpData.RoomList[i].Count;
                    }
                    text.text = $"{current-roomvalue} / {cpData.RoomList[runtimeData.CurrentLevelIndex].Count}";
                    value = current;
                    DoInfoPanel();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    IEnumerator WaitDoDifficulty(int level)
    {
        yield return new WaitForSeconds(2f);
        DoDifficulty(level);
    }
    
    void DoDifficulty(int level)
    {
        var data = DataManager.Inst.GetDifficultyDataByLevel(level);
        DifficultyText.gameObject.SetActive(true);
        var difficultyText = DifficultyText.GetComponent<Text>();
        difficultyText.color = new Color(1,1,1,0);
        difficultyText.DOColor(Color.white, 0.3f);
        DifficultyText.text = data.Desc;
        var iconGo = GameObject.Instantiate(DifficultyIconPrefab, DifficultyIconPrefab.transform.parent.parent);
        iconGo.gameObject.SetActive(true);
        iconGo.transform.position = DifficultyIconPrefab.transform.position;
        var levelNumber = iconGo.transform.GetChild(0).GetComponent<Text>();
        levelNumber.text = level.ToString();
        CanvasGroup iconCanvas = iconGo.GetComponent<CanvasGroup>();
        iconCanvas.alpha = 0;
        iconCanvas.DOFade(1, 0.3f);
        difficultyText.DOColor(new Color(1,1,1,0), 0.3f).SetDelay(1f).Delay();
        var iconPrefab = transform.GetChild(2);
         
        iconGo.transform.DOScale(Vector3.one * 2f, 0.3f).SetEase(_curve).OnComplete(() =>
        {
            iconGo.transform.DOScale(Vector3.one * 0.2f, 1.3f).SetDelay(0.5f).Delay();
            
            iconGo.transform.DOMove(iconPrefab.position, 1.3f).OnComplete(() =>
            {
                GameObject.Destroy(iconGo.gameObject);
                text.text =level.ToString();
                DoInfoPanel();
            }).SetDelay(0.5f).Delay();
        });
        
    }

    [SerializeField]
    private AnimationCurve _curve;
    void DoInfoPanel()
    {
        DOTween.Kill(transform,true);
        transform.DOScale(Vector3.one*1.2f, 0.3f).SetEase(_curve);
    }
    
}
