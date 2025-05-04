
//============================================
//
//    LotteryLimitEvent.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LotteryLimitEventDT : NBaseSCDT
{

    /// <summary>
    /// id event
    /// </summary>
    public int iEventTime;
    /// <summary>
    /// cost 1 lần quay
    /// </summary>
    public string szOnceCost;
    /// <summary>
    /// cost 10 lần quay
    /// </summary>
    public string szTenCost;
    /// <summary>
    /// id quay
    /// </summary>
    public int iPoolId;
    /// <summary>
    /// card id
    /// </summary>
    public int iCardId;
    /// <summary>
    /// mốc 
    /// </summary>
    public int iNum;
    
}
