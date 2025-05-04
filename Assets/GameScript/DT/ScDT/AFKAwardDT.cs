
//============================================
//
//    AFKAward.xlsx
//    2022/02/23
//    
//
//============================================
using System;
using System.Collections.Generic;



public class AFKAwardDT : NBaseSCDT
{

    /// <summary>
    /// 奖励类型
    /// </summary>
    public int iAwardType;
    /// <summary>
    /// 奖励ID
    /// </summary>
    public int iAwardId;
    /// <summary>
    /// 奖励数量
    /// </summary>
    public int iAwardNum;
    /// <summary>
    /// 开启次数
    /// </summary>
    public int iOpenTimes;
    /// <summary>
    /// 是否清零
    /// </summary>
    public int ibClear;
    /// <summary>
    /// 权重
    /// </summary>
    public int iWeight;
    /// <summary>
    /// 是否广播
    /// </summary>
    public int ibBoardcast;
}
