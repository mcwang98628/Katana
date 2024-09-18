using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMeshOnStart : MonoBehaviour
{
    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
