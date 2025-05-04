using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 军团限时商店DT
/// </summary>
public class ShopTimeLimitPoolDT :  BasePoolDT<long>{
    /// <summary>
    /// 军团成员总的购买次数
    /// </summary>
    public int m_abuyTimes;
    /// <summary>
    /// 我的购买次数
    /// </summary>
    public int m_myBuyTimes;
    /// <summary>
    /// LegionShopTimeLimitDT
    /// </summary>
    public LegionShopTimeLimitDT m_LegionShopTimeLimitDT;
}
