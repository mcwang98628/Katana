using System;
using UnityEngine;

public interface IBattleObject
{
    BattleObjectType BattleObjectType { get; }
    Guid TemporaryId { get; }
    Transform ObjectTransform { get; }
}
