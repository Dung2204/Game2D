using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

/// <summary>
/// 顶级弹窗
/// </summary>
public class TopPopupMenuPage : UIFramwork
{
    private UILabel _title;
    private UILabel _content;
    private GameObject _btn1;
    private UILabel _btn1Label;

    //数据
    private TopPopupMenuParams mData;

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
        f_RegClickEvent(_btn1, f_Btn1ClickHandle);

    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null && !(e is TopPopupMenuParams))
        {
MessageBox.ASSERT("TopPopupMenuPage must pass a parameter TopPopupMenuParams");
        }
        Data_Pool.m_GuidancePool.m_NowOpenUIName = UINameConst.TopMoneyPage;
        //if (Data_Pool.m_GuidancePool.IsEnter)
        //{
        //    Data_Pool.m_GuidancePool.f_SetCurClickButtonNull();
        //}
        mData = (TopPopupMenuParams)e;
        _title.gameObject.SetActive(!string.IsNullOrEmpty(mData.mTitle));
        _title.text = mData.mTitle;
        _content.text = mData.mContent;
        _btn1Label.text = mData.mBtn1Label;
        if (mData.mForceQuitInterval > -1) {
            CurrentCountDown = mData.mForceQuitInterval;
            CurrentCountDownId = ccTimeEvent.GetInstance().f_RegEvent(1, true, mData.mBtn1Label, OnCountDown);
        }
    }
    float CurrentCountDown;
    int CurrentCountDownId;
    private void OnCountDown(object data)
    {
        _btn1Label.text = data.ToString() + "("+(int)CurrentCountDown+")";
        CurrentCountDown--;
        if (CurrentCountDown <= 0) {
            ccTimeEvent.GetInstance().f_UnRegEvent(CurrentCountDownId);
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_Btn1ClickHandle(GameObject go, object value1, object value2)
    {
        if (mData.mBtn1ClickHandle != null)
        {
            mData.mBtn1ClickHandle(go);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_CLOSE);
    }
}

public class TopPopupMenuParams
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

    public float mForceQuitInterval
    {
        get;
        private set;
    }

public TopPopupMenuParams(string title, string content, string btn1Label = "Confirm", ccCallback btn1ClikHandle = null,float forceQuitInterval = -1)
    {
        mTitle = title;
        mContent = content;
        mBtn1Label = btn1Label;
        mBtn1ClickHandle = btn1ClikHandle;
        mForceQuitInterval = forceQuitInterval;
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
    /// 按钮1点击回调
    /// </summary>
    public ccCallback mBtn1ClickHandle
    {
        get;
        private set;
    }
}
