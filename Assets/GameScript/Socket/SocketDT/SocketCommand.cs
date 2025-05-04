using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;

public enum SocketCommand
{
    /// <summary>
    /// PING
    /// </summary>
    PING = 10000,
    PING_Reps = 30000,

    /// <summary>
    /// --客户端<-->游戏服务器
    /// </summary>
    MSG_CGameMsg = 6,

    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 创建账户 
    /// </summary>
    CS_UserCreate = 10001,
    /// <summary>
    /// 登陆申请 CMsg_CTG_AccountEnter
    /// </summary>
    CS_UserLogin = 10002,
    /// <summary>
    /// 通过平台创建、登录
    /// </summary>
    CS_UserLoginChannel = 10003,
    /// <summary>
    /// 请求随机名字
    /// </summary>
    CS_QueryRandRoleName = 10010,
    /// <summary>
    /// 向服务器确认选角色
    /// </summary>  
    CS_RoleDIY = 10011,
    CS_ChangeName = 10012,          // 角色改名
    CS_UserReport = 10015,			// 用户报告

    /// <summary>
    /// 设置新手引导步骤
    /// </summary>
    CS_SetNewbieStep = 10013,

    CS_CardLvUp = 10100,            // 卡牌升级
    CS_CardFragmentInfo = 10101,    // 卡牌碎片初始化
    CS_CardSynthesis = 10102,       // 卡牌合成
    CS_SellCard = 10103,            // 卡牌出售
    CS_CardEvolve = 10104,            //卡牌进阶
    CS_CardAwakenEquip = 10105,		// 卡牌领悟嵌入装备
    CS_CardAwaken = 10106,			// 卡牌领悟
    CS_CardSkyDestiny = 10107,		// 卡牌天命突破
    CS_CardArtifactUpgrade = 10110, //卡牌神器升级
    CS_Recycle = 10021,				// 回收
    CS_Rebirth = 10022,             // 重生

    CS_CardTransmigration = 10111,	//卡牌转生

    CS_ItemInfo = 10200,            // 道具初始化
    CS_SellItem = 10201,            // 出售道具
    CS_UseItem = 10203,             // 使用道具

    CS_ShopLotteryInfo = 10300,     // 抽奖商店信息
    CS_ShopLotteryBuy = 10301,		// 抽奖商店购买
    CS_ShopGiftInfo = 10302,        // 礼包商店信息
    CS_ShopGiftBuy = 10303,			// 礼包商店购买
    CS_ShopResourceInfo = 10304,    // 资源商店信息
    CS_ShopResourceBuy = 10305,		// 资源商店购买
    CS_ShopLotteryChoose = 10306,
    CS_ShopLotteryGetAward = 10307,
    CS_ShopLotteryCampBuy = 10308,
    CS_Shop_CampGemBuy = 10309,

    CS_ShopRandInfo = 10310,     // 随机商店信息
    CS_ShopRandBuy = 10311,  // 随机商店购买
    CS_ShopRandRefresh = 10312,  // 随机商店刷新
    CS_Reputation = 10313,	 //声望
    CS_QueryReputation = 10314,	 //查询声望
    CS_ShopCrossServInfo = 10315,		//跨服商店信息
    CS_ShopCrossServBuy = 10316,        //跨服商店购买
                                        //TsuCode - ChaosBattle
    CS_ShopChaosInfo = 10319,		//跨服商店信息
    CS_ShopChaosBuy = 10318,		//跨服商店购买
    //
    CS_FormationChangeCard = 10401, // 阵型更换卡牌
    CS_FormationChangeEquip = 10402,// 阵型更换装备
    CS_FightPos = 10403,			// 编队更新战斗位置
    CS_FormationChangeTreasure = 10404,	//阵型更换法宝
    CS_QueryOtherFormation = 10405, // 查看他人阵容
    CS_FormationChangeEquipOneKey = 10406,// 一键阵型更换装备
    CS_FormationChangeGodEquip = 10407,// thay doi than binh


    CS_EquipFragmentInfo = 10502,     //装备碎片
    CS_EquipFragmentSell = 10508,      //装备碎片卖出
    CS_EquipSell = 10507,                //装备卖出
    CS_EquipIntensify = 10503,      // 装备强化
    CS_EquipRefine = 10504,         // 装备精炼
    CS_EquipStarsup = 10505,		// 装备升星
    CS_EquipSynthesis = 10506,      // 装备合成
    CS_EquipIntensifyOneKey = 10509,// 装备一键强化
    CS_EquipRefineOnekey = 10510,		//装备一键精练
    CS_EquipCostHistory = 10023,	// 装备强化和升星历史累计消耗
    CS_CardCostHistory = 10024,     // 卡牌(命星等)历史累计消耗

    CS_GodEquipIntensifyOneKey = 10511,// cường hóa 
    CS_GodEquipRefine = 10512,// tinh luyen
    CS_GodEquipRefineOnekey = 10513,       //tinh luyên nhanh
    CS_GodEquipStarsup = 10514,     // thang sao
    CS_GodEquipSynthesis = 10515,      // ghep manh than binh

    CS_TreasureSynthesis = 10704,   // 法宝合成
    CS_TreasureIntensify = 10705,   // 法宝强化
    CS_TreasureRefine = 10706,		// 法宝精炼
    CS_TreasureTransmigration = 10710,//法宝转生

