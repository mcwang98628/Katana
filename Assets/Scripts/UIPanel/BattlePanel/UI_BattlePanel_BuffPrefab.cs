using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePanel_BuffPrefab : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    private RoleBuff buffData;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="buffId">唯一ID</param>
    public void Init(int buffId)
    {
        RoleBuff buff = null;
        DataManager.Inst.BuffDatas.TryGetValue(buffId,out buff);
        if (buff==null)
        {
            Debug.LogError("Buff数据错误");
            return;
        }

        buffData = buff;
        icon.sprite = buffData.sprite;
        gameObject.SetActive(true);
    }

    public void ShowInfo()
    {
        // infoPanel.Open(buffData.sprite,"Buff:"+buffData.name,buffData.desc);
    }
}
