using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;

public class TransmigrationTreasurePool : BasePool
{
    public TransmigrationTreasurePool() : base("TransmigrationTreasurePoolDT", true)
    {

    }
    protected override void f_Init()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_TransmigrationTreasureSC.f_GetAll();
        for (int i = 0; i < listDT.Count; i++)
        {
            TransmigrationTreasureDT dt = listDT[i] as TransmigrationTreasureDT;
            TransmigrationTreasurePoolDT poolDT = new TransmigrationTreasurePoolDT();
            poolDT.iId = dt.iId;
            poolDT.m_TransmigrationTreasureDT = dt;
            f_Save(poolDT);
        }
    }

    //请求法宝熔炼
    public void f_GetTransTreasureRequest(int target, int count, long[] TreasureSeverId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TreasureTransmigration, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        ////向服务器提交数据
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(target);
        tCreateSocketBuf.f_Add(count);
        for (int i = 0; i < TreasureSeverId.Length; i++)
        {
            tCreateSocketBuf.f_Add(TreasureSeverId[i]);
        }
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TreasureTransmigration, bBuf);
    }

    //检测是否有红点
    public void f_CheckTransTreasureRedPoint()
    {
        List<BasePoolDT<long>> _treasureList = Data_Pool.m_TreasurePool.f_GetAll();
        List<BasePoolDT<long>> _treasureTransList = Data_Pool.m_TransmigrationTreasurePool.f_GetAll();
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TransmigrationTreasure);
        for (int i= 0;i< _treasureTransList.Count;i++)
        {
            TransmigrationTreasurePoolDT data = (TransmigrationTreasurePoolDT)_treasureTransList[i];
            for (int j =0;j< _treasureList.Count;j++)
            {
                TreasurePoolDT dataTrea = (TreasurePoolDT)_treasureList[j];
                int countTreasure = UITool.f_GetTreasureNum(dataTrea);
                if (countTreasure >= data.m_TransmigrationTreasureDT.iCost && dataTrea.m_TreasureDT.iImportant == (int)EM_Important.Oragen)
                {
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TransmigrationTreasure);
                    Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TransmigrationTreasure);
                    return;
                }
            }
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
}
