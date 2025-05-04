
//============================================
//
//    CrossTournamentShop.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CrossTournamentQualifyingRoundDT : NBaseSCDT
{

    /// <summary>
    /// rank bắt đầu
    /// </summary>
    public int iBeginNum;
    /// <summary>
    /// rank kết thúc
    /// </summary>
    public int iEndNum;
    /// <summary>
    /// name
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
    /// quà
    /// </summary>
    public string szAward;
}
