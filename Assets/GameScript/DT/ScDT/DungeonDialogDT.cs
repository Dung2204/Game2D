
//============================================
//
//    DungeonDialog来自DungeonDialog.xlsx文件自动生成脚本
//    2017/6/28 16:39:48
//    
//
//============================================
using System;
using System.Collections.Generic;



public class DungeonDialogDT : NBaseSCDT
{

    /// <summary>
    /// 关卡Id
    /// </summary>
    public int iTollgateId;
    /// <summary>
    /// 触发条件（1:战斗开始 2:战斗结束3某个站位的卡牌临死前）
    /// </summary>
    public int iCondition;
    /// <summary>
    /// 条件参数（条件为3：代表站位）
    /// </summary>
    public int iConditionParam;
    /// <summary>
    /// 角色名字
    /// </summary>
    public string _szRoleName;
    public string szRoleName
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szRoleName);
        }
    }
    /// <summary>
    /// 模型ID
    /// </summary>
    public int iModeId;
    /// <summary>
    /// 锚点类型(1：左,2：右)
    /// </summary>
    public int iAnchor;
    /// <summary>
    /// 对话(里面包含表情)
    /// </summary>
    public string _szDialog;
    public string szDialog
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szDialog);
        }
    }
    /// <summary>
    /// 对话音效
    /// </summary>
    public string szMusic;
}
