using System;
using System.Collections;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [LabelText("新存档")]
    public bool IsNewArchive;

    [LabelText("编辑器模式")]
    public bool EditorMode;

    public static GameManager Inst;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private IndicatorManager indicatorManager;
    [SerializeField]
    private BattleManager battleManager;
    [SerializeField]
    private ScenesManager scenesManager;
    [SerializeField]
    private FeedbackManager feedbackManager;
    [SerializeField]
    private ProcedureManager procedureManager;
    [SerializeField]
    private TimeManager timeManager;
    [SerializeField]
    private InteractManager interactManager;
    [SerializeField]
    private BreakableObjManager breakableObjManager;

    public Volume FogVolume;


    // public void SetFog(int cpId)
    // {
    //     string path = $"Assets/Arts/Misc/LightSettings/Chapter_{cpId}.asset";
    //     ResourcesManager.Inst.GetAsset<VolumeProfile>(path, delegate(VolumeProfile profile)
    //     {
    //         if (profile == null)
    //         {
    //             Debug.LogError(path);
    //             return;
    //         }
    //         FogVolume.profile = profile;
    //     });
    // }
    
    
    public bool IsInit { get; private set; }

#if TEST_MODE
    public bool TestMode;
#endif

    public void Awake()
    {
        Debug.LogError("Awake-------");

#if !UNITY_EDITOR
        EditorMode = false;
#endif
        Inst = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        // SetResolutionFromLevel(3);
        Screen.orientation = ScreenOrientation.Portrait;
        StartCoroutine(initAllManager());
    }


    private void OnDestroy()
    {
#if !UNITY_EDITOR
        ArchiveManager.Inst.SaveArchive();
#endif
    }

    private bool allInitOver =>
        LocalizationManger.Inst.IsInit &&
        DataManager.Inst.IsInit;
    private IEnumerator initAllManager()
    {
        Debug.LogError("InitAllManager");
        ResourcesManager.Inst.EditorMode = EditorMode;
        ResourcesManager.Inst.Init();
        LocalizationManger.Inst.Init();
        timeManager.Init();
        procedureManager.Init();
        uiManager.Init();
        audioManager.Init();
        indicatorManager.Init();
        battleManager.Init();
        scenesManager.Init();
        feedbackManager.Init();
        DataManager.Inst.Init();
        interactManager.Init();
        breakableObjManager.Init();
        ArchiveManager.Inst.Init();

        while (!allInitOver)
        {
            yield return null;
        }

        procedureManager.StartProcedure(new MainSceneProcedure());
        // EventManager.Inst.DistributeEvent(EventName.OnAppStart);
        if (IsNewArchive)
        {
            LocalizationManger.Inst.SetLanguage(SystemLanguage.English);
        }
    }


    /// <summary>
    /// 通过等级设置分辨率
    /// </summary>
    /// <param name="level"></param>
    public void SetResolutionFromLevel(int level)
    {//360,540,720
        int width = 0;
        switch (level)
        {
            case 1:
                width = 360;
                break;

            case 2:
                width = 540;
                break;

            case 3:
                width = 720;
                break;

            default:
                goto case 1;
        }
        Resolution resolution = Screen.currentResolution;

        int height = width * resolution.height / resolution.width;

        SetResolution(width, height);
    }
    /// <summary>
    /// 设置分辨率
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public void SetResolution(int width, int height, int depth = 0)
    {
        // #if !UNITY_STANDALONE
        Screen.SetResolution(width, height, true);
        // #elif UNITY_STANDALONE
        //         Screen.SetResolution(1080, 1920, true);
        // #endif
    }

}
