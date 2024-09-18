using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class BreakableObj : MonoBehaviour
{
    public bool IsBroken;
    [Header("破坏后")]
    public GameObject Shard;
    [Header("破坏前")]
    public GameObject Model;
    [Header("声音")]
    public AudioClip BreakSound;
    [Header("破坏粒子")]
    public GameObject BreakParticles;


    protected List<GameObject> shards;
    protected virtual void Start()
    {
        BreakableObjManager.Inst.AddBreakableObj(this);
    }
    private void OnDestroy()
    {
        BreakableObjManager.Inst.RemoveBreakableObj(this);
    }
    public virtual void BreakObj()
    {
        IsBroken = true;
        SpawnObj();
        SwitchModel();
        PlayPartical();
        PlaySound();
        BreakableObjManager.Inst.PlayFeedBack();
    }


    protected void SpawnObj()
    {
        EventManager.Inst.DistributeEvent(EventName.OnBreakableObjBreake, transform.position);
    }
    protected void SwitchModel()
    {
        if (Model != null)
        {
            Model.SetActive(false);
        }
        if (Shard != null)
        {
            Shard.SetActive(true);
        }
    }
    protected virtual void PlayPartical()
    {
        if (BreakParticles != null)
        {
            Instantiate(BreakParticles, transform.position, Quaternion.identity);
        }
    }
    protected void PlaySound()
    {
        if (BreakSound != null)
            AudioManager.Inst.PlaySource(BreakSound, 1, 100);
    }


}
