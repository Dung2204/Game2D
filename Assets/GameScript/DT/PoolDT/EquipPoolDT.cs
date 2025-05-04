using UnityEngine;
using System.Collections;
using ccU3DEngine;



/// <summary>
/// 装备PoolDT
/// </summary>
public class EquipPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 星数
    /// </summary>
    public byte m_sstars;
    /// <summary>
    /// 强化等级
    /// </summary>
    public int m_lvIntensify
    {
        get
        {
            return iData3;
        }
        set
        {
            _iData3 = value;
        }
    }
    /// <summary>
    /// 精炼等级
    /// </summary>
    public int m_lvRefine
    {
        get
        {
            return iData2;
        }
        set
        {
            _iData2 = value;
        }
    }

    /// <summary>
    /// 精炼经验
    /// </summary>
    public int m_iexpRefine;
    /// <summary>
    /// 幸运值
    /// </summary>
    public short m_slucky;
    /// <summary>
    /// 升星经验
    /// </summary>
    public short m_sexpStars;
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
                m_EquipDT = (EquipDT)glo_Main.GetInstance().m_SC_Pool.m_EquipSC.f_GetSC(_iTempleteId);

                _iData1 = m_EquipDT.iColour;
                _iData4 = m_EquipDT.iId;
            }
        }
    }

    public EquipDT m_EquipDT;

    public SetEquipPoolDT m_SetEquip;    //套装   ,   调用先用TeamPool.f_UpdateSetEquip来刷新



}

