using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    private RoleController _roleController;
    private RoleNode _roleNode;
    private int _roleColorLevel;//1,2,3
    
    [System.Serializable]
    public struct Appearance
    {
        public List<GameObject> AddObjs;
        public Material RoleMat;
    }

    public List<Appearance> AppearanceList;

    public virtual void Init(int colorLevel = 1)
    {
        _roleController = this.gameObject.GetComponent<RoleController>();
        _roleNode = this.gameObject.GetComponent<RoleNode>();

        if (_roleController != null)
            //_roleColorLevel = ArchiveManager.Inst.GetHeroUpgradeData(_roleController.UniqueID).ColorLevel;
            _roleColorLevel = 3;
        else
            _roleColorLevel = colorLevel;
        SetAppearance();

    }


    protected virtual void SetAppearance()
    {

        for (int i = 0; i < AppearanceList.Count; i++)
        {
            for (int j = 0; j < AppearanceList[i].AddObjs.Count; j++)
            {
                AppearanceList[i].AddObjs[j].SetActive(false);
            }
        }
        for (int i = 0; i < AppearanceList.Count; i++)
        {
            if (_roleColorLevel == i + 1)
            {
                for (int j = 0; j < AppearanceList[i].AddObjs.Count; j++)
                {
                    AppearanceList[i].AddObjs[j].SetActive(true);
                }
            }
        }

        if (_roleNode != null)
            _roleNode.SetRoleMaterial(AppearanceList[_roleColorLevel - 1].RoleMat);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _roleColorLevel = 1;
            SetAppearance();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _roleColorLevel = 2;
            SetAppearance();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _roleColorLevel = 3;
            SetAppearance();
        }
    }
}