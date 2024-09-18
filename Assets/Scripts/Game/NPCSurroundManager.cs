using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSurroundManager : MonoBehaviour
{
    [SerializeField]
    private float SurroundSpeed = 1;
    [SerializeField]
    private float SurroundHeight = 1;
    [SerializeField]
    private float SurroundDistance = 1;
    public List<GameObject> SurroundObjs;

    Vector3 FirstBallDir = Vector3.forward;

    private void Update()
    {
      
        FirstBallDir = Quaternion.AngleAxis(SurroundSpeed * Time.deltaTime, Vector3.up) * FirstBallDir;

        foreach (GameObject obj in SurroundObjs)
        {
            if (obj == null)
            {
                continue;
            }
            obj.transform.position = transform.position + FirstBallDir.normalized * SurroundDistance;
        }

        for (int i = 0; i < SurroundObjs.Count; i++)
        {
            Transform orb = SurroundObjs[i].transform;
            Vector3 Dir = Quaternion.AngleAxis((360 / SurroundObjs.Count) * i, Vector3.up) * FirstBallDir;
            orb.position = transform.position + Dir.normalized * SurroundDistance + Vector3.up * SurroundHeight;
        }

    }
}
