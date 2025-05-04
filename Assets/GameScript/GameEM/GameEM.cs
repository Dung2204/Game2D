
public enum EM_UI
{
    /// <summary>
    /// 不显示图标,查找对象为空时不显示
    /// </summary>
    UNSHOW = -99,



}

/// <summary>
/// 场景枚举
/// </summary>
public enum EM_Scene
{
    Login = 0,
    GameMain = 1,
    BattleMain = 2,
}

/// <summary>
/// 语言枚举
/// </summary>
public enum eLanguage
{
    Cn = 1,
    En = 2,
    Zn = 3,
}
public enum EM_NextDaySource
{
    /// <summary>
    /// 副本pool
    /// </summary>
    DungeonPool = 0,
    /// <summary>
    /// 竞技场pool
    /// </summary>
    ArenaPool,
    /// <summary>
    /// 好友pool
    /// </summary>
    FriendPool,
    /// <summary>
    /// 任务pool
    /// </summary>
    TaskDailyPool,
    /// <summary>
    /// 军团副本pool
    /// </summary>
    LegionDungeonPool,

    /// <summary>
    /// 过关斩将Pool
    /// </summary>
    RunningManPool,
}


//方向定义        
//        A上
//    H       B右上
//G               C右
//    F       D右下
//        E下
//ABCDEFGH对应为01234567
/// <summary>
/// 方向枚举
/// </summary>
public enum FACE2WAY
{
    /// <summary>
    /// A上
    /// </summary>
    eWayU = 0,

    /// <summary>
    /// B右上
    /// </summary>
    eWayRU = 1,

    /// <summary>
    /// C右
    /// </summary>
    eWayR = 2,

    /// <summary>
    /// D右下
    /// </summary>
    eWayRD = 3,

    /// <summary>
    /// E下
    /// </summary>
    eWayD = 4,

    /// <summary>
    /// F左下
    /// </summary>
    eWayLD = 5,

    /// <summary>
    /// G左
    /// </summary>
    eWayL = 6,

    /// <summary>
    /// H左上
    /// </summary>
    eWayLU = 7,


}


/// <summary>
/// 动画模板 UITweener类型
/// </summary>
public enum UITweenerType
{
    Position = 0,
    Scale = 1,
    Color = 2,
}


//////////////////////////////////////////////////////////////////////////

#region 本地数据枚举定义
/// <summary>
/// 本地数据类型
/// 通过LocalDataManager获取和保存本地数据
/// 命名要求：根据数据类型的命名前缀'Int'、 'Float'、'String'、'Bool'等基本数据类型，也可以根据类名等作为前缀。
/// 可以接受由基本类型定义的字段或属性封装的类等
/// </summary>
public enum LocalDataType
{
    Float_MusicVolumn = 0,
    Float_EffectVolumn = 1,
    String_UserName = 2,
    String_Password = 3,
    Int_BattleSpeed = 4,
    String_ServerID = 5, //上次登录的服务器Id
    Bool_ShowLog = 6,
    Long_LastLoginUserID = 7,//记录上一次登录的userid,如果切换用户，初始化加速数据等
    /// <summary>
    /// 第一次新手引导是否完成
    /// </summary>
    FirstGuidance = 8,

    String_ServerID2 = 9, //上次登录的服务器Id2
    Long_LastRebelArmyId = 10,//上次攻打的叛军id
    AutoSky = 11,
    SevenStarBlessUp = 12,
    LevelGift = 13,
	EventTimeReddotReset = 14,
    Int_LanguageSetting = 15
}
#endregion

/// <summary>
/// 音效片段类型
/// </summary>
public enum AudioEffectType
{
    TreasureRfine = 1,
    TreasureInten,
    Recycle_01,
    Recycle_02,
    FitMagic,
    CardRefine,
    CardUpLevel,
    TenRaffle,
    Gain_Whith,
    Gain_Orange,
    Gain_Red,
    Gain_Purple,
    Victory1 = 13,
    Victory2 = 14,
    Victory3 = 15,
    BattleFormLight,
    EquipRefine,
    EquipInten,
    EquipStar,
	Intro_Boy,
	Intro_Girl,
	TenRaffle2,

    UI_Close,
    OpenMarket,
    WearEquipment,
    Awaken,
    SkyLife,
    ArtifactUp,
    Rebirth,
    EquipStarUp,
    Crit,
    miss,
}

/// <summary>
/// 音乐类型
/// </summary>
public enum AudioMusicType
{
    LoginBg,   //加载页面
    MainBg,    //主界面
    BattleMusic,  //战斗音效
    challenge,    //挑战界面
    BattleFail, //战斗失败
    BattleVictory,   //战斗胜利
    Loading,   //加载
	ChargeBg,
	Dungeon,
	UI_Challenge,
	Arena,
	Legion,
	UI_Rank,
   
}
public enum AudioButtle
{
    ButtonNormal,
}

/// <summary>
/// 角色属性类型
/// </summary>
public enum EM_RoleProperty
{
    /// <summary>
    /// 攻击
    /// </summary>
    Atk = 1,
    /// <summary>
    /// 生命
    /// </summary>
    Hp,
    /// <summary>
    /// 物防
    /// </summary>
    Def,
    /// <summary>
    /// 法防
    /// </summary>
    MDef,
    /// <summary>
    /// 攻击加成
    /// </summary>
    AddAtk,
    /// <summary>
    /// 生命加成
    /// </summary>
    AddHp,
    /// <summary>
    /// 物防加成
    /// </summary>
    AddDef,
    /// <summary>
    /// 法防加成
    /// </summary>
    AddMDef,
    /// <summary>
    /// 基础怒气
    /// </summary>
    Anger,
    /// <summary>
    /// 活力
    /// </summary>
    Energy = 10,
    /// <summary>
    /// 暴击率
    /// </summary>
    CritR,
    /// <summary>
    /// 抗爆率
    /// </summary>
    AntiknockR,
    /// <summary>
    /// 命中率
    /// </summary>
    HitR,
    /// <summary>
    /// 闪避率
    /// </summary>
    DodgeR,
    /// <summary>
    /// 伤害加成
    /// </summary>
    DamageR,
    /// <summary>
    /// 伤害减免
    /// </summary>
    InjurySR,
    /// <summary>
    /// PVP增伤
    /// </summary>
    PInjuryAR,
    /// <summary>
    /// PVP减伤
    /// </summary>
    PInjurySR,
    /// <summary>
    /// 致命一击
    /// </summary>
    Deadly,
    /// <summary>
    /// 忽视防御发生率
    /// </summary>
    IgDefCR = 20,
    /// <summary>
    /// 忽视防御比例
    /// </summary>
    IgDefR,
    /// <summary>
    /// 吸血发生率
    /// </summary>
    SuckCR,
    /// <summary>
    /// 吸血比例
    /// </summary>
    SuckR,
    /// <summary>
    /// 物理反弹发生率
    /// </summary>
    BounceCR,
    /// <summary>
    /// 物理反弹伤害比例
    /// </summary>
    BounceR,
    /// <summary>
    /// 法术反弹发生率
    /// </summary>
    MBounceCR,
    /// <summary>
    /// 法术反弹伤害比例
    /// </summary>
    MBounceR,
    /// <summary>
    /// 生命恢复发生率
    /// </summary>
    RcvHpCR,
    /// <summary>
    /// 生命恢复伤害比例
    /// </summary>
    RcvHpR,
    /// <summary>
    /// 怒气恢复
    /// </summary>
    RcvAnger = 30,
    /// <summary>
    /// 经验加成
    /// </summary>
    ExpR = 31,
    /// <summary>
    /// 结束
    /// </summary>
    End = 32,

}

/// <summary>
/// 品质(1，白。2，绿。3，蓝。4，紫。5，橙。6，红。7，金)
/// 
/// 金阶神将、红阶神将、橙阶名将、紫阶名将、蓝阶战将、绿阶战将
/// </summary>
public enum EM_Important
{
    None,
    /// <summary>
    /// 白
    /// </summary>
    White = 1,
    /// <summary>
    /// 绿
    /// </summary>
    Green,
    /// <summary>
    /// 蓝
    /// </summary>
    Blue,
    /// <summary>
    /// 紫
    /// </summary>
    Purple,
    /// <summary>
    /// 橙
    /// </summary>
    Oragen,
    /// <summary>
    /// 红
    /// </summary>
    Red,
    /// <summary>
    /// 金
    /// </summary>
    Gold,
    /// <summary>
    /// 暗金
    /// </summary>
    BlackGold,
    /// <summary>
    /// 彩金
    /// </summary>
    ColorGload,
}

/// <summary>
/// 装备属性
/// </summary>
public enum EM_EquipProperty
{
    /// <summary>
    /// 攻击
    /// </summary>
    Atk = 1,
    /// <summary>
    /// 生命
    /// </summary>
    Hp,
    /// <summary>
    /// 物理防御
    /// </summary>
    PhyDef,

