using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
public class TreasureFragmentPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 数量
    /// </summary>
    public int m_num;

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
                _iData1 = value;
                _iTempleteId = value;
                m_TreasureFragmentsDT = (TreasureFragmentsDT)glo_Main.GetInstance().m_SC_Pool.m_TreasureFragmentsSC.f_GetSC(_iTempleteId);
            }
        }
    }

    public TreasureFragmentsDT m_TreasureFragmentsDT;
}

