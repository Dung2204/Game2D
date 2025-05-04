using UnityEngine;
using System.Collections;
using ccU3DEngine;


/// <summary>
/// 卡牌碎片PoolDT
/// </summary>
public class CardFragmentPoolDT : BasePoolDT<long>
{
    public int m_iNum = 0;              // 数量

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
                m_CardFragmentDT = (CardFragmentDT)glo_Main.GetInstance().m_SC_Pool.m_CardFragmentSC.f_GetSC(_iTempleteId);
            }
        }
    }

    public CardFragmentDT m_CardFragmentDT;
}