    /// <summary>
    /// 魔法防御
    /// </summary>
    MagDef,
    /// <summary>
    /// 攻击加成
    /// </summary>
    AddAtk,
    /// <summary>
    /// 生命加成
    /// </summary>
    AddHp,
    /// <summary>
    /// 物防加成
    /// </summary>
    AddPhyDef,
    /// <summary>
    /// 法防加成
    /// </summary>
    AddMagDef,
    /// <summary>
    /// 暴击率
    /// </summary>
    CriticalHitsR = 11,
    /// <summary>
    /// 抗爆率
    /// </summary>
    ResistCriHitsR,
    /// <summary>
    /// 命中率
    /// </summary>
    ScoreAHitR,
    /// <summary>
    /// 闪避率
    /// </summary>
    DodgeR,
    /// <summary>
    /// 伤害加成
    /// </summary>
    AddHarm,
    /// <summary>
    /// 伤害减免
    /// </summary>
    ReduceHarm,
    /// <summary>
    /// PVP增伤
    /// </summary>
    PVPAddHarm,
    /// <summary>
    /// PVP减伤
    /// </summary>
    PVPReduceHarm,
    /// <summary>
    /// 致命一击
    /// </summary>
    CriticalStrike
}
/// <summary>
/// 能否被叠加
/// 1，不可以，>1，叠加数量
/// </summary>
public enum EM_IsOverlay
{
    /// <summary>
    /// 不能被叠加
    /// </summary>
    NoOverlay = 1,
    /// <summary>
    /// 可以以及叠加数量
    /// </summary>
    CanOverlay
}

/// <summary>
/// 道具能否被使用
/// 1，不可使用。2，可使用【背包中该类道具显示使用按钮】。3，可一键使用【背包中该类道具显示使用和一键使用2个按钮】
/// </summary>
public enum EM_IsUse
{
    /// <summary>
    /// 不可使用
    /// </summary>
    NoUse = 1,
    /// <summary>
    /// 可使用
    /// </summary>
    CanUse,
    /// <summary>
    /// 可一键使用
    /// </summary>
    RapidUse
}
/// <summary>
/// 强化成就
/// 1、装备强化。2、装备精炼。3、宝物强化。4、宝物精炼					
/// </summary>
public enum EM_Master
{
    /// <summary>
    /// 装备强化
    /// </summary>
    EquipIntensify = 1,
    /// <summary>
    /// 装备精炼
    /// </summary>
    EquipRefine,
    /// <summary>
    /// 宝物强化
    /// </summary>
    TreasureIntensify,
    /// <summary>
    /// 宝物精炼
    /// </summary>
    TreasureRefine
}

/// <summary>
/// 玩家个人属性
/// </summary>
public enum EM_UserAttr
{
    eUserAttr_None = 0,
    eUserAttr_MainCard = 1,     // 主卡(确定头像)
    eUserAttr_IconFrame = 2,    // 头像边框     (该值服务器暂未使用)
    eUserAttr_CardPower = 3,    // (上阵)卡牌战力  (该值服务器暂未使用)
    eUserAttr_Level = 4,        // 等级
    eUserAttr_Exp = 5,          // 经验   (该值服务器暂未使用)
    eUserAttr_Vip = 6,          // Vip
    eUserAttr_Sycee = 7,        // 元宝(钻石)
    eUserAttr_Money = 8,        // 银币(游戏币)
    eUserAttr_Gold = 9,         // 黄金(没有黄金)
    eUserAttr_GodSoul = 10,     // 神魂
    eUserAttr_Fame = 11,        // 声望
    eUserAttr_BattleFeat = 12,  // 战功
    eUserAttr_GeneralSoul = 13, // 将魂
    eUserAttr_Prestige = 14,    // 威名
    eUserAttr_Vigor = 15,       // 精力
    eUserAttr_Energy = 16,      // 体力
    eUserAttr_CrusadeToken = 17,// 征讨令
    eUserAttr_LegionContribution = 18,       //军团贡献
    eUserAttr_CrossServerScore = 19,       //跨服积分
    eUserAttr_ChaosScore = 20, //TsuCode - ChaosBattle
    eUserAttr_Coin = 21, //TsuCode - Coin - Kim Phieu
    eUserAttr_ArenaCorssMoney = 22, //xu dinh cao
    eUserAttr_TournamentPoint = 23, //điểm vấn đỉnh
    eUserAttr_TempVip = 24, //điểm giảm vip
    /// <summary>
    /// 结束
    /// </summary>
    End = 32,

    eUserAttr_TaskScore = 999, // 任务积分 只在前端使用，用于显示奖励
};

/// <summary>
/// 通用奖励 来源枚举
/// </summary>
public enum EM_AwardSource
{
    eAward_Dungeon = 1,  //副本奖励
    eAward_Sweep = 2,    //扫荡奖励
    eAward_Lottery = 3,
    eAward_UseItem = 4,
    eAward_BattleFormation = 5,
    eAward_Cdkey = 6,		// 激活码奖励
}


public enum EM_CardType
{
    RoleCard = 1,    //主角卡牌
    NormalCard = 2, //普通卡牌

    CaiWenji = 1416,   //蔡文姬
    HuangYueYing = 1217,  //黄月英
}

/// <summary>
/// 战斗类型枚举
/// </summary>
public enum EM_Fight_Enum
{
    eFight_Invalid = 0, //无效的
    eFight_DungeonMain = 1, //主线
    eFight_DungeonElite = 2, //精英
    eFight_Legend = 3, //传说
    eFight_DailyPve = 4, //日常

    eFight_Rebel = 6,  //叛军
    eFight_Friend = 7, //好友切磋
    eFight_Guild = 8,  //工会战

    eFight_Arena = 10,//竞技场
    eFight_ArenaSweep = 11, //竞技场扫荡
    eFight_CrusadeRebel = 12,   //叛军入侵
    eFight_Boss = 13,  //BOSS战 

    eFight_LegionDungeon = 14, //军团副本
    eFight_GrabTreasure = 15, //夺宝
    eFight_GrabTreasureSweep = 16,		//夺宝扫荡

    eFight_RunningMan = 18, //过关斩将
    eFight_RunningManElite = 19, //过关斩将精英挑战

    eFight_Patrol = 20, //领地巡逻挑战

    eFight_LegionBattle = 21, //军团战

    eFight_CrossServerBattle = 22, //跨服战

    eFight_CardBattle = 23, //跨服斗将

    eFight_TrialTower = 24,   //试炼之塔
    //TsuCode - ChaosBattle
    eFight_ChaosBattle = 25, //Chaosbattle
    //
    eFight_ArenaCross = 26,//竞技场

}

public enum EM_BattleBgId
{
    Dungeon = 1,   //副本
    DailyPve = 2,   //日常
    ArenaOrGrabTreasure = 3,   //竞技和夺宝
    LegionDungeon = 4,   //公会战
    Invalid = 5, //保留
	Patrol = 1,
	Rebel = 2,
	ChaosBattle = 5,
	CrossServerBattle = 7,
	RunningMan = 8,
}

/// <summary>
/// 由战斗场景切回主场景需要处理的相关内容
/// </summary>
public enum EM_Battle2MenuProcess
{
    None = 0,
    /// <summary>
    /// 副本相关处理
    /// </summary>
    Dungeon = 1,

    /// <summary>
    /// 竞技场相关处理
    /// </summary>
    Arena = 2,

    /// <summary>
    /// 叛军入侵相关处理
    /// </summary>
    CrusadeRebel = 3,

    /// <summary>
    /// 夺宝相关处理
    /// </summary>
    GrabTreasure = 4,

    /// <summary>
    /// 过关斩将处理
    /// </summary>
    RunningMan = 5,

    /// <summary>
    /// 过关斩将精英处理
    /// </summary>
    RunningManElite = 6,

    /// <summary>
    /// 战斗失败 提升处理
    /// </summary>
    BattleLose = 7,

    /// <summary>
    /// 古都巡礼
    /// </summary>
    Patrol = 8,

    /// <summary>
    /// 军团副本处理
    /// </summary>
    LegionDungeon = 9,

    /// <summary>
    /// 军团战处理
    /// </summary>
    LegionBattle = 10,

    /// <summary>
    /// 跨服战处理
    /// </summary>
    CrossServerBattle = 11,
    /// <summary>
    /// 斗将处理
    /// </summary>
    CardBattle = 12,

    /// <summary>
    /// 试炼之塔
    /// </summary>
    TrialTower = 13,
    ChaosBattle = 14, //TsuCode - ChaosBattle
    ArenaCrossBattle = 15,
}

public enum EM_BattleLoseProcess
{
    /// <summary>
    /// 招募卡牌
    /// </summary>
    GetMoreCard = 0,
    /// <summary>
    /// 卡牌强化
    /// </summary>
    CardIntensify = 1,
    /// <summary>
    /// 装备强化
    /// </summary>
    EquipIntensify = 2,
    /// <summary>
    /// 阵容改变
    /// </summary>
    LineupChange = 3,
}

/// <summary>
/// 宝箱类型
/// </summary>
public enum EM_BoxType
{
    Chapter = 0, //章节宝箱
    Tollgate = 1, //关卡宝箱
    Task = 2, //任务宝箱
}

/// <summary>
/// 宝箱领取状态
/// </summary>
public enum EM_BoxGetState
{
    Lock = 0, //不可领取
    CanGet = 1, //可领取状态
    AlreadyGet = 2, //已经领取
    Invalid = 3, //无效的
}
/// <summary>
/// 主线任务状态
/// </summary>
public enum EM_MainTaskState
{
    Lock = 0,//未开启
    Doing = 1,//已开启但还未完成
    CanGetAward = 2,//任务已完成可领取奖励
}
/// <summary>
/// 奖励领取状态
/// </summary>
public enum EM_AwardGetState
{
    Lock = 0,    //不可领取
    CanGet = 1,  //可领取
    AlreadyGet = 2, //已经领取
}

public enum EM_CommonItemShowType
{
    All = 0, //展示名字和数量等全部内容
    Icon = 1,//只展示Icon
}

public enum EM_CommonItemClickType
{
    AllTip = 0, //点击统一显示为Tip
    Normal = 1, //按类型响应
    Goods = 2,   //道具
    EquipManage = 6,//跳转到装备界面
    CardManage = 4,//跳转到卡牌界面
    TreasureManage = 8,//跳转到法宝界面
}

