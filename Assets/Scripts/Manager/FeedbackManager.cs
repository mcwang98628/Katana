using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Inst => inst;
    private static FeedbackManager inst;
    
    
    // private static readonly int CoverColor = Shader.PropertyToID("_CoverColor");
    // private static readonly int CoverColorAlpha = Shader.PropertyToID("_CoverColorAlpha");
    
    
    public void Init()
    {
        inst = this;
    }

    private Coroutine zoomCoroutine;
    private Coroutine timeScaleCoroutine;
    //private Coroutine changeColorCoroutine;
    //private Coroutine MulScreenColorCoroutine;
    private Coroutine ShakeModelCoroutine;
    private FadeScreen ScreenFader;
    // Dictionary<Renderer,Coroutine> changeColorCoroutines = new Dictionary<Renderer, Coroutine>();

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnEnvironmentBack,OnEnvironmentBackEvent);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnEnvironmentBack,OnEnvironmentBackEvent);
    }

    public void UseFeedBack(RoleController roleController,FeedBackObject data)
    {
        if (data == null ||
            roleController == null)
        {
            return;
        }
        if (roleController == null ||
            roleController.roleNode == null)
        {
            Debug.LogWarning("Error: Role==NULL");
            //return;
            //有些时候不是roleController发出的，不应该反回
        }


        if (data.UseCameraShake)
        {
            CameraShake(data.CameraShakeAmplitude,data.CameraShakeDuration);
        }

        if (data.UseCameraZoom)
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
                CameraController.Inst.ZoomOffset = oldZoomOffset;
            }
            zoomCoroutine = StartCoroutine(ZoomCamera(data.ZoomValue,data.EnterZoomTime,data.ZoomDuration,data.ExitZoomTime));
        }

        if (data.UseAudio && data.AudioClips.Count>0)
        {
            UseAudio(data.AudioClips[Random.Range(0, data.AudioClips.Count)],data.SFXPriority);
        }

        if (data.UseChangeColor)
        {
            roleController.roleNode.SetColor(
                new RoleColorData(){BuffColor = data.Color,BuffColorEndTime = Time.time+data.ColorDuration,BuffColorType = BuffColorType.Continued}
            );
        }

        if (data.UseDeadFade)
        {
            roleController.roleNode.SetFade(data.duration,data.NoiseStep,data.EdgeColor);
        }
        
        if (data.UseModelShake && roleController.roleNode.Model!=null)
        {
            if(ShakeModelCoroutine!=null)
            {
                StopCoroutine(ShakeModelCoroutine);
                roleController.Animator.gameObject.transform.localPosition = Vector3.zero;
            }
            ShakeModelCoroutine = StartCoroutine(ShakeModel(roleController,data.ModelShakeAmplitude,data.ModelShakeTimeBetween,data.ModelShakeDuration));
        }

        if (data.UsePauseFrame)
        {
            PauseFrame(data.PauseFrameDuration);
        }

        if (data.UseTimeScale)
        {
            if (timeScaleCoroutine != null)
            {
                StopCoroutine(timeScaleCoroutine);
                TimeManager.Inst.SetTimeScale(1f);
            }
            timeScaleCoroutine = StartCoroutine(TimeScale(data.TimeScaleValue,data.TimeScaleDuration));
        }
        if(data.AddScreenColor)
        { 
            //if(MulScreenColorCoroutine!=null)
            //{
            //    StopCoroutine(MulScreenColorCoroutine);
            //    //MulScreenColorCoroutine=StartCoroutine();
            //}
            if (data.ScreenColor != null && data.AddMul != 0)
            {
                if(ScreenFader==null)
                    ScreenFader = FindObjectOfType<FadeScreen>();

                if (ScreenFader!= null)
                {
                    ScreenFader.StartFadeIn(data.ScreenColor, data.AddMul);
                }
            }
        }

        if (data.UseChangeRimColor)
        {
            roleController.roleNode.Set_RimColor(
                new RoleColorData(){IsRim = true,BuffColor = data.RimColor,BuffColorEndTime = Time.time+data.RimColorDuration,BuffColorType = BuffColorType.Continued,Range = data.RimRange}
            );
        }

        if (data.UseEnvironment)
        {
            if (data.UseEventBackEnvironment)
            {
                environmentBackEvent = false;
                StartCoroutine(WaitEventBackOldEnvironment(EnvironmentManager.Inst.CurrentEnvironment,data.EnvironmentExitTime));
            }
            else
            {
                StartCoroutine(WaitBackOldEnvironment(EnvironmentManager.Inst.CurrentEnvironment,data.EnvironmentTime,data.EnvironmentExitTime));
            }
            EnvironmentManager.Inst.SetEnvironment(data.EnvironmentItem,data.EnvironmentEnterTime);
        }
        
    }

    //延迟返回原来的环境颜色。
    IEnumerator WaitBackOldEnvironment(EnvironmentItemScript data,float time,float exitTime)
    {
        yield return new WaitForSecondsRealtime(time);
        EnvironmentManager.Inst.SetEnvironment(data,exitTime);
    }

    void OnEnvironmentBackEvent(string str,object obj)
    {
        environmentBackEvent = true;
    }
    private bool environmentBackEvent = false;
    //等待事件  延迟返回原来的环境颜色。
    IEnumerator WaitEventBackOldEnvironment(EnvironmentItemScript data,float exitTime)
    {
        while (!environmentBackEvent)
            yield return null;
        EnvironmentManager.Inst.SetEnvironment(data,exitTime);
    }


    void CameraShake(float amplitude,float duration)
    {
        CameraController.Inst.DoShakeOffset(duration, Vector3.one * amplitude);
        //CameraController.Inst.DoShakeRotation(duration, new Vector3(0, 0, 1) * amplitude,120,90,false); 
    }

    private Vector3 oldZoomOffset;
    IEnumerator ZoomCamera(float value,float enterZoomTime,float durationTime,float exitZoomTime)
    {
        oldZoomOffset = CameraController.Inst.ZoomOffset;
        CameraController.Inst.DoZoom(value,enterZoomTime);
        yield return new WaitForSeconds(durationTime);
        CameraController.Inst.DoZoom(oldZoomOffset,exitZoomTime);
    }

    void UseAudio(AudioClip audioClip,int Priority)
    {
        AudioManager.Inst.PlaySource(audioClip,1,Priority);
    }
    IEnumerator ShakeModel(RoleController roleController,float Amplitude,float TimeBetween,float Duration)
    {
        float StartTime = Time.time;
        GameObject shakedObj;
        float LastShakeTime=-1;
        bool LastShakeDir = false;
        shakedObj = roleController.Animator.gameObject;
        //shakedObj.GetComponent<Animator>().speed = 0;
        while(Time.time<StartTime +Duration)
        {
            //Shake!
            if(Time.time>LastShakeTime+TimeBetween)
            {
                LastShakeTime = Time.time;
                if(shakedObj!=null)
                shakedObj.transform.localPosition = (LastShakeDir?1 : -1) * new Vector3(Random.Range(Amplitude - 0.05f, Amplitude + 0.05f), 0, Random.Range(Amplitude - 0.05f, Amplitude + 0.05f));
                LastShakeDir = LastShakeDir ? false : true;
            }
            yield return null;
            
        }

        if (shakedObj != null)
            //shakedObj.GetComponent<Animator>().speed = 1;
        shakedObj.transform.localPosition = Vector3.zero;

    }

    private Coroutine pauseFrameCoroutine;
    void PauseFrame(float duration)
    {
        if (pauseFrameCoroutine != null)
        {
            StopCoroutine(pauseFrameCoroutine);
        }
        pauseFrameCoroutine = StartCoroutine(TimeScale(0, duration));
    }

    IEnumerator TimeScale(float value,float duration)
    {
        TimeManager.Inst.SetTimeScale(value);
        yield return new WaitForSecondsRealtime(duration);
        TimeManager.Inst.SetTimeScale(1f);
    }
    
    
}
