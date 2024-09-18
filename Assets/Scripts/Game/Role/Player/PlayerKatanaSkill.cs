using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKatanaSkill : MonoBehaviour
{
    RoleController roleController;
    [Header("剑气波")]
    public Projectile WaveObj;
    public float DamageMutiply = 0.5f;
    //所有道具
    public ParticleSystem FinishParticles;
    public AudioClip FinishSFX;
    public ParticleSystem OnHitParticles;
    public FeedBackObject OnHitFeedback;
    public FeedBackObject OnHitFeedback_Boss;
    public ParticleSystem SamuraiUltimateParticles;
    void Start()
    {
        roleController = GetComponent<RoleController>();
        if (WaveObj != null)
            WaveObj.gameObject.SetActive(false);
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
    }
    private void OnDestroy()
    {
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
    }
    IEnumerator SpawnWave(float time, Vector3 position, Vector3 dir)
    {
        yield return new WaitForSecondsRealtime(time);
        SpawnWave(position, dir);
        //SpawnWave(WaveObj.transform.position,WaveObj.transform.forward);
    }
    void SpawnWave(Vector3 position, Vector3 dir)
    {
        if (WaveObj == null)
            return;
        Projectile waveInstance = Instantiate(WaveObj.gameObject, position, WaveObj.transform.rotation).GetComponent<Projectile>();
        waveInstance.transform.forward = dir;
        waveInstance.gameObject.SetActive(true);
        waveInstance.Init(roleController, roleController.AttackPower * DamageMutiply);
    }
    void SpawnMutiWaveAround(Vector3 position, Vector3 middleDir, float angleRange, int waveCount)
    {
        float angle = angleRange / waveCount;
        SamuraiUltimateParticles.gameObject.Duplicate().GetComponent<ParticleSystem>().Play();
        for (int i = 0; i < waveCount; i++)
        {
            Vector3 dir = Quaternion.Euler(0, -angleRange / 2 + i * angle, 0) * middleDir;
            StartCoroutine(SpawnWave(0.1f * i, WaveObj.transform.position, dir));
            //SpawnWave(WaveObj.transform.position,dir);
        }
    }

    void FreezeTarget(float time)
    {
        List<RoleController> enemyList = BattleTool.GetEnemysInDistance(roleController, 3f);
        foreach (var enemyTarget in enemyList)
        {
            if (!enemyTarget)
                return;

            // enemyTarget.SetDizziness(true);
            enemyTarget.SetFreeze(true);
            enemyTarget.Animator.speed = 0;
            //调用反馈
            GameObject par = Instantiate(OnHitParticles.gameObject, enemyTarget.transform.position + Vector3.up, Quaternion.identity);

            float freezeTime = time;
            if (enemyTarget.roleTeamType == RoleTeamType.EliteEnemy ||
            enemyTarget.roleTeamType == RoleTeamType.Enemy_Boss)
            {
                freezeTime *= 0.5f;
                if (OnHitFeedback_Boss)
                {
                    FeedbackManager.Inst.UseFeedBack(enemyTarget, OnHitFeedback_Boss);
                }
            }
            else
            {
                FeedbackManager.Inst.UseFeedBack(enemyTarget, OnHitFeedback);
            }
            StartCoroutine(UnFreezeTarget(enemyTarget, freezeTime));
        }
    }
    IEnumerator UnFreezeTarget(RoleController enemyTarget, float time)
    {
        yield return new WaitForSeconds(time);

        // enemyTarget.SetDizziness(false);
        enemyTarget.SetFreeze(false);
        enemyTarget.Animator.speed = 1;
        GameObject par = Instantiate(FinishParticles.gameObject, enemyTarget.transform.position, Quaternion.identity);
        FinishSFX.Play();

        //这里变成暴击屏幕会白一下
        var dmg = new DamageInfo(enemyTarget.TemporaryId, roleController.AttackPower * 3f, roleController,
            roleController.transform.position, DmgType.Physical);
        enemyTarget.HpInjured(dmg);
        EventManager.Inst.DistributeEvent(EventName.OnRoleAttack, roleController.TemporaryId);
        EventManager.Inst.DistributeEvent(EventName.OnPlayerAttackHitEnemy, dmg);
    }

    protected virtual void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }

        if (eventName.Contains("Katana_SpawnWave_"))
        {
            eventName = eventName.Replace("Katana_SpawnWave_", "");
            float time = float.Parse(eventName);
            if (WaveObj != null)
                StartCoroutine(SpawnWave(time > 0 ? time : 0, WaveObj.transform.position, WaveObj.transform.forward));
        }
        if (eventName.Contains("Katana_SpawnMutiWaveAround_"))
        {
            eventName = eventName.Replace("Katana_SpawnMutiWaveAround_", "");
            string[] strArray = eventName.Split('_');
            float angleRange = float.Parse(strArray[0]);
            int waveCount = int.Parse(strArray[1]);
            if (WaveObj != null)
                SpawnMutiWaveAround(WaveObj.transform.position, roleController.Animator.transform.forward, angleRange, waveCount);
        }
        if (eventName.Contains("Katana_FreezeAttackTargets_"))
        {
            eventName = eventName.Replace("Katana_FreezeAttackTargets_", "");
            float time = float.Parse(eventName);
            FreezeTarget(time);
        }
    }
}