/// <summary>
/// 游戏的资源类型
/// </summary>
public enum EM_ResourceType
{
    Money = 1, //货币
    Good = 2, //道具
    AwakenEquip = 3, //领悟装备
    Card = 4, //卡牌整卡
    CardFragment = 5,//卡牌碎片
    Equip = 6, //装备
    EquipFragment = 7, //装备碎片
    Treasure = 8, //宝物
    TreasureFragment = 9, //宝物碎片
    Fashion = 10,           // 时装
    GodEquip = 11, //than binh
    GodEquipFragment = 12, //mảnh than binh

}
/// <summary>
/// 货币类型
/// 7-元宝 8-银币 9- 黄金 10-神魂 11-声望 12-战功 13-将魂 14-威名 15-精力 16-体力 17-征讨令 18 军团贡献  19跨服积分
/// </summary>
public enum EM_MoneyType
{
    eUserAttr_None = 0,//不存在
    eUserAttr_Sycee = 7,        // 元宝
    eUserAttr_Money = 8,        // 银币
    eUserAttr_Gold = 9,         // 黄金
    eUserAttr_GodSoul = 10,     // 神魂
    eUserAttr_Fame = 11,        // 声望
    eUserAttr_BattleFeat = 12,  // 战功
    eUserAttr_GeneralSoul = 13, // 将魂
    eUserAttr_Prestige = 14,    // 威名
    eUserAttr_Vigor = 15,       // 精力
    eUserAttr_Energy = 16,      // 体力
    eUserAttr_CrusadeToken = 17,// 征讨令
    eUserAttr_LegionContribution = 18,       //军团贡献
    eUserAttr_CrossServerScore = 19, //跨服积分
    eUserAttr_ChaosScore = 20, //TsuCode -ChaosBattle - ChaosScore
    eUserAttr_Coin = 21, //TsuCode -Coin - KimPhieu
    eUserAttr_ArenaCorssMoney = 22, //Xu dinh cao
    eUserAttr_TournamentPoint = 23, //điểm vấn đỉnh

    eCardRefineStone = 100,//进阶石
    eCardAwakenStone = 106,  //领悟石(领悟单)
    eCardArtifact = 107,  //进阶丹 (神器)
    eCardSkyPill = 108,    //天命丹(道具)
    eBattleFormFragment = 109,//三国残卷(道具)
    eRedEquipElite = 110, //红色装备精华
    eRedBattleToken = 111,//红色武将精华
    eFreshToken = 203,//刷新令
    eTreasureRefinePill = 105,   // 法宝精炼石
    eNorAd = 206,//战将令(道具)
    eGenAd = 207,//神将令(道具)
    eCampAd = 501,// vé quay phe
    eLimitAd = 502,// vé quay giới hạng
    eGemCamp = 503,// đá phe
}

#region 背包相关的枚举
/// <summary>
/// 卡牌背包类型
/// </summary>
public enum CardBagType
{
    Card = 0, //整卡
    Fragment, //碎片
}
public enum EquipBagType
{
    Equip = 0, //一件
    Fragment, //碎片
}

public enum TreasureBagType
{
    Treasure,
    Fragment,
}

public enum GodEquipBagType
{
    Equip = 0, //hoan chinh
    Fragment, //manh vo
}
#endregion

public enum CrossFightType
{
    CrossBattle = 0, //整卡
    CrossArena = 1, //碎片
}
public enum EM_CardCamp
{
    eCardMain = 0,//主角
    eCardWei = 1,//魏国
    eCardShu = 2,//蜀国
    eCardWu = 3,//吴国
    eCardGroupHero = 4,//群雄
    eCardNo,//无效
}

#region 商店枚举相关
public enum EM_ShopType
{
    Card = 1,//卡牌商店
    Awake = 2,//领悟商店
}
//不只6个的商店，可以拖动，并且有分页
public enum EM_ShopMutiType
{
    Reputation = 3,//声望商店
    Legion = 4,//军团商店
    RunningMan = 5, //过关斩将
    BattleFeatShop = 6,  //战功商店
    CrossServerBattle = 7, //跨服战商店
    ChaosBattle = 8, //TsuCode - CHaosBattle - Shop
    CrossArena= 9,
    CrossTournament = 10, // shop vấn đỉnh
    CampGemShop = 11,  //shop đá phe
}
#endregion

//卡牌定位类型：1，物攻型。2，法攻型。3，辅助型。4，防御型

public enum EM_CardFightType
{
    eCardAll = 0,// toàn nghề
    eCardPhyAtt = 1,    //物攻
    eCardMagAtt,   //法攻型
    eCardSup,       //辅助型
    eCardTank,    //防御型
    eCardKiller,    //kích tướng
    eCardPhysician,    //y sư
    eCardLogistics,    //hậu cần
}

public enum EM_CardEle
{
    eCardEle1 = 1,    //Kim
    eCardEle2,   // mộc 
    eCardEle3,       //thủy
    eCardEle4,    //hỏa
    eCardEle5,    // thổ
}

// 道具使用方式枚举
public enum EM_ItemUse
{
    eItemEffect_None = 1,       // 不可使用
    eItemEffect_Use = 2,        // 可使用
    eItemEffect_BatchUse = 3,   // 可批量使用
    eItemEffect_Jump = 4,		// 跳转界面(纯前端功能)
};
/// <summary>
/// 加成属性对象
/// </summary>
public enum EM_ProTarget
{
    eMyself = 1,
    eAllCard,
    eAllWei,
    eAllShu,
    eAllWu,
    eAllQun,
}


// 道具效果枚举
public enum EM_ItemEffect
{
    eItemEffect_Energy = 1,
    eItemEffect_Vigor = 2,
    eItemEffect_CrusadeToken = 3,
    eItemEffect_RandAward = 4,
    eItemEffect_ChoseAward = 5,
    eItemEffect_CardExp = 6,
    eItemEffect_EquipRefineExp = 7,
    eItemEffect_FixedAward = 8,
};

// 阵型站位枚举
public enum EM_FormationPos
{
    eFormationPos_Main = 0,     // 主
    eFormationPos_Assist1,      // 辅助1
    eFormationPos_Assist2,
    eFormationPos_Assist3,
    eFormationPos_Assist4,
    eFormationPos_Assist5,
    eFormationPos_Assist6,
    eFormationPos_Pet,          // 战宠
    eFormationPos_Reinforce,    // 援军
    eFormationPos_INVALID
};
public enum EM_FormationSlot
{
    eFormationSlot_Assist0 = 0,     // 主
    eFormationSlot_Assist1,      // 辅助1
    eFormationSlot_Assist2,
    eFormationSlot_Assist3,
    eFormationSlot_Assist4,
    eFormationSlot_Assist5,
    eFormationSlot_Assist6,
    eFormationSlot_Assist7,
    eFormationSlot_Assist8,
    eFormationSlot_Assist9,
    eFormationSlot_Assist10,
    eFormationSlot_Assist11,
    eFormationSlot_Assist12,
    eFormationSlot_Assist13,
    eFormationSlot_Assist14,
    eFormationSlot_Assist15,
    eFormationSlot_Assist16,
    eFormationSlot_Assist17,
    eFormationSlot_Assist18,
    eFormationSlot_Assist19,
    eFormationSlot_INVALID
};
// 援军站位枚举
public enum EM_ReinforcePos
{
    eReinforcePos_B1 = 0,
    eReinforcePos_B2,
    eReinforcePos_B3,
    eReinforcePos_B4,
    eReinforcePos_B5,
    eReinforcePos_B6,
};
public enum EM_Equip
{
    eEquipPart_Weapon = 1,   //武器
    eEquipPart_Armour,      // 铠甲
    eEquipPart_Helmet,      // 头盔
    eEquipPart_Belt,        // 腰带
}
public enum EM_Treasure
{
    eEquipPare_MagicLeft = 5,        //法宝左
    eEquipPare_MagicRight = 6,       //法宝右

    HuFu = 5004,   //虎符
}

public enum EM_GodEquip
{
    eEquipPart_GodWeapon = 7,
}
// 装备部位枚举
public enum EM_EquipPart
{
    eEquipPart_NONE = 0,
    eEquipPart_Weapon = 1,   //武器
    eEquipPart_Armour,      // 铠甲
    eEquipPart_Helmet,      // 头盔
    eEquipPart_Belt,        // 腰带
    eEquipPare_MagicLeft,        //法宝左
    eEquipPare_MagicRight,       //法宝右
    eEquipPart_GodWeapon,       // than binh
    eEquipPart_INVALID,          //无效的
                                 

    Canglang = 3000,    //苍狼枪刃  
};
/// <summary>
/// 列阵部位
/// </summary>
public enum EM_CloseArrayPos
{
    eCloseArray_PosOne = 0,//位置1（一手）
    eCloseArray_PosTwo,//位置2（二手）
    eCloseArray_PosThree,//位置3（三手）
    eCloseArray_PosFour,//位置4
    eCloseArray_PosFive,//位置5
    eCloseArray_PosSix,//位置6
    eCloseArray_Pos7,
    eCloseArray_Pos8,
    eCloseArray_Pos9,
    eCloseArray_Pos10,
    eCloseArray_Pos11,
    eCloseArray_Pos12,
    eCloseArray_Pos13,
    eCloseArray_Pos14,
    eCloseArray_Pos15,
    eCloseArray_Pos16,
    eCloseArray_Pos17,
    eCloseArray_Pos18,
    eCloseArray_Pos19,
    eCloseArray_Pos20,
}

