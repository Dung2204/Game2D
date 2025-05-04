
//============================================
//
//    HalfDiscount来自HalfDiscount.xlsx文件自动生成脚本
//    2017/11/23 19:01:05
//    
//
//============================================
using System;
using System.Collections.Generic;



public class HalfDiscountDT : NBaseSCDT
{

    /// <summary>
    /// 描述
    /// </summary>
    public string _szDesc;
    public string szDesc
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szDesc);
        }
    }
    /// <summary>
    /// 类型
    /// </summary>
    public int iResType;
    /// <summary>
    /// 出售ID
    /// </summary>
    public int iResId;
    /// <summary>
    /// 出售道具数量
    /// </summary>
    public int iResNum;
    /// <summary>
    /// 可购买次数
    /// </summary>
    public int iBuyNum;
    /// <summary>
    /// 消耗数量（写死元宝）
    /// </summary>
    public int iCostNum;
    /// <summary>
    /// 折扣
    /// </summary>
    public int iDiscount;
}
