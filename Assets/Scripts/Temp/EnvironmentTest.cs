using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTest : MonoBehaviour
{
    [InlineEditor]
    public EnvironmentItemScript TestEnvironmentData;
    public void OnEnable()
    {
        if(TestEnvironmentData)
            EnvironmentManager.Inst.SetEnvironment(TestEnvironmentData);
    }
    private void Update()
    {
        if (TestEnvironmentData != null)
        {
            EnvironmentManager.Inst.Apply();
        }
    }
}
