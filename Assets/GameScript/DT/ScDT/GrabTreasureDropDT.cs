
//============================================
//
//    GrabTreasureDrop来自GrabTreasureDrop.xlsx文件自动生成脚本
//    2017/7/13 13:46:26
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GrabTreasureDropDT : NBaseSCDT
{
    /// <summary>
    /// 掉落触发文字
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
    /// 掉落几率
    /// </summary>
    public int iDropRate;
}
