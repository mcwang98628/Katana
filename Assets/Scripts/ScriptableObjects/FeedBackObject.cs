using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "BeedBack/BeedBackObj")]
public class FeedBackObject : ScriptableObject
{
    [Space]
    [LabelText("使用相机震动")]
    public bool UseCameraShake;
    [LabelText("相机震动幅度")]
    [ShowIf("UseCameraShake")]
    public float CameraShakeAmplitude;
    [LabelText("相机震动时长")]
    [ShowIf("UseCameraShake")]
    public float CameraShakeDuration;
    [LabelText("改变屏幕颜色")]
    public bool AddScreenColor;
    [ShowIf("AddScreenColor")]
    [LabelText("屏幕颜色")]
    public Color ScreenColor;
    [ShowIf("AddScreenColor")]
    public float AddMul;
    
    [Space]
    [Space]
    [LabelText("使用相机缩放")]
    public bool UseCameraZoom;
    [LabelText("缩放Value")] 
    [ShowIf("UseCameraZoom")]
    public float ZoomValue;
    [LabelText("进入缩放时间")]
    [ShowIf("UseCameraZoom")]
    public float EnterZoomTime;
    [LabelText("推出缩放时间")]
    [ShowIf("UseCameraZoom")]
    public float ExitZoomTime;
    [LabelText("缩放持续时间")]
    [ShowIf("UseCameraZoom")]
    public float ZoomDuration;

    
    [Space]
    [Space]
    [LabelText("使用音效")]
    public bool UseAudio;
    [LabelText("音效列表 （随机选取一个）")]
    [ShowIf("UseAudio")]
    public List<AudioClip> AudioClips = new List<AudioClip>();
    [ShowIf("UseAudio")]
    public int SFXPriority=0;



    [Space]
    [Space]
    [LabelText("改变模型颜色")]
    //[LabelText("此处必须把rolenode里面的model给设置好")]
    public bool UseChangeColor;
    [LabelText("目标颜色")]
    [ShowIf("UseChangeColor")]
    [ColorUsage(true,true)] 
    public Color Color;
    [LabelText("持续时间")]
    [ShowIf("UseChangeColor")]
    public float ColorDuration;

    [Space] [Space] 
    [LabelText("死亡时消失")]
    public bool UseDeadFade;
    [ShowIf("UseDeadFade")]
    public AnimationCurve NoiseStep;
    [ShowIf("UseDeadFade")]
    [GradientUsage(true)]
    public Gradient EdgeColor;
    [ShowIf("UseDeadFade")]
    public float duration;
    
    [Space]
    [Space]
    [LabelText("改变模型轮廓颜色")]
    public bool UseChangeRimColor;
    [LabelText("目标颜色")]
    [ShowIf("UseChangeRimColor")]
    [ColorUsage(true,true)] 
    public Color RimColor;
    [LabelText("范围")]
    [ShowIf("UseChangeRimColor")]
    public float RimRange;
    [LabelText("持续时间")]
    [ShowIf("UseChangeRimColor")]
    public float RimColorDuration;
    
    
    
    [Space]
    [Space]
    [LabelText("使用顿帧")]
    public bool UsePauseFrame;
    [LabelText("顿帧持续时间")]
    [ShowIf("UsePauseFrame")]
    public float PauseFrameDuration;

    [Space]
    [Space]
    [LabelText("使用时间缩放")]
    public bool UseTimeScale;
    [LabelText("时间缩放 持续时间")]
    [ShowIf("UseTimeScale")]
    public float TimeScaleDuration;
    [LabelText("时间缩放 Value")]
    [ShowIf("UseTimeScale")]
    public float TimeScaleValue;

    [Space]
    [Space]
    [LabelText("使用模型震动")]
    public bool UseModelShake;
    [LabelText("模型震动幅度")]
    [ShowIf("UseModelShake")]
    public float ModelShakeAmplitude;
    [LabelText("模型震动时长")]
    [ShowIf("UseModelShake")]
    public float ModelShakeDuration;
    [ShowIf("UseModelShake")]
    public float ModelShakeTimeBetween;

    [Space] [Space] 
    [LabelText("使用环境")]
    public bool UseEnvironment;
    [LabelText("使用事件回滚环境")]
    public bool UseEventBackEnvironment;
    [LabelText("持续时间")]
    [ShowIf("UseEnvironment")]
    [HideIf("UseEventBackEnvironment")]
    public float EnvironmentTime;
    [LabelText("淡入时间")]
    [ShowIf("UseEnvironment")]
    public float EnvironmentEnterTime;
    [LabelText("淡出时间")]
    [ShowIf("UseEnvironment")]
    public float EnvironmentExitTime;
    [LabelText("环境配置")]
    [ShowIf("UseEnvironment")]
    public EnvironmentItemScript EnvironmentItem;

}
