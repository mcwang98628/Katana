using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    public string PanelName;
    public void Open()
    {
        UIManager.Inst.Open(PanelName);
    }
}
