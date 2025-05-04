/// <summary>
/// 军团相关常量
/// </summary>
public class LegionConst
{
    #region 要移到功能配置表的

    /// <summary>
    /// 军团副本次数恢复时间
    /// </summary>
    public const int LEGION_DUNGEON_TIMES_RECOVER_TIME = 2;

    /// <summary>
    /// 军团副本挑战开始时间
    /// </summary>
    public const int LEGION_DUNGEON_BEGIN_TIME = 10;

    /// <summary>
    /// 军团副本挑战结束时间
    /// </summary>
    public const int LEGION_DUNGEON_END_TIME = 22;

    /// <summary>
    /// 军团副本挑战初始次数限制
    /// </summary>
    public const int LEGION_DUNGEON_INIT_TIMES_LIMIT = 5;

    /// <summary>
    /// 购买军团副本次数元宝消耗参数 公式：第N次数 = N*10
    /// </summary>
    public const int LEGION_DUNGEON_SYCEE_PRE_TIME = 10;

    #endregion

    /// <summary>
    /// 军团创建花费
    /// </summary>
    public const int LEGION_CREATE_COST = 500;

    /// <summary>
    /// 军团创建VIP等级
    /// </summary>
    public const int LEGION_CREATE_VIPLV = 4;

    /// <summary>
    /// 弹劾军团长花费
    /// </summary>
    public const int LEGION_IMPEACH_COST = 500;

    /// <summary>
    /// 弹劾军团长时间间隔
    /// </summary>
    public const int LEGION_IMPEACH_TIME_DIS = 86400 * 5;

    /// <summary>
    /// 军团解散人数限制  大于此值不能解散
    /// </summary>
    public const int LEGION_DISSOLVE_MEMBER_LIMIT = 10;

    /// <summary>
    /// 军团图标
    /// </summary>
    public readonly static byte[] LEGION_ICON_LIST = new byte[] { 201, 202, 203, 204, 205, 206, 207, 208 };

    public readonly static string[] LEGION_POSTION_FLAG_NAME = new string[4] {"","Icon_ChiefFlag","Icon_DeputyFlag","Icon_NormalFlag" };
    
    /// <summary>
    /// 请求军团列表时间间隔  单位为秒
    /// </summary>
    public const int LEGION_LIST_TIME_DIS = 30;

    /// <summary>
    /// 军团列表每页的个数
    /// </summary>
    public const int LEGION_LIST_NUM_PRE_PAGE = 10;

    /// <summary>
    /// 自己申请军团的最大数量
    /// </summary>
    public const int LEGION_SELF_APPLY_MAX_NUM = 5;

    /// <summary>
    /// 军团的副团长最大数量
    /// </summary>
    public const int LEGION_DEPUTY_MAX_NUM = 2;

    /// <summary>
    /// 再次加入军团的时间间隔 （24小时） 单位：秒
    /// </summary>
    public const int LEGION_JOIN_AGAIN_TIME_DIS = 86400;

    /// <summary>
    /// 军团名字最小字节数
    /// </summary>
    public const int LEGION_NAME_BYTE_MIN_NUM = 4;

    /// <summary>
    /// 军团名字最大字节数
    /// </summary>
    public const int LEGION_NAME_BYTE_MAX_NUM = 18;

    /// <summary>
    /// 公告字节限制 发送加多2字节
    /// </summary>
    public const int LEGION_NOTICE_BYTE_LIMIT = 78;

    /// <summary>
    /// 宣言字节限制  发送加多2字节
    /// </summary>
    public const int LEGION_MANIFESTO_BYTE_LIMIT = 38;

public const string LEGION_NOTICE_DEFAULT_VALUE = "Đoàn trưởng lười biếng, không viết gì cả";

public const string LEGION_MANIFESTO_DEFAULT_VALUE = "Chào mừng bạn tham gia";

    /// <summary>
    /// 军团副本额外奖励军团贡献
    /// </summary>
    public const int LEGION_DUNGEON_EXTRA_CONTRI = 20;


    #region 军团战常量 可能部分移到参数表

    //军团战报名成员数限制
    public const int LEGION_BATTLE_SIGNUP_MEMBERNUM_LIMIT = 7+7+7+1;
    //TsuCode
    //public const int LEGION_BATTLE_SIGNUP_MEMBERNUM_LIMIT = 7;
    //

    //军团战报名等级限制
    public const int LEGION_BATTLE_SIGNUP_LV_LIMIT = 2;

    //当天军团战开始的小时
    public const int LEGION_BATTLE_BEGIN_HOUR = 12;

    //TsuCode
    public const int LEGION_BATTLE_BEGIN_Minute =0;
    //

    //当天军团战结束的小时
    public const int LEGION_BATTLE_END_HOUR = 23;

    //当天军团战结束的分钟
    public const int LEGION_BATTLE_END_Minute = 55;

    //public readonly static int[] LEGION_BATTLE_BEGIN_DAYOFWEEKS = new int[5] {2,3,4,5,6}; //TsuComment
    public readonly static int[] LEGION_BATTLE_BEGIN_DAYOFWEEKS = new int[4] { 0,1,3,5};

    /// <summary>
    /// idx: 0：失败奖励 1-3： 1-3星奖励 4:胜利奖励 5：失败奖励
    /// </summary>
    public readonly static int[] LEGION_BATTLE_AWARD_STAR = new int[6] { 14, 11, 12, 13, 15, 16 };

    /// <summary>
    /// 军团战挑战时间限制 加入时间必须比军团战开始时间多24小时
    /// </summary>
    public const int LEGION_BATTLE_CHALLENGE_TIME_LIMIT = 86400;

    #endregion

    /// <summary>
    /// 军团名字字节数（跟服务器）
    /// </summary>
    public const int LEGION_NAME_BYTE_NUM = 20;

}
