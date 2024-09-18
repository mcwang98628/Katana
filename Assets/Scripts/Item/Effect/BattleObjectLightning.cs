

using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleObjectLightning:ItemEffect
{
    private float maxDis = 3;//闪电链链接最大距离
    private float hitInterval = 0.5f;//攻击间隔
    private float attackPowerPercentage = 0.5f;//攻击力百分比

    private List<LightningNode> AttackLine = new List<LightningNode>();
    private Dictionary<string, float> enemyLastHitTime = new Dictionary<string, float>(); 
    private List<LineRenderer> lineRendererUseObjects = new List<LineRenderer>();
    private List<LineRenderer> lineRendererUnusedObjects = new List<LineRenderer>();
    private LineRenderer lineRendererPrefab;
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        lineRendererPrefab = (Resources.Load("ChainLightningLineRenderer") as GameObject)?.GetComponent<LineRenderer>();
    }

    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        AttackLine = GetLink();
        RenderLine();
        LineAttack();
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        foreach (LineRenderer lineRenderer in lineRendererUseObjects)
            if (lineRenderer != null)
                GameObject.Destroy(lineRenderer.gameObject);
        foreach (LineRenderer lineRenderer in lineRendererUnusedObjects)
            if (lineRenderer != null)
                GameObject.Destroy(lineRenderer.gameObject);
    }

    void LineAttack()
    {
        if (AttackLine.Count == 0)
            return;
        foreach (var line in AttackLine)
        {
            var hits = Physics.RaycastAll(line.Object1.position, line.Object2.position);
            foreach (RaycastHit raycastHit in hits)
            {
                if (raycastHit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                    continue;
                var targetRole = raycastHit.collider.GetComponent<RoleController>();
                if (targetRole == null)
                    continue;
                HitEnemy(targetRole);
            }
        }
        
    }

    void HitEnemy(RoleController target)
    {
        if (enemyLastHitTime.ContainsKey(target.TemporaryId) && Time.time - enemyLastHitTime[target.TemporaryId] < hitInterval)
            return;
        if (!enemyLastHitTime.ContainsKey(target.TemporaryId))
            enemyLastHitTime.Add(target.TemporaryId,Time.time);
        enemyLastHitTime[target.TemporaryId] = Time.time;
        DamageInfo dmg = new DamageInfo(
            target.TemporaryId,
            roleController.AttackPower * attackPowerPercentage, 
            roleController, 
            target.transform.position);
        target.HpInjured(dmg);
    }
    void RenderLine()
    {

        for (int i = lineRendererUseObjects.Count - 1; i >= 0; i--)
            RecoverLineRenderer(lineRendererUseObjects[i]);
        
        if (AttackLine.Count == 0)
            return;
        for (int i = 0; i < AttackLine.Count; i++)
        {
            var line = GetLineRenderer();
            line.positionCount = 2;
            line.SetPosition(0,AttackLine[i].Object1.position);
            line.SetPosition(1,AttackLine[i].Object2.position);
        }
    }

    List<LightningNode> GetLink()
    {
        List<Transform> battleObjects = new List<Transform>();
        foreach (KeyValuePair<BattleObjectType,Dictionary<Guid,IBattleObject>> valuePair in BattleManager.Inst.BattleObject)
            foreach (var battleObject in valuePair.Value)
                battleObjects.Add(battleObject.Value.ObjectTransform);

        Dictionary<int, int> objLinkTimes = new Dictionary<int, int>();
        List<LightningNode> LightningNodes = new List<LightningNode>();
        
        for (int i = 0; i < battleObjects.Count; i++)
        {
            var objTransform = battleObjects[i];
            int objId = objTransform.GetInstanceID();

            float minDis = 99;
            Transform obj2Transform = null;
            for (int j = 0; j < battleObjects.Count; j++)
            {
                if (j==i)
                    continue;
                if (LightningNodes.Count > 0 && LightningNodes[LightningNodes.Count-1].Object1 == battleObjects[j])
                    continue;
                int obj2Id = battleObjects[j].GetInstanceID();
                if (objLinkTimes.ContainsKey(obj2Id) && objLinkTimes[obj2Id] >= 2)                    
                    continue;

                float magnitude = (battleObjects[j].position - objTransform.position).magnitude;
                if (minDis > magnitude && maxDis >= magnitude)
                {
                    minDis = magnitude;
                    obj2Transform = battleObjects[j];
                }
            }

            if (obj2Transform == null)
                continue;

            LightningNode lightningNode = new LightningNode();
            lightningNode.Object1 = objTransform;
            lightningNode.Object2 = obj2Transform;
            
            LightningNodes.Add(lightningNode);

            if (!objLinkTimes.ContainsKey(objId))
                objLinkTimes.Add(objId,0);
            objLinkTimes[objId]++;
            
            int newObjId = obj2Transform.GetInstanceID();
            if (!objLinkTimes.ContainsKey(newObjId))
                objLinkTimes.Add(newObjId,0);
            objLinkTimes[newObjId]++;
            
        }

        return LightningNodes;
    }

    LineRenderer GetLineRenderer()
    {
        LineRenderer obj = null;
        if (lineRendererUnusedObjects.Count > 0)
        {
            obj = lineRendererUnusedObjects[lineRendererUnusedObjects.Count - 1];
            lineRendererUnusedObjects.Remove(obj);
        }
        else
        {
            obj = GameObject.Instantiate(lineRendererPrefab);
        }
        lineRendererUseObjects.Add(obj);
        obj.gameObject.SetActive(true);
        return obj;
    }

    void RecoverLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.gameObject.SetActive(false);
        lineRenderer.positionCount = 0;
        lineRendererUseObjects.Remove(lineRenderer);
        lineRendererUnusedObjects.Add(lineRenderer);
    }
    
}

public class LightningNode
{
    public Transform Object1;
    public Transform Object2;
}