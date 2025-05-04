using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class PatrolVisitFriendPage :UIFramwork
{
    private GameObject m_FriendItemParent;
    private GameObject m_FriendItem;
    private UILabel m_PacifyRiotTimes;
    
    private List<BasePoolDT<long>> friendList;
    private UIWrapComponent _friendWrapComponent;
    private UIWrapComponent m_FriendWrapComponent
    {
        get
        {
            if (_friendWrapComponent == null)
            {
                friendList = Data_Pool.m_PatrolPool.m_FriendInfoList;
                _friendWrapComponent = new UIWrapComponent(230, 1, 690, 6, m_FriendItemParent, m_FriendItem, friendList, f_UpdateFriendItem, null);
            }
            return _friendWrapComponent;
        }
    }
    private GameObject m_FriendNullTip;

    private long curUserId;
    private ccCallback m_Callback_UpdatePatrolDt;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_FriendItemParent = f_GetObject("FriendItemParent");
        m_FriendItem = f_GetObject("FriendItem");
        m_PacifyRiotTimes = f_GetObject("PacifyRiotTimes").GetComponent<UILabel>();
        f_RegClickEvent("BtnClose", f_CloseMask);
        f_RegClickEvent("CloseMask", f_CloseMask);
        m_FriendNullTip = f_GetObject("FriendNullTip");
        f_RegClickEvent(m_FriendNullTip, f_FriendNullTipClick);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is object[]))
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(942));
        object[] paramArr = (object[])e;
        curUserId = (long)paramArr[0];
        m_Callback_UpdatePatrolDt = (ccCallback)paramArr[1];
        m_FriendWrapComponent.f_ResetView();
        m_PacifyRiotTimes.text = string.Format(CommonTools.f_GetTransLanguage(943), Data_Pool.m_PatrolPool.m_iPacifyTimesLimit - Data_Pool.m_PatrolPool.m_iPacifyTimes);
        m_FriendNullTip.SetActive(friendList.Count == 0);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_UpdateFriendItem(Transform tf,BasePoolDT<long> data)
    {
        PatrolVisitFriendItem tItem = tf.GetComponent<PatrolVisitFriendItem>();
        tItem.f_UpdateByInfo((PatrolFriendInfoPoolDT)data);
        f_RegClickEvent(tItem.m_BtnVisit,f_BtnVisitFriend,data);
    }

    private void f_BtnVisitFriend(GameObject go, object value1, object value2)
    {
        PatrolFriendInfoPoolDT tItem = (PatrolFriendInfoPoolDT)value1;
        if (tItem.iId == curUserId)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(944));
            return;
        }
        curUserId = tItem.iId;
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolInit;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolInit;
        Data_Pool.m_PatrolPool.f_PatrolInit(curUserId, socketCallbackDt);
    }

    private void f_Callback_PatrolInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(945));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolVisitFriendPage, UIMessageDef.UI_CLOSE);
            if (m_Callback_UpdatePatrolDt != null)
                m_Callback_UpdatePatrolDt(curUserId);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(946) + result);
        }
    }

    private void f_CloseMask(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolVisitFriendPage, UIMessageDef.UI_CLOSE);
    }

    //推荐好友
    private void f_FriendNullTipClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RecommendFriendPage, UIMessageDef.UI_OPEN);
    }
}
