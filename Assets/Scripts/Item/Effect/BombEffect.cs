using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : ItemEffect
{
    private DmgBuffOnTouch BombExplosion;
    private ThrowProjectile BombThrowProjectile;
    private float BombMoveTime;
    private float BombHeight;
    private float BombPosOffset;
    private SpwanPosType BombDesPosType;
    private AudioClip ThrowSound;
    private int BombCount;
    public BombEffect(int bombCount,DmgBuffOnTouch dmgBuffOnTouch,ThrowProjectile throwProjectile,float bombMoveTime,float bombHeight,float bombPosOffset,SpwanPosType spawnPosType,AudioClip throwSound)
    {
        BombExplosion = dmgBuffOnTouch;
        BombThrowProjectile = throwProjectile;
        BombMoveTime = bombMoveTime;
        BombHeight = bombHeight;
        BombPosOffset = bombPosOffset;
        BombDesPosType = spawnPosType;
        BombCount = bombCount;
        ThrowSound = throwSound;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        for(int i=0;i<BombCount;i++)
        {
            roleController.StartCoroutine(DelaySpwanBomb(0.15f*i));
        }
    }

    IEnumerator DelaySpwanBomb(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Vector3 desPos = roleController.transform.position;
        switch (BombDesPosType)
        {
            case SpwanPosType.RoleCenter:               
                break;
            case SpwanPosType.EnemyTarget:
                if(((PlayerController)roleController).EnemyTarget)
                    desPos = ((PlayerController)roleController).EnemyTarget.transform.position;
                break;
            case SpwanPosType.RandomEnemy:
                RoleController target = BattleTool.GetRandomEnemy(roleController, 15);
                if (target)
                    desPos = target.transform.position;
                break;
            default:
                break;
        }
       
        desPos += new Vector3(Random.Range(-BombPosOffset, BombPosOffset), 0, Random.Range(-BombPosOffset, BombPosOffset));
        var throwProjectile = GameObject.Instantiate(BombThrowProjectile);
        throwProjectile.transform.position = roleController.transform.position;
        desPos = BombHeight > 0 ? desPos : throwProjectile.transform.position;

        if(ThrowSound!=null)
        {
            AudioManager.Inst.PlaySource(ThrowSound);
        }
        throwProjectile.Init(
            desPos
            , BombMoveTime
            , BombHeight
            , BombExplosion.gameObject
            ,BombExplosion.radius
            ,BombExplosion.Damage.DmgValue
            ,roleController
            );
    } 
    
}
