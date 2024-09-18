using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UIText/TextSetting")]
public class TextSetting : ScriptableObject
{
    [Serializable]
    public class LanguageSetting
    {
        public SystemLanguage Language;
        public Font TextFont;
    }
    
    public Font CommonTextFont;
    public List<LanguageSetting> Languages = new List<LanguageSetting>();
} 
