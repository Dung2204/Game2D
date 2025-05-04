using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 等级礼包
/// </summary>
public class RankGiftPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 购买次数
    /// </summary>
    public int m_buyTimes;
    /// <summary>
    /// 升级时间
    /// </summary>
    public int m_levelTime;
    /// <summary>
    /// 礼包item
    /// </summary>
    public RankGiftDT m_RankGiftDT;
}
