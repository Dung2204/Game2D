using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using System.Collections;

public class NewYearSellingPool : BasePool
{

    bool _IsFisrst;
    public NewYearSellingPool() : base("NewYearSellingPoolDT", false)
    {
        _IsFisrst = true;
        NewYearSellingDT tNewYearSellingDT;
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_NewYearSelling.f_GetAll().Count; i++)
        {
            tNewYearSellingDT = glo_Main.GetInstance().m_SC_Pool.m_NewYearSelling.f_GetAll()[i] as NewYearSellingDT;
            NewYearSellingPoolDT tNewYearSellingPool = new NewYearSellingPoolDT();

            tNewYearSellingPool.m_BuyTime = 0;
            tNewYearSellingPool.m_iTempleteId = tNewYearSellingDT.iId;
            tNewYearSellingPool.iId = tNewYearSellingDT.iId;
            f_Save(tNewYearSellingPool);
        }
    }
    #region  Pool数据管理
    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        f_Socket_UpdateData(Obj);

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_NewYearSellingInfo tNewYearSellingPool = (SC_NewYearSellingInfo)Obj;
        NewYearSellingPoolDT tNewYearSellingPoolDT = f_GetForId(tNewYearSellingPool.goodId) as NewYearSellingPoolDT;
        if (tNewYearSellingPoolDT == null)
        {
MessageBox.ASSERT("New Year Shopping PoolDT is empty" + tNewYearSellingPool.goodId);
        }
        tNewYearSellingPoolDT.m_BuyTime = tNewYearSellingPool.times;
        GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < gameParamDTOpenLevel.iParam1)
        {
            return;
        }
        if (tNewYearSellingPoolDT.m_bIsShowBtn && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) >= tNewYearSellingPoolDT.m_NewYearSelling.iDiscountPrice)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.NewYearSelling);
        }

    }

    protected override void RegSocketMessage()
    {
        SC_NewYearSellingInfo tSC_NewYearSellingInfo = new SC_NewYearSellingInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_NewYearSellingInfo, tSC_NewYearSellingInfo, ccCallback_Int2_V2);
    }

    public void ccCallback_Int2_V2(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT item in aData)
        {
            f_Socket_AddData(item, false);
        }
    }
    #endregion


    public void f_NewYearSellingBuy(int Goodid, short Time, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearSellingBuy, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(Goodid);
        tCreateSocketBuf.f_Add(Time);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearSellingBuy, bBuf);
    }

    public void f_NewYearSellingInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT == null)
        {
            if (!_IsFisrst)
                return;
        }
        _IsFisrst = false;
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearSellingInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearSellingInfo, bBuf);
    }
}

