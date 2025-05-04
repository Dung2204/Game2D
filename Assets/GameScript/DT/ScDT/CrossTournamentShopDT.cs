
//============================================
//
//    CrossTournamentShop.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CrossTournamentShopDT : NBaseSCDT
{

    /// <summary>
    /// 显示等级
    /// </summary>
    public int iShowLv;
    /// <summary>
    /// 出售商品的资源类型
    /// </summary>
    public int iResourceType;
    /// <summary>
    /// 出售商品的资源Id
    /// </summary>
    public int iResourceId;
    /// <summary>
    /// 出售商品的资源数量
    /// </summary>
    public int iResourceNum;
    /// <summary>
    /// 显示位置
    /// </summary>
    public int iShowIdx;
    /// <summary>
    /// 购买次数限制
    /// </summary>
    public int iBuyTimesLimit;
    /// <summary>
    /// 购买所需的积分(跨服战的货币)
    /// </summary>
    public int iNeedScore;
}
