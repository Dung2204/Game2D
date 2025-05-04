using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//剧情状态
public enum EM_PlotState {   
    EM_PlotState_TextTyping,            //打字效果
    EM_PlotState_Dialog,                //剧情对话
    EM_PlotState_ChangeFightRole,       //更换战斗卡牌
    EM_PlotState_ShowFightRole,         //出现某个战斗卡牌   
    EM_PlotState_ArtAni,                //美术动画表现效果  
    EM_PlotState_ChangeFightRoleSkill,  //改变武将技能
    EM_PlotState_Wait,                  //待机
}

//剧情触发条件类型
public enum EM_PlotTriggerType {
    PreplotEnd,      //前置剧情完成
    Round,           //指定回合
    FightRoleAction, //指定回合某个阵营某个站位武将行动前或行动后
    FightRoleHp,     //指定某个阵营某个站位武将血量达到一定值
    FightRoleAnger,  //指定某个阵营某个站位武将怒气达到一定值
    FightRoleSkill,  //某个阵营某个站位武将技能触发
    FightWin,        //敌军全灭，战斗胜利
    Max,
}

//战斗武将行动类型
public enum EM_FightRoleActionType {
    BeforAction,   //行动前
    AfterAction,   //行动后
}