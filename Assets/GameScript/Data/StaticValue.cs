using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;

/// <summary>
/// 保存玩家的各数据
/// </summary>
public class StaticValue
{


    //////////////////////////////////////////////////////////////////////////
    //环境相关

    public static bool m_isPlayMusic = true;
    public static string m_MusicName = "";
    public static bool m_isPlaySound = true;

    //////////////////////////////////////////////////////////////////////////
    public static string m_ChannelUserId = "1111";
    public static string m_PayChannel = "2222";
    public static int m_iServerId;
    public static string m_strServerName;
    public static bool m_bFirstReg = false;
    /// <summary>
    /// SDK的令牌
    /// </summary>
    public static string m_SDKToken = "test";

    /// <summary>
    /// 设计的游戏屏幕宽高
    /// </summary>
    public static float m_DesignScreenWidth = 1920;
    public static float m_DesignScreenHeight = 1080;
    /// <summary>
    /// 已经进行的时间
    /// </summary>
    public static float m_fPlayingTime = 0;

    //TsuCode
    public static int countOpenPopupFirstRecharge = 0;
    //
    public static int OpenPopupLevelGiftId = 0;

    //-----------------游戏环境变量------------------------------


    //-----------------------------------------------------------

    public static bool m_IsCancelQueue = false; //是否取消排队

    public static string m_LoginName = "";
    public static string m_LoginPwd = "";
    public static int m_iNewServerTime = -99;
    public static string m_LocalLevelGift = "";


    /// <summary>
    /// 玩家等级信息
    /// </summary>
    public static PlayerLevelInfo m_sLvInfo;
    /// <summary>
    /// 当前场景
    /// </summary>
	public static EM_Scene m_preScene = EM_Scene.Login;
    public static EM_Scene m_curScene = EM_Scene.Login;
    /// <summary>
    /// 当前战斗公共配置数据
    /// </summary>
    public static BattleCommonConfig m_CurBattleConfig = new BattleCommonConfig();

    public static Battle2MenuProcessParam m_Battle2MenuProcessParam = new Battle2MenuProcessParam();
    public static GetWayToBattleParam mGetWayToBattleParam = new GetWayToBattleParam();
    //等级升级界面参数配置
    public static int mOldEnergyValue = 0;
    public static int mOldVigorValue = 0;
    public static bool mIsNeedShowLevelPage = false;
    public static LevelUpPageParam mLevelUpPageParam;

    //管理员
    public static bool m_EquipRefine = false;

    //副本挑战关卡idKey
    public static string LastChallengeDungeonIdKey = "LastChallengeDungeonIdKey";

    //上一场战斗是否超时
    public static bool m_LastFightIsTimeOut;

    /// <summary>
    /// 初始化
    /// </summary>
    public static void f_ReInit()
    {
        m_CurBattleConfig = new BattleCommonConfig();
        m_Battle2MenuProcessParam = new Battle2MenuProcessParam();
        mGetWayToBattleParam = new GetWayToBattleParam();
    }
}
/// <summary>
/// 玩家等级信息
/// </summary>
public struct PlayerLevelInfo
{
    public int m_iLv
    {
        private set;
        get;
    }

    public int m_iExp
    {
        private set;
        get;
    }

    /// <summary>
    /// 添加经验后的等级
    /// </summary>
    public int m_iAddLv
    {
        private set;
        get;
    }

    /// <summary>
    /// 添加经验后的经验
    /// </summary>
    public int m_iAddExp
    {
        private set;
        get;
    }

    /// <summary>
    /// 添加的总经验值
    /// </summary>
    public int m_iTotalAddExp
    {
        private set;
        get;
    }

    /// <summary>
    /// 更新等级信息
    /// </summary>
    /// <param name="lv"></param>
    /// <param name="exp"></param>
    public void f_UpdateInfo(int lv, int exp)
    {
        m_iLv = lv;
        m_iExp = exp;
        m_iAddLv = lv;
        m_iAddExp = exp;
        m_iTotalAddExp = 0;
    }

    /// <summary>
    /// 添加经验
    /// </summary>
    /// <param name="exp"></param>
    public void f_AddExp(int exp)
    {
        m_iTotalAddExp += exp;
        int lvUpNeedExp = 0;
        CarLvUpDT carLvUpDT;
        while (exp > 0)
        {
            carLvUpDT = (CarLvUpDT)glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(m_iAddLv + 1);
            if (carLvUpDT != null)
            {
                lvUpNeedExp = carLvUpDT.iCardType;
                if (lvUpNeedExp > m_iAddExp + exp)
                {
                    m_iAddExp += exp;
                    exp = 0;
                }
                else
                {
                    exp -= lvUpNeedExp - m_iAddExp;
                    m_iAddLv++;
                    m_iAddExp = 0;
                }
            }
            else
            {
                exp = 0;
            }
        }

    }
}


public class BattleCommonConfig
{
    public BattleCommonConfig()
    {
        m_eBattleType = EM_Fight_Enum.eFight_Invalid;
        m_iChapterId = 0;
        m_iTollgateId = 0;
        m_iSceneId = 0;
    }

