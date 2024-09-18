
using System.Collections;
using UnityEngine;

public class EmmitThunderEffect : ItemEffect
{
    private int MaxCount;
    private int MinCount;
    private GameObject VerticleThunder;
    private int AttackPower;
    private float AttackPowerPercentage;
    public EmmitThunderEffect(GameObject prefab, int attackPower, float attackPowerPercentage, int maxCount, int minCount)
    {
        MaxCount = maxCount;
        MinCount = minCount;
        VerticleThunder = prefab;
        AttackPower = attackPower;
        AttackPowerPercentage = attackPowerPercentage;
    }


    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.StartCoroutine(WaitEmiitThunder());
    }


    //每个闪电之间有间隔
    IEnumerator WaitEmiitThunder()
    {
        if (BattleManager.Inst.CurrentPlayer.EnemyTarget != null)
        {
            foreach (var target in BattleTool.GetRandomEnemys(roleController, UnityEngine.Random.Range(MinCount, MaxCount),6))
            {
                if (target != null)
                {

                    yield return new WaitForSeconds(0.2f);

                    GameObject thunder = GameObject.Instantiate(VerticleThunder,
                    target.transform.position,
                    Quaternion.identity);

                    thunder.GetComponent<DmgBuffOnTouch>().Init(BattleManager.Inst.CurrentPlayer, AttackPower + (int)(roleController.OriginalAttackPower * AttackPowerPercentage));

                }
            }
        }
        else
        {
            GameObject thunder = GameObject.Instantiate(VerticleThunder,
            roleController.transform.position + new Vector3(Random.value + 0.5f, 0, Random.value + 0.5f),
            Quaternion.identity);
            GameObject.Destroy(thunder,1f);

            //thunder.GetComponent<DmgBuffOnTouch>().Init(BattleManager.Inst.CurrentPlayer, AttackPower + (int)(roleController.OriginalAttackPower * AttackPowerPercentage));
        }
    } 
}
