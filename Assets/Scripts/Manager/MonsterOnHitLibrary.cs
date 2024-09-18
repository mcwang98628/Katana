using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OnHitEffect
{
    public DmgType DmgType;
    public bool IsCrit;
    public FeedBackObject OnhitFeedbacks;
    public GameObject particles;
}
[CreateAssetMenu(menuName ="GameItem/OnHitFeedbackList")]
public class MonsterOnHitLibrary : ScriptableObject
{
    //[System.Serializable]
    [SerializeField]
    public List<OnHitEffect> OnHitEffectsList;


    public FeedBackObject GetFeedbackObj(DmgType ty,bool isCrit)
    {
        for(int i=0;i<OnHitEffectsList.Count;i++)
        {
            if(ty==OnHitEffectsList[i].DmgType && OnHitEffectsList[i].IsCrit == isCrit)
            {
                return OnHitEffectsList[i].OnhitFeedbacks;
            }
        }
        switch (ty)
        {
            case DmgType.ArrowHit:
            case DmgType.Physical:
            case DmgType.Other:

                break;
            default:
                break;
        };


        //Debug.LogError("未找到对应伤害类型的Feedback");
        return null;
    }
    public GameObject GetParticles(DmgType ty,bool isCrit)
    {
        for (int i = 0; i < OnHitEffectsList.Count; i++)
        {
            if (ty == OnHitEffectsList[i].DmgType && OnHitEffectsList[i].IsCrit == isCrit)
            {
                return OnHitEffectsList[i].particles;
            }
        }
        //Debug.LogError("未找到对应伤害类型的粒子");
        return null;
    }
}
