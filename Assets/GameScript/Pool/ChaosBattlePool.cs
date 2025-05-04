using ccU3DEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 跨服战(跨服演武) pool
/// </summary>
public class ChaosBattlePool : BasePool
{
    /// <summary>
    /// 购买次数元宝参数 公式：第N次数 = N * value
    /// </summary>
    public readonly int BuyTimesSyceeParam;
    
    //战斗剩余次数
    private int m_LeftBattleTimes;
    //战斗购买次数
    private int m_BattleBuyTimes;
    //战斗购买次数限制
    private int m_BattleBuyTimesLimit;

    //连胜场次
    private int m_Winstreak;

    //当前战区Id 通过ZoneId来设置 
    private int m_CurZoneId;
    //当前战区模板 不需要手动设置 ZoneId会自己设置
    private ChaosBattleZoneDT m_CurZoneTemplate;


    /// 当前头衔Id 通过TitleId来设置 
    private int m_CurTitleId;
    //头衔模板 不需要手动设置 TitleId会自己设置
    private ChaosBattleTitleDT m_CurTitleTemplate;
    //当前头衔星数
    private int m_CurTitleStar;
    //头衔模板表最大星数
    private int m_TitleStarMax;

    //对手玩家信息
    private ChaosBattlePoolDT m_EnemyInfo;

    //请求匹配回调
    private SocketCallbackDT m_MatchSocketCallbackDt;

    //是否结算
    private bool m_IsFinish;
    //战斗结果
    private int m_BattleResult;
    //战斗输了，获得的分数
    private int m_LoseScore;
    //战斗赢了，获得的分数
    private int m_WinScore;

    //初始化信息
    private bool m_IsInitInfo;

    #region 对外属性

    /// <summary>
    /// 跨服战购买次数
    /// </summary>
    public int BattleBuyTimes
    {
        get
        {
            return m_BattleBuyTimes;
        }
    }

    /// <summary>
    /// 跨服战购买次数限制
    /// </summary>
    public int BattleBuyTimesLimit
    {
        get
        {
            return m_BattleBuyTimesLimit;
        }
    }

    /// <summary>
    /// 跨服战剩余次数
    /// </summary>
    public int BattleLeftTimes
    {
        get
        {
            return m_LeftBattleTimes;
        }
    }

    /// <summary>
    /// 战区Id
    /// </summary>
    public int ZoneId
    {
        private set
        {
            m_CurZoneId = value;
            m_CurZoneTemplate = (ChaosBattleZoneDT)glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleZoneSC.f_GetSC(value);
            if (m_CurZoneTemplate == null)
            {
MessageBox.ASSERT("Model ID does not exist, id:" + value);
                //容错
                m_CurZoneTemplate = (ChaosBattleZoneDT)glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleZoneSC.f_GetSC(1);
            }
        }
        get
        {
            return m_CurZoneId;
        }
    }

    /// <summary>
    /// 战区模板
    /// </summary>
    public ChaosBattleZoneDT ZoneTemplate
    {
        get
        {
            return m_CurZoneTemplate;
        }
    }

    /// <summary>
    /// 官衔Id
    /// </summary>
    public int TitleId
    {
        private set
        {
            m_CurTitleId = value;
            m_CurTitleTemplate = (ChaosBattleTitleDT)glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleTitleSC.f_GetSC(value);
            if (m_CurZoneTemplate == null)
            {
MessageBox.ASSERT("Formal Title Setup ID does not exist,id:" + value);
                //容错
                m_CurTitleTemplate = (ChaosBattleTitleDT)glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleTitleSC.f_GetSC(1);
            }
        }
        get
        {
            return m_CurTitleId;
        }
    }

    /// <summary>
    /// 官衔模板
    /// </summary>
    public ChaosBattleTitleDT TitleTemplate
    {
        get
        {
            return m_CurTitleTemplate;
        }
    }

    /// <summary>
    /// 头衔星数
    /// </summary>
    public int TitleStar
    {
        get
        {
            return m_CurTitleStar;
        }
    }

    /// <summary>
    /// 头衔最大星数
    /// </summary>
    public int TitleStarMax
    {
        get
        {
            return m_TitleStarMax;
        }
    }

    /// <summary>
    /// 连胜场次
    /// </summary>
    public int Winstreak
    {
        get
        {
            return m_Winstreak;
        }
    }
    
