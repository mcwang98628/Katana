using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceShader : MonoBehaviour
{
    public Shader ReplacementShader;

    void OnEnable()
    {
        if (ReplacementShader != null)
        {
            GetComponent<Camera>().SetReplacementShader(ReplacementShader, "RenderType");
            //1.GetComponent<Camera>().SetReplacementShader(ReplacementShader, "MyType");
            //GetComponent<Camera>().camera.RenderWithShader(ReplacementShader, "RenderType");
        }
    }

    void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