    CS_DungeonChapter = 10600,      // 副本章节
    CS_DungeonTollgate = 10601,     // 副本关卡
    CS_DungeonChallenge = 10602,    // 挑战
    CS_DungeonSweep = 10604,        // 扫荡
    CS_DungeonTollgateBox = 10605,  // 关卡宝箱奖励
    CS_DungeonChapterBox = 10606,	// 星数奖励
    CS_DungeonLegend = 10607,       // 名将副本
    CS_DungeonReset = 10608,		// 副本重置
    CS_DungeonBoxOneKeyGet = 10610, // 一键领取关卡和星数奖励
    CS_DungeonSweepAll = 10611,  //主线宝鉴一键扫荡

    CS_AwakenEquipInfo = 10800,     // 领悟装备
    CS_AwakenEquipSynthesis = 10801,// 领悟装备合成
    CS_AwakenEquipSell = 10802,		// 领悟装备卖出

    CS_TaskDaily = 10900,           // 日常任务
    CS_TaskDailyAward = 10901,		// 请求日常任务奖励
    CS_TaskBox = 10902,             // 任务积分宝箱
    CS_TaskAchievement = 10903,     // 成就
    CS_TaskAchievementAward = 10904,// 成就领取奖励


    CS_UserSigned = 11000,      //用户每日/豪华签到
    CS_IsUserSigned = 11001,    //用户当天是否已经签到
    CS_Recharge = 11101,			// 充值   白名单充值
    CS_FirstRecharge = 11102,       //首充
    CS_FirstRechargeInfo = 11103,	//首充信息
    CS_PaySDK = 11104, //充值SDK

    CS_GetPower = 11200,			// 领取体力
    CS_QueryGetPower = 11201,		// 查询是否领取体力
    CS_LuckySymbol = 11300,			//招财符
    CS_GetTreasureBox = 11301,		//领取招财符宝箱
    CS_QueryLuckySymbol = 11302,	//查询招财符领取
    CS_RankGift = 11400,            //购买等级礼包
    CS_QueryRankGfit = 11401,		//查询等级礼包
    CS_QueryBattleFormation = 11500,//查询阵图
    CS_BattleFormation = 11501,     //点亮
    CS_QueryWealth = 11600,             //查询迎财神
    CS_GetWealth = 11601,				//迎财神
    CS_GetWealthFortune = 11602,            //领取迎财神宝宝箱
    CS_QueryActLoginGift = 11700,			//查询登陆送礼
    CS_ActLoginGift = 11701,				//登陆送礼
    CS_QueryActLoginGiftNewServ = 11702,    //查询登陆送礼（新服豪礼）
    CS_ActLoginGiftNewServ = 11703,		    //登陆送礼（新服豪礼）
    CS_QueryOpenServFund = 11800,           //查询开服基金(包含查询全民福利)
    CS_GetOpenServFund = 11801,         //领取开服基金
    CS_GetAllServFund = 11803,				//领取全民福利
    CS_BuyOpenServFund = 11804,			//购买开服基金
    CS_QBuyOpenServFund = 11805,            //查询购买全服基金
    CS_OpenServOnlineAwardInfo = 11806,	 //在线奖励查询
    CS_OpenServOnlineAward = 11807,	 //在线奖励领取
    //CS_RemainMonCardDay = 11900,    //月卡剩余天数(已不用)
    CS_QueryMonthCard = 11901,  //查询月卡
    CS_GetMonthCardSycee = 11902,	//领取月卡元宝
    CS_TaskMainInit = 10910,        // 主线任务初始化
    CS_TaskMainAward = 10911,		// 主线任务领取奖励

    CS_AwardCenterRecvAll = 10018,  //一键领取
    CS_AwardCenter = 10019,			// 奖励中心
    CS_AwardCenterRecv = 10020,     // 奖励中心 领取

    CS_DailyPveFight = 12000,                   //日常副本战斗
    CS_DailyPveFightInfo = 12001,				//日常副本战斗信息

    CS_ArenaList = 12701,       // 竞技场对手列表
    CS_ArenaChallenge = 12702,  // 竞技场挑战
    CS_ArenaSweep = 12703,      // 竞技场扫荡
    CS_ArenaChooseAward = 12704,	// 抽卡
    CS_ArenaRankList = 12705,    //竞技场排行榜

    CS_CrossArenaList = 12751,       // 竞技场对手列表
    CS_CrossArenaChallenge = 12752,  // 竞技场挑战
    CS_CrossArenaSweep = 12753,      // 竞技场扫荡
    CS_CrossArenaChooseAward = 12754,	// 抽卡
    CS_CrossArenaRankList = 12755,    //竞技场排行榜
    CS_CrossArenaRecordList = 12756,    //竞技场排行榜
    CS_CrossArenaShop = 12757,    //竞技场排行榜
    CS_CrossArenaBuy = 12758,    //竞技场排行榜

    CS_CrusadeRebelExploitAward = 12903,// 功勋奖励
    CS_CrusadeRebelDmgRank = 12904,     // 伤害排行榜
    CS_CrusadeRebelExploitRank = 12905,	// 功勋排行榜
    CS_CrusadeRebelInit = 12900,	// 征讨叛军初始化
    CS_CrusadeRebelShare = 12906,		// 共享

