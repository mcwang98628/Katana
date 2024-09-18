using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLookAtPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (BattleManager.Inst.CurrentPlayer != null)
        {
            gameObject.transform.LookAt(BattleManager.Inst.CurrentPlayer.transform);

            Vector3 euler = gameObject.transform.rotation.eulerAngles;
            gameObject.transform.rotation = Quaternion.Euler( new Vector3(0,euler.y,0));
        }
        
    }

}
