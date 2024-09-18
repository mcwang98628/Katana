using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_EventPanel_EventPrefab : MonoBehaviour
{
    [SerializeField]
    private UIText eventName;
    [SerializeField]
    private UIText eventDesc;
    [SerializeField]
    private Text timeText;

    public EventData EventData => _eventData;
    private EventData _eventData;
    public void Init(EventData eventData)
    {
        _eventData = eventData;
        gameObject.SetActive(true);
    }
    
    public void OnEnterBtnClick()
    {
        BattleManager.Inst.LoadEndlessBattle(EventData.Id);
    }

}
