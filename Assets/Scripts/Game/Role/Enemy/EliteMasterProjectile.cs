using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMasterProjectile : DmgBuffOnTouch
{
    public float StartLerpTime=1;
    public float LerpMoveSpeed=0.2f;

    public float Speed;
    public Vector3 StartPos;
    public bool DestroySelfOnHit;
    //子弹表现方式，有如下表现：
    //1.开始直接一排，然后向前。
    //2.直接像蘑菇发孢子一样向四周发静止子弹。
    //3.向玩家附近发射一些子弹，过一段时间追踪玩家。
    public int BehaviorWay;
    // 死亡之后释放小子弹。
    public bool EmmitProOnDeath;
    public EliteMasterProjectile DeathPro;

    public int DeathBehavior = 0;
    //是否打到了玩家，如果打到那么自毁。
    //bool HitPlayer;

    private void Start()
    {
        //OnUse=false;
        StartCoroutine(DelayDamageOn());
        StartCoroutine(projectileMove());
        
    }
    public IEnumerator DelayDamageOn()
    {
        yield return new WaitForSeconds(StartLerpTime);
        //OnUse=true;
    }
    public IEnumerator projectileMove()
    {

        float StartTime = Time.time;
        while(Time.time<StartTime+StartLerpTime)
        {
            LerpMoveToPos(StartPos);
            yield return null;
        }

        Vector3 dir = BattleManager.Inst.CurrentPlayer.transform.position - transform.position;
        dir.y = 0;
        while (true)
        {
            MoveToDirection(dir);
            yield return null;
        }
    }


    public void LerpMoveToPos(Vector3 Des)
    {
        transform.position = Vector3.Lerp(transform.position, Des ,LerpMoveSpeed);
    }
    public void MoveForaward()
    {
        transform.position += transform.forward * Time.deltaTime * Speed;
    }
    public void MoveToDirection(Vector3 Dir)
    {
        transform.position += Speed*Dir.normalized*Time.deltaTime;
    }

    private void OnDestroy()
    {
        if(DeathPro!=null&&EmmitProOnDeath)
        {
            //for (int i = 0; i < 4; i++)
            //{
            if (DeathBehavior == 0)
            {
                GameObject CurrentDeathPro = DeathPro.gameObject.Duplicate();
                CurrentDeathPro.transform.position = transform.position;
                //CurrentDeathPro.transform.rotation = Quaternion.Euler(0, 0 ,0);
                CurrentDeathPro.GetComponent<EliteMasterProjectile>().Init(owner);
                CurrentDeathPro.GetComponent<EliteMasterProjectile>().StartPos = CurrentDeathPro.transform.position;
                CurrentDeathPro.GetComponent<EliteMasterProjectile>().BehaviorWay = 2;
            }
            else
            if(DeathBehavior==1)
            {

                GameObject CurrentDeathPro = DeathPro.gameObject.Duplicate();
                CurrentDeathPro.transform.position = transform.position;
                //CurrentDeathPro.transform.rotation = Quaternion.Euler(0, 0 ,0);
                CurrentDeathPro.GetComponent<EliteMasterProjectile>().Init(owner);
                CurrentDeathPro.GetComponent<EliteMasterProjectile>().StartPos = CurrentDeathPro.transform.position;
                CurrentDeathPro.GetComponent<EliteMasterProjectile>().BehaviorWay = 0;
            }
            //}
        }
    }
}
