using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class FriendPage : UIFramwork
{
    private string[] _nullDataBtnLabelArr = null;
    private string[] _nullDataTitleLabelArr = null;

    private GameObject _nullDataSubPanel;
    private GameObject _nullDataBtn;
    private UILabel _nullDataBtnLabel;
    private UILabel _nullDataTitle;

    private UILabel _friendNumLabel;
    private GameObject _recommendBtn;
    private GameObject _addBtn;
    private GameObject _friendPagePanel;
    private GameObject _friendItemParent;
    private GameObject _friendItem;
    private List<BasePoolDT<long>> _friendList;
    private UIWrapComponent _friendWrapComponent;
    private UIWrapComponent mFriendWrapComponent
    {
        get
        {
            if (_friendWrapComponent == null)
            {
                _friendList = Data_Pool.m_FriendPool.f_GetDataListByType(EM_FriendListType.Friend);
                _friendWrapComponent = new UIWrapComponent(210, 1, 1392,4,_friendItemParent,_friendItem, _friendList,f_UpdateFriendItem, null);
            }
            return _friendWrapComponent;
        }
    }

    private UILabel _vigorRecvNum;
    private GameObject _onekeyRecvVigorBtn;
    private GameObject _vigorPagePanel;
    private GameObject _vigorItemParent;
    private GameObject _vigorItem;
    private List<BasePoolDT<long>> _vigorList;
    private UIWrapComponent _vigorWrapComponent;
    private UIWrapComponent mVigorWrapComponent
    {
        get
        {
            if (_vigorWrapComponent == null)
            {
                _vigorList = Data_Pool.m_FriendPool.f_GetDataListByType(EM_FriendListType.Vigor);
                _vigorWrapComponent = new UIWrapComponent(210, 1, 1392, 4, _vigorItemParent, _vigorItem, _vigorList, f_UpdateVigorItem, null);
            }
            return _vigorWrapComponent;
        }
    }

    private GameObject _friendApplyPagePanel;
    private GameObject _friendApplyItemParent;
    private GameObject _friendApplyItem;
    private GameObject _onekeyAgreeBtn;
    private List<BasePoolDT<long>> _friendApplyList;
    private UIWrapComponent _friendApplyWrapComponent;
    private UIWrapComponent mFriendApplyWrapComponent
    {
        get
        {
            if (_friendApplyWrapComponent == null)
            {
                _friendApplyList = Data_Pool.m_FriendPool.f_GetDataListByType(EM_FriendListType.Apply);
                _friendApplyWrapComponent = new UIWrapComponent(210, 1,1392,4,_friendApplyItemParent,_friendApplyItem,_friendApplyList,f_UpdateFriendApplyItem,null);
            }
            return _friendApplyWrapComponent;
        }
    }

    private GameObject _blacklistPagePanel;
    private GameObject _blacklistItemParent;
    private GameObject _blacklistItem;
    private List<BasePoolDT<long>> _blacklistList;
    private UIWrapComponent _blacklistWrapComponent;
    private UIWrapComponent mBlacklistWrapComponent
    {
        get
        {
            if (_blacklistWrapComponent == null)
            {
                _blacklistList = Data_Pool.m_FriendPool.f_GetDataListByType(EM_FriendListType.Blacklist);
                _blacklistWrapComponent = new UIWrapComponent(210, 1, 1392, 6, _blacklistItemParent, _blacklistItem, _blacklistList, f_UpdateBlacklistItem, null);
            }
            return _blacklistWrapComponent;
        }
    }

    private GameObject _friendBtn;
    private GameObject _vigorBtn;
    private GameObject _blacklistBtn;
    private GameObject _friendApplyBtn;

    private GameObject _backBtn;

    public EM_FriendListType mCurPageType
    {
        get;
        private set;
    }

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        _nullDataBtnLabelArr = new string[] { CommonTools.f_GetTransLanguage(1158), CommonTools.f_GetTransLanguage(1159), "", CommonTools.f_GetTransLanguage(1160), "", "" };
        _nullDataTitleLabelArr = new string[] { CommonTools.f_GetTransLanguage(1161), CommonTools.f_GetTransLanguage(1162), "", CommonTools.f_GetTransLanguage(1163), "", "" };
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _nullDataSubPanel = f_GetObject("NullDataSubPanel");
        _nullDataBtn = f_GetObject("NullDataBtn");
        _nullDataBtnLabel = f_GetObject("NullDataBtnLabel").GetComponent<UILabel>();
        _nullDataTitle = f_GetObject("NullDataTitle").GetComponent<UILabel>();
        f_RegClickEvent(_nullDataBtn, f_NullDataBtnHandle);
        _recommendBtn = f_GetObject("RecommendBtn");
        _addBtn = f_GetObject("AddBtn");
        _friendNumLabel = f_GetObject("FriendNumLabel").GetComponent<UILabel>();
        _friendPagePanel = f_GetObject("FriendPagePanel");
        _friendItemParent = f_GetObject("FriendItemParent");
        _friendItem = f_GetObject("FriendItem");
        _friendApplyPagePanel = f_GetObject("FriendApplyPagePanel");
        _friendApplyItemParent = f_GetObject("FriendApplyItemParent");
        _friendApplyItem = f_GetObject("FriendApplyItem");
        _onekeyAgreeBtn = f_GetObject("OnekeyAgreeBtn");
        f_RegClickEvent(_onekeyAgreeBtn, f_OnekeyAgreeBtn);
        _blacklistPagePanel = f_GetObject("BlacklistPagePanel");
        _blacklistItemParent = f_GetObject("BlacklistItemParent");
        _blacklistItem = f_GetObject("BlacklistItem");
        _vigorRecvNum = f_GetObject("VigorRecvNum").GetComponent<UILabel>();
        _onekeyRecvVigorBtn = f_GetObject("OnekeyRecvVigorBtn");
        _vigorPagePanel = f_GetObject("VigorPagePanel");
        _vigorItemParent = f_GetObject("VigorItemParent");
        _vigorItem = f_GetObject("VigorItem");
        f_RegClickEvent(_recommendBtn, f_RecommentBtnHandle);
        f_RegClickEvent(_addBtn, f_AddBtnHandle);
        f_RegClickEvent(_onekeyRecvVigorBtn, f_OnekeyRecvVigorBtn);

        _friendBtn = f_GetObject("FriendBtn");
        _vigorBtn = f_GetObject("VigorBtn");
        _blacklistBtn = f_GetObject("BlacklistBtn");
        _friendApplyBtn = f_GetObject("FriendApplyBtn");
        f_RegClickEvent(_friendBtn,f_PageBtnHandle,EM_FriendListType.Friend);
        f_RegClickEvent(_vigorBtn, f_PageBtnHandle, EM_FriendListType.Vigor);
        f_RegClickEvent(_blacklistBtn, f_PageBtnHandle, EM_FriendListType.Blacklist);
        f_RegClickEvent(_friendApplyBtn, f_PageBtnHandle, EM_FriendListType.Apply);

        _backBtn = f_GetObject("BackBtn");
        f_RegClickEvent(_backBtn, f_BackBtnHandle);
        f_RegClickEvent(f_GetObject("BtnClose"), f_BackBtnHandle);
        GameObject ModelParent = f_GetObject("ModelParent");
        UITool.f_GetStatelObject(1100, ModelParent.transform, Vector3.zero, new Vector3(100f, -50f, 0), 18, "Model", 80);
    }

    #region BtnHandle

    private void f_NullDataBtnHandle(GameObject go, object value1, object value2)
    {
        if (mCurPageType == EM_FriendListType.Friend || mCurPageType == EM_FriendListType.Apply)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RecommendFriendPage, UIMessageDef.UI_OPEN);
        }
        else if (mCurPageType == EM_FriendListType.Vigor)
        {
            //跳转回好友界面
            f_PageBtnHandle(this.gameObject, EM_FriendListType.Friend,null);
        }
    }

    private void f_RecommentBtnHandle(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RecommendFriendPage, UIMessageDef.UI_OPEN);
    }

    private void f_AddBtnHandle(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AddFriendPage, UIMessageDef.UI_OPEN);
    }

    private void f_PageBtnHandle(GameObject go, object value1, object value2)
    {
        EM_FriendListType tType = (EM_FriendListType)value1;
        if (mCurPageType != tType)
        {
            f_UpdateByType(tType);
        }
    }

    private void f_BackBtnHandle(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FriendPage, UIMessageDef.UI_CLOSE);
        ccU3DEngine.ccUIHoldPool.GetInstance().f_UnHold();
    }

    #endregion
    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.FriendVigor, f_GetObject("VigorBtn"), ReddotCallback_Show_VigorBtn);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.FriendApply, f_GetObject("FriendApplyBtn"), ReddotCallback_Show_FriendApplyBtn);
        UpdateReddotUI();
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.FriendVigor, f_GetObject("VigorBtn"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.FriendApply, f_GetObject("FriendApplyBtn"));
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FriendVigor);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FriendApply);
    }
    private void ReddotCallback_Show_VigorBtn(object Obj)
    {
        int iNum = (int)Obj;
        GameObject VigorBtn = f_GetObject("VigorBtn");
        UITool.f_UpdateReddot(VigorBtn, iNum, new Vector3(146, 25, 0), 2500);
    }
    private void ReddotCallback_Show_FriendApplyBtn(object Obj)
    {
        int iNum = (int)Obj;
        GameObject FriendApplyBtn = f_GetObject("FriendApplyBtn");
        UITool.f_UpdateReddot(FriendApplyBtn, iNum, new Vector3(146, 25, 0), 2500);
    }
    #endregion

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Vigor);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        f_UpdateByType(EM_FriendListType.Friend);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_FRIENDDATA_UPDATE, f_UpdateByDataMsg, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_UpdateByNextDay, this);
        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/MainMenu/CommonBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
		//My Code
		float windowAspect = (float)Screen.width /  (float) Screen.height ;
		MessageBox.ASSERT("" + windowAspect);
		if(windowAspect <= 1.55)
		{
			f_GetObject("Anchor-Center").transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
		//
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexBg = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexBg;
        }
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage,UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_FRIENDDATA_UPDATE, f_UpdateByDataMsg, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_UpdateByNextDay, this);
    }
    //数据更新消息
    private void f_UpdateByDataMsg(object value)
    {
        EM_FriendListType tType = (EM_FriendListType)value;

        if (mCurPageType == EM_FriendListType.Friend)
        {
            if (mCurPageType == tType || tType == EM_FriendListType.Vigor)
            {
                if (_friendList != null && _friendList.Count == 1)
                    mFriendWrapComponent.f_ResetView();
                else
                    mFriendWrapComponent.f_UpdateView();
                _friendNumLabel.text = string.Format("{0}/{1}", Data_Pool.m_FriendPool.mHaveFriendNum, GameParamConst.FriendMaxNum);
                _nullDataSubPanel.SetActive(_friendList.Count == 0);
            }
        }
        else if (mCurPageType == EM_FriendListType.Apply)
        {
            if (mCurPageType == tType)
            {
                if (_friendApplyList != null && _friendApplyList.Count == 1)
                    mFriendApplyWrapComponent.f_ResetView();
                else
                    mFriendApplyWrapComponent.f_UpdateView();
                _nullDataSubPanel.SetActive(_friendApplyList.Count == 0);
            }
        }
        else if (mCurPageType == EM_FriendListType.Blacklist)
        {
            if (mCurPageType == tType)
            {
                if (_blacklistList != null && _blacklistList.Count == 1)
                    mBlacklistWrapComponent.f_ResetView();
                else
                    mBlacklistWrapComponent.f_UpdateView();
            }
        }
        else if (mCurPageType == EM_FriendListType.Vigor)
        {
            if (mCurPageType == tType)
            {
                //更新精力相关
                if (_vigorList != null && _vigorList.Count == 1)
                    mVigorWrapComponent.f_ResetView();
                else
                    mVigorWrapComponent.f_UpdateView();
                _vigorRecvNum.text = string.Format("{0}/{1}", Data_Pool.m_FriendPool.mRecvVigorNum, GameParamConst.RecvVigorMaxNum);
                _nullDataSubPanel.SetActive(_vigorList.Count == 0);
            }
        }

    }
    //跨天处理
    private void f_UpdateByNextDay(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.FriendPool)
            return;
        if (mCurPageType == EM_FriendListType.Friend)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1164));
            mFriendWrapComponent.f_ResetView();
        }
        else if (mCurPageType == EM_FriendListType.Vigor)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1164));
        }
    }

    private void f_UpdateByType(EM_FriendListType pageType)
    {
        mCurPageType = pageType;
        _nullDataBtnLabel.text = _nullDataBtnLabelArr[(int)mCurPageType];
        _nullDataTitle.text = _nullDataTitleLabelArr[(int)mCurPageType];
        _friendPagePanel.SetActive(mCurPageType == EM_FriendListType.Friend);
        _friendApplyPagePanel.SetActive(mCurPageType == EM_FriendListType.Apply);
        _blacklistPagePanel.SetActive(mCurPageType == EM_FriendListType.Blacklist);
        _vigorPagePanel.SetActive(mCurPageType == EM_FriendListType.Vigor);

        _friendBtn.transform.Find("BtnPress").gameObject.SetActive(mCurPageType == EM_FriendListType.Friend);
        _vigorBtn.transform.Find("BtnPress").gameObject.SetActive(mCurPageType == EM_FriendListType.Vigor);
        _blacklistBtn.transform.Find("BtnPress").gameObject.SetActive(mCurPageType == EM_FriendListType.Blacklist);
        _friendApplyBtn.transform.Find("BtnPress").gameObject.SetActive(mCurPageType == EM_FriendListType.Apply);
        if (mCurPageType == EM_FriendListType.Friend)
        {
            f_UpdateFriendPage();
        }
        else if (mCurPageType == EM_FriendListType.Vigor)
        {
            f_UpdateVigorPage();
        }
        else if (mCurPageType == EM_FriendListType.Blacklist)
        {
            f_UpdateBlacklistPage();
        }
        else if (mCurPageType == EM_FriendListType.Apply)
        {
            f_UpdateApplyPage();
        }
    }
    #region 相关类型界面更新

    private void f_UpdateFriendPage()
    {
        mFriendWrapComponent.f_ResetView();
        _friendNumLabel.text = string.Format("{0}/{1}", Data_Pool.m_FriendPool.mHaveFriendNum, GameParamConst.FriendMaxNum);
        _nullDataSubPanel.SetActive(_friendList.Count == 0);
    }

    private void f_UpdateVigorPage()
    {
        mVigorWrapComponent.f_ResetView();
        _vigorRecvNum.text = string.Format("{0}/{1}", Data_Pool.m_FriendPool.mRecvVigorNum, GameParamConst.RecvVigorMaxNum);
        _nullDataSubPanel.SetActive(_vigorList.Count == 0);
    }

    private void f_UpdateBlacklistPage()
    {
        mBlacklistWrapComponent.f_ResetView();
        _nullDataSubPanel.SetActive(false);
    }

    private void f_UpdateApplyPage()
    {
        mFriendApplyWrapComponent.f_ResetView();
        _nullDataSubPanel.SetActive(_friendApplyList.Count == 0);
    }

    #endregion

    #region 好友Item相关

    private void f_UpdateFriendItem(Transform tf, BasePoolDT<long> dt)
    {
        BasePlayerPoolDT tData = (BasePlayerPoolDT)dt;
        FriendItem tItem = tf.GetComponent<FriendItem>();
        tItem.f_UpdateByInfo(tData);
        f_RegClickEvent(tItem.mDonateVigorBtn, f_FriendItemDonateBtn,dt);
        f_RegClickEvent(tItem.mIcon.gameObject, f_ItemIconClickHandle,dt);
    }

    private void f_FriendItemDonateBtn(GameObject go, object value1, object value2)
    {
        BasePlayerPoolDT tData = (BasePlayerPoolDT)value1;
        SocketCallbackDT tCallbackDt = new SocketCallbackDT();
        tCallbackDt.m_ccCallbackSuc = f_DonateVigorSuc;
        tCallbackDt.m_ccCallbackFail = f_DonateVigorFail;
        Data_Pool.m_FriendPool.f_SendVigor(tData.iId,tCallbackDt);
        
    }

    private void f_DonateVigorSuc(object result)
    {
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1165));             
    }

    private void f_DonateVigorFail(object result)
    {
        MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1166) + result);
    }

    #endregion


    #region 领取精力Item相关

    /// <summary>
    /// 一键领取精力
    /// </summary>
    private void f_OnekeyRecvVigorBtn(GameObject go,object value1,object value2)
    {
        int tVigorHave = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor);
        if (Data_Pool.m_FriendPool.f_CheckRecvVigorIsFull())
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1167));
            return;
        }
        else if (_vigorList.Count == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1168));
            return;
        }
        else if (tVigorHave >= GameParamConst.VigorEcoverLimit)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1169));
            return;
        }
        SocketCallbackDT tCallbackDt = new SocketCallbackDT();
        tCallbackDt.m_ccCallbackSuc = f_OnekeyRecvVigorSuc;
        tCallbackDt.m_ccCallbackFail = f_OnekeyRecvVigorFail;
        Data_Pool.m_FriendPool.f_RecvVigor(0, tCallbackDt);
    }

    private void f_UpdateVigorItem(Transform tf, BasePoolDT<long> dt)
    {
        BasePlayerPoolDT tData = (BasePlayerPoolDT)dt;
        FriendVigorItem tItem = tf.GetComponent<FriendVigorItem>();
        tItem.f_UpdateByInfo(tData);
        f_RegClickEvent(tItem.mGetVigorBtn, f_FriendVigorItemGetBtn,dt);
        f_RegClickEvent(tItem.mIcon.gameObject, f_ItemIconClickHandle,dt);
    }

    private void f_FriendVigorItemGetBtn(GameObject go, object value1, object value2)
    {
        BasePlayerPoolDT tData = (BasePlayerPoolDT)value1;
        int tVigorHave = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor);
        if (Data_Pool.m_FriendPool.f_CheckRecvVigorIsFull())
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1167));
            return;
        }
        else if (tVigorHave >= GameParamConst.VigorEcoverLimit)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1169));
            return;
        }
        SocketCallbackDT tCallbackDt = new SocketCallbackDT();
        tCallbackDt.m_ccCallbackSuc = f_RecvVigorSuc;
        tCallbackDt.m_ccCallbackFail = f_RecvVigorFail;
        Data_Pool.m_FriendPool.f_RecvVigor(tData.iId,tCallbackDt);
    }

    private void f_RecvVigorSuc(object result)
    {
        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1170), Data_Pool.m_FriendPool.mRecvVigorNum - Data_Pool.m_FriendPool.mLastRecvVigorNum));
    }

    private void f_RecvVigorFail(object result)
    {
        MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1171) + result);
    }

    private void f_OnekeyRecvVigorSuc(object result)
    {
        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1170), Data_Pool.m_FriendPool.mRecvVigorNum - Data_Pool.m_FriendPool.mLastRecvVigorNum));
    }

    private void f_OnekeyRecvVigorFail(object result)
    {
        MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1172) + result);
    }

    #endregion

    #region 黑名单Item相关

    private void f_UpdateBlacklistItem(Transform tf, BasePoolDT<long> dt)
    {
        BasePlayerPoolDT tData = (BasePlayerPoolDT)dt;
        FriendBlacklistItem tItem = tf.GetComponent<FriendBlacklistItem>();
        tItem.f_UpdateByInfo(tData);
        f_RegClickEvent(tItem.mDisblacklistBtn, f_DisblacklistBtn, dt);
        f_RegClickEvent(tItem.mIcon.gameObject, f_ItemIconClickHandle, dt);
    }

    /// <summary>
    /// 解除黑名单
    /// </summary>
    private void f_DisblacklistBtn(GameObject go, object value1, object value2)
    {
        BasePlayerPoolDT tData = (BasePlayerPoolDT)value1;
        SocketCallbackDT tCallbackDt = new SocketCallbackDT();
        tCallbackDt.m_ccCallbackSuc = f_DisblacklistSuc;
        tCallbackDt.m_ccCallbackFail = f_DisblacklistFail;
        Data_Pool.m_FriendPool.f_Disblacklist(tData.iId, tCallbackDt);
    }

    private void f_DisblacklistSuc(object result)
    {
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1173));
    }

    private void f_DisblacklistFail(object result)
    {
        UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1174) + result);
    }

    #endregion


    #region 好友申请Item相关

    private void f_UpdateFriendApplyItem(Transform tf, BasePoolDT<long> dt)
    {
        BasePlayerPoolDT tData = (BasePlayerPoolDT)dt;
        FriendApplyItem tItem = tf.GetComponent<FriendApplyItem>();
        tItem.f_UpdateByInfo(tData);
        f_RegClickEvent(tItem.mAgreeBtn, f_FriendItemAgreeBtn,dt);
        f_RegClickEvent(tItem.mRefuseBtn, f_FriendItemRefuseBtn,dt);
        f_RegClickEvent(tItem.mIcon.gameObject, f_ItemIconClickHandle,dt);
    }

    private void f_FriendItemAgreeBtn(GameObject go, object value1, object value2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        BasePlayerPoolDT tData = (BasePlayerPoolDT)value1;
        SocketCallbackDT tCallbackDt = new SocketCallbackDT();
        tCallbackDt.m_ccCallbackSuc = f_ReplyFriendSuc;
        tCallbackDt.m_ccCallbackFail = f_ReplyFriendFail;
        Data_Pool.m_FriendPool.f_ReplyFriend(tData.iId, 1,tCallbackDt);
    }

    private void f_FriendItemRefuseBtn(GameObject go, object value1, object value2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        BasePlayerPoolDT tData = (BasePlayerPoolDT)value1;
        SocketCallbackDT tCallbackDt = new SocketCallbackDT();
        tCallbackDt.m_ccCallbackSuc = f_ReplyFriendSuc;
        tCallbackDt.m_ccCallbackFail = f_ReplyFriendFail;
        Data_Pool.m_FriendPool.f_ReplyFriend(tData.iId, 0, tCallbackDt);
    }

    private void f_ReplyFriendSuc(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1175));
        mFriendApplyWrapComponent.f_UpdateView();
    }

    private void f_ReplyFriendFail(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.eOR_PeerInBlack)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1176));
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_InPeerBlack)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1177));
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_FriendListIsFull)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1178));
        }
        else
        {
            MessageBox.ASSERT(string.Format("ReplyFriend Result = {0}", result));
        }
        mFriendApplyWrapComponent.f_UpdateView();
    }

    private int onekeyAgreeNum = 0;
    private int curOnekeyAgreeIdx = 0;
    private void f_OnekeyAgreeBtn(GameObject go, object value1, object value2)
    {
        onekeyAgreeNum = _friendApplyList.Count;
        if (onekeyAgreeNum == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1179));
            return;
        }
        curOnekeyAgreeIdx = 0;
        UITool.f_OpenOrCloseWaitTip(true);
        for (int i = 0; i <onekeyAgreeNum; i++)
        {
            SocketCallbackDT tCallbackDt = new SocketCallbackDT();
            tCallbackDt.m_ccCallbackSuc = f_Callback_OnekeyAgreeFriend;
            tCallbackDt.m_ccCallbackFail = f_Callback_OnekeyAgreeFriend;
            Data_Pool.m_FriendPool.f_ReplyFriend(_friendApplyList[i].iId, 1, tCallbackDt);
        }
    }

    private void f_Callback_OnekeyAgreeFriend(object result)
    {
        curOnekeyAgreeIdx++;
        if (curOnekeyAgreeIdx == onekeyAgreeNum)
        {
            UITool.f_OpenOrCloseWaitTip(false);
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1180));
            mFriendApplyWrapComponent.f_UpdateView();
        }
        else if (curOnekeyAgreeIdx > onekeyAgreeNum)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1181));
        }
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
            MessageBox.ASSERT(string.Format("ReplyFriend Result = {0}", result));
        }
    }

    #endregion


    /// <summary>
    /// Item头像点击 每个Item 头像点击处理都一样
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_ItemIconClickHandle(GameObject go,object value1,object value2)
    {
        BasePlayerPoolDT tData = (BasePlayerPoolDT)value1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_OPEN, tData);
    }
}
