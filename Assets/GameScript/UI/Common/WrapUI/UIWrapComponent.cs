using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class UIWrapComponent
{
    private UIWrapBase _uiwrapBase;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="宽度">水平？itemSize = wide ：itemSize = high;</param>
    /// <param name="itemNum">水平时：一列要显示多少个   垂直时：一行显示多少个   一般为：1</param>
    /// <param name="itemExtraSize">和itemSize相反，只有需要显示多行或者多列时 用到</param>
    /// <param name="rowColNum">行或者列数量</param>
    /// <param name="gridObject">item的父对象</param>
    /// <param name="itemPrefab">复用对象Item</param>
    /// <param name="sourceData">显示的对象数据</param>
    /// <param name="updateByInfo">更新回调</param>
    /// <param name="itemClick">点击回调</param>
    public UIWrapComponent(int itemSize, int itemNum, int itemExtraSize, int rowColNum, GameObject gridObject, GameObject itemPrefab, List<ccU3DEngine.BasePoolDT<long>> sourceData, ccCallBack_WrapItemUpdate updateByInfo, ccCallBack_WrapItemClick itemClick)
    {
        _uiwrapBase = gridObject.AddComponent<UIWrapBase>();
        _uiwrapBase.f_InitData(itemSize, itemNum, itemExtraSize, rowColNum, itemPrefab, sourceData, updateByInfo, itemClick);
    }
    /// <summary>
    /// NBaseSCDT类型
    /// </summary>
    /// <param name="itemSize"></param>
    /// <param name="itemNum"></param>
    /// <param name="itemExtraSize"></param>
    /// <param name="rowColNum"></param>
    /// <param name="gridObject"></param>
    /// <param name="itemPrefab"></param>
    /// <param name="sourceData"></param>
    /// <param name="updateByInfo"></param>
    /// <param name="itemClick"></param>
    public UIWrapComponent(int itemSize, int itemNum, int itemExtraSize, int rowColNum, GameObject gridObject, GameObject itemPrefab, List<NBaseSCDT> sourceData, ccCallBack_WrapItemNBaseSCDTUpdate updateByInfo, ccCallBack_WrapItemNBaseSCDTClick itemClick, int i = 0)
    {
        _uiwrapBase = gridObject.AddComponent<UIWrapBase>();
        _uiwrapBase.f_InitData(itemSize, itemNum, itemExtraSize, rowColNum, itemPrefab, sourceData, updateByInfo, itemClick);
    }

    public void f_UpdateView()
    {
        if (_uiwrapBase)
            _uiwrapBase.f_UpdateView();
    }

    public void f_ResetView()
    {
        if (_uiwrapBase)
            _uiwrapBase.f_ResetView();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="realIdx">数据下标</param>
    /// <param name="showNumPrePage">一页显示多少个</param>
    public void f_ViewGotoRealIdx(int realIdx, int showNumPrePage)
    {
        if (_uiwrapBase)
        {
            _uiwrapBase.f_ResetView();
            _uiwrapBase.f_ViewGotoRealIdx(realIdx, showNumPrePage);
        }

    }

    //更新List的引用
    public void f_UpdateList(List<BasePoolDT<long>> newList)
    {
        if (_uiwrapBase)
            _uiwrapBase.f_UpdateList(newList);
    }

    public void f_UpdateList(List<NBaseSCDT> newList)
    {
        if (_uiwrapBase)
            _uiwrapBase.f_UpdateList(newList);
    }

    public void f_DeleList(Transform tran)
    {
        if (_uiwrapBase)
            _uiwrapBase.MChildren.Remove(tran);
    }

    public void f_SetHide(bool hide)
    {
        _uiwrapBase.hideInactive = hide;
    }

    public void f_DeleAllChild()
    {
        if (_uiwrapBase)
        {
            _uiwrapBase.f_ClearAllChild();
        }
    }

    public void f_OnClickIndex(int index = 0)
    {
        if (_uiwrapBase && _uiwrapBase.MChildren.Count > 0 && _uiwrapBase.MChildren.Count >= index)
        {
            _uiwrapBase.GOClickHandle(_uiwrapBase.MChildren[index].gameObject, null , null);
        }

    }
}