    /// <summary>
    /// 对手玩家信息
    /// </summary>
    public ChaosBattlePoolDT EnemyInfo
    {
        get
        {
            return m_EnemyInfo;
        }
    }

    /// <summary>
    /// 战斗是否已经结算
    /// </summary>
    public bool IsFinish
    {
        get
        {
            return m_IsFinish;
        }
    }

    /// <summary>
    /// 战斗结果
    /// </summary>
    public int BattleResult
    {
        get
        {
            return m_BattleResult;
        }
    }

    /// <summary>
    /// 战斗结束获得分数
    /// </summary>
    public int ResultScore
    {
        get
        {
            return BattleResult > 0 ? m_WinScore : m_LoseScore;
        }
    }

    #endregion

    public ChaosBattlePool() : base("ChaosPoolDT", true)
    {
        //根据参数表数据初始化
        GameParamDT tParam = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_NeedLevel.ChaosBattle) as GameParamDT;
        if (tParam != null)
        {
            BuyTimesSyceeParam = tParam.iParam3; 
        }
        else
        {
            BuyTimesSyceeParam = 10;
        }
        
    }

    protected override void f_Init()
    {
        m_BattleBuyTimes = 0;
        m_BattleBuyTimesLimit = 0;
        ZoneId = 1;
        TitleId = 1;
        m_IsInitInfo = false;
        //官衔最大星数
        m_TitleStarMax = 0;
        List<NBaseSCDT> tList = glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleTitleSC.f_GetAll();
        for (int i = 0; i < tList.Count; i++)
        {
            ChaosBattleTitleDT tNode = (ChaosBattleTitleDT)tList[i];
            if (tNode.iStarNum > m_TitleStarMax)
                m_TitleStarMax = tNode.iStarNum;
        }
        m_EnemyInfo = new ChaosBattlePoolDT();

        //初始排行榜数据
        m_RankDict = new Dictionary<byte, CSChaosBattleRankZone>();
        List<NBaseSCDT> tZoneIniList = glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleZoneSC.f_GetAll();
        for (int i = 0; i < tZoneIniList.Count; i++)
        {
            ChaosBattleZoneDT tNode = (ChaosBattleZoneDT)tZoneIniList[i];
            if (!m_RankDict.ContainsKey((byte)tNode.iId))
                m_RankDict.Add((byte)tNode.iId, new CSChaosBattleRankZone(tNode));
            else
MessageBox.ASSERT("Available with the same ID");
        }
        
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, f_UpdateDataByNextDay);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.PlayerVipUpdate, f_UpdateDataByVipLv);
    }

    private void f_UpdateDataByNextDay(object value)
    {
        m_BattleBuyTimes = 0;
        m_Winstreak = 0;
        m_IsInitInfo = false;
    }

    private void f_UpdateDataByVipLv(object value)
    {
        m_BattleBuyTimesLimit = Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_ChaosBattleBuyTimes);
        //m_BattleBuyTimesLimit = Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_CrossServerBattleBuyTimes);
    }

    protected override void RegSocketMessage()
    {
        CMsg_SC_ChaosBattleInfor tSC_ChaosBattleInfor = new CMsg_SC_ChaosBattleInfor();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ChaosBattleInfor, tSC_ChaosBattleInfor, f_SC_ChaosBattleInfoHandle);

        CMsg_SC_ChaosBattlePlayer tSC_ChaosBattlePlayer = new CMsg_SC_ChaosBattlePlayer();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ChaosBattlePlayer, tSC_ChaosBattlePlayer, f_SC_ChaosBattlePlayerHandle);

        CMsg_SC_ChaosBattleResult tSC_ChaosBattleResult = new CMsg_SC_ChaosBattleResult();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ChaosBattleResult, tSC_ChaosBattleResult, f_SC_ChaosBattleResultHandle);

        CMsg_SC_ChaosRankNode tSC_ChaosRankNode = new CMsg_SC_ChaosRankNode();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_ChaosRank, tSC_ChaosRankNode, f_SC_ChaosRankHandle);
        //GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_ChaosRank, tSC_ChaosRankNode, f_SC_ChaosRankHandle_Score);

        CMsg_SC_ChaosRankNode tSC_ChaosRankNodeSelf = new CMsg_SC_ChaosRankNode();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ChaosRankSelf, tSC_ChaosRankNodeSelf, f_SC_ChaosRankSelfHandle);

        //Record ------------
        CMsg_SC_ChaosHistory tSC_ChaosHistory = new CMsg_SC_ChaosHistory();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_ChaosHistory, tSC_ChaosHistory, f_SC_ChaosHistoryHandle);
    }

    private void f_SC_ChaosHistoryHandle(int iData1, int iData2, int iNum, ArrayList aData)
    {
        MessageBox.ASSERT("In Here SC_CHAOSHISTORy");
        if (m_listChaosHistory != null) m_listChaosHistory.Clear();
        m_listChaosHistory = new List<ChaosHistoryPoolDT>(); 
        for (int i = 0; i < aData.Count; i++)
        {
            CMsg_SC_ChaosHistory serverData = (CMsg_SC_ChaosHistory)aData[i];
            ChaosHistoryPoolDT tNode = new ChaosHistoryPoolDT(serverData.userId, serverData.serverId, serverData.userName, serverData.enemyId, serverData.serverEnemyId, serverData.enemyName, serverData.battleRes, serverData.recordTime, serverData.note);
            //ChaosHistoryPoolDT tNode = new ChaosHistoryPoolDT(serverData.userId, serverData.serverId, 
            //    serverData.userName, serverData.enemyId, serverData.serverEnemyId, serverData.battleRes,
            //    serverData.recordTime, serverData.note);
            m_listChaosHistory.Add(tNode);
        }
    }


    #region 协议处理

    private void f_SC_ChaosBattleInfoHandle(object value)
    {
        CMsg_SC_ChaosBattleInfor serverInfo = (CMsg_SC_ChaosBattleInfor)value;
        //CMsg_SC_CrossBattleInfor serverInfo1 = (CMsg_SC_CrossBattleInfor)value;
        ////Convert
        //CMsg_SC_ChaosBattleInfor serverInfo = new CMsg_SC_ChaosBattleInfor();
        //serverInfo.iBuyTimes = serverInfo.iBuyTimes;
        //serverInfo.iCrossIntegral = serverInfo1.iCrossIntegral;
        //serverInfo.iLeftTimes = serverInfo1.iLeftTimes;
        //serverInfo.iCrossLv = serverInfo1.iCrossLv;
        //serverInfo.iWinningTimes = serverInfo1.iWinningTimes;
        //serverInfo.iZone = serverInfo1.iZone;
        //
        m_IsInitInfo = true;
        m_WinScore = 0;
        m_LoseScore = 0;
        m_Winstreak = serverInfo.iWinningTimes;
        if (null != ZoneTemplate)
        {
            //连胜是胜利次数减一，，两场胜算连胜一场
            int continueWin = m_Winstreak > 1 ? m_Winstreak - 1 : 0;
            int integralArard = continueWin >= 10 ? ZoneTemplate.iWinSteakAward * 10 : ZoneTemplate.iWinSteakAward * continueWin;
            m_WinScore = ZoneTemplate.iWinAward + integralArard;
            m_LoseScore = ZoneTemplate.iLoseAward;
        }
        ZoneId = serverInfo.iZone;
        TitleId = serverInfo.iCrossLv;
        m_CurTitleStar = serverInfo.iCrossIntegral;
        m_LeftBattleTimes = serverInfo.iLeftTimes;
        m_BattleBuyTimes = serverInfo.iBuyTimes;
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ChaosBattle_TimesUpdate, m_BattleBuyTimes);
    }

    private void f_SC_ChaosBattlePlayerHandle(object value)
    {
        CMsg_SC_ChaosBattlePlayer serverInfo = (CMsg_SC_ChaosBattlePlayer)value;
        //CMsg_SC_CrossBattlePlayer serverInfo1 = (CMsg_SC_CrossBattlePlayer)value;
        ////Convert
        //CMsg_SC_ChaosBattlePlayer serverInfo = new CMsg_SC_ChaosBattlePlayer();
        //serverInfo.iServerId = serverInfo1.iServerId;
        //serverInfo.iTempId = serverInfo1.iTempId;
        //serverInfo.lv = serverInfo1.lv;
        //serverInfo.power = serverInfo1.power;
        //serverInfo.uFrameId = serverInfo1.uFrameId;
        //serverInfo.szRoleName = serverInfo1.szRoleName;
        //serverInfo.userId = serverInfo1.userId;
        //serverInfo.uVipLv = serverInfo1.uVipLv;
        //
        m_EnemyInfo.f_UpdateInfo(serverInfo.userId, serverInfo.iServerId, serverInfo.iTempId,
            serverInfo.uFrameId, serverInfo.power, serverInfo.szRoleName, serverInfo.lv);
    }

    private void f_SC_ChaosBattleResultHandle(object value)
    {
        CMsg_SC_ChaosBattleResult serverInfo = (CMsg_SC_ChaosBattleResult)value;
        //CMsg_SC_CrossBattleResult serverInfo1 = (CMsg_SC_CrossBattleResult)value;
        ////Convert
        //CMsg_SC_ChaosBattleResult serverInfo = new CMsg_SC_ChaosBattleResult();
        //serverInfo.iOperateResult = serverInfo1.iOperateResult;
        //serverInfo.iResult = serverInfo1.iResult;
        //

        m_IsFinish = true;
        m_BattleResult = serverInfo.iResult;
        if (m_MatchSocketCallbackDt != null &&
            m_MatchSocketCallbackDt.m_ccCallbackSuc != null &&
            m_MatchSocketCallbackDt.m_ccCallbackFail != null)
        {
            if (serverInfo.iOperateResult == (int)eMsgOperateResult.OR_Succeed)
            {
                //更新战斗数据
                StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_ChaosBattle, 0, 0, 0);
                m_MatchSocketCallbackDt.m_ccCallbackSuc(serverInfo.iOperateResult);
            }
            else
            {
                m_MatchSocketCallbackDt.m_ccCallbackFail(serverInfo.iOperateResult);
            }
            m_MatchSocketCallbackDt = null;
        }
    }

    private void f_SC_ChaosRankHandle(int zoneId, int value2, int num, System.Collections.ArrayList arrayList)
    {
        m_listChaosRank = new List<ChaosBattleRankPoolDT>(); //ChaosRank by Score
        if (!m_RankDict.ContainsKey((byte)zoneId))
        {
MessageBox.ASSERT(string.Format("The server sent data from regions that do not exist，zoneId:{0}", zoneId));
            return;
        }
        for (int i = 0; i < arrayList.Count; i++)
        {
            CMsg_SC_ChaosRankNode serverData = (CMsg_SC_ChaosRankNode)arrayList[i];
            //CMsg_SC_CrossRankNode serverData = (CMsg_SC_CrossRankNode)arrayList[i];
            ChaosBattleRankPoolDT tNode = new ChaosBattleRankPoolDT(serverData.uRankIdx, serverData.uRankTitle, serverData.uServerId, serverData.uPower, serverData.szRoleName, serverData.uChaosScore);
            m_RankDict[(byte)zoneId].f_AddRankNode(tNode);
            //TsuCode - Rank by ChaosScore
            m_listChaosRank.Add(tNode);
        }
    }
    private void f_SC_ChaosRankHandle_Score(int zoneId, int value2, int num, System.Collections.ArrayList arrayList)
    {
        m_listChaosRank = new List<ChaosBattleRankPoolDT>();
        for (int i = 0; i < arrayList.Count; i++)
        {
            CMsg_SC_ChaosRankNode serverData = (CMsg_SC_ChaosRankNode)arrayList[i];
            ChaosBattleRankPoolDT tNode = new ChaosBattleRankPoolDT(serverData.uRankIdx, serverData.uRankTitle, serverData.uServerId, serverData.uPower, serverData.szRoleName, serverData.uChaosScore);
            m_listChaosRank.Add(tNode);
        }
    }

    private void f_SC_ChaosRankSelfHandle(object value)
    {
        CMsg_SC_ChaosRankNode serverData = (CMsg_SC_ChaosRankNode)value;
        //CMsg_SC_CrossRankNode serverData1 = (CMsg_SC_CrossRankNode)value;
        ////Convert
        //CMsg_SC_ChaosRankNode serverData = new CMsg_SC_ChaosRankNode();
        //serverData.szRoleName = serverData1.szRoleName;
        //serverData.uPower = serverData1.uPower;
        //serverData.uRankIdx = serverData1.uRankIdx;
        //serverData.uRankTitle = serverData1.uRankTitle;
        //serverData.uServerId = serverData1.uServerId;
            //
        m_SelfRankInfo = new ChaosBattleRankPoolDT(serverData.uRankIdx,serverData.uRankTitle,serverData.uServerId,serverData.uPower,serverData.szRoleName, serverData.uChaosScore);
    }

    #endregion

    #region 发送协议
    /// <summary>
    /// 初始化跨服战数据
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_ChaosBattleInit(SocketCallbackDT socketCallbackDt)
    {
        if (m_IsInitInfo && socketCallbackDt != null && socketCallbackDt.m_ccCallbackSuc != null)
        {
            socketCallbackDt.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ChaosBattleInfor, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ChaosBattleInfor, bBuf);
    }

    /// <summary>
    /// 匹配战斗
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_MatchBattle(SocketCallbackDT socketCallbackDt)
    {
        m_EnemyInfo.f_ResetInfo();
        m_IsFinish = false;
        m_MatchSocketCallbackDt = socketCallbackDt;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ChaosBattle, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ChaosBattle, bBuf);
    }

    /// <summary>
    /// 购买次数
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_BuyTimes(byte buyTimes,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ChaosBattleBuyTims, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(buyTimes);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ChaosBattleBuyTims, bBuf);
    }

    public void f_RankList(short zoneId, byte pageIdx, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ChaosRank, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(zoneId);
        tCreateSocketBuf.f_Add(pageIdx);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ChaosRank, bBuf);
    }

    public void f_RankListSelf(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ChaosRankSelf, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ChaosRankSelf, bBuf);
    }

    //TsuCode - Chaos Record  //--Record
    public void f_ChaosHistory(SocketCallbackDT socketCallbackDt)
    {
        MessageBox.ASSERT("TSULOG CHEck ChaosHistory Send CS");
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ChaosHistory, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ChaosHistory, bBuf);
    }

  
    //

    #endregion

    #region 对外接口

    #endregion

    #region 排行榜相关
    /// <summary>
    /// 排行榜X个每页
    /// </summary>
    //private const int RANK_LIST_NUM_PRE_PAGE = 10;
    private const int RANK_LIST_NUM_PRE_PAGE = 30; //TsuCode - ChaosList By Rank
    /// <summary>
    /// 排行榜页数最大值
    /// </summary>
    private const int RANK_LIST_PAGE_MAX = 10;

    private Dictionary<byte, CSChaosBattleRankZone> m_RankDict;

    private int m_SelfRequestTime = 0;
    private ChaosBattleRankPoolDT m_SelfRankInfo;

    /// <summary>
    /// 排行榜数据
    /// </summary>
    public Dictionary<byte, CSChaosBattleRankZone> RankDict
    {
        get
        {
            return m_RankDict;
        }
    }

    //TsuCode - Chaos History
    private List<ChaosHistoryPoolDT> m_listChaosHistory;
    public List<ChaosHistoryPoolDT> ListChaosHistory
    {
        get
        {
            return m_listChaosHistory;
        }
    }
    //------------------------------

    /// <summary>
    /// ChaosRank by Score - TsuCode
    /// </summary>
    private List<ChaosBattleRankPoolDT> m_listChaosRank;
    int chaosRankRequestTime = 0;
    public List<ChaosBattleRankPoolDT> ListChaosRank
    {
        get
        {
            return m_listChaosRank;
        }
    }
    /// <summary>
    /// 个人排行榜信息
    /// </summary>
    public ChaosBattleRankPoolDT SelfRankInfo
    {
        get
        {
            if (null != Data_Pool.m_TeamPool)
            {
                //服务器定时更得，造成战斗力跟其他地方不一致，，这里直接用本地得计算，保持统一
                m_SelfRankInfo.UpdateChangeData(TitleId, Data_Pool.m_TeamPool.f_GetTotalBattlePower());
            }
            return m_SelfRankInfo;
        }
    }

    private ccCallback m_Callback_RankList;
    private byte m_CurRankZoneId;
    private bool m_RankListWait = false;
    public void f_ExecuteAfterRankList(byte zoneId, bool first, ref bool needUpdate, ccCallback callbackRankList)
    {
        if (!m_RankDict.ContainsKey(zoneId))
        {
MessageBox.ASSERT("The leaderboard received from the region does not exist");
            return;
        }
        if (m_RankListWait)
        {
MessageBox.DEBUG("Waiting for a response from the server");
            return;
        } 
        m_CurRankZoneId = zoneId;
        m_Callback_RankList = callbackRankList; 
        int tNow = GameSocket.GetInstance().f_GetServerTime();
        int tLast = m_RankDict[zoneId].RequestTime;
        //排行榜个人信息不是同一天 请求刷新
        bool selfNeedUpdate = ccMath.f_GetTotalDaysByTime(tNow) != ccMath.f_GetTotalDaysByTime(m_SelfRequestTime);
        if (selfNeedUpdate)
        {
            m_SelfRequestTime = tNow;
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_RankListSelf;
            socketCallbackDt.m_ccCallbackFail = f_Callback_RankListSelf;
            f_RankListSelf(socketCallbackDt);
        }
        //排行榜 不是同一天就刷新 跨天刷新
        needUpdate = ccMath.f_GetTotalDaysByTime(tNow) !=  ccMath.f_GetTotalDaysByTime(tLast);
        if (needUpdate)
        {
            m_RankDict[zoneId].f_UpdateRequestTime(tNow);
            m_RankDict[zoneId].f_UpdatePageIdx(0);
            m_RankDict[zoneId].RankList.Clear();
            m_RankListWait = true;
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_RankList;
            socketCallbackDt.m_ccCallbackFail = f_Callback_RankList;
            f_RankList((byte)zoneId, (byte)m_RankDict[zoneId].PageIdx, socketCallbackDt);
        }
        else
        {
            if (first)
            {
                if (m_Callback_RankList != null)
                    m_Callback_RankList(eMsgOperateResult.OR_Succeed);
            }
            else
            {
                if (m_RankDict[zoneId].RankList.Count == m_RankDict[zoneId].PageIdx * RANK_LIST_NUM_PRE_PAGE && m_RankDict[zoneId].PageIdx < RANK_LIST_PAGE_MAX)
                {
                    m_RankListWait = true;
                    SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
                    socketCallbackDt.m_ccCallbackSuc = f_Callback_RankList;
                    socketCallbackDt.m_ccCallbackFail = f_Callback_RankList;
                    f_RankList((byte)zoneId, (byte)m_RankDict[zoneId].PageIdx, socketCallbackDt);
                }
                else
                {
                    if (m_Callback_RankList != null)
                        m_Callback_RankList(eMsgOperateResult.OR_Succeed);
                }
            }
        }
    }


    private void f_Callback_RankList(object result)
    {
        m_RankDict[m_CurRankZoneId].f_UpdatePageIdx(m_RankDict[m_CurRankZoneId].PageIdx + 1);
        m_RankListWait = false;
        if (m_Callback_RankList != null)
            m_Callback_RankList(result);
    }

    private void f_Callback_RankList_Score(object result)
    {
        m_RankListWait = false;
        if (m_Callback_RankList != null)
            m_Callback_RankList(result);
    }
    private void f_Callback_RankListSelf(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
MessageBox.ASSERT("Error requesting personal data,code:" + (int)result);
        }
    }

    #endregion

    #region 无用

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }

    #endregion

}


