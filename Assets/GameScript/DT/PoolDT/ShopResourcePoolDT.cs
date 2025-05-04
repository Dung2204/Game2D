using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class ShopResourcePoolDT : BasePoolDT<long>
{
    public ShopResourcePoolDT()
    {

    }

    public ShopResourcePoolDT(ShopResourceDT dt)
    {
        iId = dt.iId;
        _iTempleteId = dt.iId;
        m_iBuyTimes = 0; 
        m_ShopResourceDT = dt;
    }

    /// <summary>
    /// 累计购买次数
    /// </summary>
    public int m_iBuyTimes;	    

    /// <summary>
    /// 商店资源模版
    /// </summary>
    private int _iTempleteId;
    public int m_iTempleteId
    {
        get
        {
            return _iTempleteId;
        }

        set
        {
            if (_iTempleteId != value)
            {
                _iTempleteId = value;
                m_ShopResourceDT = (ShopResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ShopResourceSC.f_GetSC(_iTempleteId);
            }
        }
    }

    public ShopResourceDT m_ShopResourceDT;

}
