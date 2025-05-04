using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrossArenaPool : BasePool
{
    private bool m_IsShopInit = false;
    private List<BasePoolDT<long>> m_CurLvShopList;

    public List<BasePoolDT<long>> ShopList
    {
        get
        {
            return m_CurLvShopList;
        }
    }
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
    private bool m_IsFinish;
    public bool IsFinish
    {
        get
        {
            return m_IsFinish;
        }
    }

    private CMsg_SC_CrossArenaResult m_BattleResult;
    public CMsg_SC_CrossArenaResult BattleResult
    {
        get
        {
            return m_BattleResult;
        }
    }

    private CMsg_SC_CrossArenaInfo m_CrossArenaInfo;
    public CMsg_SC_CrossArenaInfo CrossArenaInfo
    {
        get
        {
            return m_CrossArenaInfo;
        }
    }

    private CMsg_ArenaCrossInfo m_EnemyInfo;
    public CMsg_ArenaCrossInfo EnemyInfo
    {
        get
        {
            return m_EnemyInfo;
        }
    }

    private ArrayList mArenaList;
    private List<BasePoolDT<long>> mArenaRankList;
    private List<BasePoolDT<long>> mArenaRecordList;

    public List<BasePoolDT<long>> f_GetRecordList()
    {
        return mArenaRecordList;
    }
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

    public CrossArenaPool() : base("CrossArenaPoolDT")
    {

    }

    protected override void f_Init()
    {
        mRank = 0;
        mTargetRank = 0;
        mTimes = 0;
        mIsOnRank = false;
        mArenaList = new ArrayList();
        m_CurLvShopList = new List<BasePoolDT<long>>();
        List<NBaseSCDT> tInitList = glo_Main.GetInstance().m_SC_Pool.m_CrossArenaShopSC.f_GetAll();
        for (int i = 0; i < tInitList.Count; i++)
        {
            CrossArenaShopDT tInitNode = (CrossArenaShopDT)tInitList[i];
            f_Save(new CrossArenaShopPoolDT(tInitNode));
        }
        f_SortData1();
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.PlayerLvUpdate, f_UpdateDataByLv);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, f_UpdateDataByNextDay);
    }

    private void f_UpdateDataByLv(object value)
    {
        int lv = (int)value;
        m_CurLvShopList.Clear();
        List<BasePoolDT<long>> tSourceList = f_GetAll();
        for (int i = 0; i < tSourceList.Count; i++)
        {
            CrossArenaShopPoolDT tNode = (CrossArenaShopPoolDT)tSourceList[i];
            if (tNode.Template.iShowLv <= lv)
            {
                m_CurLvShopList.Add(tNode);
            }
        }
    }

    private void f_UpdateDataByNextDay(object value)
    {
        f_Reset();
        List<BasePoolDT<long>> tResetList = f_GetAll();
        for (int i = 0; i < tResetList.Count; i++)
        {
            CrossArenaShopPoolDT tNode = (CrossArenaShopPoolDT)tResetList[i];
            tNode.f_Reset();
        }
    }
    private void f_Reset()
    {
        mTimes = 0;
    }

    protected override void RegSocketMessage()
    {
        //玩家数据 游戏服返回
        //CMsg_SC_Arena tSC_Arena = new CMsg_SC_Arena();
        //GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossArena, tSC_Arena, f_Callback_Arena);

        //玩家列表数据 关系服返回
        CMsg_ArenaCrossInfo tSC_ArenaList = new CMsg_ArenaCrossInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_CrossArenaList, tSC_ArenaList, Callback_SocketData_ArenaList);


        CMsg_SC_CrossArenaResult tSC_CrossArenaResult = new CMsg_SC_CrossArenaResult();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossArenaResult, tSC_CrossArenaResult, f_SC_CrossBattleResultHandle);

        CMsg_SC_CrossArenaInfo tSC_CrossArenaInfo = new CMsg_SC_CrossArenaInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossArenaInfo, tSC_CrossArenaInfo, f_SC_CrossArenaInfoHandle);

        CMsg_ArenaCrossInfo tSC_ArenaRankList = new CMsg_ArenaCrossInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_CrossArenaRankList, tSC_ArenaRankList, Callback_SocketData_ArenaRankList);

        CMsg_SC_CrossArenaRecordList tSC_CrossArenaRecordList = new CMsg_SC_CrossArenaRecordList();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_CrossArenaRecordList, tSC_CrossArenaRecordList, Callback_SocketData_ArenaRecordList);

        CMsg_SC_GoodInfoNode tSC_GoodNode = new CMsg_SC_GoodInfoNode();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_CrossArenaShopInfo, tSC_GoodNode, f_SC_ShopInfoUpdateHandle);

        ////挑战结果
        //CMsg_SC_ArenaRet tSC_ArenaRet = new CMsg_SC_ArenaRet();
        //GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossArenaRet, tSC_ArenaRet, f_Callback_ArenaRet);

        ////扫荡结果处理
        //SC_Award tSC_ArenaSweepRet = new SC_Award();
        //GameSocket.GetInstance().f_RegMessage_Int3((int)SocketCommand.SC_CrossArenaSweepRet, tSC_ArenaSweepRet, f_Callback_ArenaSweepRet);

        ////选择奖励结果
        //CMsg_SC_ArenaChooseAward tSC_ArenaChooseAward = new CMsg_SC_ArenaChooseAward();
        //GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossArenaChooseAward, tSC_ArenaChooseAward, f_Callback_ArenaChooseAward);

        ////竞技场
        //CMsg_RTC_ArenaRankList tRC_ArenaRankList = new CMsg_RTC_ArenaRankList();
        //ChatSocket.GetInstance().f_RegMessage((int)ChatSocketCommand.RC_CrossArenaRankList, tRC_ArenaRankList, f_Callback_ArenarRankList);

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
    /// 前2个int 前端没用到，后端使用，所以不存在节点类型
    /// </summary>
    private void Callback_SocketData_ArenaList(int iData1, int iData2, int iNum, ArrayList aData)
    {
        mArenaList = aData;
        for (int i = 0; i < mArenaList.Count; i++)
        {
            CMsg_ArenaCrossInfo arenaPoolDT = (CMsg_ArenaCrossInfo)mArenaList[i];
            if (arenaPoolDT.userId == Data_Pool.m_UserData.m_iUserId)
            {
                mRank = arenaPoolDT.uRank;
            }
        }

        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_CrossArenaPool, mArenaList.Count);
    }

    private void Callback_SocketData_ArenaRankList(int iData1, int iData2, int iNum, ArrayList aData)
    {
        mRank = iData1;
        mArenaRankList = new List<BasePoolDT<long>>();
        for (int i = 0; i < iNum; i++)
        {
            CMsg_ArenaCrossInfo cMsg_ArenaCrossInfo = (CMsg_ArenaCrossInfo)aData[i];
            ArenaPoolDT poolDt = new ArenaPoolDT();
            poolDt.iId = cMsg_ArenaCrossInfo.userId;
            poolDt.f_UpdateArenaCrossInfo(cMsg_ArenaCrossInfo);
            mArenaRankList.Add(poolDt);
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_CrossArenaRankPool, mArenaRankList.Count);      
    }


    private void Callback_SocketData_ArenaRecordList(int iData1, int iData2, int iNum, ArrayList aData)
    {
        mArenaRecordList = new List<BasePoolDT<long>>();
        for (int i = 0; i < iNum; i++)
        {
            CMsg_SC_CrossArenaRecordList cMsg_ArenaCrossRecord= (CMsg_SC_CrossArenaRecordList)aData[i];
            ArenaCrossRecordPoolDT poolDt = new ArenaCrossRecordPoolDT();
            poolDt.iId = cMsg_ArenaCrossRecord.id;
            poolDt.f_UpdateArenaCrossRecord(cMsg_ArenaCrossRecord);
            mArenaRecordList.Add(poolDt);
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_CrossArenaRecordPool, mArenaRecordList.Count);
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
                MessageBox.ASSERT("Không tìm thấy thông tin người chơi tương ứng, Id:" + tServerData.rivalId);
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
    //当前请求竞技场排名列表时间
    private int mCurRankListTime = 0;
    private const int RANK_LIST_INIT_TIME = 100;
    #region 发送协议
    /// <summary>
    /// 请求竞技场玩家列表
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_ArenaList()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossArenaList, bBuf);
    }

    /// <summary>
    /// 发送挑战玩家协议
    /// </summary>
    /// <param name="rank"></param>
    /// <param name="socketCallbackDt"></param>
    private SocketCallbackDT m_MatchSocketCallbackDt;
    public void f_ArenaChallenge(int rank, SocketCallbackDT socketCallbackDt, CMsg_ArenaCrossInfo enemyInfo)
    {
        //DungeonTollgatePoolDT tPoolDt = Data_Pool.m_DungeonPool.f_GetTollgatePoolDTByType(EM_Fight_Enum.eFight_ArenaCross);
        //StaticValue.m_CurBattleConfig.f_UpdateInfo((EM_Fight_Enum)tPoolDt.mChapterType, tPoolDt.mChapterId, tPoolDt.mTollgateId, tPoolDt.mTollgateTemplate.iSceneId);

        //mIsFinish = false;
        //GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossArenaChallenge, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        //CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        //tCreateSocketBuf.f_Add(rank);
        //tCreateSocketBuf.f_Add(nIsPlotFight);
        //byte[] bBuf = tCreateSocketBuf.f_GetBuf();

        //GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossArenaChallenge, bBuf);
        //enemyLevel = _enemyLevel;
        //enemySex = _enemySex;

        m_EnemyInfo = enemyInfo;
        m_IsFinish = false;
        m_MatchSocketCallbackDt = socketCallbackDt;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossArenaChallenge, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(rank);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossArenaChallenge, bBuf);
    }

    public void f_CrossArenaRankList(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossArenaRankList, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossArenaRankList, bBuf);
    }

    private void f_SC_CrossArenaInfoHandle(object value)
    {
        CMsg_SC_CrossArenaInfo crossArenaInfo = (CMsg_SC_CrossArenaInfo)value;
        m_CrossArenaInfo = crossArenaInfo;
    }

    private void f_SC_CrossBattleResultHandle(object value)
    {
        CMsg_SC_CrossArenaResult serverInfo = (CMsg_SC_CrossArenaResult)value;
        m_IsFinish = true;
        m_BattleResult = serverInfo;
        if (m_MatchSocketCallbackDt != null &&
            m_MatchSocketCallbackDt.m_ccCallbackSuc != null &&
            m_MatchSocketCallbackDt.m_ccCallbackFail != null)
        {
            if (serverInfo.iOperateResult == (int)eMsgOperateResult.OR_Succeed)
            {
                //更新战斗数据
                StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_ArenaCross, 0, 0, 0);
                m_MatchSocketCallbackDt.m_ccCallbackSuc(serverInfo.iOperateResult);
            }
            else
            {
                m_MatchSocketCallbackDt.m_ccCallbackFail(serverInfo.iOperateResult);
            }
            m_MatchSocketCallbackDt = null;
        }
    }

    /// <summary>
    /// 请求竞技场扫荡 无须战斗流程，直接返回战斗结果和奖励
    /// </summary>
    /// <param name="rank"></param>
    /// <param name="socketCallbackDt"></param>
    public void f_ArenaSweep(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossArenaSweep, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossArenaSweep, bBuf);
    }

    public void f_ShopInit(SocketCallbackDT socketCallbackDt)
    {
        if (m_IsShopInit)
        {
            socketCallbackDt.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        m_IsShopInit = true;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossArenaShop, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossArenaShop, bBuf);
    }

    public void f_Buy(int shopItemId, int buyNum, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossArenaBuy, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(shopItemId);
        tCreateSocketBuf.f_Add(buyNum);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossArenaBuy, bBuf);
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
    public void f_ArenaRankList()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossArenaRankList, bBuf);
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
    public ArrayList f_GetArenaList()
    {
        return mArenaList;
    }

    public List<BasePoolDT<long>> f_GetRankList()
    {
        return mArenaRankList;
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

    public void f_ExecuteAfterInitRankList()
    {
        int tNowTime = GameSocket.GetInstance().f_GetServerTime();
        if (tNowTime - mCurRankListTime > RANK_LIST_INIT_TIME)
        {
            mCurRankListTime = tNowTime;
            f_ArenaRankList();
        }
        else
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_CrossArenaRankPool, mArenaRankList.Count);
        }
    }

    public void f_GetCrossArenaRecordList()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        //tCreateSocketBuf.f_Add(0);//listCount
        //tCreateSocketBuf.f_Add(0);//idMin
        //tCreateSocketBuf.f_Add(0);///idMax
        //tCreateSocketBuf.f_Add(0);//itype
        //tCreateSocketBuf.f_Add(3);//itypeGet
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();

        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossArenaRecordList, bBuf);
    }

    /// <summary>
    /// 获取我的排行榜数据
    /// </summary>
    /// <returns></returns>
    #endregion

    #region 竞技场限制

    public bool f_CheckArenaLvLimit(int curLv)
    {
        return curLv < UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel);
    }

    #endregion

    public void f_SC_ShopInfoUpdateHandle(int value1, int value2, int num, System.Collections.ArrayList arrayList)
    {
        for (int i = 0; i < arrayList.Count; i++)
        {
            CMsg_SC_GoodInfoNode serverData = (CMsg_SC_GoodInfoNode)arrayList[i];
            CrossArenaShopPoolDT shopItem = (CrossArenaShopPoolDT)f_GetForId(serverData.goodsId);
            if (shopItem != null)
            {
                shopItem.f_UpdateInfo(serverData.times);
            }
            else
            {
                MessageBox.ASSERT(string.Format("Máy chủ gửi dữ liệu vật phẩm không tồn tại,NodeType:{0}; GoodId:{1}", value1, serverData.goodsId));
            }
        }
    }

}

