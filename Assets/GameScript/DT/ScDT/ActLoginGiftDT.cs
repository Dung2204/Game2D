
//============================================
//
//    ActLoginGift来自ActLoginGift.xlsx文件自动生成脚本
//    2017/11/14 15:15:17
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ActLoginGiftDT : NBaseSCDT
{

    /// <summary>
    /// 类型
    /// </summary>
    public int itype;
    /// <summary>
    /// 天数
    /// </summary>
    public int iday;
    /// <summary>
    /// 活动名称
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
    /// TsuCode - điều kiện ngày
    /// </summary>
    public int iCondition;
    /// <summary>
    /// 活动开启时间
    /// </summary>
    public string szStartTime;
    /// <summary>
    /// 活动结束时间
    /// </summary>
    public string szEndTime;
    /// <summary>
    /// 资源表1
    /// </summary>
    public int iType1;
    /// <summary>
    /// 资源ID
    /// </summary>
    public int iID1;
    /// <summary>
    /// 资源数量
    /// </summary>
    public int iCount1;
    /// <summary>
    /// 资源表2
    /// </summary>
    public int iType2;
    /// <summary>
    /// 资源ID
    /// </summary>
    public int iID2;
    /// <summary>
    /// 资源数量
    /// </summary>
    public int iCount2;
    /// <summary>
    /// 资源表3
    /// </summary>
    public int iType3;
    /// <summary>
    /// 资源ID
    /// </summary>
    public int iID3;
    /// <summary>
    /// 资源数量
    /// </summary>
    public int iCount3;
    /// <summary>
    /// 资源表4
    /// </summary>
    public int iType4;
    /// <summary>
    /// 资源ID
    /// </summary>
    public int iID4;
    /// <summary>
    /// 资源数量
    /// </summary>
    public int iCount4;
}
