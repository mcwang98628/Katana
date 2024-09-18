using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.SceneManagement;


namespace LevelEditor
{
    public class LevelEditorSceneHandler:MonoBehaviour
    {

        public LevelEditorSceneHandler()
        {
        }

        public static Vector2 gridSize = new Vector2(0.875f,0.875f);

        private bool isInited = false;
        private bool isDestroy = false;

        private bool defaultSceneViewOrthographic = true;
        private Quaternion defaultSceneViewRotation = Quaternion.identity;

        private Vector3 mouseWorldPos;
        private Vector3 mouseGridsPos;

        private BattleLevelEditorCreater levelEditorCreater;

        private LevelContentType curSelectContentType;
        private string curSelectContentPathName;
        private GameObject previewGameObj;
        //private List<GameObject> previewGameObjList;
        bool CollisionTesterFollowMouse;
        bool CollisionTesterOn;
        bool SpawnPointsOn;
        Vector3 CollisionTestStartPos;
        public void Init(BattleLevelEditorCreater battleLevelEditorCreater)
        {
            levelEditorCreater = battleLevelEditorCreater;
            SceneView.duringSceneGui += DuringSceneGui;
        }
        public void Destroy()
        {
            ClearSelectContent();

            isDestroy = true;
            SceneView.RepaintAll();
        }
        public void DeleteAllInScene()
        {
            Transform Trans1 = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.transform.Find("RoomContent/CollisionObj");
            Transform Trans2 = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.transform.Find("RoomContent/NoCollisionObj");
            Transform Trans3 = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.transform.Find("RoomContent/BreakableObj");
            Transform Trans4 = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.transform.Find("RoomContent/OtherModuleObj");
            DeleteChildren(Trans1);
            DeleteChildren(Trans2);
            DeleteChildren(Trans3);
            DeleteChildren(Trans4);
        }
        public static void DeleteChildren(Transform trans)
        {
            //Undo.RecordObject(trans.gameObject,"Delete");
            if(trans!=null)
            {
                for(int i=0;i<trans.childCount;i++)
                {
                    GameObject.DestroyImmediate(trans.GetChild(i).gameObject);
                }
            }
        }
        private void DuringSceneGui(SceneView sceneView)
        {
            if(!isInited)
            {
                isInited = true;
                defaultSceneViewOrthographic = sceneView.orthographic;
                defaultSceneViewRotation = sceneView.rotation;

                //调整场景视角。但是好像没什么用
                //sceneView.orthographic = true;
                //sceneView.rotation = Quaternion.Euler(90,0,0);
                //sceneView.pivot = Vector3.zero;
                //sceneView.isRotationLocked = true;
            }
            if(isDestroy)
            {
                SceneView.duringSceneGui -= DuringSceneGui;
                sceneView.orthographic = defaultSceneViewOrthographic;
                sceneView.rotation = defaultSceneViewRotation;
                sceneView.isRotationLocked = false;

                return;
            }
            UpdateSceneViewGUI(sceneView);
            UpdateSceneViewMousePos(sceneView);
            UpdateSceneViewInputKey(sceneView);
            if(SpawnPointsOn)
            UpdateSpawnPoints();
            if(CollisionTesterOn)
            TestCollider(sceneView);
            //有预览物体的时候禁止操作
            if (previewGameObj != null)
            {
                //Selection.activeObject = null;
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }
            UpdatePreviewGameObjs();
        }
        //public void OnDrawGizmos()
        //{
        //    Debug.LogError("IsDrawingGizmos");
        //    Vector3 pos = Camera.current.transform.position;
            
        //    for (float y = pos.y - 800.0f; y < pos.y + 800.0f; y += gridSize.y)
        //    {
        //        Gizmos.DrawLine(new Vector3(-1000000.0f, Mathf.Floor(y / gridSize.y) * gridSize.y, 0.0f),
        //                        new Vector3(1000000.0f, Mathf.Floor(y / gridSize.y) * gridSize.y, 0.0f));
        //    }

