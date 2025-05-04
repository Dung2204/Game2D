using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

//协议操作类型
public enum eMsgOperateType
{
    OT_NULL = -99,      //不存在错误码

    OT_CreateAccount = 0, // 创建帐号
    OT_LoginGame = 1, // 登陆游戏
};

//协议操作结果
public enum eMsgOperateResult
{
    OR_Succeed = 0, // 成功
    #region 客户端专用
    OR_Fail = 1, //未知原因失败
    OR_SocketConnectFail = 2, //网格无法连接     
    OR_VerFail = 3, //获取版本失败 
    OR_ScFail = 4, //获取脚本失败 
    OR_ResourceFail = 5, //加载资源失败
    OR_Error_NoAccount = 21, // 登陆：账号不存在
    OR_Error_AccountOnline = 24, // 登陆：账号在线
    OR_Error_NameRepetition = 23, // 改名：名称重复
    /////////////////////////////////////////////////////////////////
    //客户端专用提示
    OR_Error_WIFIConnectTimeOut = 993, //WIFI网络未开
    OR_Error_ConnectTimeOut = 994, //连接超时
    OR_Error_CreateAccountTimeOut = 995, //注册超时
    OR_Error_LoginTimeOut = 996, //登陆超时
    OR_Error_ExitGame = 997, //游戏出错，强制玩家离开
    OR_Error_ServerOffLine = 998, //服务器未开启
    OR_Error_Disconnect = 999, //游戏断开连接
    OR_Error_Default = 10000, //操作失败
    eOR_ParamErr = 50057,				//参数表没有配置

    //好友相关错误结果
    cOR_Error_StringLength, //字符串长度错误
    cOR_Error_AddSelf, //添加自己 错误
    cOR_Error_AddRepeat, //重复添加错误
    cOR_Error_AddNumFull,//可添加数已满
    cOR_Error_InApplyList,//在申请列表中
    cOR_Error_InBlacklist, //在黑名单中
    #endregion

    eOR_Default = 50001,    // 操作失败
    OR_Error_AccountRepetition = 50002, // 注册：账号重复
    OR_Error_Password = 50003, // 登陆：密码错误 
    OR_Error_ElseWhereLogin = 50004, //异地登录 2016-7-8 
    OR_Error_SeverMaintain = 50005, //服务器维护 2016-7-8 
    OR_Error_VersionNotMatch = 50006, //版本不匹配 2016-7-8 
    eOR_CreateAndLogin = 50007,		// 创建账号并登陆
    eOR_IP_Forbidden = 50008,       // IP封禁
    eOR_Account_Forbidden = 50009,	// 账号封禁
    eOR_DuplicateRoleName = 50021,	// 重复角色名

    eOR_LevelLimit = 50030,         // 等级限制
    eOR_TimesLimit = 50031,	 // 次数限制
    eOR_Sycee = 50032,	 // 元宝不足
    eOR_Money = 50033,                   //银币不足
    eOR_Energy = 50034,				// 体力不足
    eOR_VipLimit = 50035, //Vip等级不足

    eOR_ResLimit = 50038,				//资源不足
    eOR_SellOut = 50039,				//资源已售空
    eOR_ItemError = 50040,          // 道具错误
    eOR_CardError = 50041,          // 卡牌错误
    eOR_CardFragmentError = 50042,  // 卡牌碎片错误
    eOR_EquipError = 50043,         // 装备错误
    eOR_EquipFragmentError = 50044, // 装备碎片错误
    eOR_TreasureError = 50045,          // 法宝错误
    eOR_TreasureFragmentError = 50046,	// 法宝碎片错误
    eOR_AwakenEquipError = 50047,   // 领悟装备错误
    eOR_NotyetFreeTime = 50048,		// 尚未达到免费时间

    eOR_OutOfTimeRange = 50050,         //超出时间范围限制
    eOR_InCoolingTime = 50051,          //在限制时间内
    eOR_NotDT = 50052,                  //找不到对应DT	
    eOR_NotUserRecord = 50053,          //没有对应的UserRecord
    eOR_NotAllowBuyCond = 50054,		//没有满足条件

    eOR_LoginInDeque = 50058,        //排队登陆中

    //聊天相关
    eOR_UserNotFound = 60000,    // 未找到指定用户
    eOR_UserOffline = 60001,     // 目标用户已经离线
    eOR_PeerInBlack = 60002,     // 对方在黑名单中
    eOR_InPeerBlack = 60003,     // 在对方黑名单中
    eOR_FriendListIsFull = 60004,    // 对方好友列表已满

