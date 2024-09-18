using System;
using System.Collections.Generic;
using UnityEngine;

public class GuidTools : MonoBehaviour
{
    public static string GetGUID()
    {
        return Guid.NewGuid().ToString();
    }
}
