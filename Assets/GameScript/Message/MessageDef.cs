using UnityEngine;
using System.Collections;
using Beebyte.Obfuscator;

[Skip]
public class MessageDef
{
    public static string LOADSCSUC = "LOADSCSUC";
    
    public static string PAUSEGAME = "PAUSEGAME";
    public static string RESUMEGAME = "RESUMEGAME";

    public static string LOGINEROINFOR = "LOGINEROINFOR";

    /// <summary>
    /// 全局错误提示
    /// </summary>
    public static string GAMEMESSAGEBOX = "GAMEMESSAGEBOX";

    /// <summary>
    /// 重试连接网络
    /// </summary>
    public static string RETRYCONNECT = "RETRYCONNECT";

    /// <summary>
    /// 重试连接成功
    /// </summary>
    public static string RETRYCONNECTSUC = "RETRYCONNECTSUC";

    /// <summary>
    /// 玩家等级更新事件
    /// </summary>
    public static string PlayerLvUpdate = "PlayerLvUpdate";

    /// <summary>
    /// 玩家战斗力更新事件
    /// </summary>
    public static string PlayerFightPowerChange = "PlayerFightPowerChange";

    /// <summary>
    /// 玩家VIP数据更新事件
    /// </summary>
    public static string PlayerVipUpdate = "PlayerVipUpdate";

    /// <summary>
    /// 跨天事件
    /// </summary>
    public static string TheNextDay = "TheNextDay";

    /// <summary>
    /// 客户端 更新成就任务进度事件 事件参数 对应 EM_AchievementTaskCondition
    /// </summary>
    public static string TaskAchvUpdateProgress = "TaskAchvUpdateProgress";

    /// <summary>
    /// 军团解散或者离开事件
    /// </summary>
    public static string LEGION_DISSOLVE_OR_QUIT = "LEGION_DISSOLVE_OR_QUIT";


    #region SDK_EVENT

    /// <summary>
    /// SDK登录结果
    /// </summary>
    public static string SDK_LOGIN_RESULT = "SDK_LOGIN_RESULT";
    /// <summary>
    /// 应用宝YSDK显示登录界面
    /// </summary>
    public static string SDK_YSDK_SHOWLOGINVIEW = "SDK_YSDK_SHOWLOGINVIEW";
    //应用宝显示用户别名
    public static string SDK_YSDK_SHOWSHOWNICKNAME = "SDK_YSDK_SHOWSHOWNICKNAME";
    /// <summary>
    /// SDK 通过游戏界面 处理离开游戏 可以选择退出，不退出
    /// </summary>
    public static string SDK_EXIT_BY_GAMEUI = "SDK_EXIT_BY_GAMEUI";
    
    /// <summary>
    /// SDK 强制退出 只能选择退出
    /// </summary>
    public static string SDK_EXIT_BY_FORCE = "SDK_EXIT_BY_FORCE";


    public static string SDK_PAY_RESULT = "SDK_PAY_RESULT";

    /// <summary>
    /// SDK 改变账号 （登出或者切换账号）
    /// </summary>
    public static string SDK_CHANGE_ACCOUNT = "SDK_CHANGE_ACCOUNT";

    #endregion

    /// <summary>
    /// 切换登录场景成功
    /// </summary>
    public static string LOADING_LOGIN_SCENE_SUC = "LOADING_LOGIN_SCENE_SUC";

    /// <summary>
    /// 启动日志功能
    /// </summary>
    public static string START_LOG = "START_LOG";

    /// <summary>
    /// 登陆排队处理事件
    /// </summary>
    public static string OnLoginQueueEvent = "OnLoginQueueEvent";



}
