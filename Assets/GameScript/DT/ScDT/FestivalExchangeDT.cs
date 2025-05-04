
//============================================
//
//    FestivalExchange来自FestivalExchange.xlsx文件自动生成脚本
//    2018/5/21 17:42:57
//    
//
//============================================
using System;
using System.Collections.Generic;



public class FestivalExchangeDT : NBaseSCDT
{

    /// <summary>
    /// 本期活动内的（索引）
    /// </summary>
    public int ikey;
    /// <summary>
    /// 所需道具
    /// </summary>
    public string szResNeed;
    /// <summary>
    /// 兑换道具
    /// </summary>
    public string szResAward;
    /// <summary>
    /// 次数
    /// </summary>
    public int iCount;
    /// <summary>
    /// 刷新规则(0不刷新，1每日刷新）
    /// </summary>
    public int iRefresh;
    /// <summary>
    /// 开始时间
    /// </summary>
    public int iBeginTime;
    /// <summary>
    /// 结束时间（默认多加一天）
    /// </summary>
    public int iEndTime;
}
