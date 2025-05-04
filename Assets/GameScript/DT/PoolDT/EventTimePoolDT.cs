using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

class EventTimePoolDT : BasePoolDT<long>
{
    private int _iEventId;
    public int IEventId
    {
        get
        {
            return _iEventId;
        }

        set
        {
            _iEventId = value;
           // m_VoteAppDT = glo_Main.GetInstance().m_SC_Pool.m_VoteAppSC.f_GetSC(_iEventId) as VoteAppDT;
        }
    }

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
            m_EventTimeDT = glo_Main.GetInstance().m_SC_Pool.m_EventTimeSC.f_GetSC(_iEventTimeId) as EventTimeDT;
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

    public int idata1;
    public int idata2;
    public int idata3;
    public int idata4;

    public EventTimeDT m_EventTimeDT;

    //public VoteAppDT m_VoteAppDT;
}

