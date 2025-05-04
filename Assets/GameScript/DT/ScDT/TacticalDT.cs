
//============================================
//
//    Tactical来自Tactical.xlsx文件自动生成脚本
//    2018/4/4 10:36:36
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TacticalDT : NBaseSCDT
{

    /// <summary>
    /// ��������
    /// </summary>
    public int iSkillType;
    /// <summary>
    /// ���ܵȼ�
    /// </summary>
    public int iSkillLv;
    /// <summary>
    /// ����ID
    /// </summary>
    public int iProId;
    /// <summary>
    /// ����
    /// </summary>
    public int iProNum;
    /// <summary>
    /// ��Ҫ���ĵ�
    /// </summary>
    public int iNeedConsume;
    /// <summary>
    /// ��������
    /// </summary>
    public string _szSkillName;
    public string szSkillName
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szSkillName);
        }
    }
}
