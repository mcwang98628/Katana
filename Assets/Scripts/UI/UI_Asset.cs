using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Asset : MonoBehaviour
{
    public enum AssetType
    {
        Diamond,
        Soul,
        Fire,
    }
    
    [SerializeField]
    private Text text;
    [SerializeField]
    private AssetType type;
    [SerializeField]
    private Text addNumberText;

    private float _timer = 1;
    private void Update()
    {
        if (ArchiveManager.Inst.ArchiveData == null)
        {
            return;
        }
        UpdateAssetText();
    }

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnArchiveDiamondChange,OnChangeDiamond);
        EventManager.Inst.AddEvent(EventName.OnArchiveSoulChange,OnChangeSoul);
        EventManager.Inst.AddEvent(EventName.OnArchiveFireChange,OnChangeFire);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnArchiveDiamondChange,OnChangeDiamond);
        EventManager.Inst.RemoveEvent(EventName.OnArchiveSoulChange,OnChangeSoul);
        EventManager.Inst.RemoveEvent(EventName.OnArchiveFireChange,OnChangeFire);
    }

    private void OnChangeDiamond(string arg1, object arg2)
    {
        if (type != AssetType.Diamond)
            return;
        
        ShowNumberTips((int)arg2);
    }

    private void OnChangeSoul(string arg1, object arg2)
    {
        if (type != AssetType.Soul)
            return;

        ShowNumberTips((int)arg2);
    }

    private void OnChangeFire(string arg1, object arg2)
    {
        if (type != AssetType.Fire)
            return;

//        ShowNumberTips((int)arg2);
    }

    void ShowNumberTips(int num)
    {
        if (num<=0)
            return;
        
        
        var numText = GameObject.Instantiate(addNumberText, transform);
        numText.gameObject.SetActive(true);
        numText.text = "+ " + num;
        numText.transform.localPosition = new Vector3(20, -70, 0);
        numText.transform.DOLocalMoveY(0, 0.5f).SetDelay(3f).Delay();
        numText.DOColor(new Color(1, 1, 1, 0), 0.5f).OnComplete(() =>
        {
            GameObject.Destroy(numText.gameObject);
        }).SetDelay(1.5f).Delay();
    }


    private int value = 0;
    private void UpdateAssetText()
    {
        var globalData = ArchiveManager.Inst.ArchiveData.GlobalData;
        switch (type)
        {
            case AssetType.Diamond:
                if (value == globalData.Diamond)
                    return;
                text.text = globalData.Diamond.ToString();
                value = globalData.Diamond;
                break;
            case AssetType.Soul:
                if (value == globalData.Soul)
                    return;
                text.text = globalData.Soul.ToString();
                value = globalData.Soul;
                break;
            case AssetType.Fire:
                string str;
                var time = globalData.FireTimeLeft;
                if (globalData.Fire > 0)
                {
                    if (value == globalData.Fire)
                        return;
                    str = $"{globalData.Fire}/{globalData.MaxFire}";
                    text.color = Color.white;
                }
                else
                {
                    TimeSpan ts = new TimeSpan(time*10000000);
                    str = $"{ts.Hours*60 + ts.Minutes}:{ts.Seconds}";
                    text.color = Color.gray;
                }
                text.text = str;
                value = globalData.Fire;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    
    
    
}
