
//============================================
//
//    GuidanceDialog来自GuidanceDialog.xlsx文件自动生成脚本
//    2017/9/9 13:36:32
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GuidanceDialogDT : NBaseSCDT
{

    /// <summary>
    /// 组
    /// </summary>
    public int iGroup;
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
