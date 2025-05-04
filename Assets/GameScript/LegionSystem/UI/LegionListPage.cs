using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 军团列表界面
/// </summary>
public class LegionListPage : UIFramwork
{
    private enum Em_LegionListPageType
    {
        List = 0,
        Search = 1,
    }
    private Em_LegionListPageType mCurType;
    
    private GameObject mLegionCreateBtn;
    private UIScrollView mListScrollView;
    private GameObject mLegionListPanel;
    private GameObject mLegionListParent;
    private GameObject mLegionListItem;
    private List<BasePoolDT<long>> _legionList;
    private UIWrapComponent _legionListWrapComponent;
    private UIWrapComponent mLegionListWrapComponent
    {
        get
        {
            if (_legionListWrapComponent == null)
            {
                _legionList = LegionMain.GetInstance().m_LegionInfor.f_GetLegionList();
                _legionListWrapComponent = new UIWrapComponent(230, 2, 780, 5, mLegionListParent, mLegionListItem, _legionList, f_LegionListItemUpdateByInfo, null);
            }
            return _legionListWrapComponent;
        }
    }

    private GameObject mSearchBtn;
    private UIInput mSearchInput;

    private GameObject mLegionSearchPanel;
    private LegionListItem mLegionSearchItem;
    private GameObject mReturnListBtn;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mLegionCreateBtn = f_GetObject("LegionCreateBtn");
        mListScrollView = f_GetObject("ListScrollView").GetComponent<UIScrollView>();
        mListScrollView.onDragFinished = f_MomnetEnds;
        mLegionListPanel = f_GetObject("LegionListPanel");
        mLegionListParent = f_GetObject("LegionListParent");
        mLegionListItem = f_GetObject("LegionListItem");
        f_RegClickEvent("BackBtn", f_BackBtn);
        f_RegClickEvent(mLegionCreateBtn, f_LegionCreateBtn);

        mSearchBtn = f_GetObject("SearchBtn");
        mSearchInput = f_GetObject("SearchInput").GetComponent<UIInput>();
        f_RegClickEvent(mSearchBtn, f_SearchBtn);

