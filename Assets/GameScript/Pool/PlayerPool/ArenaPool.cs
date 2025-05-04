using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaPool : BasePool
{
    /// <summary>
    /// 竞技场排名
    /// </summary>
    private int mRank;
    /// <summary>
    /// 竞技场排名
    /// </summary>
    public int m_iRank
    {
        get
        {
            return mRank;
        }
    }

    private int mHighstRank;
    /// <summary>
    /// 历史最高排名
    /// </summary>
    public int m_iHighstRank
    {
        get
        {
            return mHighstRank;
        }
    }

    //目标
    private int mTargetRank;
    public int m_iTargetRank
    {
        get
        {
            return mTargetRank;
        }
    }

    /// <summary>
    /// 今日挑战次数
    /// </summary>
    private int mTimes;
    public int m_iTimes
    {
        get
        {
            return mTimes;
        }
    }

    /// <summary>
    /// 是否上榜
    /// </summary>
    private bool mIsOnRank;
    public bool m_IsOnRank
    {
        get
        {
            return mIsOnRank;
        }
    }

    private List<BasePoolDT<long>> mArenaList;

    /// <summary>
    /// 竞技场挑战结果
    /// </summary>
    public CMsg_SC_ArenaRet m_ArenaRet
    {
        private set;
        get;
    }

    public int enemyLevel = 1;
    public int enemySex = 0;

    public SocketCallbackDT arenaSweepCallbackDt;
    public List<SC_Award> mHandilyChallengeRewardList;
    public CMsg_SC_ArenaSweepRet m_sArenaSweepRet
    {
        private set;
        get;
    }

    public CMsg_SC_ArenaChooseAward m_sArenaChooseAwardRet
    {
        private set;
        get;
    }

    public bool mIsFinish
    {
        private set;
        get;
    }

    public ArenaBreakRankInfo mBreakRankInfo = new ArenaBreakRankInfo();

    public ArenaPool() : base("ArenaPoolDT")
    {

    }

    protected override void f_Init()
    {
        mRank = 0;
        mTargetRank = 0;
        mTimes = 0;
        mIsOnRank = false;
        mArenaList = new List<BasePoolDT<long>>();
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, f_UpdateDataByNextDay);
    }

    #region 跨天处理

    /// <summary>
    /// 跨天处理
    /// </summary>
    /// <param name="value"></param>
    private void f_UpdateDataByNextDay(object value)
    {
        f_Reset();
        //跨天UI事件  通知关心此消息的UI更新
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_THENEXTDAY_UIPROCESS, EM_NextDaySource.ArenaPool);
    }

    //重置数据
    private void f_Reset()
    {
        mTimes = 0;
    }

    #endregion

    protected override void RegSocketMessage()
    {
        //玩家数据 游戏服返回
        CMsg_SC_Arena tSC_Arena = new CMsg_SC_Arena();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_Arena, tSC_Arena, f_Callback_Arena);

        //玩家列表数据 关系服返回
        CMsg_RTC_ArenaListNode tRC_ArenaListNode = new CMsg_RTC_ArenaListNode();
        ChatSocket.GetInstance().f_RegMessage_Int2((int)ChatSocketCommand.RC_ArenaList, tRC_ArenaListNode, Callback_SocketData_ArenaList);

        //挑战结果
        CMsg_SC_ArenaRet tSC_ArenaRet = new CMsg_SC_ArenaRet();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ArenaRet, tSC_ArenaRet, f_Callback_ArenaRet);

        //扫荡结果处理
        SC_Award tSC_ArenaSweepRet = new SC_Award();
        GameSocket.GetInstance().f_RegMessage_Int3((int)SocketCommand.SC_ArenaSweepRet, tSC_ArenaSweepRet, f_Callback_ArenaSweepRet);

        //选择奖励结果
        CMsg_SC_ArenaChooseAward tSC_ArenaChooseAward = new CMsg_SC_ArenaChooseAward();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ArenaChooseAward, tSC_ArenaChooseAward, f_Callback_ArenaChooseAward);

        //竞技场
        CMsg_RTC_ArenaRankList tRC_ArenaRankList = new CMsg_RTC_ArenaRankList();
        ChatSocket.GetInstance().f_RegMessage((int)ChatSocketCommand.RC_ArenaRankList, tRC_ArenaRankList, f_Callback_ArenarRankList);

    }

    #region 无用

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }

    #endregion

    /// <summary>
    /// 修改自己的竞技场数据
    /// </summary>
    /// <param name="result"></param>
    private void f_Callback_Arena(object result)
    {
        CMsg_SC_Arena tServerData = (CMsg_SC_Arena)result;
        mIsOnRank = tServerData.isOnRank == 0 ? false : true;
        mRank = tServerData.uRank;
        mTimes = tServerData.uTimes;
        mHighstRank = tServerData.uiHighstRank;
        int tRankIdx = -99;
        //处理目标名次
        List<NBaseSCDT> tList = glo_Main.GetInstance().m_SC_Pool.m_ArenaRankAwardSC.f_GetAll();
        for (int i = 0; i < tList.Count; i++)
        {
            ArenaRankAwardDT tNode = (ArenaRankAwardDT)tList[i];
            if (mRank >= tNode.iMin && mRank <= tNode.iMax)
            {
                tRankIdx = i;
            }
        }
        if (tRankIdx > 0 && tRankIdx < tList.Count)
        {
            tRankIdx--;
            ArenaRankAwardDT tNode = (ArenaRankAwardDT)tList[tRankIdx];
            mTargetRank = tNode.iMax;
        }
        else if (tRankIdx == 0)
        {
            ArenaRankAwardDT tNode = (ArenaRankAwardDT)tList[tRankIdx];
            mTargetRank = tNode.iMax;

        }
        else
        {
            ArenaRankAwardDT tNode = (ArenaRankAwardDT)tList[tList.Count - 1];
            mTargetRank = tNode.iMax;
        }
    }

    /// <summary>
    /// 前2个int 前端没用到，后端使用，所以不存在节点类型
    /// </summary>
    private void Callback_SocketData_ArenaList(int iData1, int iData2, int iNum, ArrayList aData)
    {
        mArenaList.Clear();
        foreach (SockBaseDT tData in aData)
        {
            f_ArenaList_AddData(tData);
        }
        mArenaList.Sort(delegate (BasePoolDT<long> node1, BasePoolDT<long> node2)
        {
            ArenaPoolDT item1 = (ArenaPoolDT)node1;
            ArenaPoolDT item2 = (ArenaPoolDT)node2;
            return item1.m_iRank.CompareTo(item2.m_iRank);
        });
    }

    private void f_ArenaList_AddData(SockBaseDT Obj)
    {
        CMsg_RTC_ArenaListNode tServerData = (CMsg_RTC_ArenaListNode)Obj;
        BasePlayerPoolDT tPlayerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.rivalId);


        if (tServerData.rivalId == Data_Pool.m_UserData.m_iUserId)
        {
            LegionTool.f_SelfPlayerInfoDispose(ref tPlayerInfo);
            ArenaPoolDT poolDt = new ArenaPoolDT();
            poolDt.iId = tServerData.rivalId;
            poolDt.f_UpdateInfo(tServerData.uRank, tPlayerInfo);
            mArenaList.Add(poolDt);
        }
        else
        {
            if (tPlayerInfo != null)
            {
                ArenaPoolDT poolDt = new ArenaPoolDT();
                poolDt.iId = tServerData.rivalId;
                poolDt.f_UpdateInfo(tServerData.uRank, tPlayerInfo);
                mArenaList.Add(poolDt);
            }
            else
MessageBox.ASSERT("No corresponding player information found, Id:" + tServerData.rivalId);
        }
    }

    /// <summary>
    /// 处理竞技场挑战结果
    /// </summary>
    /// <param name="result"></param>
    private void f_Callback_ArenaRet(object result)
    {
        CMsg_SC_ArenaRet tServerData = (CMsg_SC_ArenaRet)result;
        m_ArenaRet = tServerData;
        Data_Pool.m_GuidancePool.m_OtherSave = true;
        mIsFinish = true;
        //设置突破相关的数据
        mBreakRankInfo.f_SetInfo(mRank, m_ArenaRet.rankBreakNum);
    }

    /// <summary>
    /// 处理竞技场扫荡结果
    /// </summary>
    /// <param name="result"></param>
    private void f_Callback_ArenaSweepRet(int iData1, int iData2, int iData3, int iNum, ArrayList aData)
    {
        if (BitTool.BitTest(iData1, 1))
        {
            //数据开始
            mHandilyChallengeRewardList = new List<SC_Award>();
            mHandilyChallengeRewardList.AddRange((SC_Award[])aData.ToArray(typeof(SC_Award)));
        }
        if (BitTool.BitTest(iData1, 2))
        {
            //追加数据
            mHandilyChallengeRewardList.AddRange((SC_Award[])aData.ToArray(typeof(SC_Award)));
        }
        if (BitTool.BitTest(iData1, 3))
        {
            //结束,如果前面追加了数据则不再追加
            if (!BitTool.BitTest(iData1, 1))
            {
                mHandilyChallengeRewardList.AddRange((SC_Award[])aData.ToArray(typeof(SC_Award)));
            }

            //打开快捷扫荡界面
            if (null == arenaSweepCallbackDt.m_ccCallbackSuc)
                return;
            HandilyChallengeResultParam handilyChallengeResultParam = new HandilyChallengeResultParam();
            handilyChallengeResultParam.RewardList = mHandilyChallengeRewardList;
            handilyChallengeResultParam.useItemNum = iData2;
            handilyChallengeResultParam.ChallengeTimes = iData3;
            arenaSweepCallbackDt.m_ccCallbackSuc(handilyChallengeResultParam);
        }
    }

    /// <summary>
    /// 处理选择奖励结果
    /// </summary>
    /// <param name="result"></param>
    private void f_Callback_ArenaChooseAward(object result)
    {
        CMsg_SC_ArenaChooseAward tServerData = (CMsg_SC_ArenaChooseAward)result;
        m_sArenaChooseAwardRet = tServerData;
        if (mChooseAwardCallback != null)
        {
            mChooseAwardCallback(result);
        }
    }

    private const int RANK_LIST_MAX_NUM = 30;
    private List<BasePoolDT<long>> mRankList;
    //请求竞技场排名列表 时间间隔
    private const int RANK_LIST_INIT_TIME = 100;
    //当前请求竞技场排名列表时间
    private int mCurRankListTime = 0;

    private void f_Callback_ArenarRankList(object result)
    {
        CMsg_RTC_ArenaRankList tServerData = (CMsg_RTC_ArenaRankList)result;
        if (mRankList == null)
        {
            mRankList = new List<BasePoolDT<long>>();
            for (int i = 0; i < RANK_LIST_MAX_NUM; i++)
            {
                if (i >= tServerData.rankUserId.Length)
                {
MessageBox.ASSERT("The total number of people on the leaderboard is less than RANK_LIST_MAX_NUM（30）");
                    break;
                }
                BasePlayerPoolDT dt = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.rankUserId[i]);
                if (dt != null)
                {
                    ArenaPoolDT poolDt = new ArenaPoolDT();
                    poolDt.iId = tServerData.rankUserId[i];
                    poolDt.f_UpdateInfo(i + 1, dt);
                    mRankList.Add(poolDt);
                }
                else
MessageBox.ASSERT(string.Format("No player data on leaderboard ,Rank:{0},userId:{1}", i + 1, tServerData.rankUserId[i]));
            }
        }
        else
        {
            for (int i = 0; i < RANK_LIST_MAX_NUM; i++)
            {
                if (i >= tServerData.rankUserId.Length)
                {
MessageBox.ASSERT("The total number of people on the leaderboard is less than RANK_LIST_MAX_NUM（30）");
                    break;
                }
                BasePlayerPoolDT dt = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.rankUserId[i]);
                if (dt != null)
                {
                    ArenaPoolDT poolDt = null;
                    if (i >= mRankList.Count)
                    {
                        poolDt = new ArenaPoolDT();
                        mRankList.Add(poolDt);
                    }
                    else
                    {
                        poolDt = (ArenaPoolDT)mRankList[i];
                    }
                    poolDt.iId = tServerData.rankUserId[i];
                    poolDt.f_UpdateInfo(i + 1, dt);
                }
                else
MessageBox.ASSERT(string.Format("No player data on leaderboard ,Rank:{0},userId:{1}", i + 1, tServerData.rankUserId[i]));
            }
        }
    }

    #region 发送协议
    /// <summary>
    /// 请求竞技场玩家列表
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_ArenaList(SocketCallbackDT socketCallbackDt)
    {
        //游戏服请求，关系服返回操作结果
        ChatSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ArenaList, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ArenaList, bBuf);
    }

    /// <summary>
    /// 发送挑战玩家协议
    /// </summary>
    /// <param name="rank"></param>
    /// <param name="socketCallbackDt"></param>
    public void f_ArenaChallenge(int rank, SocketCallbackDT socketCallbackDt, int _enemyLevel, int _enemySex, byte nIsPlotFight = 0)
    {
        DungeonTollgatePoolDT tPoolDt = Data_Pool.m_DungeonPool.f_GetTollgatePoolDTByType(EM_Fight_Enum.eFight_Arena);
        StaticValue.m_CurBattleConfig.f_UpdateInfo((EM_Fight_Enum)tPoolDt.mChapterType, tPoolDt.mChapterId, tPoolDt.mTollgateId, tPoolDt.mTollgateTemplate.iSceneId);

        mIsFinish = false;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ArenaChallenge, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(rank);
        tCreateSocketBuf.f_Add(nIsPlotFight);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();

        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ArenaChallenge, bBuf);
        enemyLevel = _enemyLevel;
        enemySex = _enemySex;
    }

    /// <summary>
    /// 请求竞技场扫荡 无须战斗流程，直接返回战斗结果和奖励
    /// </summary>
    /// <param name="rank"></param>
    /// <param name="socketCallbackDt"></param>
    public void f_ArenaSweep(uint rank, byte challengeTimes, byte isAutoCostVigor, SocketCallbackDT socketCallbackDt)
    {
        int tLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        int tExp = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        StaticValue.m_sLvInfo.f_UpdateInfo(tLv, tExp);

        arenaSweepCallbackDt = socketCallbackDt;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ArenaSweep, null, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(rank);
        tCreateSocketBuf.f_Add(challengeTimes);
        tCreateSocketBuf.f_Add(isAutoCostVigor);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ArenaSweep, bBuf);
    }

    /// <summary>
    /// 请求选择竞技场抽卡奖励
    /// </summary>
    /// <param name="idx">[0,2]</param>
    public void f_ArenaChooseAward(byte idx, ccCallback chooseAwardCallback)
    {
        mChooseAwardCallback = chooseAwardCallback;
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(idx);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ArenaChooseAward, bBuf);
    }
    private ccCallback mChooseAwardCallback;

    /// <summary>
    /// 请求排行榜数据
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_ArenaRankList(SocketCallbackDT socketCallbackDt)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ArenaRankList, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ArenaRankList, bBuf);
    }

    #endregion

    #region 对外接口

    /// <summary>
    /// 检查挑战排名限制(前10名必须要在前20名才能挑战) true限制不能挑战 false没有限制
    /// </summary>
    /// <returns></returns>
    public bool f_CheckChallengeRankLimit(int rank)
    {
        if (rank > 10)
            return false;
        if (mRank <= 20)
            return false;
        return true;
    }

    /// <summary>
    /// 获取玩家列表
    /// </summary>
    /// <returns></returns>
    public List<BasePoolDT<long>> f_GetArenaList()
    {
        return mArenaList;
    }

    /// <summary>
    /// 获取突破排名奖励元宝
    /// </summary>
    /// <param name="curRank"></param>
    /// <param name="breakRank"></param>
    /// <returns></returns>
    public int f_GetRankBreakSycee(int curRank, int breakRank)
    {
        int result = 0;
        if (breakRank == 0)
            return result;
        int endRank = curRank + breakRank;
        //if (endRank < m_ArenaRet.oldRank)
        //return result;
        List<NBaseSCDT> tList = glo_Main.GetInstance().m_SC_Pool.m_ArenaMatchSC.f_GetAll();
        int startIdx = -99;
        int endIdx = -99;
        for (int i = 0; i < tList.Count; i++)
        {
            ArenaMatchDT tNode = (ArenaMatchDT)tList[i];
            if (startIdx == -99 && curRank >= tNode.iMin && curRank <= tNode.iMax)
            {
                startIdx = i;
            }
            if (endIdx == -99 && endRank >= tNode.iMin && endRank <= tNode.iMax)
            {
                endIdx = i;
            }
            if (startIdx == -99)
                continue;
            else if (endIdx != -99 && i > endIdx)
                continue;
            if (startIdx == endIdx)
            {
                result += breakRank * tNode.iBreakSycee;
            }
            else
            {
                if (endIdx == i)
                {
                    result += (endRank - tNode.iMin) * tNode.iBreakSycee;
                }
                else if (startIdx == i)
                {
                    result += (tNode.iMax - curRank + 1) * tNode.iBreakSycee;
                }
                else
                {
                    result += (tNode.iMax - tNode.iMin + 1) * tNode.iBreakSycee;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 获取排行榜列表
    /// </summary>
    /// <returns></returns>
    public List<BasePoolDT<long>> f_GetRankList()
    {
        return mRankList;
    }

    private ccCallback callbackExcuteAfterInitRankList;
    public void f_ExecuteAfterInitRankList(ccCallback callbackHandle)
    {
        callbackExcuteAfterInitRankList = callbackHandle;
        int tNowTime = GameSocket.GetInstance().f_GetServerTime();
        if (tNowTime - mCurRankListTime > RANK_LIST_INIT_TIME)
        {
            mCurRankListTime = tNowTime;
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_ArenaRankListSuc;
            socketCallbackDt.m_ccCallbackFail = f_ArenaRankListFail;
            f_ArenaRankList(socketCallbackDt);
        }
        else
        {
            if (callbackExcuteAfterInitRankList != null)
                callbackExcuteAfterInitRankList(eMsgOperateResult.OR_Succeed);
        }
    }

    private void f_ArenaRankListSuc(object result)
    {
        if (callbackExcuteAfterInitRankList != null)
            callbackExcuteAfterInitRankList(result);
    }

    private void f_ArenaRankListFail(object result)
    {
        if (callbackExcuteAfterInitRankList != null)
            callbackExcuteAfterInitRankList(result);
    }

    /// <summary>
    /// 获取我的排行榜数据
    /// </summary>
    /// <returns></returns>
    public BasePoolDT<long> f_GetMyRankData()
    {
        for (int i = 0; i < mRankList.Count; i++)
        {
            ArenaPoolDT dt = (ArenaPoolDT)mRankList[i];
            if (dt.m_PlayerInfo.iId == Data_Pool.m_UserData.m_iUserId)
                return dt;
        }

        //未上榜（排名30外的就不上榜）的数据取不到，构造一个
        ArenaPoolDT myDt = new ArenaPoolDT();
        BasePlayerPoolDT basePlay = new BasePlayerPoolDT();
        LegionTool.f_SelfPlayerInfoDispose(ref basePlay);
        int iRank = m_iRank > 50 ? 0 : m_iRank;
        myDt.f_UpdateInfo(iRank, basePlay);
        return myDt;
    }
    #endregion

    #region 竞技场限制

    public bool f_CheckArenaLvLimit(int curLv)
    {
        return curLv < UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel);
    }

    #endregion

}

public class ArenaBreakRankInfo
{
    public ArenaBreakRankInfo()
    {
        mShowInfo = false;
        mCurRank = 0;
        mRankBreakNum = 0;
        mBreakSyceeNum = 0;
    }

    private bool mShowInfo;
    public bool m_bShowInfo
    {
        get
        {
            return mShowInfo;
        }
    }

    /// <summary>
    /// 当前排名
    /// </summary>
    private int mCurRank;

    /// <summary>
    /// 突破数目
    /// </summary>
    private int mRankBreakNum;

    /// <summary>
    /// 奖励元宝数量
    /// </summary>
    private int mBreakSyceeNum;

    //设置相关信息
    public void f_SetInfo(int curRank, int rankBreakNum)
    {
        mCurRank = curRank;
        mRankBreakNum = rankBreakNum;
        mBreakSyceeNum = Data_Pool.m_ArenaPool.f_GetRankBreakSycee(mCurRank, mRankBreakNum);
        if (mRankBreakNum > 0 && mBreakSyceeNum > 0)
            mShowInfo = true;
    }

    //获取相关信息
    public void f_GetInfo(ref int curRank, ref int rankBreakNum, ref int breakSyceeNum)
    {
        curRank = mCurRank;
        rankBreakNum = mRankBreakNum;
        breakSyceeNum = mBreakSyceeNum;
        mShowInfo = false;
    }

}
