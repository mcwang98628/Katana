using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventManager:TSingleton<EventManager>
{

    Dictionary<string,Action<string,object>> _events = new Dictionary<string, Action<string,object>>();
    List<Action<GameObject,string>> _animatorEvents = new List<Action<GameObject,string>>();


    public bool HasEvent(string eventName)
    {
        return _events.ContainsKey(eventName);
    }
    
    /// <summary>
    /// 监听事件(游戏物体销毁时需要ReMove_Event)
    /// </summary>
    /// <param name="eventName">事件名字</param>
    /// <param name="callBack">事件</param>
    public void AddEvent(string eventName,Action<string,object> callBack)
    {
        if (!_events.ContainsKey(eventName))
        {
            _events.Add(eventName,delegate(string eventname,object o) {  });
        }

        _events[eventName] += callBack;
    }

    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="eventName">事件名字</param>
    /// <param name="callBack">事件</param>
    public void RemoveEvent(string eventName,Action<string,object> callBack)
    {
        if (_events.ContainsKey(eventName))
        {
            // ReSharper disable once DelegateSubtraction
            _events[eventName] -= callBack;
        }

    }

    /// <summary>
    /// 派发事件
    /// </summary>
    /// <param name="eventName">事件名字</param>
    /// <param name="value">事件参数(默认为null)</param>
    public void DistributeEvent(string eventName,object value = null)
    {
        if (!_events.ContainsKey(eventName))
        {
            return;
        }

        _events[eventName](eventName,value);
    }


    public void AddAnimatorEvent(Action<GameObject,string> callback)
    {
        _animatorEvents.Add(callback);
    }

    public void RemoveAnimatorEvent(Action<GameObject,string> callback)
    {
        if (!_animatorEvents.Contains(callback))
        {
            return;
        }
        _animatorEvents.Remove(callback);
    }

    public void DistributeAnimatorEvent(GameObject _gameObject, string eventName)
    {
        for (int i = 0; i < _animatorEvents.Count; i++)
        {
            _animatorEvents[i](_gameObject,eventName);
        }
    }
}

