using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTiming : MonoBehaviour
{
    [SerializeField] private float time;

    private void Start()
    {
        StartCoroutine(waitTime());
    }

    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
