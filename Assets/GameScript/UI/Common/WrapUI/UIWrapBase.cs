using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 重复利用Item显示内容 不要直接添加脚本，通过构造UIWrapComponent来使用
/// </summary>
public class UIWrapBase : MonoBehaviour
{
    /// <summary>
    /// 显示数据
    /// </summary>
    public List<BasePoolDT<long>> mData
    {
        private set;
        get;
    }
    public List<NBaseSCDT> mNBaseDT
    {
        private set;
        get;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void f_InitData(int itemSize, int itemNum, int itemExtraSize, int rowColNum, GameObject itemPrefab, List<BasePoolDT<long>> data, ccCallBack_WrapItemUpdate updateByInfo, ccCallBack_WrapItemClick clickHandle, ccUIEventListener.VoidDelegateV2 ccclickHandle = null)
    {
        if (!CacheScrollView())
        {
            Debug.LogError("UIWrapBase Exist Error!");
            return;
        }
        this.itemSize = itemSize;
        this.itemNum = itemNum;
        this.itemExtraSize = itemExtraSize;
        mData = data;
        mNBaseDT = null;
        _updateByInfo = updateByInfo;
        _itemClickHandle = clickHandle;
        MChildren.Clear();
        mTrans = transform;
        //初始化Item
        for (int i = 0; i < itemNum * rowColNum; ++i)
        {
            GameObject go = NGUITools.AddChild(mTrans.gameObject, itemPrefab);
            go.name = i.ToString();
            NGUITools.MarkParentAsChanged(go);
            if (hideInactive && !go.gameObject.activeInHierarchy) continue;
            if (clickHandle != null)
                ccUIEventListener.Get(go).f_RegClick(GOClickHandle, null, null);
            //if (ccclickHandle != null)
            //    ccUIEventListener.Get(go).onClickV2= ccclickHandle;
            MChildren.Add(go.transform);
        }
        // Sort the list of children so that they are in order
        MChildren.Sort(UIGrid.SortByName);
        ResetChildPositions();
        WrapContent();
        if (mScroll != null)
        {
            mScroll.GetComponent<UIPanel>().onClipMove = OnMove;
            //mScroll.RestrictWithinBounds(true);
        }
        mFirstTime = false;
    }

    public void f_InitData(int itemSize, int itemNum, int itemExtraSize, int rowColNum, GameObject itemPrefab, List<NBaseSCDT> data, ccCallBack_WrapItemNBaseSCDTUpdate updateByInfo, ccCallBack_WrapItemNBaseSCDTClick clickHandle)
    {
        if (!CacheScrollView())
        {
            Debug.LogError("UIWrapBase Exist Error!");
            return;
        }
        this.itemSize = itemSize;
        this.itemNum = itemNum;
        this.itemExtraSize = itemExtraSize;
        mData = null;
        mNBaseDT = data;
        _NBaseSCDTInfo = updateByInfo;
        _NBaseSCDTClick = clickHandle;
        MChildren.Clear();
        mTrans = transform;
        //初始化Item
        for (int i = 0; i < itemNum * rowColNum; ++i)
        {
            GameObject go = NGUITools.AddChild(mTrans.gameObject, itemPrefab);
            go.name = i.ToString();
            NGUITools.MarkParentAsChanged(go);
            if (hideInactive && !go.gameObject.activeInHierarchy) continue;
            if (clickHandle != null)
                ccUIEventListener.Get(go).f_RegClick(GOClickHandle, null, null);
            MChildren.Add(go.transform);
        }
        // Sort the list of children so that they are in order
        MChildren.Sort(UIGrid.SortByName);
        ResetChildPositions();
        WrapContent();
        if (mScroll != null)
        {
            mScroll.GetComponent<UIPanel>().onClipMove = OnMove;
            //mScroll.RestrictWithinBounds(true);
        }
        mFirstTime = false;
    }
    /// <summary>
    /// 水平方向？itemSize = wide ：itemSize = high;
    /// </summary>
    public int itemSize = 100;

    /// <summary>
    /// 水平时：一列要显示多少个   垂直时：一行显示多少个   一般为：1
    /// </summary>
    public int itemNum = 1;

    /// <summary>
    /// 只有需要显示多行或者多列时 用到
    /// horizontal？itemExtraSize = high ：itemExtraSize = wide;
    /// </summary>
    public int itemExtraSize = 100;

    public int minIndex
    {
        get
        {
            return 0;
        }
    }
    public int maxIndex
    {
        get
        {
            if (mData != null)
                return mData.Count - 1;
            else if (mNBaseDT != null)
                return mNBaseDT.Count - 1;
            else
                return 0;
        }
    }

    public List<Transform> MChildren
    {
        get
        {
            return mChildren;
        }

        set
        {
            mChildren = value;
        }
    }


    /// <summary>
    /// Whether hidden game objects will be ignored for the purpose of calculating bounds.
    /// </summary>

    public bool hideInactive = false;

    protected Transform mTrans;
    protected UIPanel mPanel;
    protected UIScrollView mScroll;
    protected bool mHorizontal = false;
    /// <summary>
    /// 是否相反 默认为false 左上角为起点。 true 右下角为起点
    /// </summary>
    protected bool mOpposite = false;
    protected bool mFirstTime = true;
    private List<Transform> mChildren = new List<Transform>();

    /// <summary>
    /// Callback triggered by the UIPanel when its clipping region moves (for example when it's being scrolled).
    /// </summary>
    protected virtual void OnMove(UIPanel panel) { WrapContent(); }

    /// <summary>
	/// Cache the scroll view and return 'false' if the scroll view is not found.
	/// </summary>
    protected bool CacheScrollView()
    {
        mTrans = transform;
        mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
        mScroll = mPanel.GetComponent<UIScrollView>();
        if (mScroll == null) return false;
        if (mScroll.movement == UIScrollView.Movement.Horizontal) mHorizontal = true;
        else if (mScroll.movement == UIScrollView.Movement.Vertical) mHorizontal = false;
        else return false;
        if (mScroll.contentPivot == UIWidget.Pivot.TopLeft) mOpposite = false;
        else if (mScroll.contentPivot == UIWidget.Pivot.BottomRight) mOpposite = true;
        return true;
    }

    /// <summary>
    /// 重置坐标点
    /// </summary>
    protected virtual void ResetChildPositions()
    {
        for (int i = 0, imax = MChildren.Count; i < imax; ++i)
        {
            Transform t = MChildren[i];
            int idx = i / itemNum;
            int extraIdx = i % itemNum;
            if (!mOpposite)
                t.localPosition = mHorizontal ? new Vector3(idx * itemSize, -extraIdx * itemExtraSize, 0f) : new Vector3(extraIdx * itemExtraSize, -idx * itemSize, 0f);
            else
                t.localPosition = mHorizontal ? new Vector3(-idx * itemSize, extraIdx * itemExtraSize, 0f) : new Vector3(-extraIdx * itemExtraSize, idx * itemSize, 0f);
            UpdateItem(t, i);
        }
    }

    /// <summary>
    /// Wrap all content, repositioning all children as needed.
    /// </summary>
    public virtual void WrapContent()
    {
        float extents = itemSize * MChildren.Count * 0.5f / itemNum;
        Vector3[] corners = mPanel.worldCorners;

        for (int i = 0; i < 4; ++i)
        {
            Vector3 v = corners[i];
            v = mTrans.InverseTransformPoint(v);
            corners[i] = v;
        }

        Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
        bool allWithinRange = true;
        float ext2 = extents * 2f;

        if (mHorizontal)
        {
            float min = corners[0].x - itemSize;
            float max = corners[2].x + itemSize;

            for (int i = 0, imax = MChildren.Count; i < imax; ++i)
            {
                Transform t = MChildren[i];
                float distance = t.localPosition.x - center.x;

                if (distance < -extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.x += ext2;
                    distance = pos.x - center.x;
                    int realIndex = GetRealIndex(pos);

                    if ((minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);
                    }
                    else allWithinRange = false;
                }
                else if (distance > extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.x -= ext2;
                    distance = pos.x - center.x;
                    int realIndex = GetRealIndex(pos);

                    if ((minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);
                    }
                    else allWithinRange = false;
                }
                else if (mFirstTime) UpdateItem(t, i);
            }
        }
        else
        {
            float min = corners[0].y - itemSize;
            float max = corners[2].y + itemSize;

            for (int i = 0, imax = MChildren.Count; i < imax; ++i)
            {
                Transform t = MChildren[i];
                float distance = t.localPosition.y - center.y;

                if (distance < -extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.y += ext2;
                    distance = pos.y - center.y;
                    int realIndex = GetRealIndex(pos);

                    if ((minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);
                    }
                    else allWithinRange = false;
                }
                else if (distance > extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.y -= ext2;
                    distance = pos.y - center.y;
                    int realIndex = GetRealIndex(pos);

                    if ((minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);
                    }
                    else allWithinRange = false;
                }
                else if (mFirstTime) UpdateItem(t, i);
            }
        }
        mScroll.restrictWithinPanel = !allWithinRange;
        //mScroll.InvalidateBounds();
    }

    private ccCallBack_WrapItemUpdate _updateByInfo;
    private ccCallBack_WrapItemClick _itemClickHandle;
    private ccCallBack_WrapItemNBaseSCDTUpdate _NBaseSCDTInfo;
    private ccCallBack_WrapItemNBaseSCDTClick _NBaseSCDTClick;

    /// <summary>
    /// 点击Item处理
    /// </summary>
    /// <param name="go"></param>
    public void GOClickHandle(GameObject go, object obj1, object obj2)
    {
        int realIdx = GetRealIndex(go.transform.localPosition);
        if (realIdx >= 0 && realIdx <= maxIndex && mData != null)
        {
            if (_itemClickHandle != null)
            {
                _itemClickHandle(go.transform, mData[realIdx]);
            }
        }
        //Debug.Log(string.Format("item-Name:{0} realIdx{1}", go.name, realIdx));
    }

    /// <summary>
    /// 更新Item
    /// </summary>
    private void UpdateItem(Transform item, int index)
    {
        int realIdx = GetRealIndex(item.localPosition);
        if (realIdx >= 0 && realIdx <= maxIndex && (mData != null || mNBaseDT != null))
        {
            if (!item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(true);
            }
            if (_updateByInfo != null)
            {
                _updateByInfo(item, mData[realIdx]);
            }
            else if (_NBaseSCDTInfo != null)
            {
                _NBaseSCDTInfo(item, mNBaseDT[realIdx]);
            }
        }
        else
        {
            if (item.gameObject.activeSelf)
                item.gameObject.SetActive(false);
            /*数据为空不给回调 直接设为不激活
            if (_updateByInfo != null)
            {
                _updateByInfo(item, null);
                
            }*/
        }
        //Debug.Log(string.Format("item-Name:{0} idx:{1} realIdx{2}", item.name, index, realIdx));
    }

    private int GetRealIndex(Vector3 pos)
    {
        int index = 0;
        int extraIndex = 0;
        if (!mOpposite)
        {
            index = (mScroll.movement == UIScrollView.Movement.Vertical) ?
             Mathf.RoundToInt(-pos.y / itemSize) :
             Mathf.RoundToInt(pos.x / itemSize);
            extraIndex = (mScroll.movement == UIScrollView.Movement.Vertical) ?
                Mathf.RoundToInt(pos.x / itemExtraSize) :
                Mathf.RoundToInt(-pos.y / itemExtraSize);
        }
        else
        {
            index = (mScroll.movement == UIScrollView.Movement.Vertical) ?
             Mathf.RoundToInt(pos.y / itemSize) :
             Mathf.RoundToInt(-pos.x / itemSize);
            extraIndex = (mScroll.movement == UIScrollView.Movement.Vertical) ?
                Mathf.RoundToInt(-pos.x / itemExtraSize) :
                Mathf.RoundToInt(pos.y / itemExtraSize);
        }

        int realIdx = itemNum * index + extraIndex;
        return realIdx;
    }

    private void SetRealIndexChildPos(int realIdx)
    {
        //realIdx 小于0不做处理
        if (realIdx < 0)
            return;
        int childIdx = realIdx % MChildren.Count;
        int idx = realIdx / itemNum;
        int extraIdx = realIdx % itemNum;
        if (!mOpposite)
            MChildren[childIdx].localPosition = mHorizontal ? new Vector3(idx * itemSize, -extraIdx * itemExtraSize, 0f) : new Vector3(extraIdx * itemExtraSize, -idx * itemSize, 0f);
        else
            MChildren[childIdx].localPosition = mHorizontal ? new Vector3(-idx * itemSize, extraIdx * itemExtraSize, 0f) : new Vector3(-extraIdx * itemExtraSize, idx * itemSize, 0f);
        UpdateItem(MChildren[childIdx], realIdx);
    }

    /// <summary>
    /// 刷新界面
    /// </summary>
    public void f_UpdateView()
    {
        for (int i = 0, imax = MChildren.Count; i < imax; ++i)
        {
            Transform t = MChildren[i];
            UpdateItem(t, i);
        }
        if (mScroll != null)
        {
            mScroll.Scroll(0.001f);
        }
    }

    /// <summary>
    /// 定位到对应的数据Idx
    /// </summary>
    /// <param name="realIdx">数据下标</param>
    /// <param name="showNumPrePage">一页显示多少个</param>
    public void f_ViewGotoRealIdx(int realIdx, int showNumPrePage)
    {
        int beforeNum = showNumPrePage - itemNum + (realIdx % itemNum);
        //int beforeNum = 4;//realIdx % itemNum;

        for (int i = beforeNum; i > 0; i--)
        {
            SetRealIndexChildPos(realIdx - i);
        }
        for (int i = 0; i < MChildren.Count - beforeNum; i++)
        {
            SetRealIndexChildPos(realIdx + i);
        }
        if (mScroll != null)
        {
            mScroll.RestrictWithinBounds(true);
        }
    }

    //重置界面回最高点
    public void f_ResetView()
    {
        if (!CacheScrollView()) return;
        ResetChildPositions();
        if (mScroll != null)
        {
            mScroll.ResetPosition();
        }
    }

    //更新数据List的引用
    public void f_UpdateList(List<BasePoolDT<long>> newList)
    {
        mData = newList;
    }

    //更新数据List的引用
    public void f_UpdateList(List<NBaseSCDT> newList)
    {
        mNBaseDT = newList;
    }
    /// <summary>
    /// 清除子物体
    /// </summary>
    public void f_ClearAllChild() {
        if (mChildren!=null) {
            for (int i = 0; i < mChildren.Count; i++)
            {
                Destroy(mChildren[i].gameObject);
            }
            mChildren.Clear();
        }
    }
}
