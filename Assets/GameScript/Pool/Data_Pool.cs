using UnityEngine;
using System.Collections;
/// <summary>
/// 游戏数据数据结构
/// </summary>
public class Data_Pool
{
    #region POOL
    /// <summary>
    /// 保存玩家的个人资料
    /// </summary>
    public static UserDataUnit m_UserData;
    /// <summary>
    /// 角色
    /// </summary>
    public static CharacterPool m_CharacterPool;
    /// <summary>
    /// 解锁卡牌
    /// </summary>
    public static CardPool m_CardPool;
    /// <summary>
    /// 基础物品Pool
    /// </summary>
    public static BaseGoodsPool m_BaseGoodsPool;
    /// <summary>
    /// 卡牌碎片
    /// </summary>
    public static CardFragmentPool m_CardFragmentPool;
    /// <summary>
    /// 商城道具
    /// </summary>
    public static ShopResourcePool m_ShopResourcePool;
    /// <summary>
    /// 商城抽牌
    /// </summary>
    public static ShopLotteryPool m_ShopLotteryPool;
    /// <summary>
    /// 商城礼包
    /// </summary>
    public static ShopGiftPool m_ShopGiftPool;
    /// <summary>
    /// 随机商店（包含领悟和卡牌商店）
    /// </summary>
    public static ShopStorePool m_ShopStorePool;
    /// <summary>
    /// 声望商店
    /// </summary>
    public static ShopReputationPool m_shopReputationPool;
    /// <summary>
    /// 战功商店
    /// </summary>
    public static BattleFeatShopPool m_BattleFeatShopPool;
    /// <summary>
    /// 阵型
    /// </summary>
    public static TeamPool m_TeamPool;
    /// <summary>
    /// 编队
    /// </summary>
    public static ClothArrayData m_ClosethArrayData;
    /// <summary>
    /// 副本
    /// </summary>
    public static DungeonPool m_DungeonPool;
    /// <summary>
    /// 每日副本
    /// </summary>
    public static DailyPveInfoPool m_DailyPveInfoPool;
    /// <summary>
    /// 奖励Pool
    /// </summary>
    public static AwardPool m_AwardPool;

    /// <summary>
    /// 法宝Pool
    /// </summary>
    public static TreasurePool m_TreasurePool;
    /// <summary>
    /// 法宝碎片Pool
    /// </summary>
    public static TreasureFragmentPool m_TreasureFragmentPool;
    /// <summary>
    /// 装备Pool
    /// </summary>
    public static EquipPool m_EquipPool;

    public static EquipFragmentPool m_EquipFragmentPool;

    public static AwakenEquipPool m_AwakenEquipPool;

    // than binh pool
    public static GodEquipPool m_GodEquipPool;

    public static GodEquipFragmentPool m_GodEquipFragmentPool;
    /// <summary>
    /// 充值次数DT
    /// </summary>
    public static RechargePool m_RechargePool;

    /// <summary>
    /// 累计消费    (以天为单位)
    /// </summary>
    public static HistoryConstPool m_HistoryConstPool;
    /// <summary>
    /// 战斗动画数据
    /// </summary>
    public static BattleDataPool m_BattleDataPool;

    public static ChatPool m_ChatPool;

    public static RebelArmyPool m_RebelArmyPool;

