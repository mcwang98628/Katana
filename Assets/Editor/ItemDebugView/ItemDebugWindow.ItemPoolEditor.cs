

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ItemDebugView
{
    public partial class ItemDebugWindow
    {
        private const string ItemPoolPath = "Assets/AssetsPackage/ScriptObject_Item/ItemPool.asset";
        private ItemPool _itemPool;
    
        private void DrawItemPoolTypeBtn()
        {
            Color oldBgColor = GUI.backgroundColor;
            for (int i = 0; i < PoolTypeNames.Length; i++)
            {
                string typeName = PoolTypeNames[i];
                Rect currentBtnRect = _poolTypeBtnRect;
                currentBtnRect.xMin = i * PoolTypeBtnWidth;
                currentBtnRect.xMax = currentBtnRect.xMin + PoolTypeBtnWidth;
                
                GUI.backgroundColor = currentSelectPoolType.ToString() == typeName ? Color.cyan : oldBgColor;
                
                if (GUI.Button(currentBtnRect,typeName))
                {
                    currentSelectPoolType = (ItemPoolType)Enum.Parse(typeof(ItemPoolType), typeName);
                    GUI.FocusControl("");
                }
            }
            GUI.backgroundColor = oldBgColor;
        }

        
        private void DrawItemPoolEditor()
        {
            DrawItemPoolTypeBtn();
            Rect newRect = _itemListZoneRect;
            newRect.yMin = _poolTypeBtnRect.yMax;
            DrawItemList(_allItemObj.objs,newRect,ItemPoolOnItemMouseL,ItemPoolOnItemMouseR);
        }

        private ItemPoolData CurrentPoolIsContains(ItemScriptableObject item)
        {
            List<ItemPoolData> poolDatas = new List<ItemPoolData>();
            switch (currentSelectPoolType)
            {
                case ItemPoolType.Lv1:
                    poolDatas = _itemPool.Lv1;
                    break;
                case ItemPoolType.Lv2:
                    poolDatas = _itemPool.Lv2;
                    break;
                case ItemPoolType.Lv3:
                    poolDatas = _itemPool.Lv3;
                    break;
                case ItemPoolType.Shop:
                    poolDatas = _itemPool.Shop;
                    break;
            }

            return IsContains(poolDatas,item);
        }
        

        private void ItemPoolOnItemMouseL(ItemScriptableObject itemObj)
        {
            SelectItem(itemObj);
            if (currentSelectDebugType == ItemDebugType.ItemPoolEditor)
            {
                List<ItemPoolData> list = null;
                switch (currentSelectPoolType)
                {
                    case ItemPoolType.Lv1:
                        list = _itemPool.Lv1;
                        break;
                    case ItemPoolType.Lv2:
                        list = _itemPool.Lv2;
                        break;
                    case ItemPoolType.Lv3:
                        list = _itemPool.Lv3;
                        break;
                    case ItemPoolType.Shop:
                        list = _itemPool.Shop;
                        break;
                }
                
                if (list != null)
                {
                    var poolData = IsContains(list, itemObj);
                    if (poolData != null)
                        list.Remove(poolData);
                    else
                        list.Add(new ItemPoolData()
                        {
                            Item = itemObj,
                            ProbabilityWeight = 0
                        });
                }
                EditorUtility.SetDirty(_itemPool);
            }
        }
        private void ItemPoolOnItemMouseR(ItemScriptableObject itemObj)
        {
            ItemEventContextMenu(itemObj).ShowAsContext();
        }

        private GenericMenu ItemEventContextMenu(ItemScriptableObject itemObj)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Test"),false, () =>
            {
                Debug.LogError(itemObj.Name);
            });
            
            return menu;
        }
        ItemPoolData IsContains(List<ItemPoolData> list,ItemScriptableObject item)
        {
            foreach (var poolData in list)
                if (poolData.Item == item)
                    return poolData;

            return null;
        }
        
    }
    
    
}