/// <summary>
/// 战斗阵营
/// </summary>
public enum EM_Factions
{
    /// <summary>
    /// 玩家A阵营
    /// </summary>
    ePlayer_A,
    /// <summary>
    /// 敌方B阵营
    /// </summary>
    eEnemy_B,


}


/// <summary>
/// 角色类型
/// </summary>
public enum EM_RoleType
{
    /// <summary>
    /// 玩家角色
    /// </summary>
    ePlayerRole = 1,

    /// <summary>
    /// 电脑角色
    /// </summary>
    eNpcRole = 2,
}

/// <summary>
/// 招募类型
/// </summary>
public enum EM_RecruitType
{
    /// <summary>
    /// 战将
    /// </summary>
    NorAd = 1,
    /// <summary>
    /// 神将
    /// </summary>
    GenAd = 2,
    CampAd = 3,
}


/// <summary>
/// 战斗出手顺序
/// </summary>
public enum EM_BattleIndex
{
    A1 = 0,
    A2,
    A3,
    A4,
    A5,
    A6,
    A7,
    A8,
    A9,
    A10,
    A11,
    A12,
    A13,
    A14,
    A15,
    A16,
    A17,
    A18,
    A19,
    A20,
    B1 = 20,
    B2,
    B3,
    B4,
    B5,
    B6,
    B7,
    B8,
    B9,
    B10,
    B11,
    B12,
    B13,
    B14,
    B15,
    B16,
    B17,
    B18,
    B19,
    B20,
    AA1 = 40,
    AA2,
    AA3,
    AA4,
    AA5,
    AA6,
    AA7,
    AA8,
    AA9,
    AA10,
    AA11,
    AA12,
    AA13,
    AA14,
    AA15,
    AB16,
    AB17,
    AB18,
    AB19,
    AB20,
    AB1 = 60,
    AB2,
    AB3,
    AB4,
    AB5,
    AB6,
    AB7,
    AB8,
    AB9,
    AB10,
    AB11,
    AB12,
    AB13,
    AB14,
    AB15,
    AA16,
    AA17,
    AA18,
    AA19,
    AA20,
    AAC1 = 80,
    AAC2,
    AAC3,
    ABC1 = 83,
    ABC2,
    ABC3,
    My,
    MyLeft,
    MyRight,
}



/// <summary>
/// 活动类型
/// </summary>
public enum EM_ActivityType
{
    DaySignIn = 1,//每日签到
    GrandSignIn = 2,//豪华签到
    Banquet = 3,//铜雀台 饭宴领体力（已改名宫宴）
    LuckySymbol = 4,//招财符
    WealthMan = 6,//迎财神
    LoginGift = 7,//登录送礼（限时）
    OpenServFund = 8,//开服基金
    OpenWelfare = 9,//全民福利
    MonthCard = 10,//月卡
    OnlineAward = 11,//在线奖励
    SkyFortune = 12,//天降横财
    LoginGiftNewServ = 13,//登录送礼（限时）新服豪礼
    ExchangeCode = 14,//礼品码
    TenSycee = 15,//十万元宝
    VipGift = 16,//vip礼包
    FirstRecharge = 17,//首充
    WeekFund = 18,//周基金
    ExclusionSpin= 19, // 
    End,//结束
}
/// <summary>
/// 新春活动类型
/// </summary>
public enum EM_NewYearActType
{
    RedPacketExchange,//红包兑换
    RedPacketTask,//红包任务
    NewYearSelling,  //新年贩售
    MammonSendGifts,  //财神送礼
    ValentinesDay,   //情人节
    DogStep,  //萌犬闹新春
    RecruitGift,    //神将招募有礼
    SingleRecharge, //单笔充值
    MutiRecharge, //累计充值
    TotalConst, //累计消费
    MammonSendGiftsNew,   //财神送礼(新)
    NewYearSign,    //新年豪华签到
    FestivalExchange,  //节日兑换
    DealsEveryDay,
    ExclusionSpin,
}
public enum EM_VipPrivilege
{
    eVip_LvExp = 1,             // 累计获得【x】VIP经验即可提升至下一级
    eVip_LootTreasure5Times = 2,// 开启夺宝5次（VIP3并且到达16级开启）
    eVip_OpenSweep = 3,         // 主线副本开启扫荡功能
    eVip_LootTreasureOnkey = 4, // 开启一键夺宝
    eVip_RefineOnekey = 5,      // 开启一键精炼功能
    eVip_BattleTrebleSpeed = 6, // 开启战斗三倍速
    eVip_GeneralBag = 7,        // 武将背包额外增加【x】
    eVip_TreasureBag = 8,       // 宝物背包额外增加【x】
    eVip_EquipUpstarOnkey = 9,  // 装备升星可以使用一键操作
    eVip_TreasureUpOnkey = 10,  // 宝物可以使用一键升级
    eVip_EquipCritDoubleLv1 = 11,    // 装备强化暴击提升1级
    eVip_EquipCritDoubleLv2 = 12,    // 装备强化暴击提升2级
    eVip_AwakenShopRefresh = 13,    // 领悟商店每日可以刷新【x】次
    eVip_CallValuablesTimes = 14,   // 招财符，每日可以招财【x】次
    eVip_TollgateReset = 15,        // 主线副本每日可以重置【x】次
    eVip_ShopGodGeneralRefresh = 16,// 神将商店每日可以刷新【x】次
    eVip_BuyCrusadeToken = 17,      // 每日可以购买征讨令【x】个
    eVip_BuyOrangeTreasureBox = 18, // 每天可以购买【x】次橙色宝物箱子
    eVip_BuyOrangeEquipBox = 19,    // 每天可以购买【x】次橙色装备箱子
    eVip_BuyAdvancedExpStone = 20,  // 每天可以购买【x】次高级经验石
    eVip_BuyVigorPill = 21,         // 每天可以购买【x】次精力丹
    eVip_BuyEnergyPill = 22,    // 每天可以购买【x】次体力丹
    eVip_BuyMoney = 23,         // 每天可以购买【x】次银两
    eVip_FamousGeneral = 24,    // 名将副本每天可以额外攻打【x】次
    eVip_LegionDungeonBuyTimes = 25, //军团副本每天可购买挑战次数：【x】
    eVip_RunningManResetTimes = 26, //过关斩将每天可以手动重置【x】次
    eVip_RunningManEliteBuyTimes = 27, //过关斩将精英副本每天可购买挑战次数：【x】
    eVip_DungeonSkip = 28, //副本战斗直接跳过（VIP3或40级）
    eVip_CrossServerBattleBuyTimes = 29, //跨服战（跨服演武）挑战次数的VIP购买次数
    eVip_CrusadeRebel = 30,  //叛军入侵跳过需要的VIP等级
    eVip_ChaosBattleBuyTimes = 31, //TsuCode - ChaosBattle
    eVip_AFKBonus = 32, //TsuCode - ChaosBattle
    eVip_AFKBonusResources = 33, // tỉ lệ %: tài nguyên afk
    eVip_EnergyLimit = 34, // giới hạn thể lực
    eVip_INVALIDE,
}

public enum ePayFromConfig
{
    ePay_Pay = 0,       // SDK³äÖµ
    ePay_LevelGift = 1,     // ²âÊÔÖ±½Ó³äÖµ
    ePay_TripleMoney = 2,   // ºóÌ¨Ä£Äâ³äÖµ
    ePay_BattlePass = 3,	// ºóÌ¨Ä£Äâ³äÖµ
    ePay_DealsEveryDay = 4	// ºóÌ¨Ä£Äâ³äÖµ
};


#region 剧情对话 相关枚举

/// <summary>
/// 对话位置：左，右
/// </summary>
public enum EM_DialogAnchor
{
    Dialog_Left = 1,
    Dialog_Right = 2,
}

/// <summary>
/// 对话触发条件
/// </summary>
public enum EM_DialogcCondition
{
    BattleStart = 1,//战斗开始
    BattleFinish = 2,//战斗结束
    RoleBeforDeath = 3,//战斗某个站位死亡触发
    ForPlotSys = 4,  //剧情系统使用
}

#endregion

#region 任务 相关枚举

public enum EM_TaskType
{
    Daily = 0, //日常任务
    Achievement = 1, //成就任务
}

public enum EM_TaskState
{
    Finish = 0, //完成
    Unfinished = 1,  //未完成 
    AlreadyAward = 2, //已经领取了奖励
}

public enum EM_DailyTaskCondition
{
    MainTollgate = 1,         // 主线副本x次
    LegendTollgate = 2,       // 名将副本x次
    RunningMan = 3,         // 过关斩将x次
    Arena = 4,                // 竞技场x次
    TreasureSynthesis = 5,    // 法宝合成x次
    EquipIntensify = 6,       // 装备强化x次
    TreasureIntensify = 7,    // 法宝强化x次
    Train = 8,                // 培养x次
    EquipRefine = 9,          // 装备精炼x次
    SendVigor2Friend = 10,    // 赠送精力给x好友
    ShopBattleGeneral = 11,   // 商城战将抽x次
    ShopGodGeneral = 12,      // 商城神将抽x次
    CrusadeRebel = 13,        // 征讨叛军x次
    ShareCrusadeRebel = 14,   // 分享叛军x次
    ShopVigorPill = 15,       // 商城购买精力丹x个
    ShopEnergyPill = 16,      // 商城购买体力丹x个
    HelpAgainstRiot = 17,     // 助阵好友镇压暴动x次
    Patrol = 18,              // 领地巡逻x小时
    EliteTollgate = 19,       // 精英副本x次
    DailyTollgate = 20,       // 日常副本x次
    CardLevelUp = 21,         //武将升X级
    CardEnvolveUp = 22,       //武将进阶X级
    EquipRefine2 = 23,         //装备精炼x次(用9)
    TreasureRefine = 24,       //法宝精炼X级
    CardSky = 25,              //武将命星升X级
    CardAwake = 26,            //武将领悟X次
    CardUpStar = 27,           //装备升星X次
    BattleForm = 28,           //点亮阵图X次
    Reinforce = 29,            //上阵X个援军
    MainCardLvUp = 30,         //主角升到XX级
}

