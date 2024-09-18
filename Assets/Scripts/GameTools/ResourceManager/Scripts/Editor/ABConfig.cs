using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ABConfig",menuName = "GameTools/ABConfig")]
public class ABConfig : ScriptableObject
{
    //prefab名字必须唯一，每个prefab都会单独打一个Bundle包。
    public List<string> m_AllPrefabPath = new List<string>();
    public List<FileDirABName> m_AllFileDirAB = new List<FileDirABName>();

    [System.Serializable]
    public struct FileDirABName
    {
        public string ABName;
        public string Path;
    }
}
