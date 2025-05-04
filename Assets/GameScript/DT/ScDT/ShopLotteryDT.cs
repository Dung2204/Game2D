
//============================================
//
//    ShopLottery来自ShopLottery.xlsx文件自动生成脚本
//    2017/3/22 20:26:42
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopLotteryDT : NBaseSCDT
{

    /// <summary>
    /// 免费抽奖间隔(分钟)
    /// </summary>
    public int iFreeCD;
    /// <summary>
    /// 单次消耗类型1
    /// </summary>
    public string szOnceCost1;
    /// <summary>
    /// 单次消耗类型2
    /// </summary>
    public string szOnceCost2;
    /// <summary>
    /// 十次消耗类型1
    /// </summary>
    public string szTenCost1;
    /// <summary>
    /// 十次消耗类型2
    /// </summary>
    public string szTenCost2;
    /// <summary>
    /// 奖池id
    /// </summary>
    public int iPoolId;
    /// <summary>
    /// 节点奖池id
    /// </summary>
    public int iExtraPoolId;
    /// <summary>
    /// quay tùy chọn: 0. k cho phép , 1.cho phép
    /// </summary>
    public int iChoose;
    /// <summary>
    /// số lần quay yêu cầu để mở
    /// </summary>
    public int iOpenNum;
    /// <summary>
    /// danh sách item
    /// </summary>
    public string szItems;
}
