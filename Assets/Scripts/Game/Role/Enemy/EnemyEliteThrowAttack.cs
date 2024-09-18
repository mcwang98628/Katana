using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEliteThrowAttack : EnemyThrowAttack
{

    [Header("另外一只手新投掷物")]
    public GameObject Projectile2;
    public GameObject MeleeProjectile;
    public GameObject Explosion2;
    public Transform SpwanPoint2;
    public Transform MeleeExplosionPoint;
    public bool MeleeMotherAttack;
    bool throwProjitle2 = false;

    [Header("追踪炸弹相关")]
    public float FollowExpInterval = 0.6f;
    public float FollowExpWaitTime = 1.25f;
    float timer = 0;
    bool IsFlollowExp = false;
    int FollowProjectType = 0;

    [Header("技能音效")]
    public AudioClip FollowSkillScream;
    public AudioClip RoundSkillScream;
    private void FixedUpdate()
    {
        if (IsFlollowExp && (!roleController.IsDie))
        {
            Transform Target = UpdateTarget();
            if (!Target)
                return;
            roleController.Animator.transform.LerpLookAt(Target, 0.1f);

            if (Time.time - timer > FollowExpInterval)
            {

                SpwanFollowProjectile();
                timer = Time.time;
            }
        }
    }


    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);


        if (eventName.Contains("StartJump"))
        {
            JumpAway();
        }
        else if (eventName.Contains("RoundExplosion"))
        {
            if (RoundSkillScream != null)
                AudioManager.Inst.PlaySource(RoundSkillScream);
            //扔一圈炸弹,距离什么的都在这里面配置
            if (MeleeMotherAttack)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector3 DesPos = MeleeExplosionPoint.position + Quaternion.AngleAxis(i * 360 / 6, Vector3.up) * roleController.Animator.transform.forward * 4f;
                    // IndicatorManager.Inst.ShowAttackIndicator().SetEnable(true).SetAngle(360).SetSize(BombRange).SetTime(ThrowMoveTime).SetPosition(DesPos + Vector3.up * 0.1f);

                    // IndicatorManager.Inst.ShowAttackIndicator().Show(roleController,DesPos + Vector3.up * 0.1f,360,BombRange,ThrowMoveTime, Color.red);
                    GameObject MeleeSurroundPro = Instantiate(Projectile, MeleeExplosionPoint.position, Quaternion.identity);
                    MeleeSurroundPro.GetComponent<ThrowProjectile>().Init(DesPos, ThrowMoveTime, ThrowHeight, Explosion, BombRange, AttackPower, roleController);
                }
            }
        }
        else if (eventName.Contains("MeleeExplosion"))
        {
            //roleController.InputMove(new Vector2(3,0));
            //怪物在使出这次攻击时需要直接朝向玩家，并且向前位移。

            //roleController.Animator.transform.LookAt(BattleManager.Inst.CurrentPlayer.transform);
            MeleeAttackMove(8);

            GameObject MeleePro = Instantiate(MeleeProjectile, MeleeExplosionPoint.position, Quaternion.identity);
            MeleePro.GetComponent<ThrowProjectile>().Init(MeleeExplosionPoint.position, 0.01f, 0.01f, Explosion2, BombRange, AttackPower, roleController);
        }
        else if (eventName.Contains("StartFollowExplosion"))
        {

            if (FollowSkillScream != null)
                AudioManager.Inst.PlaySource(FollowSkillScream);
            IsFlollowExp = true;
            timer = 0;
        }
        else if (eventName.Contains("EndFollowExplosion"))
        {
            IsFlollowExp = false;
        }

    }
    //往地上炸的时候会有小位移
    public void MeleeAttackMove(float force)
    {
        roleController.Animator.transform.LerpLookAt(BattleManager.Inst.CurrentPlayer.transform,0.6f);
        Vector2 v2 = BattleTool.GetWorldDir(roleController.Animator.transform,new Vector2(0, 5));
        roleController.FastMove(0.1f, force, new Vector3(v2.x, 0, v2.y), null);
    }



    protected override void SpwanProjectile()
    {

        //近身攻击
        if (AttackStatus == 1)
        {
            IndicatorManager.Inst.ShowAttackIndicator().Show(roleController,MeleeExplosionPoint.position+Vector3.up*0.1f,360,BombRange,ThrowMoveTime+0.1f, Color.red);
            // IndicatorManager.Inst.ShowAttackIndicator().SetEnable(true).SetAngle(360).SetSize(BombRange).SetTime(ThrowMoveTime + 0.1f).SetPosition(MeleeExplosionPoint.position+Vector3.up*0.1f);
        }
        //丢炸弹
        else
        {

            Transform Target = UpdateTarget();
            if (!Target)
                return;

            Vector3 desPos = Target.position + new Vector3(RandomOffst * (Random.value - 0.5f), 0, RandomOffst * (Random.value - 0.5f));
            // IndicatorManager.Inst.ShowAttackIndicator().Show(roleController,desPos,360,BombRange,ThrowMoveTime+0.1f, Color.red);
            // IndicatorManager.Inst.ShowAttackIndicator().SetEnable(true).SetAngle(360).SetSize(BombRange).SetTime(ThrowMoveTime + 0.1f).SetPosition(desPos);

            if (throwProjitle2)
            {
                GameObject projectile2 = Instantiate(Projectile2, SpwanPoint.position, Quaternion.identity);
                projectile2.GetComponent<ThrowProjectile>().Init(desPos, ThrowMoveTime, ThrowHeight, Explosion2, BombRange, AttackPower, roleController);
            }
            else
            {
                GameObject projectile = Instantiate(Projectile, SpwanPoint.position, Quaternion.identity);
                projectile.GetComponent<ThrowProjectile>().Init(desPos, ThrowMoveTime, ThrowHeight, Explosion, BombRange, AttackPower, roleController);
            }
            throwProjitle2 = !throwProjitle2;
        }
    }
    void SpwanProjectile2()
    {
        Transform Target = UpdateTarget();
        if (!Target)
            return;

        Vector3 desPos = Target.position + new Vector3(RandomOffst * (Random.value - 0.5f), 0, RandomOffst * (Random.value - 0.5f));
        GameObject projectile2 = Instantiate(Projectile2, SpwanPoint.position, Quaternion.identity);
        projectile2.GetComponent<ThrowProjectile>().Init(desPos, ThrowMoveTime, ThrowHeight, Explosion2,BombRange, AttackPower, roleController);
        // IndicatorManager.Inst.ShowAttackIndicator().Show(roleController,desPos,360,BombRange,ThrowMoveTime+0.1f, Color.red);
        // IndicatorManager.Inst.ShowAttackIndicator().SetEnable(true).SetAngle(360).SetSize(BombRange).SetTime(ThrowMoveTime+0.1f).SetPosition(desPos);
    }
    void SpwanFollowProjectile()
    {
        Transform Target = UpdateTarget();
        if (!Target)
            return;
        Vector3 desPos = Target.position;

        if (FollowProjectType % 2 == 0)
        {
            GameObject projectile = Instantiate(Projectile, SpwanPoint.position, Quaternion.identity);
            projectile.GetComponent<ThrowProjectile>().Init(desPos, FollowExpWaitTime, ThrowHeight, Explosion, BombRange, AttackPower, roleController);
        }
        else
        {
            GameObject projectile = Instantiate(Projectile2, SpwanPoint.position, Quaternion.identity);
            projectile.GetComponent<ThrowProjectile>().Init(desPos, FollowExpWaitTime, ThrowHeight, Explosion2, BombRange, AttackPower, roleController);
        }
        FollowProjectType++;
        // IndicatorManager.Inst.ShowAttackIndicator().Show(roleController,desPos,360,BombRange,ThrowMoveTime+0.1f, Color.red);
        // IndicatorManager.Inst.ShowAttackIndicator().SetEnable(true).SetAngle(360).SetSize(BombRange).SetTime(ThrowMoveTime + 0.1f).SetPosition(desPos);
    }
     void JumpAway()
    {
        roleController.FastMove(0.6f,10f,roleController.Animator.transform.forward,null);
    }


}
