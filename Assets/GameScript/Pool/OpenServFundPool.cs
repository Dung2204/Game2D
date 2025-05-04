using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 活动开服基金/全民福利
/// </summary>
public class OpenServFundPool : BasePool {
    /// <summary>
    /// 是否购买了基金
    /// </summary>
    public bool m_HasBuyOpenServFund;
    /// <summary>
    /// 全服购买基金人数
    /// </summary>
    public int m_buyFundCount = 0;
    public OpenServFundPool() : base("OpenServFundPoolDT",true)
    {

    }
    protected override void f_Init()
    {
        List<NBaseSCDT> listOpenServerDT = glo_Main.GetInstance().m_SC_Pool.m_OpenServFundSC.f_GetAll();
        for (int i = 0; i < listOpenServerDT.Count; i++)
        {
            OpenServFundPoolDT openServFundPoolDT = new OpenServFundPoolDT();
            OpenServFundDT dt = listOpenServerDT[i] as OpenServFundDT;
            openServFundPoolDT.iId = dt.iId;
            openServFundPoolDT.eOpenServFundType = (EM_OpenServFundType)dt.iActType;
            openServFundPoolDT.m_OpenServFundDT = dt;
            openServFundPoolDT.m_buyTimes = 0;
            f_Save(openServFundPoolDT);
        }
    }

    protected override void RegSocketMessage()
    {
        SC_OpenServFund SC_OpenServFund = new SC_OpenServFund();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_OpenServFund, SC_OpenServFund, Callback_SocketData_Update);
        SC_OpenServFund SC_OpenServFund2 = new SC_OpenServFund();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_AllServFund, SC_OpenServFund2, Callback_SocketData_Update);
        SC_QBuyOpenServFund scQBuyOpenServFund = new SC_QBuyOpenServFund();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_BuyOpenServFund, scQBuyOpenServFund, QBuyOpenServFundCallback);
    }
    private void QBuyOpenServFundCallback(object data)
    {
        SC_QBuyOpenServFund scQBuyOpenServFund = (SC_QBuyOpenServFund)data;
        m_HasBuyOpenServFund = scQBuyOpenServFund.isBuy == 1 ? true : false;
        m_buyFundCount = scQBuyOpenServFund.count;
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_OpenServFund scOpenServFund = (SC_OpenServFund)Obj;
        OpenServFundPoolDT openServFundPoolDT = (OpenServFundPoolDT)f_GetForId(scOpenServFund.GiftId);
        openServFundPoolDT.m_buyTimes = 1;
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_OpenServFund scOpenServFund = (SC_OpenServFund)Obj;
        OpenServFundPoolDT openServFundPoolDT = (OpenServFundPoolDT)f_GetForId(scOpenServFund.GiftId);
        openServFundPoolDT.m_buyTimes = 1;
    }
    #region 外部接口
    /// <summary>
    /// 检查开服基金和全民福利红点提示
    /// </summary>
    public void f_CheckRedPoint()
    {
        SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
        QueryCallback.m_ccCallbackSuc = QueryInfoSuc;
        QueryCallback.m_ccCallbackFail = QueryInfoFailBack;
        f_QueryBuyInfo(QueryCallback);
    }
    /// <summary>
    /// 红点提示回调
    /// </summary>
    private void QueryInfoSuc(object data)
    {
        SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
        QueryCallback.m_ccCallbackSuc = QueryOpenServFundInfoSucRedPoint;
        QueryCallback.m_ccCallbackFail = QueryInfoFailBack;
        Data_Pool.m_OpenServFundPool.f_QueryOpenServFundInfo(QueryCallback);
    }
    private void QueryInfoFailBack(object data)
    {
    }
    public void QueryOpenServFundInfoSucRedPoint(object data)
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ActOpenServFund);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ActOpenWelfare);
        List<BasePoolDT<long>> listOpenServFundPoolDT = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_OpenServFundPool.f_GetAll());
        int palyerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        for (int i = listOpenServFundPoolDT.Count - 1; i >= 0; i--)
        {
            OpenServFundPoolDT openServFundPoolDT = listOpenServFundPoolDT[i] as OpenServFundPoolDT;
            OpenServFundDT openServFundDT = openServFundPoolDT.m_OpenServFundDT;
            int buyTimes = openServFundPoolDT.m_buyTimes;
            //开服基金
            if (Data_Pool.m_OpenServFundPool.m_HasBuyOpenServFund && openServFundPoolDT.eOpenServFundType == EM_OpenServFundType.OpenServFund
                && palyerLevel >= openServFundDT.iCondiction && buyTimes <= 0)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.ActOpenServFund);
                //ActivityPage.SortAct(EM_ActivityType.OpenServFund); // move supermarket
            }
            //全民福利
            if (openServFundPoolDT.eOpenServFundType == EM_OpenServFundType.OpenWelfare && Data_Pool.m_OpenServFundPool.m_buyFundCount >= openServFundDT.iCondiction
                && buyTimes <= 0)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.ActOpenWelfare);
                ActivityPage.SortAct(EM_ActivityType.OpenWelfare);
            }
        }
    }
    /// <summary>
    /// 查询玩家和服务器基金信息
    /// 玩家是否已经购买基金
    /// 全服玩家购买基金人数
    /// </summary>
    public void f_QueryBuyInfo(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_QBuyOpenServFund, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QBuyOpenServFund, bBuf);
    }
    private bool isQueryOpenSevInfo = false;
    /// <summary>
    /// 查询开服基金信息
    /// </summary>
    public void f_QueryOpenServFundInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (isQueryOpenSevInfo)
        {
            if (tSocketCallbackDT != null)
            {
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            }
            return;
        }
        isQueryOpenSevInfo = true;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_QueryOpenServFund, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QueryOpenServFund, bBuf);
    }
    /// <summary>
    /// 购买基金
    /// </summary>
    public void f_BuyServFund(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_BuyOpenServFund, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_BuyOpenServFund, bBuf);
    }
    /// <summary>
    /// 领取
    /// </summary>
    public void f_Get(EM_OpenServFundType eOpenServFundType,int buyIndex, SocketCallbackDT tSocketCallbackDT)
    {
        //(f_GetForId(buyIndex) as OpenServFundPoolDT).m_buyTimes++;
        //tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
        SocketCommand socketCommand = SocketCommand.CS_GetOpenServFund;
        switch (eOpenServFundType)
        {
            case EM_OpenServFundType.OpenServFund:
                socketCommand = SocketCommand.CS_GetOpenServFund;
                break;
            case EM_OpenServFundType.OpenWelfare:
                socketCommand = SocketCommand.CS_GetAllServFund;
                break;
        }
        GameSocket.GetInstance().f_RegCommandReturn((int)socketCommand, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(buyIndex);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(socketCommand, bBuf);
    }
    #endregion
}
public enum EM_OpenServFundType
{
    /// <summary>
    /// 开服基金
    /// </summary>
    OpenServFund = 1,
    /// <summary>
    /// 全民福利
    /// </summary>
    OpenWelfare =2, 
}
