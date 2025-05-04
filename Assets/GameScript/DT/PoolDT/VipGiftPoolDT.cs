using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// vip礼包
/// </summary>
public class VipGiftPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 表数据DT
    /// </summary>
    public VipGiftDT mVipGiftDT;
    /// <summary>
    /// 今日是否领取
    /// </summary>
    public bool mToDayGet;
}
