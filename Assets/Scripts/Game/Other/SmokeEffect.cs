using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class SmokeEffect:MonoBehaviour
{
    public float duration;
    public float radius;
    [LabelText("闪避值0-100"),MaxValue(100)]
    public float value;
    
    private float timer=0;

    private RoleController playerRole;
    private AttributeBonus attb;
    private ParticleSystem[] particalSys;
    
    private void Start()
    {
        particalSys = GetComponentsInChildren<ParticleSystem>();
        if (duration<=0)
        {
            Debug.LogWarning($"SmokeEffect duration={duration}");
            Destroy(gameObject);
            return;
        }
        attb = new AttributeBonus {Type = AttributeType.DodgeProbability, Value = value};
    }

    private void OnDestroy()
    {
        
        foreach (var t in particalSys)
        {
            t.Stop();
        }
        
    }


    private bool entered;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer>duration)
        {
            Destroy(gameObject,1);
        }

        if (BattleManager.Inst.CurrentPlayer)
        {
            var dis = BattleManager.Inst.CurrentPlayer.transform.position - transform.position;
            dis.y = 0;
            if (dis.magnitude<=radius&&!entered)
            {
                entered = true;
                EnterRange();
            }
            else
            {
                entered = false;
                ExitRange();
            }
        }
    }

    void EnterRange()
    {
        //添加闪避
        BattleManager.Inst.CurrentPlayer.AddAttributeBonus(attb);
    }

    void ExitRange()
    {
        //移除闪避
        BattleManager.Inst.CurrentPlayer.RemoveAttributeBonus(attb);
    }
}