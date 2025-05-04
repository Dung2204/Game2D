using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 红包兑换数据
/// </summary>
public class RedPacketExchangePoolDT : BasePoolDT<long> {
    /// <summary>
    /// 已经兑换次数
    /// </summary>
    public int mHasExchangeCount;
    // DT
    public RedPacketExchangeDT mRedPacketExChangeDT;
}
