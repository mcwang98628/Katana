using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.Audio)]
    [LabelText("音效")]
    public AudioClip AudioClip;
    [ShowIf("EffectType", EffectType.Audio)]
    [LabelText("音量")]
    public float AudioVolume;
    [ShowIf("EffectType", EffectType.Audio)]
    [LabelText("音效播放延迟")]
    public float AudioDelay;
}
public class AudioEffect : ItemEffect
{
    private AudioClip _audioClip;
    private float _volume;
    private float _audioDelay;
    public AudioEffect(AudioClip audio,float volume,float audioDelay)
    {
        _audioClip = audio;
        _volume = volume;
        _audioDelay = audioDelay;
    }
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.StartCoroutine(waitPlaySource());
    }

    IEnumerator waitPlaySource()
    {
        yield return new WaitForSeconds(_audioDelay);
        AudioManager.Inst.PlaySource(_audioClip, _volume);
    }
}
