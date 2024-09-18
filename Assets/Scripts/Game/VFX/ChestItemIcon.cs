using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChestItemIcon : MonoBehaviour
{
    public Image Icon;
    public Item _item;
    Vector3 Target;
    public void SetIcon(Sprite sprite)
    {
        Icon.sprite = sprite;
    }
    public void SetTarget(Vector3 target)
    {
        Target = target;
    }
    private void LateUpdate()
    {
        if (Camera.main != null)
        {
           transform.position= Camera.main.WorldToScreenPoint(Target);
        }
    }
}
