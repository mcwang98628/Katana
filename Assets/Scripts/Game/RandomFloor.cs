using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Transform floorChoosen = transform.GetChild(Random.Range(0, transform.childCount));
        floorChoosen.gameObject.SetActive(true);

    }
}
