using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 军团红包排行poolDT
/// </summary>
public class LegionRedPacketRankPoolDT : BasePoolDT<long> {
    /// <summary>
    /// 玩家信息
    /// </summary>
    public BasePlayerPoolDT m_BasePlayerPoolDT;
    /// <summary>
    /// 玩家排名
    /// </summary>
    public int m_Rank;
    /// <summary>
    /// 抢到或者发放的元宝数
    /// </summary>
    public int m_SyceeCount;
}
