using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public float RotSpeed;
    public bool UseUnscaledDeltaTime;
    public Transform AxisTrans;
    private void Update()
    {
        if (!UseUnscaledDeltaTime)
        {
            transform.RotateAround(AxisTrans.forward,RotSpeed*Time.deltaTime);  
        }
        else
        {
            transform.RotateAround(AxisTrans.forward,RotSpeed*Time.unscaledDeltaTime);    
        }
    }
}