    #region 活动类
    /// <summary>
    /// 游戏活动
    /// </summary>
    public static ActivityCommonData m_ActivityCommonData;
    /// <summary>
    /// 签到
    /// </summary>
    public static SignPool m_SignPool;
    /// <summary>
    /// 等级礼包
    /// </summary>
    public static RankGiftPool m_RankGiftPool;
    /// <summary>
    /// 显示折扣pool
    /// </summary>
    public static TimeDiscountPool m_TimeDiscountPool;
    /// <summary>
    /// 活动开服基金和全民福利
    /// </summary>
    public static OpenServFundPool m_OpenServFundPool;
    /// <summary>
    /// 在线奖励
    /// </summary>
    public static OnlineAwardPool m_OnlineAwardPool;
    /// <summary>
    /// 天降横财
    /// </summary>
    public static SkyFortunePool m_SkyFortunePool;
    /// <summary>
    /// 十万元宝
    /// </summary>
    public static TenSyceePool m_TenSyceePool;
    /// <summary>
    /// vip礼包
    /// </summary>
    public static VipGiftPool m_VipGiftPool;
    /// <summary>
    /// 首充礼包
    /// </summary>
    public static FirstRechargePool m_FirstRechargePool;
    /// <summary>
    /// 周基金
    /// </summary>
    public static WeekFundPool m_WeekFundPool;
    /// <summary>
    /// 限时神装
    /// </summary>
    public static GodDressPool m_GodDressPool;

    public static SevenStarBlessPool m_SevenStarPool;

    #endregion
    #region 春节活动类
    /// <summary>
    /// 红包兑换
    /// </summary>
    public static RedPacketExchangePool m_RedPacketExchangePool;
    /// <summary>
    /// 红包任务
    /// </summary>
    public static RedPacketTaskPool m_RedPacketTaskPool;
    /// <summary>
    /// 单充福利
    /// </summary>
    public static SingleRechargePool m_SingleRechargePool;
    /// <summary>
    /// 累充奖励
    /// </summary>
    public static MutiRechargePool m_MutiRechargePool;
    /// <summary>
    /// 累计消费
    /// </summary>
    public static TotalConsumptionPool m_TotalConsumptionPool;
    /// <summary>
    /// 新年签到
    /// </summary>
    public static NewYearSignPool m_NewYearSignPool;
    #endregion
    /// <summary>
    /// 日常任务Pool
    /// </summary>
    public static TaskDailyPool m_TaskDailyPool;

    /// <summary>
    /// 成就任务Pool
    /// </summary>
    public static TaskAchvPool m_TaskAchvPool;

    /// <summary>
    /// 跨服战（跨服演武）
    /// </summary>
    public static CrossServerBattlePool m_CrossServerBattlePool;

    //TsuCode-ChaosBattle
    public static ChaosBattlePool m_ChaosBattlePool;
    public static ChaosBattleShopPool m_ChaosBattleShopPool;
    //TsuCode - AFK module
    public static AFKPool m_AFKPool;
    //-----------------
    /// <summary>
    /// 跨服战（跨服演武）
    /// </summary>
    public static CrossServerBattleShopPool m_CrossServerBattleShopPool;

    /// <summary>
    /// 斗将Pool
    /// </summary>
    public static CardBattlePool m_CardBattlePool;

    /// <summary>
    /// 全局玩家信息Pool
    /// </summary>
    public static GeneralPlayerPool m_GeneralPlayerPool;

    /// <summary>
    /// 好友Pool
    /// </summary>
    public static FriendPool m_FriendPool;
    /// <summary>
    /// 天命系统
    /// </summary>
    public static BattleFormPool m_BattleFormPool;
    /// <summary>
    /// 竞技场Pool
    /// </summary>
    public static ArenaPool m_ArenaPool;
    /// <summary>
    /// 竞技场Pool
    /// </summary>
    public static CrossArenaPool m_CrossArenaPool;
    /// <summary>
    /// 夺宝Pool
    /// </summary>
    public static GrabTreasurePool m_GrabTreasurePool;

    /// <summary>
    /// 过关斩将Pool
    /// </summary>
    public static RunningManPool m_RunningManPool;
    /// <summary>
    /// 过关斩将商店Pool
    /// </summary>
    public static RunningManShopPool m_RunningManShopPool;
    /// <summary>
    /// 查看其他玩家阵容pool
    /// </summary>
    public static ViewPlayerLineUpPool m_ViewPlayerLineUpPool;

    /// <summary>
    /// 巡逻
    /// </summary>
    public static PatrolPool m_PatrolPool;

    /// <summary>
    /// 排行榜Pool(暂时有战力和军团)
    /// </summary>
    public static RankListPool m_RankListPool;

