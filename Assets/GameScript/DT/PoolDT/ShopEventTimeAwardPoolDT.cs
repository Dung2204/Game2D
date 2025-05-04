using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

class ShopEventTimeAwardPoolDT : BasePoolDT<long>
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
            m_AwardDT = glo_Main.GetInstance().m_SC_Pool.m_ShopEventTimeAwardSC.f_GetSC(_iAwardId) as ShopEventTimeAwardDT;
        }
    }
    // số lần mua gói
    public int idata1;// num
    public int idata2;
    public int idata3;
    public int idata4;

    public ShopEventTimeAwardDT m_AwardDT;
    public int zzIndex = 0;
}