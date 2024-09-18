#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
// [InitializeOnLoad]
public class ZY_Tester : MonoBehaviour
{

    bool whosyourdaddy = false;
    [MenuItem("Tools/Reset Transform withoutchild &r")]
    static void ResetTransform()
    {
        GameObject[] selection = Selection.gameObjects;
        if (selection.Length < 1) return;
        Undo.RegisterCompleteObjectUndo(selection, "Zero Position");

        List<Vector3> SavedChildPos = new List<Vector3>();
        List<Vector3> SavedChildScale = new List<Vector3>();
        //foreach (GameObject go in selection)
        //{
        GameObject go = selection[0];
        Vector3 SavedScale = go.transform.lossyScale;

        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform childTrans = go.transform.GetChild(i);
            {
                SavedChildPos.Add(childTrans.position);
                SavedChildScale.Add(childTrans.lossyScale);
            }
        }
        InternalZeroPosition(go);
        InternalZeroRotation(go);
        InternalZeroScale(go);
        Vector3 newScale = go.transform.lossyScale;
        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform childTrans = go.transform.GetChild(0);
            childTrans.parent = null;
            childTrans.localScale = SavedChildScale[i];
            childTrans.SetParent(go.transform);
            //childTrans.localScale = new Vector3(SavedChildScale[i].x * (SavedScale.x / newScale.x), SavedChildScale[i].y * (SavedScale.y / newScale.y), SavedChildScale[i].z * (SavedScale.z / newScale.z));
            childTrans.position = SavedChildPos[i];
        }
        SavedChildPos.Clear();
        SavedChildScale.Clear();
        //}
    }
    [MenuItem("Tools/SetLocalY => 0 &1")]
    static void ResetLocalY()
    {
        GameObject[] selection = Selection.gameObjects;
        foreach (GameObject go in selection)
        {
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, 0, go.transform.localPosition.z);
        }
    }

    private static void InternalZeroPosition(GameObject go)
    {
        go.transform.localPosition = Vector3.zero;
    }
    private static void InternalZeroRotation(GameObject go)
    {
        go.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
    private static void InternalZeroScale(GameObject go)
    {
        go.transform.localScale = Vector3.one;
    }
    public List<GameObject> List1;
    public List<GameObject> List2;
    void CloneList()
    {
        List2 = new List<GameObject>(List1);
        List2.RemoveAt(1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {CloneList();
        }
           
        if (Input.GetKeyDown(KeyCode.I))
        {
            ItemScriptableObject item = Selection.objects[0] as ItemScriptableObject;
            if (item != null)
            {
                BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(item), isOk => { });
                Debug.Log("添加道具：" + item.name);
            }
            else
            {
                GameObject currentObj = Selection.gameObjects[0];
                currentObj = Instantiate(currentObj, BattleManager.Inst.CurrentPlayer.transform.position + BattleManager.Inst.CurrentPlayer.Animator.transform.forward * 2, Quaternion.identity);
                if (currentObj.GetComponent<DmgBuffOnTouch>() != null)
                {
                    currentObj.GetComponent<DmgBuffOnTouch>().Init(BattleManager.Inst.CurrentPlayer);
                }
                if (currentObj.GetComponentInChildren<DmgBuffOnTouch>() != null)
                {
                    currentObj.GetComponentInChildren<DmgBuffOnTouch>().Init(BattleManager.Inst.CurrentPlayer);
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            whosyourdaddy = !whosyourdaddy;
            Debug.LogError(whosyourdaddy ? "自动秒怪模式，启动！" : "关闭秒怪模式");
        }
        if (whosyourdaddy)
        {
            DestroyAllCurrentMonsters();
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {

            Application.targetFrameRate -= 10;
            if (Application.targetFrameRate < 10)
                Application.targetFrameRate = 10;
            Debug.LogError("设置游戏帧率为：" + Application.targetFrameRate);
        }
        if (Input.GetKeyDown(KeyCode.Plus))
        {

            Application.targetFrameRate += 10;
            if (Application.targetFrameRate > 60)
                Application.targetFrameRate = 60;
            Debug.LogError("设置游戏帧率为：" + Application.targetFrameRate);
        }

        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     Debug.LogError("生命难度值："+BattleManager.Inst.DifficulityMapHp);
        //     Debug.LogError("攻击难度值："+BattleManager.Inst.DifficulityAttackPower);
        // }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LocalizationManger.Inst.SetLanguage(SystemLanguage.Chinese);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LocalizationManger.Inst.SetLanguage(SystemLanguage.English);
        } 
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LocalizationManger.Inst.SetLanguage(SystemLanguage.Russian);
        } 
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LocalizationManger.Inst.SetLanguage(SystemLanguage.Indonesian);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            LocalizationManger.Inst.SetLanguage(SystemLanguage.Portuguese);
        }
    }
    // static ZY_Tester()
    // {
    //     Debug.Log("IsRunning");
    //     //EditorApplication.update += This.Update;
    // }
    public ZY_ObjShortCut shortcut;
    public List<ItemScriptableObject> allitems;
    bool ShowItemPanel;
    bool ShowObjShortCutPanel;
    bool IsOpen;
    public float HorizontalLength = 300;
    public float VerticalLength = 100;
    private Vector2 scrollViewVector = Vector2.zero;
    public int ItemVerticalCount;
    


    public bool DrawButton(float HorizontalPos, float VerticalPos, string Name)
    {
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;

        //GUI.skin.label.fontSize = 100;
        return GUI.Button(new Rect(HorizontalPos * HorizontalLength, VerticalPos * VerticalLength, HorizontalLength, VerticalLength), Name, style);
    }
    public void DrawTexture(float HorizontalPos, float VerticalPos, Texture texture)
    {
        if (texture != null)
            GUI.DrawTexture(new Rect(10 + HorizontalPos * HorizontalLength, 10 + VerticalPos * VerticalLength + 10, 80, 80), texture);
    }
    public void DrawString(float HorizontalPos, float VerticalPos, string words)
    {
        if (words != null)
        {
            GUI.skin.textField.fontSize = 30;
            GUI.TextField(new Rect(HorizontalPos * HorizontalLength, VerticalPos * VerticalLength, HorizontalLength * 3, VerticalLength), words);
        }
    }
    private void OnGUI()
    {

        if (GameManager.Inst.EditorMode)
        {
            if (!IsOpen)
            {
                if (DrawButton(0, 0, "打开工具栏"))
                {
                    IsOpen = true;
                }
            }
            else
            {
                if (DrawButton(0, 0, "结束房间"))
                {
                    EndCurrentRoom();
                }
                if (DrawButton(0, 1, "清怪"))
                {
                    DestroyAllCurrentMonsters();
                }
                if (DrawButton(0, 3, "无敌"))
                {
                    Invincible();
                    //Heal();
                }
                if (DrawButton(0, 4, "加局外钱"))
                {
                    //BattleManager.Inst.AddGold(999999);
                    ArchiveManager.Inst.ChangeDiamond(9999);
                }
                if (DrawButton(0, 5, "满级经验"))
                {
                    ArchiveManager.Inst.ChangeExp(9999);
                }if (DrawButton(0, 6, "加魂"))
                {
                    ArchiveManager.Inst.ChangeSoul(9999);
                }
                if (DrawButton(0, 7, "超速"))
                {
                    BattleManager.Inst.CurrentPlayer.roleMove.SetMultiplyMoveSpeed(4f);
                }
                if (DrawButton(0, 8, "无碰撞"))
                {
                    BattleManager.Inst.CurrentPlayer.GetComponent<Collider>().enabled = false;
                }
                //if(DrawButton(0,6,"下一层"))
                //{

                //}
                if (ShowItemPanel == false)
                {
                    if (DrawButton(0, 2, "获取道具"))
                    {
                        if (allitems == null)
                        {
                            allitems = new List<ItemScriptableObject>();
                        }
                        allitems = FindAssetsByType<ItemScriptableObject>();
                        ShowItemPanel = true;
                    }
                }
                if (ShowItemPanel)
                {
                    if (DrawButton(0, 2, "关闭道具栏"))
                    {
                        ShowItemPanel = false;
                    }
                }
                if (ShowItemPanel)
                {
                    scrollViewVector = GUI.BeginScrollView(new Rect(HorizontalLength, 0, 5 * HorizontalLength, ItemVerticalCount * VerticalLength), scrollViewVector, new Rect(0, 0, allitems.Count / ItemVerticalCount, VerticalLength * ItemVerticalCount));
                    for (int i = 0; i < allitems.Count; i++)
                    {

                        int HorizontalIndex = i / ItemVerticalCount;
                        int VerticalIndex = i % ItemVerticalCount;

                        //if (DrawButton(HorizontalIndex, VerticalIndex, allitems[i].Name))
                        if (DrawButton(HorizontalIndex, VerticalIndex, ""))
                        {
                            BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(allitems[i]), isOk => { });
                        }
                        if (allitems[i].Icon != null)
                            DrawTexture(HorizontalIndex, VerticalIndex, allitems[i].Icon.texture);
                    }
                    GUI.EndScrollView();
                }


                if (!ShowObjShortCutPanel)
                {
                    if (DrawButton(0, 8, "释放测试Prefab"))
                    {
                        ShowObjShortCutPanel = true;
                    }
                }
                if (ShowObjShortCutPanel)
                {
                    if (DrawButton(0, 8, "关闭测试栏"))
                    {
                        ShowObjShortCutPanel = true;
                    }

                }


                if (DrawButton(0, 9, "关闭"))
                {
                    IsOpen = false;
                }

            }

        }
    }
    public void Invincible()
    {
        BattleManager.Inst.CurrentPlayer.GetComponent<RoleHealth>().SetGod(true);
    }
    public void Heal()
    {
        BattleManager.Inst.CurrentPlayer.GetComponent<RoleHealth>().SetCurrentHpOfMaxHpPercentage(1f);
    }
    public void DestroyAllCurrentMonsters()
    {
        EnemyController[] EnemyHealths = FindObjectsOfType<EnemyController>();
        foreach (EnemyController health in EnemyHealths)
        {
            DamageInfo dmg = new DamageInfo(health.TemporaryId, 9999, BattleManager.Inst.CurrentPlayer, BattleManager.Inst.CurrentPlayer.transform.position, DmgType.Physical, false, false, 0, 0);
            health.HpInjured(dmg);
        }
    }
    public void EndCurrentRoom()
    {
        FightRoom currentroom = FindObjectOfType<FightRoom>();
        if (currentroom != null)
        {
            //currentroom.JumpOverCurrentRoom();
        }
        DestroyAllCurrentMonsters();
    }

    private void Start()
    {
#if !UNITY_EDITOR
            Destroy(this);
#endif

    }

    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        //Debug.Log("TryFindAssets");
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T).ToString().Replace("UnityEngine.", "")));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }
}
#endif