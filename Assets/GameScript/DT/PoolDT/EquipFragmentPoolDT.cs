using UnityEngine;
using System.Collections;
using ccU3DEngine;



/// <summary>
/// 装备PoolDT
/// </summary>
public class EquipFragmentPoolDT : BasePoolDT<long>
{
    public int m_iNum;

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
                m_EquipFragmentsDT = (EquipFragmentsDT)glo_Main.GetInstance().m_SC_Pool.m_EquipFragmentsSC.f_GetSC(_iTempleteId);
            }
        }
    }

    public EquipFragmentsDT m_EquipFragmentsDT;
}
