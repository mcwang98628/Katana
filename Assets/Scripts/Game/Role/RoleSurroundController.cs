using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

public class RoleSurroundController : MonoBehaviour
{
    [ReadOnly][ShowInInlineEditors]
    private float SurroundSpeed=180;
    [ReadOnly][ShowInInlineEditors]
    private float SurroundDistance=1.5f;
    //[ShowInInspector]
    public List<Surround_Obj> SurroundObjs = new List<Surround_Obj>();


    bool isInSort = false;
    Vector3 FirstBallDir = Vector3.forward;

    bool AddInMiddle = false;
    
    public void AddObj(Surround_Obj go)
    {
        if (!AddInMiddle)
        {
            SurroundObjs.Add(go);
        }
        else
        {
            SurroundObjs.Insert( SurroundObjs.Count / 2,go);
        }
        AddInMiddle = !AddInMiddle;
    }

    public void RemoveObj(Surround_Obj go)
    {
        if (SurroundObjs.Contains(go))
        {
            GameObject.Destroy(go.gameObject);
            SurroundObjs.Remove(go);
        }
    }

    public void AddSurroundSpeed(float surroundSpeed)
    {
        SurroundSpeed += surroundSpeed;
    }


    private Vector3 dir;
    private void Update()
    {
        if (isInSort)
            return;

        FirstBallDir = Quaternion.AngleAxis(SurroundSpeed * Time.deltaTime, Vector3.up) * FirstBallDir;

        foreach (Surround_Obj obj in SurroundObjs)
        {
            //if (obj.offset != Vector3.zero)
            //{
            //    obj.go.transform.position = transform.position + obj.offset;
            //}

            //dir = (obj.go.transform.position - transform.position);
            //var v3 = transform.position + dir.normalized * SurroundDistance;
            //obj.go.transform.position = new Vector3(v3.x, transform.position.y + 0.5f, v3.z);

            //obj.go.transform.RotateAround(transform.position, Vector3.up, SurroundSpeed * Time.deltaTime);
            //obj.offset = obj.go.transform.position - transform.position;

            obj.transform.position = transform.position + FirstBallDir.normalized * SurroundDistance;
        }

        for (int i = 0; i < SurroundObjs.Count; i++)
        {
            Transform orb = SurroundObjs[i].transform;
            Vector3 Dir= Quaternion.AngleAxis((360 / SurroundObjs.Count)*i, Vector3.up) * FirstBallDir;
            orb.position = transform.position + Dir.normalized * SurroundDistance+Vector3.up*1.5f;
            orb.forward = Dir.normalized;
        }

    }


}


