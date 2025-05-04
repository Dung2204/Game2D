
//============================================
//
//    DailyPveGate来自DailyPveGate.xlsx文件自动生成脚本
//    2017/10/13 10:46:20
//    
//
//============================================
using System;
using System.Collections.Generic;



public class DailyPveGateDT : NBaseSCDT
{

    /// <summary>
    /// 类型
    /// </summary>
    public int iType;
    /// <summary>
    /// 难度1名称
    /// </summary>
    public string _szLevelName1;
    public string szLevelName1
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szLevelName1);
        }
    }
    /// <summary>
    /// 难度1图标
    /// </summary>
    public string szLevelIcon1;
    /// <summary>
    /// 解锁主角等级
    /// </summary>
    public int iLevelLimit1;
    /// <summary>
    /// 难度1推荐战力
    /// </summary>
    public string szZhanLi1;
    /// <summary>
    /// 奖励
    /// </summary>
    public string szAward;
    /// <summary>
    /// 怪物ID1
    /// </summary>
    public int iMoster11;
    /// <summary>
    /// 怪物ID2
    /// </summary>
    public int iMoster12;
    /// <summary>
    /// 怪物ID3
    /// </summary>
    public int iMoster13;
    /// <summary>
    /// 怪物ID4
    /// </summary>
    public int iMoster14;
    /// <summary>
    /// 怪物ID5
    /// </summary>
    public int iMoster15;
    /// <summary>
    /// 怪物ID6
    /// </summary>
    public int iMoster16;
}
