using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class RecommendFriendPage :UIFramwork
{
    private GameObject _refreshGrayBtn;
    private UILabel _refreshGrayBtnLabel;
    private GameObject _refreshBtn;
                 
    private GameObject _recommendItemParent;
    private GameObject _recommendItem;
    private List<BasePoolDT<long>> _recommendList;
    private UIWrapComponent _recommendWrapComponent;
    private UIWrapComponent mRecommendWrapComponent
    {
        get
        {
            if (_recommendWrapComponent == null)
            {
                _recommendList = Data_Pool.m_FriendPool.f_GetDataListByType(EM_FriendListType.Recommend);
                _recommendWrapComponent = new UIWrapComponent(210, 2, 650, 4, _recommendItemParent, _recommendItem, _recommendList,f_UpdateRecommendItem,null);
            }
            return _recommendWrapComponent;
        }
    }

    private int _regTimeId = -99;

    private bool _bLock = false;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _refreshGrayBtn = f_GetObject("RefreshGrayBtn");
        _refreshGrayBtnLabel = f_GetObject("RefreshGrayBtnLabel").GetComponent<UILabel>();
        _refreshBtn = f_GetObject("RefreshBtn");
        _recommendItemParent = f_GetObject("RecommendItemParent");
        _recommendItem = f_GetObject("RecommendItem");
        f_RegClickEvent(_refreshBtn, f_RefreshBtn);
        f_RegClickEvent("MaskClose", f_MaskClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (_regTimeId == -99)
            _regTimeId = ccTimeEvent.GetInstance().f_RegEvent(1.0f, true, null, f_UpdateByPreSecond);
        f_UpdateView();
        Data_Pool.m_FriendPool.f_RefreshRecommendData(f_FinishRefresh, false);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_FRIENDDATA_UPDATE,f_UpdateByDataMsg, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_FRIENDDATA_UPDATE, f_UpdateByDataMsg, this);
        if (_regTimeId != -99)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(_regTimeId);
            _regTimeId = -99;
        }
    }

    private void f_UpdateByPreSecond(object obj)
    {
        if (!_bLock)
            return;
        f_UpdateView();
    }

    private void f_UpdateByDataMsg(object value)
    {
        EM_FriendListType tType = (EM_FriendListType)value;
        if (tType == EM_FriendListType.Recommend)
        {
            mRecommendWrapComponent.f_UpdateView();
        }
    }

    //更新界面
    private void f_UpdateView()
    {
        int tVal = GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_FriendPool.mRefreshRecommendTime;
        _bLock = tVal < GameParamConst.RecommendRefreshInterval;
        _refreshBtn.SetActive(!_bLock);
        _refreshGrayBtn.SetActive(_bLock);
        if (_bLock)
            _refreshGrayBtnLabel.text = (GameParamConst.RecommendRefreshInterval - tVal).ToString();
    }

    private void f_RefreshBtn(GameObject go, object value1, object value2)
    {
        Data_Pool.m_FriendPool.f_RefreshRecommendData(f_FinishRefresh, true); 
    }

    private void f_FinishRefresh(object result)
    {
        _bLock = true;
        f_UpdateView();
        mRecommendWrapComponent.f_ResetView();
    }

    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RecommendFriendPage, UIMessageDef.UI_CLOSE);
    }

    #region item相关

    private void f_UpdateRecommendItem(Transform tf, BasePoolDT<long> dt)
    {
        BasePlayerPoolDT tData = (BasePlayerPoolDT)dt;
        RecommendFriendItem tItem = tf.GetComponent<RecommendFriendItem>();
        tItem.f_UpdateByInfo(tData);
        f_RegClickEvent(tItem.mApplyBtn, f_RecommendItemApplyBtn, dt);
        f_RegClickEvent(tItem.mIcon.gameObject, f_RecommendItemIconClik, dt);
    }

    private void f_RecommendItemApplyBtn(GameObject go, object value1, object value2)
    {
        BasePlayerPoolDT tDT = (BasePlayerPoolDT)value1;
        SocketCallbackDT tCallbackDt = new SocketCallbackDT();
        tCallbackDt.m_ccCallbackSuc = f_AddFriendSuc;
        tCallbackDt.m_ccCallbackFail = f_AddFriendFail;
        //申请加好友
        Data_Pool.m_FriendPool.f_ApplyFriend(tDT.iId, tCallbackDt);
        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1003),tDT.m_szName));
    }

    private void f_AddFriendSuc(object result)
    {
        mRecommendWrapComponent.f_UpdateView();
    }

    private void f_AddFriendFail(object result)
    {
        mRecommendWrapComponent.f_UpdateView();
        MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1004) + result);
    }

    private void f_RecommendItemIconClik(GameObject go, object value1, object value2)
    {
        BasePlayerPoolDT tDT = (BasePlayerPoolDT)value1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_OPEN, tDT);
    }

    #endregion

}
