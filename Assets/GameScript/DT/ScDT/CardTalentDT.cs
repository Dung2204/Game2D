
//============================================
//
//    CardTalent来自CardTalent.xlsx文件自动生成脚本
//    2017/5/22 11:14:49
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardTalentDT : NBaseSCDT
{

    /// <summary>
    /// 守护
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
    /// 属性对象：1，自身。2，全队。3，魏国。4，蜀国。5，吴国。6，群雄
    /// </summary>
    public int iTarget;
    /// <summary>
    /// 属性ID1
    /// </summary>
    public int iPropertyId1;
    /// <summary>
    /// 属性值1
    /// </summary>
    public int iPropertyNum1;
    /// <summary>
    /// 属性ID2
    /// </summary>
    public int iPropertyId2;
    /// <summary>
    /// 属性值2
    /// </summary>
    public int iPropertyNum2;
    /// <summary>
    /// 策划备注
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