    CS_DiscountPropInfo = 13090,                //折扣道具信息
    CS_DiscountProp = 13091,					//购买折扣道具
    CS_DiscountRechargeInfo = 13092,            //充值优惠信息
    CS_DsicountRecharge = 13093,				//充值优惠
    CS_DiscountAllServInfo = 13094,         //全民福利信息
    CS_DiscountAllServ = 13095,             //全民福利

    CS_RunningManInit = 13100,              // 过关斩将
    CS_RunningManChallenge = 13102,     // 挑战
    CS_RunningManChapBox = 13103,       // 章节宝箱
    CS_RunningManBuff = 13104,          // 兑换Buff
    CS_RunningManReset = 13106,         // 重置
    CS_RunningManSweep = 13107,         // 三星扫荡
    CS_RunningManRank = 13108,          // 历史最高星数排行榜
    CS_RunningManElite = 13109,         // 精英关卡挑战
    CS_RunningManEliteTimes = 13110,    // 精英关卡购买次数

    CS_RunningManShopInit = 13120,      // 商店初始化
    CS_RunningManShopBuy = 13121,		// 商店购买



    CS_GrabTreasure = 13200,                            //夺宝
    CS_GrabTreasureInfo = 13201,                        //夺宝信息(显示机器人)
    //CS_Synthetise = 13202,                              //合成
    CS_GrabTreasureSweep = 13202,						//夺宝(扫荡)
    CS_GrabTreasureOneKey = 13203,						//一键夺宝
    CS_TreasureSynthesisOnKey = 10708,// 法宝一键合成

    CS_PatrolInit = 13300,          // 领地巡逻初始化
    CS_PatrolChallenge = 13301,     // 挑战
    CS_PatrolEvent = 13302,         // 事件历史
    CS_PatrolBegin = 13303,         // 开始巡逻
    CS_PatrolPacify = 13304,        // 镇压暴动
    CS_PatrolUpgrade = 13305,       // 领地升级
    CS_PatrolAward = 13306,         //领奖励
    CS_PatrolPacifyOnekey = 13307,	// 一键镇压暴动(比较特殊，需要关系服发送)
    CS_PatrolEventEx = 13308, //一键请求事件

    CS_ReinforceFight = 13401,							//援军上阵
    CS_DescendFortuneInfo = 13500,                  //天降横财查询
    CS_DescendFortune = 13501,                          //天降横财
    CS_DescendForuneRecord = 13502,					//天降横财记录

    CS_BattleFeatBuy = 12910,           // 战功商店

    ///七日活动
    CS_SevenDayTaskInfo = 13600,						//七日活动信息查询
    CS_SevenDayTaskAward = 13601,                       //七日活动达成领取
    CS_SevenDayAchieve = 13602,                        //七日任务达成
    CS_SevenDayTaskHalfDiscountInfo = 13603,                //七日半价折扣
    CS_SevenDayTaskHalfDiscount = 13604,                //七日半价折扣    
    CS_EveryDayHotSaleInfo = 13605,                 //七日活动热卖
    CS_EveryDayHotSaleBuy = 13606,					//七日活动购买
    CS_OpenServTime = 13607,                            //开服时间

    //礼品吗
    CS_CdkeyAward = 10016,			// 激活码礼包

    CS_FashionEquip = 14002,				// 时装:穿
    CS_FashionUnequip = 14003,              // 时装:脱

    //新年活动
    CS_NewYearSellingInfo = 15000,          // 新年贩售:初始化
    CS_NewYearSellingBuy = 15001,			// 新年贩售:购买

    CS_NewYearGiftInfo = 15002,             // 新年送礼:初始化
    CS_NewYearGiftRecv = 15003,             // 新年送礼:领取

    CS_RedPacketExchange = 15100,			//红包兑换
    CS_RedPacketExchangeInfo = 15101,		//红包兑换信息
    CS_RedPacketTask = 15102,				//红包任务
    CS_RedPacketTaskInfo = 15103,			//红包任务信息

    CS_NewYearStepInfo = 15004,             // 闹新春:初始化
    CS_NewYearStep = 15005,                 // 闹新春


    CS_ValentineSendRose = 15007,			// 情人节送花
    CS_ValentineSendRoseBoxAward = 15008,	// 情人节送花 积分奖励
    CS_ValentineSendRoseInit = 15006,		// 情人节初始化

    CS_NewYearSyceeConsumeInfo = 15011,         //元宝消费累计信息
    CS_NewYearSyceeConsume = 15012,				//累计元宝消费领取
    CS_NewYearRechargeAwardInfo = 15015,    //查询
    CS_NewYearSingleRechargeAward = 15016,      //春节单充奖励
    CS_NewYearMultiRechargeAward = 15017,		//春节累充奖励

    CS_NewYearSignInfo = 15013,                 //新春签到查询
    CS_NewYearSign = 15014,						//新春签到

    CS_MammonGiftInfo = 15020,          // 财神送礼初始化
    CS_MammonGiftAward = 15021,			// 财神送礼领奖

    CS_SyceeAwardInfo = 15030,              //十万元宝查询
    CS_SyceeAward = 15031,					//十万元宝领取
    CS_VipGiftInfo = 15032,             //VIP礼包查询
    CS_VipGift = 15033,                     //VIP礼包领取

    CS_RankerUserPower = 15040,                     //玩家战力排行榜
    CS_RankerLegionPower = 15041,                   //军团战力排行榜
    CS_UserPowerPraise = 15043,						//玩家战力点赞

