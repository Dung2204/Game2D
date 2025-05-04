using UnityEngine;
using System.Collections;

public enum LegionSocketCmd
{

    CS_UserLegionInit = 13000,  // 玩家军团初始化

    CS_LegionFound = 13010,			// 创建 
    CS_LegionApply = 13012,         // 申请
    CS_LegionApplyList = 13003,     // 初始化申请列表(无军团人士)
    CS_LegionApplicantList = 13004,	// 初始化申请者列表(军团管理成员) 
    CS_LegionDisapply = 13013,      // 取消申请
    CS_LegionRespond = 13014,		// 回应申请

    CS_LegionDissolve = 13011,		// 解散
    CS_LegionKickout = 13015,       // 踢人
    CS_LegionQuit = 13016,          // 退出
    CS_LegionAppoint = 13020,       // 任命
    CS_LegionDismiss = 13021,       // 罢免
    CS_LegionHandover = 13022,      // 禅让
    CS_LegionImpeach = 13023,       // 弹劾    

    CS_Manifesto = 13024,           // 修改宣言	
    CS_Notice = 13025,				// 修改公告


    CS_LegionInit = 13001,          // 军团初始化
    
    CS_LegionShop = 13030,   //军团商店购买
    CS_LegionShopTimeLimit = 13031,	 //军团限时商店
    CS_LegionShopInfo = 13032,	 //军团商店查询
    CS_LegionShopTimeLimitIDInfo = 13033,		//军团限时商店随机ID

    CS_LegionUpLevel = 13040,       // 升级
    CS_LegionSacrifice = 13041,     // 祭天
    CS_LegionSacrificeAward = 13042,// 祭天奖励    /// <summary>

    CS_LegionSkillOpen = 13050,  //军团技能等级开发(提升也用这个)
    CS_LegionSkillUp = 13051,	 //军团技能升级（学习也用这个）
    CS_LegionSkillInfo = 13052,         //军团技能上限信息 军团
    CS_LegionSkillLv = 13053,			//军团技能等级信息 个人(该请求不再主动请求，改为服务器登录前主动推送)

    CS_LegionAwardRedPacket = 13070,            //军团发红包
    CS_LegionReceiveRedPacket = 13071,      //军团收红包
    CS_LegionRankList = 13072,					//军团红包排行榜
    CS_LegionRedPacketInfo = 13073,         //请求军团红包列表信息

    CS_LegionDungeonInitChapter = 13060,    // 初始化章节
    CS_LegionDungeonInitFiniChapter = 13061,// 初始化当前/已完成章节
    CS_LegionDungeonInitTollgate = 13062,   // 初始化关卡奖励信息
    CS_LegionDungeonChallenge = 13064,      // 挑战关卡
    CS_LegionDungeonTollgateAward = 13065,  // 关卡奖励
    CS_LegionDungeonChapterAward = 13066,   // 章节奖励
    CS_LegionDungeonReset = 13067,			// 重置
    CS_LegionDungeonTimes = 13068,			// 购买挑战次数

    CS_LegionLvRankList = 13080,                //军团等级排行
    CS_LegionPveRankList = 13081,				//军团副本排行
    /// 玩家军团初始化
    /// </summary>
    SC_UserLegionInit = 33000,

    SC_LegionApplyList = 33003,     // 初始化申请列表(无军团人士)
    RC_LegionApplicantList = 33004,	// 初始化申请者列表(军团管理成员)


    RC_UserLegion = 40014,	// 玩家军团信息


    CR_LegionInfo = 43000,      // 军团信息
    RC_LegionInfo = 43001,
    CR_LegionMemInit = 43002,   // 军团成员列表
    RC_LegionMemInit = 43003,

    CR_LegionSearchByName = 43004,  // 根据名称查找军团
    RC_LegionSearchByName = 43005,

    RC_LegionLvRankList = 44000,//军团等级排行
    RC_LegionPveRankList = 44001,//军团副本排行

    SC_LegionInit = 33001,			// 军团祭天初始化

    SC_LegionShop = 33030,   //军团商店(查询)
    SC_LegionShopTimeLimit = 33031,	 //军团限时商店(查询)
    SC_LegionSkillInfo = 33052,         //军团技能上限信息 军团
    SC_LegionSkillLv = 33053,			//军团技能等级信息 个人


    SC_LegionAwardRedPacket = 33070,//军团发红包
    SC_LegionReceiveRedPacket = 33071,//军团收红包
    RC_LegionRankList = 33072,			//军团红包排行榜
    SC_LegionRedPacketInfo = 33073, //军团红包列表信息

    SC_LegionDungeonInitChapter = 33060,    // 
    SC_LegionDungeonInitFiniChapter = 33061,    //
    SC_LegionDungeonInitTollgate = 33062,	//
    SC_LegionDungeonChallengeRet = 33063, //
    SC_LegionDungeonTimes = 33068,		// 推送挑战次数

}