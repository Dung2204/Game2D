using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 商店PoolDT
/// </summary>
public class ShopStorePoolDT : BasePoolDT<long> {
    /// <summary>
    /// 商店类型
    /// </summary>
    public EM_ShopType eShopShop;
    /// <summary>
    /// 今日道具刷新次数
    /// </summary>
    public short propFreshTimes;
    /// <summary>
    /// 免费刷新次数
    /// </summary>
    public short freeTimes;
    /// <summary>
    /// 上次免费刷新次数回复时间
    /// </summary>
    public int lastTime;
    /// <summary>
    /// 十进制掩码从高至低 如110000表示前2个商品已经购买
    /// </summary>
    public int buyMask;    // 十进制掩码从高至低 如110000表示前2个商品已经购买
    /// <summary>
    /// 数组为6的数据
    /// 当商店类型为神将商店时，为ShopRandGoodDT
    /// 当商店类型为领悟商店时，为
    /// </summary>
    public NBaseSCDT[] m_shopRandItemDTArray = new NBaseSCDT[6];
}
