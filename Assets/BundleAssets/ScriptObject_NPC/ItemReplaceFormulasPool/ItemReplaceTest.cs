using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReplaceTest : MonoBehaviour
{
    public ItemReplaceFormulas itemReplaceFormulas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            itemReplaceFormulas.GetReplaceFormula();
        }
    }
}
