using UnityEngine;
using System.Collections;


public class SelfDestruct : MonoBehaviour {
    public float selfdestruct_in = 4; // Setting this to 0 means no selfdestruct.
    bool StartDestroy = false;
  
     void Start () {

        particleSystems = GetComponentsInChildren<ParticleSystem>();
        StartCoroutine(DestroyIE());
	}
    public IEnumerator DestroyIE()
    {
        yield return new WaitForSeconds(selfdestruct_in);
        StartDestroy = true;
    }
    private ParticleSystem[] particleSystems;    


    void FixedUpdate()
    {
       if(StartDestroy)
        {
            TryDestroy();
        }
    }

    public void TryDestroy()
    {
        bool allStopped = true;

        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps)
            {
                if (!ps.isStopped)
                {
                    allStopped = false;
                }
            }
        }

        if (allStopped)
            GameObject.Destroy(gameObject);
    }
}
