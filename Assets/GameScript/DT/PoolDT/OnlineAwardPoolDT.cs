using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 在线奖励poolDT
/// </summary>
public class OnlineAwardPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 在线时长，分钟
    /// </summary>
    public int mTime;
    /// <summary>
    /// 是否领取奖励
    /// </summary>
    public bool m_isGet;
    /// <summary>
    /// 在线奖励
    /// </summary>
    public OnlineAwardDT m_OnlineAwardDT;
}