//前端需要处理的Id  {1,2,3,4,5,6,7,11,12,13,14,15}
public enum EM_AchievementTaskCondition
{
    eAchv_Level = 1,            // 等级到达【x】级
    eAchv_MainTollgateStars = 2,// 主线冒险星数达到【x】星
    eAchv_Power = 3,            // 战斗力达到【x】                           
    eAchv_Formation6CardLv = 4,     // 强化六名上阵卡牌等级到【x】级
    eAchv_Formation6CardEvolve = 5, // 六名上阵卡牌突破等级到达【x】级
    eAchv_RunningManStars = 6,    // 过关斩将达到【x】星
    eAchv_VipLv = 7,            // VIP等级达到【x】
    eAchv_HelpAgainstRiot = 8,  // 帮助好友解决暴动事件【x】次    次数由服务器处理
    eAchv_Patrol = 9,           // 领地累积巡逻【x】小时          次数由服务器处理
    eAchv_CrusadeRebel = 10,    // 累积消灭叛军【x】次            次数由服务器处理
    eAchv_EliteTollgateStars = 11,      // 精英副本星数达到【x】星
    eAchv_Formation6CardAwaken = 12,    // 六名上阵卡牌领悟达到【x】星
    eAchv_EliteChapters = 13,           // 通关精英副本第【x】章
    eAchv_4EquipFormationCards = 14,    // 上阵【x】个卡牌穿齐四件装备
    eAchv_2TreasureFormationCards = 15, // 上阵【x】个卡牌穿戴齐法宝
}

#endregion

/// <summary>
/// 服务器状态
/// </summary>
public enum EM_ServerState
{
    New = 1,//新
    Hot = 2,//火热
    UnOpen = 3,//关闭状态（不显示在服务器列表）
    Maintain = 4,//维护
    Unhindered = 5,//顺畅
    Preheat = 6,//预热（仅显示在选择服务器列表）
    Max = 7,
}


/// <summary>
/// 玩家信息读取进度
/// 后面进度的数据包含前面的信息
/// </summary>
public enum EM_ReadPlayerStep
{
    /// <summary>
    /// 只读取基础信息
    /// </summary>
    Base = 0,
    /// <summary>
    /// 读取好友附加信息
    /// </summary>
    Extend1 = 1,
    /// <summary>
    /// 读取军团附加信息
    /// </summary>
    Extend2 = 2,
}
/// <summary>
/// 好友列表类型
/// </summary>
public enum EM_FriendListType
{
    Friend = 0, //好友信息
    Vigor = 1, //领取精力
    Blacklist = 2, //黑名单
    Apply = 3, //好友申请列表
    Recommend = 4, //推荐列表
    END, //结尾标记
}

/// <summary>
/// 赠送体力情况
/// </summary>
public enum EM_VigorFlag
{
    Donate = 0,     //我赠送给好友的
    CanGet = 1,     //好友赠送给我，可领取
    AlreadyGet = 3, //好友赠送给我，已领取
}

/// <summary>
/// 聊天内容
/// </summary>
public enum EM_ChatChan
{
    /// <summary>
    /// 世界频道
    /// </summary>
    eChan_World = 1,
    /// <summary>
    /// 军团
    /// </summary>
    eChan_Legion = 2,
    /// <summary>
    /// 队伍
    /// </summary>
    eChan_Team = 3,
    /// <summary>
    /// 好友私聊
    /// </summary>
    eChan_Private = 4,
    eChan_System = 5,

    eChan_INVALID
};

#region 道具相关
/// <summary>
/// 道具使用类型
/// </summary>
public enum EM_GoodsEffect
{
    /// <summary>
    /// 获得体力
    /// </summary>
    GetPhysical = 1,
    /// <summary>
    /// 获得精力
    /// </summary>
    GetVigor,
    /// <summary>
    /// 获得讨伐
    /// </summary>
    GetKill,
    /// <summary>
    /// 随机奖励
    /// </summary>
    RandomRewards,
    /// <summary>
    /// 可选奖励
    /// </summary>
    OptionalReward,
    /// <summary>
    /// 卡牌经验
    /// </summary>
    CardExp,
    /// <summary>
    /// 装备精炼经验
    /// </summary>
    EquipRefineExp,
    /// <summary>
    /// 固定奖励
    /// </summary>
    FixedReward,
    MainCardLvUp1,
    /// <summary>
    /// tìm item tinh luyện thần binh
    /// </summary>
    GodEquipRefineExp,
}

public enum EM_GoodsGotoPage
{
    /// <summary>
    ///卡牌进阶
    /// </summary>
    CardRefine,
    /// <summary>
    /// 卡牌升级
    /// </summary>
    CardUpLv,
    /// <summary>
    /// 卡牌培养
    /// </summary>
    Cardcultivate,
    /// <summary>
    /// 卡牌天命
    /// </summary>
    CardSky,
    /// <summary>
    /// 卡牌领悟(领悟)
    /// </summary>
    CardAwaken,
    /// <summary>
    /// 法宝精炼
    /// </summary>
    TreasureRefine,
    /// <summary>
    /// 装备精炼
    /// </summary>
    EquipRefine,
    /// <summary>
    /// 阵图    已改铸剑
    /// </summary>
    BattleFormPage,
    /// <summary>
    /// 招募
    /// </summary>
    Shop,

    /// <summary>
    /// 送花
    /// </summary>
    ValentinesDay,
    /// <summary>
    /// 神器升级
    /// </summary>
    ArtifactUp,
    /// <summary>
    /// 神器进阶
    /// </summary>
    ArtifactRefine,
    /// <summary>
    /// 试炼之塔扫荡
    /// </summary>
    TrialTowerSweep,
    /// <summary>
    /// 试炼之塔挑战
    /// </summary>
    TrialTowerChalleng,
    /// <summary>
    /// 七星命灯
    /// </summary>
    SevenStarPage,
}

#endregion

public enum EM_RecyleOrRebirth
{
    None,
    /// <summary>
    /// 装备回收
    /// </summary>
    EquipRecyle,
    /// <summary>
    /// 装备重生
    /// </summary>
    EquipRebirth,
    /// <summary>
    /// 卡牌回收
    /// </summary>
    CardRecyle,
    /// <summary>
    /// 卡牌重生
    /// </summary>
    CardRebirth,
    /// <summary>
    /// 法宝重生
    /// </summary>
    TreasureRebirth,
    /// thu hoi than binh
    /// </summary>
    GodEquipRebirth,
}

public enum EM_RoleSex
{
    Man = 0,    //男
    Woman = 1, //女
}

public enum EM_ArenaResult
{
    Lose = 0,  //失败
    Win = 1,   //胜利
}


#region 戰斗相關

public enum EM_AttackType
{
    //0基本攻击伤害 1附加伤害 2前段BUF显示效果 3前段BUF伤害 4后段BUF伤害 5后段BUF显示效果

    /// <summary>
    /// 0基本攻击伤害 
    /// </summary>
    eBase = 0,

    /// <summary>
    /// 1附加伤害 
    /// </summary>
    eExt = 1,

    /// <summary>
    /// 2前段BUF显示效果
    /// </summary>
    eRoleBeginEffect = 2,

    /// <summary>
    /// 前段BUF伤害 
    /// </summary>
    eBufBegin = 3,

    /// <summary>
    /// 后段BUF伤害 
    /// </summary>
    eBufEnd = 4,

    /// <summary>
    /// 5后段BUF显示效果
    /// </summary>
    eRoleEndEffect = 5,

    eRoleGodEquipEffect = 6,

    eRoleBuffIcon = 7
}

/// <summary>
/// 攻击类型
/// </summary>
public enum EM_HpChangeType
{
    eHpNormal = 0, //普通攻击
    eHpDeadly = 1, //致命一击
    eHpCritical = 2, //暴击
    eHpDodge = 3, //闪避
    eHpFire = 4, //灼烧
    eHpPoison = 5, //中毒
    eHpTreat = 6, //治疗
    // thêm mới
    eHpPride = 7,//kiêu hãnh
    eHpPersist = 8,//kiên trì
    eHpIndomitable = 9,//bất khuất

    eHpDied = 99,//死亡
}

/// <summary>
/// 技能攻击顺序
/// </summary>
public enum EM_MagicAttackIndex
{
    /// <summary>
    /// 1基础攻击
    /// </summary>
    eBase = 1,

    /// <summary>
    /// 2附加攻击1
    /// </summary>
    eExt1 = 2,

    /// <summary>
    /// 3附加攻击2
    /// </summary>
    eExt2 = 3,

    /// <summary>
    /// 4Buf攻击1
    /// </summary>
    eBuf1 = 4,

    /// <summary>
    /// 4Buf攻击2
    /// </summary>
    eBuf2 = 5,

}

/// <summary>
/// 攻击进度
/// </summary>
public enum EM_AttackStep
{
    AttackC = 1,
    AttackH = 2,
    AttackB = 3,

}

