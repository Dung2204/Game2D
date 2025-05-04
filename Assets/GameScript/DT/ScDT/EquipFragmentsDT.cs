
//============================================
//
//    EquipFragments来自EquipFragments.xlsx文件自动生成脚本
//    2017/3/23 14:03:57
//    
//
//============================================
using System;
using System.Collections.Generic;



public class EquipFragmentsDT : NBaseSCDT
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
    /// 碎片图标
    /// </summary>
    public int iIcon;
    /// <summary>
    /// 碎片品质
    /// </summary>
    public int iColour;
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
    /// 堆叠数量
    /// </summary>
    public int iPileNum;
    /// <summary>
    /// 排序
    /// </summary>
    public int iList;
    /// <summary>
    /// 合成数量
    /// </summary>
    public int iBondNum;
    /// <summary>
    /// 货币类型
    /// </summary>
    public int iGoldType;
    /// <summary>
    /// 出售价格
    /// </summary>
    public int iGoldNum;
    /// <summary>
    /// 合成目标Id
    /// </summary>
    public int iDstEquipId;
}
