using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class FirstRechargePoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 领取次数
    /// </summary>
    public int mGetTimes;
    //TsuCode - FirstRechargeNew -NapDau
    public int received_1;
    public int received_2;
    public int received_3;
    public long timeAllReceived;
    public FirstRechargeDT m_FirstRechargeDT;//dt
}
