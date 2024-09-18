using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    public AudioClip sound;
    [SerializeField]
    private float volume=1;
    public bool PlayOnStart=true;
    private void OnEnable()
    {
        if(PlayOnStart)
        PlaySound();
    }
    public void PlaySound()
    {
        if (sound != null)
        {
            AudioManager.Inst.PlaySource(sound, volume);
        }
    }
}
