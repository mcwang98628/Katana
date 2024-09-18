using Sirenix.OdinInspector;
using UnityEngine;


public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.GoldManuscript)]
    [LabelText("Prefab")]
    public GameObject GoldManuscriptPrefab;
    [ShowIf("EffectType", EffectType.GoldManuscript)]
    [LabelText("概率")]
    public int GoldManuscriptProbability;
}

public class GoldManuscript:ItemEffect
{
    private GameObject _prefab;
    private int _probability;
    public GoldManuscript(GameObject prefab,int probability)
    {
        _prefab = prefab;
        _probability = probability;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);

        int randomValue = Random.Range(0, 100);
        if (randomValue > _probability)
            return;
        
        if (BattleManager.Inst.CurrentRoom is FightRoom fightRoom)
        {
            int childIndex = Random.Range(0, fightRoom.Enemypoints.childCount);
            var pos = fightRoom.Enemypoints.GetChild(childIndex).transform.position;
            var go = GameObject.Instantiate(_prefab,fightRoom.transform);
            go.transform.position = pos;
        }
    }
}