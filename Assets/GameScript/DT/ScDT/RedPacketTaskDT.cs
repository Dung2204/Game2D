
//============================================
//
//    RedPacketTask来自RedPacketTask.xlsx文件自动生成脚本
//    2018/3/9 15:25:18
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RedPacketTaskDT : NBaseSCDT
{

    /// <summary>
    /// 任务描述
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
    /// 任务类型（参考日常任务）（1，挑战x次主线副本。2，挑战x次传说副本。3，挑战x次三国无双。4，挑战x次竞技场。5，进行x次宝物合成。6，强化装备x次。7，宝物强化x次。8，健身x次。9，装备精炼x次。10，为x名好友赠送精力。11，商城进行x次紫将招募。12，进行x次橙将招募。13，进行x次叛军攻打。14，分享x次叛军。15，购买x个精力丹。16，购买x个体力。17，帮助好友解决x次暴动。18，领地巡逻x小时。19，挑战x次精英副本。20，挑战x次日常副本。21，武将升X级。22，武将进阶X级。24，法宝精炼X级，25，武将命星升X级，26，武将领悟X次，27，装备升星X次，28，点亮阵图X次，29，上阵X个援军,30,主角升到XX级）
    /// </summary>
    public int iTaskType;
    /// <summary>
    /// 条件参数
    /// </summary>
    public int iConditonParam;
    /// <summary>
    /// 奖励1
    /// </summary>
    public string szAward1;
    /// <summary>
    /// 奖励2
    /// </summary>
    public string szAward2;
    /// <summary>
    /// 奖励3
    /// </summary>
    public string szAward3;
    /// <summary>
    /// UI名字
    /// </summary>
    public string szUIName;
    /// <summary>
    /// UI参数
    /// </summary>
    public int iUIParam;
    /// <summary>
    /// 开始日期
    /// </summary>
    public int iTimeBegin;
    /// <summary>
    /// 结束日期
    /// </summary>
    public int iTimeEnd;
}
