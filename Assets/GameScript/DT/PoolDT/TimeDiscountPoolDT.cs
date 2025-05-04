using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 限时折扣item
/// </summary>
public class TimeDiscountPoolDT : BasePoolDT<long> {
    /// <summary>
    /// 购买次数
    /// </summary>
    public int m_BuyTimes;
    /// <summary>
    /// 限时折扣DT
    /// </summary>
    public DiscountPropDT m_DiscountPropDT;
}
