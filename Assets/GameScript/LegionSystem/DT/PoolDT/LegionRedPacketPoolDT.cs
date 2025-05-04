using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 军团红包PoolDT
/// </summary>
public class LegionRedPacketPoolDT : BasePoolDT<long> {
    /// <summary>
    /// 红包类型
    /// </summary>
    public EM_RedPacket em_redPacket;
    /// <summary>
    /// 发红包人
    /// </summary>
    public long m_PlayerId;
    /// <summary>
    /// 发红包人信息
    /// </summary>
    public BasePlayerPoolDT m_BasePlayerPoolDT;
    /// <summary>
    /// 已经领取红包的个数
    /// </summary>
    public int m_CurCount;
    /// <summary>
    /// 当前已经领取了的元宝数
    /// </summary>
    public int m_CurSycee;
    /// <summary>
    /// 我是否已经领取了该红包
    /// </summary>
    public bool m_MyIsGet;
}
