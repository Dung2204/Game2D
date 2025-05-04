using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
public class BattleFeatShopPool : BasePool
{
    public BattleFeatShopPool() : base("BattleFeatShopPool", true)
    {

    }

    protected override void f_Init()
    {
        List<NBaseSCDT> tScDT = glo_Main.GetInstance().m_SC_Pool.m_BattleFeatShopSC.f_GetAll();
        BattleFeatShopDT tShopPoolDT;
        for (int i = 0; i < tScDT.Count; i++)
        {
            tShopPoolDT = tScDT[i] as BattleFeatShopDT;
            BattleFeatShopPoolDT tPoolDT = new BattleFeatShopPoolDT();
            tPoolDT.iId = tShopPoolDT.iId;
            tPoolDT.m__iTempleteId = tShopPoolDT.iId;
            f_Save(tPoolDT);
        }
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
    }

    protected override void RegSocketMessage()
    {
    }


    public void f_BattleFeatShopBuy(int id, int times, SocketCallbackDT tsocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_BattleFeatBuy, tsocketCallbackDT.m_ccCallbackSuc, tsocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tBuf = new CreateSocketBuf();
        tBuf.f_Add(id);
        tBuf.f_Add(times);
        byte[] bBuf = tBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_BattleFeatBuy, bBuf);
    }
}
