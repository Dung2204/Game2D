
//============================================
//
//    LegionChapter来自LegionChapter.xlsx文件自动生成脚本
//    2018/1/13 21:29:56
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LegionChapterDT : NBaseSCDT
{

    /// <summary>
    /// 章节名字
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
    /// 关卡Id1
    /// </summary>
    public int iTollgateId1;
    /// <summary>
    /// 关卡Id2
    /// </summary>
    public int iTollgateId2;
    /// <summary>
    /// 关卡Id3
    /// </summary>
    public int iTollgateId3;
    /// <summary>
    /// 关卡Id4
    /// </summary>
    public int iTollgateId4;
    /// <summary>
    /// 章节底图
    /// </summary>
    public int iImage;
    /// <summary>
    /// 章节通关奖励
    /// </summary>
    public string szAward;
}
