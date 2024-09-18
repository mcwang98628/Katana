using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_IconJump : MonoBehaviour
{
    public GameObject Icon;
    public void CreateIcon(Vector3 WorldPos,Sprite sprite)
    {
        GameObject CurrentIcon;
        if (Camera.main != null)
        {
            CurrentIcon = Instantiate(Icon, Camera.main.WorldToScreenPoint(WorldPos), Quaternion.identity,gameObject.transform);
            if (CurrentIcon != null)
            {
                CurrentIcon.SetActive(true);
                CurrentIcon.GetComponent<ChestItemIcon>().SetIcon(sprite);
                CurrentIcon.GetComponent<ChestItemIcon>().SetTarget(WorldPos);
            }
        }
    }
}
