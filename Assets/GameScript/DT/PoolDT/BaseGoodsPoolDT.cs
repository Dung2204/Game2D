using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;



/// <summary>
/// 基础物品PoolDT
/// </summary>
public class BaseGoodsPoolDT : BasePoolDT<long>
{
    public int m_iNum;              // 数量

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
                m_BaseGoodsDT = (BaseGoodsDT)glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(_iTempleteId);
            }
        }
    }

    public BaseGoodsDT m_BaseGoodsDT;
       //设置IData5为模版ID
    public void SetIData5(int IData)
    {
        base._iData5 = IData;
    }
}
