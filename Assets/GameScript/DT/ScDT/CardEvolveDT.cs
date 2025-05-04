
//============================================
//
//    CardEvolve来自CardEvolve.xlsx文件自动生成脚本
//    2017/5/22 11:11:18
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardEvolveDT : NBaseSCDT
{

    /// <summary>
    /// 进化等级
    /// </summary>
    public int iEvoLv;
    /// <summary>
    /// 卡牌等级需求
    /// </summary>
    public int iNeedLv;
    /// <summary>
    /// 下一级应对Id
    /// </summary>
    public int iNextLvId;
    /// <summary>
    /// 游戏币
    /// </summary>
    public int iMoney;
    /// <summary>
    /// 进化丹
    /// </summary>
    public int iEvolvePill;
    /// <summary>
    /// 自身卡牌数量
    /// </summary>
    public int iNeedCardNum;
    /// <summary>
    /// 天赋名称
    /// </summary>
    public string _szTalentName;
    public string szTalentName
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szTalentName);
        }
    }
    /// <summary>
    /// 天赋描述
    /// </summary>
    public string _szTalentDescribe;
    public string szTalentDescribe
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szTalentDescribe);
        }
    }
    /// <summary>
    /// 天赋属性库ID
    /// </summary>
    public int iTalentId;
    /// <summary>
    /// 备注
    /// </summary>
    public string _szRemark;
    public string szRemark
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szRemark);
        }
    }
    /// <summary>
    /// 形态ID
    /// </summary>
    public int iTypeId;
}
