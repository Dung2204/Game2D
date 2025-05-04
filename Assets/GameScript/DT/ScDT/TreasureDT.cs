
//============================================
//
//    Treasure来自Treasure.xlsx文件自动生成脚本
//    2017/3/24 13:29:09
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TreasureDT : NBaseSCDT
{

    /// <summary>
    /// 法宝名字
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
    /// 法宝图标
    /// </summary>
    public int iIcon;
    /// <summary>
    /// 法宝品质
    /// </summary>
    public int iImportant;
    /// <summary>
    /// 法宝描述
    /// </summary>
    public string _szDescribe;
    public string szDescribe
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szDescribe);
        }
    }
    /// <summary>
    /// 法宝部位
    /// </summary>
    public int iSite;
    /// <summary>
    /// 强化属性Id
    /// </summary>
    public int iIntenProId1;
    /// <summary>
    /// 强化初始值
    /// </summary>
    public int iStartPro1;
    /// <summary>
    /// 强化升级值
    /// </summary>
    public int iAddPro1;
    /// <summary>
    /// 强化属性Id
    /// </summary>
    public int iIntenProId2;
    /// <summary>
    /// 强化初始值
    /// </summary>
    public int iStartPro2;
    /// <summary>
    /// 强化升级值
    /// </summary>
    public int iAddPro2;
    /// <summary>
    /// 精炼属性Id1
    /// </summary>
    public int iRefinProId1;
    /// <summary>
    /// 每级属性值1
    /// </summary>
    public int iRefinPro1;
    /// <summary>
    /// 精炼属性Id2
    /// </summary>
    public int iRefinProId2;
    /// <summary>
    /// 每级属性值2
    /// </summary>
    public int iRefinPro2;
    /// <summary>
    /// 法宝经验值
    /// </summary>
    public int iExp;
    /// <summary>
    /// 排序
    /// </summary>
    public int iList;
}
