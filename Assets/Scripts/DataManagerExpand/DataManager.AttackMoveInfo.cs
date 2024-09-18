using System;

public partial class DataManager : TSingleton<DataManager>
{
        
    public void LoadAttackInfo(string fileName,Action<AttackInfo> callback)
    {            
        ResourcesManager.Inst.GetAsset<AttackInfo>("Assets/AssetsPackage/ScriptObject_Combat/AttackInfo/Enemy/" + fileName + ".asset",
        delegate(AttackInfo ai)
        {
            if (ai == null)
            {
                ResourcesManager.Inst.GetAsset<AttackInfo>("Assets/AssetsPackage/ScriptObject_Combat/AttackInfo/Player/" + fileName + ".asset",
                    delegate(AttackInfo info)
                    {
                        callback?.Invoke(info);
                    });
            }

            if (ai!=null)
            {
                callback?.Invoke(ai);
            }
        });
    }

    public void LoadMoveInfo(string fileName,Action<MoveInfo> callBack)
    {
        ResourcesManager.Inst.GetAsset<MoveInfo>("Assets/AssetsPackage/ScriptObject_Combat/MoveInfo/" + fileName + ".asset",
            delegate(MoveInfo mi)
            {
                callBack?.Invoke(mi);
            });
    }

}