using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeOrbShadow : MonoBehaviour
{
    public GameObject Shadow;
    private void FixedUpdate()
    {
        if(Shadow!=null)
        Shadow.transform.position = new Vector3(transform.position.x,0.1f,transform.position.z);
    }
}
