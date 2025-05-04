using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

public class TreasurePoolDT : BasePoolDT<long>
{

    /// <summary>
    /// 强化等级
    /// </summary>
    public short m_lvIntensify;
    /// <summary>
    /// 强化经验
    /// </summary>
    public int m_ExpIntensify;
    /// <summary>
    /// 叠加数量
    /// </summary>
    public int m_Num;
    /// <summary>
    /// 精炼等级
    /// </summary>
    public short m_lvRefine;
    private int _iTempleteId;

    /// <summary>
    /// 临时用数量
    /// </summary>
    public int m_iTempNum;
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
                m_TreasureDT = (TreasureDT)glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(_iTempleteId);
            }
        }
    }

    public TreasureDT m_TreasureDT;
}
