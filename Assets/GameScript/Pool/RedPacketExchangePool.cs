using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;
using System.Collections.Generic;
/// <summary>
/// 红包兑换pool
/// </summary>
public class RedPacketExchangePool : BasePool
{
    private bool mIsInitForSer = false;//防止重复请求
    public RedPacketExchangePool() : base("RedPacketExchangePoolDT", true)
    {
    }

    protected override void f_Init()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_RedPacketExChangeSC.f_GetAll();
        for (int i = 0; i < listDT.Count; i++)
        {
            RedPacketExchangeDT dt = listDT[i] as RedPacketExchangeDT;
            RedPacketExchangePoolDT poolDT = new RedPacketExchangePoolDT();
            poolDT.iId = dt.iId;
            poolDT.mHasExchangeCount = 0;
            poolDT.mRedPacketExChangeDT = dt;
            f_Save(poolDT);
        }
    }

    protected override void RegSocketMessage()
    {
        SC_RedPacketExchangeInfo scRedPacketExchangeInfo = new SC_RedPacketExchangeInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_RedPacketExchangeInfo, scRedPacketExchangeInfo, Callback_RecordData_Update);
    }
    /// <summary>
    /// 红包信息回调
    /// </summary>
    protected void Callback_RecordData_Update(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            SC_RedPacketExchangeInfo scRedPacketExchangeInfo = (SC_RedPacketExchangeInfo)tData;
            RedPacketExchangePoolDT poolDT = f_GetForId(scRedPacketExchangeInfo.id) as RedPacketExchangePoolDT;
            poolDT.mHasExchangeCount = scRedPacketExchangeInfo.iExchangeTimes;
        }
        f_CheckRedPoint();
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #region 检查红点
    public void f_CheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RedPacketExchange);
        GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
        int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (userLevel < gameParamDTOpenLevel.iParam1)
        {
            return;
        }
        List<BasePoolDT<long>> listPoolDT = f_GetAll();
        for (int i = 0; i < listPoolDT.Count; i++)
        {
            RedPacketExchangePoolDT poolDT = listPoolDT[i] as RedPacketExchangePoolDT;
            bool isOpen = CommonTools.f_CheckTime(poolDT.mRedPacketExChangeDT.iTimeBegin.ToString(), poolDT.mRedPacketExChangeDT.iTimeEnd.ToString());
            if (isOpen && poolDT.mHasExchangeCount < poolDT.mRedPacketExChangeDT.iExchangeTimes)
            {
                List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(poolDT.mRedPacketExChangeDT.szConsumeRes);
                bool isGet = true;
                for (int a = 0; a < listCommonDT.Count; a++)
                {
                    ResourceCommonDT commonDT = listCommonDT[a];
                    int num = UITool.f_GetGoodNum((EM_ResourceType)commonDT.mResourceType, commonDT.mResourceId);
                    if (num < commonDT.mResourceNum)
                    {
                        isGet = false;
                        break;
                    }
                }
                if (isGet)
                {
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.RedPacketExchange);
                    NewYearActPage.SortAct(EM_NewYearActType.RedPacketExchange);
                    return;
                }
            }
        }
    }
    #endregion
    #region 外部接口
    /// <summary>
    /// 查询信息
    /// </summary>
    public void f_QueryInfo(SocketCallbackDT tSocketCallbackDT)
    {
        f_CheckRedPoint();
        if (mIsInitForSer)
        {
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        mIsInitForSer = true;
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RedPacketExchangeInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);

        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RedPacketExchangeInfo, bBuf);
    }
    /// <summary>
    /// 兑换红包
    /// </summary>
    public void f_Exchange(int id,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RedPacketExchange, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RedPacketExchange, bBuf);
    }
    #endregion
}
