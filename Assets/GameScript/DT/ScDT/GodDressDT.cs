
//============================================
//
//    GodDress来自GodDress.xlsx文件自动生成脚本
//    2018/4/11 15:30:38
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GodDressDT : NBaseSCDT
{

    /// <summary>
    /// 开始时间
    /// </summary>
    public int iBeginTime;
    /// <summary>
    /// 结束时间（领奖结束时间顺延一天）
    /// </summary>
    public int iEndTime;
    /// <summary>
    /// 活动奖励
    /// </summary>
    public string szAward;
    /// <summary>
    /// 购买奖励
    /// </summary>
    public int iBuyAward;
    /// <summary>
    /// 每日次数
    /// </summary>
    public int iBuyTimes;
    /// <summary>
    /// 买一次价格
    /// </summary>
    public int iOnePrice;
    /// <summary>
    /// 买10次价格
    /// </summary>
    public int iTenPrice;
    /// <summary>
    /// 奖励ID
    /// </summary>
    public int iRankAwardID;
    /// <summary>
    /// 宝箱ID
    /// </summary>
    public int iBoxAwardID;
    /// <summary>
    /// 描述
    /// </summary>
    public string szTheme;
}
