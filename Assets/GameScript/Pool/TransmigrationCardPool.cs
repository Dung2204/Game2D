using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;

public class TransmigrationCardPool : BasePool
{
    public TransmigrationCardPool() : base("TransmigrationCardPoolDT", true)
    {

    }
    protected override void f_Init()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_TransmigrationCardSC.f_GetAll();
        for(int i =0;i< listDT.Count;i++)
        {
            TransmigrationCardDT dt = listDT[i] as TransmigrationCardDT;
            TransmigrationCardPoolDT poolDT = new TransmigrationCardPoolDT();
            poolDT.iId = dt.iId;
            poolDT.m_TransmigrationCardData = dt;
            f_Save(poolDT);
        }
    }

    //请求卡牌转换
    public void f_GetChangeCard(int target,int count,long[] cardSeverId,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CardTransmigration, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        ////向服务器提交数据
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(target);
        tCreateSocketBuf.f_Add(count);
        for (int i = 0; i < cardSeverId.Length; i++)
        {
            tCreateSocketBuf.f_Add(cardSeverId[i]);
        }
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CardTransmigration, bBuf);
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


}
