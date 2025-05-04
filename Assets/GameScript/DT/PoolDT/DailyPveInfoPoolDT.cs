using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 日常副本PoolDT
/// </summary>
public class DailyPveInfoPoolDT : BasePoolDT<long> {

    public int TodayPassTimes;//当天通关次数（每天只能打一次）
    
    public DailyPveInfoDT m_DailyPveInfoDT;
}