/// <summary>
/// 战斗场景分层
/// </summary>
public enum EM_BattleDepth
{
    TOP_UI = 500,
    UI = 300,
    HP = 200,
    AttackHP = 101,
    Attack = 100,
    BeAttackHP = 91,
    BeAttack = 90,
    MaskBg = 60,
    OtherHP = 51,
    Other = 50,
    GameBg = 0,
}

/// <summary>
/// 戰斗MP更新類型
/// </summary>
public enum EM_BattleMpType
{
    /// <summary>
    /// 正常怒氣更新，UI不需要顯示
    /// </summary>
    Default = 0,

    /// <summary>
    /// 增加怒氣，UI需要顯示
    /// </summary>
    Add = 1,

    /// <summary>
    /// 減少怒氣，UI需要顯示
    /// </summary>
    Lost = 2,

}

#endregion

public enum EM_MagicType
{
    Attack = 1,     //普攻
    anger,        //怒气
    fit,          //合击
    superFit,     //超合计
}
/// <summary>
/// 游戏玩法
/// </summary>
public enum EM_PropertyControl
{
    isTalent,    //天赋 
    isFate,      //缘分
    isAwaken,    //领悟
    isSkyDestiny,  //天命
    isLegionSkill,  //军团技能
    isArtifact, //神器
    isEquipFinsh,  //时装
    isTactical,  //阵法
    isMonthCard, //月卡
    isAuraCamp,// vòng sáng phe
    isElementalSeason,
    End = 11,
}
/// <summary>
/// 获取途径转至战斗回主界面类型
/// </summary>
public enum EM_GetWayToBattle
{
    None = 1,
    CardBag,//由卡牌背包转入
    EquipBag,//装备背包
    LineUpPage,//阵容界面
}

public enum EM_GameNameParamType
{
    /// <summary>
    /// 25元月卡奖励
    /// </summary>
    MothCard25 = 1,
    /// <summary>
    /// 50元月卡奖励
    /// </summary>
    MothCard50 = 2,
    /// <summary>
    /// 红包兑换活动名称
    /// </summary>
    RedPacketExchangeName = 3,
    /// <summary>
    /// 红包任务活动名称
    /// </summary>
    RedPacketTaskName = 4,
    /// <summary>
    /// 新年贩售活动名称
    /// </summary>
    NewYearSellingName = 5,
    /// <summary>
    /// 情人节选美大赛活动名称
    /// </summary>
    ValentinesDayName = 6,
    /// <summary>
    /// 萌犬闹新春活动名称
    /// </summary>
    DogStepName = 7,
    /// <summary>
    /// 招募有礼活动名称
    /// </summary>
    RecruitGiftName = 8,
    /// <summary>
    /// 单笔充值福利活动名称
    /// </summary>
    SingleRechargeName = 9,
    /// <summary>
    /// 累积充值福利活动名称
    /// </summary>
    MutiRechargeName = 10,
    /// <summary>
    /// 累积消费福利活动名称
    /// </summary>
    TotalConstName = 11,
    /// <summary>
    /// 财神送礼活动名称
    /// </summary>
    MammonSendGiftsNewName = 12,
    /// <summary>
    /// 新春签到活动名称
    /// </summary>
    NewYearSignName = 13,
    /// <summary>
    /// 限时兑换
    /// </summary>
    FestivalExchange = 14,
    /// <summary>
    /// 限时兑换
    /// </summary>
    DealsEveryDay = 15,
    ExclusionSpin = 16,
    //
    AwardTimeLimit = 17,
    AwardEndow = 18,
    /// <summary>
    /// 月卡
    /// </summary>
    MonthCard = 87,
    /// <summary>
    /// 永久卡
    /// </summary>
    PerpetualCard = 88
}
public enum EM_GameParamType
{
    /// <summary>
    /// 体力恢复周期
    /// </summary>
    EnergyRecover = 1,
    /// <summary>
    /// 精力恢复周期
    /// </summary>
    VigorRecover = 2,
    /// <summary>
    /// 体力消耗获取经验
    /// </summary>
    Energy2Exp = 3,
    /// <summary>
    /// 精力消耗获取经验
    /// </summary>
    Vigor2Exp = 4,
    /// <summary>
    /// 体力消耗获取银币
    /// </summary>
    Energy2Money = 5,
    /// <summary>
    /// 精力消耗获取银币
    /// </summary>
    Vigor2Money = 6,
    /// <summary>
    /// 首充礼包
    /// </summary>
    FirstRecharge = 7,
    /// <summary>
    /// 征讨令恢复周期
    /// </summary>
    CrusadeTokenRecover = 8,

    /// <summary>
    /// 财神送礼开启时间
    /// </summary>
    MammonSendGiftsOpenTime = 40,
    /// <summary>
    /// 财神送礼配置ID
    /// </summary>
    MammonSendGiftAwardId = 42,
    /// <summary>
    /// 财神送礼图标显示
    /// </summary>
    MammonSendGiftIcon = 43,
    /// <summary>
    /// 情人节抽奖池ID
    /// </summary>
    ValentinesDayAwardId = 45,
    /// <summary>
    /// 情人节宝箱需要积分
    /// </summary>
    ValentinesDayBoxNeedScore = 46,
    /// <summary>
    /// 情人节宝箱奖励ID
    /// </summary>
    ValentinesDayBoxAwardId = 47,
    /// <summary>
    ///情人节开放时间
    /// </summary>
    ValentinesDayOpenTime = 49,
    /// <summary>
    /// 新春签到开放时间
    /// </summary>
    NewYearSign = 50,
    /// <summary>
    /// 所有春节活动的开启等级
    /// </summary>
    NewYearOpenLevel = 52,
    /// <summary>
    /// 红包任务开启时间（已失效，读表）
    /// </summary>
    RedPacketTaskOpenTime = 54,
    /// <summary>
    /// 招募有礼开启时间（已失效，读表）
    /// </summary>
    RecruitGiftOpenTime = 55,
    /// <summary>
    /// 阵法开启等级
    /// </summary>
    TariningOpenSv = 70,
    /// <summary>
    /// 单场单次练兵价格
    /// </summary>
    OneTariningNeedMoneny = 71,
    /// <summary>
    /// 元宝练兵价格
    /// </summary>
    SyceeTarining = 72,
    /// <summary>
    /// 每周几开启 对应DayOfWeek   -1:代表无效
    /// </summary>
    CardBattleOpenDayOfWeek = 76,
    /// <summary>
    /// 斗将报名时间
    /// </summary>
    CardBattleApplyTime = 77,
    /// <summary>
    /// 斗将战斗时间
    /// </summary>
    CardBattleBattleTime = 78,
    /// <summary>
    /// 通天转盘抽奖价格
    /// </summary>
    TurntableLottery = 79,
    /// <summary>
    /// 新手引导全部跳过
    /// </summary>
    AllGuidanceEsc = 80,
    /// <summary>
    /// 幸运转盘限制等级
    /// </summary>
    TurntableLimitLv = 81,
    /// <summary>
    /// 试炼之塔
    /// </summary>
    TrialTower = 84,
    /// <summary>
    /// 七星命灯
    /// </summary>
    SevenStar = 85,
    AfkInfo = 107,
    DealsEveryDay = 119,
    VipX2 = 121,
    NotShow = 131,

}

public enum EM_AwardCenter
{
    ArenaAward = 1,   //竞技场奖励
    KillRebelArmyAward = 2, //叛军击杀奖励
    FindRebelArmyAward = 3,  //叛军发现奖励
    RebelArmyRankExploit = 4,   //叛军功勋排行
    RebelarmyRankDPS = 5,   //叛军伤害排行
    eAward_FirstRechargePresent = 65,  // 开服首充奖励
    eAward_ValentineRose = 32,			// 情人节玫瑰奖励 uParam0:获胜美人[0, 4) uParam1:赠送玫瑰数
    PayBeta = 20, //删档测试服充值返利uParam0: 充值金额

    CrossTournament = 91,
    CrossTournamentBonusTop1Point = 92,
    CrossTournamentBonusTop1 = 93,
    CrossTournamentFightMail = 94,
}

#region RunningMan (过关斩将)

/// <summary>
/// 通关条件类型
/// </summary>
public enum EM_RM_PassType
{
    /// <summary>
    /// 未定义
    /// </summary>
    Undefined = 1,
    /// <summary>
    /// 在{0}回合内获胜
    /// </summary>
    BoutLimit = 2,
    /// <summary>
    /// 我方血量高于{0}%
    /// </summary>
    HpLimit = 3,
    /// <summary>
    /// 我方死亡人数不超过{0}人
    /// </summary>
    DieNumLimit = 4,
}

#endregion


#region Patrol 巡逻(古都巡礼)

public enum EM_PatrolState
{
    Lock = 0,       //锁定
    CanAttack = 1,  //可以攻打 
    CanPatrol = 2,  //可以巡逻
    Patroling = 3,  //巡逻中
    GetAward = 4,   //巡逻完成可以领取奖励
}

public enum EM_PatrolType
{
    Low = 1, //低级巡逻
    Mid = 2, //中级巡逻
    Hight = 3, //高级巡逻
}

#endregion

#region 商店相关
/// <summary>
/// 底部分页菜单按钮
/// </summary>
public enum EM_ShopMutiPageTap
{
    BtnShop,
    BtnAward,
    BtnLimit,
    BtnPurpleEquip,
    BtnRedEquip,
}
#endregion


#region 红点提示类型

