using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorToolkit
{
    public static Vector3 SetY(this Vector3 SourceVector,float y)
    {
        return new Vector3(SourceVector.x, y, SourceVector.z);
    }
    public static Vector3 SetX(this Vector3 SourceVector, float x)
    {
        return new Vector3(x, SourceVector.y, SourceVector.z);
    }
    public static Vector3 SetZ(this Vector3 SourceVector, float z)
    {
        return new Vector3(SourceVector.x,SourceVector.y,z);
    }
    public static void SetX(this Transform _trans,float x)
    {
        _trans.position = _trans.position.SetX(x);
    }
    public static void SetY(this Transform _trans, float y)
    {
        _trans.position = _trans.position.SetX(y);
    }
    public static void SetZ(this Transform _trans, float z)
    {
        _trans.position = _trans.position.SetX(z);
    }
    public static void LerpLookAt(this Transform _trans,Transform Target,float LerpMul)
    {
        if (Target != null)
        {
            Vector3 direction = Target.transform.position - _trans.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {

                Quaternion toRotation = Quaternion.LookRotation(direction);
                _trans.rotation = Quaternion.Lerp(_trans.rotation, toRotation, LerpMul);
            }
        }

    }
    //LookAt，但是不管Y方向
    public static void LookAtNoY(this Transform _trans, Transform Target)
    {
        if (Target != null)
        {
            _trans.LookAt(Target.transform.position.SetY(_trans.position.y));
        }
    }

    public static void LerpTo(this ref float Base,float Target,float Mul)
    {
        //Debug.Log(Mathf.Lerp(Base, Target, Mul));
        //return Mathf.Lerp(Base,Target,Mul);
        Base = Mathf.Lerp(Base, Target, Mul);
    }
    public static int CompareDistance(Vector3 Point1,Vector3 Point2,float Distance,bool NoY=true)
    {
        float DisPow=0;
        DisPow = Mathf.Pow(Point1.x - Point2.x, 2) + Mathf.Pow(Point1.y - Point2.y, 2) + Mathf.Pow(Point1.z - Point2.z, 2);
        //if (!NoY)
        //{
        //    DisPow= Mathf.Pow(Point1.x - Point2.x, 2) + Mathf.Pow(Point1.y - Point2.y, 2) + Mathf.Pow(Point1.z - Point2.z, 2);
        //}
        //else
        //{
        //     DisPow= Mathf.Pow(Point1.x - Point2.x, 2) + Mathf.Pow(Point1.y - Point2.y, 2) + Mathf.Pow(Point1.z - Point2.z, 2);
        //}
        float TargetDisPow = Mathf.Pow(Distance, 2);
        if (DisPow> TargetDisPow)
        {
            return 1;
        }
        if(DisPow== TargetDisPow)
        {
            return 0;
        }
        if(DisPow< TargetDisPow)
        {
            return -1;
        }
        return -999;

    }
}