        //    for (float x = pos.x - 1200.0f; x < pos.x + 1200.0f; x += gridSize.x)
        //    {
        //        Gizmos.DrawLine(new Vector3(Mathf.Floor(x / gridSize.x) * gridSize.x, -1000000.0f, 0.0f),
        //                        new Vector3(Mathf.Floor(x / gridSize.x) * gridSize.x, 1000000.0f, 0.0f));
        //    }
        //}


        //--------------------------------------------

        public void SetGridSize(Vector2 size)
        {
            if(size.x > 0 && size.y > 0)
                gridSize = size;
        }

        public void SetSelectContent(LevelContentType selectContentType,string selectContentPathName)
        {
            curSelectContentType = selectContentType;
            curSelectContentPathName = selectContentPathName;
            if(previewGameObj != null)
                GameObject.DestroyImmediate(previewGameObj);
            previewGameObj = levelEditorCreater.CreatePreviewGameObj(curSelectContentPathName);
        }
        private void ClearSelectContent()
        {
            curSelectContentPathName = null;
            if(previewGameObj != null)
            {
                GameObject.DestroyImmediate(previewGameObj);
                previewGameObj = null;
            }
        }
        public void UpdateSpawnPoints()
        {
            Transform SpawnPointTrans = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.transform.Find("EnemySpawnPoints");
            if(SpawnPointTrans!=null)
            for(int i=0;i<SpawnPointTrans.childCount; i++)
            {
                    //Y设置为零
                SpawnPointTrans.GetChild(i).transform.position = new Vector3(SpawnPointTrans.GetChild(i).transform.position.x, 0, SpawnPointTrans.GetChild(i).transform.position.z);
                RaycastHit hit;
                    float CastHeight = 4f;
                PhysicsSceneExtensions.GetPhysicsScene(PrefabStageUtility.GetCurrentPrefabStage().scene).Raycast(SpawnPointTrans.GetChild(i).transform.position + CastHeight * Vector3 .up , Vector3.down , out hit, 10.1f);
                    //正常
                    Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
                    if (hit.transform ==null)
                {
                    Handles.color = Color.green;
                    Handles.DrawLine(SpawnPointTrans.GetChild(i).transform.position + CastHeight * Vector3.up, SpawnPointTrans.GetChild(i).transform.position);
                }
                //刷怪点有物体
                else if(hit.point.y>0.01f)
                {
                    Handles.color=Color.red;
                    Handles.DrawLine(SpawnPointTrans.GetChild(i).transform.position + CastHeight * Vector3.up, hit.point);
                }
                    Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
                    //SpawnPointTrans.GetChild(i).transform;
                    EditorGUI.BeginChangeCheck();
                    Vector3 newTargetPosition = Handles.FreeMoveHandle (SpawnPointTrans.GetChild(i).transform.position, Quaternion.identity ,0.25f, Vector3.one, Handles.SphereHandleCap);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(SpawnPointTrans.GetChild(i).transform, "Change Look At Target Position");
                        SpawnPointTrans.GetChild(i).transform.position = newTargetPosition;
                    }

                }

        }


        //--------------------------------------------

