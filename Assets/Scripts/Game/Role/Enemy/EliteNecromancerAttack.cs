using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EliteNecromancerAttack : EnemyMagicAttack
{
    public int ShootCount;
    public float Angle;
    public int SummonCount;

    public DmgBuffOnTouch GhostHandStraight;
    //鬼手数量
    public int GhostHandLineCount = 20;
    //鬼手之间的间距
    public int GhostHandLineDis;
    //鬼手出生处离精英怪的距离
    public int GhostHandBornDis;


    protected GameObject spawnPoints;
    [SerializeField]
    private Transform createPoint;

    [SerializeField]
    private List<int> EnemyIds = new List<int>();

    
    private void Start()
    {
        spawnPoints = GameObject.Find("EnemySpawnPoints");
    }
    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);

      
        if (eventName.Contains("StartCharge"))
        {
            if (StartChargeAudio != null)
            {
                AudioManager.Inst.PlaySource(StartChargeAudio, 0.5f);
                //ShowMagicBall();
            }
        }
        // if (eventName.Contains(AnimatorEventName.EnemyShowAttackWarning_))
        // {
        //     eventName = eventName.Replace(AnimatorEventName.EnemyShowAttackWarning_, "");
        //     AttackInfo ai = DataManager.Inst.GetAttackInfo(eventName);
        //     ShowAttackerDebug(ai.AttackType, ai.AttackRadius, ai.AttackAngle);
        //     //ShowMagicBall();
        // }
        //else if (eventName.Contains(AnimatorEventName.DmgStart_))
        //{
        //    HideAttackerDebug();
        //    SpwanProjectile();
            

        //}
        else if (eventName.Contains(AnimatorEventName.EndAttack_))
        {

        }
        // else if (eventName.Contains(AnimatorEventName.EnemyShowWarning_))
        // {
        //     //ShowMagicBall();
        // }
        // else if (eventName.Contains(AnimatorEventName.EndInput))
        // {
        //     isLookAt = false;
        // }
        if (eventName.Contains("StopLookAt"))
        {
            isLookAt = false;
        }
        if (eventName.Contains("StartLookAt"))
        {
            isLookAt = true;
        }
    }
    public override void SpwanProjectile()
    {
        Debug.Log("SpawnProjectile");
        if (currentAttackStatus == 0)
        {


                StartCoroutine(CreateOrbLine());
        }
        //散射幽灵爪
        else if (currentAttackStatus == 1)
        {
            int OffsetCount = (ShootCount - 1) / 2;
            for (int i = 0; i < ShootCount; i++)
            {
                DmgBuffOnTouch projectile = Instantiate(Projectile, SpwanPoint.position, Quaternion.Euler(SpwanPoint.eulerAngles + new Vector3(0, (-OffsetCount + i) * Angle, 0)));
                projectile.Init(roleController, AttackPower);
            }
            HideChargeVFX();
        }
    }
    public IEnumerator CreateOrbLine()
    {
        //鬼手波数
        for (int j = 0; j < 2; j++)
        {
            float Angle = Random.Range(0, 360);
            //创建一条法球线
            for (int i = 0; i < GhostHandLineCount; i++)
            {
                DmgBuffOnTouch pro = Instantiate(GhostHandStraight, new Vector3(0, 1, 0) + transform.position + GhostHandBornDis * new Vector3(-Mathf.Sin(Mathf.Deg2Rad* Angle), 0, -Mathf.Cos(Mathf.Deg2Rad * Angle)) + GhostHandLineDis * (i-GhostHandLineCount/2) * new Vector3( Mathf.Cos(Mathf.Deg2Rad * Angle), 0, -Mathf.Sin(Mathf.Deg2Rad * Angle)), Quaternion.Euler(0, Angle, 0));
                pro.Init(roleController,AttackPower);
            }
            yield return new WaitForSeconds(1f);
        }
    }


    public void CreatEnemy(Transform _trans)
    {
        ShowStartAttackParticle();
        BattleTool.CreateEnemy(EnemyIds[Random.Range(0, EnemyIds.Count)], delegate(EnemyController controller)
        {
            if (controller == null)
                return;
        
            controller.transform.position = _trans.position;
            //怪物出来会有个小动画。
            controller.Animator.transform.localScale = new Vector3(0, 0, 0);
            controller.Animator.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        });

    }
}
