using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BlackHoleDmg : DmgBuffOnTouch
{
     bool Absorded;
    public GameObject BlackHoleDeath;
    //public GameObject ScaleCurve;
    protected override void doDamageAndBuff(List<RoleController> targets)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (!Absorded)
            {
                //记录和判断 目标触发时间是否在限制间隔内
                if (Targets.ContainsKey(targets[i].TemporaryId))
                {
                    if (Time.time - Targets[targets[i].TemporaryId] >= interval)
                    {
                        Targets[targets[i].TemporaryId] = Time.time;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Targets.Add(targets[i].TemporaryId, Time.time);
                }

                if (buffObj != null)
                {
                    // buffObj.LifeCycle = LifeCycle;
                    var buff = DataManager.Inst.ParsingBuff(buffObj,LifeCycle);
                    targets[i].roleBuffController.AddBuff(buff, owner);
                }
                if (Damage != null)
                {
                    DamageInfo dmg= new DamageInfo(targets[i].TemporaryId,Damage.DmgValue, owner, transform.position,Damage.DmgType,Damage.Interruption,Damage.IsUseMove,Damage.MoveTime,Damage.MoveSpeed,Damage.IsRemotely,Damage.RemotelyObject);
                    
                    OtherEffect(targets[i]);
                    targets[i].HpInjured(dmg);
                }
            }
        }
    }
    public override void OtherEffect(RoleController _controller)
    {
        if (Absorded != true)
        {
            if (_controller.tag != "Player")
            {
                _controller.transform.DOScale(0, 1.5f);
                _controller.transform.DOMove(transform.position, 0.5f);
                BlackHoleDeath.DuplicateUnderTransform(transform);
                ParticleToolkit.DisableEmmisions(gameObject);
                Absorded = true;
                Destroy(this);
            }
        }
    }
}