        //这里需要绘制网格。
        private void UpdateSceneViewGUI(SceneView sceneView)
        {
            Handles.color = new Color(1f, 1f, 1f, 0.1f);
            //for (float i = -20; i < 20; i += gridSize.x)
            //{
            //    for (float j = -20; j < 20; j += gridSize.y)
            //    {
            //        Handles.DrawSphere(0, new Vector3(i, 0, j), Quaternion.identity, 0.1f);
            //    }
            //}

            DrawGrid();
            //Handles.BeginGUI();
            //if(GUI.Button(new Rect(0,0,70,20),"重置视角"))
            //{
            //    sceneView.orthographic = true;
            //    sceneView.rotation = Quaternion.Euler(90,0,0);
            //    sceneView.pivot = Vector3.zero;
            //    sceneView.isRotationLocked = true;
            //}
            //if(GUI.Button(new Rect(0,25,70,20),"解除锁定"))
            //{
            //    if(sceneView.orthographic != defaultSceneViewOrthographic)
            //    {
            //        sceneView.orthographic = defaultSceneViewOrthographic;
            //        sceneView.rotation = defaultSceneViewRotation;
            //    }
            //    sceneView.isRotationLocked = false;
            //}
            //Handles.EndGUI();
        }
        public void DrawGrid()
        {
            if (gridSize.x > 0 && gridSize.y > 0)
            {
                for (float y = -20f; y < 20.0f; y += gridSize.y)
                {
                    Handles.DrawLine(new Vector3(-1000000.0f, 0.0f, Mathf.Floor(y / gridSize.y) * gridSize.y),
                                    new Vector3(1000000.0f, 0.0f, Mathf.Floor(y / gridSize.y) * gridSize.y));
                }

                for (float x = -20.0f; x < 20.0f; x += gridSize.x)
                {
                    Handles.DrawLine(new Vector3(Mathf.Floor(x / gridSize.x) * gridSize.x, 0, -1000000f),
                                    new Vector3(Mathf.Floor(x / gridSize.x) * gridSize.x, 0, 1000000f));
                }
            }
            Handles.color = new Color(1, 0.92f, 0.086f, 0.5f);
            Vector2 StartPoint = new Vector2( -3.5f,-12.25f);

            for (float z = StartPoint.y; z <= StartPoint.y+5* 3.5f; z += 3.5f)
            {
                Handles.DrawLine(new Vector3(StartPoint.x, 0.0f,z),
                                new Vector3(StartPoint.x + 4 * 3.5f, 0.0f, z));
            }
            for (float x = StartPoint.x; x <= StartPoint.x+4*3.5f; x += 3.5f)
            {
                Handles.DrawLine(new Vector3(x, 0, StartPoint.y),
                                new Vector3(x, 0, StartPoint.y + 5 * 3.5f));
            }


        }


        private Vector2 mouseGUIPos;
        private float multiplier;
        private void UpdateSceneViewMousePos(SceneView sceneView)
        {
            //计算鼠标位置
            //当前屏幕坐标，左上角（0，0）右下角（camera.pixelWidth，camera.pixelHeight）
            mouseGUIPos = Event.current.mousePosition;
            //Retina屏幕需要拉伸值
            multiplier = EditorGUIUtility.pixelsPerPoint;
            //转换成摄像机可接受的屏幕坐标，左下角（0，0，0）右上角（camera.pixelWidth，camera.pixelHeight，0）
            mouseGUIPos.y = sceneView.camera.pixelHeight - mouseGUIPos.y * multiplier;
            mouseGUIPos.x *= multiplier;

            //获取相机射线
            Ray ray = sceneView.camera.ScreenPointToRay(mouseGUIPos);
            if(ray.origin.y * ray.direction.y < 0)
            {
                //计算
                //0 = ray.origin.y + ray.direction.y * n;
                float n = -ray.origin.y / ray.direction.y;
                mouseWorldPos.x = ray.origin.x + ray.direction.x * n;
                mouseWorldPos.z = ray.origin.z + ray.direction.z * n;
                mouseWorldPos.y = 0;
            }
            else
            {
                //坐标转换
                mouseWorldPos = sceneView.camera.ScreenToWorldPoint(mouseGUIPos);
                mouseWorldPos.y = 0;
            }

            //
            mouseGridsPos.x = Mathf.RoundToInt(mouseWorldPos.x / gridSize.x) * gridSize.x;
            mouseGridsPos.y = 0;
            mouseGridsPos.z = Mathf.RoundToInt(mouseWorldPos.z / gridSize.y) * gridSize.y;
        }
        //private void OnSceneGUI(SceneView sceneView)
        //{
        //    if(sceneView == null)
        //        mousePosition = Vector3.zero;
        //    //当前屏幕坐标，左上角（0，0）右下角（camera.pixelWidth，camera.pixelHeight）
        //    Vector2 mouseGUIPos = Event.current.mousePosition;
        //    //Retina屏幕需要拉伸值
        //    float mult = EditorGUIUtility.pixelsPerPoint;
        //    //转换成摄像机可接受的屏幕坐标，左下角（0，0，0）右上角（camera.pixelWidth，camera.pixelHeight，0）
        //    mouseGUIPos.y = sceneView.camera.pixelHeight - mouseGUIPos.y * mult;
        //    mouseGUIPos.x *= mult;
        //    //近平面往里一些，才能看得到摄像机里的位置
        //    mousePosition = mouseGUIPos;
        //    mousePosition.z = 20;
        //    mousePosition = sceneView.camera.ScreenToWorldPoint(mousePosition);

