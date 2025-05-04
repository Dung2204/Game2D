
//============================================
//
//    OpenServFund来自OpenServFund.xlsx文件自动生成脚本
//    2017/4/26 14:42:29
//    
//
//============================================
using System;
using System.Collections.Generic;



public class OpenServFundDT : NBaseSCDT
{

    /// <summary>
    /// 活动类型（1，开服基金  2，全民福利）
    /// </summary>
    public int iActType;
    /// <summary>
    /// 活动名称
    /// </summary>
    public string _szActName;
    public string szActName
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szActName);
        }
    }
    /// <summary>
    /// 达成条件描述
    /// </summary>
    public string _szActContext;
    public string szActContext
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szActContext);
        }
    }
    /// <summary>
    /// 等级;购买人数
    /// </summary>
    public int iCondiction;
    /// <summary>
    /// 道具表ID
    /// </summary>
    public int iGiftTabID;
    /// <summary>
    /// 道具ID
    /// </summary>
    public int iGiftID;
    /// <summary>
    /// 道具数量
    /// </summary>
    public int iGiftCount;
}
