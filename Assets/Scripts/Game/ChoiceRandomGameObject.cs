using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceRandomGameObject : MonoBehaviour
{

    public List<GameObject> objList;
    // Start is called before the first frame update
    void Awake()
    {
        if (objList.Count <= 0)
            return;

        GameObject result = objList[Random.Range(0, objList.Count)];
        result.SetActive(true);
        objList.Remove(result);

        foreach (var obj in objList)
        {
            Destroy(obj);
        }
    }

   
}
