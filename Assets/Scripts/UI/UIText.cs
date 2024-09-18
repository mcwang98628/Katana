using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class UIText : MonoBehaviour
{
    public bool Translate = true;
    public bool IsUseUppercase = false;

    public TextSetting TextSetting;
    
    private Text t;
    public Text Te
    {
        get
        {
            if (t==null)
            {
                t = this.transform.GetComponent<Text>();
                if (t==null)
                {
                    t = this.gameObject.AddComponent<Text>();
                }
            }
            return t;
        }
        set { t = value; }
    }

    [SerializeField]
    [TextArea]
    [LabelText("TextKey")]
    private string str;

    public string text
    {
        set
        {
            str = value;
            LanguageEvent();
        }
        get { return str; }
    }


    void Awake()
    {
        LanguageEvent();
        EventManager.Inst.AddEvent(EventName.OnUpdateLanguage,OnUpdateLanguage);
    }

    void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnUpdateLanguage,OnUpdateLanguage);
    }

    private void OnUpdateLanguage(string eventName, object value)
    {
        LanguageEvent();
    }

#if UNITY_EDITOR
    protected void OnValidate()
    {
        LanguageEvent();
    }
#endif


    void LanguageEvent()
    {
        if (Translate)
        {
            Te.text = LocalizationManger.Inst.GetText(str);
            // Te.text = Te.text.Replace("\\n", "\n");
        }
        else
        {
            Te.text = str;
        }

        if (IsUseUppercase)
        {
            Te.text = Te.text.ToUpper();
        }

        if (TextSetting!=null)
        {
            bool isSetting = false;
            for (int i = 0; i < TextSetting.Languages.Count; i++)
            {
                if (TextSetting.Languages[i].TextFont != null  && 
                    TextSetting.Languages[i].Language == LocalizationManger.Inst.CurrentLanguageType)
                {
                    Te.font = TextSetting.Languages[i].TextFont;
                    isSetting = true;
                    break;
                }
            }

            if (!isSetting)
            {
                Te.font = TextSetting.CommonTextFont;
            }
        }
    }

}

