using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 测试语音用
/// </summary>
public class PlayerVoice : MonoBehaviour
{
    private AudioSource audioSource;
    // [SerializeField]
    // private AudioClip get;
    [SerializeField]
    private AudioClip OnHurt;
    float HurtVoiceInterval = 3f;
    float LastHurtTime;
    
    // [SerializeField]
    // private AudioClip skill;
    [SerializeField]
    private AudioClip OnUseSkill;

    [SerializeField]
    private List<AudioClip> OnKillEnemy;
    [SerializeField]
    private List<AudioClip> OnIdle;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        EventManager.Inst.AddEvent(EventName.OnRoleDead, OnEnemyDead);
        EventManager.Inst.AddEvent(EventName.OnRoleInjured, OnPlayerInjured);
        EventManager.Inst.AddEvent(EventName.OnUseItem, OnPlayerUseSkill);
    }


    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead, OnEnemyDead);
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured, OnPlayerInjured);
        EventManager.Inst.RemoveEvent(EventName.OnUseItem, OnPlayerUseSkill);
    }
    private void OnPlayerInjured(string arg1, object arg2)
    {
        RoleInjuredInfo injuredInfo = (RoleInjuredInfo)arg2;
        if (injuredInfo.Dmg.HitRoleId == BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            if (Time.time - LastHurtTime > HurtVoiceInterval)
            {
                LastHurtTime = Time.time;
                PlayVoice(OnHurt, true);
            }
            else
            {
                //DONothing
            }
        }
    }
    private void OnPlayerUseSkill(string arg1, object arg2)
    {
        Item item = (Item)arg2;
        if (item.ItemType == ItemType.ButtonSkill)
        {
            PlayVoice(OnUseSkill, true);
        }
    }

    private float lastVoiceTime;
    private void OnEnemyDead(string arg1, object arg2)
    {
        RoleDeadEventData eventData = (RoleDeadEventData)arg2;
        if (eventData.DeadRole.roleTeamType == RoleTeamType.Enemy_Boss)
        {
            if(OnKillEnemy.Count>0)
            PlayVoice(OnKillEnemy[Random.Range(0, OnKillEnemy.Count)], true);
        }
        else if (eventData.DeadRole.roleTeamType == RoleTeamType.Enemy)
        {
            if(OnKillEnemy.Count>0)
            PlayVoice(OnKillEnemy[Random.Range(0, OnKillEnemy.Count)], false);
        }
    }

    void PlayVoice(AudioClip clip, bool forcePlay)
    {
        if(clip!=null)
        if (!BattleManager.Inst.GameIsRuning)
            return;
        if (!ArchiveManager.Inst.ArchiveData.SettingArchiveData.Sound)
            return;
        if ((!audioSource.isPlaying && Time.time - lastVoiceTime > 3f) || forcePlay)
        {
            
            audioSource.clip = clip;
            audioSource.Play();
            lastVoiceTime = Time.time;
        }
    }

    private float idelTimer;
    private void Update()
    {
        idelTimer += Time.deltaTime;
        if (idelTimer >= 10f)
        {
            idelTimer -= 10f;
            PlayVoice(OnIdle[Random.Range(0, OnIdle.Count)], false);
        }
    }
}
