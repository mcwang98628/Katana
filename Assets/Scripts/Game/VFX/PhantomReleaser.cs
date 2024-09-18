using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomReleaser : MonoBehaviour
{
    public GameObject Phantom;
    public float TimeBetween;
    public GameObject RotObj;
    public bool EmmitOnStart;
    SkinnedMeshRenderer _renderer;
    



    private void Start()
    {
        _renderer = GetComponent<SkinnedMeshRenderer>();
       if(EmmitOnStart)
        StartCoroutine(ReleasePhantom());
    }
    public void StartEmmitPhantom()
    {
        StopAllCoroutines();
        StartCoroutine(ReleasePhantom());
    }
    public void EndEmmitPhantom()
    {
        StopAllCoroutines();
    }
    IEnumerator ReleasePhantom()
    {
        if (TimeBetween > 0)
        {
            float StartTime = Time.time;
            float LastEmmitTime = 0;
            while (true)
            {
                if (Time.time > TimeBetween + LastEmmitTime)
                {
                    LastEmmitTime = Time.time;
                    DuplicateaPhantom();
                }
                yield return null;
            }

        }
    }
    //复制一个幻影
    public void DuplicateaPhantom()
    {
        GameObject CurrentPhantom = Instantiate(Phantom, transform.position, Quaternion.identity);
        if (_renderer != null)
        {
            Mesh BakedResult = new Mesh();
            _renderer.BakeMesh(BakedResult);
            CurrentPhantom.GetComponent<ParticleSystemRenderer>().mesh = BakedResult;
        }
        ParticleSystem.MainModule main = CurrentPhantom.GetComponent<ParticleSystem>().main;
        main.startRotationY = RotObj.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
    }


}
