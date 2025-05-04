
//============================================
//
//    RankGift来自RankGift.xlsx文件自动生成脚本
//    2017/9/18 16:46:45
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RankGiftDT : NBaseSCDT
{

    /// <summary>
    /// 开放等级
    /// </summary>
    public int iOpenLevel;
    /// <summary>
    /// 显示位置
    /// </summary>
    public int iShowPos;
    /// <summary>
    /// 道具表ID
    /// </summary>
    public int iRewardType;
    /// <summary>
    /// 道具ID
    /// </summary>
    public int iRewardId;
    /// <summary>
    /// 道具数量
    /// </summary>
    public int iRewardCount;
    /// <summary>
    /// 购买价格（元宝）
    /// </summary>
    public int iBuyPrice;
    /// <summary>
    /// 购买次数
    /// </summary>
    public int iBuyTime;
    /// <summary>
    /// 折扣力度
    /// </summary>
    public int iDiscount;
    /// <summary>
    /// 描述
    /// </summary>
    public string _szDescribe;
    public string szDescribe
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szDescribe);
        }
    }
    /// <summary>
    /// 标题
    /// </summary>
    public string _szTitile;
    public string szTitile
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szTitile);
        }
    }
    /// <summary>
    /// 前往
    /// </summary>
    public string szGetWayGoPage;
    /// <summary>
    /// 图标
    /// </summary>
    public int iIcon;
}
