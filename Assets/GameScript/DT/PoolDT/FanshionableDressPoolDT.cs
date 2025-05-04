using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class FanshionableDressPoolDT : BasePoolDT<long>
{
    public FanshionableDressPoolDT()
    {
        _FashionableDressDT = null;
    }

    /// <summary>
    /// 模板Id
    /// </summary>
    public int m_iTempId;   

    /// <summary>
    /// 过期时间戳
    /// </summary>
    public int m_iLimitTime;

    /// <summary>
    /// 当前装备此时装的卡片Id (>0表示时装已装备)
    /// </summary>
    public long m_iCaridId;

    private FashionableDressDT _FashionableDressDT;
    public FashionableDressDT m_FashionableDressDT
    {
        get
        {
            if (_FashionableDressDT == null)
            {
                _FashionableDressDT = (FashionableDressDT)glo_Main.GetInstance().m_SC_Pool.m_FashionableDressSC.f_GetSC(m_iTempId);
            }
            return _FashionableDressDT;
        }
    }

}