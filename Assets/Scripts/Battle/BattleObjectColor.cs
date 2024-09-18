using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleObjectColor : MonoBehaviour
{
    [SerializeField] [ColorUsage(true,true)]
    private Color redColor;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private string tagStr;
    
    private void Awake()
    {
        if (BattleManager.Inst.CurrentPlayer.GetTagCount(tagStr) > 0)
            meshRenderer.material.SetColor("_EmissionColor",redColor);
    }
    
    
    
    
    
}
