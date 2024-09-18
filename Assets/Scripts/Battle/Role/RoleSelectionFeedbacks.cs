using AnimatorTools;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityString;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class RoleSelectionFeedbacks : MonoBehaviour
{
    protected RoleController roleController;
    [InfoBox("顺序很重要")]
    public List<ParticleSystem> Pars;
    public List<AudioClip> Sfxs;
    public List<FeedBackObject> Feedbacks;
    [SerializeField]
    public List<FeedbackEvent> Events;
    public GameObject animator;

    PhantomReleaser _phantomReleaser;
    
    
    protected virtual void Awake()
    {
        _phantomReleaser = GetComponentInChildren<PhantomReleaser>();
        roleController = GetComponent<RoleController>();
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
        for(int i=0;i<Events.Count;i++)
        {
            EventManager.Inst.AddEvent(Events[i].EventName,HandleEvent);
        }
        EventManager.Inst.AddEvent(EventName.GodStateChange,HandleRollGodStateChange);
        
    }
    public void HandleRollGodStateChange(string e,object obj)
    {
        if(e==roleController.TemporaryId)
        {
            if(roleController.roleHealth.IsGod)
            {

            }
        }
    }
    protected virtual void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.GodStateChange,HandleRollGodStateChange);
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
        for(int i=0;i<Events.Count;i++)
        {
            EventManager.Inst.RemoveEvent(Events[i].EventName,HandleEvent);
        }
    }
    public void HandleEvent(string e,object obj)
    {
        for(int i=0;i<Events.Count;i++)
        {
            if (e == Events[i].EventName)
            {
                //Debug.Log("ReceiveEvent:"+Events[i].EventName);
                if (Events[i].Feedback != null)
                {
                    FeedbackManager.Inst.UseFeedBack(roleController, Events[i].Feedback);
                }
                if (Events[i].Particles_Create != null)
                {
                    CreateParticles(Events[i].Particles_Create.gameObject);
                }
                if (Events[i].Particles_Restart != null)
                {
                    Events[i].Particles_Restart.Play();
                }
                if(Events[i].Particles_Stop!=null)
                {
                    Events[i].Particles_Stop.Stop();
                    Events[i].Particles_Stop.Clear();
                }
                if (Events[i].SFX != null)
                {
                    Events[i].SFX.Play();
                }
            }

        }
    }
    //为了粒子创建之后跟着角色旋转，故在此创建新 的粒子并跟踪。
    public void CreateParticles(GameObject Go)
    {
        GameObject newGo = GameObject.Instantiate(Go, Go.transform.position, Go.transform.rotation);
        newGo.GetComponent<ParticleSystem>().Play();
        ObjFollower _follower = newGo.AddComponent<ObjFollower>();
        _follower.SetFollowingObj(roleController.Animator.gameObject);
        newGo.AddComponent<SelfDestruct>();
    }
    public void AnimEvent(GameObject go,string eventName)
    {
        if(go!= animator)
        {
            return;
        }
        if(eventName.Contains(AnimatorEventName.CreateParticles_))
        {
            int index;
            int.TryParse(eventName.Replace(AnimatorEventName.CreateParticles_, ""), out index);
            if (index < Pars.Count)
            {
                GameObject newGo = GameObject.Instantiate(Pars[index].gameObject, Pars[index].transform.position, Pars[index].transform.rotation);
                newGo.GetComponent<ParticleSystem>().Play();
                ObjFollower _follower = newGo.AddComponent<ObjFollower>();
                _follower.SetFollowingObj(roleController.Animator.gameObject);
                newGo.AddComponent<SelfDestruct>();
            }
        }

        if(eventName.Contains(AnimatorEventName.StartParticles_))
        {
            int index;
            int.TryParse(eventName.Replace(AnimatorEventName.StartParticles_,""),out index);
            if(index<Pars.Count)
            {
                Pars[index].Play();
            }
        }
        if(eventName.Contains(AnimatorEventName.StopParticles_))
        {
            int index;
            int.TryParse(eventName.Replace(AnimatorEventName.StopParticles_, ""), out index);
            if (index < Pars.Count)
            {
                Pars[index].Stop();
                Pars[index].Clear();
            }
        }
        if(eventName.Contains(AnimatorEventName.StartFeedbacks_))
        {
            int index;
            int.TryParse(eventName.Replace(AnimatorEventName.StartFeedbacks_, ""), out index);
            if (index < Pars.Count)
            {
                FeedbackManager.Inst.UseFeedBack(roleController,Feedbacks[index]);
            }
        }




        //这个基本上没有什么用，因为没法控制创建的位置和父物体之类的。
        if(eventName.Contains(AnimatorEventName.CreateParticles_))
        {
            int index;
            int.TryParse(eventName.Replace(AnimatorEventName.CreateParticles_, ""), out index);
            if(index<Pars.Count)
            {
                
                //GameObject CurrentPars = 
                    //Instantiate(Pars[index]);

            }
        }


        if (eventName.Contains(AnimatorEventName.StartSFX_))
        {
            int index;
            int.TryParse(eventName.Replace(AnimatorEventName.StartSFX_, ""), out index);
            if (index < Sfxs.Count)
            {
                Sfxs[index].Play();
            }
        }
        if(eventName.Contains(AnimatorEventName.EmmitPhantom))
        {
            if(_phantomReleaser!=null)
            {
                _phantomReleaser.DuplicateaPhantom();
            }
        }
        if(eventName.Contains(AnimatorEventName.StartPhantom))
        {
            if(_phantomReleaser!=null)
            {
                _phantomReleaser.StartEmmitPhantom();
            }
        }
        if(eventName.Contains(AnimatorEventName.StopPhantom))
        {
            if(_phantomReleaser!=null)
            {
                _phantomReleaser.EndEmmitPhantom();
            }
        }


        //if (eventName.Contains(AnimatorEventName.AnimTimeScaleStart_))
        //{
        //    float speed;
            
        //    float.TryParse(eventName.Replace(AnimatorEventName.AnimTimeScaleStart_, ""), out speed);
        //    roleController.Animator.speed = speed;
        //}
        //if (eventName.Contains(AnimatorEventName.AnimTimeScaleEnd))
        //{
        //    float speed;

        //    float.TryParse(eventName.Replace(AnimatorEventName.AnimTimeScaleEnd, ""), out speed);
        //    roleController.Animator.speed = 1;
        //}

    }
    
    
    
    
}
