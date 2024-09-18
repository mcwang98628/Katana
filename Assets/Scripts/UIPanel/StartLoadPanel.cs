using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLoadPanel : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        EventManager.Inst.AddEvent(EventName.OnAppStart,OnAppStart);
        StartCoroutine(waitInit());
    }
    
    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnAppStart,OnAppStart);
    }
    
    private void OnAppStart(string arg1, object arg2)
    {
        StartCoroutine(WaitDestroy());
    }

    IEnumerator WaitDestroy()
    {
        yield return null;//new WaitForSecondsRealtime(1f);
        Destroy(this.gameObject);
    }

    IEnumerator waitInit()
    {
        yield return null;
        yield return null;
        SceneManager.LoadSceneAsync("GameStart");
    }
}
