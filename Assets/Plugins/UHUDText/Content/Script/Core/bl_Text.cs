using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class bl_Text : MonoBehaviour
{
    public CanvasGroup LayoutRoot = null;
    public Text m_Text = null;
    public RectTransform Rect;
    public Image frontImage;
    [HideInInspector] public Color m_Color;
    [HideInInspector] public bl_Guidance movement;
    [HideInInspector] public float Xcountervail;
    [HideInInspector] public float Ycountervail;
    [HideInInspector] public int m_Size;
    [HideInInspector] public float m_Speed;
    [HideInInspector] public float m_LifeTime;
    [HideInInspector] public float m_StartTime;
    [HideInInspector] public string m_text;
    [HideInInspector] public Transform m_Transform;
    [HideInInspector] public float Yquickness;
    [HideInInspector] public float YquicknessScaleFactor;
    [HideInInspector] public float Offset;
    
}