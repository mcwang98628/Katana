using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoUIPanelLayout : MonoBehaviour
{
    void Start()
    {
        var _canvasScaler = transform.GetComponent<CanvasScaler>();
        
        if (_canvasScaler != null)
        {
            float value = (float) Screen.width / (float) Screen.height; 
            _canvasScaler.matchWidthOrHeight = value>0.57f ? 1 : 0;
        }
    }
}
