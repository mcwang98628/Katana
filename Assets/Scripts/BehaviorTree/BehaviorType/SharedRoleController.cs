using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class SharedRoleController : SharedVariable<RoleController>
{
    public static implicit operator SharedRoleController(RoleController value)
    {
        return new SharedRoleController
        {
            mValue = value
        };
    }
}
