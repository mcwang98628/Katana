using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UI_MainPanel_IllustrationsPanel : MonoBehaviour
{
    [SerializeField]
    private UI_MainPanel_IllustrationsPanel_ItemPrefab itemPrefab;
    [SerializeField]
    private ScrollRect scroller; //滚动组件

    [SerializeField]
    private CanvasGroup btnGroup;
    [SerializeField]
    private CanvasGroup illustrationsPanelGroup;

    [SerializeField] private Transform group;

    [SerializeField]
    private Image illustrationsPanelIcon;
    [SerializeField]
    private UIText illustrationsPanelName;
    [SerializeField]
    private Text illustrationsPanelNumber;
    [SerializeField]
    private Text monsterBtnNumber;
    [SerializeField]
    private Text arifactBtnNumber;
    [SerializeField]
    private Text itemSchoolBtnNumber;

    [SerializeField]
    private Sprite monsterSprite;
    [SerializeField]
    private Sprite artifactSprite;


    [SerializeField]
    private GameObject enemyRedPoint;
    [SerializeField]
    private GameObject itemRedPoint;
    [SerializeField]
    private GameObject itemBuildRedPoint;
    
    
    List<UI_MainPanel_IllustrationsPanel_ItemPrefab> _prefabs = new List<UI_MainPanel_IllustrationsPanel_ItemPrefab>();
    
    
    
    public void OnBackBtnClick()
    {
        illustrationsPanelGroup.interactable = false;
        illustrationsPanelGroup.DOFade(0, 0.3f).OnComplete(() =>
        {
            btnGroup.gameObject.SetActive(true);
            illustrationsPanelGroup.gameObject.SetActive(false);
            btnGroup.DOFade(1, 0.3f).OnComplete(() =>
            {
                btnGroup.interactable = true;
            });
        });
    }

    void showIllustPanel()
    {
        btnGroup.interactable = false;
        btnGroup.DOFade(0, 0.3f).OnComplete(() =>
        {
            btnGroup.gameObject.SetActive(false);
            illustrationsPanelGroup.gameObject.SetActive(true);
            illustrationsPanelGroup.DOFade(1, 0.3f).OnComplete(() =>
            {
                illustrationsPanelGroup.interactable = true;
            });
        });
    }

    private void OnEnable()
    {
        initBtnNumber();
        EventManager.Inst.DistributeEvent(TGANames.MainPanelOpenArtifactPanel);
    }

    private float _timer = 1;
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 1f)
        {
            _timer -= 1f;
            UpdateRedPoint();
        }
    }

    void initBtnNumber()
    {
        monsterBtnNumber.text = getEnemyCountStr();
        arifactBtnNumber.text = getItemCountStr();
        itemSchoolBtnNumber.text = getItemSchoolStr();
    }
    public void OnMonsterBtnClick()
    {
        showIllustPanel();
        InitAllEnemy();
        illustrationsPanelIcon.sprite = monsterSprite;
        illustrationsPanelName.text = "Monster";
        illustrationsPanelNumber.text = getEnemyCountStr();
        EventManager.Inst.DistributeEvent(TGANames.MainPanelOpenMonsterArtifactPanel);
    }

    public void OnArifactBtnClick()
    {
        showIllustPanel();
        InitAllItem();
        illustrationsPanelIcon.sprite = artifactSprite;
        illustrationsPanelName.text = "Artifact";
        illustrationsPanelNumber.text = getItemCountStr();
        EventManager.Inst.DistributeEvent(TGANames.MainPanelOpenItemArtifactPanel);
    }

    public void OnItemBuildBtnClick()
    {
        showIllustPanel();
        InitItemBuild();
        illustrationsPanelIcon.sprite = artifactSprite;
        illustrationsPanelName.text = "UI_MainPanel_ItemBuild";
        illustrationsPanelNumber.text = getItemSchoolStr();
        EventManager.Inst.DistributeEvent(TGANames.MainPanelOpenItemBuildArtifactPanel);
    }

    List<EnemyInfoData> currentShowEnemy = new List<EnemyInfoData>();
    void InitAllEnemy()
    {
        currentShowEnemy.Clear();
        List<EnemyInfoData> allEnemy = new List<EnemyInfoData>();
        foreach (EnemyInfoData enemyInfoData in DataManager.Inst.EnemyDatas.Values)
        {
            if (!enemyInfoData.ShowInAtlas)
            {
                continue;
            }
            allEnemy.Add(enemyInfoData);
        }

        currentShowEnemy.AddRange(allEnemy);
        for (int i = 0; i < _prefabs.Count; i++)
        {
            Destroy(_prefabs[i].gameObject);
        }
        _prefabs.Clear();
        for (int i = 0; i < currentShowEnemy.Count; i++)
        {
            var prefabgo = GameObject.Instantiate(itemPrefab,group);
            prefabgo.Init(currentShowEnemy[i]);
            _prefabs.Add(prefabgo);
        }
        
        scroller.DOVerticalNormalizedPos(1, 0.3f);
    }

    void initEnemy(GameObject go,int index)
    {
        go.GetComponent<UI_MainPanel_IllustrationsPanel_ItemPrefab>().Init(currentShowEnemy[index]);
    }
    
    List<ItemScriptableObject> currentShowItems = new List<ItemScriptableObject>();
    void InitAllItem()
    {
        currentShowItems.Clear();
        foreach (var itemScr in DataManager.Inst.AllItemObj)
        {
            if (itemScr.Value.ItemType == ItemType.Artifact)
            {
                currentShowItems.Add(itemScr.Value);
            }
        }
        
        
        for (int i = 0; i < _prefabs.Count; i++)
        {
            Destroy(_prefabs[i].gameObject);
        }
        _prefabs.Clear();
        for (int i = 0; i < currentShowItems.Count; i++)
        {
            var prefabgo = GameObject.Instantiate(itemPrefab,group);
            prefabgo.Init(currentShowItems[i]);
            _prefabs.Add(prefabgo);
        }
        
        scroller.DOVerticalNormalizedPos(1, 0.3f);
    }

    void AddListNullObject<T>(List<T> list) where T : class
    {
        if (list.Count > 0)
        {
            int count = 4 - (list.Count % 4);
            for (int i = 0; i < count; i++)
            {
                list.Add(null);
            }
        }
    }

    void initItem(GameObject go,int index)
    {
        go.GetComponent<UI_MainPanel_IllustrationsPanel_ItemPrefab>().Init(currentShowItems[index]);
    }

    string getEnemyCountStr()
    {
        int maxEnemyCount = 0;
        int currentEnemyCount = 0;
        foreach (EnemyInfoData enemyInfoData in DataManager.Inst.EnemyDatas.Values)
        {
            if (enemyInfoData.ShowInAtlas)
            {
                maxEnemyCount++;
            }
        }

        foreach (KeyValuePair<int,int> killEnemyData in ArchiveManager.Inst.ArchiveData.StatisticsData.KillEnemys)
        {
            if (DataManager.Inst.EnemyDatas[killEnemyData.Key].ShowInAtlas)
            {
                currentEnemyCount++;
            }
        }

        return $"{currentEnemyCount}/{maxEnemyCount}";
    }

    string getItemCountStr()
    {
        List<ItemScriptableObject> items = new List<ItemScriptableObject>();
        foreach (var itemScr in DataManager.Inst.AllItemObj)
        {
            if (itemScr.Value.ItemType == ItemType.Artifact)
            {
                items.Add(itemScr.Value);
            }
        }

        int maxItemCount = items.Count;
        int unLockItemCount = 0;
        for (int i = 0; i < ArchiveManager.Inst.ArchiveData.StatisticsData.GetItemList.Count; i++)
        {
            int id = ArchiveManager.Inst.ArchiveData.StatisticsData.GetItemList[i];
            var item = DataManager.Inst.GetItemScrObj(id);
            if (item == null)
            {
                continue;
            }
            if (item.ItemType == ItemType.Artifact)
            {
                unLockItemCount++;
            }
        }

        return $"{unLockItemCount}/{maxItemCount}";
    }

    string getItemSchoolStr()
    {
        return $"{ArchiveManager.Inst.ArchiveData.StatisticsData.UnlockItemSchool.Count}/{DataManager.Inst.ItemSchool.Count}";
    }
    
    void InitItemBuild()
    {
        for (int i = 0; i < _prefabs.Count; i++)
        {
            Destroy(_prefabs[i].gameObject);
        }
        _prefabs.Clear();
        foreach (var itemSchool in DataManager.Inst.ItemSchool)
        {
            var prefabgo = GameObject.Instantiate(itemPrefab,group);
            prefabgo.Init(itemSchool.Value);
            _prefabs.Add(prefabgo);
        }
        for (int i = 0; i < currentShowItems.Count; i++)
        {
        }
        scroller.DOVerticalNormalizedPos(1, 0.3f);
    }

    void UpdateRedPoint()
    {
        enemyRedPoint.SetActive(ArchiveManager.Inst.EnemyIllustratedCanGetReceive());
        itemRedPoint.SetActive(ArchiveManager.Inst.ItemIllustratedCanGetReceive());
        itemBuildRedPoint.SetActive(ArchiveManager.Inst.ItemBuildIllustratedCanGetReceive());
    }

    
    
}
