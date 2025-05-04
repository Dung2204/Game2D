
//============================================
//
//    CrossServerBattleTitle来自CrossServerBattleTitle.xlsx文件自动生成脚本
//    2018/3/22 19:29:46
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CrossServerBattleTitleDT : NBaseSCDT
{

    /// <summary>
    /// 官衔星数
    /// </summary>
    public int iStarNum;
    /// <summary>
    /// 官衔名字
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
    /// 官衔结算奖励
    /// </summary>
    public int iAwardScore;
}
