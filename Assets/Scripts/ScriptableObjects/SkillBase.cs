using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Active,
    Passive
}

public class SkillBase : ScriptableObject
{
    public string iconPath;
    public string skillesc;
    public SkillType skillType;
}

[CreateAssetMenu(menuName = "A_Data/ActiveSkill")]
public class ActiveSkill : SkillBase
{
    
}



[CreateAssetMenu(menuName = "A_Data/PassiveSkill")]
public class PassiveSkill : SkillBase
{
    //public List<PassiveEffect> effectList = new List<PassiveEffect>();
}


