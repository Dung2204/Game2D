
//============================================
//
//    ShopEndowAward.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopEndowAwardDT : NBaseSCDT
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
    /// type reset: 0. k reset - 1. ngày - 2. tuần - 3. tháng
    /// </summary>
    public int iType;
    /// <summary>
    /// 
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
    /// 1.hot 0.k hot
    /// </summary>
    public int iHot;
    /// <summary>
    /// Open UI
    /// </summary>
    public string szAward;
    /// <summary>
    /// giá
    /// </summary>
    public int iMoney;
    /// <summary>
    /// free
    /// </summary>
    public int iFree;

}
