using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleToolkit
{
    //复制一个粒子系统
    public static GameObject DuplicatePaticles(GameObject Obj)
    {
        return Object.Instantiate(Obj,Obj.transform.position,Obj.transform.rotation);
    }
    //关闭粒子系统
    public static void DisableEmmisions(GameObject Obj)
    {
        ParticleSystem selfpar = Obj.GetComponent<ParticleSystem>();
        if (selfpar != null)
        {
            ParticleSystem.EmissionModule emission = selfpar.emission;
            emission.enabled = false;
            //par.enableEmission = true;
        }
        foreach (Transform child in Obj.transform)
        {
            ParticleSystem par = child.GetComponent<ParticleSystem>();
            if(par!=null)
            {
                ParticleSystem.EmissionModule emission = par.emission;
                emission.enabled = false;
                //par.enableEmission = true;
            }
        }
    }
    public static void InsParticles(GameObject Particles,Vector3 Position)
    {
        Object.Instantiate(Particles, Position, Quaternion.identity);
    }
    public static GameObject Duplicate(this GameObject Particles)
    {
        return Object.Instantiate(Particles, Particles.transform.position, Quaternion.identity);
    }
    public static GameObject DuplicateAtPlayer(this GameObject Particles,float Offset=0)
    {
        return Object.Instantiate(Particles,BattleManager.Inst.CurrentPlayer.transform.position.SetY(Offset),Quaternion.identity);
    }
    public static GameObject DuplicateUnderTransform(this GameObject Particles,Transform _trans)
    {
        GameObject InsdObj = Object.Instantiate(Particles, _trans.position, Quaternion.identity);
        InsdObj.transform.SetParent(_trans);
        return InsdObj;
    }
    public static GameObject DuplicateAtPosition(this GameObject Particles, Vector3 _pos)
    {
        return Object.Instantiate(Particles,_pos, Quaternion.identity);
    }
    //复制一份角色身上的粒子（自动指定父物体，自动激活）
    public static GameObject DuplicateRoleParticles(this GameObject Particles,bool DoSelfDestruct=true)
    {
        GameObject InsdObj = Object.Instantiate(Particles, Particles.transform.position, Quaternion.identity);
        InsdObj.SetActive(true);
        if (DoSelfDestruct)
        {
            InsdObj.AddComponent<SelfDestruct>();
        }
        if(Particles.transform.parent!=null)
        InsdObj.transform.SetParent(Particles.transform.parent);
        return InsdObj;
    }
    public static GameObject DuplicateAtGround(this GameObject Particles,Transform _trans,float Offset=0.1f)
    {
        return Object.Instantiate(Particles, _trans.position.SetY(Offset), Quaternion.identity);
    }

    }
