using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXLookPlayer : MonoBehaviour
{
    private void Start()
    {
        gameObject.transform.LookAtNoY(BattleManager.Inst.CurrentPlayer.transform);
    }
}
