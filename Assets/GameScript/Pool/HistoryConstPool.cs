using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using System.Collections;

public class HistoryConstPool : BasePool
{

    public HistoryConstPool() : base("HistoryConstPoolDT", false)
    {
    }

    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_HistoryConst tSC_HistoryConst = (SC_HistoryConst)Obj;
        HistoryConstPoolDT tHistoryConstPoolDT = new HistoryConstPoolDT();
        tHistoryConstPoolDT.m_iConst = tSC_HistoryConst.uCost;
        tHistoryConstPoolDT.m_iYYYYMMDD = tSC_HistoryConst.uYYYYMMDD;
        tHistoryConstPoolDT.iId = tSC_HistoryConst.uYYYYMMDD;
        f_Save(tHistoryConstPoolDT);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_HistoryConst tSC_HistoryConst = (SC_HistoryConst)Obj;
        HistoryConstPoolDT tHistoryConstPoolDT = f_GetForId(tSC_HistoryConst.uYYYYMMDD) as HistoryConstPoolDT;
        if (tHistoryConstPoolDT == null)
        {
            f_Socket_AddData(Obj, true);
        }
        else
        {
            tHistoryConstPoolDT.m_iConst = tSC_HistoryConst.uCost;
        }
    }

    protected override void RegSocketMessage()
    {
        SC_HistoryConst tSC_HistoryConst = new SC_HistoryConst();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_CostRecord, tSC_HistoryConst, Callback_SocketData_Update);

    }

    /// <summary>
    /// 获得时间区间内累计消耗了多少元宝
    /// </summary>
    /// <param name="StarTime"></param>
    /// <param name="EndTime"></param>
    public int f_GetAppointTimeRanges(long StarTime, long EndTime)
    {
        HistoryConstPoolDT tHistoryConstPoolDT;
        int tConst = 0;
        int HistoryTime = 0;
        for (int i = 0; i < f_GetAll().Count; i++)
        {
            tHistoryConstPoolDT = f_GetAll()[i] as HistoryConstPoolDT;
            HistoryTime = ccMath.f_Data2Int(tHistoryConstPoolDT.m_iYYYYMMDD);

            if (HistoryTime >= StarTime && HistoryTime <= EndTime)
            {
                tConst += tHistoryConstPoolDT.m_iConst;
            }
        }
        return tConst;
    }

}