    public static SevenActivityTaskPool m_SevenActivityTaskPool;
    public static EveryDayHotSalePool m_EveryDayHotSalePool;
    /// <summary>
    /// 广播消息pool
    /// </summary>
    public static BroadcastPool m_BroadcastPool;
    /// <summary>
    /// 跑马灯广播消息pool
    /// </summary>
    public static BroadcastNoticePool m_BroadcastNoticePool;

    /// <summary>
    /// 游戏公告POOL
    /// </summary>
    public static GameNoticePool m_GameNoticePool;

    /// <summary>
    /// 时装Pool
    /// </summary>
    public static FanshionableDressPool m_FanshionableDressPool;

    public static NewYearSellingPool m_NewYearSellingPool;

    public static NewYearActivityPool m_NewYearActivityPool;

    public static ValentinesDayPool m_ValentinesDayPool;
    public static TariningAndTacticalPool m_TariningAndTacticalPool;

    public static TransmigrationCardPool m_TransmigrationCardPool;//卡牌转生
    public static TransmigrationTreasurePool m_TransmigrationTreasurePool;//法宝转生
    public static TurntablePool m_TurntablePool;//通天转盘

    public static TrialTowerPool m_TrialTowerPool;

    public static EventTimePool m_EventTimePool;
    public static OnlineVipAwardPool m_OnlineVipAwardPool;
    public static RankingPowerAwardPool m_RankingPowerAwardPool;

    public static CrossTournamentPool m_CrossTournamentPool;
    public static CampGemShopPool m_CampGemShopPool;
    public static ShopEventTimePool m_ShopEventTimePool;
    public static ShopEndowPool m_ShopEndowPool;
    public static ShopSeasonPool m_ShopSeasonPool;
    #endregion

    #region 本地数据处理Pool
    /// <summary>
    /// Icon图集数据pool
    /// </summary>
    public static IconDataPool m_IconDataPool;

    /// <summary>
    /// 剧情对话Pool
    /// </summary>
    public static DialogPool m_DialogPool;

    //屏蔽字库
    public static BlockWordPool m_BlockWordPool;

    public static ReddotMessagePool m_ReddotMessagePool;
    /// <summary>
    /// 引导Pool
    /// </summary>
    public static GuidancePool m_GuidancePool;


    #endregion

