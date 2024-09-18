public class Protective_Surround : Surround_Obj
{
    public override void Update()
    {
        foreach (var arrow in BattleManager.Inst.BattleThrow)
        {
            switch (arrow)
            {
                case Projectile _arrow:
                    
                    var pos = _arrow.transform.position - transform.position;
                    pos.y=0;
                    if (pos.magnitude <= distance)
                    {
                        Destroy(_arrow.gameObject);
                    }
                    break;
                case ThrowProjectile _throwPro:
                    break;
            }
        }
    }
}