using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public static class AudioTools
{
    public static void Play(this AudioClip _audioClip, int Proirity = 0, float Volume = 1)
    {
        AudioManager.Inst.PlaySource(_audioClip, Volume, Proirity);
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Inst { get; private set; }
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource sourcePrefab;
    List<AudioSource> sourcesPool = new List<AudioSource>();
    List<AudioSource> sourcesUseingPool = new List<AudioSource>();

    [LabelText("最大音效数量")] [SerializeField] private int maxSourceCount = 16;

    //游戏的音量大小
    public float GameVolume = 1;

    public void Init()
    {
        Inst = this;
        EventManager.Inst.AddEvent(EventName.SetSound, OnSetSound);
        EventManager.Inst.AddEvent(EventName.SetBmg, OnSetBgm);
    }

    private void OnDestroy()
    {
        acTime.Clear();
        EventManager.Inst.RemoveEvent(EventName.SetSound, OnSetSound);
        EventManager.Inst.RemoveEvent(EventName.SetBmg, OnSetBgm);
    }

    private void OnSetBgm(string arg1, object arg2)
    {
        BGM.volume = ArchiveManager.Inst.ArchiveData.SettingArchiveData.Bgm ? 0.2f : 0;
    }

    private void OnSetSound(string arg1, object arg2)
    {
        GameVolume = ArchiveManager.Inst.ArchiveData.SettingArchiveData.Sound ? 1 : 0;
    }


    private Tweener bgmTweener;

    public void PlayBGM(AudioClip ac, float volume)
    {
        if (BGM == null)
        {
            return;
        }

        if (!ArchiveManager.Inst.ArchiveData.SettingArchiveData.Bgm)
        {
            volume = 0;
        }

        // volume = 0.2f;
        if (bgmTweener != null)
        {
            bgmTweener.Kill(false);
        }

        BGM.mute = false;
        BGM.volume = 0;
        // bgmTweener = BGM.DOFade(volume, 2f);
        bgmTweener = DOTween.To(() => BGM.volume, value => BGM.volume = value, volume, 2f);
        BGM.clip = ac;
        BGM.Play();
    }

    public void StopBGM()
    {
        if (bgmTweener != null)
        {
            bgmTweener.Kill(false);
        }

        bgmTweener = BGM.DOFade(0, 2f);
    }

    private Dictionary<AudioClip, int> acTime = new Dictionary<AudioClip, int>();

    public AudioSource PlaySource(AudioClip ac, float value = 1, int pro = 0)
    {
        if (ac == null)
            return null;
        var frame = Time.renderedFrameCount;
        
        if (acTime.ContainsKey(ac))
        {
            if (acTime[ac]==frame)
            {
                return null;
            }

            acTime[ac] = frame;
        }
        else
        {
            acTime.Add(ac, frame);
        }
        
        AudioSource audio = GetSources();
        sourcesUseingPool.Add(audio);
        audio.gameObject.SetActive(true);
        audio.clip = ac;
        audio.volume = value * GameVolume;
        audio.priority = pro;
        audio.Play();
        return audio;
    }

    public void RecycleAudio(AudioSource audioSource)
    {
        if (sourcesUseingPool.Contains(audioSource))
        {
            audioSource.Stop();
            sourcesUseingPool.Remove(audioSource);
            sourcesPool.Add(audioSource);
        }
    }

    private AudioSource GetSources()
    {
        AudioSource audio;
        if (sourcesUseingPool.Count >= maxSourceCount)
        {
            audio = sourcesUseingPool[0];
            sourcesUseingPool.RemoveAt(0);
        }
        else
        {
            if (sourcesPool.Count == 0)
            {
                audio = Instantiate(sourcePrefab, transform);
            }
            else
            {
                audio = sourcesPool[sourcesPool.Count - 1];
                sourcesPool.RemoveAt(sourcesPool.Count - 1);
            }
        }

        return audio;
    }


    private void Update()
    {
        UpdateSourcesUseingPool();
    }

    void UpdateSourcesUseingPool()
    {
        List<AudioSource> audioSourcess = new List<AudioSource>();
        for (int i = 0; i < sourcesUseingPool.Count; i++)
        {
            if (!sourcesUseingPool[i].isPlaying)
            {
                audioSourcess.Add(sourcesUseingPool[i]);
            }
        }

        for (int i = 0; i < audioSourcess.Count; i++)
        {
            audioSourcess[i].gameObject.SetActive(false);
            sourcesUseingPool.Remove(audioSourcess[i]);
            sourcesPool.Add(audioSourcess[i]);
        }
    }
}