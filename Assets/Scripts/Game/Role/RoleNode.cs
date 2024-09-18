using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class RoleNode : MonoBehaviour
{
    [LabelText("模型")]
    public Transform Model;
    [LabelText("头 骨骼")]
    public Transform Head;
    [LabelText("武器")]
    public List<Transform> Weapon;
    
    public Transform Body;
    
    public Transform Halo;
    
    [SerializeField]
    private List<Renderer> MeshRenderers;

    private static readonly int CoverColor = Shader.PropertyToID("_CoverColor");
    private static readonly int CoverColorAlpha = Shader.PropertyToID("_CoverColorAlpha");
    private static readonly int RimColor = Shader.PropertyToID("_RimColor");
    private static readonly int RimRange = Shader.PropertyToID("Vector1_A6257C58");
    private static readonly int NoiseStep = Shader.PropertyToID("Vector1_581FB15B");
    private static readonly int EdgeColor = Shader.PropertyToID("Color_2C5D1FC7");
    List<RoleColorData> _roleBuffColorDatas = new List<RoleColorData>();
    List<RoleColorData> _roleBuffRimColorDatas = new List<RoleColorData>();

    [LabelText("武器Mesh1")]
    public MeshFilter WeaponMesh1;
    [LabelText("武器Mesh2")]
    public MeshFilter WeaponMesh2;
    private void Update()
    {
        if (_roleBuffColorDatas.Count>0)
        {
            var colorData = _roleBuffColorDatas[_roleBuffColorDatas.Count - 1];
            if (colorData.BuffColorEndTime < Time.time)
            {
                RemoveColor(colorData);
            }
        }
        if (_roleBuffRimColorDatas.Count>0)
        {
            var colorData = _roleBuffRimColorDatas[_roleBuffRimColorDatas.Count - 1];
            if (colorData.BuffColorEndTime < Time.time)
            {
                RemoveRimColor(colorData);
            }
        }
    }

    private List<Tweener> rimTweeners = new List<Tweener>();
    public void Set_RimColor(Color color)
    {
        for (int i = 0; i < rimTweeners.Count; i++)
        {
            rimTweeners[i].Kill(false);
        }
        rimTweeners.Clear();
        
        foreach (Renderer renderer in MeshRenderers)
        {
            if (renderer == null)
            {
                continue;
            }
            var tweener1 = DOTween.To(() => renderer.material.GetColor(RimColor),
                value => renderer.material.SetColor(RimColor, value), color, doTweenerTime); 
            rimTweeners.Add(tweener1); 
        } 
    }
    
    private List<Tweener> rimRangeTweeners = new List<Tweener>();
    public void Set_RimColorRange(float range)
    {
        for (int i = 0; i < rimRangeTweeners.Count; i++)
        {
            rimRangeTweeners[i].Kill(false);
        }
        rimRangeTweeners.Clear();
        
        foreach (Renderer renderer in MeshRenderers)
        {
            if (renderer == null)
            {
                continue;
            }
            var tweener1 = DOTween.To(() => renderer.material.GetFloat(RimRange),
                value => renderer.material.SetFloat(RimRange, value), range, doTweenerTime); 
            rimRangeTweeners.Add(tweener1); 
        }
    }
    
    
    public void Set_RimColor(RoleColorData color)
    {
        Set_RimColor(color.BuffColor);
        Set_RimColorRange(color.Range);
        _roleBuffRimColorDatas.Add(color);
    }
    public void RemoveRimColor(RoleColorData colorData)
    {
        if (!_roleBuffRimColorDatas.Contains(colorData))
        {
            return;
        }
        _roleBuffRimColorDatas.Remove(colorData);
                
        if (_roleBuffRimColorDatas.Count > 0)
        {
            var data = _roleBuffRimColorDatas[_roleBuffRimColorDatas.Count - 1];
            Set_RimColor(data.BuffColor);
            Set_RimColorRange(data.Range);
        }
        else
        {
            Set_RimColor(new Color(0.4f,0.4f,0.4f,0));
            Set_RimColorRange(0.8f);
        }
    }

    public void SetColor(RoleColorData colorData)
    {
        SetColor(colorData.BuffColor);
        _roleBuffColorDatas.Add(colorData);
    }
    
    public void SetFade(float duration,AnimationCurve curve,Gradient color)
    {
        if (duration==0)
        {
            return;
        }
        foreach (Renderer renderer in MeshRenderers)
        {
            if (renderer.material.HasProperty(NoiseStep))
            {
                float t = 0;
                DOTween.To(() => t, value => t=value, 1f, duration).SetEase(curve).OnUpdate((() =>
                {
                    renderer.material.SetFloat(NoiseStep,curve.Evaluate(t));
                    renderer.material.SetColor(EdgeColor,color.Evaluate(t));
                }));
            }
        }
    }
    public void RemoveColor(RoleColorData colorData)
    {
        if (!_roleBuffColorDatas.Contains(colorData))
        {
            return;
        }
        _roleBuffColorDatas.Remove(colorData);
                
        if (_roleBuffColorDatas.Count > 0)
        {
            SetColor(_roleBuffColorDatas[_roleBuffColorDatas.Count - 1].BuffColor);
        }
        else
        {
            SetColor(new Color(0,0,0,0));
        }
    }

    private List<Tweener> _tweeners = new List<Tweener>();
    private float doTweenerTime = 0.1f;

    private void SetColor(Color color)
    {
        for (int i = 0; i < _tweeners.Count; i++)
        {
            _tweeners[i].Kill(false);
        }
        _tweeners.Clear();
        
        foreach (Renderer renderer in MeshRenderers)
        {
            if (renderer == null)
            {
                continue;
            }

            if (renderer.material.HasProperty(CoverColor))
            {
                var tweener1 = DOTween.To(() => renderer.material.GetColor(CoverColor),
                    value => renderer.material.SetColor(CoverColor, value), color, doTweenerTime);
                _tweeners.Add(tweener1);
            }

            if (renderer.material.HasProperty(CoverColorAlpha))
            {
                var tweener2 = DOTween.To(() => renderer.material.GetFloat(CoverColorAlpha),
                    value => renderer.material.SetFloat(CoverColorAlpha, value), color.a, doTweenerTime);
                _tweeners.Add(tweener2);
                
            }
        }
    }

    public void SetRoleMaterial(Material mat)
    {
        foreach (Renderer renderer in MeshRenderers)
        {
            if (renderer == null)
            {
                continue;
            }

            renderer.material = mat;
        }
    }
}


public struct RoleColorData
{
    public bool IsRim;
    public Color BuffColor;//颜色
    public BuffColorType BuffColorType;//颜色类型
    public float BuffColorEndTime;//结束时间
    public float Range;
}