    CS_RankerUserDungeonStars = 15044,        //副本排行榜
    CS_RankerUserLevel = 15042,               //等级排行榜

    CS_WeekFundInfo = 15050,           ////周基金查询
    CS_WeekFundSign = 15051,			//周基金签到


    CS_TacticalTransInfo = 15110,           //阵法练兵	-- 信息
    CS_TacticalTrans = 15111,               //阵法练兵	--	练兵
    CS_TacticalPickup = 15112,              //阵法练兵	--	拾取
    CS_TacticalTransSycee = 15113,               //阵法练兵	--	元宝练兵
    CS_TacticalTransOnekey = 15114,               //阵法练兵	-- 一键练兵
    CS_TacticalPickupOneKey = 15115,        //阵法练兵	--	一键拾取


    CS_TacticalFormat = 15116,              //阵法	--	信息
    CS_TacticalStudy = 15117,               //阵法	--	学习
    CS_TacticalUse = 15118,				//阵法	--	使用

    CS_GodDressInfo = 15120,            //请求神装信息
    CS_GodDressBuy = 15121,             //神装购买
    CS_GodDressBox = 15122,				//领取神装宝箱

    CS_CrossBattleInfor = 16000,            //请求玩家跨服战数据
    CS_CrossBattle = 16010,				//请求跨服战匹配对手
    CS_CrossBattleBuyTims = 16020,              //跨服战购买挑战次数

    //TsuCode - ChaosBattle
    CS_ChaosBattleInfor = 13139,            //请求玩家跨服战数据
    CS_ChaosBattle = 13138,				//请求跨服战匹配对手
    CS_ChaosBattleBuyTims = 13137,              //跨服战购买挑战次数
    CS_ChaosRank = 16109,                       //跨服排行榜
    CS_ChaosRankSelf = 16102,
    CS_ChaosHistory = 16103,
    //TsuCode - AFK module
    CS_AFKTimeInfo = 16104,
    CS_AFKGetAward = 16105,
    //TsuCode - Coin - kim phieu
    CS_RechargeCoin = 11109,
    CS_BuyCoinSDK = 11110,
    CS_RechargeTripleMoney = 11111,
    CS_RechargeLevelGift = 11112,
    CS_RechargeBattlePass = 11113,
    CS_RechargeScoreBattlePass = 11114,
    //-------------------------------
    CS_CrossCardBattleInfor = 16030,            //请求玩家斗将数据
    CS_CrossCardRandUser = 16040,               //请求斗将匹配4个玩家 
    CS_CrossCardBattle = 16050,                 //请求斗将战斗
    CS_CrossCardRandCard = 16060,               //请求斗将刷新20张武将
    CS_CrossCardSaveFormation = 16070,			//保存斗将阵型

    CS_CrossRank = 16100,                       //跨服排行榜
    CS_CrossRankSelf = 16101,					//跨服排行榜玩家自己信息
    CS_FestivalExchangeInfo = 16110,            //节日活动（兑换）
    CS_FestivalExchange = 16111,

    CS_TurntableInfo = 16112,                   //通天转盘信息
    CS_TurntableLottery = 16113,                //通天转盘抽奖
    CS_TurntableBox = 16114,					//领取宝箱
    CS_TurntableBoxInfo = 16115,				//宝箱信息
    CS_TurntableBoxAll = 16116,     //一键领取宝箱

    CS_DealsEveryDayInfo = 16117,
    CS_DealsEveryDayBuy = 16118,
    CS_DealsEveryDayBuy7 = 16119,
    CS_ExclusionSpin = 16150,


    CS_TowerEnter = 16200,            //进入试炼之塔
    CS_TowerChallenge = 16201,            //挑战
    CS_TowerReset = 16202,            //重置
    CS_TowerSweep = 16203,            //扫荡

    CS_SendPhoneCode = 16204,          //发送手机验证码
    CS_PhoneBind = 16205,            //手机绑定
    CS_PhoneBindAward = 16206,          //绑定礼包领取

    CS_SevenStarLightInfo = 16120,  //七星命灯信息
    CS_SevenStarLightAwardList = 16125,        //七星命灯全服中奖记录
    CS_SevenStarLightLottery = 16121,   //七星命灯抽奖
    CS_SevenStarBlessLevelUp = 16122,   //七星灯提升祝福值

    CS_SevenStarGiftAwardList = 16123,  //七星灯特殊奖励中奖列表
    CS_SevenStarGiftAwardKey = 16124,	//查看特殊奖励CDKey

    CS_EventOnlineVipInfo = 16126,	//get info
    CS_GetEventOnlineVipAward = 16127,
    CS_GetVoteAppAward = 16128,
    CS_EventTimeInfo = 16129,
    CS_RankingPowerList = 16130,    //event top luc chien
    CS_RankingGodEquipList = 16131,    //event top than binh
    CS_RankingTariningList = 16132,    //event top tran phap
    CS_GetTripleMoney = 16140,
    CS_GetLevelGift = 16141,
    CS_GetBattlePassTaskAward = 16142,
    CS_GetBattlePassInfo = 16144,
    CS_GetBattlePassAward = 16146,
    CS_RankingBattlePassList = 16148,

