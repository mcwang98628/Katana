using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

//用于控制文字的偏移
public class TransChildren
{
    public Transform TargetTransform;

    //public float Offset;
    public int Index;

    public TransChildren(Transform _trans)
    {
        TargetTransform = _trans;
        //Offset = Size;
        Index = 0;
    }
}


public class bl_HUDText : MonoBehaviour
{
    struct TextIndex
    {
        public int index;
        public bl_Text text;
    }

    /// <summary>
    /// The Canvas Root of scene.
    /// </summary>
    public Transform CanvasParent;

    /// <summary>
    /// UI Prefab to instatantiate
    /// </summary>
    public GameObject TextPrefab;

    [Space(10)] public float FadeSpeed;
    public float FloatingSpeed;
    public float HideDistance;
    [Range(0, 180)] public float MaxViewAngle;

    public bool DestroyTextOnDeath = true;

    //Privates
    private static List<bl_Text> texts = new List<bl_Text>();

    [LabelText("每个字比上一个高多少世界高度")] public float perTextHeight;
    [LabelText("最大跳字高度")] public int maxHeight = 8;
    public Sprite critSprite = null;
    private Dictionary<Transform, Dictionary<int, bl_Text>> _textss = new Dictionary<Transform, Dictionary<int, bl_Text>>();

    private Camera MCamera = null;

    //存储正在被激活显示的文字，用以控制偏移
    public List<TransChildren> TransChildren = new List<TransChildren>();

    public void AddTextTarget(Transform _trans)
    {
        //已经存在对应的目标了
        bool TargetExists = false;
        for (int i = 0; i < TransChildren.Count; i++)
        {
            if (TransChildren[i].TargetTransform == _trans)
            {
                TargetExists = true;
                TransChildren[i].Index += 1;
                if (TransChildren[i].Index > 10)
                {
                    TransChildren[i].Index = 0;
                }
            }
        }

        if (!TargetExists)
        {
            TransChildren.Add(new TransChildren(_trans));
            StartCoroutine(TextSelfDestroy(_trans));
        }
    }
    //每隔一段时间一个目标身上的字的高度就会清零

