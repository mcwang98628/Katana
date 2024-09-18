using UnityEngine;

public class PhoenixEffect : ItemEffect
{
    private GameObject PhoenixPrefab;
    private GameObject PhoenixEmmitVFX;
    private Vector3 offset = new Vector3(0, 0.8f, 0);

    private int AttackPower;
    private float AttackPowerPercentage;
    
    public PhoenixEffect(GameObject prefab, GameObject fx,int attackPower,float attackPowerPercentage)
    {
        PhoenixPrefab = prefab;
        PhoenixEmmitVFX = fx;
        AttackPower = attackPower;
        AttackPowerPercentage = attackPowerPercentage;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        var player = BattleManager.Inst.CurrentPlayer;
        var playerForward = player.roleNode.Model.forward;
        var playerRotation = player.roleNode.Model.rotation;
        GameObject projectile = GameObject.Instantiate(PhoenixPrefab, player.transform.position + offset + playerForward*2, playerRotation);
        if (PhoenixEmmitVFX != null)
        {
            GameObject.Instantiate(PhoenixEmmitVFX, player.transform.position + offset + playerForward * 2, playerRotation);
        }
        projectile.GetComponent<DmgBuffOnTouch>().Init(player,AttackPower+(int)(roleController.OriginalAttackPower*AttackPowerPercentage));
    }
}
