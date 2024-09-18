using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFollower : MonoBehaviour
{
    GameObject FollowingObj;
    Vector3 Dis;
    public void SetFollowingObj(GameObject Obj)
    {
        FollowingObj = Obj;
        Dis = FollowingObj.transform.position - transform.position;
    }
    private void Update()
    {
        if(FollowingObj!=null)
        {
            transform.position = FollowingObj.transform.position - Dis;
        }
    }
}