    /// <summary>
    /// 初始化
    /// </summary>
    public static void f_InitPool()
    {
        m_HistoryConstPool = new HistoryConstPool();
        m_UserData = new UserDataUnit();
        m_CharacterPool = new CharacterPool();
        m_CardPool = new CardPool();
        m_BaseGoodsPool = new BaseGoodsPool();
        m_CardFragmentPool = new CardFragmentPool();
        m_ShopResourcePool = new ShopResourcePool();
        m_TeamPool = new TeamPool();
        m_ClosethArrayData = new ClothArrayData();
        m_DungeonPool = new DungeonPool();
        m_DailyPveInfoPool = new DailyPveInfoPool();
        m_AwardPool = new AwardPool();
        m_EquipPool = new EquipPool();
        m_EquipFragmentPool = new EquipFragmentPool();
        m_GodEquipPool = new GodEquipPool();
        m_GodEquipFragmentPool = new GodEquipFragmentPool();
        m_ShopLotteryPool = new ShopLotteryPool();
        m_ShopGiftPool = new ShopGiftPool();
        m_ShopStorePool = new ShopStorePool();
        m_shopReputationPool = new ShopReputationPool();
        m_SignPool = new SignPool();
        m_TreasurePool = new TreasurePool();
        m_TreasureFragmentPool = new TreasureFragmentPool();
        m_AwakenEquipPool = new AwakenEquipPool();
        m_TaskDailyPool = new TaskDailyPool();
        m_TaskAchvPool = new TaskAchvPool();
        m_SevenActivityTaskPool = new SevenActivityTaskPool();
        m_EveryDayHotSalePool = new EveryDayHotSalePool();

        m_RechargePool = new RechargePool();
        m_BattleDataPool = new BattleDataPool();
        m_ActivityCommonData = new ActivityCommonData();
        m_RankGiftPool = new RankGiftPool();
        m_TimeDiscountPool = new TimeDiscountPool();
        m_OnlineAwardPool = new OnlineAwardPool();
        m_SkyFortunePool = new SkyFortunePool();
        m_TenSyceePool = new TenSyceePool();
        m_VipGiftPool = new VipGiftPool();
        m_FirstRechargePool = new FirstRechargePool();
        m_WeekFundPool = new WeekFundPool();
        m_GodDressPool = new GodDressPool();
        m_RedPacketExchangePool = new RedPacketExchangePool();
        m_RedPacketTaskPool = new RedPacketTaskPool();
        m_SingleRechargePool = new SingleRechargePool();
        m_MutiRechargePool = new MutiRechargePool();
        m_GeneralPlayerPool = new GeneralPlayerPool();
        m_FriendPool = new FriendPool();
        m_ChatPool = new ChatPool();
        m_BattleFormPool = new BattleFormPool();
        m_OpenServFundPool = new OpenServFundPool();
        m_ArenaPool = new ArenaPool();
        m_CrossArenaPool = new CrossArenaPool();
        m_RebelArmyPool = new RebelArmyPool();
        m_GrabTreasurePool = new GrabTreasurePool();
        m_RunningManPool = new RunningManPool();
        m_RunningManShopPool = new RunningManShopPool();
        m_ViewPlayerLineUpPool = new ViewPlayerLineUpPool();
        m_PatrolPool = new PatrolPool();
        m_RankListPool = new RankListPool();
        m_BroadcastPool = new BroadcastPool();
        m_BroadcastNoticePool = new BroadcastNoticePool();
        m_GameNoticePool = new GameNoticePool();
        m_FanshionableDressPool = new FanshionableDressPool();
        m_NewYearSellingPool = new NewYearSellingPool();
        m_NewYearActivityPool = new NewYearActivityPool();
        m_ValentinesDayPool = new ValentinesDayPool();
        m_NewYearSignPool = new NewYearSignPool();
        //本地数据处理Pool
        m_IconDataPool = new IconDataPool();
        m_DialogPool = new DialogPool();
        m_BlockWordPool = new BlockWordPool();
        m_ReddotMessagePool = new ReddotMessagePool();
        m_GuidancePool = new GuidancePool();
        m_BattleFeatShopPool = new BattleFeatShopPool();

        m_TotalConsumptionPool = new TotalConsumptionPool();

        m_CrossServerBattlePool = new CrossServerBattlePool();
        m_CrossServerBattleShopPool = new CrossServerBattleShopPool();

        m_TariningAndTacticalPool = new TariningAndTacticalPool();
        //TsuCode - ChaosBattle
        m_ChaosBattlePool = new ChaosBattlePool();
        m_ChaosBattleShopPool = new ChaosBattleShopPool();
        //TsuCode - AFK module
        m_AFKPool = new AFKPool();
        //---------------------------

        m_CardBattlePool = new CardBattlePool();

        m_TransmigrationCardPool = new TransmigrationCardPool();//卡牌转生
        m_TransmigrationTreasurePool = new TransmigrationTreasurePool();//法宝转生
        m_TurntablePool = new TurntablePool();//通天转盘
        m_TrialTowerPool = new TrialTowerPool();
        m_SevenStarPool = new SevenStarBlessPool();

        m_EventTimePool = new EventTimePool();
        m_OnlineVipAwardPool = new OnlineVipAwardPool();
        m_RankingPowerAwardPool = new RankingPowerAwardPool();
        m_CrossTournamentPool = new CrossTournamentPool();
        m_CampGemShopPool = new CampGemShopPool();
        m_ShopEventTimePool = new ShopEventTimePool();
        m_ShopEndowPool = new ShopEndowPool();
        m_ShopSeasonPool = new ShopSeasonPool();
    }
}