public enum EM_ReddotMsgType
{
    /// <summary>
    /// 日常任务
    /// </summary>
    TaskPage_Daily = 0,
    /// <summary>
    /// 成就任务
    /// </summary>
    TaskPage_Achievement = 1,
    /// <summary>
    /// 装备碎片可合成
    /// </summary>
    EquipBag_Fragment,
    /// <summary>
    /// 卡牌碎片可以合成
    /// </summary>
    CardBag_Fragment,
    /// <summary>
    /// 阵图碎片可激活
    /// </summary>
    BattleForm_CanAct,
    /// <summary>
    /// 战将抽牌可免费购买
    /// </summary>
    NorAdmiralFree,
    /// <summary>
    /// 神将抽牌可免费购买
    /// </summary>
    GenAdmiralFree,
    /// <summary>
    /// 商城礼包可购买
    /// </summary>
    ShopGiftBuy,
    /// <summary>
    /// 首充，可购买或者可领取奖励
    /// </summary>
    FirstRecharge,
    /// <summary>
    /// 世界、军团、队伍、私聊聊天新消息
    /// </summary>
    ChatWordNewMsg,
    ChatLegionNewMsg,
    ChatTeamNewMsg,
    ChatPrivateNewMsg,
    /// <summary>
    /// 可领取好友精力
    /// </summary>
    FriendVigor,
    /// <summary>
    /// 有新好友申请
    /// </summary>
    FriendApply,
    /// <summary>
    /// 过关斩将有攻打次数
    /// </summary>
    RunningManEliteLeftTimes,
    /// <summary>
    /// 有可合成的法宝
    /// </summary>
    TreasureCanFix,
    /// <summary>
    /// 每日副本可攻打
    /// </summary>
    DailyPve,
    /// <summary>
    /// 名将副本有挑战次数
    /// </summary>
    LegendHavaTimes,
    /// <summary>
    /// 每日签到
    /// </summary>
    UserSign,
    /// <summary>
    /// 豪华签到
    /// </summary>
    GrandSign,
    /// <summary>
    /// 宫宴
    /// </summary>
    BanQuet,
    /// <summary>
    /// 活动招财符免费
    /// </summary>
    LuckySymbolFree,
    /// <summary>
    /// 迎财神可迎财
    /// </summary>
    WealthMan,
    /// <summary>
    /// 登录送礼可领取
    /// </summary>
    LoginGift,
    /// <summary>
    /// 登录送礼可领取(开服豪礼)
    /// </summary>
    LoginGiftNewServ,
    /// <summary>
    /// 月卡可领取奖励
    /// </summary>
    MonthCardGet,
    /// <summary>
    /// 等级礼包可购买
    /// </summary>
    RankGiftBuy,
    ///// <summary>
    ///// 有紫色以下的装备可回收
    ///// </summary>
    //EquipRecycle,
    ///// <summary>
    ///// 有紫色以下的卡牌可回收
    ///// </summary>
    //CardRecycle,
    /// <summary>
    /// 活动开服基金，已购买且可领取
    /// </summary>
    ActOpenServFund,
    /// <summary>
    /// 活动全民福利，可领取
    /// </summary>
    ActOpenWelfare,
    /// <summary>
    /// 在线豪礼可领取
    /// </summary>
    OnlineAward,
    /// <summary>
    /// 天降横财有次数可抽奖
    /// </summary>
    SkyFortune,
    /// <summary>
    /// 背包有新的道具
    /// </summary>
    CardBagNewGoods,
    EquipBagNewGoods,
    GoodsBagNewGoods,
    AwakenBagNewGoods,
    TreasureBagNewGoods,
    GodEquipBagNewGoods,
    CardFragmentBagNewGoods,
    EquipFragmentBagNewGoods,
    TreasureFragmentBagNewGoods,
    GodEquipFragmentBagNewGoods,
    //军团相关红点
    /// <summary>
    /// 军团申请列表（成员管理）
    /// </summary>
    LegionApplicantList,

    /// <summary>
    /// 军团章节奖励
    /// </summary>
    LegionChapetAward,

    /// <summary>
    /// 军团红包奖励
    /// </summary>
    LegionRedPacket,

    /// <summary>
    /// 巡逻可领取奖励 红点
    /// </summary>
    PatrolGetAward,


    /// <summary>
    /// 叛军入侵
    /// </summary>
    RebelArmy,
    /// <summary>
    /// 七日活动
    /// </summary>
    SevenDayTask,
    SevenDayNumPage1,
    SevenDayNumPage2,
    SevenDayNumPage3,
    SevenDayNumPage4,
    SevenDayNumPage5,
    SevenDayNumPage6,
    SevenDayNumPage7,

    SevenTabPage1,
    SevenTabPage2,
    SevenTabPage3,
    SevenTabPage4,
    /// <summary>
    /// 单笔充值可领取红点
    /// </summary>
    SingleRecharge,
    /// <summary>
    /// 单笔充值可领取红点
    /// </summary>
    MutiRecharge,
    /// <summary>
    /// 红包任务可领取
    /// </summary>
    RedPacketTask,
    /// <summary>
    /// 招募有礼可领取
    /// </summary>
    RecruitGift,
    /// <summary>
    /// 红包兑换可兑换
    /// </summary>
    RedPacketExchange,
    /// <summary>
    /// 累计消费
    /// </summary>
    TotalConst,
    /// <summary>
    /// 新年贩售
    /// </summary>
    NewYearSelling,
    /// <summary>
    /// 财神送礼
    /// </summary>
    MammonSendGifts,
    /// <summary>
    /// 新年签到（可领取，看一眼界面）
    /// </summary>
    NewYearSign,
    /// <summary>
    /// 十万元宝
    /// </summary>
    TenSycee,
    /// <summary>
    /// vip礼包
    /// </summary>
    VipGift,
    /// <summary>
    /// 功勋奖励红点
    /// </summary>
    RebelArmyAward,
    /// <summary>
    /// 周基金
    /// </summary>
    WeekFund,

    /// <summary>
    /// 练兵可以领取的红点
    /// </summary>
    TariningGetAward,
    /// <summary>
    /// 熔炼红点
    /// </summary>
    TransmigrationTreasure,
    /// <summary>
    /// 阵法可以学习技能
    /// </summary>
    TacticalStdySkill,
    DealsEveryDay,
    ExclusionSpin,
    GodEquipBag_Fragment,
}

#endregion


#region 等级限制
public enum EM_NeedLevel
{
    CardCultivate = 9,  //（卡牌培养）
    GrabTreasureLevel = 10,   //夺宝开放等级
    RunningManLvel = 11,   //过关斩将开放等级
    PatrolLevel = 12,//古都巡礼
    LegionLevel = 13,//军团系统
    GrabTreasureFiveLevel = 14,//夺宝夺5次
    DungeonEliteLevel = 15,//精英副本
    LegendLevel = 16,//名将副本 
    BattleNumLevel = 17, //战斗倍数切换
    SweepLevel = 18,//扫荡
    OpenTwoCardLevel = 19,//武将开放等级
    OpenThereCardLevel = 20,
    OpenFourCardLevel = 21,
    OpenFiveCardLevel = 22,
    OpenSixCardLevel = 23,
    EquipRefine = 24,  //装备精炼
    OpenReinforceBuffLevel = 25,//援军助威
    EquipIntenFiveLevel = 26,   //强化5次
    OpenReinforceLevel = 27,//援军
    EquipUpStar = 28,//装备升星
    ArenaLevel = 29,    //竞技场开放等级
    CardSky = 30,   //卡牌天命
    CardAwaken = 31,  //卡牌领悟
    OpenTreasureLevel = 32,  //法宝开放等级
    DailyPveLevel = 33, //日常副本
    DungeonJumpLevel = 34,   //副本跳过等级限制
    BattleFormLevel = 35,//阵图开放等级
    GuidanceEndLevel = 36,//新手引导结束等级
    RebelArmyLevel = 37,//叛军入侵
    AwakeShopLevel = 38,//领商店开启等级
    BattleNum3Level = 44, //战斗3倍数切换
    /// <summary>
    /// 跨服战相关配置参数（1:开放等级，2：默认挑战次数，3：购买一次次数所需元宝（buyTimes*X））
    /// </summary>
    CrossServerBattle = 60, //跨服战 

    TariningLevel = 70,  //阵法开启等级

    TransmigrationCard = 74,//卡牌转换
    TransmigrationTreasure = 75,//法宝转换
	CardBattle = 96,

    /// <summary>
    /// 主线宝鉴限制等级
    /// </summary>
    MainLineTreasureBookLimitLv = 82,
    /// <summary>
    /// 一键领奖限制等级
    /// </summary>
    MainLineOneKeyGetRewardLimitLv = 83,
    /// <summary>
    /// 试炼之塔
    /// </summary>
    TritalTowerOpenLV = 84,
    /// <summary>
    /// 七星命灯
    /// </summary>
    SevenStarOpenLv = 85,
    /// <summary>
    /// 一键装备
    /// </summary>
    EquipQuick = 86,
    ChaosBattle = 99, //TsuCode - ChaosBattle
    GodEquip = 103,
    GodEquipRefine = 104,
    GodEquipUpStar = 105,
    CrossArena = 118,
    CrossTournament = 120,

    Open7CardLevel = 130,
    End,//结束
}
#endregion

