using System;
using System.Collections.Generic;
using System.Linq;
using System.CodeDom;

namespace UnityEngine.UI {

    public class RecycleScroller : CustomScroller {

        int _col;
        int _row;
        int _transCount;
        int _startIndex;
        int _priorIndex;

        private HashSet<int> _priorIndexSet = new HashSet<int>();
        private HashSet<int> _showIndexSet = new HashSet<int>();

        public new void Initialize(DataAdapterHandler handler,GameObject prefab,int itemCount) {
            base.Initialize(handler,prefab,itemCount);
            InitChildren();
            EnableScroller();
        }

        public override void Reload(int itemCount) {

            Clear();

            int offsetIndex = 0;

            if(ItemCount>_transCount&&itemCount>_transCount) {
                int maxIndex = itemCount-_transCount;

                if(_priorIndex>maxIndex)
                    offsetIndex=maxIndex;
                else
                    offsetIndex=_priorIndex;
            }

            ItemCount=itemCount;
            InitContainer();
            InitChildren(offsetIndex);
        }

        public override void Clear() {
            _priorIndexSet.Clear();
            _showIndexSet.Clear();
            base.Clear();
        }

        void InitChildren(int offsetIndex = 0) {

            if(Movement==ScrollerMovement.Vertical) {
                _col=ConstraintCount;
                _row=Mathf.CeilToInt((ScrollerRect.rect.height+Spacing.y)/ItemSize.y);
            } else {
                _row=ConstraintCount;
                _col=Mathf.CeilToInt((ScrollerRect.rect.width+Spacing.x)/ItemSize.x);
            }

            int offsetHor = Movement==ScrollerMovement.Horizontal ? 1 : 0;
            int offsetVer = Movement==ScrollerMovement.Vertical ? 1 : 0;

            _transCount=Math.Min((_col+offsetHor)*(_row+offsetVer),ItemCount);

            for(int i = 0;i<_transCount;i++) {
                int index = offsetIndex+i;
                if(index>=ItemCount)
                    break;

                ScrollerCell cell = GetCell(index);
                cell.GameObject.SetActive(true);

                _priorIndexSet.Add(index);
            }
        }

        protected override void InitChild(RectTransform rect,int index) {
            base.InitChild(rect,index);
            SetPositionByIndex(rect,index);
        }

        protected override void InitContainer() {
            float width, height;
            if(Movement==ScrollerMovement.Vertical) {
                width=_col*ItemSize.x-Spacing.x+Padding.left;
                height=Mathf.CeilToInt((ItemCount)/(float)_col)*ItemSize.y+Mathf.Abs(Padding.top);
            } else {
                width=Mathf.CeilToInt((ItemCount)/(float)_row)*ItemSize.x+Padding.left;
                height=_row*ItemSize.y-Spacing.y+Mathf.Abs(Padding.top);
            }
            SetContainerSize(new Vector2(width,height));
        }

        protected override void OnScroll() {
            if(_transCount==ItemCount)
                return;

            if(Movement==ScrollerMovement.Vertical)
                _startIndex=Mathf.Clamp(Mathf.FloorToInt(ContainerRect.anchoredPosition.y/ItemSize.y)*_col,0,ItemCount-_transCount);
            else {
                _startIndex=Mathf.Clamp(Mathf.FloorToInt(-ContainerRect.anchoredPosition.x/ItemSize.x)*_row,0,ItemCount-_transCount);
            }
            if(_startIndex!=_priorIndex) {
                SwopIndex(_startIndex);
                _priorIndex=_startIndex;
            }
            
        }

        void SwopIndex(int index) {
            _showIndexSet.Clear();
            for(int i = 0;i<_transCount;i++)
                _showIndexSet.Add(i+index);
            if(_showIndexSet.SetEquals(_priorIndexSet))
                return;
            var lhsIter = _showIndexSet.Except(_priorIndexSet).GetEnumerator();
            var rhsIter = _priorIndexSet.Except(_showIndexSet).GetEnumerator();
            while(lhsIter.MoveNext()&&rhsIter.MoveNext())
                ChangeToIndex(rhsIter.Current,lhsIter.Current);

            _priorIndexSet=new HashSet<int>(_showIndexSet);
        }

        void ChangeToIndex(int from,int to) {
            ScrollerCell cell = GetCellByIndex(from);
            OnDataAdapter(cell.GameObject,to);
            SetPositionByIndex(cell.RectTrans,to);
            ReplaceIndex(from,to);
        }

        void SetPositionByIndex(RectTransform rect,int index) {
            float posX = 0f, posY = 0f;
            if(Movement==ScrollerMovement.Vertical) {
                posX=index%_col*ItemSize.x+Padding.left;
                posY=Mathf.CeilToInt(index/_col-0.000001f)*ItemSize.y+Padding.top;
            } else {
                posX=Mathf.CeilToInt(index/_row-0.000001f)*ItemSize.x+Padding.left;
                posY=index%_row*ItemSize.y+Padding.top;
            }

            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,posX,CellSize[0]);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,posY,CellSize[1]);
        }
    }
}
