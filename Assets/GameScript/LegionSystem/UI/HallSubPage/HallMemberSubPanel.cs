using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;
using System;

public class HallMemberSubPanel : SubPanelBase
{
    public GameObject mLegionMemberParent;
    public GameObject mLegionMemberItem;
    private List<BasePoolDT<long>> _memberList;
    private UIWrapComponent _legionMemberWrapComponent;
    private UIWrapComponent mLegionMemberWrapComponent
    {
        get
        {
            if (_legionMemberWrapComponent == null)
            {
                _memberList = LegionMain.GetInstance().m_LegionPlayerPool.f_GetAll();
                _legionMemberWrapComponent = new UIWrapComponent(257, 1, 800, 6, mLegionMemberParent, mLegionMemberItem, _memberList, f_LegionMemberItemUpdateByInfo, null);
            }
            return _legionMemberWrapComponent;
        }
    }

    public override void f_Open()
    {
        base.f_Open();
        mLegionMemberWrapComponent.f_ResetView();
    }

    public override void f_Close()
    {
        base.f_Close();
    }

    public override void f_RegEvent()
    {
        base.f_RegEvent();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_MEMBER_DATA_UPDATE, f_UpdateByMsg_MemberData, this);
    }

    public override void f_UnregEvent()
    {
        base.f_UnregEvent();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_MEMBER_DATA_UPDATE, f_UpdateByMsg_MemberData, this);
    }

    private void f_UpdateByMsg_MemberData(object value)
    {
        mLegionMemberWrapComponent.f_UpdateView();
    }

    public void f_LegionMemberItemUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        LegionMemberItem item = tf.GetComponent<LegionMemberItem>();
        item.f_UpdateByInfo(dt);
        f_RegClickEvent(item.mAppointBtn, f_MemberItemAppointBtn, dt, item);
        f_RegClickEvent(item.mIcon.gameObject, f_MemberItemLookBtn, dt);
        f_RegClickEvent(item.mLookBtn, f_MemberItemLookBtn, dt);

        f_RegClickEvent(item.mExiteBtn, f_LegionQuit, dt);
        f_RegClickEvent(item.mBecomChiefBtn, f_LegionHandover, dt, item);
        f_RegClickEvent(item.mBecomDeputyBtn, f_LegionAppoint, dt, item);
        f_RegClickEvent(item.mDeposeBtn, f_LegionDismiss, dt, item);
        f_RegClickEvent(item.mDelateBtn, f_LegionImpeach, dt);
        f_RegClickEvent(item.mKickOutBtn, f_LegionKickout, dt);

    }

    private void f_MemberItemAppointBtn(GameObject go, object value1, object value2)
    {
        LegionPlayerPoolDT info = (LegionPlayerPoolDT)value1;
        LegionMemberItem item = (LegionMemberItem)value2;
        item.f_ShowAppointContent();
    }

    private void f_MemberItemLookBtn(GameObject go, object value1, object value2)
    {
        LegionPlayerPoolDT info = (LegionPlayerPoolDT)value1;
        if (info.iId == Data_Pool.m_UserData.m_iUserId)
        {
            //你点了自己的头像
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(548));
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_OPEN, info.PlayerInfo);
    }


    private void f_LegionQuit(GameObject go, object value1, object value2)
    {
        LegionPlayerPoolDT info = (LegionPlayerPoolDT)value1;
        EM_LegionPostionEnum slefPos = (EM_LegionPostionEnum)LegionMain.GetInstance().m_LegionPlayerPool.m_iSelfPos;
        if (slefPos == EM_LegionPostionEnum.eLegion_Chief)
        {
            //移交军团长职务之后,才可退出军团
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(549));
            return;
        }
        //退出军团24小时之内,无法加入军团 确定要退出?
        string tContent = string.Format(CommonTools.f_GetTransLanguage(550));
        // 系统提示 确定  取消
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(551), tContent, CommonTools.f_GetTransLanguage(552), f_SureLegionQuit, CommonTools.f_GetTransLanguage(553));
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_SureLegionQuit(object value)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionQuit;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionQuit;
        LegionMain.GetInstance().m_LegionPlayerPool.f_LegionQuit(socketCallbackDt);
    }

    private void f_LegionKickout(GameObject go, object value1, object value2)
    {
        LegionPlayerPoolDT info = (LegionPlayerPoolDT)value1;
        //确定要把{0}提出军团?
        string tContent = string.Format(CommonTools.f_GetTransLanguage(554), info.PlayerInfo.m_szName);
        //系统提示  确定  取消
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(555), tContent, CommonTools.f_GetTransLanguage(556), f_SureLegionKickout, CommonTools.f_GetTransLanguage(557), null, value1, null);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_SureLegionKickout(object value)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        LegionPlayerPoolDT info = (LegionPlayerPoolDT)value;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionKickout;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionKickout;
        LegionMain.GetInstance().m_LegionPlayerPool.f_LegionKickout(info.iId, socketCallbackDt);
    }

    private void f_LegionAppoint(GameObject go, object value1, object value2)
    {
        LegionMemberItem item = (LegionMemberItem)value2;
        item.f_HideAppointContetn();
        if (LegionMain.GetInstance().m_LegionPlayerPool.m_iDeputyNum >= LegionConst.LEGION_DEPUTY_MAX_NUM)
        {
            //军团最多有{0}个副团长
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(558), LegionConst.LEGION_DEPUTY_MAX_NUM));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        LegionPlayerPoolDT info = (LegionPlayerPoolDT)value1;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionAppoint;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionAppoint;
        LegionMain.GetInstance().m_LegionPlayerPool.f_LegionAppoint(info.iId, socketCallbackDt);
    }

    private void f_LegionDismiss(GameObject go, object value1, object value2)
    {
        LegionMemberItem item = (LegionMemberItem)value2;
        item.f_HideAppointContetn();
        LegionPlayerPoolDT info = (LegionPlayerPoolDT)value1;
        //确定要罢免{0}玩家的副军团长职务?
        string tContent = string.Format(CommonTools.f_GetTransLanguage(559), info.PlayerInfo.m_szName);
        //系统提示  确定  取消
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(560), tContent, CommonTools.f_GetTransLanguage(561), f_SureLegionDismiss, CommonTools.f_GetTransLanguage(562), null, value1, null);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_SureLegionDismiss(object value)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        LegionPlayerPoolDT info = (LegionPlayerPoolDT)value;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionDismiss;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionDismiss;
        LegionMain.GetInstance().m_LegionPlayerPool.f_LegionDimiss(info.iId, socketCallbackDt);
    }

    private void f_LegionHandover(GameObject go, object value1, object value2)
    {
        //你是否要移交军团长职务?
        string tContent = CommonTools.f_GetTransLanguage(563);
        //系统提示  确定  取消
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(560), tContent, CommonTools.f_GetTransLanguage(561), f_SureLegionHandOver, CommonTools.f_GetTransLanguage(562), null, value1, null);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
        LegionMemberItem item = (LegionMemberItem)value2;
        item.f_HideAppointContetn();
    }

    private void f_SureLegionHandOver(object value)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        LegionPlayerPoolDT info = (LegionPlayerPoolDT)value;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionHandover;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionHandover;
        LegionMain.GetInstance().m_LegionPlayerPool.f_LegionHandover(info.iId, socketCallbackDt);
    }

    private void f_LegionImpeach(GameObject go, object value1, object value2)
    {
        LegionPlayerPoolDT info = (LegionPlayerPoolDT)value1;
        int offlineTime = info.PlayerInfo.m_iOfflineTime;
        int tNow = GameSocket.GetInstance().f_GetServerTime();
        int tSycee = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
        if (info.PlayerInfo.m_iOfflineTime == 0 || tNow - offlineTime <= LegionConst.LEGION_IMPEACH_TIME_DIS)
        {
            //军团长离线时间未超过5天,当前不可弹劾
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(564));
            return;
        }
        else if (tSycee < LegionConst.LEGION_IMPEACH_COST)
        {
            //元宝不足{0}
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(565), LegionConst.LEGION_IMPEACH_COST));
            return;
        }
        //弹劾后,你将成为新的军团长.是否要花费{0}元宝,弹劾军团长
        string tContent = string.Format(CommonTools.f_GetTransLanguage(566), LegionConst.LEGION_IMPEACH_COST);
        //系统提示  确定  取消
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(560), tContent, CommonTools.f_GetTransLanguage(561), f_SureLegionImpeach, CommonTools.f_GetTransLanguage(562), null, value1, null);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_SureLegionImpeach(object value)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        LegionPlayerPoolDT info = (LegionPlayerPoolDT)value;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionImpeach;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionImpeach;
        LegionMain.GetInstance().m_LegionPlayerPool.f_LegionImpeach(socketCallbackDt);
    }


    #region 协议回调处理

    private void f_Callback_LegionQuit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
            //军团退出错误
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(567), (int)result));
            //军团退出错误
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(568) + result);
        }
    }

    private void f_Callback_LegionKickout(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //提出成功
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(569));
        }
        else
        {
            //军团提出错误
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(570), (int)result));
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(571) + result);
        }
    }

    private void f_Callback_LegionAppoint(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //任命成功
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(572));
        }
        else
        {
            //军团任命错误
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(573), (int)result));
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(574) + result);
        }
    }

    private void f_Callback_LegionDismiss(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //免职成功
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(575));
        }
        else
        {
            //军团免职错误
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(576), (int)result));
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(577) + result);
        }
    }

    private void f_Callback_LegionHandover(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //禅让成功
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(578));
        }
        else
        {
            //军团禅让错误
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(579), (int)result));
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(580) + result);
        }
    }

    private void f_Callback_LegionImpeach(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //弹劾成功
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(581));
        }
        else
        {
            //军团弹劾错误
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(582), (int)result));
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(583) + result);
        }
    }

    #endregion



}
