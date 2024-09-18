

using Sirenix.OdinInspector;
using UnityEngine;

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.KillRewardTagEffect)] 
    [LabelText("悬赏标记")]
    public GameObject KillRewardTagObject;
}

//悬赏标记
public class KillRewardTagEffect:ItemEffect
{
    private GameObject _tagObjectPrefab;

    private GameObject _currentObject;
    public KillRewardTagEffect(GameObject tagObject)
    {
        _tagObjectPrefab = tagObject;
    }
    

    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        foreach (var enemy in BattleManager.Inst.EnemyTeam)
        {
            if (enemy.Value.IsDie)
            {
                continue;
            }
            if (IsHaveKillRewardTag(enemy.Value))
                return;
        }
            
        
        if (_currentObject != null)
            GameObject.Destroy(_currentObject);
        
        foreach (var enemy in BattleManager.Inst.EnemyTeam)
        {
            if (enemy.Value.IsDie)
            {
                continue;
            }
            enemy.Value.AddTag(RoleTagName.KillReward);
            var obj = GameObject.Instantiate(_tagObjectPrefab, enemy.Value.transform);
            obj.transform.localPosition = Vector3.zero;
            _currentObject = obj;
            return;
        }
        
    }

    private bool IsHaveKillRewardTag(RoleController targetRole)
    {
        int count = targetRole.GetTagCount(RoleTagName.KillReward);
        return count > 0;
    }
    
}