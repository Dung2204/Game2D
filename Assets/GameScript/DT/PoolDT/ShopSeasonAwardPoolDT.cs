
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

class ShopSeasonAwardPoolDT : BasePoolDT<long>
{
    private int _iAwardId;
    public int IAwardId
    {
        get
        {
            return _iAwardId;
        }

        set
        {
            _iAwardId = value;
            m_PoolDT = glo_Main.GetInstance().m_SC_Pool.m_ShopSeasonAwardSC.f_GetSC(_iAwardId) as ShopSeasonAwardDT;
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

    public int idata1;// limit
    public int idata2;
    public int idata3;
    public int idata4;
    public int zzIndex = 0;
    public ShopSeasonAwardDT m_PoolDT;

}

