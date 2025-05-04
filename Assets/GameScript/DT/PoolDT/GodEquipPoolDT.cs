using UnityEngine;
using System.Collections;
using ccU3DEngine;



/// <summary>
/// 装备PoolDT
/// </summary>
public class GodEquipPoolDT : BasePoolDT<long>
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
                m_EquipDT = (GodEquipDT)glo_Main.GetInstance().m_SC_Pool.m_GodEquipSC.f_GetSC(_iTempleteId);

                _iData1 = m_EquipDT.iColour;
                _iData4 = m_EquipDT.iId;

                m_GodEquipSkillDT = (GodEquipSkillDT)glo_Main.GetInstance().m_SC_Pool.m_GodEquipSkillSC.f_GetSC(m_EquipDT.idSkillGod);
                m_MagicDT = (MagicDT)glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(m_GodEquipSkillDT.iMagicId);
            }
        }
    }

    public GodEquipDT m_EquipDT;
    public GodEquipSkillDT m_GodEquipSkillDT;
    public MagicDT m_MagicDT;

    public SetGodEquipPoolDT m_SetEquip;    //套装   ,   调用先用TeamPool.f_UpdateSetEquip来刷新



}

