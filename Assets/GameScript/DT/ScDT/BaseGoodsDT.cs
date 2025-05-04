
//============================================
//
//    BaseGoods来自BaseGoods.xlsx文件自动生成脚本
//    2017/3/7 15:45:59
//    
//
//============================================
using System;
using System.Collections.Generic;



public class BaseGoodsDT : NBaseSCDT
{

    /// <summary>
    /// 名称
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
    /// 图标
    /// </summary>
    public int iIcon;
    /// <summary>
    /// 物品品质
    /// </summary>
    public int iImportant;
    /// <summary>
    /// 物品描述
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
    /// 可堆叠数量
    /// </summary>
    public int iPileQuantity;
    /// <summary>
    /// 使用方式

    /// </summary>
    public int iCanUse;
    /// <summary>
    /// 跳转链接名称
    /// </summary>
    public string szURL;
    /// <summary>
    /// 跳转界面ID
    /// </summary>
    public int iUI;
    /// <summary>
    /// 使用效果类型
    /// </summary>
    public int iEffect;
    /// <summary>
    /// 使用效果参数1
    /// </summary>
    public int iEffectData;
    /// <summary>
    /// 包裹排序
    /// </summary>
    public int iPriority;
}
