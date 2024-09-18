using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightningChainScale : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject StartPos;
    public GameObject EndPos;
    float Distance;
    
    private void Start()
    {
        Distance = Vector3.Distance(StartPos.transform.position, EndPos.transform.position);
        GetComponent<Renderer>().material.SetFloat("_Tiling",1/Distance);
    }
}
