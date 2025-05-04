
//============================================
//
//    GuidanceTeam来自GuidanceTeam.xlsx文件自动生成脚本
//    2017/9/15 19:55:41
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GuidanceTeamDT : NBaseSCDT
{

    /// <summary>
    /// 触发条件(1，首次登录。2，升x级时。3，关卡开放时（目前少三尚未用到）。4，获取奖励时触发（比如某个关卡宝箱领取时，获得装备，触发引导装备操作）。5，进入关卡时。6，关卡战斗结束时。)
    /// </summary>
    public int iTrigger;
    /// <summary>
    /// 条件类型
    /// </summary>
    public int iCondition;
    /// <summary>
    /// 开始章节
    /// </summary>
    public int iGuidanceId;
    /// <summary>
    /// 保存关键帧
    /// </summary>
    public int iSave;
}