    /// <summary>
    /// 更新战斗公共配置信息
    /// </summary>
    /// <param name="battleType">战斗类型 EM_Fight_Enum</param>
    /// <param name="chapterId">章节Id，日常副本对应类型</param>
    /// <param name="tollgateId">关卡id</param>
    /// <param name="sceneId">场景Id，关卡脚本数据应该都有</param>
    public void f_UpdateInfo(EM_Fight_Enum battleType, int chapterId, int tollgateId, int sceneId)
    {
        m_eBattleType = battleType;
        m_iChapterId = chapterId;
        m_iTollgateId = tollgateId;
        m_iSceneId = sceneId;
        //更新剧情对话数据
        Data_Pool.m_DungeonPool.f_UpdateIsHavePlot();
        Data_Pool.m_DialogPool.f_UpdateCurDialogData(m_eBattleType, m_iChapterId, m_iTollgateId);
        f_UpdateBattle2MenuParam();
        f_UpdateLvInfo();

        if ((battleType == EM_Fight_Enum.eFight_DungeonMain || battleType == EM_Fight_Enum.eFight_DungeonElite) && tollgateId != GameParamConst.PLOT_TOLLGATEID)
            m_bIsHandle = (Data_Pool.m_DungeonPool.f_GetForId(chapterId) as DungeonPoolDT).f_GetTollgateData(tollgateId).mStarNum >= 1;
        else
            m_bIsHandle = false;
    }

    /// <summary>
    /// 更新战斗切换到主界面参数
    /// </summary>
    private void f_UpdateBattle2MenuParam()
    {
        switch (m_eBattleType)
        {
            case EM_Fight_Enum.eFight_Invalid:
                break;
            case EM_Fight_Enum.eFight_DungeonMain:
                //战斗结束会根据结果更新参数
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.Dungeon, m_eBattleType, m_iChapterId, m_iTollgateId);
                break;
            case EM_Fight_Enum.eFight_DungeonElite:
                //战斗结束会根据结果更新参数
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.Dungeon, m_eBattleType, m_iChapterId, m_iTollgateId);
                break;
            case EM_Fight_Enum.eFight_Legend:
                //战斗结束会根据结果更新参数
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.Dungeon, m_eBattleType, m_iChapterId, m_iTollgateId);
                break;
            case EM_Fight_Enum.eFight_DailyPve:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.Dungeon, m_eBattleType, m_iChapterId, m_iTollgateId);
                break;
            case EM_Fight_Enum.eFight_Rebel:
                break;
            case EM_Fight_Enum.eFight_Friend:
                break;
            case EM_Fight_Enum.eFight_Guild:
                break;
            case EM_Fight_Enum.eFight_Arena:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.Arena);
                break;
            case EM_Fight_Enum.eFight_ArenaSweep:
                break;
            case EM_Fight_Enum.eFight_CrusadeRebel:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.CrusadeRebel);
                break;
            case EM_Fight_Enum.eFight_Boss:
                break;
            case EM_Fight_Enum.eFight_LegionDungeon:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.LegionDungeon, m_iChapterId);
                break;
            case EM_Fight_Enum.eFight_GrabTreasure:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.GrabTreasure);
                break;
            case EM_Fight_Enum.eFight_GrabTreasureSweep:
                break;
            case EM_Fight_Enum.eFight_RunningMan:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.RunningMan);
                break;
            case EM_Fight_Enum.eFight_RunningManElite:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.RunningManElite);
                break;
            case EM_Fight_Enum.eFight_Patrol:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.Patrol);
                break;
            case EM_Fight_Enum.eFight_LegionBattle:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.LegionBattle, m_iChapterId);
                break;
            case EM_Fight_Enum.eFight_CrossServerBattle:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.CrossServerBattle);
                break;
            case EM_Fight_Enum.eFight_CardBattle:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.CardBattle);
                break;
            //TsuCode - CHaosBattle
            case EM_Fight_Enum.eFight_ChaosBattle:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.ChaosBattle);
                break;
            case EM_Fight_Enum.eFight_ArenaCross:
                StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.ArenaCrossBattle);
                break;
            //
            default:
                break;
        }
    }

    /// <summary>
    /// 战斗开始前更新等级信息
    /// </summary>
    private void f_UpdateLvInfo()
    {
        int lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        int exp = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Exp);
        StaticValue.m_sLvInfo.f_UpdateInfo(lv, exp);
    }

    public bool m_bIsHandle
    {
        private set;
        get;
    }

    /// <summary>
    /// 战斗类型
    /// </summary>
    public EM_Fight_Enum m_eBattleType
    {
        private set;
        get;
    }

    /// <summary>
    /// 章节Id
    /// </summary>
    public int m_iChapterId
    {
        private set;
        get;
    }

    /// <summary>
    /// 关卡Id
    /// </summary>
    public int m_iTollgateId
    {
        private set;
        get;
    }

    /// <summary>
    /// 场景Id
    /// </summary>
    public int m_iSceneId
    {
        private set;
        get;
    }

}