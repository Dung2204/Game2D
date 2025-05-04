using UnityEngine;
using System.Collections;
using ccU3DEngine;

/// <summary>
/// 签到
/// </summary>
public class SignPoolDT : BasePoolDT<long> {
    /// <summary>
    /// 签到类型
    /// </summary>
    public SignType m_ActivityType;//签到类型
    /// <summary>
    /// 子类型
    /// 对应Signed里的iType 1.（服务器开服时必须签到完）
    /// </summary>
    public int signSubType;//子类型 
    /// <summary>
    /// 已经签到过的数量
    /// </summary>
    public int signedCount;//已经签到过的数量
    /// <summary>
    /// 当天是否已经签到过
    /// </summary>
    public bool isSignToday;//当天是否已经签到过
}
