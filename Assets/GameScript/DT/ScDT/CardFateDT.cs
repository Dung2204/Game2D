
//============================================
//
//    CardFate来自CardFate.xlsx文件自动生成脚本
//    2017/4/21 12:04:38
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardFateDT : NBaseSCDT
{

    /// <summary>
    /// 卡牌名称
    /// </summary>
    public string _szName;
    public string szName
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szName);
        }
    }
    /// <summary>
    /// 卡牌品质范围上限
    /// </summary>
    public int iCardQualityLower;
    /// <summary>
    /// 卡牌品质范围下限
    /// </summary>
    public int iCardQualityUpper;
    /// <summary>
    /// 缘分ID
    /// </summary>
    public string szFateId;
}
