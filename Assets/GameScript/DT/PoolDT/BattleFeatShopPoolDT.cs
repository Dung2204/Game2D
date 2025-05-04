using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

class BattleFeatShopPoolDT : BasePoolDT<long>
{
    public EM_ShopMutiType m_Shop;

    private int _iTempleteId;

    public int m__iTempleteId
    {
        get { return _iTempleteId; }
        set
        {
            _iTempleteId = value;
            m_BattleFeatShopDT = glo_Main.GetInstance().m_SC_Pool.m_BattleFeatShopSC.f_GetSC(value) as BattleFeatShopDT;
        }
    }

    public BattleFeatShopDT m_BattleFeatShopDT;

}

