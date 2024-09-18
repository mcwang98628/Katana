using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//环绕物分裂
public class SurroundObjectSplitEffect : ItemEffect
{
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        int objOldCount = roleController.roleSurroundController.SurroundObjs.Count;
        for (int i = 0; i < objOldCount; i++)
        {
            var sourObj = roleController.roleSurroundController.SurroundObjs[0];
            roleController.roleSurroundController.RemoveObj(sourObj);
            
            var sObj = sourObj.GetComponent<Surround_Obj>().SmallObj;
            Surround_Obj surroundObj = new Surround_Obj();
            surroundObj = GameObject.Instantiate(sObj);
            roleController.roleSurroundController.AddObj(surroundObj);
            
            Surround_Obj surroundObj2 = new Surround_Obj();
            surroundObj2 = GameObject.Instantiate(sObj);
            roleController.roleSurroundController.AddObj(surroundObj2);
        }
    }
}
