using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class AwardGetSubPage : UIFramwork
{
    private UILabel mTitle;
    private UILabel mContent;
    private GameObject mLockBtn;
    private GameObject mGetBtn;
    private GameObject mAlreadyGetTip;
    private GameObject mCloseMask;

    /// <summary>
    /// 奖励显示
    /// </summary>
    private UIGrid awardGrid;
    private GameObject awardItem;
    private ResourceCommonItemComponent _awardShowComponent;
    private ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(awardGrid, awardItem);
            return _awardShowComponent;
        }
    }

    private AwardGetSubPageParam mParam;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mTitle = f_GetObject("Title").GetComponent<UILabel>();
        mContent = f_GetObject("Content").GetComponent<UILabel>();
        mLockBtn = f_GetObject("LockBtn");
        mAlreadyGetTip = f_GetObject("GetTip");
        mCloseMask = f_GetObject("CloseMask");
        awardItem = f_GetObject("ResourceCommonItem");
        awardGrid = f_GetObject("CommonItemGrid").GetComponent<UIGrid>();
        mGetBtn = f_GetObject("GetBtn");
        UIEventListener.Get(mGetBtn).onClick = f_GetBtn;
        UIEventListener.Get(mCloseMask).onClick = f_MaskClose;
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is AwardGetSubPageParam))
        {
MessageBox.ASSERT("AwardGetSubPage must be passed to AwardGetSunpageParam");
            return;
        }
        mParam = (AwardGetSubPageParam)e;
        mTitle.text = mParam.m_szTitle;
        mContent.text = mParam.m_szContent;
        mLockBtn.SetActive(mParam.m_eGetState == EM_AwardGetState.Lock);
        mGetBtn.SetActive(mParam.m_eGetState == EM_AwardGetState.CanGet);
        mAlreadyGetTip.SetActive(mParam.m_eGetState == EM_AwardGetState.AlreadyGet);
        mAwardShowComponent.f_Show(mParam.m_AwardList, EM_CommonItemShowType.All, EM_CommonItemClickType.AllTip, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        if (mParam == null)
            return;
        if (mParam.m_NeedHoldUI != null)
        {
            ccUIHoldPool.GetInstance().f_Hold(mParam.m_NeedHoldUI);
        }
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        if (mParam == null)
            return;
        if (mParam.m_NeedHoldUI != null)
        {
            ccUIHoldPool.GetInstance().f_Hold(mParam.m_NeedHoldUI);
        }
    }

    private void f_MaskClose(GameObject go)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AwardGetSubPage, UIMessageDef.UI_CLOSE);
    }

    private void f_GetBtn(GameObject go)
    {
        if (mParam == null)
            return;
        if (mParam.m_Callback_Click != null)
        {
            mParam.m_Callback_Click(mParam.m_Callback_Value);
        }
    }
}


public class AwardGetSubPageParam
{
    public AwardGetSubPageParam(EM_AwardGetState getState,string title,string content,List<AwardPoolDT> awardList,ccCallback callbackHandle,object callbackValue,ccUIBase needHoldUI)
    {
        m_eGetState = getState;
        m_szTitle = title;
        m_szContent = content;
        m_AwardList = awardList;
        m_Callback_Click = callbackHandle;
        m_Callback_Value = callbackValue;
        m_NeedHoldUI = needHoldUI;
    }

    public EM_AwardGetState m_eGetState
    {
        get;
        private set;
    }

    public string m_szTitle
    {
        get;
        private set;
    }

    public string m_szContent
    {
        get;
        private set;
    }

    public List<AwardPoolDT> m_AwardList
    {
        get;
        private set;
    }

    /// <summary>
    /// 点击回调函数
    /// </summary>
    public ccCallback m_Callback_Click
    {
        get;
        private set;
    }

    /// <summary>
    /// 点击回调函数参数
    /// </summary>
    public object m_Callback_Value
    {
        get;
        private set;
    }

    /// <summary>
    /// 奖励跳转界面是需要Hold的界面
    /// </summary>
    public ccUIBase m_NeedHoldUI
    {
        get;
        private set;
    }
}
