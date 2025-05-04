
//============================================
//
//    PatrolLand来自PatrolLand.xlsx文件自动生成脚本
//    2017/9/7 19:31:56
//    
//
//============================================
using System;
using System.Collections.Generic;



public class PatrolLandDT : NBaseSCDT
{

    /// <summary>
    /// 关卡名字
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
    /// 解锁条件
    /// </summary>
    public int iUnlock;
    /// <summary>
    /// 关卡介绍
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
    /// 模型Id
    /// </summary>
    public int iModelId;
    /// <summary>
    /// 模型对话
    /// </summary>
    public string szModelDialog;
    /// <summary>
    /// 通关奖励Id
    /// </summary>
    public string szPassAward;
    /// <summary>
    /// 巡逻奖励展示(仅前端)
    /// </summary>
    public string szPatrolAwardShow;
    /// <summary>
    /// 怪物1
    /// </summary>
    public int iMonster1;
    /// <summary>
    /// 怪物2
    /// </summary>
    public int iMonster2;
    /// <summary>
    /// 怪物3
    /// </summary>
    public int iMonster3;
    /// <summary>
    /// 怪物4
    /// </summary>
    public int iMonster4;
    /// <summary>
    /// 怪物5
    /// </summary>
    public int iMonster5;
    /// <summary>
    /// 怪物6
    /// </summary>
    public int iMonster6;
    /// <summary>
    /// 推荐战斗力
    /// </summary>
    public int iPower;
}
