using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.UI {

    [RequireComponent(typeof(ScrollRect))]
    public abstract class CustomScroller : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

        [SerializeField]
        protected RectOffset Padding;//缩进
        [SerializeField]
        protected Vector2 CellSize;//元素大小
        [SerializeField]
        protected Vector2 Spacing;//间距
        [SerializeField]
        protected int ConstraintCount;//限制个数

        //单个元素总大小
        protected Vector2 ItemSize {
            get {
                return Spacing+CellSize;
            }
        }

        protected DataAdapterHandler OnDataAdapter;//回调
        protected ScrollRect Scroller; //滚动组件
        protected RectTransform ScrollerRect;//滚动
        protected RectTransform ContainerRect;//容器
        protected Object Prefab;
        protected int ItemCount;//元素数量

        protected bool IsPointerUp;

        protected Vector2 PrevPosition;//上次的位置?

        //滑动类型
        protected enum ScrollerMovement {
            Horizontal, Vertical
        }
        protected ScrollerMovement Movement;

        protected List<ScrollerCell> Cells = new List<ScrollerCell>();
        protected List<ScrollerCell> FreeCells = new List<ScrollerCell>();

        protected virtual void Initialize(DataAdapterHandler handler,GameObject prefab,int itemCount) {
            this.OnDataAdapter=handler;
            this.Prefab=prefab;
            this.ItemCount=itemCount;

            Clear();
            InitMisc();
        }

        protected virtual void EnableScroller() {
            InitContainer();
        }

        //赋值初始化
        protected virtual void InitMisc() {
            Scroller=GetComponent<ScrollRect>();
            ScrollerRect=GetComponent<RectTransform>();
            ContainerRect=Scroller.content;

            Movement=Scroller.vertical ? ScrollerMovement.Vertical : ScrollerMovement.Horizontal;
            Scroller.onValueChanged.AddListener(TryScroll);
        }

        protected virtual void InitChild(RectTransform rect,int index) {
            rect.anchorMax=Vector2.up;
            rect.anchorMin=Vector2.up;
            rect.pivot=new Vector2(0.5f,0.5f);

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,CellSize[0]);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,CellSize[1]);
        }

        protected virtual void InitContainer() {

        }

        protected void SetContainerSize(Vector2 size) {
            SetContainerSize(size[0],size[1]);
        }

        protected void SetContainerSize(float width = 0f,float height = 0f) {
            SetContainerSizeWithAxis(0,width);
            SetContainerSizeWithAxis(1,height);
        }

        protected void SetContainerSizeWithAxis(int axis,float v) {
            ContainerRect.SetSizeWithCurrentAnchors((RectTransform.Axis)axis,v);
        }

        protected ScrollerCell GetCell(int index) {
            ScrollerCell cell = null;
            if(FreeCells.Count>0) {
                cell=FreeCells[0];
                cell.Reset();
                cell.Index=index;
                FreeCells.RemoveAt(0);
            } else {
                var g = Instantiate(Prefab) as GameObject;
                g.transform.SetParent(ContainerRect,false);
                g.SetActive(true);
                cell=new ScrollerCell(index,g);
            }
            InitChild(cell.RectTrans,index);
            OnDataAdapter(cell.GameObject,index);
            Cells.Add(cell);
            return cell;
        }

        protected void ReplaceIndex(int oldIndex,int newIndex) {
            for(int i = 0;i<Cells.Count;i++)
                if(Cells[i].Index==oldIndex)
                    Cells[i].Index=newIndex;
        }

        protected ScrollerCell GetCellByIndex(int index) {
            for(int i = 0;i<Cells.Count;i++)
                if(Cells[i].Index==index)
                    return Cells[i];
            return null;
        }

        public virtual void Clear() {
            for(int i = 0;i<Cells.Count;i++) {
                ScrollerCell cell = Cells[i];
                cell.GameObject.SetActive(false);
                FreeCells.Add(cell);
            }
            Cells.Clear();
        }

        public virtual void Reload(int itemCount) {
            this.ItemCount=itemCount;
            Clear();
        }

        private void TryScroll(Vector2 v) {
            if(Cells.Count==0) {
                return;
            }
            OnScroll();
        }

        protected virtual void OnScroll() {
            if(ContainerRect.anchoredPosition!=PrevPosition) {
                PrevPosition=ContainerRect.anchoredPosition;
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData) {
            IsPointerUp=false;
        }

        public virtual void OnPointerUp(PointerEventData eventData) {
            IsPointerUp=true;
        }
    }

    public class ScrollerCell {
        public int Index;
        public RectTransform RectTrans;
        public GameObject GameObject;
        public ScrollerCell(int index,GameObject g) {
            this.Index=index;
            this.GameObject=g;
            this.RectTrans=g.GetComponent<RectTransform>();
        }
        public void Reset() {
            GameObject.SetActive(true);
        }
    }

    public delegate void DataAdapterHandler(GameObject g,int index);
}