    // 军团
    eOR_LegionAlreadyIn = 61000,        // 已经在一个军团中
    eOR_LegionNone = 61001,             // 未加入任何军团
    eOR_LegionDuplicateName = 61002,    // 重复的军团名
    eOR_LegionNoMore = 61003,           // 已无更多
    eOR_LegionNoMoreApply = 61004,      // 不能再申请更多
    eOR_LegionNotFound = 61005,         // 未找到指定军团
    eOR_LegionApplyOvertime = 61006,	// 该申请已超时
    eOR_LegionImpeachErr = 61007,       // 必需军团长离线超过5天
    eOR_LegionRankLimit = 61008,            //军团等级限制
    eOR_LegionExpLimit = 61009,         //军团经验不足
    eOR_LegionContriLimit = 61010,              //军团贡献不足
    eOR_LegionTollgateBoxTimeErr = 61011,	// 通关前加入军团的成员才可以领取该关卡宝箱
    eOR_LegionTollgateBoxOpened = 61012,    // 这个宝箱已经被开过了
    
    //军团战                                       
    eOR_LegionInBattle = 61013,			// 军团战期间不允许解散
    eOR_LegionEnemyBusy = 61014,        // 目标正在被其他成员挑战
    eOR_LegionBattleStar = 61015,       // 必须挑战更高星数
    eOR_LegionMenNotEnough = 61016,     // 加入军团24小时以上的玩家不足
    eOR_LegionBattleInvalidMen = 61017, // 新成员无挑战资格

    //礼品吗
    eOR_CdkeyInvalid = 62000,       // 无效的激活码
    eOR_CdkeyTimesLimit = 62001,    // 此激活码使用次数已达上限
    eOR_CdkeyUserLimit = 62002,     // 此类礼包领取次数已达上限
    eOR_CdkeyTimeout = 62003,		// 激活码已过期
    eOR_CdkeyNotOpen = 62004,       // 激活码暂未开放

    //跨服战
    eOR_CrossBattleTims = 62005,    //跨服战挑战次数用完
    eOR_CrossBattleBuy = 62006,		//跨服战挑战购买次数已达上限
    eOR_CrossBattleRandUserEro = 62011,			//跨服战匹配对手失败

    //斗将
    eOR_CrossCardBattleNoRegedit = 62009,	//跨服斗将战未報名不能進行戰斗
    eOR_CrossCardBattleIsResult = 62010,    //跨服斗将今日已結算

    //TsuCode - ChaosBattle
    eOR_ChaosBattleTims = 62055,    //跨服战挑战次数用完
    eOR_ChaosBattleBuy = 62056,		//跨服战挑战购买次数已达上限
    eOR_ChaosBattleRandUserEro = 62051,			//跨服战匹配对手失败
    //


    //叛军
    eOR_CrusadeRebelDisscoverNotFound = 62100,      //叛军发现者未找到
    eOR_CrusadeRebelNotShare = 62101,      //叛军未分享
    eOR_CrusadeRebelHasRun = 62102,      //叛军已逃跑
    eOR_CrusadeRebelHasKilled = 62103,      //叛军已被击杀

    eOR_HponeAlreadyBind = 62220,//电话号码已被绑定
    eOR_HponeNoErr = 62221,//无效电话号码
    eOR_PhoneCodeErr = 62222,//验证码错误
    eOR_AccountHasBind = 62223,//账号已绑定过
    eOR_PhoneNotSame = 62224,//电话号码不匹配
    eOR_AccountNotBind = 62225,//账号未绑定，不能领取奖励
    eOR_BindAwardHasGet = 62226,//奖励已经领取过了
};

//数据节点更新类型枚举
public enum eUpdateNodeType
{
    node_default,//默认，第一次进入游戏
    node_add,//添加
    node_update,
    node_delete,
}


//角色属性枚举
public enum eChangeRoleDataType
{
    eDefault = 0,
    eAccountId = 1000,      //账号ID AccountId=UserId

    eGUID = 1001,			//uid
    eLevel = 1002,       //等级	
    eRank = 1003,		//玩家rank
    eLastTime = 1004,	//Last_Login 最后离线时间	
    eCityId = 1005,	//看板娘模板Id

    eExp = 1007,		//经验

    eGold = 1008,		//金幣
    eToken = 1009,		//魔法石

    eAdvanecPP = 1010,			//AP
    eBp = 1011,			//BP

    eNoobStep = 1015,
    eBan = 1016,

    eActive = 1017,
    eVisitor = 1018,
    eMoney = 1019,

}
