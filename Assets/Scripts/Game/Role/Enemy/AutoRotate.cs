using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public Vector3 RotateAxis;
    public bool UseUnscaledDeltaTime;
    private void Update()
    {
        if (!UseUnscaledDeltaTime)
        {
            transform.Rotate(RotateAxis*Time.deltaTime);   
        }
        else
        {
            transform.Rotate(RotateAxis*Time.unscaledDeltaTime);   
        }
    }
}
