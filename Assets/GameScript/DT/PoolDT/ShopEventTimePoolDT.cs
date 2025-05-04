
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

class ShopEventTimePoolDT : BasePoolDT<long>
{
    private int _iEventTimeId;
    public int IEventTimeId
    {
        get
        {
            return _iEventTimeId;
        }

        set
        {
            _iEventTimeId = value;
            m_EventTimeDT = glo_Main.GetInstance().m_SC_Pool.m_ShopEventTimeSC.f_GetSC(_iEventTimeId) as ShopEventTimeDT;
        }
    }

    private bool m_isFinsh;
    public bool isFinsh
    {
        get
        {
            return m_isFinsh;
        }
        set
        {
            m_isFinsh = value;
        }

    }

    public int idata1;// start
    public int idata2;//end
    public int idata3;
    public int idata4;

    public ShopEventTimeDT m_EventTimeDT;

    public Dictionary<int, ShopEventTimeAwardPoolDT> m_AwardPoolDT;// = new Dictionary<int, ShopEventTimeAwardPoolDT>();
    //List<NBaseSCDT> _aList = null;
}

