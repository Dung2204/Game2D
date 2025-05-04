
//============================================
//
//    Monster来自Monster.xlsx文件自动生成脚本
//    2017/3/30 18:58:34
//    
//
//============================================
using System;
using System.Collections.Generic;



public class MonsterDT : NBaseSCDT
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
    /// 怪物品质
    /// </summary>
    public int iImportant;
    /// <summary>
    /// 卡牌阵营
    /// </summary>
    public int iCardCamp;
    /// <summary>
    /// 怒气
    /// </summary>
    public int iAnger;
    /// <summary>
    /// 攻击
    /// </summary>
    public int iAtk;
    /// <summary>
    /// 生命
    /// </summary>
    public int iHp;
    /// <summary>
    /// 物防
    /// </summary>
    public int iDef;
    /// <summary>
    /// 法防
    /// </summary>
    public int iMDef;
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
    /// 对应的模型Id
    /// </summary>
    public int iStatelId1;
    /// <summary>
    /// 模型技能
    /// </summary>
    public string szModelMagic;
    // nghề
    public int iCardFightType;
    // nguyên tố
    public int iCardEle;
}
