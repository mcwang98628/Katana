using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAdjuster : MonoBehaviour
{
    private void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        if(source!=null)
        {
            source.volume *= AudioManager.Inst.GameVolume;
        }
    }
}
