
//============================================
//
//    RebelArmy来自RebelArmy.xlsx文件自动生成脚本
//    2017/5/11 11:24:19
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RebelArmyDT : NBaseSCDT
{

    /// <summary>
    /// 怪物名称
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
    /// 怪物类型：1，病毒入侵小怪。2，病毒魔王。3，魔王小弟
    /// </summary>
    public int iType;
    /// <summary>
    /// 怪物品质：1，白。2，绿。3，蓝。4，紫。5，橙。6，红。7，金。
    /// </summary>
    public int iImporent;
    /// <summary>
    /// 怪物等级
    /// </summary>
    public int iLv;
    /// <summary>
    /// 攻击
    /// </summary>
    public int iAtk;
    /// <summary>
    /// 生命
    /// </summary>
    public long lHp;
    /// <summary>
    /// 物防
    /// </summary>
    public int iPhyDef;
    /// <summary>
    /// 法防
    /// </summary>
    public int iMagDef;
    /// <summary>
    /// 命中率
    /// </summary>
    public int iHitR;
    /// <summary>
    /// 闪避率
    /// </summary>
    public int iDodgeR;
    /// <summary>
    /// 暴击率
    /// </summary>
    public int iCritR;
    /// <summary>
    /// 抗爆率
    /// </summary>
    public int iAntiknockR;
    /// <summary>
    /// 怪物描述
    /// </summary>
    public string _szReadme;
    public string szReadme
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szReadme);
        }
    }
}
