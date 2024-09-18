using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class PaladinSuckBlood : MonoBehaviour
{
    [LabelText("声音")]
    public AudioClip SuckBloodSFX;
    [LabelText("距离")]
    public float SuckBloodDistance;
    [LabelText("被吸的怪的粒子")]
    public GameObject SuckBloodParticles;
    [LabelText("回血量/每只")]
    public float HealCountEach;
    [LabelText("伤害")]
    public float SuckDmg;
    [LabelText("玩家Feedback")]
    public FeedBackObject SuckBloodFeedback;
    [LabelText("玩家粒子")]
    public GameObject HealParticles;
    [LabelText("每级吸血加的百分比")]
    public float LevelUpMul;


    private void Start()
    {
        //SuckBlood();
    }
    public void SuckBlood(int Level)
    {
        //被吸血的敌人的数量
        var enemyList = BattleTool.GetEnemysInDistance(BattleManager.Inst.CurrentPlayer, SuckBloodDistance);
        for (int i = 0; i < enemyList.Count; i++)
        {
            //找到范围内敌人
            RoleController target = enemyList[i];
            //对其进行输出
            DamageInfo dmg = new DamageInfo(target.TemporaryId, SuckDmg, BattleManager.Inst.CurrentPlayer, new Vector3(transform.position.x, 0, transform.position.z), DmgType.Other, true, false, 0, 0, false, null, null, null, false);
            target.roleHealth.Injured(dmg);
            //吸他们血
            Instantiate(SuckBloodParticles, target.transform);
        }
        //使用Feedback和粒子
        FeedbackManager.Inst.UseFeedBack(BattleManager.Inst.CurrentPlayer, SuckBloodFeedback);

        Instantiate(HealParticles, BattleManager.Inst.CurrentPlayer.transform);
        //播放音效
        SuckBloodSFX.Play();
        //给自己回血
        //BattleManager.Inst.CurrentPlayer.roleHealth.Treatment(
        //    new TreatmentData(HealCountEach * enemyList.Count * (1+ (Level - 1)* LevelUpMul ), BattleManager.Inst.CurrentPlayer.TemporaryId)
        //    );

    }
    private void Update()
    {
    }
}
