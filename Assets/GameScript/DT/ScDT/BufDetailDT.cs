
//============================================
//
//    BufDetail.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;



public class BufDetailDT : NBaseSCDT
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
    public string _szDesc;
    public string szDesc
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szDesc);
        }
    }
    
    public string icon;
}
