
//============================================
//
//    CardFateData来自CardFateData.xlsx文件自动生成脚本
//    2017/4/21 12:04:45
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardFateDataDT : NBaseSCDT
{

    /// <summary>
    /// 缘分名称
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
    /// 激活物品类型（1，装备。2，卡牌。3，宝物）
    /// </summary>
    public int iGoodsType;
    /// <summary>
    /// 物品ID
    /// </summary>
    public string szGoodsId;
    /// <summary>
    /// 属性ID1
    /// </summary>
    public int iAttrID1;
    /// <summary>
    /// 属性值
    /// </summary>
    public int iAttrValue1;
    /// <summary>
    /// 属性ID2
    /// </summary>
    public int iAttrID2;
    /// <summary>
    /// 属性值
    /// </summary>
    public int iAttrValue2;
    /// <summary>
    /// 属性ID3
    /// </summary>
    public int iAttrID3;
    /// <summary>
    /// 属性值
    /// </summary>
    public int iAttrValue3;
    /// <summary>
    /// 属性ID4
    /// </summary>
    public int iAttrID4;
    /// <summary>
    /// 属性值
    /// </summary>
    public int iAttrValue4;
    /// <summary>
    /// 缘分描述
    /// </summary>
    public string _szReadme;
    public string szReadme
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szReadme);
        }
    }
}
