
//============================================
//
//    RecommendTeam来自RecommendTeam.xlsx文件自动生成脚本
//    2018/3/1 16:33:02
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RecommendTeamDT : NBaseSCDT
{

    /// <summary>
    /// 阵营
    /// </summary>
    public int iCardCamp;
    /// <summary>
    /// 名称
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
    /// 位置1
    /// </summary>
    public int iPos1;
    /// <summary>
    /// 位置2
    /// </summary>
    public int iPos2;
    /// <summary>
    /// 位置3
    /// </summary>
    public int iPos3;
    /// <summary>
    /// 位置4
    /// </summary>
    public int iPos4;
    /// <summary>
    /// 位置5
    /// </summary>
    public int iPos5;
    /// <summary>
    /// 位置6
    /// </summary>
    public int iPos6;
    /// <summary>
    /// 阵容描述
    /// </summary>
    public string _szDesc;
    public string szDesc
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szDesc);
        }
    }
}
