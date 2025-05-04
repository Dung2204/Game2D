using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 开服基金
/// </summary>
public class OpenServFundPoolDT : BasePoolDT<long> {
    /// <summary>
    /// 类型
    /// 1.开服基金
    /// 2.全名福利
    /// </summary>
    public EM_OpenServFundType eOpenServFundType;
    /// <summary>
    /// 开服基金数据表item
    /// </summary>
    public OpenServFundDT m_OpenServFundDT;
    /// <summary>
    /// 购买次数
    /// </summary>
    public int m_buyTimes;
}
