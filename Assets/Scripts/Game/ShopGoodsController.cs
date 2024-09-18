// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Serialization;
// using Random = UnityEngine.Random;
//
// public class ShopGoodsController : MonoBehaviour
// {
//     public InterObj_Goods GoodsPrefab;
//     public ShopPoolObject GoodsPool;
//     public int GoodsCount;//创建几个Item商品;
//
//     //只购买一个商品
//     public bool IsOnlyBuyOneGoods;
//     //商品点
//     public Transform GoodsPoint;
//     
//     List<InterObj_Goods> Goods = new List<InterObj_Goods>();
//
//     private void Awake()
//     {
//         Init();
//     }
//
//     private void Init()
//     {
//         if (GoodsCount > GoodsPool.ItemList.Count)
//         {
//             GoodsCount = GoodsPool.ItemList.Count;
//         }
//         
//         List<ShopItem> ItemList = new List<ShopItem>();
//         List<int> indexs = new List<int>();
//         for (int i = 0; i < GoodsPool.ItemList.Count; i++)
//         {
//             indexs.Add(i);
//         }
//         for (int i = 0; i < GoodsCount; i++)
//         {
//             var index = indexs[Random.Range(0, indexs.Count)];
//             ItemList.Add(GoodsPool.ItemList[index]);
//             indexs.Remove(index);
//         }
//
//         for (int i = 0; i < ItemList.Count; i++)
//         {
//             InterObj_Goods goods = GameObject.Instantiate(GoodsPrefab, transform);
//             if (IsOnlyBuyOneGoods)
//             {
//                 goods.Init(ItemList[i],CloseGoods);
//             }
//             else
//             {
//                 goods.Init(ItemList[i],null);
//             }
//
//             goods.transform.position = GoodsPoint.GetChild(i).position;
//             Goods.Add(goods);
//         }
//     }
//
//     void CloseGoods()
//     {
//         foreach (InterObj_Goods goods in Goods)
//         {
//             if (goods != null)
//             {
//                 goods.DisableSphere();
//             }
//         }
//         Goods.Clear();
//     }
// }
