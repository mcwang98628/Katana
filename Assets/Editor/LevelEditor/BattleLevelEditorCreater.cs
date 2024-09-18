using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine.SceneManagement;

namespace LevelEditor
{
    //摆放物品类型
    public enum LevelContentType
    {
        Collision,
        NoCollision,
        Breakable,
        OtherModule,
    }

    public class BattleLevelEditorCreater
    {
        //战斗房间prefab保存路径
        public const string createPrefabDefaultPath = "Assets/Arts/RoomPrefabs/Rooms/chapter";

        //战斗房间框架prefab路径
        private const string RoomBasePath = "Modules/RoomFrame";
        //战斗房间框架prefab名字
        private const string roomFrameNodeName = "RoomFrame";
        //战斗房间摆放物品根节点名字
        private const string roomContentNodeName = "RoomContent";
        //战斗房间敌人出生点根节点名字
        private const string spawnPointsNodeName = "EnemySpawnPoints";

        //战斗房间prefab命名格式
        private const string createPrefabNameFormat = "Chapter_{0}_{1}_FightRoom_{2}";

        //战斗房间根节点缩放值
        private const float createPrefabLocalScale = 0.7f;
        //战斗房间起始Id
        private const int roomIdStartId = 1;
        //敌人出生点场景select icon,(4:黄色圆形)
        private const int enemyPointSelectIconIndex = 4;

        //敌人之间最小距离
        private const float enemyToEnemyDistance = 1;
        //敌人到障碍物边缘最小距离
        private const float enemyToBlockDistance = 0.5f;

        private Transform instanceRoot;
        public  Transform spawnPointsRoot;
        private Dictionary<LevelContentType,Transform> roomContentRootDic = new Dictionary<LevelContentType,Transform>();

        private Vector4 floorScope;

        //public void CreateOrOpenInstanc(string loadContentPath)
        //{
        //    GameObject[] sceneRootObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        //    List<Transform> levelRootList = new List<Transform>();
        //    for(int i = 0; i < sceneRootObjs.Length; i++)
        //    {
        //        if(sceneRootObjs[i].GetComponent<FightRoom>() != null)
        //            levelRootList.Add(sceneRootObjs[i].transform);
        //    }

        //    if(levelRootList.Count == 0)
        //        CreateNewInstance(loadContentPath);
        //    else if(levelRootList.Count == 1)
        //        OpenExistInstance(levelRootList[0],loadContentPath);
        //    else
        //    {
        //        EditorUtility.DisplayDialog("关卡编辑器错误","场景内存在多个关卡文件","确定");
        //        Debug.LogError("场景内存在多个关卡文件，需手动删除");
        //        return;
        //    }
        //}


        //直接加载Prefab的根文件目录
        public void SetCurrentRoot(string loadContentPath)
        {
            GameObject sceneRootObj = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot;

            //List<Transform> levelRootList = new List<Transform>();
            //    if (sceneRootObj.GetComponent<FightRoom>() != null)
            //        levelRootList.Add(sceneRootObj.transform);

            //if (levelRootList.Count == 0)
            //    CreateNewInstance(loadContentPath);
            //else if (levelRootList.Count == 1)
            //OpenExistInstance(levelRootList[0], loadContentPath);
            //else
            //{
            //    EditorUtility.DisplayDialog("关卡编辑器错误", "场景内存在多个关卡文件", "确定");
            //    Debug.LogError("场景内存在多个关卡文件，需手动删除");
            //    return;
            //}
            OpenExistInstance(sceneRootObj.transform,loadContentPath);
        }


        public void CreateConnectGameObj(LevelContentType contentType,string contentPath)
        {
            CreateConnectGameObj(contentType,contentPath,Vector3.zero);
        }
        public GameObject CreateConnectGameObj(LevelContentType contentType,string contentPath,Vector3 position)
        {
            //if(roomContentRootDic == null || !roomContentRootDic.ContainsKey(contentType) || roomContentRootDic[contentType] == null)
            //    return null;

            //如果路径为空就赶快生成一个。
            if (roomContentRootDic == null || !roomContentRootDic.ContainsKey(contentType) || roomContentRootDic[contentType] == null)
            {
                CreateRoomContentObj();
            }


            GameObject contentSource = AssetDatabase.LoadAssetAtPath<GameObject>(contentPath);

            
            if(contentSource == null)
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","加载路径错误","确定");
                Debug.LogError("加载路径错误 : " + contentPath);
                return null;
            }

