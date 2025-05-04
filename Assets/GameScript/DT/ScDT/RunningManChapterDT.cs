
//============================================
//
//    RunningManChapter来自RunningManChapter.xlsx文件自动生成脚本
//    2017/7/13 13:36:08
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RunningManChapterDT : NBaseSCDT
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
    /// 关卡ID
    /// </summary>
    public string szTollgateIds;
    /// <summary>
    /// 展示怪物
    /// </summary>
    public string szShowMonsters;
    /// <summary>
    /// 三星宝箱
    /// </summary>
    public int iBox3;
    /// <summary>
    /// 六星宝箱
    /// </summary>
    public int iBox6;
    /// <summary>
    /// 九星宝箱
    /// </summary>
    public int iBox9;
}