        mLegionSearchPanel = f_GetObject("LegionSearchPanel");
        mLegionSearchItem = f_GetObject("LegionSearchItem").GetComponent<LegionListItem>();
        mReturnListBtn = f_GetObject("ReturnListBtn");
        f_RegClickEvent(mReturnListBtn, f_ReturnListBtn);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        f_SetPageType(Em_LegionListPageType.List);
        mLegionListWrapComponent.f_ResetView();
        mSearchInput.defaultText = CommonTools.f_GetTransLanguage(427);
        mSearchInput.value = string.Empty;
        f_OpenOrCloseMonenyPage(true);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_SELf_LEGION_APPLY_LIST_UPDATE, f_UpdateByListInfo);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        f_OpenOrCloseMonenyPage(false);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_SELf_LEGION_APPLY_LIST_UPDATE, f_UpdateByListInfo);
    }

    private void f_UpdateByListInfo(object result)
    {
        mLegionListWrapComponent.f_UpdateView();
    }

    private void f_SetPageType(Em_LegionListPageType curType)
    {
        mCurType = curType;
        mLegionListPanel.SetActive(mCurType == Em_LegionListPageType.List);
        mLegionSearchPanel.SetActive(mCurType == Em_LegionListPageType.Search);
    }

    private void f_UpdateSearchItem(BasePoolDT<long> info)
    {
        mLegionSearchItem.f_UpdateByInfo(info);
        f_RegClickEvent(mLegionSearchItem.mApplyBtn, f_LegionListItemApplyBtn, info);
        f_RegClickEvent(mLegionSearchItem.mCanelApplyBtn, f_LegionListItemCanelApplyBtn, info);
    }

    private void f_BackBtn(GameObject go,object value1,object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionListPage,UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_SearchBtn(GameObject go, object value1, object value2)
    {
        string legionName = mSearchInput.value.Trim();
        int byteNum = ccMath.f_GetStringBytesLength(legionName);
        if (string.IsNullOrEmpty(legionName))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(428));
            return;
        }
        else if (!Data_Pool.m_BlockWordPool.f_CheckValidity(ref legionName))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(429));
            mSearchInput.value = legionName;
            return;
        }
        else if (byteNum < LegionConst.LEGION_NAME_BYTE_MIN_NUM)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(430));
            return;
        }
        else if (byteNum > LegionConst.LEGION_NAME_BYTE_MAX_NUM)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(431));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionSearch;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionSearch;
        LegionMain.GetInstance().m_LegionInfor.f_LegionSearch(legionName,socketCallbackDt);
    }

    private void f_ReturnListBtn(GameObject go,object value1,object value2)
    {
        f_SetPageType(Em_LegionListPageType.List);
        mLegionListWrapComponent.f_UpdateView();
    }

    private void f_LegionCreateBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionCreatePage, UIMessageDef.UI_OPEN); 
    }

    /// <summary>
    /// 拖拉到底部处理函数
    /// </summary>
    private void f_MomnetEnds()
    {
        Debug.Log("Moment Stop");
        Vector3 constraint = mListScrollView.panel.CalculateConstrainOffset(mListScrollView.bounds.min, mListScrollView.bounds.min);
        Debug.Log(constraint);
        if (constraint.y <= 0)
        {
            Debug.Log(CommonTools.f_GetTransLanguage(432));
            LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(false, false, f_Callback_LegionListInfo);
        }
    }

    private void f_LegionListItemUpdateByInfo(Transform tf,BasePoolDT<long> dt)
    {
        LegionListItem item = tf.GetComponent<LegionListItem>();
        item.f_UpdateByInfo(dt);
        f_RegClickEvent(item.mApplyBtn, f_LegionListItemApplyBtn,dt);
        f_RegClickEvent(item.mCanelApplyBtn, f_LegionListItemCanelApplyBtn, dt);
    }

    private void f_LegionListItemApplyBtn(GameObject go, object value1, object value2)
    {
        Debug.LogWarning(CommonTools.f_GetTransLanguage(433));
        LegionInfoPoolDT info = (LegionInfoPoolDT)value1;
        int tNow = GameSocket.GetInstance().f_GetServerTime();
        if (LegionMain.GetInstance().m_LegionInfor.m_iSelfApplyLegionNum >= LegionConst.LEGION_SELF_APPLY_MAX_NUM)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(434), LegionConst.LEGION_SELF_APPLY_MAX_NUM));
            return;
        }
        else if (tNow - LegionMain.GetInstance().m_LegionInfor.m_iIOTime <= LegionConst.LEGION_JOIN_AGAIN_TIME_DIS)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(435));
            return;
        }
        else if (LegionMain.GetInstance().m_LegionInfor.f_CheckIsApplying(info.iId))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(436));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionApply;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionApply;
        LegionMain.GetInstance().m_LegionInfor.f_LegionApply(info.iId, socketCallbackDt);
        
    }

    private void f_LegionListItemCanelApplyBtn(GameObject go, object value1, object value2)
    {
        Debug.LogWarning(CommonTools.f_GetTransLanguage(437));        
        LegionInfoPoolDT info = (LegionInfoPoolDT)value1;
        if (!LegionMain.GetInstance().m_LegionInfor.f_CheckIsApplying(info.iId))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(438));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionCancelApply;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionCancelApply;
        LegionMain.GetInstance().m_LegionInfor.f_LegionDisapply(info.iId, socketCallbackDt);
    }

    private void f_Callback_LegionListInfo(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(439) + result);
        mLegionListWrapComponent.f_UpdateView();
    }

    private void f_Callback_LegionApply(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(440));
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionNotFound)
        {
            UITool.f_OpenOrCloseWaitTip(true);
            LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(false, true, f_Callback_LegionListFirst,true);
        }
        else
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(441) + result);

        if (mCurType == Em_LegionListPageType.List)
            mLegionListWrapComponent.f_UpdateView();
        else
            f_UpdateSearchItem(LegionMain.GetInstance().m_LegionInfor.m_SearchLegionInfo);
    }

    private void f_Callback_LegionCancelApply(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1556));
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionNotFound)
        {
            UITool.f_OpenOrCloseWaitTip(true);
            LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(false, true, f_Callback_LegionListFirst,true);
        }
        else
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(442) + result);
        if (mCurType == Em_LegionListPageType.List)
            mLegionListWrapComponent.f_UpdateView();
        else
            f_UpdateSearchItem(LegionMain.GetInstance().m_LegionInfor.m_SearchLegionInfo);
    }

    private void f_Callback_LegionSearch(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            f_SetPageType(Em_LegionListPageType.Search);
            f_UpdateSearchItem(LegionMain.GetInstance().m_LegionInfor.m_SearchLegionInfo);
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionNotFound)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(443));
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(444) + result);
        }
    }

    private void f_Callback_LegionListFirst(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(445) + result);
        mLegionListWrapComponent.f_ResetView();
    }
    private void f_OpenOrCloseMonenyPage(bool isOpen)
    {
        if (isOpen)
        {
            List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
            listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
            listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
            listMoneyType.Add(EM_MoneyType.eUserAttr_LegionContribution);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        }
    }
}
