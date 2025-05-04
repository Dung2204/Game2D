
//============================================
//
//    SevenActivityTask来自SevenActivityTask.xlsx文件自动生成脚本
//    2017/11/30 15:41:52
//    
//
//============================================
using System;
using System.Collections.Generic;



public class SevenActivityTaskDT : NBaseSCDT
{

    /// <summary>
    /// 第几天
    /// </summary>
    public int iDayNum;
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
    /// 切页
    /// </summary>
    public int iPage;
    /// <summary>
    /// 切页名字
    /// </summary>
    public string _szPageName;
    public string szPageName
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szPageName);
        }
    }
    /// <summary>
    /// 达成条件类型（参考condition表）
    /// </summary>
    public int itype;
    /// <summary>
    /// 达成条件描述
    /// </summary>

    public string _szDonditionDesc;
    public string szDonditionDesc
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szDonditionDesc);
        }
    }
    /// <summary>
    /// 达成条件1
    /// </summary>
    public int iCondition1;
    /// <summary>
    /// 达成条件2
    /// </summary>
    public int iCondition2;
    /// <summary>
    /// 奖励类型1
    /// </summary>
    public int iAwardType1;
    /// <summary>
    /// 奖励ID7
    /// </summary>
    public int iAwardId1;
    /// <summary>
    /// 奖励数量1
    /// </summary>
    public int iAwardNum1;
    /// <summary>
    /// 奖励类型2
    /// </summary>
    public int iAwardType2;
    /// <summary>
    /// 奖励ID2
    /// </summary>
    public int iAwardId2;
    /// <summary>
    /// 奖励数量2
    /// </summary>
    public int iAwardNum2;
    /// <summary>
    /// 奖励类型3
    /// </summary>
    public int iAwardType3;
    /// <summary>
    /// 奖励ID8
    /// </summary>
    public int iAwardId3;
    /// <summary>
    /// 奖励数量3
    /// </summary>
    public int iAwardNum3;
    /// <summary>
    /// UI名字
    /// </summary>
    public string szUIName;
    /// <summary>
    /// UI参数
    /// </summary>
    public int iParam;
}
