using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy_Common_FeedBack : Role_Common_FeedBack
{
    [ShowInInspector]
    [ReadOnly]
    [LabelText("Enemy反馈库")]
    [BoxGroup("Enemy独有")]
    private MonsterOnHitLibrary hitlib;
    [SerializeField]
    [LabelText("开场反馈")]
    [BoxGroup("Enemy独有")]
    public FeedBackObject StartFeedback;

    protected override void Awake()
    {
        base.Awake();
        
        GetFeedBack();
    }
    protected override void OnEnable()
    {
        StartCoroutine(DoStartFeedback());
    }
    
    IEnumerator DoStartFeedback()
    {
        if (roleController.roleNode == null)
        {
            yield return new WaitForEndOfFrame();
        }
        if (StartFeedback != null)
        {
            FeedbackManager.Inst.UseFeedBack(roleController, StartFeedback);
        }
    }
    
    void GetFeedBack()
    {
        int heroId = ArchiveManager.Inst.ArchiveData.GlobalData.LastSelectHeroID;
        var heroLeveldata = DataManager.Inst.GetHeroData(heroId);
        string path = $"Assets/AssetsPackage/ScriptObject_FeedbackLib/{heroLeveldata.FeedBackName}.asset";
        ResourcesManager.Inst.GetAsset<MonsterOnHitLibrary>(path, delegate(MonsterOnHitLibrary library)
        {
            hitlib = library;
            if (hitlib == null)
                Debug.LogError("#Err# 没有找到对应的造成伤害的反馈的文件！这是根据地址来找的。");
        });
    }

    protected override void OnRoleInjured(string arg1, object arg2)
    {
        if (hitlib == null)
            return;
        
        
        var data = (RoleInjuredInfo) arg2;
        if (data.RoleId != roleController.TemporaryId)
        {
            return;
        }

        var particle = hitlib.GetParticles(data.Dmg.DmgType, data.Dmg.IsCrit);
        if ( particle != null)
        {
            GameObject CurrentHitPar = Instantiate(particle, gameObject.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            CurrentHitPar.SetActive(true);
            CurrentHitPar.transform.SetParent(gameObject.transform);
        }

        FeedBackObject feedBack = data.Dmg.InjuredFeedBackObject;
        if (feedBack == null)
            feedBack = hitlib.GetFeedbackObj(data.Dmg.DmgType, data.Dmg.IsCrit);
        if(feedBack != null)
            FeedbackManager.Inst.UseFeedBack(roleController, feedBack);
    }
}
