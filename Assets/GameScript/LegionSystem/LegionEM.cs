using UnityEngine;
using System.Collections;


/// <summary>
/// 军团属性类型
/// </summary>
public enum EM_LegionProperty
{
    /// <summary>
    /// 军团长id
    /// </summary>
    MasterUserId = 1,

    /// <summary>
    /// 创建时间
    /// </summary>
    FoundTime,

    /// <summary>
    /// 图标
    /// </summary>
    Icon,
    /// <summary>
    /// 边框
    /// </summary>
    Frame,
    /// <summary>
    /// 等级
    /// </summary>
    Lv,
    /// <summary>
    /// 经验
    /// </summary>
    Exp,
    /// <summary>
    /// 成员数
    /// </summary>
    MemberNum,
    /// <summary>
    /// 名字
    /// </summary>
    Name,
    /// <summary>
    /// 宣言
    /// </summary>
    Manifesto,
    /// <summary>
    /// 公告
    /// </summary>
    Notice,
   
    /// <summary>
    /// 结束
    /// </summary>
    End,

}


// 军团职位
public enum EM_LegionPostionEnum
{
    eLegion_NONE = 0,
    eLegion_Chief = 1,  // 军团长
    eLegion_Deputy = 2, // 副军团长
    eLegion_Normal = 3,	// 普通成员
};

public enum EM_LegionOperateType
{
    ApplicantList = 0  , //申请列表
    AcceptApplicant = 1,   //同意申请操作
    DisacceptApplicant = 2, //拒绝申请操作
    DissolveLegion = 3, //解散军团
    SetNotice =4, //设置军团公告
    SetManifesto = 5, //设置军团宣言
    OpenOrUpSkill = 6,//开启或提升军团技能
    SetResetDungeonChapter = 7, //设置重置军团副本
    LevelUpLegion = 8, //升级军团
    LegionBattleSignUp = 9, //军团战报名
}

public enum EM_LegionPermission
{
    Enough = 0,      //满足权限要求
    Deputy = 1,   //副团长
    Chief = 2,    //军团长
}

public enum EM_LegionOutType
{
    Dissolve = 0,  //解散的
    Quit = 1,      //退出的
}

public enum EM_LegionDungeonLockState
{
    Unlock = 0 , //解锁
    Precodition ,  //前置条件 没通过前面的关卡
    LegionLvLimit , //军团等级限
}

public enum EM_LegionGate
{
    /// <summary>
    /// 无效的
    /// </summary>
    Invalid = 0,
    /// <summary>
    /// 内城门
    /// </summary>
    Inside = 1,
    /// <summary>
    /// 主城门
    /// </summary>
    MainGate = 2,
    /// <summary>
    /// 右城门
    /// </summary>
    RightGate = 3, 
    /// <summary>
    /// 左城门
    /// </summary>
    LeftGate = 4,

    /// <summary>
    /// 结束值
    /// </summary>
    End = 5,
}

/// <summary>
/// 军团战状态 
/// </summary>
public enum EM_LegionBattleState
{
    /// <summary>
    /// 未开启
    /// </summary>
    eLegionBattle_NONE = 0,
    /// <summary>
    /// 初始化
    /// </summary>
    eLegionBattle_Init = 1,
    /// <summary>
    /// 匹配中
    /// </summary>
    eLegionBattle_Matching = 2,
    /// <summary>
    /// 战斗中
    /// </summary>
    eLegionBattle_Fighting = 3,
}

public enum EM_LegionTableRet
{
    /// <summary>
    /// 未分出胜负
    /// </summary>
    NoFinished = 0,
    /// <summary>
    /// A胜利
    /// </summary>
    AWin = 1,
    /// <summary>
    /// B胜利
    /// </summary>
    BWin = 2,
}
