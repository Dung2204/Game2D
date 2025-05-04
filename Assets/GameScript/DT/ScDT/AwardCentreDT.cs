
//============================================
//
//    AwardCentre来自AwardCentre.xlsx文件自动生成脚本
//    2017/4/5 19:31:32
//    
//
//============================================
using System;
using System.Collections.Generic;



public class AwardCentreDT : NBaseSCDT
{

    /// <summary>
    /// 标题
    /// </summary>
    public string _szTitle;
    public string szTitle
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szTitle);
        }
    }
    /// <summary>
    /// 描述文字
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
    /// 备注
    /// </summary>
    public string szNote;
}