            Undo.IncrementCurrentGroup();
            //将生成的物体放进了当前的预制体里。
            //GameObject contentObj = PrefabUtility.InstantiatePrefab(contentSource,roomContentRootDic[contentType].transform, PrefabStageUtility.GetCurrentPrefabStage().scene) as GameObject;
            GameObject contentObj = PrefabUtility.InstantiatePrefab(contentSource, roomContentRootDic[contentType].transform) as GameObject;
            contentObj.transform.position = position;
            contentObj.transform.rotation = Quaternion.identity;
            //出去这些的Scale
            //contentObj.transform.localScale = Vector3.one;

            Undo.RegisterCreatedObjectUndo(contentObj,"Create " + contentObj.name);

            return contentObj;
            //EditorUtility.SetDirty(contentObj);
        }
        public GameObject[] CreateConnectGameObjs(LevelContentType contentType,string contentPath,List<Vector3> positionList)
        {
            if (roomContentRootDic == null || !roomContentRootDic.ContainsKey(contentType) || roomContentRootDic[contentType] == null)
            {
                CreateRoomContentObj();
            }
            //记录生成的物体



            GameObject[] ResultObjArray= new GameObject[positionList.Count];



            GameObject contentSource = AssetDatabase.LoadAssetAtPath<GameObject>(contentPath);
            if(contentSource == null)
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","加载路径错误","确定");
                Debug.LogError("加载路径错误 : " + contentPath);
                return null;
            }
            if(positionList == null || positionList.Count == 0)
                return null;

            Undo.IncrementCurrentGroup();

            for(int i = 0; i < positionList.Count; i++)
            {
                //将生成的物体放进了当前的预制体里。
                GameObject contentObj = PrefabUtility.InstantiatePrefab(contentSource, roomContentRootDic[contentType].transform) as GameObject;
                contentObj.transform.position = positionList[i];
                contentObj.transform.rotation = Quaternion.identity;
                //除去这些物体的Scale
                //contentObj.transform.localScale = Vector3.one;

                ResultObjArray[i] = contentObj;
                Undo.RegisterCreatedObjectUndo(contentObj,"Create " + contentObj.name);
            }

