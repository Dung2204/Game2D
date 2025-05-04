using UnityEngine;
using System.Collections;
using ccU3DEngine;

/// <summary>
/// UI消息定义
/// </summary>
public class UIMessageDef : BaseUIMessageDef
{
    /// <summary>
    /// 资源加载进度
    /// </summary>
    public static string UI_RESOURCELOADPROGRESS = "UI_RESOURCELOADPROGRESS";
    /// <summary>
    /// 资源加载完消息
    /// </summary>
    public static string UI_RESOURCECOMPLETE = "UI_RESOURCECOMPLETE";
    /// <summary>
    /// 登录页选择服务器名字消息
    /// </summary>
    public static string UI_LOGINSERVERNAME = "UI_LOGINSERVERNAME";
    /// <summary>
    /// 资源加载错误消息
    /// </summary>
    public static string UI_DOWNLOADRESERO = "UI_DOWNLOADRESERO";

    /// <summary>
    /// 副本章节数据更新
    /// </summary>
    public static string UI_DUNGEON_CHAPTER_UPDATE = "UI_DUNGEON_CHAPTER_UPDATE";

    /// <summary>
    /// 副本一键领取宝箱成功
    /// </summary>
    public static string UI_DUNGEON_CHAPTER_ONEKEYGETREWARD_SUC = "UI_DUNGEON_CHAPTER_ONEKEYGETREWARD_SUC";

    /// <summary>
    /// 发送接口包消息
    /// </summary>
    public static string UI_SENDINTERFACEPACK = "UI_SENDINTERFACEPACK";

    /// <summary>
    /// 初始化完成，打开主场景开始游戏
    /// </summary>
    public static string UI_STARTGAME = "UI_STARTGAME";

    /// <summary>
    /// 通知主菜单界面更新界面数据（元宝，等级等）
    /// </summary>
    public static string UI_UPDATE_USERINFOR = "UI_UPDATE_USERINFO";
    /// <summary>
    /// 通知更新模型数据
    /// </summary>
    public static string UI_UPDATE_MODELINFOR = "UI_UPDATE_MODELINFOR";
    /// <summary>
    /// 主线任务进度ui更新
    /// </summary>
    public static string UI_TASKMAINUPDATE = "UI_TASKMAINUPDATE";
    /// <summary>
    /// 阵容界面更新缘分消息
    /// </summary>
    public static string UI_FATE_TRIP = "UI_FATE_TRIP";
    /// <summary>
    /// 布阵界面更新站位消息
    /// </summary>
    public static string UI_ClothArrayChanged = "UI_ClothArrayChanged";
    /// <summary>
    /// 聊天接受信息
    /// </summary>
    public static string RECEIVEMESSAGE = "RECEIVEMESSAGE";
    public static string UI_ChatRoomUpdate = "UI_ChatRoomUpdate";
    public static string UI_UpdateSevenBtn = "UI_UpdateSevenBtn";


    public static string UI_OpenGameNotice = "UI_OpenGameNotice";
    #region 剧情相关事件
    /// <summary>
    /// 战斗开始时
    /// </summary>
    public static string UI_DIALOG_BATTLESTART = "UI_DIALOG_BATTLESTART";
    /// <summary>
    /// 显示最后一轮
    /// </summary>
    public static string UI_SHOWLASTTURN = "UI_SHOWLASTTURN";
    /// <summary>
    /// 战斗结束时
    /// </summary>
    public static string UI_DIALOG_BATTLEFINISH = "UI_DIALOG_BATTLEFINISH";
    /// <summary>
    /// 某个站位的角色临死前
    /// </summary>
    public static string UI_DIALOG_ROLEBEFOREDEATH = "UI_DIALOG_ROLEBEFOREDEATH";

    #endregion

    #region 战斗场景UI更新消息
    /// <summary>
    /// 暂停战斗
    /// </summary>
    public static string FIGHTPAUSE = "FIGHTPAUSE";

    /// <summary>
    /// 恢复战斗
    /// </summary>
    public static string FIGHTRESUME = "FIGHTRESUME";

    /// <summary>
    /// 回合更新消息
    /// </summary>
    public static string UI_BATTLE_TURNINFOR = "UI_BATTLE_TURNINFOR";

    /// <summary>
    /// 隐藏战斗场景
    /// </summary>
    public static string UI_BATTLE_ACTIVE = "UI_BATTLE_ACTIVE";

