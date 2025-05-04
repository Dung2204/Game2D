using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

class SevenActivityTaskPoolDT : BasePoolDT<long>
{
    private int _iTempleteId;

    private int iDayNum;
    private int m_iPageNum;


    public int[] m_result;
    public int ITempleteId
    {
        get
        {
            return _iTempleteId;
        }

        set
        {
            _iTempleteId = value;
            m_SevenActivityTaskDT = glo_Main.GetInstance().m_SC_Pool.m_SevenActivityTaskSC.f_GetSC(_iTempleteId) as SevenActivityTaskDT;
            _iData3 = m_SevenActivityTaskDT.itype;
        }
    }

    public int IDayNum
    {
        get
        {
            return iDayNum;
        }

        set
        {
            iDayNum = value;
            _iData1 = iDayNum;
        }
    }
    public byte isFinsh;
    public byte isGain;   
    public int IPageNum
    {
        set
        {
            m_iPageNum = value;
            _iData2 = value;
        }
    }

    public SevenActivityTaskDT m_SevenActivityTaskDT;
}

