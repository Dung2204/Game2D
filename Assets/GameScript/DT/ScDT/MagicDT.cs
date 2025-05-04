
//============================================
//
//    Magic来自Magic.xlsx文件自动生成脚本
//    2017/3/28 14:53:07
//    
//
//============================================
using System;
using System.Collections.Generic;



public class MagicDT : NBaseSCDT
{

    /// <summary>
    /// 技能名称
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
    /// 技能等级
    /// </summary>
    public int iLv;
    /// <summary>
    /// 技能描述
    /// </summary>
    public string _szReadme;
    public string szReadme
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szReadme);
        }
    }
    /// <summary>
    /// 技能类别：1，普攻。2，怒技。3，合击。4，超合击
    /// </summary>
    public int iClass;
    /// <summary>
    /// 怒气消耗
    /// </summary>
    public int iMp;
    /// <summary>
    /// 合击发动英雄ID
    /// </summary>
    public int iGroupHero1;
    /// <summary>
    /// 合击配合英雄ID1
    /// </summary>
    public int iGroupHero2;
    /// <summary>
    /// 合击配合英雄ID2
    /// </summary>
    public int iGroupHero3;
    /// <summary>
    /// 发动进化等级
    /// </summary>
    public int iUseLv;
    /// <summary>
    /// 技能加持命中率
    /// </summary>
    public int iHit;
    /// <summary>
    /// 技能加持暴击率
    /// </summary>
    public int iCrit;
    /// <summary>
    /// 释放目标：1，友方2，敌方
    /// </summary>
    public int iTarget;
    /// <summary>
    /// 目标类型：1) 前排2) 前排单体3) 后排4) 后排单体5) 一列6) 全体7) 目标及相邻(上下左右)8) 目标及目标外随机x个9) 随机x个10) 生命最少的x个11) 怒气最高的x个12) 所有生命低于x%13) 自身
    /// </summary>
    public int iTargetPos;
    /// <summary>
    /// 攻击目标个数
    /// </summary>
    public int iTartgetNum;
    /// <summary>
    /// 基础效果类型1，物理伤害2，魔法伤害3，治疗（伤害和治疗效果=攻击*x+y）
    /// </summary>
    public int iType;
    /// <summary>
    /// 攻击加成
    /// </summary>
    public int iFp;
    /// <summary>
    /// 天命加成
    /// </summary>
    public int iFate;
    /// <summary>
    /// 化神加成
    /// </summary>
    public int iGod;
    /// <summary>
    /// 固定治疗
    /// </summary>
    public int iHp;
    /// <summary>
    /// 多段伤害（就是伤害的客户端表现，是飘血1次还是多次。比如计算的最终结果，卡牌需要扣血100点。2段伤害的表现，就是飘2个扣血50）
    /// </summary>
    public string szSpellDamage;
    /// <summary>
    /// 效果附加几率：万分率
    /// </summary>
    public int iExtRand1;
    /// <summary>
    /// 释放目标：1，友方2，敌方
    /// </summary>
    public int iExtTarget1;
    /// <summary>
    /// 目标类型：1) 前排2) 前排单体3) 后排4) 后排单体5) 一列6) 全体7) 目标及相邻8) 目标及目标外随机x个9) 随机x个10) 生命最少的x个11) 怒气最高的x个12) 所有生命低于x%13) 自身14) 技能命中目标
    /// </summary>
    public int iExtTargetPos1;
    /// <summary>
    /// 目标类型参数x
    /// </summary>
    public int iExtTartgetNum1;
    /// <summary>
    /// 效果类型1，物理伤害2，魔法伤害3，治疗（伤害和治疗效果=攻击*（x1+x2）+y）4，减少x怒气5，增加x怒气6，清除中毒和灼烧效果7，清除不利BUFF8，清除增益BUFF
    /// </summary>
    public int iExtType1;
    /// <summary>
    /// 额外效果类型1~3时，表示参数x1额外效果类型4~5时，表示参数x
    /// </summary>
    public int iExtData11;
    /// <summary>
    /// 额外效果类型1~3时，表示参数x2
    /// </summary>
    public int iExtData12;
    /// <summary>
    /// 效果附加几率：万分率
    /// </summary>
    public int iExtRand2;
    /// <summary>
    /// 释放目标：1，友方2，敌方
    /// </summary>
    public int iExtTarget2;
    /// <summary>
    /// 目标类型：1) 前排2) 前排单体3) 后排4) 后排单体5) 一列6) 全体7) 目标及相邻8) 目标及目标外随机x个9) 随机x个10) 生命最少的x个11) 怒气最高的x个12) 所有生命低于x%13) 自身14) 技能命中目标
    /// </summary>
    public int iExtTargetPos2;
    /// <summary>
    /// 攻击目标个数
    /// </summary>
    public int iExtTargetNum2;
    /// <summary>
    /// 额外效果类型1，物理伤害2，魔法伤害3，治疗（伤害和治疗效果=攻击*（x1+x2）+y）4，减少x怒气5，增加x怒气6，清除中毒和灼烧效果7，清除不利BUFF8，清除增益BUFF
    /// </summary>
    public int iExtType2;
    /// <summary>
    /// 额外效果类型1~3时，表示参数x1额外效果类型4~5时，表示参数x
    /// </summary>
    public int iExtData21;
    /// <summary>
    /// 额外效果类型1~3时，表示参数x2
    /// </summary>
    public int iExtData22;
    /// <summary>
    /// 附加BUFFID
    /// </summary>
    public int iBufId1;
    /// <summary>
    /// BUFF附加几率：万分率
    /// </summary>
    public int iBufRand1;
    /// <summary>
    /// 释放目标：1，友方2，敌方
    /// </summary>
    public int iBufTarget1;
    /// <summary>
    /// 目标类型：1) 前排2) 前排单体3) 后排4) 后排单体5) 一列6) 全体7) 目标及相邻8) 目标及目标外随机x个9) 随机x个10) 生命最少的x个11) 怒气最高的x个12) 所有生命低于x%13) 自身14) 技能命中目标
    /// </summary>
    public int iBufTargetPos1;
    /// <summary>
    /// 攻击目标个数
    /// </summary>
    public int iBufTargetNum1;
    /// <summary>
    /// 持续回合数
    /// </summary>
    public int iBufLive1;
    /// <summary>
    /// 附加BUFFID
    /// </summary>
    public int iBufId2;
    /// <summary>
    /// BUFF附加几率：万分率
    /// </summary>
    public int iBufRand2;
    /// <summary>
    /// 释放目标：1，友方2，敌方
    /// </summary>
    public int iBufTarget2;
    /// <summary>
    /// 目标类型：1) 前排2) 前排单体3) 后排4) 后排单体5) 一列6) 全体7) 目标及相邻8) 目标及目标外随机x个9) 随机x个10) 生命最少的x个11) 怒气最高的x个12) 所有生命低于x%13) 自身14) 技能命中目标
    /// </summary>
    public int iBufTargetPos2;
    /// <summary>
    /// 攻击目标个数
    /// </summary>
    public int iBufTargetNum2;
    /// <summary>
    /// 持续回合数
    /// </summary>
    public int iBufLive2;
}
