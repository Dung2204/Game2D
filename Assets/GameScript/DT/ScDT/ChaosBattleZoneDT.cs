
//============================================
//
//    CrossServerBattleZone来自CrossServerBattleZone.xlsx文件自动生成脚本
//    2018/3/22 19:30:02
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ChaosBattleZoneDT : NBaseSCDT
{

    /// <summary>
    /// 战区名字
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
    /// 战区开始战力[
    /// </summary>
    public int iBeginPower;
    /// <summary>
    /// 战区结算战力]
    /// </summary>
    public int iEndPower;
    /// <summary>
    /// 战胜奖励积分
    /// </summary>
    public int iWinAward;
    /// <summary>
    /// 战败奖励积分
    /// </summary>
    public int iLoseAward;
    /// <summary>
    /// 连胜奖励积分(连胜场次*X)
    /// </summary>
    public int iWinSteakAward;
}
