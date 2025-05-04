using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 十万元宝是否已经领取
/// </summary>
public class TenSyceePoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 是否已经领取
    /// </summary>
    public bool mIsGet;
    /// <summary>
    /// 表数据DT
    /// </summary>
    public SyceeAwardDT mSyceeAwardDT;
}
