
//============================================
//
//    GetWay来自GetWay.xlsx文件自动生成脚本
//    2017/8/24 10:53:07
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GetWayDT : NBaseSCDT
{

    /// <summary>
    /// 资源表ID
    /// </summary>
    public int iResType;
    /// <summary>
    /// 资源名称
    /// </summary>
    public string _szResName;
    public string szResName
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szResName);
        }
    }
    /// <summary>
    /// 产出功能ID（功能开放表:注意，功能ID为DungeonChapterPage1、DungeonChapterPage2。资源表ID为3、5、7时，需要遍历副本奖池。其他不需要，卡牌获取途径取该卡牌碎片的获取途径）
    /// </summary>
    public string szGetWayGoPage1;
    /// <summary>
    /// 参数1
    /// </summary>
    public int iParam1;
    /// <summary>
    /// 功能名称(在获取途径中显示的功能名称)
    /// </summary>
    public string _szGetWayName1;
    public string szGetWayName1
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szGetWayName1);
        }
    }
    /// <summary>
    /// 功能描述(在获取途径中显示的产出描述)
    /// </summary>
    public string _szGetWayDesc1;
    public string szGetWayDesc1
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szGetWayDesc1);
        }
    }
    /// <summary>
    /// 产出功能ID2
    /// </summary>
    public string szGetWayGoPage2;
    /// <summary>
    /// 参数2
    /// </summary>
    public int iParam2;
    /// <summary>
    /// 功能名称2
    /// </summary>
    private string _szGetWayName2;
    public string szGetWayName2
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szGetWayName2);
        }
        set
        {
            _szGetWayName2 = value;
        }
    }
    /// <summary>
    /// 功能描述2
    /// </summary>
    public string _szGetWayDesc2;
    public string szGetWayDesc2
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szGetWayDesc2);
        }
    }
    /// <summary>
    /// 产出功能ID3
    /// </summary>
    public string szGetWayGoPage3;
    /// <summary>
    /// 参数3
    /// </summary>
    public int iParam3;
    /// <summary>
    /// 功能名称3
    /// </summary>
    public string szGetWayName3;
    /// <summary>
    /// 功能描述3
    /// </summary>
    public string szGetWayDesc3;
    /// <summary>
    /// 产出功能ID4
    /// </summary>
    public string szGetWayGoPage4;
    /// <summary>
    /// 参数4
    /// </summary>
    public int iParam4;
    /// <summary>
    /// 功能名称4
    /// </summary>
    public string szGetWayName4;
    /// <summary>
    /// 功能描述4
    /// </summary>
    public string szGetWayDesc4;
    /// <summary>
    /// 产出功能ID5
    /// </summary>
    public string szGetWayGoPage5;
    /// <summary>
    /// 参数5
    /// </summary>
    public int iParam5;
    /// <summary>
    /// 功能名称5
    /// </summary>
    public string szGetWayName5;
    /// <summary>
    /// 功能描述5
    /// </summary>
    public string szGetWayDesc5;
    /// <summary>
    /// 备注
    /// </summary>
    public string szRemarks;
}