    public IEnumerator TextSelfDestroy(Transform _trans)
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < TransChildren.Count; i++)
        {
            if (TransChildren[i].TargetTransform == _trans)
            {
                TransChildren.RemoveAt(i);
            }
        }
    }

    //public void RemoveTextTarget(Transform _trans)
    //{
    //    for (int i = 0; i < TransChildren.Count; i++)
    //    {
    //        if (TransChildren[i].TargetTransform == _trans)
    //        {
    //            TransChildren[i].Index -= 1;
    //            if (TransChildren[i].Index <=0)
    //            {
    //                TransChildren.RemoveAt(i);
    //            }
    //        }
    //    }
    //}

    //获取自己的展示次序。
    public int GetOrder(Transform _trans)
    {
        //int count;
        foreach (TransChildren offsets in TransChildren)
        {
            if (offsets.TargetTransform == _trans)
            {
                return offsets.Index;
            }
        }

        //其实应该算是错误了
        return 0;
    }


    Camera m_Cam
    {
        get
        {
            if (MCamera == null)
            {
                MCamera = (Camera.main != null) ? Camera.main : Camera.current;
            }

            return MCamera;
        }
    }

    /// <summary>
    /// Disable all text when this script gets disabled.
    /// </summary>
    void OnDisable()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            if (texts[i] && texts[i].Rect)
            {
                Destroy(texts[i].Rect.gameObject);
            }

            texts[i] = null;
            texts.Remove(texts[i]);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        if (m_Cam == null)
        {
            return;
        }

        if (Event.current.type == EventType.Repaint)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                //when target is destroyed then remove it from list.
                if (texts[i].m_Transform == null)
                {
                    //When player / Object death, destroy all last text.
                    if (DestroyTextOnDeath)
                    {
                        Destroy(texts[i].Rect.gameObject);
                        texts[i] = null;
                    }

                    texts.Remove(texts[i]);

                    return;
                }
                else if (texts[i] == null)
                {
                    texts.Remove(texts[i]);

                    return;
                }

                bl_Text temporal = texts[i];
                //fade text

                if (Time.time - temporal.m_StartTime > temporal.m_LifeTime)
                {
                    temporal.m_Color -= new Color(0f, 0f, 0f, (Time.deltaTime * FadeSpeed) / 100f);
                }

                //if Text have more than a target graphic
                //add a canvas group in the root for fade all
                if (texts[i].LayoutRoot != null)
                {
                    texts[i].LayoutRoot.alpha = texts[i].m_Color.a;
                }

                //if complete fade remove and destroy text
                if (texts[i].m_Color.a <= 0f)
                {
                    Destroy(texts[i].Rect.gameObject);
                    texts[i] = null;
                    texts.Remove(texts[i]);
                }
                else //if UI visible
                {
                    //Convert Word Position in screen position for UI
                    int mov = ScreenPosition(texts[i].m_Transform);

                    bl_Text m_Text = texts[i];
                    m_Text.Yquickness += Time.deltaTime;
                    //Get center up of target,并且带有偏移
                    Vector3 position = m_Text.m_Transform.GetComponent<Collider>().bounds.center + new Vector3(0, m_Text.Offset, 0);
                    // + (((Vector3.up * texts[i].m_Transform.GetComponent<Collider>().bounds.size.y) * 0.1f));


                    Vector3 front = position - MCamera.transform.position;
                    switch (m_Text.movement)
                    {
                        case bl_Guidance.Up:
                            m_Text.Ycountervail += (((Time.deltaTime * FloatingSpeed) * mov));
                            m_Text.Xcountervail = 0;
                            break;
                        case bl_Guidance.Down:
                            m_Text.Ycountervail -= (((Time.deltaTime * FloatingSpeed) * mov));
                            m_Text.Xcountervail = 0;
                            break;
                        case bl_Guidance.Left:
                            m_Text.Ycountervail = 0;
                            m_Text.Xcountervail -= ((Time.deltaTime * FloatingSpeed) * mov);
                            break;
                        case bl_Guidance.Right:
                            m_Text.Ycountervail = 0;
                            m_Text.Ycountervail += ((Time.deltaTime * FloatingSpeed) * mov);
                            break;
                        case bl_Guidance.RightUp:
                            m_Text.Ycountervail += (((Time.deltaTime * FloatingSpeed) * mov));
                            m_Text.Xcountervail += ((Time.deltaTime * FloatingSpeed) * mov);
                            break;
                        case bl_Guidance.RightDown:
                            m_Text.Ycountervail -= (((Time.deltaTime * FloatingSpeed) * mov));
                            m_Text.Xcountervail += ((Time.deltaTime * FloatingSpeed) * mov);
                            break;
                        case bl_Guidance.LeftUp:
                            m_Text.Ycountervail += (((Time.deltaTime * FloatingSpeed) * mov));
                            m_Text.Xcountervail -= ((Time.deltaTime * FloatingSpeed) * mov);
                            break;
                        case bl_Guidance.LeftDown:
                            m_Text.Ycountervail -= (((Time.deltaTime * FloatingSpeed) * mov));
                            m_Text.Xcountervail -= ((Time.deltaTime * FloatingSpeed) * mov);
                            break;
                    }

                    position += new Vector3(m_Text.Xcountervail, m_Text.Ycountervail, 0).normalized * 0.15f;

                    //its in camera view
                    if ((front.magnitude <= HideDistance) && (Vector3.Angle(MCamera.transform.forward, position - MCamera.transform.position) <= MaxViewAngle))
                    {
                        //Convert position to view port
                        Vector2 v = MCamera.WorldToViewportPoint(position);
                        //configure each text
                        m_Text.m_Text.fontSize = m_Text.m_Size; //((int)(((mov / 2) * 1)) + texts[i].m_Size) / 2;
                        m_Text.m_Text.text = m_Text.m_text;
                        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(m_Text.m_text));

                        //Calculate the movement 
                        Vector2 v2 = new Vector2((v.x - size.x * 0.5f) + m_Text.Xcountervail, -((v.y - size.y) - m_Text.Ycountervail));
                        v2 = v2.normalized * m_Text.m_Speed;
                        //Apply to Text
                        m_Text.Rect.anchorMax = v;
                        m_Text.Rect.anchorMin = v;

                        m_Text.Rect.anchoredPosition = v2;
                        m_Text.m_Text.color = m_Text.m_Color;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Simple way
    /// </summary>
    /// <param name="text"></param>
    /// <param name="trans"></param>
    public void NewText(string text, Transform trans)
    {
        NewText(text, trans, bl_Guidance.Up);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="trans"></param>
    public void NewText(string text, Transform trans, Color color)
    {
        NewText(text, trans, color, 8, 1, 20f, 1, 2.2f, bl_Guidance.Up);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="trans"></param>
    /// <param name="place"></param>
    public void NewText(string text, Transform trans, bl_Guidance place)
    {
        NewText(text, trans, Color.white, 8, 1, 20f, 0, 0, place);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="trans"></param>
    /// <param name="place"></param>
    public void NewText(string text, Transform trans, Color color, int size, float lifetime, bl_Guidance place, bool crit = false)
    {
        NewText(text, trans, color, size, lifetime, 36f, 2f, 3, place, crit);
    }

    /// <summary>
    /// send a new event, to create a new floating text
    /// </summary>
    public void NewText(string text, Transform trans, Color color, int size, float lifeTime, float speed, float yAcceleration, float yAccelerationScaleFactor, bl_Guidance movement,
        bool crit = false)
    {
        GameObject t = Instantiate(TextPrefab) as GameObject;
        //Create new text info to instatiate 
        bl_Text item = t.GetComponent<bl_Text>();

        item.m_Speed = speed;
        item.m_Color = color;
        item.m_Transform = trans;
        item.m_text = text;
        item.m_Size = size;
        item.m_LifeTime = lifeTime;
        item.m_StartTime = Time.time;
        item.frontImage.enabled = crit;
        if (crit)
        {
            item.frontImage.sprite = critSprite;
        }

        if (movement == bl_Guidance.Random)
        {
            item.movement = (bl_Guidance) Random.Range(0, 8);
            isDamageText = false;
        }
        else
        {
            item.movement = movement;
            isDamageText = true;
        }

        textIndex = 0;
        while (isDamageText)
        {
            if (_textss.ContainsKey(item.m_Transform))
            {
                if (_textss[item.m_Transform].ContainsKey(textIndex))
                {
                    if (_textss[item.m_Transform][textIndex] == null)
                    {
                        _textss[item.m_Transform][textIndex] = item;
                        break;
                    }
                    else
                    {
                        textIndex++;
                        if (textIndex >= maxHeight)
                        {
                            textIndex = privousDic[item.m_Transform];
                            _textss[item.m_Transform][textIndex] = item;
                            privousDic[item.m_Transform]++;
                            privousDic[item.m_Transform] %= maxHeight;
                            break;
                        }
                    }
                }
                else
                {
                    _textss[item.m_Transform].Add(textIndex, item);
                    break;
                }
            }
            else
            {
                _textss.Add(item.m_Transform, new Dictionary<int, bl_Text>()
                {
                    {textIndex, item}
                });
                privousDic.Add(item.m_Transform, 0);
                break;
            }
        }

        item.Offset = 1.5f + perTextHeight * textIndex;


        item.Yquickness = yAcceleration;
        item.YquicknessScaleFactor = yAccelerationScaleFactor;

        t.transform.SetParent(CanvasParent, false);
        t.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        texts.Add(item);
    }

    private int textIndex = 0;
    private int previousIndex;
    private Dictionary<Transform, int> privousDic=new Dictionary<Transform, int>();
    private bool isDamageText;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private int ScreenPosition(Transform t)
    {
        int p = (int) (m_Cam.WorldToScreenPoint(t.GetComponent<Collider>().bounds.center + (((Vector3.up * t.GetComponent<Collider>().bounds.size.y) * 0.5f))).y
                       - this.m_Cam.WorldToScreenPoint(t.GetComponent<Collider>().bounds.center - (((Vector3.up * t.GetComponent<Collider>().bounds.size.y) * 0.5f))).y);
        return p;
    }

    public void ClearJumpDic(Transform deadRoleTransform)
    {
        privousDic.Remove(deadRoleTransform);
        _textss.Remove(deadRoleTransform);
    }
}