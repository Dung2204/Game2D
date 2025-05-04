using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
public class EveryDayHotSalePool : BasePool
{
    public EveryDayHotSalePool() : base("EveryDayHotSalePoolDT", true)
    {
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_EveryDayHotSaleSC.f_GetAll().Count; i++)
        {
            EveryDayHotSalePoolDT tEvery = new EveryDayHotSalePoolDT();
            EveryDayHotSaleDT ttttttt = glo_Main.GetInstance().m_SC_Pool.m_EveryDayHotSaleSC.f_GetAll()[i] as EveryDayHotSaleDT;
            tEvery.IDayNum = ttttttt.iDayNum;
            tEvery.ITempleteId = ttttttt.iId;
            tEvery.iId = ttttttt.iId;
            tEvery.m_EveryDayHotSaleDT = ttttttt;
            f_Save(tEvery);
        }
    }

    #region Pool数据管理
    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        hotSaleInfo tHotSaleInfo = (hotSaleInfo)Obj;

        EveryDayHotSalePoolDT tEveryDayHotSalePoolDT = f_GetForId((long)tHotSaleInfo.taskID) as EveryDayHotSalePoolDT;
        if (tEveryDayHotSalePoolDT == null)
        {
            MessageBox.DEBUG("EveryDayHotSalePool null");
        }
        tEveryDayHotSalePoolDT.BuyTime = tHotSaleInfo.buyTime;
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        hotSaleInfo tHotSaleInfo = (hotSaleInfo)Obj;

        EveryDayHotSalePoolDT tEveryDayHotSalePoolDT = f_GetForId((long)tHotSaleInfo.taskID) as EveryDayHotSalePoolDT;
        if (tEveryDayHotSalePoolDT == null)
        {
            MessageBox.DEBUG("EveryDayHotSalePool null");
        }
        tEveryDayHotSalePoolDT.BuyTime = tHotSaleInfo.buyTime;
    }

    protected override void RegSocketMessage()
    {
        hotSaleInfo tHotSaleInfo = new hotSaleInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_EveryDayHotSaleBuy, tHotSaleInfo, Callback_SocketData_Update);
    }
    #endregion
    /// <summary>
    /// 请求初始化热卖
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_EveryDayInfo(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_EveryDayHotSaleInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(1);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_EveryDayHotSaleInfo, bBuf);
    }

    public void f_EveryDayBuy(short tempID, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_EveryDayHotSaleBuy, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tempID);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_EveryDayHotSaleBuy, bBuf);
    }
}

