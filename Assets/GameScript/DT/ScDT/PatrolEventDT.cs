
//============================================
//
//    PatrolEvent来自PatrolEvent.xlsx文件自动生成脚本
//    2017/9/6 10:37:54
//    
//
//============================================
using System;
using System.Collections.Generic;



public class PatrolEventDT : NBaseSCDT
{

    /// <summary>
    /// 事件描述
    /// </summary>
    public string _szDes;
    public string szDes
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szDes);
        }
    }
    /// <summary>
    /// 奖励id
    /// </summary>
    public int iAwardId;
    /// <summary>
    /// 翻倍几率
    /// </summary>
    public int iDoubleOdds;
    /// <summary>
    /// 领地1
    /// </summary>
    public int iLand1;
    /// <summary>
    /// 领地2
    /// </summary>
    public int iLand2;
    /// <summary>
    /// 领地3
    /// </summary>
    public int iLand3;
    /// <summary>
    /// 领地4
    /// </summary>
    public int iLand4;
    /// <summary>
    /// 领地5
    /// </summary>
    public int iLand5;
    /// <summary>
    /// 领地6
    /// </summary>
    public int iLand6;
}
