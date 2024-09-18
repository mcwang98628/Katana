using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "A_Data/MoveInfo")]
public class MoveInfo : ScriptableObject
{
    [LabelText("位 移")]
    public Vector2 AttackMove;
    [LabelText("移 速")]
    public float MoveSpeed;
    [LabelText("时 间")]
    public float MoveTime;

    public AnimationCurve MoveCurve;
}
