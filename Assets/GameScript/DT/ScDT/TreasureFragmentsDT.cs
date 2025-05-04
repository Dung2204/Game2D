
//============================================
//
//    TreasureFragments来自TreasureFragments.xlsx文件自动生成脚本
//    2017/3/24 13:29:18
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TreasureFragmentsDT : NBaseSCDT
{

    /// <summary>
    /// 碎片名称
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
    /// 碎片Icon
    /// </summary>
    public int iIcon;
    /// <summary>
    /// 碎片品质
    /// </summary>
    public int iImportant;
    /// <summary>
    /// 碎片描述
    /// </summary>
    public string _szDescribe;
    public string szDescribe
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szDescribe);
        }
    }
    /// <summary>
    /// 可堆叠数量
    /// </summary>
    public int iPileNum;
    /// <summary>
    /// 物品类型
    /// </summary>
    public int iType;
    /// <summary>
    /// 是否可使用
    /// </summary>
    public int iIsUse;
    /// <summary>
    /// 获取途径1
    /// </summary>
    public int iPath1;
    /// <summary>
    /// 获取途径2
    /// </summary>
    public int iPath2;
    /// <summary>
    /// 获取途径3
    /// </summary>
    public int iPath3;
    /// <summary>
    /// 获取途径4
    /// </summary>
    public int iPath4;
    /// <summary>
    /// 合成法宝id
    /// </summary>
    public int iTreasureId;
}
