using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Inst { get; private set; }
    //当前设置的TimeScale
    public float CurrentSetTimeScale { get; private set; }

    public bool IsPause => pauseTimes > 0;
    int pauseTimes;

    public void Init()
    {
        Inst = this;
        CurrentSetTimeScale = 1f;
    }

    public void Pause()
    {
        pauseTimes++;
        checkTimePause();
    }
    public void UnPause()
    {
        pauseTimes--;
        if (pauseTimes < 0 )
        {
            pauseTimes = 0;
        }
        checkTimePause();
    }
    void checkTimePause()
    {
        if (pauseTimes>0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = CurrentSetTimeScale;
        }
    }

    public void SetTimeScale(float scale)
    {
        CurrentSetTimeScale = scale;
        if (!IsPause)
        {
            Time.timeScale = scale;
        }

        if (scale>0)
        {
            Time.fixedDeltaTime = 0.02f * scale;
        }
    }
}