    CS_CrossTournamentRegedit = 16150, // đăng ký or báo danh
    CS_CrossTournamentTest = 16152,
    CS_CrossTournamentUserList = 16153,// lấy danh sách user
    CS_CrossTournamentGroupStageList = 16154,// lấy danh sách đánh vòng bảng
    CS_CrossTournamentAllKnockList = 16155,// lấy danh sách đánh vòng bảng
    CS_CrossTournamentBuy = 16156,
    CS_CrossTournamentShop = 16157,
    CS_CrossTournamentInfo = 16158,
    CS_CrossTournamentTheBetInfo = 16159,
    CS_CrossTournamentTheBet = 16160,

    CS_LotteryLimit_Draw = 17001,
    CS_BuyShopEventTime = 17101,
    CS_BuyShopEndow = 17102,
    CS_BuyShopSeason = 17103,
    //////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 创建账户结果 
    /// </summary>
    SC_UserCreate = 30001,
    /// <summary>
    /// 登陆结果返回 CMsg_AccountLoginRelt
    /// </summary>
    SC_UserLogin = 30002,
    SC_Kickout = 30003,
    /// <summary>
    /// 聊天服登陆通知
    /// </summary>
    SC_RelationServer = 30004,

    SC_ChatOffLine = 30005,

    SC_ReturnRandRoleName = 30010,
    SC_RoleDIY = 30011,
    /// <summary>
    /// 新手引导步骤
    /// </summary>
    SC_NewbieStep = 30013,
    /// <summary>
    /// 游戏开始通知  
    /// </summary>
    SC_RoleEnterGame = 30016,
    /// <summary>
    /// 操作结果回应
    /// </summary>
    CONTROL_CTG_OperateResult = 30017,

    SC_RandAward = 30018,// 通用奖励

    SC_UserAttrInit = 30021,
    /// <summary>
    /// 角色：数据更新
    /// </summary>
    SC_UserAttr = 30022,
    /// <summary>
    /// 战斗结果
    /// </summary>
    SC_FightRet = 30050,            // 战斗结果

    SC_CardInit = 30100,
    SC_CardRemove = 30101,
    SC_CardFragmentInfo = 30102,
    SC_CardFragmentRemove = 30103,
    SC_CardSkyDestiny = 30107,		// 卡牌天命突破返回

    SC_ItemInfo = 30200,
    SC_ItemRemove = 30201,

    SC_ShopLotteryInfo = 30300,		// 抽奖商店信息
    SC_ShopGiftInfo = 30302,		// 礼包商店信息
    SC_ShopResourceInfo = 30304,	// 资源商店信息

    SC_ShopRandInfo = 30310,     // 随机商店信息
    SC_QueryReputation = 30311,	//查询声望

    SC_ShopCrossServInfo = 30315,		//跨服商店信息
    SC_ShopLotteryGetAward = 30317,
    SC_FormationInfo = 30400,		// 阵型
    SC_FightPos = 30403,			// 编队设定战斗位置
    SC_QueryOtherFormation = 30405,	// 查看他人阵容返回
    //TsuCode - ChaosBattle
    SC_ShopChaosInfo = 30319,
    //
    SC_DungeonChapter = 30600,      // 副本章节
    SC_DungeonTollgate = 30601,		// 副本关卡
    SC_DungeonFinish = 30602,		// 结算
    SC_DungeonLegend = 30607,      //名将副本信息
    SC_DungeonAwardChanged = 30608,  // 奖励领取变化


    SC_EquipInfo = 30500,           // 装备
    SC_EquipRemove = 30501,			// 装备移除
    SC_EquipCostHistory = 30023,	// 装备强化和升星历史累计消耗
    SC_CardCostHistory = 30024,     // 装备(命星等)历史累计消耗

    SC_GodEquipInfo = 31400,           // than binh
    SC_GodEquipRemove = 31401,          // gỡ thần binh
    SC_GodEquipIntensify = 31403,       // trả về cường hóa
    SC_GodEquipFragmentInfo = 31420,   // manh than binh
    SC_GodEquipFragmentRemove = 31421, // 装备碎片移除

    SC_EquipFragmentInfo = 30502,   // 装备碎片
    SC_EquipFragmentRemove = 30503, // 装备碎片移除
    SC_EquipIntensify = 30504, // 装备强化返回

    SC_TreasureInfo = 30700,            // 法宝
    SC_TreasureRemove = 30701,          // 法宝移除

    SC_TreasureFragmentInfo = 30702,    // 法宝碎片
    SC_TreasureFragmentRemove = 30703,	// 法宝碎片移除

    SC_AwakenEquipInfo = 30800,		// 领悟装备
    SC_AwakenEquipRemove = 30801,	// 领悟装备移除

    SC_TaskDaily = 30900,           // 日常任务数据
    SC_TaskDailyAwarded = 30901,	// 日常任务已完成的
    SC_TaskBox = 30902,				// 任务积分宝箱
    SC_TaskAchievement = 30903,		// 成就

    SC_TaskMainInit = 30910,		// 主线任务初始化

    SC_UserSigned = 31000, //用户每日签到
    SC_IsUserSigned = 31001, //用户当天是否已经签到