public enum EM_eSevenDay
{
    eSevenDay_Login = 1,                            //每日登录游戏
    eSevenDay_Days_Recharge_X = 2,                  //多日累计充值
    eSevenDay_Lv_X = 3,                         //玩家等级达到X级（唯一）
    eSevenDay_FightPower_X = 4,                 //玩家战斗力达到X（唯一）
    eSevenDay_MainPve_X = 5,                        //主线副本通关第X章（唯一）
    eSevenDay_Battle_X_Card = 6,                    //上阵X名卡牌（唯一）
    eSevenDay_Battle_X_Card_Q_X = 7,                //上阵X名X品质卡牌（唯一）
    eSevenDay_Battle_X_CardWear_Q_X = 8,            //上阵X名卡牌  全部穿戴X品质以上装备（包括当前品质）（唯一）
    eSevenDay_Battle_X_WearEquip_X = 9,         //上阵X名卡牌 ，所有穿戴装备强化等级达到X级（唯一）
    eSevenDay_Battle_X_WearRefine_X = 10,       //上阵X名卡牌 ，所有穿戴装备精炼等级达到X级（唯一）
    eSevenDay_Battle_X_MagicStren_X = 11,       //上阵X名卡牌 ，所有穿戴法宝强化等级达到X级（唯一）
    eSevenDay_Battle_X_MagicRefine_X = 12,      //上阵X名卡牌 ，所有穿戴法宝精炼等级达到X级（唯一）
    eSevenDay_WearEquip_Refine_X = 13,          //穿戴装备最高精炼等级达到X级	
    eSevenDay_FightGeneral_Draw_X = 14,         //战将抽卡X次
    eSevenDay_GodGeneral_Draw_X = 15,               //神将抽卡X次	
    eSevenDay_Arena_Rank_X = 16,                    //竞技场排名达到X名

    eSevenDay_Card_Fit_X = 17,                  //卡牌健身提升x次
    eSevenDay_Card_DiamondFit_X = 18,               //卡牌健身钻石提升x次

    eSevenDay_Magic_Synthesis_X = 19,               //合成X个法宝   .
    eSevenDay_Magic_Synthesis_Q_X = 20,         //合成X个X品质法宝   .
    eSevenDay_Through_Rank_X = 21,              //过关斩将排名达到X名   .
    eSevenDay_Through_Reset_X = 22,             //过关斩将重置X次  .
    eSevenDay_Through_Star_X = 23,              //过关斩将达到X星

    eSevenDay_Retain24 = 24,                        //通关名将副本BOSS关卡 

    eSevenDay_Legion_Make = 25,                 //创建或者加入一个军团
    eSevenDay_Legion_Publicity_H = 26,          //军团高级宣传一次
    eSevenDay_Legion_Publicity_M = 27,          //军团中级宣传一次
    eSevenDay_Legion_Publicity_L = 28,          //军团初级宣传一次
    eSevenDay_FightGeneral_Shop_Refresh_X = 29, //神将商店刷新X次    .
    eSevenDay_FightGeneral_Shop_Buy_X = 30,     //神将商店购买X次      .
    eSevenDay_Arena_Shop_Consume_X = 31,            //竞技场声望商店消耗X声望     .
    eSevenDay_Rebels_Shop_Consume_X = 32,       //叛军入侵商店消耗X点战绩        .
    eSevenDay_God_Shop_Consume_X = 33,          //神将商店消耗X点将魂       .
    eSevenDay_Shop_Sycee_Consume_X = 34,            //商城消耗X元宝        .
    eSevenDay_MainCard_Advanced_X = 35,         //主卡牌进阶等级达到X级
    eSevenDay_LifStar_X = 36,                       //上阵6名，命星等级达到X级
    eSevenDay_LifStar_Max = 37,                 //上阵卡牌中命星最高等级达到x级
    eSevenDay_Crusade_Buy = 38,                 //在商城中购买指定道具(购买征讨令，写死)        .
    eSevenDay_Rebels_Dam_Max = 39,              //（每日）叛军最高伤害达到X         
    eSevenDay_Rebels_Exploits_X = 40,               //（每日）叛军每日累计战功到达X   .
    eSevenDay_Patrol_H_X = 41,                  //（累计）高级巡逻X小时  .
    eSevenDay_Patrol_X = 42,                        //（累计）普通巡逻X小时 .
    eSevenDay_Magic_Refine_Max = 43,                //穿戴法宝中精炼最高等级达到X级
    eSevenDay_ThroughShop_Consume_X = 44,       //过关斩将商店消耗X威名  .
    eSevenDay_Recharge_X = 45,                  //今日单笔充值
    eSevenDay_Treasure_Grab_X = 46,             //今日参与夺宝X次      .
    eSevenDay_MainPve_Win_X = 47,                   //今日主线副本胜利X次       .
    eSevenDay_Arena_Win_X = 48,                 //今日竞技场胜利X次         .
    eSevenDay_ElitePve_Fight_X = 49,                //攻略精英副本X次         .

    eSevenDay_Retain50 = 50,                        //(累计)数码考古X次
    eSevenDay_Retain51 = 51,                        //(累计)加装X次

    eSevenDay_High_CardDraw10_X = 52,           //高级抽卡10连X次          .
    eSevenDay_PluginLv_X = 53,              //插件达到X级
    eSevenDay_BattleCard_Advance_X = 54,        //上阵X名卡牌，进阶等级达到X级
    eSevenDay_SendEngery_X = 55,                //(每日)为X名好友赠送好友精力      .
    eSevenDay_Rebels_Attack_X = 56,         //攻打叛军X次    .
    eSevenDay_MainPve_Star_X = 57,          //主线副本达到X星    
    eSevenDay_ElitePve_Clearance_X = 58,        //精英副本通关X章   
    eSevenDay_ElitePve_Star_X = 59,         //精英副本达到X星   
    eSevenDay_Rebels_Beat_X = 60,               //击退叛军入侵X次     .
    eSevenDay_AllServ_Welfare_X = 61,           //本服购买全民福利人数达到X人（开服即开始累积计算）    
    eSevenDay_Login_Count = 62,             //累积登陆

    eSevenDay_Retain63 = 63,					//拥有X辆座驾

    eSevenDay_MonthCard = 64,                   //月卡

    eSevenDay_Treasure_Syn_Purple = 65,                 //合成紫色法宝
    eSevenDay_Treasure_Syn_Orange = 66,					//合成橙色法宝
}
/// <summary>
/// 军团技能类型
/// </summary>
public enum EM_LegionSkillType
{
    SkillOne = 1,
    SkillTwo = 2,
    SkillThree = 3,
    SkillFour = 4,
    SkillFive = 5,
    SkillSix = 6,
    SkillSeven = 7,
    SkillEight = 8,
    SkillNine = 9,
}

/// <summary>
/// 特效资源id
/// </summary>
public enum EM_MagicId
{
    /// <summary>
    /// 主界面模型脚下发光特效
    /// </summary>
    eRoleBottomLine = 20012,
    /// <summary>
    /// 结算星星特效
    /// </summary>
    eFightFinishStar = 20013,
    /// <summary>
    /// 结算失败特效
    /// </summary>
    eFightFinishFail = 20014,
    /// <summary>
    /// 结算成功特效
    /// </summary>
    eFightFinishSuc = 20015,
    /// <summary>
    /// 回收特效
    /// </summary>
    eRecycle = 20016,
    /// <summary>
    /// 首冲特效
    /// </summary>
    eFristRechargePage = 20017,
    /// <summary>
    /// 招财符背景特效
    /// </summary>
    eLuckySymbolBg = 20018,
    /// <summary>
    /// 在线豪礼背景特效
    /// </summary>
    eOnlineAwardBg = 20019,
    /// <summary>
    /// 在线豪礼按钮特效
    /// </summary>
    eOnlineAwardGet = 20020,
    /// <summary>
    /// 将星特效
    /// </summary>
    eBattleForm = 20021,
    /// <summary>
    /// 名将boss
    /// </summary>
    eLegeongBoss = 20022,
    /// <summary>
    /// 合体技过场动画
    /// </summary>
    eFitSkill = 20023,
    /// <summary>
    /// 影子
    /// </summary>
    eShadow = 20024,
    /// <summary>
    /// 神装背景
    /// </summary>
    eLimiGodEquip=20025,
    /// <summary>
    /// 怒技过场
    /// </summary>
    eSkill2Mv = 20027,
    /// <summary>
    /// 进阶特效
    /// </summary>
    eAdvanceEffect = 20028,
    /// <summary>
    /// 进阶特效
    /// </summary>
    eAdvanceDoneEffect = 20029,
    /// <summary>
    /// 登录花瓣
    /// </summary>
    eLoginEffect = 20030,
    /// <summary>
    /// 创角背景特效
    /// </summary>
    eCreateRoleBg = 200015,
}

public enum eTacticalFormat
{
    eTactical_LifeAdd = 1,          //生命加成
    eTactical_AttackAdd = 2,        //攻击加成
    eTactical_Pdef = 3,         //物理防御加成
    eTactical_ManaDef = 4,          //法力防御加成
    eTactical_DamAdd = 5,           //伤害加成
    eTactical_DamReduction = 6, //伤害减免
    eTactical_CritRate = 7,         //暴击率
    eTactical_UprisingHitRate = 8,//抗暴击率
    eTactical_HitRate = 9,          //命中率
    eTactical_EvadeRate = 10,		//闪避率
};

public enum eTrialTowerState
{

    Suc,   //成功
    Fail,  //失败
    NoEnter,//未挑战
}
public enum EM_SupermarketType
{
    AwardTimeLimit,//quà hạn giờ 
    AwardEndow,//quà ưu đãi
    MonthCard,// thẻ tháng
    OpenServFund,//quỹ trưởng thành
    DealsEveryDay,// ưu đãi mỗi ngày
    AwardSeason,// "Gói Ưu Đãi Mùa"
	WeekFund,
}

public enum EM_AwardEndowType
{
    Nor, // k reset
    Daily,//reset hằng ngày
    Weekly,//reset hằng tuần
    Monthly,//reset hằng tháng
    END,
}