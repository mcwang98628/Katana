using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchiveCheckPoint : MonoBehaviour
{
    [SerializeField]
    private float _waitPlayFXTime;
    [SerializeField]
    private ParticleSystem _fireFx;
    [SerializeField]
    private Animator _animator;

    private static readonly int Bloom = Animator.StringToHash("Bloom");


    bool CheckIfUseCheckPoint()
    {
        return false;//目前先不用存档点
        if (BattleManager.Inst.RuntimeData.LevelStructData.LevelStructType != LevelStructType.Adventure)
            return false;
        if (BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData runtimeData)
        {
            if (runtimeData.CurrentLevelIndex == 0)
                return false;
        }

        return true;
    }

    private void Start()
    {
        if (!CheckIfUseCheckPoint())
        {
            Destroy(gameObject);
            return;
        }

        StartCoroutine(waitShowFx());

        var battleData = ArchiveManager.Inst.ArchiveData.BattleData;
        if (battleData != null)
        {
            if (BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData chapterRulesData)
            {
                if (chapterRulesData.CurrentChapterId == battleData.ChapterData.CurrentCPId && 
                    chapterRulesData.CurrentLevelIndex == battleData.ChapterData.CurrentLevelIndex)
                {
                    return;//一层只有一个存档点，如果有两个则会 只保存第一个。
                }
            }
        }
        ArchiveManager.Inst.SaveBattleArchive();
    }

    IEnumerator waitShowFx()
    {
        yield return new WaitForSecondsRealtime(_waitPlayFXTime);
        _fireFx.Play();
        _animator.SetTrigger(Bloom);
    }
}
