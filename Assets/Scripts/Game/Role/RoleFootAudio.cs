using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoleFootAudio : MonoBehaviour
{
    
    protected GameObject roleAnim;

    [SerializeField]
    private List<AudioClip> foots;
    // Start is called before the first frame update
    void Awake()
    {
        roleAnim = GetComponent<RoleController>().Animator.gameObject;
        EventManager.Inst.AddAnimatorEvent(OnFoot);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveAnimatorEvent(OnFoot);
    }

    public virtual void OnFoot(GameObject go, string eventname)
    {
        //Debug.Log("OnFoot");

        //Debug.Log(go);
        //Debug.Log(roleAnim);
        if (roleAnim != go)
        {
            return;
        }
        //AudioManager.Inst.PlaySource(foots[Random.Range(0, foots.Count)], 1);
        if (AnimatorEventName.Foot == eventname)
        {
            if (foots != null && foots.Count > 0)
            {
                //Debug.Log("PlayFoot");
                AudioManager.Inst.PlaySource(foots[Random.Range(0, foots.Count)], 0.4f);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
