
//============================================
//
//    ShopResource来自ShopResource.xlsx文件自动生成脚本
//    2017/3/15 18:22:20
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopResourceDT : NBaseSCDT
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public int iType;
    /// <summary>
    /// 资源id
    /// </summary>
    public int iTempId;
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
    /// 资源数量
    /// </summary>
    public int iNum;
    /// <summary>
    /// 显示等级
    /// </summary>
    public int iShowLv;
    /// <summary>
    /// 显示位置
    /// </summary>
    public int iShowSite;
    /// <summary>
    /// 限购刷新方式
    /// </summary>
    public int iRefrsh;
    /// <summary>
    /// 限购刷新时间
    /// </summary>
    public int iRefrshTime;
    /// <summary>
    /// 折扣力度
    /// </summary>
    public string szDIscount;
    /// <summary>
    /// 购买货币值(元宝)
    /// </summary>
    public string szNewNum;
    /// <summary>
    /// VIP限购次数
    /// </summary>
    public string szVipLimitTimes;
}
