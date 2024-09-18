using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameBuff/AllBuffObj")]
public class AllBuffObj : ScriptableObject
{
    public List<BuffScriptableObject> objs = new List<BuffScriptableObject>();
    
    
#if UNITY_EDITOR
    
    [Button("刷新")]
    public void OnBtnClick()
    {
        Debug.LogError("开始刷新Buff");
        string[] itemGuids = UnityEditor.AssetDatabase.FindAssets("t:BuffScriptableObject");
        objs.Clear();
        for (int i = 0; i < itemGuids.Length; i++)
        {
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(itemGuids[i]);
            var buff = UnityEditor.AssetDatabase.LoadAssetAtPath<BuffScriptableObject>(path);
            
            objs.Add(buff);
        }
        Debug.LogError("刷新Buff结束");
    }
    
#endif
}
