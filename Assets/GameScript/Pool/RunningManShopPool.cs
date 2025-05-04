using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RunningManShopPool : BasePool
{
    public RunningManShopPool() : base("RunningManShopPoolDT")
    {

    }

    protected override void f_Init()
    {
        List<NBaseSCDT> tInitList =  glo_Main.GetInstance().m_SC_Pool.m_RunningManShopSC.f_GetAll();
        for (int i = 0; i < tInitList.Count; i++)
        {
            RunningManShopDT tItem = (RunningManShopDT)tInitList[i];
            RunningManShopPoolDT tPoolDt = new RunningManShopPoolDT(tItem);
            f_Save(tPoolDt);
        }
    }

    private void f_Reset()
    {
        List<BasePoolDT<long>> tResetList = f_GetAll();
        for (int i = 0; i < tResetList.Count; i++)
        {
            RunningManShopPoolDT tItem = (RunningManShopPoolDT)tResetList[i];
            tItem.f_Reset();
        }
    }

    protected override void RegSocketMessage()
    {
        CMsg_SC_RunningManShopNode tsc_RunningManShopNode = new CMsg_SC_RunningManShopNode();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_RunningManShopInit, tsc_RunningManShopNode, f_SC_RunningManShopInitHandle);
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }

    #region 处理协议

    private void f_SC_RunningManShopInitHandle(int value1,int value2,int num,ArrayList dataArr)
    {
        for (int i = 0; i < dataArr.Count; i++)
        {
            CMsg_SC_RunningManShopNode tServerData = (CMsg_SC_RunningManShopNode)dataArr[i];
            RunningManShopPoolDT tPoolDt = (RunningManShopPoolDT)f_GetForId(tServerData.id);
            if (tPoolDt != null)
                tPoolDt.f_UpdateTimes(tServerData.buyTimes);
            else
MessageBox.ASSERT("Data does not exist，id:" + tServerData.id);
        }
    }

    #endregion

    #region 发送协议

    public void f_RunningManShopInit(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RunningManShopInit, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RunningManShopInit, bBuf);
    }

    public void f_RunningManShopBuy(ushort goodId,ushort buyTimes,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RunningManShopBuy, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(goodId);
        tCreateSocketBuf.f_Add(buyTimes);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RunningManShopBuy, bBuf);
    }


    #endregion

    #region 对外接口

    public void f_ExecuteAferShopInit(ccCallback callbackHandle)
    {
        if (isInited)
        {
            callbackHandle(eMsgOperateResult.OR_Succeed);
        }
        else
        {
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = callbackHandle;
            socketCallbackDt.m_ccCallbackFail = callbackHandle;
            f_RunningManShopInit(socketCallbackDt);
        }
    }
    #endregion

    private bool isInited = false;
}
