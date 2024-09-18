using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PhantomVFX : MonoBehaviour
{
    private bool Emit;
    public float Interval;
    public float PhantomLifeTime;
    public Color PhantomColor1;
    public Color PhantomColor2;
    float timer;
    public SkinnedMeshRenderer targetRenderer;
    public Material PhantomMat;
    List<SkinnedMeshRenderer> phantomList;
    void Start()
    {
        Emit=false;
        phantomList = new List<SkinnedMeshRenderer>();
        //targetRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Emit && Time.time > timer)
        {
            CreatePhantom();
            timer = Time.time + Interval;
        }
    }

    public void CreatePhantom()
    {
        GameObject obj = new GameObject();
        obj.transform.position = targetRenderer.transform.position;
        obj.transform.rotation = targetRenderer.transform.rotation;

        SkinnedMeshRenderer renderer = obj.AddComponent<SkinnedMeshRenderer>();
        Mesh bakedMesh = new Mesh();
        targetRenderer.BakeMesh(bakedMesh);
        renderer.sharedMesh = bakedMesh;
        renderer.material = PhantomMat;
        renderer.material.SetColor("_BaseColor",PhantomColor1);
        renderer.material.DOColor(new Color(PhantomColor2.r, PhantomColor2.g, PhantomColor2.b, 0), "_BaseColor", PhantomLifeTime)
            .SetEase(Ease.InQuart)   
            .OnComplete(() =>
            {
                Destroy(renderer.gameObject);
            });
    }

    protected virtual void Awake()
    {
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
    }

    protected virtual void OnDestroy()
    {
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
    }
    protected virtual void AnimEvent(GameObject go, string eventName)
    {
        if (eventName.Contains("PlayPhantom_"))
        {
            eventName = eventName.Replace("PlayPhantom_","");
            int time = int.Parse(eventName);
            Emit=true;
            StartCoroutine(DelayEmitOff(time));
        }
    }

    IEnumerator DelayEmitOff(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Emit=false;
    }

    public void StartPhantomVFX(float interval,float phantomlifeTime,float time)
    {
        Interval = interval;
        PhantomLifeTime = phantomlifeTime;
        Emit = true;
        StartCoroutine(DelayEmitOff(time));
    }


}
