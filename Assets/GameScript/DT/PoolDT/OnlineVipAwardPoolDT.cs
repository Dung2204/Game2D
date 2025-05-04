using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

class OnlineVipAwardPoolDT : BasePoolDT<long>
{
    private int _iTempleteId;

    public int ITempleteId
    {
        get
        {
            return _iTempleteId;
        }

        set
        {
            _iTempleteId = value;
            m_OnlineVipAwardDT = glo_Main.GetInstance().m_SC_Pool.m_OnlineVipAwardSC.f_GetSC(_iTempleteId) as OnlineVipAwardDT;
        }
    }

    public byte isFinsh;
    public byte isGain;   


    public OnlineVipAwardDT m_OnlineVipAwardDT;
}

