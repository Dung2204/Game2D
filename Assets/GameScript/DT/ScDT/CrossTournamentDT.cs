
//============================================
//
//    CrossTournamentShop.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CrossTournamentDT : NBaseSCDT
{

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
