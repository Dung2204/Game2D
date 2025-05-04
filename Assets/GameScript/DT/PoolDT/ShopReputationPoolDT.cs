using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 声望商店PoolDT
/// </summary>
public class ShopReputationPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 购买次数
    /// </summary>
    public int buyTimes;
    /// <summary>
    /// 数据DT
    /// </summary>
    public ReputationShopDT m_ReputationShopDT;
}