    ///// <summary>
    ///// 战斗结束
    ///// </summary>
    //public static string UI_BATTLE_END = "UI_BATTLE_END";

    /// <summary>
    /// 战斗速度改变
    /// </summary>
    public static string UI_BATTLE_SPEED = "UI_BATTLE_SPEED";

    /// <summary>
    /// 设置战斗场景的UI的层级
    /// </summary>
    public static string UI_SETBATTLEUIDEPTH = "UI_SETBATTLEUIDEPTH";

    /// <summary>
    /// 剧情开始后战斗
    /// </summary>
    public static string DIALOGSTARBATTLE = "DIALOGSTARBATTLE";
    #endregion

    /// <summary>
    /// 跨天UI处理事件
    /// </summary>
    public static string UI_THENEXTDAY_UIPROCESS = "UI_THENEXTDAY_UIPROCESS";

    /// <summary>
    /// 好友数据更新
    /// </summary>
    public static string UI_FRIENDDATA_UPDATE = "UI_FRIENDDATA_UPDATE";

    public static string UI_PATROLEVENT_UPDATE = "UI_PATROLEVENT_UPDATE";

    #region 军团相关界面刷新消息

    /// <summary>
    /// 申请军团列表更新
    /// </summary>
    public static string UI_LEGION_SELf_LEGION_APPLY_LIST_UPDATE = "UI_LEGION_SELf_LEGION_APPLY_LIST_UPDATE";

    /// <summary>
    /// 军团申请者列表更新（军团管理成员）
    /// </summary>
    public static string UI_LEGION_APPLICANT_DATA_UPDATE = "UI_LEGION_APPLICANT_DATA_UPDATE";

    /// <summary>
    /// 军团成员数据更新
    /// </summary>
    public static string UI_LEGION_MEMBER_DATA_UPDATE = "UI_LEGION_MEMBER_DATA_UPDATE";

    /// <summary>
    /// 自己的军团信息更新
    /// </summary>
    public static string UI_SELF_LEGION_INFO_UPDATE = "UI_SELF_LEGION_INFO_UPDATE";

    /// <summary>
    /// 军团显示弹窗警告
    /// </summary>
    public static string UI_LEGION_WARN_SHOW = "UI_LEGION_WARN_SHOW";

    /// <summary>
    /// 军团强制关闭事件
    /// </summary>
    public static string UI_LEGION_FORCE_CLOSE = "UI_LEGION_FORCE_CLOSE";

    /// <summary>
    /// 军团战防守列表更新
    /// </summary>
    public static string UI_LEGION_BATTLE_DEFENCE_LIST_UPDATE = "UI_LEGION_BATTLE_DEFENCE_LIST_UPDATE";

    #endregion

    /// <summary>
    /// 红点更新消息
    /// </summary>
    public static string UI_ReddotUpdateMessage = "UI_ReddotUpdateMessage";

    /// <summary>
    /// 战斗横震屏消息打开
    /// </summary>
    public static string UI_BattlePopOpenW = "UI_BattlePopOpenW";

    /// <summary>
    /// 战斗横震屏消息关闭
    /// </summary>
    public static string UI_BattlePopCloseW = "UI_BattlePopCloseW";

    /// <summary>
    /// 战斗竖震屏消息打开
    /// </summary>
    public static string UI_BattlePopOpenH = "UI_BattlePopOpenH";

    /// <summary>
    /// 战斗竖震屏消息关闭
    /// </summary>
    public static string UI_BattlePopCloseH = "UI_BattlePopCloseH";
	
	public static string UI_BATTLE_START = "UI_BATTLE_START";
    public static string UI_BATTLE_SKILL_OPEN = "UI_BATTLE_SKILL_OPEN";
    public static string UI_BATTLE_SKILL_CLOSE = "UI_BATTLE_SKILL_CLOSE";


    #region 背包
    public static string UI_UpdateGoodsBag = "UI_UpdateGoodsBag";
    #endregion


    /// <summary>
    /// 充值处理 更新UI
    /// </summary>
    public static string UI_PAY_UPDATE_VIEW = "UI_PAY_UPDATE_VIEW";

    /// <summary>
    /// 布阵界面关闭事件
    /// </summary>
    public static string UI_ClothArrayPage_CLOSE = "UI_ClothArrayPage_CLOSE";

