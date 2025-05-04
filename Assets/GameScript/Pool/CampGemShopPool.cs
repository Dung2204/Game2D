using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
public class CampGemShopPool : BasePool
{
    public CampGemShopPool() : base("CampGemShopPool", true)
    {

    }

    protected override void f_Init()
    {
        List<NBaseSCDT> tScDT = glo_Main.GetInstance().m_SC_Pool.m_CampGemShopSC.f_GetAll();
        CampGemShopDT tShopPoolDT;
        for (int i = 0; i < tScDT.Count; i++)
        {
            tShopPoolDT = tScDT[i] as CampGemShopDT;
            CampGemShopPoolDT tPoolDT = new CampGemShopPoolDT();
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


    public void f_Buy(int id, int times, SocketCallbackDT tsocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_Shop_CampGemBuy, tsocketCallbackDT.m_ccCallbackSuc, tsocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tBuf = new CreateSocketBuf();
        tBuf.f_Add(id);
        tBuf.f_Add(times);
        byte[] bBuf = tBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_Shop_CampGemBuy, bBuf);
    }
}
