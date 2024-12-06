using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrailer : MonoBehaviour
{
    public GameObject center;

    public float rotateSpeed;
    
    public float duration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        if (duration > 0)
        {
            transform.RotateAround(center.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}