    SC_RechargeHistory = 31100,     // 充值历史
    SC_RechargeSuc = 31101,			// 充值成功
    SC_FirstRechargeInfo = 31102,	//首充信息
    SC_UnsettledPay = 31104, //未处理的充值
    SC_RechargeRecord = 31105, //充值记录 充值时间戳
    SC_GetPower = 31200,			// 领取体力
    SC_LuckySymbol = 31300,			//招财符
    SC_RankAllow = 32401,				//24小时内允许进行等级礼包购买的等级
    SC_GiftBuy = 32402,				//允许购买礼包的购买信息
    SC_BattleForm = 32500,		//阵图ID
    SC_WealthManInfo = 32600,			//迎财神信息
    SC_ActLoginGift = 31700,			//登陆送礼
    SC_ActLoginGiftInfo = 31701, //登陆送礼查询返回
    SC_ActLoginGiftNewServ = 31702,	    //登陆送礼（开服豪礼）
    SC_NewServGift = 31703,        //新服豪礼奖励领取
    SC_OpenServFund = 31800,            //开服基金
    SC_AllServFund = 31801,			//全民福利(开服基金)
    SC_BuyOpenServFund = 31802,		//查询购买开服基金
    SC_OpenServOnlineAwardInfo = 31806, //在线奖励查询
    SC_QueryMonthCard = 31901,			//查询月卡

    SC_AwardCenter = 30019,			// 奖励中心
    SC_AwardCenterRemove = 30020,	// 奖励中心 移除奖励
    SC_AwardCenterRcvAll = 30025,  // 奖励中心一键领取
    SC_EnergyTime = 30030,          // 体力恢复计时节点
    SC_VigorTime = 30031,           // 精力恢复计时节点
    SC_CrusadeTokenTime = 30032,	// 征讨令恢复计时节点

    SC_DailyPveFight = 32000,			//日常副本战斗
    SC_DailyPveFightInfo = 32001,		//日常战斗信息

    SC_Arena = 32700,  //玩家自身竞技场数据

    SC_ArenaRet = 32702,            // 竞技场战斗结果
    SC_ArenaSweepRet = 32703,		// 竞技场扫荡结果
    SC_ArenaChooseAward = 32704,    // 竞技场抽卡返回

    SC_CrossArena = 32750,  //玩家自身竞技场数据
    SC_CrossArenaList = 32751,
    SC_CrossArenaRet = 32752,            // 竞技场战斗结果
    SC_CrossArenaSweepRet = 32753,		// 竞技场扫荡结果
    SC_CrossArenaChooseAward = 32754,   // 竞技场抽卡返回
    SC_CrossArenaResult = 32755,    // 竞技场抽卡返回
    SC_CrossArenaInfo = 32756,  //玩家自身竞技场数据
    SC_CrossArenaRankList = 32757,  //玩家自身竞技场数据
    SC_CrossArenaRecordList = 32758,  //玩家自身竞技场数据
    SC_CrossArenaShopInfo = 32759,

    /// 叛军
    SC_CrusadeRebelTrigger = 32901,	// 叛军触发
    SC_CrusadeRebelInit = 32900,    // 征讨叛军初始化
    SC_CrusadeRebelRet = 32902,     // 征讨结果
    SC_CrusadeRebelDmgRank = 32904,     // 伤害排行榜
    SC_CrusadeRebelExploitRank = 32905,	// 功勋排行榜

    SC_DiscountPropInfo = 33090,        //折扣购买信息
    SC_DiscountProp = 33091,            //购买折扣信息
    SC_DiscountRechargeInfo = 33092,    //充值优惠信息
    SC_DiscountAllServInfo = 33094, //全民福利信息
    SC_DiscountAllServ = 33095,		//全民福利

    SC_RunningMan = 33100,          // 过关斩将
    SC_RunningManChap = 33101,      // 过关斩将章节
    SC_RunningManRet = 33102,       // 挑战结果
    CS_RunningManSweepRet = 33107,	// 三星扫荡返回
    SC_RunningManRank = 33108,		// 历史最该星数排行榜
    SC_RunningManEliteRet = 33109,	// 精英关卡挑战结果
    SC_RunningManRankMySelf = 33110, // 历史最该星数排行榜自己
    SC_RunningManShopInit = 33120,	// 商店初始化


    SC_GrabTreasure = 33200,    //夺宝的结算
    SC_GrabTreasureInfo = 33201,	 //夺宝信息(显示机器人)
    SC_GrabTreasureSweep = 33202,    //夺宝扫荡的结算
    SC_GrabTreasureOneKey = 33203,					//一键夺宝

    SC_PatrolInit = 33300,          // 领地巡逻初始化
    SC_PatrolChallengeRet = 33301,  // 挑战结果
    SC_PatrolEvent = 33302,         // 事件历史
    SC_PatrolPacify = 33304,		// 镇压暴动
    SC_PatrolPacifyOnekey = 33306,	// 一键镇压暴动

    SC_ReinforceInfo = 33400,			//援军上阵查询
    SC_DescendFortuneInfo = 33500,                  //天降横财查询
    SC_DescendFortune = 33501,							//天降横财
    SC_DescendForuneRecord = 33502,                 //天降横财记录\
    ///七日活动
    SC_SevenDayTaskInfo = 33600,						//七日活动信息
    SC_SevenDayTaskAward = 33601,                       //七日活动达成领取
    SC_SevenDayTaskRecharge = 33602,                    //七日活动充值    前端不用
    SC_SevenDayTaskHalfDiscountInfo = 33603,          //半价折扣初始化
    SC_EveryDayHotSaleBuy = 33606,					//七日活动购买信息
    SC_OpenServTime = 33607,							//开服时间

