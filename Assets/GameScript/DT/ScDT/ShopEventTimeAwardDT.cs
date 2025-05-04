
//============================================
//
//    ShopEventTimeAward.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopEventTimeAwardDT : NBaseSCDT
{
    /// <summary>
    /// tên
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
    /// giá
    /// </summary>
    public string szCost;
    /// <summary>
    /// icon
    /// </summary>
    public string szIcon;
    /// <summary>
    /// giới hạn mua
    /// </summary>
    public int iLimit;
    /// <summary>
    /// trạng thái: 0. không hot 1. hot
    /// </summary>
    public int iHot;
    /// <summary>
    /// quà
    /// </summary>
    public string szAward;
    /// <summary>
    /// money
    /// </summary>
    public int iMoney;
    /// <summary>
    /// 
    /// </summary>
    public int iFree;
}
