
//============================================
//
//    CardFragment来自CardFragment.xlsx文件自动生成脚本
//    2017/3/7 15:46:15
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardFragmentDT : NBaseSCDT
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
    /// 合成整卡需要数量
    /// </summary>
    public int iNeedNum;
    /// <summary>
    /// 合成新卡Id
    /// </summary>
    public int iNewCardId;
    /// <summary>
    /// 掉落关卡Id
    /// </summary>
    public string szStage;
    
}
