
//============================================
//
//    BattleFormations来自BattleFormations.xlsx文件自动生成脚本
//    2017/4/17 19:48:33
//    
//
//============================================
using System;
using System.Collections.Generic;



public class BattleFormationsDT : NBaseSCDT
{

    /// <summary>
    /// 阵图类型
    /// </summary>
    public int iType;
    /// <summary>
    /// 类型名称
    /// </summary
    public string _szTypeName;
    public string szTypeName
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szTypeName);
        }
    }
    /// <summary>
    /// 阵图位置
    /// </summary>
    public int iPosition;
    /// <summary>
    /// 描述
    /// </summary
    public string _szDescribe;
    public string szDescribe
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szDescribe);
        }
    }
    /// <summary>
    /// 激活道具ID
    /// </summary>
    public int iActivePorpID;
    /// <summary>
    /// 激活道具数量
    /// </summary>
    public int iActivePorpCount;
    /// <summary>
    /// 属性ID
    /// </summary>
    public int iAttrID;
    /// <summary>
    /// 属性值
    /// </summary>
    public int iAttrValue;
    /// <summary>
    /// 道具ID
    /// </summary>
    public int iPropID;
    /// <summary>
    /// 道具数量
    /// </summary>
    public int iPropCount;
    /// <summary>
    /// 主角品质
    /// </summary>
    public int iRoleQuality;
    /// <summary>
    /// 掉落奖池ID
    /// </summary>
    public int iDropID;
    /// <summary>
    /// 类型图片
    /// </summary>
    public string szTypeIcon;
    /// <summary>
    /// 碎片Icon
    /// </summary>
    public string szIconID;
}
