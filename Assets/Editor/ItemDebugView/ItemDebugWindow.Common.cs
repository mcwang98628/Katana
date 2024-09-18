
using System;
using System.Collections.Generic;
using DG.DemiEditor;
using UnityEditor;
using UnityEngine;

namespace ItemDebugView
{
    public partial class ItemDebugWindow
    {
        private void DrawItemZone()
        {
            switch (currentSelectDebugType)
            {
                case ItemDebugType.All:
                    DrawItemList(_allItemObj.objs,_itemListZoneRect,SelectItem);
                    DrawItemTypeBtn();
                    DrawBottomBtn();
                    break;
                case ItemDebugType.ItemPoolEditor:
                    DrawItemPoolEditor();
                    DrawItemTypeBtn();
                    break;
                case ItemDebugType.ShopEditor:
                    DrawShopItemList();
                    break;
                case ItemDebugType.RuntimePlayer:
                    DrawRuntimePlayerItems();
                    DrawItemTypeBtn();
                    DrawBottomBtn();
                    break;
                
            }
        }
        
        private void DrawItemList(
            List<ItemScriptableObject> objList,
            Rect listZone,
            Action<ItemScriptableObject> mouseLBtn,
            Action<ItemScriptableObject> mouseRBtn = null)
        {
            List<ItemScriptableObject> objs = new List<ItemScriptableObject>();
            switch (currentSelectItemType)
            {
                case ItemType.All:
                    objs = objList;
                    break;
                default:
                    foreach (ItemScriptableObject itemScrObj in objList)
                        if (itemScrObj.ItemType == currentSelectItemType)
                            objs.Add(itemScrObj);
                    break;
            }

            if (objs.Count == 0)
            {
                this.ShowNotification(new GUIContent("空空如也"));
                return;
            }
            
            float xCountValue = listZone.width / (_itemWidth + _itemXInterval);
            //横行数量
            int xCount = Mathf.FloorToInt(xCountValue);
            if (xCount <= 0)
            {
                this.ShowNotification(new GUIContent("太窄了"));
                return;
            }

            Rect clipRect = listZone;
            clipRect.yMax = _fullWindowRect.yMax - 25;
            float xOffset = (listZone.width - (xCount*_itemWidth + (xCount-1) * _itemXInterval))/2f;
            int yCount = Mathf.CeilToInt(objs.Count / (float)xCount);
            float yHeigth = yCount * _itemHeight + yCount * _itemYInterval;
            GUI.BeginClip(clipRect);
            for (int i = 0; i < objs.Count; i++)
            {
                int xIndex = i % xCount;
                int yIndex = Mathf.FloorToInt(i / (float)xCount);
                Rect currentItemRect = listZone;
                currentItemRect.xMin = listZone.xMin + xOffset + (xIndex * _itemWidth + xIndex * _itemXInterval);
                currentItemRect.xMax = currentItemRect.xMin + _itemWidth;
                currentItemRect.yMin = (yIndex * _itemHeight + yIndex * _itemYInterval) - yHeigth*_itemListSliderValue;
                currentItemRect.yMax = currentItemRect.yMin + _itemHeight;
                ItemScriptableObject itemObj = objs[i];
                DrawItem(currentItemRect,itemObj,mouseLBtn,mouseRBtn);
            }
            GUI.EndClip();

            _itemListSliderRect = listZone;
            _itemListSliderRect.xMin = listZone.xMax;
            _itemListSliderRect.xMax = _itemListSliderRect.xMin + 20;
            _itemListSliderRect.yMin = listZone.yMin;
            _itemListSliderRect.yMax = listZone.yMax;
            _itemListSliderValue = GUI.VerticalSlider(_itemListSliderRect, _itemListSliderValue, 0f, 1f);
        }

        private void DrawItem(
            Rect rect,
            ItemScriptableObject itemObj,
            Action<ItemScriptableObject> mouseLBtn,
            Action<ItemScriptableObject> mouseRBtn = null)
        {
            if (itemObj == null)
                return;
            
            bool isSelect = CurrentSelectItem == itemObj;
            Color oldColor = GUI.color;
            
            var poolData = CurrentPoolIsContains(itemObj);
            if (poolData != null && currentSelectDebugType == ItemDebugType.ItemPoolEditor)
            {
                GUI.color = Color.magenta;
            }
            else
            {
                if (isSelect)
                    GUI.color = Color.green;
            }

            Texture2D t2D = null;
            if (itemObj.Icon != null)
                t2D = itemObj.Icon.texture;
            GUI.Box(rect,new GUIContent(t2D),GUI.skin.button);
            Event current = Event.current;
            if (current!=null)
            {
                if (current.type == EventType.MouseUp && rect.Contains(current.mousePosition))
                {
                    switch (current.button)
                    {
                        case 0:
                            mouseLBtn?.Invoke(itemObj);
                            break;
                        case 1:
                            mouseRBtn?.Invoke(itemObj);
                            break;
                    }
                }
            }
            
            
            
            if (poolData != null && currentSelectDebugType == ItemDebugType.ItemPoolEditor)
            {
                Rect newRect = rect;
                newRect.yMax += 10;
                newRect.yMin = newRect.yMax - 15;
                EditorGUI.BeginChangeCheck();
                poolData.ProbabilityWeight = EditorGUI.IntField(newRect, poolData.ProbabilityWeight);
                if (EditorGUI.EndChangeCheck())
                    EditorUtility.SetDirty(_itemPool);
            }
            
            GUI.color = oldColor;
            
        }
    }
}