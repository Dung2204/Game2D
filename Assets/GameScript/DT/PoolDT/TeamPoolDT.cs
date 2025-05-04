using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;

public class TeamPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 放置位置 0,1,2,3,4,5
    /// </summary>
    public EM_FormationPos m_eFormationPos;

    public EM_FormationSlot m_eFormationSlot;

    private long _iCardId;
    /// <summary>
    /// 出占卡牌
    /// </summary>    
    public long m_iCardId
    {
        get
        {
            return _iCardId;
        }
        set
        {
            if (_iCardId != value)
            {
                _iCardId = value;
                _CardPoolDT = (CardPoolDT)Data_Pool.m_CardPool.f_GetForId(_iCardId);
            }
        }
    }

    private CardPoolDT _CardPoolDT;
    /// <summary>
    /// 角色卡牌模版
    /// </summary>
    public CardPoolDT m_CardPoolDT
    {
        get
        {
            return _CardPoolDT;
        }
    }

    /// <summary>
    /// 装备位与EM_EquipPart枚举相对应
    /// </summary>
    public long[] m_aEqupId = new long[(int)EM_EquipPart.eEquipPart_INVALID - 1];

    /// <summary>
    /// 保存装备的数据结构不存在为null
    /// </summary>
    public EquipPoolDT[] m_aEquipPoolDT = new EquipPoolDT[4];
    /// <summary>
    /// 保存法宝的数据结构不存在为null
    /// </summary>
    public TreasurePoolDT[] m_aTreamPoolDT = new TreasurePoolDT[2];


    public GodEquipPoolDT[] m_aGodEquipPoolDT = new GodEquipPoolDT[1];
    /// <summary>
    /// 获取该卡牌对应位置的装备
    /// </summary>
    /// <param name="eEquip">装备部位类型</param>
    /// <returns></returns>
    public EquipPoolDT f_GetEquipPoolDT(EM_Equip eEquip)
    {
        return m_aEquipPoolDT[(int)eEquip - 1];
    }
    public long f_GetEquipPoolDTId(EM_Equip eEquip)
    {
        if (f_GetEquipPoolDT(eEquip) == null)
            return 0;
        else
            return m_aEquipPoolDT[(int)eEquip - 1].m_EquipDT.iId;
    }
    /// <summary>
    /// 获取该卡牌对应位置的法宝
    /// </summary>
    /// <param name="eTreasure">法宝部位类型</param>
    /// <returns></returns>
    public TreasurePoolDT f_GetTreasurePoolDT(EM_Treasure eTreasure)
    {
        return m_aTreamPoolDT[(int)eTreasure - 5];
    }

    public GodEquipPoolDT f_GetGodEquipPoolDT(EM_GodEquip eEquip)
    {
        return m_aGodEquipPoolDT[(int)eEquip - 7];
    }
}
