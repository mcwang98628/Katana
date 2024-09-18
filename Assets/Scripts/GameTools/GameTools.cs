using System;
using System.Reflection;
using UnityEngine;


public class GameTools : MonoBehaviour
{
    public static MethodInfo CallFunc(object obj,string funcName,params object[] args)
    {
        Type[] argTypes = new Type[args.Length];
        for (int i = 0; i < argTypes.Length; i++)
        {
            if (args[i] == null)
            {
                Debug.LogError("参数NULL");
                return null;
            }
            argTypes[i] = args[i].GetType();
        }

        MethodInfo method = obj.GetType().GetMethod(funcName, argTypes);
        if (null != method)
        {
            method.Invoke(obj, args);
            return method;
        }

        return null;
    }
}
