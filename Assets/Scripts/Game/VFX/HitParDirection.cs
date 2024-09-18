using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParDirection : MonoBehaviour
{
    ParticleSystem _particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule _main = _particleSystem.main;
        //_particleSystem.main = ParticleSystem.MainModule;
        Vector3 Dir = (BattleManager.Inst.CurrentPlayer.transform.position - transform.position).normalized;
        Dir.y = 0;
        float Angle = Mathf.Atan2(Dir.x,Dir.z);
        //Debug.Log(Angle*Mathf.Rad2Deg);
        if(Mathf.Abs(Angle)>90)
        {
            _main.startRotation= Random.Range(30, 60);
            //_particleSystem.main= Random.Range(30, 60);
        }
        else
        {
            _main.startRotation = Random.Range(-30, -60);
        }

    }
    
}
