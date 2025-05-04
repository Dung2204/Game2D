using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// 军团主控制
/// </summary>
public class LegionMain
{
    
    public LegionInfor m_LegionInfor;
    public LegionPlayerPool m_LegionPlayerPool;
    /// <summary>
    /// 军团商店pool(道具和奖励)
    /// </summary>
    public ShopLegionPool m_ShopLegionPool;
    /// <summary>
    /// 军团商店pool(限时)
    /// </summary>
    public ShopTimeLimitPool m_ShopTimeLimitPool;
    /// <summary>
    /// 军团技能pool
    /// </summary>
    public LegionSkillPool m_LegionSkillPool;
    /// <summary>
    /// 军团红包pool
    /// </summary>
    public LegionRedPacketPool m_LegionRedPacketPool;
    /// <summary>
    /// 军团排行pool
    /// </summary>
    public LegionRankPool m_LegionRankPool;

    /// <summary>
    /// 军团副本pool
    /// </summary>
    public LegionDungeonPool m_LegionDungeonPool;

    /// <summary>
    /// 军团战pool
    /// </summary>
    public LegionBattlePool m_LegionBattlePool;

    #region 单例
    /// <summary>
    /// 获取单例
    /// </summary>
    private static LegionMain _Instance = null;
    /// <summary>
    /// 获取单例
    /// </summary>
    /// <returns></returns>
    public static LegionMain GetInstance()
    {
        if (null == _Instance)
        {           
            _Instance = new LegionMain();
        }
        return _Instance;
    }
    public static LegionMain f_ClearData()
    {
        _Instance = new LegionMain();
        return _Instance;
    }
    #endregion

    public LegionMain()
    {
        f_InitPoolData();
    }

    public void f_InitPoolData()
    {
        m_LegionInfor = new LegionInfor();
        m_LegionPlayerPool = new LegionPlayerPool();
        m_ShopLegionPool = new ShopLegionPool();
        m_ShopTimeLimitPool = new ShopTimeLimitPool();
        m_LegionSkillPool = new LegionSkillPool();
        m_LegionRedPacketPool = new LegionRedPacketPool();
        m_LegionRankPool = new LegionRankPool();
        m_LegionDungeonPool = new LegionDungeonPool();
        m_LegionBattlePool = new LegionBattlePool();
    }
}