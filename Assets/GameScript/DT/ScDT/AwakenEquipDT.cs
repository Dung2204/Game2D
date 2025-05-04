
//============================================
//
//    AwakenEquip来自AwakenEquip.xlsx文件自动生成脚本
//    2017/3/30 15:02:27
//    
//
//============================================
using System;
using System.Collections.Generic;



public class AwakenEquipDT : NBaseSCDT
{

    /// <summary>
    /// 物品名称
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
    /// 物品Icon
    /// </summary>
    public int iIcon;
    /// <summary>
    /// 物品品质
    /// </summary>
    public int iImportant;
    /// <summary>
    /// 物品描述
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
    /// 可堆叠数量
    /// </summary>
    public int iCount;
    /// <summary>
    /// 神魂数量
    /// </summary>
    public int iResolveCount;
    /// <summary>
    /// 包裹排序
    /// </summary>
    public int iList;
    /// <summary>
    /// 资源类型(策划备注)
    /// </summary>
    public int iType;
    /// <summary>
    /// 合成材料ID1
    /// </summary>
    public int iSynthesisId1;
    /// <summary>
    /// 数量
    /// </summary>
    public int iSynthesisNum1;
    /// <summary>
    /// 合成材料ID2
    /// </summary>
    public int iSynthesisId2;
    /// <summary>
    /// 数量
    /// </summary>
    public int iSynthesisNum2;
    /// <summary>
    /// 合成材料ID3
    /// </summary>
    public int iSynthesisId3;
    /// <summary>
    /// 数量
    /// </summary>
    public int iSynthesisNum3;
    /// <summary>
    /// 合成材料ID4
    /// </summary>
    public int iSynthesisId4;
    /// <summary>
    /// 数量
    /// </summary>
    public int iSynthesisNum4;
    /// <summary>
    /// 合成消耗银币数量
    /// </summary>
    public int iMoenyNum;
    /// <summary>
    /// 加成属性ID1
    /// </summary>
    public int iAddProId1;
    /// <summary>
    /// 加成属性1
    /// </summary>
    public int iAddPro1;
    /// <summary>
    /// 加成属性ID2
    /// </summary>
    public int iAddProId2;
    /// <summary>
    /// 加成属性2
    /// </summary>
    public int iAddPro2;
    /// <summary>
    /// 加成属性ID3
    /// </summary>
    public int iAddProId3;
    /// <summary>
    /// 加成属性3
    /// </summary>
    public int iAddPro3;
    /// <summary>
    /// 加成属性ID4
    /// </summary>
    public int iAddProId4;
    /// <summary>
    /// 加成属性4
    /// </summary>
    public int iAddPro4;
}
