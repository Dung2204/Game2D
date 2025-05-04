using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
class EveryDayHotSalePoolDT : BasePoolDT<long>
{
    private int _iTempleteId;
    private int m_iDayNum;
    public int ITempleteId
    {
        get
        {
            return _iTempleteId;
        }

        set
        {
            _iTempleteId = value;
            m_EveryDayHotSaleDT = glo_Main.GetInstance().m_SC_Pool.m_SevenActivityTaskSC.f_GetSC(_iTempleteId) as EveryDayHotSaleDT;
        }
    }
    public short BuyTime;
    public int IDayNum
    {
        set
        {
            _iData1 = value;
            m_iDayNum = value;
        }
    }

    public EveryDayHotSaleDT m_EveryDayHotSaleDT;
}

