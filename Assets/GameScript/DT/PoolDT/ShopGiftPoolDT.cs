using UnityEngine;
using System.Collections;
using ccU3DEngine;
public class ShopGiftPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 购买次数
    /// </summary>
    public int m_buyTimes;
    /// <summary>
    /// 表数据
    /// </summary>
    public ShopGiftDT m_shopGiftDT;
}
