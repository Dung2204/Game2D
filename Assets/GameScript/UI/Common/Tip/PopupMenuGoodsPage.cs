using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class PopupMenuGoodsPage : UIFramwork
{

    private UILabel _title;
    private UILabel _content;
    private GameObject _btn1;
    private UILabel _btn1Label;
    private GameObject _btn2;
    private UILabel _btn2Label;
    private UILabel _content2;
    private UIGrid _btnGrid;
    private GameObject _maskClose;
    private UIToggle _Toggle;
    private ResourceCommonItem _Item;
    //数据
    private PopupMenuGoodsParams mData;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _title = f_GetObject("Title").GetComponent<UILabel>();
        _content = f_GetObject("Content").GetComponent<UILabel>();
        _btn1 = f_GetObject("Btn1");
        _btn1Label = f_GetObject("Btn1Label").GetComponent<UILabel>();
        _btn2 = f_GetObject("Btn2");
        _btn2Label = f_GetObject("Btn2Label").GetComponent<UILabel>();
        _btnGrid = f_GetObject("BtnGrid").GetComponent<UIGrid>();
        _maskClose = f_GetObject("MaskClose");
        _Toggle = f_GetObject("Toggle").GetComponent<UIToggle>();
        _content2 = f_GetObject("Content2").GetComponent<UILabel>();
        _Item = f_GetObject("AwardTipShowItem").GetComponent<ResourceCommonItem>();

        f_RegClickEvent(_btn1, f_Btn1ClickHandle);
        f_RegClickEvent(_btn2, f_Btn2ClickHandle);
        f_RegClickEvent(_maskClose, f_MaskClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null && !(e is PopupMenuGoodsParams))
        {
MessageBox.ASSERT("PopupMenuPage must pass PopupMenuParams");
        }
        Data_Pool.m_GuidancePool.m_NowOpenUIName = UINameConst.PopupMenuPage;
        //if (Data_Pool.m_GuidancePool.IsEnter)
        //{

        //    Data_Pool.m_GuidancePool.f_SetCurClickButtonNull();
        //}
        mData = (PopupMenuGoodsParams)e;
        _title.gameObject.SetActive(!string.IsNullOrEmpty(mData.mTitle));
        _title.text = mData.mTitle;
        _content.text = mData.mContent;
        _btn1.SetActive(!string.IsNullOrEmpty(mData.mBtn1Label));
        _btn2.SetActive(!string.IsNullOrEmpty(mData.mBtn2Label));
        _btn1Label.text = mData.mBtn1Label;
        _btn2Label.text = mData.mBtn2Label;
        _btnGrid.repositionNow = true;

        _Toggle.gameObject.SetActive(false);
        _content2.text = mData.mContent2;
        _Item.f_UpdateByInfo(mData.mResourceData);
    }


    private void TTT(object Obj)
    {
        Data_Pool.m_GuidancePool.f_SetCurClickButton(Data_Pool.m_GuidancePool.GuidanceBtnName, Data_Pool.m_GuidancePool.m_GuidanceCallback);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        if (Data_Pool.m_GuidancePool.IsEnter)
        {
            ccTimeEvent.GetInstance().f_Resume();
            ccTimeEvent.GetInstance().f_RegEvent(0.1f, false, null, TTT);
        }
    }

    private void f_Btn1ClickHandle(GameObject go, object value1, object value2)
    {
        if (mData.mBtn1ClickHandle != null)
        {
            mData.mBtn1ClickHandle(mData.mParam1);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuGoodsPage, UIMessageDef.UI_CLOSE);
    }

    private void f_Btn2ClickHandle(GameObject go, object value1, object value2)
    {
        if (mData.mBtn2ClickHandle != null)
        {
            mData.mBtn2ClickHandle(mData.mParam2);
        }
        //_SetSaveParam();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuGoodsPage, UIMessageDef.UI_CLOSE);
    }

    //如果只有一个按钮，点击背景会关闭 并且触发按钮1的回调
    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        if (string.IsNullOrEmpty(mData.mBtn2Label))
        {
            if (mData.mBtn1ClickHandle != null)
            {
                mData.mBtn1ClickHandle(go);
            }
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuGoodsPage, UIMessageDef.UI_CLOSE);
        }
    }
}

public class PopupMenuGoodsParams
{
    /// <summary>
    /// 标头为空时，界面的标头不会显示
    /// </summary>
    public string mTitle
    {
        get;
        private set;
    }

    /// <summary>
    /// 弹窗文字内容
    /// </summary>
    public string mContent
    {
        get;
        private set;
    }

    public string mContent2
    {
        get;
        private set;
    }

public PopupMenuGoodsParams(string title, string content, string content2, string btn1Label = "Confirm", ccCallback btn1ClikHandle = null,
                string btn2Label = "", ccCallback btn2ClickHandle = null, object param1 = null, object param2 = null, ResourceCommonDT data = null)
    {
        mTitle = title;
        mContent = content;
        mContent2 = content2;
        mBtn1Label = btn1Label;
        mBtn1ClickHandle = btn1ClikHandle;
        mBtn2Label = btn2Label;
        mBtn2ClickHandle = btn2ClickHandle;
        mParam1 = param1;
        mParam2 = param2;
        mResourceData = data;
    }

    /// <summary>
    /// 按钮1描述 为空该按钮不显示
    /// </summary>
    public string mBtn1Label
    {
        get;
        private set;
    }

    /// <summary>
    /// 按钮2描述 为空该按钮不显示
    /// </summary>
    public string mBtn2Label
    {
        get;
        private set;
    }

    /// <summary>
    /// 按钮1点击回调
    /// </summary>
    public ccCallback mBtn1ClickHandle
    {
        get;
        private set;
    }


    /// <summary>
    /// 按钮2点击回调
    /// </summary>
    public ccCallback mBtn2ClickHandle
    {
        get;
        private set;
    }

    public object mParam1
    {
        get;
        private set;
    }

    public object mParam2
    {
        get;
        private set;
    }

    public ResourceCommonDT mResourceData
    {
        get;
        private set;
    }
}
