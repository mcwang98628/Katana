using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AutoAssetsTips : MonoBehaviour
{
    [SerializeField] private RectTransform assetBar;
    [SerializeField] private Transform expPoint;
    [SerializeField] private Transform diamondPoint;
    [SerializeField] private Transform soulPoint;

    [SerializeField] private Sprite soulSprite;
    [SerializeField] private Sprite diamondSprite;
    [SerializeField] private Sprite expSprite;

    [SerializeField] private Image assetIconPrefab;

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnArchiveDiamondChange, OnDiamondChange);
        // EventManager.Inst.AddEvent(EventName.OnArchiveExpChanage, OnExpChange);
        // EventManager.Inst.AddEvent(EventName.OnArchiveSoulChange, OnSoulChange);
        EventManager.Inst.AddEvent(EventName.OnAssetTips, OnAssetTips);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnArchiveDiamondChange, OnDiamondChange);
        // EventManager.Inst.RemoveEvent(EventName.OnArchiveExpChanage, OnExpChange);
        // EventManager.Inst.RemoveEvent(EventName.OnArchiveSoulChange, OnSoulChange);
        EventManager.Inst.RemoveEvent(EventName.OnAssetTips, OnAssetTips);
    }


    private void Start()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return null;
        assetBar.sizeDelta = new Vector2(assetBar.sizeDelta.x, 75 + Screen.safeArea.min.y);
    }


    private void OnSoulChange(string arg1, object arg2)
    {
        int value = (int) arg2;
        if (value <= 0)
        {
            return;
        }

        TweenAssetIcon(soulPoint, soulSprite, GetAssetIconCount(value));
    }

    private void OnExpChange(string arg1, object arg2)
    {
        int value = (int) arg2;
        if (value <= 0)
        {
            return;
        }

        TweenAssetIcon(expPoint, expSprite,  GetAssetIconCount(value));
    }

    private void OnDiamondChange(string arg1, object arg2)
    {
        int value = (int) arg2;
        if (value <= 0)
        {
            return;
        }

        TweenAssetIcon(diamondPoint, diamondSprite,  GetAssetIconCount(value));
    }

    void TweenAssetIcon(Transform poinit, Sprite sprite, int count)
    {
        if (count < 1)
        {
            count = 1;
        }

        for (int i = 0; i < count; i++)
        {
            var iconGo = GameObject.Instantiate(assetIconPrefab, transform);
            iconGo.gameObject.SetActive(true);
            iconGo.sprite = sprite;
            iconGo.transform.localPosition = Vector3.zero;
            iconGo.transform.DOLocalMove(new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0),
                Random.Range(0.2f, 0.4f)).OnComplete(() =>
            {
                iconGo.transform.DOMove(poinit.position, 0.5f).OnComplete(() =>
                {
                    GameObject.Destroy(iconGo.gameObject);
                }).SetDelay(0.5f).Delay();
            });
        }
    }

    int GetAssetIconCount(int assetCount)
    {
        int iconCount=0;
        if (assetCount< 10) iconCount = 1;
        else if (assetCount< 40) iconCount = 5;
        else if (assetCount < 100) iconCount = 10;
        else if (assetCount < 300) iconCount = 20;
        else iconCount = 30;
        return iconCount;
    }

    private void OnAssetTips(string arg1, object arg2)
    {
        AssetTipsData data = (AssetTipsData) arg2;
        
        switch (data.Type)
        {
            case UI_Asset.AssetType.Diamond:
                TweenAssetIcon(diamondPoint.position, data.TargetPoint, diamondSprite, GetAssetIconCount(data.AssetCount));
                break;
            case UI_Asset.AssetType.Soul:
                TweenAssetIcon(soulPoint.position, data.TargetPoint, soulSprite,  GetAssetIconCount(data.AssetCount));
                break;
        }

    }


    void TweenAssetIcon(Vector3 startPoinit, Vector3 targetPoint, Sprite sprite, int count)
    {
        if (count < 1)
        {
            count = 1;
        }

        for (int i = 0; i < count; i++)
        {
            var iconGo = GameObject.Instantiate(assetIconPrefab, transform);
            iconGo.transform.position = startPoinit;
            iconGo.gameObject.SetActive(true);
            iconGo.sprite = sprite;
            iconGo.gameObject.GetComponent<AssetIcon>().Init(targetPoint,0.2f,2000);
            
        }
    }
}

public class AssetTipsData
{
    public UI_Asset.AssetType Type;
    public Vector3 TargetPoint;
    public int AssetCount;
}