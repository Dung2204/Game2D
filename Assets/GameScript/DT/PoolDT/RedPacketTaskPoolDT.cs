using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 红包任务数据
/// </summary>
public class RedPacketTaskPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 已经奖励次数
    /// </summary>
    public int mHasGetCount;
    /// <summary>
    /// 任务进度
    /// </summary>
    public int mProgress;
    // DT
    public RedPacketTaskDT mRedPacketTaskDT;
}
