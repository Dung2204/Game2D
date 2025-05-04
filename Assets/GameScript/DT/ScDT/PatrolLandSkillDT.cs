
//============================================
//
//    PatrolLandSkill来自PatrolLandSkill.xlsx文件自动生成脚本
//    2017/9/6 10:38:14
//    
//
//============================================
using System;
using System.Collections.Generic;



public class PatrolLandSkillDT : NBaseSCDT
{

    /// <summary>
    /// 领地id
    /// </summary>
    public int iLandId;
    /// <summary>
    /// 等级
    /// </summary>
    public int iLv;
    /// <summary>
    /// 描述
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
    /// 触发几率
    /// </summary>
    public int iOdds;
    /// <summary>
    /// 奖励倍率
    /// </summary>
    public int iMulti;
    /// <summary>
    /// 需要巡逻累计时长
    /// </summary>
    public int iNeedTime;
    /// <summary>
    /// 消耗元宝数量
    /// </summary>
    public int iCostSycee;
}
