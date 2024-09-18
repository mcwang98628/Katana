using BehaviorDesigner.Runtime.Tasks.Unity.UnityLight;
using UnityEngine;
using UnityEngine.UI;

public class UI_Battle_JumpText : MonoBehaviour
{
    public bl_HUDText HUDRoot;
    
    [Header("停留时间")]
    public float FastTextTime = 0.25f;
    public float NormalTextTime = 0.5f;
    public float SlowTextTime = 0.8f;
     [Header("字号")]
    public int BigTextSize=72;
    public int NormalTextSize=32;
    public int SmallTextSize=36;
    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnRoleInjured);
        EventManager.Inst.AddEvent(EventName.OnRoleDodgeInjured, OnRoleDodgeInjured);
        EventManager.Inst.AddEvent(EventName.OnRoleTreatment, OnRoleTreatment);
        EventManager.Inst.AddEvent(EventName.OnAddMoney, OnAddMoney);
        EventManager.Inst.AddEvent(EventName.NeedPower, OnNeedPower);
        EventManager.Inst.AddEvent(EventName.OnRoleDead, OnRoleDead);
        EventManager.Inst.AddEvent(EventName.OnShieldDefense, OnRoleShield);
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnRoleInjured);
        EventManager.Inst.RemoveEvent(EventName.OnRoleDodgeInjured,OnRoleDodgeInjured);
        EventManager.Inst.RemoveEvent(EventName.OnRoleTreatment, OnRoleTreatment);
        EventManager.Inst.RemoveEvent(EventName.OnAddMoney, OnAddMoney);
        EventManager.Inst.RemoveEvent(EventName.NeedPower, OnNeedPower);
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead, OnRoleDead);
        EventManager.Inst.RemoveEvent(EventName.OnShieldDefense, OnRoleShield);
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
    }
    public void AnimEvent(GameObject go,string eventName)
    {
        if(eventName.Split('_')[0]!="ShowText")
        {
            return;
        }
        string content;
        Color color;
        content = eventName.Split('_')[1];
        color = (Color)typeof(Color).GetProperty(eventName.Split('_')[2].ToLowerInvariant()).GetValue(null, null);
        ShowText(go,NormalTextSize,content,color);
    }
    private void OnRoleDead(string arg1, object roleDeadEventData)
    {
        RoleDeadEventData eventData = (RoleDeadEventData)roleDeadEventData;
        if (eventData.AttackerRole == null || eventData.DeadRole == null)
            return;

        if (eventData.DeadRole == eventData.AttackerRole)
        {
            HUDRoot.NewText(LocalizationManger.Inst.GetText("Tip_Suicide"), eventData.DeadRole.transform, Color.red, NormalTextSize, SlowTextTime,  bl_Guidance.Up);
        }
        else if (eventData.DeadRole.roleTeamType == eventData.AttackerRole.roleTeamType)
        {
            HUDRoot.NewText(LocalizationManger.Inst.GetText("Tip_Cannibalism"), eventData.DeadRole.transform, Color.red, NormalTextSize, SlowTextTime, bl_Guidance.Up);
        }
        HUDRoot.ClearJumpDic(eventData.DeadRole.transform);

    }
    public void OnRoleShield(string arg1, object arg2)
    {
        ShowText(BattleManager.Inst.CurrentPlayer.gameObject,NormalTextSize,"格 挡！",Color.white);
    }
    private void OnRoleDodgeInjured(string arg1, object arg2)
    {
        var data = (RoleInjuredInfo)arg2;
        if (data.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }

        HUDRoot.NewText(LocalizationManger.Inst.GetText("Tip_Evade"), BattleManager.Inst.CurrentPlayer.transform, new Color(0,0.75f,1), NormalTextSize, NormalTextTime, bl_Guidance.Up);
    }
    public void ShowText(GameObject go,int fontsize, string text,Color color)
    {
        HUDRoot.NewText(text, go.transform, color, 36, NormalTextTime, bl_Guidance.Up);
    }


    private void OnNeedPower(string arg1, object arg2)
    {
        HUDRoot.NewText(LocalizationManger.Inst.GetText("Tip_RollCd"), BattleManager.Inst.CurrentPlayer.transform, new Color(1f, 1, 1), NormalTextSize, FastTextTime,  bl_Guidance.Up);
    }

    private void OnRoleTreatment(string arg1, object arg2)
    {
        var data = (TreatmentData)arg2;
        RoleController target = null;
        
        if (BattleManager.Inst.PlayerTeam.ContainsKey(data.RoleId))
        {
            target = BattleManager.Inst.PlayerTeam[data.RoleId];
        }
        else if (BattleManager.Inst.EnemyTeam.ContainsKey(data.RoleId))
        {
            return;
            target = BattleManager.Inst.EnemyTeam[data.RoleId];
        }
        else
        {
            //Debug.LogError("空？");
            return;
        }

        if (data.TreatmentValue>0)
        {
            HUDRoot.NewText("+" + (int)data.TreatmentValue, target.transform, new Color(0.4f, 0.8f, 0.2f), NormalTextSize, FastTextTime, bl_Guidance.Random);
        }
        else
        {
            HUDRoot.NewText("" + (int)data.TreatmentValue, target.transform, new Color(0.95f,0.2f,0.14f), NormalTextSize, FastTextTime, bl_Guidance.Random);
        }
    }
    private void OnAddMoney(string arg1, object arg2)
    {
        var value = (int)arg2;
        HUDRoot.NewText((value>0?"+":"") + (int)value, BattleManager.Inst.CurrentPlayer.transform, new Color(1,0.85f,0), NormalTextSize, SlowTextTime, bl_Guidance.Random);
    }

    public void OnRoleInjured(string eventName,object value)
    {
        var dmgInfo = (RoleInjuredInfo) value;
        
        if (dmgInfo.Dmg.DmgValue==0)
        {
            return;
        }
        if (dmgInfo.Dmg.DmgType == DmgType.Fire ||
            dmgInfo.Dmg.DmgType == DmgType.Other ||
            dmgInfo.Dmg.DmgType == DmgType.Vampire ||
            dmgInfo.Dmg.DmgType == DmgType.ThunderWeak)
        {
            //小伤害不显示数字
            if (dmgInfo.Dmg.DmgValue < 10)
            {
                return;
            }
        }
        
        bool isPlayer = BattleManager.Inst.CurrentPlayer.TemporaryId == dmgInfo.RoleId;
        if (!BattleManager.Inst.EnemyTeam.ContainsKey(dmgInfo.RoleId) &&
            !isPlayer)
        {
            return;
        }

        int textSize = NormalTextSize;
        Color textColor = Color.white;
        bl_Guidance textDir;
        Transform target;
        float lifeTime = FastTextTime;
        //暂时移除随机
        //if (Random.Range(0, 2) == 1)
        //{
        //    textDir = bl_Guidance.RightDown;
        //}
        //else
        //{
        //    textDir = bl_Guidance.LeftDown;
        //}

        textDir = bl_Guidance.Up;

        if (isPlayer)
        {
            textSize = NormalTextSize;
            textColor = new Color(0.95f,0.2f,0.14f);
            target = BattleManager.Inst.CurrentPlayer.transform;
            textDir = bl_Guidance.Up;
        }
        else
        {
            target = BattleManager.Inst.EnemyTeam[dmgInfo.RoleId].transform;
        }
        //加上偏移！！！！
        //GameObject newObj = new GameObject();
        //newObj.transform.position = target.position + Vector3.up * 0f;
        //newObj.transform.SetParent(target);
        
        
        string TextContent =((int)dmgInfo.Dmg.DmgValue).ToString();
        if (dmgInfo.Dmg.IsCrit)
        {
            textColor = new Color(0.95f, 0f, 0f);
            lifeTime = NormalTextTime;
            textSize = BigTextSize;
            TextContent += "！";
            //TextContent = TextContent;
        }


        //HUDRoot.NewText(TextContent, target, textColor, textSize,  lifeTime , textDir);
        HUDRoot.NewText(TextContent, target, textColor, textSize, lifeTime, textDir,dmgInfo.Dmg.IsCrit);
    }
}
