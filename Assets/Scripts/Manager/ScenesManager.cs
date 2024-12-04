using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Inst { get; private set; }
    
    public void Init()
    {
        Inst = this;
    }
    
    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading Scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    
}
