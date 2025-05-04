using ccU3DEngine;
using UnityEngine;
using System.Collections;

/// <summary>
/// 弹窗
/// </summary>
public class PopupMenuPage : UIFramwork
{
    private UISprite _title;
    private UILabel _content;
    private GameObject _btn1;
    private UILabel _btn1Label;
    private GameObject _btn2;
    private UILabel _btn2Label;
    private UIGrid _btnGrid;
    private GameObject _maskClose;
    private UIToggle _Toggle;
    //数据
    private PopupMenuParams mData;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _title = f_GetObject("Title").GetComponent<UISprite>();
        _content = f_GetObject("Content").GetComponent<UILabel>();
        _btn1 = f_GetObject("Btn1");
        _btn1Label = f_GetObject("Btn1Label").GetComponent<UILabel>();
        _btn2 = f_GetObject("Btn2");
        _btn2Label = f_GetObject("Btn2Label").GetComponent<UILabel>();
        _btnGrid = f_GetObject("BtnGrid").GetComponent<UIGrid>();
        _maskClose = f_GetObject("MaskClose");
        _Toggle = f_GetObject("Toggle").GetComponent<UIToggle>();
        f_RegClickEvent(_btn1, f_Btn1ClickHandle);
        f_RegClickEvent(_btn2, f_Btn2ClickHandle);
        f_RegClickEvent(_maskClose, f_MaskClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null && !(e is PopupMenuParams))
        {
MessageBox.ASSERT("PopupMenuPage parameter must be passed to PopupMenuParams");
        }
        Data_Pool.m_GuidancePool.m_NowOpenUIName = UINameConst.PopupMenuPage;
        //if (Data_Pool.m_GuidancePool.IsEnter)
        //{

        //    Data_Pool.m_GuidancePool.f_SetCurClickButtonNull();
        //}
        mData = (PopupMenuParams)e;
        _title.gameObject.SetActive(!string.IsNullOrEmpty(mData.mTitle));
        //_title.spriteName = mData.mTitle;
        _content.text = mData.mContent;
        _btn1.SetActive(!string.IsNullOrEmpty(mData.mBtn1Label));
        _btn2.SetActive(!string.IsNullOrEmpty(mData.mBtn2Label));
        _btn1Label.text = mData.mBtn1Label;
        _btn2Label.text = mData.mBtn2Label;
        _btnGrid.repositionNow = true;

        _Toggle.gameObject.SetActive(mData.mSaveParam!= PopupMenuParams.PopSaveParam.None);


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
        _SetSaveParam();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_CLOSE);
    }

    private void f_Btn2ClickHandle(GameObject go, object value1, object value2)
    {
        if (mData.mBtn2ClickHandle != null)
        {
            mData.mBtn2ClickHandle(mData.mParam2);
        }
        //_SetSaveParam();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_CLOSE);
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
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_CLOSE);
        }
    }

    private void _SetSaveParam() {
        switch (mData.mSaveParam)
        {
            case PopupMenuParams.PopSaveParam.None:
                break;
            case PopupMenuParams.PopSaveParam.Sky:
                LocalDataManager.f_SetLocalData<bool>(LocalDataType.AutoSky,_Toggle.value);
                break;
            case PopupMenuParams.PopSaveParam.SevenStarBless:
                LocalDataManager.f_SetLocalData<bool>(LocalDataType.SevenStarBlessUp, _Toggle.value);
                break;
            default:
                break;
        }
    }
}


public class PopupMenuParams
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

public PopupMenuParams(string title, string content, string btn1Label = "Xác nhận", ccCallback btn1ClikHandle = null,
                string btn2Label = "", ccCallback btn2ClickHandle = null, object param1 = null, object param2 = null,PopSaveParam Save= PopSaveParam.None)
    {
        mTitle = title;
        mContent = content;
        mBtn1Label = btn1Label;
        mBtn1ClickHandle = btn1ClikHandle;
        mBtn2Label = btn2Label;
        mBtn2ClickHandle = btn2ClickHandle;
        mParam1 = param1;
        mParam2 = param2;
        mSaveParam = Save;
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

    public PopSaveParam mSaveParam {
        get;
        private set;
    }

    public enum PopSaveParam {
        None,
        Sky,
        SevenStarBless,
    }
}
