public class GameParamConst
{
    /// <summary>
    /// 精力自然恢复最大值
    /// </summary>
    public const int VigorEcoverLimit = 30;

    /// <summary>
    /// 角色名字最小字节长度
    /// </summary>
    public const int RoleNameByteMinNum = 4;

    /// <summary>
    /// 角色名字最大字节长度
    /// </summary>
    public const int RoleNameByteMaxNum = 20;

    /// <summary>
    /// 每关的星星数
    /// </summary>
    public const int StarNumPreTollgate = 3;

    /// <summary>
    /// 关卡能扫荡的最大次数
    /// </summary>
    public const int TollgateSweepMaxNum = 5;

    /// <summary>
    /// UI layer名字
    /// </summary>
    public const string UILayerName = "UI";

    /// <summary>
    /// 玩家角色卡牌Id   跟策划约定好的
    /// </summary>
    public const int PlayerRoleId = 99999;

    /// <summary>
    /// 描述中替换值标记
    /// </summary>
    public const string ReplaceFlag = "【x】";
    public const string ReplaceFlag2 = "【x1】";

    public const string StringToIntSign = ";";

    /// <summary>
    /// 任务宝箱数目最大值
    /// </summary>
    public const int TaskBoxMaxNum = 5;

    /// <summary>
    /// 成就任务 需要检测的上阵英雄数目
    /// </summary>
    public const int TaskTeamCheckNum = 6;

    /// <summary>
    /// 成就任务 需要检测的装备数目
    /// </summary>
    public const int TaskEquipCheckNum = 4;

    /// <summary>
    /// 成就任务 需要检测的上阵法宝数目
    /// </summary>
    public const int TaskTreasureCheckNum = 2;

    /// <summary>
    /// vip等级最大值
    /// </summary>
    public const int VipLvMaxNuM = 15;

    /// <summary>
    /// 推荐列表刷新时间间隔
    /// </summary>
    public const int RecommendRefreshInterval = 10;

    /// <summary>
    /// 男性角色卡牌Id
    /// </summary>
    public const int ManCardId = 1000;
    /// <summary>
    /// 女性角色卡牌Id
    /// </summary>
    public const int WomanCardId = 1001;


    #region 很大可能以后移到游戏参数表的

    /// <summary>
    /// 好友最大数目
    /// </summary>
    public const int FriendMaxNum = 100;

    /// <summary>
    /// 领取精力次数最大数目
    /// </summary>
    public const int RecvVigorMaxNum = 100;

    /// <summary>
    /// 竞技场每次消耗的精力
    /// </summary>
    public const int ArenaVigorCost = 2;

    /// <summary>
    /// 竞技场胜利声望奖励
    /// </summary>
    public const int ArenaWinFame = 100;

    /// <summary>
    /// 竞技场失败声望奖励
    /// </summary>
    public const int ArenaLoseFame = 50;

    /// <summary>
    /// 任务等级限制
    /// </summary>
    public const int TaskLvLimit = 10;

    #endregion


    #region PlayerPrefs 相关Key

    /// <summary>
    /// 剧情通过{0} 玩家Id {1}:副本类型EM_DungeonType
    /// </summary>
    public const string DialogTollgateIdx = "Dialog{0}{1}";


    #endregion

    /// <summary>
    /// 剧情关卡id
    /// </summary>
    public const int PLOT_TOLLGATEID = 0xffff;

    /// <summary>
    /// 过关斩将每章节关卡数量
    /// </summary>
    public const int RMTollgateNumPreChap = 3;

    /// <summary>
    /// 过关斩将每关卡的难度数量
    /// </summary>
    public const int RMModeNumPreTollgate = 3;

    /// <summary>
    /// 过关斩将精英次数限制
    /// </summary>
    public const int RMEliteTimesLimit = 3;


    /// <summary>
    /// 只有前面4关能跨天重置
    /// </summary>
    public const int LegendDungeonRestIdx = 4;

    /// <summary>
    /// 体力最大值
    /// </summary>
    public const int EnergyMax = 500;

    /// <summary>
    /// 精力最大值
    /// </summary>
    public const int VigorMax = 200;

    /// <summary>
    /// 体力丹Id
    /// </summary>
    public const int EnergyGoodId = 200;

    /// <summary>
    /// 精力丹Id
    /// </summary>
    public const int VigorGoodId = 201;

    /// <summary>
    /// 征讨令id
    /// </summary>
    public const int CrusadeToken = 202;

    #region 领地巡逻相关

    /// <summary>
    /// 替换成卡牌名字
    /// </summary>
    public const string PatrolReplaceCardName = "【x】";

    public const string PatrolReplaceAward = "【x1】";

    public const string PatrolReplaceFriendName = "【x2】";

    public const string PatrolReplacePatrolTime = "【x3】";

    public const int PatrolEventPacify = 101;

    #endregion

    /// <summary>
    /// 神器X级一阶
    /// </summary>
    public const int ArtifactLvPreStep = 10;

    /// <summary>
    /// 影子名称
    /// </summary>
    public const string createShadowBoneName = "yingzi";
    public const string prefabShadowName = "Shadow";
    public const float shadowScalePerc = 0.3f;

    public const int MAX_FIGHT_POS = 7;
}
