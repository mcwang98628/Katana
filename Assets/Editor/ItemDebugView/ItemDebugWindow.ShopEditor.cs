

using UnityEditor;
using UnityEngine;

namespace ItemDebugView
{
    public partial class ItemDebugWindow
    {
        private void DrawShopItemList()
        {
            Rect listZone = _itemListZoneRect;

            float xCountValue = listZone.width / (_itemWidth + _itemXInterval);
            //横行数量
            int xCount = Mathf.FloorToInt(xCountValue);
            if (xCount <= 0)
                return;
            
            Rect clipRect = listZone;
            clipRect.yMax = _fullWindowRect.yMax - 25;
            float xOffset = (listZone.width - (xCount*_itemWidth + (xCount-1) * _itemXInterval))/2f;
            int yCount = Mathf.CeilToInt(_itemPool.Shop.Count / (float)xCount);
            float yHeigth = yCount * _itemHeight + yCount * _itemYInterval;
            GUI.BeginClip(clipRect);
            for (int i = 0; i < _itemPool.Shop.Count; i++)
            {
                int xIndex = i % xCount;
                int yIndex = Mathf.FloorToInt(i / (float)xCount);
                Rect currentItemRect = listZone;
                currentItemRect.xMin = listZone.xMin + xOffset + (xIndex * _itemWidth + xIndex * _itemXInterval);
                currentItemRect.xMax = currentItemRect.xMin + _itemWidth;
                currentItemRect.yMin = (yIndex * _itemHeight + yIndex * _itemYInterval) - yHeigth*_itemListSliderValue;
                currentItemRect.yMax = currentItemRect.yMin + _itemHeight;
                ItemPoolData poolData = _itemPool.Shop[i];
                DrawShopItem(currentItemRect,poolData);
            }
            GUI.EndClip(); 
        }

        void DrawShopItem(Rect rect,ItemPoolData poolData)
        {
            GUI.Box(rect,new GUIContent(poolData.Item.Icon.texture),GUI.skin.button);
            
            Rect newRect = rect;
            newRect.yMax += 15;
            newRect.yMin = newRect.yMax - 15;
            EditorGUI.BeginChangeCheck();
            poolData.Price = EditorGUI.IntField(newRect, poolData.Price);
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(_itemPool);
        }
        
    }
    
    
    
}