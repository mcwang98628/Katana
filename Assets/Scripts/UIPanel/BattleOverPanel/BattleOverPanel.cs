using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class BattleOverPanel : PanelBase
{
    // private static int backPressed = 0;
    [SerializeField] [BoxGroup("Audio")] private AudioClip numberAudio;
    [SerializeField] [BoxGroup("Audio")] private AudioClip numberOverAudio;
    [SerializeField] [BoxGroup("Audio")] private AudioClip winAudio;
    [SerializeField] [BoxGroup("Audio")] private AudioClip failAudio;


    [SerializeField] [BoxGroup("Title")] private Image TitleBg;
    [SerializeField] [BoxGroup("Title")] private Image TitleIcon;
    [SerializeField] [BoxGroup("Title")] private UIText TitleText;

    [SerializeField] [BoxGroup("Title")] private Color WinTitleBgColor;
    [SerializeField] [BoxGroup("Title")] private Color DeadTitleBgColor;
    [SerializeField] [BoxGroup("Title")] private Color WinTitleIconColor;
    [SerializeField] [BoxGroup("Title")] private Color DeadTitleIconColor;


    [SerializeField] [BoxGroup("Item")] private BattleOverPanel_ItemPrefab ItemPrefab;
    [SerializeField] [BoxGroup("Item")] private Transform ItemPrefabGroup;

    [SerializeField] [BoxGroup("Text")] private Text EnemyText;
    [SerializeField] [BoxGroup("Text")] private Text BossText;
    [SerializeField] [BoxGroup("Text")] private Text LevelText;
    [SerializeField] [BoxGroup("Text")] private Text ChapterText;
    [SerializeField] [BoxGroup("Text")] private Text UpdateText;

    [SerializeField] [BoxGroup("Text")] private CanvasGroup EnemyAsset;
    [SerializeField] [BoxGroup("Text")] private CanvasGroup BossAsset;
    [SerializeField] [BoxGroup("Text")] private CanvasGroup LevelAsset;
    [SerializeField] [BoxGroup("Text")] private CanvasGroup ChapterAsset;
    [SerializeField] [BoxGroup("Text")] private CanvasGroup UpdateAsset;

    [SerializeField] [BoxGroup("Text")] private Text EnemySoulText;
    [SerializeField] [BoxGroup("Text")] private Text EnemyDiamondText;
    [SerializeField] [BoxGroup("Text")] private Text BossSoulText;

    [SerializeField] [BoxGroup("Text")] private Text BossDiamondText;

    // [SerializeField] [BoxGroup("Text")] private Text LevelSoulText;
    [SerializeField] [BoxGroup("Text")] private Text LevelDiamondText;
    [SerializeField] [BoxGroup("Text")] private Text ChapterSoulText;
    [SerializeField] [BoxGroup("Text")] private Text ChapterDiamondText;
    [SerializeField] [BoxGroup("Text")] private Text UpdateSoulText;
    [SerializeField] [BoxGroup("Text")] private Text UpdateExpText;

    [SerializeField] [BoxGroup("Score")] private Image ScoreBg;
    [SerializeField] [BoxGroup("Score")] private Text ScoreText;

    [SerializeField] private CanvasGroup BackBtn;
    // [SerializeField] private CanvasGroup EndBtn;


    [SerializeField] [BoxGroup("DoPanel")] private Transform Title;
    [SerializeField] [BoxGroup("DoPanel")] private Transform Items;

    [SerializeField] [BoxGroup("DoPanel")] private Transform Score1;
    [SerializeField] [BoxGroup("DoPanel")] private Transform Score2;


    List<BattleOverPanel_ItemPrefab> itemPrefabs = new List<BattleOverPanel_ItemPrefab>();


    public void OnOpen(GameOverData data)
    {
        if (data.isVictory)
        {
            data.Progress = 100;
        }

        InitTitle(data.isVictory);
        InitTexts(data);
        InitScore(data.Progress);
        // InitItem();
        InitEquipment();
        DoPanel(data.Progress, data.isVictory);
    }

    private void InitTitle(bool isWin)
    {
        if (isWin)
        {
            TitleBg.color = WinTitleBgColor;
            TitleIcon.color = WinTitleIconColor;
        }
        else
        {
            TitleBg.color = DeadTitleBgColor;
            TitleIcon.color = DeadTitleIconColor;
        }

        TitleText.text = $"{LocalizationManger.Inst.GetText("Finish")} {0}%";
    }


    private void InitTexts(GameOverData data)
    {
        EnemyText.text = $"{LocalizationManger.Inst.GetText("DefeatEnemies")} {data.KillEnemyNumber}";
        BossText.text = $"{LocalizationManger.Inst.GetText("DefeatBoss")} {data.KillSEnemyNumber}";
        LevelText.text = $"{LocalizationManger.Inst.GetText("PassLevel")} {data.LevelIndex + (data.isVictory ? 1 : 0)}";
        ChapterText.text = $"{LocalizationManger.Inst.GetText("Clearance")} {(data.isVictory ? 1 : 0)}";

        UpdateText.text = $"{LocalizationManger.Inst.GetText("NewRecord")}";

        // UIText backText = BackBtn.GetComponentInChildren<UIText>();
        // if (backPressed < 1)
        // {
        //     backText.text = $"{LocalizationManger.Inst.GetText("StartOver")} {0}%";
        // }
        // else
        // {
        //     backText.text = $"{LocalizationManger.Inst.GetText("Quit")} {0}%";
        // }

        // EnemySoulText.text = "+" + data.KillEnemySoul.ToString();
        // EnemyDiamondText.text = "+" + data.KillEnemyDiamond.ToString();
        // BossSoulText.text = "+" + data.KillSEnemySoul.ToString();
        // BossDiamondText.text = "+" + data.KillSEnemyDiamond.ToString();
        // LevelDiamondText.text = "+" + data.LevelDiamond.ToString();
        // ChapterSoulText.text = "+" + data.BattleWinSoul.ToString();
        // ChapterDiamondText.text = "+" + data.BattleWinDiamond.ToString();
        // UpdateSoulText.text = "+" + data.SoulValue.ToString();
        // UpdateExpText.text = "+" + data.Exp.ToString();

        if (data.isVictory)
        {
            ChapterText.color = new Color(1, 0.5f, 0);
        }
        else
        {
            ChapterText.color = Color.grey;
        }

        // if (data.Exp > 0 || data.SoulValue > 0)
        // {
        //     UpdateText.color = new Color(1, 1f, 0);
        // }
        // else
        // {
        //     UpdateText.color = Color.grey;
        // }

    }

    private void InitScore(int progress)
    {
        if (progress < 19)
        {
            ScoreText.text = "E";
        }
        else if (progress < 39)
        {
            ScoreText.text = "D";
        }
        else if (progress < 59)
        {
            ScoreText.text = "C";
        }
        else if (progress < 79)
        {
            ScoreText.text = "B";
        }
        else if (progress < 99)
        {
            ScoreText.text = "A";
        }
        else
        {
            ScoreText.text = "S";
        }
    }

    // private void InitItem()
    // {
    //     var items = BattleManager.Inst.CurrentPlayer.roleItemController.Items;
    //     for (int i = 0; i < items.Count; i++)
    //     {
    //         if (items[i].ItemType != ItemType.Artifact)
    //         {
    //             continue;
    //         }
    //
    //         var itemGo = GameObject.Instantiate(ItemPrefab, ItemPrefabGroup);
    //         itemGo.Init(items[i]);
    //         itemPrefabs.Add(itemGo);
    //     }
    // }

    private void InitEquipment()
    {
        List<Equipment> equipList = ArchiveManager.Inst.ArchiveData.TemporaryData.EquipmentList;
        for (int i = 0; i < equipList.Count; i++)
        {
            var itemGo = GameObject.Instantiate(ItemPrefab, ItemPrefabGroup);
            itemGo.Init(equipList[i]);
            itemPrefabs.Add(itemGo);
        }
        // var diamGo = GameObject.Instantiate(ItemPrefab, ItemPrefabGroup);
        // int diamondValue = ArchiveManager.Inst.ArchiveData.TemporaryData.AddDiamondValue;
        // diamGo.Init(diamondValue);
        // itemPrefabs.Add(diamGo);
    }


public void OnBackBtnClick()
{
    UIManager.Inst.ShowMask(() =>
    {
        // UIManager.Inst.HideMask(null);
        TimeManager.Inst.SetTimeScale(1f);
        UIManager.Inst.Close("BattleOverPanel");
        ProcedureManager.Inst.StartProcedure(new MainSceneProcedure());
        UIManager.Inst.HideMask(null);
    });
    // if (backPressed < 1)
    // {
    //    
    // }
    // else
    // {
    //     Application.Quit();
    // }
    // backPressed++;
}

public void OnEndBtnClick()
{
    Application.Quit();
}

    //Animator-----------

    private void DoPanel(int progress, bool isVectory)
    {
        StartCoroutine(doPanel(progress, isVectory));
    }

    IEnumerator doPanel(int progress, bool isVectory)
    {
        float timeScale = isVectory ? 1 : 0.5f;
        yield return new WaitForSecondsRealtime(0.5f * timeScale);
        DoTitleText(progress);
        //  Title.DOLocalMoveY(372, 0.4f * timeScale);
        //
        //  Score1.DOLocalMoveY(372, 0.4f * timeScale)
        //      .OnComplete(() => { Score1.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0.2f), 0.5f * timeScale); });
        //
        //  yield return new WaitForSecondsRealtime(0.4f * timeScale);
        //  DoTitleText(progress);
        //  yield return new WaitForSecondsRealtime(1f * timeScale);
        //  Items.DOLocalMoveY(196.64f, 0.4f * timeScale);
        //  yield return new WaitForSecondsRealtime(0.4f * timeScale);
        ShowItem();
        //  yield return new WaitForSecondsRealtime(0.3f * timeScale);
        //  EnemyText.transform.DOLocalMoveY(90, 0.4f * timeScale);
        //  yield return new WaitForSecondsRealtime(0.1f * timeScale);
        //  BossText.transform.DOLocalMoveY(40, 0.4f * timeScale);
        //  yield return new WaitForSecondsRealtime(0.1f * timeScale);
        //  LevelText.transform.DOLocalMoveY(-10, 0.4f * timeScale);
        //  yield return new WaitForSecondsRealtime(0.1f * timeScale);
        //  ChapterText.transform.DOLocalMoveY(-60, 0.4f * timeScale);
        //  yield return new WaitForSecondsRealtime(0.1f * timeScale);
        //  UpdateText.transform.DOLocalMoveY(-110, 0.4f * timeScale);
        //  yield return new WaitForSecondsRealtime(2f * timeScale);
        //
        //  EnemyText.DOColor(new Color(1, 1, 1, 0), 0.3f);
        //  BossText.DOColor(new Color(1, 1, 1, 0), 0.3f);
        //  LevelText.DOColor(new Color(1, 1, 1, 0), 0.3f);
        //  ChapterText.DOColor(new Color(1, 1, 1, 0), 0.3f);
        //  UpdateText.DOColor(new Color(1, 1, 1, 0), 0.3f);
        //  EnemyAsset.DOFade(1, 0.3f);
        //  BossAsset.DOFade(1, 0.3f);
        //  LevelAsset.DOFade(1, 0.3f);
        //  ChapterAsset.DOFade(1, 0.3f);
        //  UpdateAsset.DOFade(1, 0.3f);
        //
        // Score1.DOLocalMoveY(372, 0.4f);
        //  Score2.DOLocalMoveY(-224, 0.4f * timeScale);
        if (progress >= 100)
        {
            AudioManager.Inst.PlaySource(winAudio);
        }
        else
        {
            AudioManager.Inst.PlaySource(failAudio);
        }
        //
        // yield return new WaitForSecondsRealtime(2f * timeScale);
        // BackBtn.interactable = true;
        // BackBtn.DOFade(1, 0.3f * timeScale);
    }

    private void DoTitleText(int progress)
    {
        int number = 0;
        var numberAudioSource = AudioManager.Inst.PlaySource(numberAudio);

        DOTween.To(() => number, value =>
        {
            number = value;
            TitleText.text = $"{LocalizationManger.Inst.GetText("Finish")} {number}%";
        }, progress, 0.5f).OnComplete(() =>
        {
            if (numberAudioSource.clip == numberAudio && numberAudioSource.isPlaying)
            {
                AudioManager.Inst.RecycleAudio(numberAudioSource);
            }

            AudioManager.Inst.PlaySource(numberOverAudio);
        });
    }


    private void ShowItem()
    {
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            itemPrefabs[i].Show(i * 0.1f);
        }
    }
}