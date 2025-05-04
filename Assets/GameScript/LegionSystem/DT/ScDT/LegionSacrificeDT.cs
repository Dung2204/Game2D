
//============================================
//
//    LegionSacrifice来自LegionSacrifice.xlsx文件自动生成脚本
//    2017/5/23 12:00:28
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LegionSacrificeDT : NBaseSCDT
{

    /// <summary>
    /// 名称
    /// </summary>
    public string szName;
    /// <summary>
    /// 祭天类型
    /// </summary>
    public int iSacrificeType;
    /// <summary>
    /// 消耗货币ID
    /// </summary>
    public int iCostType;
    /// <summary>
    /// 消耗数量
    /// </summary>
    public int iCostCount;
    /// <summary>
    /// 祭天进度
    /// </summary>
    public int iSacrificeNum;
    /// <summary>
    /// 军团贡献数量
    /// </summary>
    public int iSacrificeContributeNum;
    /// <summary>
    /// 军团经验数量
    /// </summary>
    public int iSacrificeExpNum;
    /// <summary>
    /// 祭天奖励翻倍值
    /// </summary>
    public int iSacrificeCritNum;
    /// <summary>
    /// 祭天奖励翻倍几率
    /// </summary>
    public int iSacrificeCritOdds;
    /// <summary>
    /// 祭天npc模型
    /// </summary>
    public int iNpc;
    /// <summary>
    /// 未祭天时npc泡泡
    /// </summary>
    public string szNotText;
    /// <summary>
    /// 祭天npc泡泡
    /// </summary>
    public string _szAlreadyText;

    public string szAlreadyText
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szAlreadyText);
        }
    }
}
