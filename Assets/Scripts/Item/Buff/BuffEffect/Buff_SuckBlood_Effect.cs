
public class Buff_SuckBlood_Effect:BuffEffect
{
    public override void Awake()
    {
        base.Awake();
        EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnEnemyInjured);
    }

    public override void Destroy()
    {
        base.Destroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnEnemyInjured);
    }

    private float _suckBloodValue;
    public Buff_SuckBlood_Effect(float value)
    {
        _suckBloodValue = value;
    }
    
    private void OnEnemyInjured(string arg1, object arg2)
    {
        RoleInjuredInfo infoData = (RoleInjuredInfo) arg2;
        if (infoData.RoleId == roleBuff.roleController.TemporaryId)
            return;
        if (infoData.Dmg.DmgType != DmgType.Physical)
            return;
        
        TreatmentData data = new TreatmentData(infoData.Dmg.DmgValue * _suckBloodValue,roleBuff.roleController.TemporaryId);
        roleBuff.roleController.HpTreatment(data);
    }
    
    
}