public class CSChaosBattleRankZone
{
    private int m_ZoneId;
    private ChaosBattleZoneDT m_ZoneTemplate;
    private int m_RequestTime;
    private int m_PageIdx;
    private List<BasePoolDT<long>> m_RankList;

    public ChaosBattleZoneDT ZoneTemplate
    {
        get
        {
            return m_ZoneTemplate;
        }
    }

    public int RequestTime
    {
        get
        {
            return m_RequestTime;
        }
    }

    public int PageIdx
    {
        get
        {
            return m_PageIdx;
        }
    }

    public List<BasePoolDT<long>> RankList
    {
        get
        {
            return m_RankList;
        }
    }

    public CSChaosBattleRankZone(ChaosBattleZoneDT zoneTemplate)
    {
        m_ZoneId = zoneTemplate.iId;
        m_ZoneTemplate = zoneTemplate;
        m_RequestTime = 0;
        m_PageIdx = 0;
        m_RankList = new List<BasePoolDT<long>>(); 
    }

    public void f_UpdateRequestTime(int requestTime)
    {
        m_RequestTime = requestTime;
    }

    public void f_UpdatePageIdx(int pageIdx)
    {
        m_PageIdx = pageIdx;
    }

    public void f_AddRankNode(ChaosBattleRankPoolDT rankNode)
    {
        m_RankList.Add(rankNode);
    }
    
}
