using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ColoredFoldoutGroupAttribute : PropertyGroupAttribute
{
    public float R, G, B, A;

    public ColoredFoldoutGroupAttribute(string path)
        : base(path)
    {
    }

    public ColoredFoldoutGroupAttribute(string path, float r, float g, float b, float a = 1f)
        : base(path)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }
    protected override void CombineValuesWith(PropertyGroupAttribute other)
    {
        var otherAttr = (ColoredFoldoutGroupAttribute)other;

        this.R = Mathf.Max(otherAttr.R, this.R);
        this.G = Mathf.Max(otherAttr.G, this.G);
        this.B = Mathf.Max(otherAttr.B, this.B);
        this.A = Mathf.Max(otherAttr.A, this.A);
    }
}
