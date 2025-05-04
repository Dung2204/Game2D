
//============================================
//
//    TaskDaily来自TaskDaily.xlsx文件自动生成脚本
//    2017/11/23 19:04:43
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TaskDailyDT : NBaseSCDT
{

    /// <summary>
    /// 任务类型
    /// </summary>
    public int iTaskType;
    /// <summary>
    /// 开启等级
    /// </summary>
    public int iOpenLv;
    /// <summary>
    /// 关闭等级
    /// </summary>
    public int iCloseLv;
    /// <summary>
    /// 任务Icon
    /// </summary>
    public int iIconId;
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
    /// 完成条件类型（1，挑战x次主线副本。2，挑战x次传说副本。3，挑战x次三国无双。4，挑战x次竞技场。5，进行x次宝物合成。6，强化装备x次。7，宝物强化x次。8，健身x次。9，装备精炼x次。10，为x名好友赠送精力。11，商城进行x次紫将招募。12，进行x次橙将招募。13，进行x次叛军攻打。14，分享x次叛军。15，购买x个精力丹。16，购买x个体力。17，帮助好友解决x次暴动。18，领地巡逻x小时。19，挑战x次精英副本。20，挑战x次日常副本。21，武将升X级。22，武将进阶X级，23，装备精炼X次。24，法宝精炼X级，25，武将命星升X级，26，武将领悟X次，27，装备升星X次，28，点亮阵图X次，29，上阵X个援军,30,主角升到XX级）
    /// </summary>
    public int iCondition;
    /// <summary>
    /// 条件参数
    /// </summary>
    public int iConditionParam;
    /// <summary>
    /// 前往界面ID（参考弓开放表中界面ID）（1，阵容界面。2，数码兽背包。3，竞技场。4，主线副本界面。5，精英副本界面。6，传说副本界面。7，日常副本界面。8，将魂商店。9，插件商店。10，充值界面。11，夺宝界面。12，三国无双界面。13，领地攻伐界面。14，围剿叛军界面。15，征战界面。16，商城-招募界面。17，商城-道具界面。18，商城-VIP礼包界面。19，装备背包。20，宝物背包。21，好友。22，数码王冠，23，卡牌插件界面。24，装备优化界面。25，卡牌友情界面。26，公会界面。27，公会宣传界面。28，数码器优化界面。29，数码考古界面。30，交易所界面。99-功能未开启）
    /// </summary>
    public int iGotoId;
    /// <summary>
    /// 银币
    /// </summary>
    public int iAwardMoney;
    /// <summary>
    /// 奖池ID1
    /// </summary>
    public int iAwardId1;
    /// <summary>
    /// 奖池ID2
    /// </summary>
    public int iAwardId2;
    /// <summary>
    /// 奖池ID3
    /// </summary>
    public int iAwardId3;
    /// <summary>
    /// 奖池ID4
    /// </summary>
    public int iAwardId4;
    /// <summary>
    /// 奖池ID5
    /// </summary>
    public int iAwardId5;
    /// <summary>
    /// 奖池ID6
    /// </summary>
    public int iAwardId6;
    /// <summary>
    /// 奖池ID7
    /// </summary>
    public int iAwardId7;
    /// <summary>
    /// 奖池ID8
    /// </summary>
    public int iAwardId8;
    /// <summary>
    /// 奖池ID9
    /// </summary>
    public int iAwardId9;
    /// <summary>
    /// 奖池ID10
    /// </summary>
    public int iAwardId10;
    /// <summary>
    /// 奖池ID11
    /// </summary>
    public int iAwardId11;
    /// <summary>
    /// 积分1
    /// </summary>
    public int iScore1;
    /// <summary>
    /// 积分2
    /// </summary>
    public int iScore2;
    /// <summary>
    /// 积分3
    /// </summary>
    public int iScore3;
    /// <summary>
    /// 积分4
    /// </summary>
    public int iScore4;
    /// <summary>
    /// 积分5
    /// </summary>
    public int iScore5;
    /// <summary>
    /// 积分6
    /// </summary>
    public int iScore6;
    /// <summary>
    /// 积分7
    /// </summary>
    public int iScore7;
    /// <summary>
    /// 积分8
    /// </summary>
    public int iScore8;
    /// <summary>
    /// 积分9
    /// </summary>
    public int iScore9;
    /// <summary>
    /// 积分10
    /// </summary>
    public int iScore10;
    /// <summary>
    /// 积分11
    /// </summary>
    public int iScore11;
}