        //    //射线检测网格
        //    Ray ray = sceneView.camera.ScreenPointToRay(mouseGUIPos);
        //    MeshFilter[] meshFilters = GameObject.FindObjectsOfType<MeshFilter>();
        //    if(meshFilters.Length == 0)
        //        return;
        //    float hitDistance = float.PositiveInfinity;
        //    RaycastHit hit;
        //    foreach(MeshFilter meshFilter in meshFilters)
        //    {
        //        if(meshFilter.sharedMesh == null)
        //            continue;
        //        if(SceneViewIntersectRayMesh.IntersectRayMesh(ray,meshFilter.sharedMesh,meshFilter.transform.localToWorldMatrix,out hit))
        //        {
        //            if(hit.distance < hitDistance)
        //            {
        //                mousePosition = hit.point;
        //                hitDistance = hit.distance;
        //            }
        //        }
        //    }
        //    //Repaint();
        //}


        private bool isMouseDown = false;
        private Vector3 mouseStartPos, mouseEndPos;

        private bool isRectangleDown = false;
        private Vector3 rectangleStartPos, rectangleEndPos;
        private List<float> rectanglePointXList = new List<float>();
        private List<float> rectanglePointZList = new List<float>();

        public void TestCollider(SceneView sceneView)
        {
            if (CollisionTesterFollowMouse)
            {
                CollisionTestStartPos = mouseWorldPos + Vector3.up * 0.2f;
            }
            int RayCount=180;
            for (int i = 0; i < RayCount; i++)
            {
                Vector3 Direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * i * (360 / RayCount)), 0, Mathf.Sin(Mathf.Deg2Rad * i * (360 / RayCount)));
                RaycastHit hit;
                //Physics.Raycast(StartPos, Direction, out hit, 500f);
                 PhysicsSceneExtensions.GetPhysicsScene(PrefabStageUtility.GetCurrentPrefabStage().scene).Raycast(CollisionTestStartPos, Direction, out hit, 500f);
                if (hit.transform != null)
                {
                    Handles.color = Color.green;
                    Handles.DrawLine(CollisionTestStartPos, hit.point);
                }
                else
                {
                    Handles.color = Color.red;
                    Handles.DrawLine(CollisionTestStartPos, CollisionTestStartPos + 500f * Direction);
                }
            }
        }
        


        //事件监听
        private void UpdateSceneViewInputKey(SceneView sceneView)
        {
            Event curEvent = Event.current;

            //一直更新场景
            if (Event.current.type == EventType.MouseMove) SceneView.RepaintAll();
            //输入事件
            if (curEvent.button == 0)
            {
                if(curEvent.type == EventType.MouseDown)
                {
                    isMouseDown = true;
                    mouseStartPos = mouseWorldPos;

                    ///鼠标按下时开始绘制
                    if (!isRectangleDown && previewGameObj!=null)
                    {
                        Debug.Log(previewGameObj);
                        Debug.Log("LeftMouseDown");
                        isRectangleDown = true;
                        rectangleStartPos = mouseWorldPos;
                    }
                }
                if(curEvent.type == EventType.MouseUp || curEvent.type == EventType.MouseLeaveWindow)
                {
                    if(isMouseDown)
                    {
                        isMouseDown = false;
                        mouseEndPos = mouseWorldPos;

                    }
                }
                if(curEvent.type == EventType.MouseDrag)
                {
                }


                //鼠标抬起时完成绘制
                if (curEvent.type == EventType.MouseUp)
                {
                    if (isRectangleDown)
                    {
                        isRectangleDown = false;
                        rectangleEndPos = mouseWorldPos;

                        //填充矩形内的全部物体
                        if (rectanglePointXList.Count > 0 && rectanglePointZList.Count > 0)
                        {
                            List<Vector3> pointList = new List<Vector3>();
                            for (int i = 0; i < rectanglePointXList.Count; i++)
                            {
                                for (int j = 0; j < rectanglePointZList.Count; j++)
                                {
                                    pointList.Add(new Vector3(rectanglePointXList[i], 0, rectanglePointZList[j]));
                                }
                            }
                            Debug.Log("Reselect!");
                            Selection.objects= levelEditorCreater.CreateConnectGameObjs(curSelectContentType, curSelectContentPathName, pointList);
                            rectanglePointXList.Clear();
                            rectanglePointZList.Clear();
                        }
                        //或者，释放一个最近物体
                        else
                        {
                            Selection.activeObject = levelEditorCreater.CreateConnectGameObj(curSelectContentType, curSelectContentPathName, mouseGridsPos);
                            rectanglePointXList.Clear();
                            rectanglePointZList.Clear();
                        }
                    }
                }


            }



            if(curEvent.isKey)
            {
                if (curEvent.type == EventType.KeyDown)
                {
                    if (curEvent.keyCode == KeyCode.E)
                    {
                        levelEditorCreater.CreateEnemySpawnPointByMouse(mouseWorldPos);
                    }
                    else if (curEvent.keyCode == KeyCode.O)
                    {
                        if (!string.IsNullOrEmpty(curSelectContentPathName))
                        {
                            levelEditorCreater.CreateConnectGameObj(curSelectContentType, curSelectContentPathName, mouseGridsPos);
                        }
                    }
                    else if (curEvent.keyCode == KeyCode.B)
                    {
                        if (!CollisionTesterFollowMouse)
                        {
                            CollisionTesterFollowMouse = true;
                        }
                        else
                        {
                            CollisionTesterFollowMouse = false;
                        }
                    }
                    else if(curEvent.keyCode == KeyCode.Q)
                    {
                        if(SpawnPointsOn)
                        {
                            SpawnPointsOn = false;
                        }
                        else
                        {
                            SpawnPointsOn = true;
                        }
                    }
                    else if(curEvent.keyCode == KeyCode.C)
                    {
                        if (CollisionTesterOn)
                        {
                            CollisionTesterOn = false;
                        }
                        else
                        {
                            CollisionTesterOn = true;
                        }
                    }
                    else if (curEvent.keyCode == KeyCode.Escape)
                    {
                        ClearSelectContent();
                      
                    }
                    else if (curEvent.keyCode == KeyCode.RightBracket)
                    {
                        gridSize += new Vector2(0.21875f, 0.21875f);
                    }
                    else if (curEvent.keyCode == KeyCode.LeftBracket)
                    {
                        gridSize -= new Vector2(0.21875f, 0.21875f);
                    }
                }
                else if(curEvent.type == EventType.KeyUp)
                {
                }
            }
            //--------------------------------------------
            ////FocusType.Passive:禁止接收控制焦点
            //Event e = Event.current;
            //if(e != null)
            //{
            //    int controlID = GUIUtility.GetControlID(FocusType.Passive);
            //    if(e.type == EventType.Layout)
            //    {
            //        HandleUtility.AddDefaultControl(controlID);
            //    }
            //}
        }

        Vector3[] drawLinePoints = new Vector3[8];
        private void UpdatePreviewGameObjs()
        {
            //绘制当前幻影所在位置
            if(previewGameObj != null)
            {
                previewGameObj.transform.position = mouseGridsPos;
                //Handles.DrawSphere(0, mouseGridsPos, Quaternion.identity, 0.1f);
                Handles.color = new Color(1,0,0,0.7f);
                Handles.DrawWireArc(mouseGridsPos, Vector3.up, Vector3.right, 360, 0.5f);
                Handles.DrawLine(mouseGridsPos,mouseGridsPos+Vector3.up*1000f);
                Handles.DrawLine(mouseGridsPos- 0.5f * gridSize.y * Vector3.right,mouseGridsPos+ 0.5f * gridSize.y * Vector3.right);
                Handles.DrawLine(mouseGridsPos - 0.5f * gridSize.x * Vector3.forward, mouseGridsPos + 0.5f * gridSize.x * Vector3.forward);
            }
            if(isRectangleDown)
            {
                rectanglePointXList.Clear();
                rectanglePointZList.Clear();

                float startX, endX, startZ, endZ;

                if(rectangleStartPos.x <= mouseWorldPos.x)
                {
                    startX = rectangleStartPos.x;
                    endX = mouseWorldPos.x;
                }
                else
                {
                    startX = mouseWorldPos.x;
                    endX = rectangleStartPos.x;
                }
                if(rectangleStartPos.z <= mouseWorldPos.z)
                {
                    startZ = rectangleStartPos.z;
                    endZ = mouseWorldPos.z;
                }
                else
                {
                    startZ = mouseWorldPos.z;
                    endZ = rectangleStartPos.z;
                }

                startX = Mathf.CeilToInt(startX / gridSize.x) * gridSize.x;
                endX = Mathf.FloorToInt(endX / gridSize.x) * gridSize.x;

                startZ = Mathf.CeilToInt(startZ / gridSize.y) * gridSize.y;
                endZ = Mathf.FloorToInt(endZ / gridSize.y) * gridSize.y;

                for(float x = startX; x <= endX; x += gridSize.x)
                {
                    rectanglePointXList.Add(x);
                }
                for(float z = startZ; z <= endZ; z += gridSize.y)
                {
                    rectanglePointZList.Add(z);
                }

                //if(rectanglePointXList.Count > 0 && rectanglePointZList.Count > 0)
                //{

           
                //绘制选中的矩形
                    for(int i = 0; i < rectanglePointXList.Count; i++)
                    {
                        for(int j = 0; j < rectanglePointZList.Count; j++)
                        {
                            Handles.color = Color.green;
                            Vector3 pointPos = new Vector3(rectanglePointXList[i],0,rectanglePointZList[j]);
                            //Handles.Label(pointPos,"X");
                            Handles.DrawSphere(0,pointPos,Quaternion.identity,0.1f);
                        }
                    }
                    drawLinePoints[0].x = rectangleStartPos.x;
                    drawLinePoints[0].z = rectangleStartPos.z;
                    drawLinePoints[1].x = mouseWorldPos.x;
                    drawLinePoints[1].z = rectangleStartPos.z;

                    drawLinePoints[2].x = mouseWorldPos.x;
                    drawLinePoints[2].z = rectangleStartPos.z;
                    drawLinePoints[3].x = mouseWorldPos.x;
                    drawLinePoints[3].z = mouseWorldPos.z;

                    drawLinePoints[4].x = mouseWorldPos.x;
                    drawLinePoints[4].z = mouseWorldPos.z;
                    drawLinePoints[5].x = rectangleStartPos.x;
                    drawLinePoints[5].z = mouseWorldPos.z;

                    drawLinePoints[6].x = rectangleStartPos.x;
                    drawLinePoints[6].z = mouseWorldPos.z;
                    drawLinePoints[7].x = rectangleStartPos.x;
                    drawLinePoints[7].z = rectangleStartPos.z;

                    Handles.DrawLines(drawLinePoints);
                //}
            }
        }

    }

    [InitializeOnLoad]
    public class SceneViewIntersectRayMesh
    {
        private static MethodInfo intersectRayMeshMethod;

        static SceneViewIntersectRayMesh()
        {
            Type[] editorTypes = typeof(Editor).Assembly.GetTypes();
            Type handleUtilityType = editorTypes.FirstOrDefault(t => t.Name == "HandleUtility");
            intersectRayMeshMethod = handleUtilityType.GetMethod("IntersectRayMesh",BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static bool IntersectRayMesh(Ray ray,MeshFilter meshFilter,out RaycastHit hit)
        {
            return IntersectRayMesh(ray,meshFilter.mesh,meshFilter.transform.localToWorldMatrix,out hit);
        }

        private static object[] parameters = new object[4];
        public static bool IntersectRayMesh(Ray ray,Mesh mesh,Matrix4x4 matrix,out RaycastHit hit)
        {
            parameters[0] = ray;
            parameters[1] = mesh;
            parameters[2] = matrix;
            parameters[3] = null;
            bool result = (bool)intersectRayMeshMethod.Invoke(null,parameters);
            hit = (RaycastHit)parameters[3];
            return result;
        }
    }
}