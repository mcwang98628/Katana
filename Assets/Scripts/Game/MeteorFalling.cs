using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeteorFalling : MonoBehaviour
{
    public float FallSpeed = 1f;
    public float FallTime = 2;
    public GameObject Core;
    float NextFramePos;
    bool TouchGroundNextFrame;
    public AudioClip ExplosionSFX;
    public GameObject Explosion;
    int attackPower;
    float size;
    RoleController role;

    public List<GameObject> SpwanEnemyList;
    
    public void Init(float _size,int _attackPower,RoleController _role)
    {
        size=_size;
        attackPower=_attackPower;
        role=_role;
        // IndicatorManager.Inst.ShowAttackIndicator().SetEnable(true).SetAngle(360).SetSize(size).SetTime(FallTime).SetPosition(transform.position);
        IndicatorManager.Inst.ShowAttackIndicator().Show(role,transform.position,360,size,FallTime,Color.red);

        Core.transform.DOMove(transform.position, FallTime).SetEase(Ease.InQuart).OnComplete(() =>
        {
            Explode();
            Destroy(gameObject);
        });

    }
    public void Explode()
    {
        GameObject exp=Instantiate(Explosion, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
        if (ExplosionSFX != null)
        {
            AudioManager.Inst.PlaySource(ExplosionSFX, 1.5f);
        }

        exp.GetComponent<DmgBuffOnTouch>().Init(role,attackPower); //= new DamageInfo(attackPower,role,transform.position,DmgType.Explosion,true,true,0.1f,5);
        exp.GetComponent<DmgBuffOnTouch>().radius=size;
        exp.GetComponent<DmgBuffOnTouch>().Init(role);
        Instantiate(SpwanEnemyList[Random.Range(0,SpwanEnemyList.Count)],transform.position, Quaternion.identity );
    }
}
