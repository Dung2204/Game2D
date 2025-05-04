using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 军团商店PoolDT
/// </summary>
public class ShopLegionPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 购买次数
    /// </summary>
    public int buyTimes;
    /// <summary>
    /// 数据DT
    /// </summary>
    public LegionShopDT m_LegionShopDT;
}