    SC_FashionInfo = 34000,         // 时装
    SC_FashionRemove = 34001,       // 时装移除

    SC_RedPacketExchangeInfo = 35101,		//红包兑换信息
    SC_RedPacketTaskInfo = 35102,			//红包任务完成信息
    SC_RedPacketTaskState = 35103,		//红包任务领取状态信息

    //新年活动
    SC_NewYearSellingInfo = 35000,  // 新年贩售:初始化
    SC_NewYearGiftInfo = 35002,		// 新年送礼:初始化

    SC_NewYearStepInfo = 35004,     // 闹新春:初始化
    SC_NewYearStepRet = 35005,		// 闹新春奖励

    SC_NewYearGiftRet = 35003,		// 新年送礼:奖励

    SC_NewYearSignInfo = 35013,                 //新春签到查询


    SC_FestivalExchange = 36111,  //节日活动（兑换）

    SC_DealsEveryDayInfo = 36330,  //节日活动（兑换）
    SC_ExclusionroTationInfo = 36331,  //节日活动（兑换）
    SC_ExclusionSpinInfo = 36333,  //节日活动（兑换）


    //游戏公告
    SC_GameNotice = 90002,    //游戏主界面的公告

    SC_ValentineSendRoseInit = 35006,	// 情人节初始化

    SC_ValentineSendRoseRet = 35007,	// 情人节送花奖励
    SC_CostRecord = 31106,     //消费信息

    SC_NewYearSyceeConsumeInfo = 35011,			//元宝消费累计信息
    SC_NewYearRechargeAwardInfo = 35015,	//单充和累充信息

    SC_MammonGiftInfo = 35020,			// 财神送礼初始化
    SC_SyceeAwardInfo = 35030,				//十万元宝查询
    SC_VipGiftInfo = 35032,             //VIP礼包查询
    SC_WeekFundInfo = 35050,            //周基金查询
    SC_WeekFundSign = 35051,            //周基金领取奖励

    SC_TacticalTransInfo = 35110,           //阵法练兵 -- 练兵初始
    SC_TacticalFormat = 35116,				//阵法	--	信息初始

    SC_GodDressInfo = 35120,                //请求神装信息
    SC_GodDressBox = 35122,                 //领取神装宝箱
    SC_GodDressRankInfo = 35123,			//神装排行榜信息
    SC_GodDressBuy = 35124,				//神装购买信息

    SC_CrossBattleInfor = 36000,            //玩家跨服战数据
    SC_CrossBattlePlayer = 36010,			//跨服战匹配对手 
    SC_CrossBattleResult = 36020,			//跨服战结果

    SC_CrossCardBattleInfor = 36040,        //玩家斗将基础数据
    SC_CrossCardBattleRandCard = 36050,     //斗将随机卡牌数据
    SC_CrossCardBattlePlayer = 36060,       //斗将随机匹配4个对手 
    SC_CrossCardBattleResult = 36070,       //斗将战斗结果
    SC_CrossCardBattleFormation = 36080,	//斗将阵型数据

    SC_CrossRank = 36100,                       //跨服排行榜
    SC_CrossRankSelf = 36101,                   //跨服排行玩家自己信息
                                                //TsuCode - ChaosBattle
    SC_ChaosBattleInfor = 36131,            //玩家跨服战数据
    SC_ChaosBattlePlayer = 36132,			//跨服战匹配对手 
    SC_ChaosBattleResult = 36133,
    SC_ChaosRank = 36135,                       //跨服排行榜
    SC_ChaosRankSelf = 36102,   //跨服战结果
    SC_ChaosHistory = 36103,


    //
    SC_TurntableInfo = 36112,                   //通天转盘信息
    SC_TurntableLottery = 36113,                //通天转盘抽奖
    SC_TurntableBoxInfo = 36115,				//宝箱信息
    SC_TurntableRemainSycee = 36116,            //转盘奖池

    SC_TowerEnter = 36200,            //进入试炼之塔
    SC_TowerInit = 36204,            //初始化信息
    SC_TowerChallengeRet = 36201,            //挑战
    SC_TowerReset = 36202,            //重置
    SC_TowerSweepRet = 36203,          //扫荡

    SC_SevenStarInfo = 36120,   //七星命灯信息
    SC_SevenStarLottery = 36121,    //七星命灯抽奖
    SC_SevenStarBlessLevelUp = 36122,   //七星灯提示祝福值

    SC_SevenStarGiftAwardList = 36123,  //七星灯特殊奖励中奖列表
    SC_SevenStarGiftAwardKey = 36124,   //七星灯特殊奖励key
    SC_SevenStarLotteryAwardList = 36125,        //七星命灯全服中奖记录

    SC_PhoneBindInfo = 36205,//手机绑定信息和领取情况
    SC_PhoneBindAward = 36206,//手机绑定奖励领

