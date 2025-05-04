
//============================================
//
//    ShopEventTime.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopEventTimeDT : NBaseSCDT
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
    /// banner
    /// </summary>
    public string szBanner;
    /// <summary>
    /// type time
    /// </summary>
    public int iType;
    /// <summary>
    /// thời gian bắt đầu
    /// </summary>
    public int iOpenTime;
    /// <summary>
    /// thời gian kết thúc
    /// </summary>
    public int iEndTime;
    /// <summary>
    /// Open UI
    /// </summary>
    public string szNameConst;
    /// <summary>
    /// chuỗi gói quà
    /// </summary>
    public string szItems;
    /// <summary>
    /// o:k reset/ 1. reset hằng ngày
    /// </summary>
    public int iResetType;
    /// <summary>
    /// Poster
    /// </summary>
    public string szPoster;

}
