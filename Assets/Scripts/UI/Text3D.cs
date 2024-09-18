using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class Text3D : MonoBehaviour
{
    public bool Translate = true;
    public bool IsUseUppercase = false;
    private TextMesh t;
    public TextMesh Te
    {
        get
        {
            if (t==null)
            {
                t = this.transform.GetComponent<TextMesh>();
                if (t==null)
                {
                    t = this.gameObject.AddComponent<TextMesh>();
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
        // LanguageEvent();
    }
#endif


    void LanguageEvent()
    {
        if (Translate)
        {
            Te.text = LocalizationManger.Inst.GetText(str);
            Te.text = Te.text.Replace("\\n", "\n");
        }
        else
        {
            Te.text = str;
        }

        if (IsUseUppercase)
        {
            Te.text = Te.text.ToUpper();
        }
    }
}