    SC_EventOnlineVipInfo = 36210,   //info của newbie về nhận quà online vip 
    SC_EventTimeInfo = 36211,
    SC_RankingPowerList = 32706,
    SC_RankingPowerMyRank = 32707,
    SC_RankingBattlePassList = 32708,       // ÅÅÐÐ°ñ
    SC_RankingBattlePassMyRank = 32709,		// ÅÅÐÐ°ñ
    SC_RankingGodEquipList = 32710,
    SC_RankingTariningList = 32711,
    //Tournament
    SC_CrossTournamentRegedit = 36300,
    SC_CrossTournamentShopInfo = 36301,
    SC_CrossTournamentGroupStageList = 36302,
    SC_CrossTournamentThe = 36303,
    SC_CrossTournamentUserList = 36304,
    SC_CrossTournamentInfo = 36305,
    SC_CrossTournamentTheBetInfo = 36306,
    SC_CrossTournamentTheBetRet = 36307,

    SC_ShopEventTimeInfo = 36401,
    SC_BuyShopEventTime = 36402,
    SC_ShopEndowInfo = 36403,
    SC_BuyShopEndow = 36404,
    SC_ShopSeasonInfo = 36405,
    SC_BuyShopSeason = 36406,

    //TsuCode - AFK module
    SC_AFKInfo = 36119,
    SC_AFKGetAward = 37000,
    //------------------------

    RC_LegionPowerInfo = 43006,		//军团战力信息（游服发送，关系服返回）
    RC_RankerUserPower = 46000,     //玩家战力排行 （游服发送，关系服返回）
    RC_RankerLegionPower = 46001,       //军团战力排行（游服发送，关系服返回）
    RC_RankerUserPowerSelf = 46002,     //排行玩家自身战力（游服发送，关系服返回）
    RC_RankerLegionPowerSelf = 46003,       //排行军团自身战斗力（游服发送，关系服返回）

    RC_RankerUserDungeonStar = 46004,    //玩家副本星数排行榜
    RC_RankerUserDungeonStarSelf = 46005,  //玩家副本星数

    RC_RankerUserLevel = 46006,    //玩家等级排行榜
    RC_RankerUserLevelSelf = 46007,    //玩家自身等级排行


    #region 军团战

    CS_LegionBattleInit = 13800,            // 军团战初始化
    CS_LegionBattleApply = 13802,           // 军团战报名
    CS_LegionBattleDefenceList = 13803,     // 军团战防守列表
    CS_LegionBattleChallenge = 13804,		// 军团战挑战
    CS_LegionBattleTable = 13805,			// 军团战对阵表

    SC_LegionBattleInit = 33800,            // 军团战初始化
    SC_LegionBattleView = 33801,            // 军团战信息预览
    SC_LegionBattleDefenceList = 33803,     // 军团战防守列表
    SC_LegionBattleChallengeRet = 33804,	// 军团战挑战
    SC_LegionBattleTable = 33805,			// 军团战对阵表

    #endregion

}


public enum ChatSocketCommand
{
    PING = 10000,
    PING_Reps = 30000,

    //竞技场玩家数据 SC_ArenaList;
    RC_ArenaList = 32701,

    //排行榜数据 SC_ArenaRankList
    RC_ArenaRankList = 32705,

    RC_CrossArenaRankList = 32755,

    CR_Login = 40001,
    RC_Login = 40002,

    /// <summary>
    /// 玩家关系信息
    /// </summary>
    RC_RelationUser = 40010,

    /// <summary>
    /// 玩家基础信息
    /// </summary>
    RC_UserView = 40011,    // 
    /// <summary>
    /// 请求玩家扩展信息
    /// </summary>
    CR_UserExtra = 40012,   // 
    /// <summary>
    /// 玩家扩展信息
    /// </summary>
    RC_UserExtra = 40013,   // 
    RC_UserName = 40016,  //玩家角色名
    /// <summary>
    /// 聊天(双向)
    /// </summary>
    CR_Chat = 40100,        // 
    CR_GetChatRooms = 40102,        // 
    RC_GetChatRoom = 40101,

    CR_InitFriend = 40200,  // 请求初始化好友
    RC_InitFriend = 40201,  // 好友
    CR_InitFriendApply = 40202, // 请求初始化申请列表
    RC_InitFriendApply = 40203, // 好友申请列表
    CR_RecomFriend = 40204,     // 请求推荐好友
    RC_RecomFriend = 40205,     // 推荐好友
    CR_ApplyFriend = 40206, // 申请好友
    CR_ReplyFriend = 40207, // 好友申请回应(双向)
    CR_RemoveFriend = 40210,    // 移除好友
    RC_RemoveFriend = 40211,    // 好友移除

    CR_SendVigor = 40220,   // 赠送精力
    CR_RecvVigor = 40221,	// 领取精力
    CR_VigorHistory = 40222,// 精力记录
    RC_VigorHistory = 40223,// 精力记录

    CR_ApplyFriendByName = 40208,	// 通过名字申请好友

    CR_InitBlack = 40300,   // 请求黑名单
    RC_InitBlack = 40301,   // 黑名单
    CR_Black = 40302,       // 拉黑
    CR_Disblack = 40303,    // 取消拉黑


    CR_CrusadeRebelList = 42901,    // 叛军列表初始化
    CR_CrusadeRebel = 42902,        // 征讨


    RC_CrusadeRebelList = 42903,	// 叛军列表初始化

    CR_FriendsPatrol = 41000,   // 好友领地
    RC_FriendsPatrol = 41001,

    RC_OrangeCard = 45000,		// 获得橙色及以上品质卡牌广播
    BS_HourseLight = 90003,     //服务器跑马灯
}


