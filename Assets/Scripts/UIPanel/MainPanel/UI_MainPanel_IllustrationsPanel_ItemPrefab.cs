using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_IllustrationsPanel_ItemPrefab : MonoBehaviour
{
    [SerializeField]
    private Image colorBg;
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject redPoint;

    private ItemScriptableObject _itemobj;
    private EnemyInfoData _enemyInfo;
    private ItemSchoolData? _schoolData;
    
    
    public void Init(ItemScriptableObject item)
    {
        this.gameObject.SetActive(true);
        _itemobj = item;
        _enemyInfo = null;
        if (item == null)
        {
            colorBg.gameObject.SetActive(false);
            itemIcon.gameObject.SetActive(false);
            button.interactable = false;
            return;
        }
        else
        {
            colorBg.gameObject.SetActive(true);
            itemIcon.gameObject.SetActive(true);
        }
        int itemId = DataManager.Inst.GetItemId(item);
        bool isUnLock = ArchiveManager.Inst.ArchiveData.StatisticsData.GetItemList.Contains(itemId);
        bool isCanGetReceive = !ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveItemIllustrated.Contains(itemId) && isUnLock;
        redPoint.SetActive(isCanGetReceive);
        setIsUnLock(isUnLock);
        itemIcon.sprite = _itemobj.Icon;

        colorBg.color = new Color(0.15f,0.15f,0.15f);
        
    }
    
    private string enemyIconPath = "Assets/Arts/Textures/UISprites/EnemyIcon/{0}.png";
    public void Init(EnemyInfoData enemyInfo)
    {
        this.gameObject.SetActive(true);
        _enemyInfo = enemyInfo;
        _itemobj = null;
        if (enemyInfo == null)
        {
            colorBg.gameObject.SetActive(false);
            itemIcon.gameObject.SetActive(false);
            button.interactable = false;
            return;
        }
        bool isUnLock = ArchiveManager.Inst.ArchiveData.StatisticsData.KillEnemys.ContainsKey(enemyInfo.EnemyID);
        setIsUnLock(isUnLock);
        bool isCanGetReceive = !ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveEnemyIllustrated.Contains(enemyInfo.EnemyID) && isUnLock;
        redPoint.SetActive(isCanGetReceive);
        ResourcesManager.Inst.GetAsset<Sprite>(string.Format(enemyIconPath, enemyInfo.EnemyIcon),
            delegate(Sprite sprite)
            {
                itemIcon.sprite = sprite;
            });
       
        colorBg.color = new Color(0.15f,0.15f,0.15f);
    }



    private string itemSchoolIconPath = "Assets/Arts/Textures/UISprites/BuildIcon/{0}.png";
    public void Init(ItemSchoolData itemSchoolData)
    {
        this.gameObject.SetActive(true);
        colorBg.color = new Color(0.15f,0.15f,0.15f);
        _schoolData = itemSchoolData;
        ResourcesManager.Inst.GetAsset<Sprite>(string.Format(itemSchoolIconPath, itemSchoolData.Icon),
            delegate(Sprite sprite)
            {
                itemIcon.sprite = sprite;
            });
        bool isUnlock = ArchiveManager.Inst.ArchiveData.StatisticsData.UnlockItemSchool.Contains(itemSchoolData.Id);
        bool isCanGetReceive = !ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveItemSchoolIllustrated.Contains(itemSchoolData.Id) && isUnlock;
        redPoint.SetActive(isCanGetReceive);
        setIsUnLock(isUnlock);
    }


    private bool _isUnlock;
    void setIsUnLock(bool isUnLock)
    {
        _isUnlock = isUnLock;
        if (isUnLock)
        {
            itemIcon.color = new Color(1,1,1,1);
            // itemIcon.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            itemIcon.color = new Color(1,1,1,0.05f);
            // itemIcon.transform.GetChild(0).gameObject.SetActive(true);
        }
        // button.interactable = isUnLock;
        
    }
    public void OnBtnClick()
    {
        if (_itemobj!=null)
        {
            UIManager.Inst.Open("InfoPanel",false,_itemobj);
            int itemId = DataManager.Inst.GetItemId(_itemobj);
            bool isCanGetReceive = !ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveItemIllustrated.Contains(itemId);
            if (isCanGetReceive && _isUnlock)
            {
                ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveItemIllustrated.Add(itemId);
                ArchiveManager.Inst.ChangeSoul(10);
                ArchiveManager.Inst.SaveArchive();
                redPoint.SetActive(false);
            }
        }else if (_enemyInfo!=null)
        {
            UIManager.Inst.Open("InfoPanel",false,_enemyInfo);
            bool isCanGetReceive = !ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveEnemyIllustrated.Contains(_enemyInfo.EnemyID);
            if (isCanGetReceive && _isUnlock)
            {
                ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveEnemyIllustrated.Add(_enemyInfo.EnemyID);
                ArchiveManager.Inst.ChangeSoul(10);
                ArchiveManager.Inst.SaveArchive();
                redPoint.SetActive(false);
            }
        }else if (_schoolData != null)
        {
            UIManager.Inst.Open("InfoPanel",false,_schoolData);
            bool isCanGetReceive = !ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveItemSchoolIllustrated.Contains(_schoolData.Value.Id);
            if (isCanGetReceive && _isUnlock)
            {
                ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveItemSchoolIllustrated.Add(_schoolData.Value.Id);
                ArchiveManager.Inst.ChangeSoul(10);
                ArchiveManager.Inst.SaveArchive();
                redPoint.SetActive(false);
            }
        }
    }
}