    /// <summary>
    /// 竞技场排行榜 关闭事件
    /// </summary>
    public static string UI_ArenaRankPage_CLOSE = "UI_ClothArrayPage_CLOSE";

    /// <summary>
    /// 跨服战挑战次数更新
    /// </summary>
    public static string UI_CrossServerBattle_TimesUpdate = "UI_CrossServerBattle_TimesUpdate";

    //TsuCode - ChaosBattle
    public static string UI_ChaosBattle_TimesUpdate = "UI_ChaosBattle_TimesUpdate";
    //------------

    /// <summary>
    /// 斗将阵容界面 更新阵容
    /// </summary>
    public static string UI_CardBattleClothPage_UpdateCloth = "UI_CardBattleClothPage_UpdateCloth";

    /// <summary>
    /// 通天转盘界面  更新记录链表信息
    /// </summary>
    public static string UI_TurntablePage_UpdateRecord = "UI_TurntablePage_UpdateRecord";

    /// <summary>
    /// 通天转盘界面  更新宝箱信息
    /// </summary>
    public static string UI_TurntablePage_UpdateBoxInfo = "UI_TurntablePage_UpdateBoxInfo";

    /// <summary>
    /// 提示有未处理订单
    /// </summary>
    public static string UI_UnsettledPay_Tip = "UI_UnsettledPay_Tip";
    /// <summary>
    /// 服务器广播消息
    /// </summary>
    public static string UI_ServerBroadcast = "UI_ServerBroadcast";
    /// <summary>
    /// 跑马灯服务器广播消息
    /// </summary>
    public static string UI_ServerNoticeBroadcast = "UI_ServerNoticeBroadcast";


    /// <summary>
    /// 初始化某个关卡剧情
    /// </summary>
    public static string UI_INIT_CHECKPOINT_PLOT = "UI_INIT_CHECKPOINT_PLOT";

    /// <summary>
    /// 剧情检测
    /// </summary>
    public static string UI_PLOT_CHECK = "UI_PLOT_CHECK";

    /// <summary>
    /// 换武将
    /// </summary>
    public static string UI_CHANGE_FIGHT_ROLE = "UI_CHANGE_FIGHT_ROLE";

    /// <summary>
    /// 换武将结束
    /// </summary>
    public static string UI_CHANGE_FIGHT_ROLE_END = "UI_CHANGE_FIGHT_ROLE_END";

    /// <summary>
    /// 显示武将
    /// </summary>
    public static string UI_SHOW_FIGHT_ROLE = "UI_SHOW_FIGHT_ROLE";

    /// <summary>
    /// 显示武将结束
    /// </summary>
    public static string UI_SHOW_FIGHT_ROLE_END = "UI_SHOW_FIGHT_ROLE_END";

    /// <summary>
    /// 指定某个武将释放某个技能
    /// </summary>
    public static string UI_SET_FIGHT_ROLE_SKILL = "UI_SET_FIGHT_ROLE_SKILL";

    /// <summary>
    /// 指定某个武将释放某个技能结束
    /// </summary>
    public static string UI_SET_FIGHT_ROLE_SKILL_END = "UI_SET_FIGHT_ROLE_SKILL_END";

    public static string UI_ONLINE_VIP_AWARD = "UI_ONLINE_VIP_AWARD";

    public static string UI_VOTE_APP_AWARD = "UI_VOTE_APP_AWARD";

    public static string UI_EVENT_TIME = "UI_EVENT_TIME";
    public static string UI_OPEN_EVENT_TIME = "UI_OPEN_EVENT_TIME";

    public static string UI_RANKING_POWER_LIST = "UI_RANKING_POWER_LIST";

    public static string UI_CrossArenaPool = "UI_CrossArenaPool";
    public static string UI_CrossArenaRankPool = "UI_CrossArenaRankPool";
    public static string UI_CrossArenaRecordPool = "UI_CrossArenaRecordPool";

    public static string CrossTournament_Regedit_SUC = "CrossTournament_Regedit_SUC";
    public static string CrossTournament_Bet_SUC = "CrossTournament_Bet_SUC";
    public static string CrossTournament_Update_Knock = "CrossTournament_Update_Knock";
    public static string CrossTournament_Update_GroupStage = "CrossTournament_Update_GroupStage";
    public static string CrossTournament_Bet_Info = "CrossTournament_Bet_Info";

    public static string BATTLE_SHOW_AURA_DETAIL = "BATTLE_SHOW_AURA_DETAIL";


}
