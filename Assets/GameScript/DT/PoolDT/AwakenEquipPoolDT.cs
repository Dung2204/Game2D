using ccU3DEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AwakenEquipPoolDT : BasePoolDT<long>
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
                _iTempleteId = value;
                m_AwakenEquipDT = (AwakenEquipDT)glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(_iTempleteId);
            }
        }
    }

    public AwakenEquipDT m_AwakenEquipDT;

    public void SetData5(int data)
    {
        base._iData5 = data;
    }
}
