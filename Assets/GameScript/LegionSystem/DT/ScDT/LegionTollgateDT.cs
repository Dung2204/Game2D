
//============================================
//
//    LegionTollgate来自LegionTollgate.xlsx文件自动生成脚本
//    2018/1/18 14:31:44
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LegionTollgateDT : NBaseSCDT
{

    /// <summary>
    /// 关卡名称
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
    /// 关卡宝箱头像图片
    /// </summary>
    public int iBoxImage;
    /// <summary>
    /// 怪物1
    /// </summary>
    public int iMonster1;
    /// <summary>
    /// 怪物2
    /// </summary>
    public int iMonster2;
    /// <summary>
    /// 怪物3
    /// </summary>
    public int iMonster3;
    /// <summary>
    /// 怪物4
    /// </summary>
    public int iMonster4;
    /// <summary>
    /// 怪物5
    /// </summary>
    public int iMonster5;
    /// <summary>
    /// 怪物6
    /// </summary>
    public int iMonster6;
    /// <summary>
    /// 战斗场景
    /// </summary>
    public int iScene;
    /// <summary>
    /// 战斗Buff
    /// </summary>
    public int iBuff;
    /// <summary>
    /// 挑战奖励贡献
    /// </summary>
    public int iContri;
    /// <summary>
    /// 击杀额外贡献
    /// </summary>
    public int iKillContri;
    /// <summary>
    /// 完成奖励军团经验
    /// </summary>
    public int iFiniExp;
}
