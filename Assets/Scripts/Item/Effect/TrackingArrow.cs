using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TrackingArrowEffect : ItemEffect
{
    private GameObject _arrow;
    private AudioClip _audioClip;
    public TrackingArrowEffect(AudioClip audioClip,GameObject arrow)
    {
        _arrow = arrow;
        _audioClip = audioClip;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (value.HasValue && ! string.IsNullOrEmpty(value.Value.TargetId))
        {
            Trigger(value.Value.TargetId);
        }
        else
        {
            var role = BattleTool.FindNearestEnemy(roleController);
            if (role!=null)
            {
                Trigger(role.TemporaryId);
            }
        }
    }
 
    void Trigger(string targetId)
    {
        if (!BattleManager.Inst.EnemyTeam.ContainsKey(targetId))
        {
            return;
        }

        var role = BattleManager.Inst.EnemyTeam[targetId];
        var arrowGo = GameObject.Instantiate(_arrow, role.transform.position + Vector3.up * 80f, Quaternion.identity);
        arrowGo.transform.SetParent(BattleManager.Inst.CurrentRoom.transform);
        
        IndicatorManager.Inst.ShowAttackIndicator().Show(roleController,role.transform.position,360,1,0.5f, Color.red);
        arrowGo.transform.DOMove(role.transform.position, 0.5f).OnComplete(() =>
        {
            // BattleManager.Inst.BattleData.BattleObjectRegistered(BattleObjectType.Arrow,arrowGo);
            List<RoleController> hitRoles = new List<RoleController>();
            foreach (var controller in BattleManager.Inst.EnemyTeam)
            {
                if ((controller.Value.transform.position - arrowGo.transform.position).magnitude <= 1f)
                {
                    DamageInfo damageInfo = new DamageInfo(controller.Value.TemporaryId,roleController.AttackPower,roleController,arrowGo.transform.position);
                    controller.Value.HpInjured(damageInfo);
                }
            }
        });
        AudioManager.Inst.PlaySource(_audioClip,1);


    }
}

public class ArrowLightningChain:ItemEffect
{
    private UVChainLightning _lineRenderer;
    public ArrowLightningChain()
    {
        GameObject fx = Resources.Load("ChainLightning") as GameObject;

        _lineRenderer = GameObject.Instantiate(fx).GetComponent<UVChainLightning>();
        _lineRenderer.gameObject.SetActive(false);
        GameObject.DontDestroyOnLoad(_lineRenderer.gameObject);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        GameObject.Destroy(_lineRenderer.gameObject);
    }

    private Coroutine _coroutine;
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (!BattleManager.Inst.BattleObject.ContainsKey(BattleObjectType.Arrow) || 
            BattleManager.Inst.BattleObject[BattleObjectType.Arrow].Count < 2)
        {
            return;
        }

        if (_coroutine!=null)
        {
            GameManager.Inst.StopCoroutine(_coroutine);
        }
        _coroutine = GameManager.Inst.StartCoroutine(LightningChain());
    }

    IEnumerator LightningChain()
    {
        List<IBattleObject> arrows = BattleManager.Inst.BattleObject[BattleObjectType.Arrow].Values.ToList();
        
        _lineRenderer.gameObject.SetActive(true);
        for (int i = 0; i < arrows.Count-1; i++)
        {
            if (arrows[i] == null)
            {
                continue;
            }
            _lineRenderer.start.position = arrows[i].ObjectTransform.position + Vector3.up;
            _lineRenderer.end.position = arrows[i+1].ObjectTransform.position+Vector3.up;
            var dir = arrows[i + 1].ObjectTransform.position - arrows[i].ObjectTransform.position;
            RaycastHit hit;
            // Debug.DrawLine()
            bool grounded = Physics.Raycast(arrows[i].ObjectTransform.position+Vector3.up, dir.normalized, out hit, dir.magnitude, 1 << LayerMask.NameToLayer("Enemy"));
            if (grounded)
            {
                var role = hit.collider.gameObject.GetComponent<RoleController>();
                if (role)
                {
                    DamageInfo damageInfo = new DamageInfo(role.TemporaryId,roleController.AttackPower,roleController,role.transform.position);
                    role.HpInjured(damageInfo);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        
        yield return new WaitForSeconds(0.5f);
        _lineRenderer.gameObject.SetActive(false);
    }
 
}

public class ArrowAOE : ItemEffect
{
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (!BattleManager.Inst.BattleObject.ContainsKey(BattleObjectType.Arrow) || 
            BattleManager.Inst.BattleObject[BattleObjectType.Arrow].Count <= 0)
        {
            return;
        }
        List<RoleController> hits = new List<RoleController>();
        var arrowList = BattleManager.Inst.BattleObject[BattleObjectType.Arrow].Values.ToList();
        for (int i = 0; i < arrowList.Count; i++)
        {
            if (arrowList[i] == null)
            {
                continue;
            }
            IndicatorManager.Inst.ShowAttackIndicator().Show(roleController,arrowList[i].ObjectTransform.position,360,2,0.5f, Color.red);
            foreach (var enemy in BattleManager.Inst.EnemyTeam)
            {
                if (hits.Contains(enemy.Value))
                {
                    continue;
                }
                var dir = enemy.Value.transform.position - arrowList[i].ObjectTransform.position;
                if (dir.magnitude <= 2f)
                {
                    DamageInfo damageInfo = new DamageInfo(enemy.Value.TemporaryId,roleController.AttackPower,roleController,arrowList[i].ObjectTransform.position);
                    enemy.Value.HpInjured(damageInfo);
                    hits.Add(enemy.Value);
                }

            }
            
        }
    }
}
