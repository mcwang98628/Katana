using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusGuide : MonoBehaviour
{
    public static FocusGuide Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
    }

    public enum GuideType
    {
        Rune,Hero,Event,Equipment
    }

    List<FocusGuideData> currentGuideDatas;
    private int currentGuideIndex;
    public void StartGuide(GuideType guideType)
    {
        currentGuideIndex = 0;
        switch (guideType)
        {
            case GuideType.Equipment:
                currentGuideDatas = GetEquipmentGuide();
                break;
            case GuideType.Hero:
                currentGuideDatas = GetHeroGuide();
                break;
        }

        DoGuide(currentGuideDatas[currentGuideIndex]);
    }

    public void DoGuide(FocusGuideData guideData)
    {
        TimeManager.Inst.SetTimeScale(0.1f);
        var go = GameObject.Find(guideData.TriggerName);

        if (go == null)
            return;
        
        
        switch (guideData.CurrentTriggerType)
        {
            case FocusGuideData.TriggerType.Toggle:
                _toggle = go.GetComponent<Toggle>();
                if (!_toggle)
                {
                    _toggle = go.GetComponentInParent<Toggle>();
                }
                _toggle.onValueChanged.AddListener(OnToggle);
                break;
            case FocusGuideData.TriggerType.Button:
                _button = go.GetComponent<Button>();
                _button.onClick.AddListener(OnButton);
                break; 
            case FocusGuideData.TriggerType.Wait:
                float waitTime = float.Parse(guideData.TriggerName);
                StartCoroutine(WaitFunc(waitTime));
                break; 
        }

        if (go != null)
            UIManager.Inst.FocusMask.Focus(go.GetComponent<RectTransform>());
    }

    IEnumerator WaitFunc(float time)
    {
        TimeManager.Inst.SetTimeScale(1f);
        UIManager.Inst.FocusMask.ShowClickMask(0,Vector2.zero);
        yield return new WaitForSecondsRealtime(time);
        OnButton();
    }

    private Toggle _toggle;
    private Button _button;
    void OnToggle(bool isOn)
    {
        if (isOn)
        {
            OnButton();
            if (_toggle)
            {
                _toggle.onValueChanged.RemoveListener(OnToggle);
                _toggle = null;
            }
        }
    }

    void OnButton()
    {
        TimeManager.Inst.SetTimeScale(1f);
        currentGuideIndex++;
        UIManager.Inst.FocusMask.Hide();
        if (currentGuideDatas != null && currentGuideIndex<currentGuideDatas.Count)
        {
            StartCoroutine(waitNextDoGuide(() =>
            {
                DoGuide(currentGuideDatas[currentGuideIndex]);
            }));
        }
        
        if (_button)
        {
            _button.onClick.RemoveListener(OnButton);
            _button = null;
        }
        
        TimeManager.Inst.SetTimeScale(1f);
    }

    IEnumerator waitNextDoGuide(Action action)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        action?.Invoke();
    }

    List<FocusGuideData> GetRuneGuide()
    {
        List<FocusGuideData> focusGuideDatas = new List<FocusGuideData>();
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Toggle,"Rune"));
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Button,"GetRuneButton"));
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Wait,"5"));
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Toggle,"Chapter"));
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Button,"StartBtn"));
        return focusGuideDatas;
    }
    // List<FocusGuideData> GetEventGuide()
    // {
    //     List<FocusGuideData> focusGuideDatas = new List<FocusGuideData>();
    //     focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Toggle,"Event"));
    //     focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Button,"EnterBtn"));
    //     return focusGuideDatas;
    // }
    List<FocusGuideData> GetHeroGuide()
    {
        List<FocusGuideData> focusGuideDatas = new List<FocusGuideData>();
        
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Toggle,"Hero"));
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Toggle,"HeroPrefab1"));
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Button,"BuytBtn"));
        // focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Button,"SelectBtn"));
        
        return focusGuideDatas;
    }
    List<FocusGuideData> GetEquipmentGuide()
    {
        List<FocusGuideData> focusGuideDatas = new List<FocusGuideData>();
        
        // focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Toggle,"Equipment"));
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Toggle,"Equipment/Icon"));

        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Button,"Equip0"));
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Button,"EquipBtn"));
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Button,"Equip0"));
        focusGuideDatas.Add(new FocusGuideData(FocusGuideData.TriggerType.Button,"EquipBtn"));
        
        return focusGuideDatas;
    }
    
}

public class FocusGuideData
{
    public enum TriggerType
    {
        Toggle,
        Button,
        Wait,//等待
    }
    
    public TriggerType CurrentTriggerType;
    public string TriggerName; 

    public FocusGuideData(TriggerType triggerType,string triggerName )
    {
        CurrentTriggerType = triggerType;
        TriggerName = triggerName; 
    }
}