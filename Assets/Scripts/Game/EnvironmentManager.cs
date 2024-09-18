
using System.Collections;
using UnityEngine;

public class EnvironmentManager : TSingleton<EnvironmentManager>
{
    public EnvironmentItemScript CurrentEnvironment;

    public static void SetCurrentCharacterLight()
    {
        if (Inst.CurrentEnvironment != null)
            Inst.CurrentEnvironment.SetCharacterLight();
        else
        {
            Debug.LogError("aaa");
        }
    }
    
    public void SetEnvironment(EnvironmentItemScript environmentData,float time = 0)
    {
        CurrentEnvironment = environmentData;
        GameManager.Inst.StartCoroutine(DelayApplyEnvirment(time));
    }
    IEnumerator DelayApplyEnvirment(float time=0)
    {
        yield return new WaitForEndOfFrame();
        while (BattleManager.Inst.CurrentPlayer == null)
        {
            yield return null;
        }
        CurrentEnvironment.ApplyEnvironment(time);
    }
    
    public void Apply()
    {
        if (CurrentEnvironment != null)
            GameManager.Inst.StartCoroutine(DelayApplyEnvirment());
        else
            Debug.Log("当前环境数据为空");
    }
}
