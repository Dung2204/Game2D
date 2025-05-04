
//============================================
//
//    TaskAchievement来自TaskAchievement.xlsx文件自动生成脚本
//    2017/10/23 10:38:17
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TaskAchievementDT : NBaseSCDT
{

    /// <summary>
    /// 任务开启等级
    /// </summary>
    public int iOpenLv;
    /// <summary>
    /// 成就Icon
    /// </summary>
    public int iIcon;
    /// <summary>
    /// 成就描述
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
    /// 成就类型ID（1，等级达到x级。2，主线副本星数达到x星。3，战斗力达到x。4，强化六名上阵武将等级到x级。5，六名上阵武将进阶等级到达x级。6，过关斩将达到x星。7，VIP等级达到x。8，领地解决暴动事件x次。9，累积巡逻x小时。10，累积终结病毒x次。11，精英副本星数达到x星。12，六名上阵武将插件达到1星。13，通关精英副本第x章。14，上阵6个武将穿齐4件装备。15，上阵6个武将穿戴齐2件宝物）
    /// </summary>
    public int iCondition;
    /// <summary>
    /// 条件参数
    /// </summary>
    public int iConditionParam;
    /// <summary>
    /// 奖励Id
    /// </summary>
    public int iAwardId;
}