            return ResultObjArray;
        }

        public GameObject CreatePreviewGameObj(string contentPath)
        {
            GameObject contentSource = AssetDatabase.LoadAssetAtPath<GameObject>(contentPath);
            if(contentSource == null)
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","加载路径错误","确定");
                Debug.LogError("加载路径错误 : " + contentPath);
                return null;
            }
            //将预览物体放置到当前的预制体里
            GameObject contentObj = PrefabUtility.InstantiatePrefab(contentSource, PrefabStageUtility.GetCurrentPrefabStage().scene) as GameObject;

            //contentObj.transform.position = position;
            contentObj.transform.rotation = Quaternion.identity;

            contentObj.transform.localScale = contentObj.transform.localScale * createPrefabLocalScale;

            return contentObj;
        }

        public void CreateEnemySpawnPointsByRandom(int pointCount,bool reset)
        {
            if(spawnPointsRoot == null)
                return;

            if(reset)
            {
                if(spawnPointsRoot.transform.childCount > 0)
                {
                    Undo.IncrementCurrentGroup();

                    for(int i = spawnPointsRoot.transform.childCount - 1; i >= 0; i--)
                    {
                        //GameObject.DestroyImmediate(spawnPointsRoot.transform.GetChild(i).gameObject);
                        Undo.DestroyObjectImmediate(spawnPointsRoot.transform.GetChild(i).gameObject);
                    }
                }

                List<Vector3> enemyPointList = GetRandomEnemyPoint(null,pointCount);
                if(enemyPointList == null || enemyPointList.Count == 0)
                    return;

                Undo.IncrementCurrentGroup();

                for(int i = 0; i < enemyPointList.Count; i++)
                {
                    Transform newPoint = new GameObject().transform;
                    newPoint.SetParent(spawnPointsRoot);
                    newPoint.position = enemyPointList[i];
                    newPoint.rotation = Quaternion.identity;
                    newPoint.localScale = Vector3.one;

                    Undo.RegisterCreatedObjectUndo(newPoint.gameObject,"Create New Point");

                    SetGameObjSelectIcon(newPoint.gameObject,enemyPointSelectIconIndex);
                }
            }
            else
            {
                if(pointCount == spawnPointsRoot.transform.childCount)
                    return;

                if(pointCount < spawnPointsRoot.transform.childCount)
                {
                    Undo.IncrementCurrentGroup();

                    for(int i = spawnPointsRoot.transform.childCount - 1; i >= pointCount; i--)
                    {
                        //GameObject.DestroyImmediate(spawnPointsRoot.transform.GetChild(i).gameObject);
                        Undo.DestroyObjectImmediate(spawnPointsRoot.transform.GetChild(i).gameObject);
                    }
                }
                else
                {
                    List<Vector3> alreadyPointList = new List<Vector3>();

                    for(int i = 0; i < spawnPointsRoot.transform.childCount; i++)
                    {
                        alreadyPointList.Add(spawnPointsRoot.transform.GetChild(i).transform.position);
                    }

                    List<Vector3> enemyPointList = GetRandomEnemyPoint(alreadyPointList,pointCount);
                    if(enemyPointList == null || enemyPointList.Count == 0)
                        return;

                    Undo.IncrementCurrentGroup();

                    for(int i = 0; i < enemyPointList.Count; i++)
                    {
                        Transform newPoint = new GameObject().transform;
                        newPoint.SetParent(spawnPointsRoot);
                        newPoint.position = enemyPointList[i];
                        newPoint.rotation = Quaternion.identity;
                        newPoint.localScale = Vector3.one;

                        Undo.RegisterCreatedObjectUndo(newPoint.gameObject,"Create New Point");

                        SetGameObjSelectIcon(newPoint.gameObject,enemyPointSelectIconIndex);
                    }
                }
            }
        }

        public void CreateEnemySpawnPointByMouse(Vector3 mousePosition)
        {
            if(spawnPointsRoot == null)
            {
                Debug.LogError("未创建敌人出生点根节点");
                return;
            }

            if(mousePosition.y < -0.1f || mousePosition.y > 0.1f)
                return;

            float minPosX = floorScope.x + enemyToEnemyDistance;
            float maxPosX = floorScope.y - enemyToEnemyDistance;
            float minPosY = floorScope.z + enemyToEnemyDistance;
            float maxPosY = floorScope.w - enemyToEnemyDistance;

            //if(mousePosition.x < minPosX || mousePosition.x > maxPosX || mousePosition.z < minPosY || mousePosition.z > maxPosY)
            //{
            //    Debug.LogError("位置超出地图范围");
            //    return;
            //}

            //RaycastHit raycastHit;
            //if(Physics.SphereCast(mousePosition + new Vector3(0,20,0),enemyToBlockDistance,Vector3.down,out raycastHit,30))
            //{
            //    Debug.LogError("位置与已有物品重叠");
            //    return;
            //}

            mousePosition.y = 0;

            Undo.IncrementCurrentGroup();

            Transform newPoint = new GameObject().transform;
            newPoint.SetParent(spawnPointsRoot);
            newPoint.position = mousePosition;
            newPoint.rotation = Quaternion.identity;
            newPoint.localScale = Vector3.one;

            Undo.RegisterCreatedObjectUndo(newPoint.gameObject,"Create New Point");

            SetGameObjSelectIcon(newPoint.gameObject,enemyPointSelectIconIndex);
        }

        public void SaveLevelPrefab(string createPrefabPath,int chapter,int level)
        {
            if(string.IsNullOrEmpty(createPrefabPath))
            {
                Debug.LogError("保存路径为空");
                return;
            }
            if(chapter < 0 || level < 0)
            {
                Debug.LogError("Chapter / Level < 0");
                return;
            }
            createPrefabPath = createPrefabPath.Replace('\\','/');

            int roomId = roomIdStartId;
            if(Directory.Exists(createPrefabPath))
                roomId = GetNextRoomId(createPrefabPath,chapter,level);
            else
                Directory.CreateDirectory(createPrefabPath);

            string prefabName = string.Format(createPrefabNameFormat,chapter,level,roomId);
            string savePath = string.Format("{0}/{1}.prefab",createPrefabPath,prefabName);

            bool savePrefabResult;
            PrefabUtility.SaveAsPrefabAsset(instanceRoot.gameObject,savePath,out savePrefabResult);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if(savePrefabResult)
                Debug.Log("保存成功 : " + savePath);
            else
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","保存失败","确定");
                Debug.LogError("保存失败 : " + prefabName);
            }
        }

        //--------------------------------------------------

        private void CreateNewInstance(string loadContentPath)
        {
            loadContentPath = loadContentPath.Replace('\\','/');

            //创建根节点
            instanceRoot = new GameObject("PrefabRoot").transform;
            instanceRoot.position = Vector3.zero;
            instanceRoot.rotation = Quaternion.identity;
            instanceRoot.localScale = new Vector3(createPrefabLocalScale,createPrefabLocalScale,createPrefabLocalScale);
            instanceRoot.gameObject.AddComponent<FightRoom>();

            //加载房间框架
            string roomFramePath = string.Format("{0}/{1}.prefab",loadContentPath,RoomBasePath);
            GameObject roomFrameSource = AssetDatabase.LoadAssetAtPath<GameObject>(roomFramePath);
            if(roomFrameSource == null)
            {
                Debug.LogError("加载路径错误 : " + roomFramePath);
                return;
            }
            //会直接创建一个RoomFrame
            //Transform roomFrameTrans = (PrefabUtility.InstantiatePrefab(roomFrameSource,instanceRoot) as GameObject).transform;
            //roomFrameTrans.position = Vector3.zero;
            //roomFrameTrans.rotation = Quaternion.identity;
            //roomFrameTrans.localScale = Vector3.one;
            //计算房间范围
            //floorScope = GetFloorScope(roomFrameTrans);

           
            //CreateRoomContentObj();

            //创建敌人出根生点
            spawnPointsRoot = new GameObject(spawnPointsNodeName).transform;
            spawnPointsRoot.SetParent(instanceRoot);
            spawnPointsRoot.position = Vector3.zero;
            spawnPointsRoot.rotation = Quaternion.identity;
            spawnPointsRoot.localScale = Vector3.one;
        }
        public void CreateRoomContentObj()
        {
            //创建摆放物根节点
            Transform roomContentRoot = new GameObject(roomContentNodeName).transform;
            roomContentRoot.SetParent(PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.transform);
            roomContentRoot.position = Vector3.zero;
            roomContentRoot.rotation = Quaternion.identity;
            roomContentRoot.localScale = Vector3.one;
            foreach (LevelContentType contentType in System.Enum.GetValues(typeof(LevelContentType)))
            {
                roomContentRootDic[contentType] = new GameObject(contentType.ToString() + "Obj").transform;
                roomContentRootDic[contentType].SetParent(roomContentRoot);
                roomContentRootDic[contentType].position = Vector3.zero;
                roomContentRootDic[contentType].rotation = Quaternion.identity;
                roomContentRootDic[contentType].localScale = Vector3.one;
            }
            Undo.RegisterCreatedObjectUndo(roomContentRoot,"CreateContentRoot");
        }

        //检查房间
        private void OpenExistInstance(Transform roomRootTrans,string loadContentPath)
        {
            if(PrefabUtility.IsAnyPrefabInstanceRoot(roomRootTrans.gameObject))
            {
                PrefabUtility.UnpackPrefabInstance(roomRootTrans.gameObject,PrefabUnpackMode.OutermostRoot,InteractionMode.AutomatedAction);
            }

            //设置/校验根节点
            instanceRoot = roomRootTrans;
            instanceRoot.position = Vector3.zero;
            instanceRoot.rotation = Quaternion.identity;
            instanceRoot.localScale = new Vector3(createPrefabLocalScale,createPrefabLocalScale,createPrefabLocalScale);

            ////校验房间框架
            ///






            //Transform roomFrameTrans = instanceRoot.Find(roomFrameNodeName);
            //if(roomFrameTrans == null)
            //{
            //    loadContentPath = loadContentPath.Replace('\\','/');

            //    string roomFramePath = string.Format("{0}/{1}.prefab",loadContentPath,RoomBasePath);
            //    GameObject roomFrameSource = AssetDatabase.LoadAssetAtPath<GameObject>(roomFramePath);
            //    if(roomFrameSource == null)
            //    {
            //        Debug.LogError("加载路径错误 : " + roomFramePath);
            //        return;
            //    }
            //    GameObject roomFrameObj = PrefabUtility.InstantiatePrefab(roomFrameSource,instanceRoot.transform) as GameObject;
            //    roomFrameTrans = roomFrameObj.transform;
            //}
            //roomFrameTrans.position = Vector3.zero;
            //roomFrameTrans.rotation = Quaternion.identity;
            //roomFrameTrans.localScale = Vector3.one;
            ////计算房间范围
            //floorScope = GetFloorScope(roomFrameTrans);

            //校验摆放物根节点
            CheckRoomContentPath();

            //校验敌人出根生点
            spawnPointsRoot = instanceRoot.Find(spawnPointsNodeName);
            if(spawnPointsRoot == null)
            {
                spawnPointsRoot = new GameObject(spawnPointsNodeName).transform;
                spawnPointsRoot.SetParent(instanceRoot);
            }
            spawnPointsRoot.position = Vector3.zero;
            spawnPointsRoot.rotation = Quaternion.identity;
            spawnPointsRoot.localScale = Vector3.one;
        }
        //检查房间内是否有可以摆放物品的地方
        public void CheckRoomContentPath()
        {
            Transform roomContentRoot = instanceRoot.Find(roomContentNodeName);
            if (roomContentRoot == null)
            {
                roomContentRoot = new GameObject(roomContentNodeName).transform;
                roomContentRoot.SetParent(instanceRoot);
            }
            roomContentRoot.position = Vector3.zero;
            roomContentRoot.rotation = Quaternion.identity;
            roomContentRoot.localScale = Vector3.one;

            foreach (LevelContentType contentType in System.Enum.GetValues(typeof(LevelContentType)))
            {
                roomContentRootDic[contentType] = roomContentRoot.Find(contentType.ToString() + "Obj");
                if (roomContentRootDic[contentType] == null)
                {
                    roomContentRootDic[contentType] = new GameObject(contentType.ToString() + "Obj").transform;
                    roomContentRootDic[contentType].SetParent(roomContentRoot);
                }
                roomContentRootDic[contentType].position = Vector3.zero;
                roomContentRootDic[contentType].rotation = Quaternion.identity;
                roomContentRootDic[contentType].localScale = Vector3.one;
            }
        }
        public void CheckRoomFrame()
        {

        }

        private int GetNextRoomId(string createPrefabPath,int chapter,int level)
        {
            createPrefabPath = createPrefabPath.Replace('\\','/');

            if(!Directory.Exists(createPrefabPath))
                return roomIdStartId;

            string[] levelPrefabFullPaths = Directory.GetFiles(createPrefabPath,"*.prefab",SearchOption.TopDirectoryOnly);
            if(levelPrefabFullPaths.Length == 0)
                return roomIdStartId;

            string levelPrefabName = string.Format(createPrefabNameFormat,chapter,level,null);

            List<int> roomIdList = new List<int>();
            for(int i = 0; i < levelPrefabFullPaths.Length; i++)
            {
                string levelName = levelPrefabFullPaths[i].Split('/').LastOrDefault().Split('.').FirstOrDefault();
                if(levelName.Contains(levelPrefabName))
                {
                    levelName = levelName.Split('_').LastOrDefault();
                    int tempRoomId;
                    if(int.TryParse(levelName,out tempRoomId))
                    {
                        roomIdList.Add(tempRoomId);
                    }
                }
            }
            roomIdList.Sort();
            if(roomIdList.Count > 0)
            {
                if(roomIdList[0] > roomIdStartId)
                    return roomIdStartId;

                for(int j = 0; j < roomIdList.Count - 1; j++)
                {
                    if(roomIdList[j + 1] - roomIdList[j] > 1)
                        return roomIdList[j] + 1;
                }

                return roomIdList[roomIdList.Count - 1] + 1;
            }

            return roomIdStartId;
        }

        private Vector4 GetFloorScope(Transform roomFrameTrans)
        {
            //前提条件：地板行数>=2,列数>=2
            RandomFloor[] randomFloors = roomFrameTrans.GetComponentsInChildren<RandomFloor>();
            if(randomFloors.Length == 0)
                return Vector4.zero;

            Vector2 firstFloorPos = new Vector2(randomFloors[0].transform.position.x,randomFloors[0].transform.position.z);
            float minPosX = firstFloorPos.x, maxPosX = firstFloorPos.x, minPosY = firstFloorPos.y, maxPosY = firstFloorPos.y;
            float spaceX = 0, spaceY = 0;
            for(int i = 1; i < randomFloors.Length; i++)
            {
                Vector2 floorPos = new Vector2(randomFloors[i].transform.position.x,randomFloors[i].transform.position.z);

                minPosX = floorPos.x < minPosX ? floorPos.x : minPosX;
                maxPosX = floorPos.x > maxPosX ? floorPos.x : maxPosX;
                minPosY = floorPos.y < minPosY ? floorPos.y : minPosY;
                maxPosY = floorPos.y > maxPosY ? floorPos.y : maxPosY;

                float tempSpace = Mathf.Abs(floorPos.x - firstFloorPos.x);
                if(tempSpace > 0.1f)
                    spaceX = spaceX < 0.1f ? tempSpace : (tempSpace < spaceX ? tempSpace : spaceX);

                tempSpace = Mathf.Abs(floorPos.y - firstFloorPos.y);
                if(tempSpace > 0.1f)
                    spaceY = spaceY < 0.1f ? tempSpace : (tempSpace < spaceY ? tempSpace : spaceY);
            }
            minPosX -= spaceX * 0.5f;
            maxPosX += spaceX * 0.5f;
            minPosY -= spaceY * 0.5f;
            maxPosY += spaceY * 0.5f;
            return new Vector4(minPosX,maxPosX,minPosY,maxPosY);
        }

        private List<Vector3> GetRandomEnemyPoint(List<Vector3> alreadyPointList,int pointCount)
        {
            if(pointCount <= 0)
                return null;

            float minPosX = floorScope.x + enemyToBlockDistance;
            float maxPosX = floorScope.y - enemyToBlockDistance;
            float minPosY = floorScope.z + enemyToBlockDistance;
            float maxPosY = floorScope.w - enemyToBlockDistance;

            if(minPosX >= maxPosX || minPosY >= maxPosY)
                return null;

            bool isRemote;
            Vector3 tempPoint;
            RaycastHit raycastHit;

            List<Vector3> pointList = new List<Vector3>();
            //出生点数量
            for(int i = 0; i < pointCount; i++)
            {
                //每个出生点随机次数上限
                for(int c = 0; c < 1000; c++)
                {
                    //射线检测障碍物
                    tempPoint = new Vector3(Random.Range(minPosX,maxPosX),20,Random.Range(minPosY,maxPosY));
                    if(Physics.SphereCast(tempPoint,enemyToBlockDistance,Vector3.down,out raycastHit,30))
                    {
                    }
                    else
                    {
                        //判断与已有出生点距离
                        isRemote = true;
                        tempPoint.y = 0;
                        if(alreadyPointList != null && alreadyPointList.Count > 0)
                        {
                            for(int m = 0; m < alreadyPointList.Count; m++)
                            {
                                if((tempPoint - alreadyPointList[m]).sqrMagnitude < enemyToEnemyDistance * enemyToEnemyDistance)
                                {
                                    isRemote = false;
                                    break;
                                }
                            }
                        }
                        if(isRemote)
                        {
                            for(int n = 0; n < pointList.Count; n++)
                            {
                                if((tempPoint - pointList[n]).sqrMagnitude < enemyToEnemyDistance * enemyToEnemyDistance)
                                {
                                    isRemote = false;
                                    break;
                                }
                            }
                        }
                        if(isRemote)
                        {
                            pointList.Add(tempPoint);
                            break;
                        }
                    }
                }
            }
            if(pointList.Count < pointCount)
                Debug.LogError(string.Format("实际生成出生点数量 : {0}/{1}",pointList.Count,pointCount));
            return pointList;
        }

        #region --- Select Icon ---

        private static GUIContent[] largeIcons;

        public static void SetGameObjSelectIcon(GameObject gameObj,int iconIndex)
        {
            if(gameObj == null)
                return;

            if(largeIcons == null)
            {
                string baseName = "sv_icon_dot";
                string postFix = "_pix16_gizmo";
                int startIndex = 0;
                int count = 16;

                largeIcons = new GUIContent[count];

                var ty = typeof(EditorGUIUtility);
                var iconContentMethod = ty.GetMethod("IconContent",BindingFlags.Public | BindingFlags.Static,null,new System.Type[] { typeof(string) },null);

                for(int index = 0; index < count; index++)
                {
                    largeIcons[index] = iconContentMethod.Invoke(null,new object[] { baseName + (object)(startIndex + index) + postFix }) as GUIContent;
                }
            }

            if(iconIndex < 0 || iconIndex >= largeIcons.Length)
                return;

            Texture2D texture = largeIcons[iconIndex].image as Texture2D;

            var type = typeof(EditorGUIUtility);
            var setIconForObjectMethod = type.GetMethod("SetIconForObject",BindingFlags.NonPublic | BindingFlags.Static);
            setIconForObjectMethod.Invoke(null,new object[] { gameObj,texture });
        }

        #endregion
    }

}