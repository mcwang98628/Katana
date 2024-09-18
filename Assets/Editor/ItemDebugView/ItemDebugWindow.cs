using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ItemDebugView
{
    public enum ItemDebugType
    {
        All,//所有道具
        ItemPoolEditor,//道具池配置
        ShopEditor,//商店编辑
        RuntimePlayer,//运行时 Player身上已经有的
    }
    
    public partial class ItemDebugWindow : EditorWindow
    {
        [MenuItem("GameTools/ItemDebug/MainWindow")]
        private static void ShowWindow()
        {
            var window = GetWindow<ItemDebugWindow>();
            window.titleContent = new GUIContent("ItemDebugWindow");
            window.Init();
            window.Show();
            // TestFunc();
        }
        
        // static void TestFunc()
        // {
        //     float value = 0;
        //     float value2 = 0.1f;
        //     for (int i = 0; i < 30; i++)
        //     {
        //         value += value2;
        //         value2 -= value2*0.1f;
        //     }
        //     Debug.LogError(value+"==="+value2);
        // }

        public static ItemDebugWindow Inst { get; private set; }
        public ItemScriptableObject CurrentSelectItem;
        
        private bool ItemIsNull => CurrentSelectItem == null;
        private bool IsCanAdd => BattleManager.Inst != null &&
                                 BattleManager.Inst.CurrentPlayer != null &&
                                 BattleManager.Inst.GameIsRuning &&
                                 !ItemIsNull;
        
        private const string AllItemPath = "Assets/AssetsPackage/ScriptObject_Item/AllItem.asset";
        private AllItemObj _allItemObj;
        private string[] DebugTypeNames;
        private string[] ItemTypeNames;
        private string[] PoolTypeNames;
        private ItemDebugType currentSelectDebugType = ItemDebugType.All;
        private ItemType currentSelectItemType = ItemType.All;
        private ItemPoolType currentSelectPoolType = ItemPoolType.Lv1;
        private void Init()
        {
            _allItemObj = AssetDatabase.LoadAssetAtPath<AllItemObj>(AllItemPath);
            _itemPool = AssetDatabase.LoadAssetAtPath<ItemPool>(ItemPoolPath);
            DebugTypeNames = Enum.GetNames(typeof(ItemDebugType));
            ItemTypeNames = Enum.GetNames(typeof(ItemType));
            PoolTypeNames = Enum.GetNames(typeof(ItemPoolType));
        }

        private void OnEnable()
        {
            Inst = this;
        }

        private void OnGUI()
        {
            RebuildLayout(this.position);
            DrawGUI();
            EventFunc();
        }

        private Rect _fullWindowRect;
        
        private Rect _debugTypeBtnRect;
        private const int _debugTypeBtnHeight = 20;
        private float DebugTypeBtnWidth => _debugTypeBtnRect.width / DebugTypeNames.Length;
        
        private Rect _itemTypeBtnRect;
        private const int _itemTypeBtnHeight = 20;
        private float ItemTypeBtnWidth => _itemTypeBtnRect.width / ItemTypeNames.Length;

        private Rect _itemListZoneRect;//Item展示区
        private const int _itemWidth = 70;//宽度
        private const int _itemHeight = 70;//高度
        private const int _itemXInterval = 10;//X间隔
        private const int _itemYInterval = 20;//Y间隔

        private Rect _itemListSliderRect;//垂直滚动条
        private float _itemListSliderValue = 0;

        
        private Rect _itemAddBtnRect;//添加道具
        private Rect _itemRemoveBtnRect;//移除道具
        private Rect _itemSelectRect;//选择资源
        
        private Rect _poolTypeBtnRect;
        private const int _poolTypeBtnHeight = 20;
        private float PoolTypeBtnWidth => _poolTypeBtnRect.width / PoolTypeNames.Length;
        private Rect _probabilityWeights;//概率权重
        
        private void RebuildLayout(Rect rect)
        {
            _fullWindowRect = rect;
            _fullWindowRect.yMax = _fullWindowRect.height;
            _fullWindowRect.yMin = 0;
            _fullWindowRect.xMax = _fullWindowRect.width;
            _fullWindowRect.xMin = 0;
            
            _debugTypeBtnRect = _fullWindowRect;
            _debugTypeBtnRect.yMax = _debugTypeBtnRect.yMin + _debugTypeBtnHeight;
            
            _itemTypeBtnRect = _fullWindowRect;
            _itemTypeBtnRect.yMin = _debugTypeBtnRect.yMax;
            _itemTypeBtnRect.yMax = _itemTypeBtnRect.yMin + _itemTypeBtnHeight;

            _itemListZoneRect = _fullWindowRect;
            _itemListZoneRect.yMin = _itemTypeBtnRect.yMax + 7;
            _itemListZoneRect.xMax = _itemListZoneRect.xMax - 20;


            _itemAddBtnRect = _itemListZoneRect;
            _itemRemoveBtnRect = _itemListZoneRect;
            _itemSelectRect = _itemListZoneRect;
            _itemAddBtnRect.yMin = _itemAddBtnRect.yMax - 25;
            _itemRemoveBtnRect.yMin = _itemRemoveBtnRect.yMax - 25;
            _itemSelectRect.yMin = _itemSelectRect.yMax - 25;
            
            float itemWidth = _itemListZoneRect.width / 3f;
            _itemAddBtnRect.xMax = _itemAddBtnRect.xMin + itemWidth;
            _itemRemoveBtnRect.xMin = _itemAddBtnRect.xMax;
            _itemRemoveBtnRect.xMax = _itemRemoveBtnRect.xMin + itemWidth;
            _itemSelectRect.xMin = _itemRemoveBtnRect.xMax; 
            _itemSelectRect.xMax = _itemSelectRect.xMin + itemWidth;

            
            _poolTypeBtnRect = _fullWindowRect;
            _poolTypeBtnRect.yMin = _itemTypeBtnRect.yMax;
            _poolTypeBtnRect.yMax = _poolTypeBtnRect.yMin + _poolTypeBtnHeight;
            
            _probabilityWeights = _itemListZoneRect;
            _probabilityWeights.yMin = _probabilityWeights.yMax - 25;
            
        }

        private void DrawGUI()
        {
            DrawDebugTypeBtn();
            DrawItemZone();
        }

        private void DrawDebugTypeBtn()
        {
            Color oldBgColor = GUI.backgroundColor;
            for (int i = 0; i < DebugTypeNames.Length; i++)
            {
                string typeName = DebugTypeNames[i];
                Rect currentBtnRect = _debugTypeBtnRect;
                currentBtnRect.xMin = i * DebugTypeBtnWidth;
                currentBtnRect.xMax = currentBtnRect.xMin + DebugTypeBtnWidth;
                
                GUI.backgroundColor = currentSelectDebugType.ToString() == typeName ? Color.cyan : oldBgColor;
                
                if (GUI.Button(currentBtnRect,typeName))
                {
                    currentSelectDebugType = (ItemDebugType)Enum.Parse(typeof(ItemDebugType), typeName);
                    GUI.FocusControl("");
                }
            }
            GUI.backgroundColor = oldBgColor;
        }
        private void DrawItemTypeBtn()
        {
            Color oldBgColor = GUI.backgroundColor;
            for (int i = 0; i < ItemTypeNames.Length; i++)
            {
                string typeName = ItemTypeNames[i];
                Rect currentBtnRect = _itemTypeBtnRect;
                currentBtnRect.xMin = i * ItemTypeBtnWidth;
                currentBtnRect.xMax = currentBtnRect.xMin + ItemTypeBtnWidth;
                
                GUI.backgroundColor = currentSelectItemType.ToString() == typeName ? Color.cyan : oldBgColor;
                
                if (GUI.Button(currentBtnRect,typeName))
                {
                    currentSelectItemType = (ItemType)Enum.Parse(typeof(ItemType), typeName);
                }
            }
            GUI.backgroundColor = oldBgColor;
        }




        private void DrawBottomBtn()
        {
            Color oldColor = GUI.color;
            
            GUI.color = Color.green;
            if (GUI.Button(_itemAddBtnRect,"添加道具") && IsCanAdd)
            {
                BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(
                    DataManager.Inst.ParsingItemObj(CurrentSelectItem), delegate(bool b)
                    {
                        if (b)
                            this.ShowNotification(new GUIContent($"已添加:{LocalizationManger.Inst.GetText(CurrentSelectItem.Name)}"));
                        else
                            this.ShowNotification(new GUIContent($"Error"));
                    });
            }
            
            GUI.color = Color.red;
            if (GUI.Button(_itemRemoveBtnRect,"移除道具") && IsCanAdd)
            {
                int itemId = DataManager.Inst.GetItemId(CurrentSelectItem);
                bool remove = BattleManager.Inst.CurrentPlayer.roleItemController.ReMoveItemByID(itemId);
                if (remove)
                    this.ShowNotification(new GUIContent($"移除:{LocalizationManger.Inst.GetText(CurrentSelectItem.Name)}"));
                else
                    this.ShowNotification(new GUIContent($"玩家-无:{LocalizationManger.Inst.GetText(CurrentSelectItem.Name)}"));
            }
            
            GUI.color = Color.yellow;
            if (GUI.Button(_itemSelectRect,"选中资源"))
            {
                Selection.SetActiveObjectWithContext(CurrentSelectItem,this);
                this.ShowNotification(new GUIContent($"选中资源"));
            }
            GUI.color = oldColor;
        }

        private void DrawRuntimePlayerItems()
        {
            if (!Application.isPlaying)
            {
                this.ShowNotification(new GUIContent("游戏没运行"));
                return;
            }
            if (!BattleManager.Inst.GameIsRuning)
            {
                this.ShowNotification(new GUIContent("没有进入战斗"));
                return;
            }
            if (BattleManager.Inst.CurrentPlayer == null)
            {
                this.ShowNotification(new GUIContent("找不到Player"));
                return;
            }

            var itemIds = BattleManager.Inst.CurrentPlayer.roleItemController.GetItemsId();
            List<ItemScriptableObject> itemObjs = new List<ItemScriptableObject>();
            foreach (int itemId in itemIds)
                itemObjs.Add(DataManager.Inst.GetItemScrObj(itemId));
            DrawItemList(itemObjs,_itemListZoneRect,SelectItem);
        }
        

        private void SelectItem(ItemScriptableObject itemObj)
        {
            if (ItemDebugInspector.Inst != null)
            {
                ItemDebugInspector.Inst.TargetItemObject = itemObj;
                ItemDebugInspector.Inst.Repaint();
            }
            CurrentSelectItem = itemObj;
            this.Repaint();
        }

        private void EventFunc()
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.ScrollWheel)
            {
                if (_itemListZoneRect.Contains(currentEvent.mousePosition))
                {
                    _itemListSliderValue += currentEvent.delta.y * 0.01f;
                    if (_itemListSliderValue < 0)
                        _itemListSliderValue = 0;
                    else if (_itemListSliderValue > 1)
                        _itemListSliderValue = 1;
                    currentEvent.Use();
                    this.Repaint();
                }
            }
        }
        
        
        
    }
}