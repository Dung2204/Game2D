using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 过关斩将Pool（三国无双）
/// </summary>
public class RunningManPool : BasePool
{
    #region 通关描述
private readonly string[] m_PassTypeDescs = new string[4] {"Quét sạch tất cả kẻ thù {0}",
                                                               "Thắng trong {0} lượt",
                                                               "Tổng % hp của đội > {0}%",
                                                               "Số tướng tử vong tối đa {0}" };
    /// <summary>
    /// 根据类型返回通关描述
    /// </summary>
    /// <param name="type">EM_RM_PassType</param>
    /// <returns></returns>
    public string f_GetPassTypeDesc(int type)
    {
        if (type > 0 && type <= m_PassTypeDescs.Length)
            return m_PassTypeDescs[type - 1];
        else
        {
MessageBox.ASSERT("Invalid data type description " + type);
            return "ErrorCode:{0}";
        }
    }
    #endregion


    #region 结算暴击描述

    private readonly int[] m_CritRate = new int[5] {100,
                                                      120,
                                                      140,
                                                      160,
                                                      200};

    private readonly string[] m_CritDescs = new string[5] {"",
                                                           "[F5BF3D]（Normal）[-]",
                                                           "[F5BF3D]（Small）[-]",
                                                           "[F5BF3D]（Big）[-]",
                                                           "[F5BF3D]（Lucky）[-]"};
    /// <summary>
    /// 获取暴击描述
    /// </summary>
    /// <param name="rate">暴击倍数</param>
    /// <returns></returns>
    public string f_GetCritDesc(int rate)
    {
        int idx = 0;
        for (int i = 0; i < m_CritRate.Length; i++)
        {
            if (m_CritRate[i] == rate)
                idx = i;
        }
        return m_CritDescs[idx];
    }

    #endregion

    public RunningManPool() : base("RunningManPoolDT")
    {

    }

    protected override void f_Init()
    {
        buffPropertyList = new List<RunningManBuffProperty>();
        eliteList = new List<BasePoolDT<long>>();
        List<NBaseSCDT> tInitList = glo_Main.GetInstance().m_SC_Pool.m_RunningManChapterSC.f_GetAll();
        for (int i = 0; i < tInitList.Count; i++)
        {
            RunningManChapterDT tItem = (RunningManChapterDT)tInitList[i];
            RunningManPoolDT tPoolDt = new RunningManPoolDT(tItem);
            f_Save(tPoolDt);
            chapterMaxId = UnityEngine.Mathf.Max(chapterMaxId, tItem.iId);
        }
        List<NBaseSCDT> tEliteInitList = glo_Main.GetInstance().m_SC_Pool.m_RunningManEliteSC.f_GetAll();
        for (int i = 0; i < tEliteInitList.Count; i++)
        {
            RunningManEliteDT tPreItem = (RunningManEliteDT)tEliteInitList[UnityEngine.Mathf.Max(i - 1, 0)];
            RunningManEliteDT tItem = (RunningManEliteDT)tEliteInitList[i];
            RunningManElitePoolDT tPoolDt = new RunningManElitePoolDT(tPreItem, tItem);
            eliteList.Add(tPoolDt);
            eliteTollgateMax = UnityEngine.Mathf.Max(eliteTollgateMax, tItem.iId);
        }
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, f_ProcessNextDay);
    }

    /// <summary>
    /// 跨天处理
    /// </summary>
    private void f_ProcessNextDay(object value)
    {
        //跨天重新请求
        initForServer = false;
        eliteTimes = 0;
        eliteBuyTimes = 0;
        f_Reset();
        f_ExecuteAfterRunningManInit(f_InitByNextDay);
    }

    private void f_InitByNextDay(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //跨天UI事件  通知关心此消息的UI更新
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_THENEXTDAY_UIPROCESS, EM_NextDaySource.RunningManPool);
        }
        else
        {
MessageBox.ASSERT("Initialization failed：code:" + result);
        }
    }

    /// <summary>
    /// 过关斩将重置
    /// </summary>
    private void f_Reset()
    {
        buffPropertyList.Clear();
        curPassChapter = 0;
        isLose = false;
        List<BasePoolDT<long>> tResetList = f_GetAll();
        for (int i = 0; i < tResetList.Count; i++)
        {
            RunningManPoolDT tPoolDt = (RunningManPoolDT)tResetList[i];
            tPoolDt.f_Reset();
        }
    }

    protected override void RegSocketMessage()
    {
        //添加过关斩将数据
        CMsg_SC_RunningMan tsc_RunningMan = new CMsg_SC_RunningMan();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_RunningMan, tsc_RunningMan, f_SC_RunningManHandle);

        //过关斩将章节数据
        CMsg_SC_ChapInfoNode tsc_ChapInfoNode = new CMsg_SC_ChapInfoNode();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_RunningManChap, tsc_ChapInfoNode, f_SC_RunningManChapHandle);

        //过关斩将结算
        CMsg_SC_RunningManRet tsc_RunningManRet = new CMsg_SC_RunningManRet();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_RunningManRet, tsc_RunningManRet, f_SC_RunningManRetHandle);

        //历史最高星数排行榜
        CMsg_SC_RunningManRank tsc_RunningManRank = new CMsg_SC_RunningManRank();
        ChatSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_RunningManRank, tsc_RunningManRank, f_SC_RunningManRankHandle);

        //历史最该星数排行榜自己
        CMsg_SC_RunningManRankMySelf tsc_RunningManRankMySelf = new CMsg_SC_RunningManRankMySelf();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_RunningManRankMySelf, tsc_RunningManRankMySelf, f_SC_RunningManRankHandleSelf);

        //精英挑战
        CMsg_SC_RunningManEliteRet tsc_RunningManEliteRet = new CMsg_SC_RunningManEliteRet();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_RunningManEliteRet, tsc_RunningManEliteRet, f_SC_RunningManEliteRetHandle);

        //扫荡
        CMsg_SC_RunningManSweepRetNode tsc_RunningManSweepRetNode = new CMsg_SC_RunningManSweepRetNode();
        GameSocket.GetInstance().f_RegMessage_Int2((int)SocketCommand.CS_RunningManSweepRet, tsc_RunningManSweepRetNode, f_SC_RunningManSweepRetHandle);
    }





    #region 接受协议
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }

    private void f_SC_RunningManHandle(object value)
    {
        CMsg_SC_RunningMan serverData = (CMsg_SC_RunningMan)value;
        historyChapter = serverData.hisChap;
        history3StarsChapter = serverData.his3StarsChap;
        resetTimes = serverData.resetTimes;
        //星星相关
        historyStarNum = serverData.hisStars;
        curStarNum = serverData.uStars;
        usedStarNum = serverData.uesdStars;
        //精英关卡相关
        if (eliteFirstProg != serverData.eliteFirstProg)
        {
            for (int i = 0; i < eliteList.Count; i++)
            {
                RunningManElitePoolDT poolDt = (RunningManElitePoolDT)eliteList[i];
                poolDt.f_UpdatePassInfo(serverData.eliteFirstProg);
            }
        }
        eliteFirstProg = serverData.eliteFirstProg;
        eliteTimes = serverData.eliteTimes;
        eliteBuyTimes = serverData.eliteBuyTimes;
        //过关斩将星数信息 （任务）
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
            new int[] { (int)EM_AchievementTaskCondition.eAchv_RunningManStars, historyStarNum });
        if (historyChapter > 0 && UITool.f_GetIsOpensystem(EM_NeedLevel.RunningManLvel))
        {
            int eliteLeftTimes = m_iEliteBuyTimes + GameParamConst.RMEliteTimesLimit - m_iEliteTimes;
            if (eliteLeftTimes > 0)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RunningManEliteLeftTimes);
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.RunningManEliteLeftTimes);
            }
            else
            {
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RunningManEliteLeftTimes);
            }
        }
    }

    private void f_SC_RunningManChapHandle(int value1, int value2, int num, ArrayList aData)
    {
        for (int i = 0; i < aData.Count; i++)
        {
            CMsg_SC_ChapInfoNode serverData = (CMsg_SC_ChapInfoNode)aData[i];
            RunningManPoolDT tPoolDt = (RunningManPoolDT)f_GetForId((long)serverData.uChapId);
            if (tPoolDt == null)
MessageBox.ASSERT("The server sent the chapter data does not exist ,Chapter Id:" + serverData.uChapId);
            if (serverData.uBuffIdx > 0 && tPoolDt.m_iBuffIdx == 0)
            {
                f_AddBuff(serverData.uBuffIdx, serverData.uBuff[serverData.uBuffIdx - 1]);
            }
            tPoolDt.f_UpdateInfo(serverData.uBoxTimes, serverData.uBuffIdx, serverData.uBuff, serverData.iRet);
            if (curPassChapter < serverData.uChapId && serverData.iRet[2] > 0)
            {
                curPassChapter = serverData.uChapId;
            }
            for (int j = 0; j < serverData.iRet.Length; j++)
            {
                if (serverData.iRet[j] < 0)
                    isLose = true;
            }
        }
    }

    /// <summary>
    /// 结算界面
    /// </summary>
    /// <param name="value"></param>
    private void f_SC_RunningManRetHandle(object value)
    {
        CMsg_SC_RunningManRet result = (CMsg_SC_RunningManRet)value;
        Data_Pool.m_GuidancePool.m_OtherSave = true;
        challengeFinish = true;
        challengeFinishRet = result;
    }

    private void f_SC_RunningManEliteRetHandle(object value)
    {
        CMsg_SC_RunningManEliteRet result = (CMsg_SC_RunningManEliteRet)value;
        eliteChallengeFinish = true;
        eliteChallengeFinishRet = result;
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    /// <param name="value"></param>
    private void f_SC_RunningManRankHandle(object value)
    {
        CMsg_SC_RunningManRank serverData = (CMsg_SC_RunningManRank)value;
        for (int i = 0; i < serverData.info.Length; i++)
        {
            CMsg_RunningManRankNode tNode = serverData.info[i];
            if (tNode.userId != 0)
            {
                
                int rankIndex = rankList.FindIndex((BasePoolDT<long> item) => { return item.iId == tNode.userId; });
                if (rankIndex >= 0)
                {
                    RunningManRankPoolDT tPoolDt = rankList[rankIndex] as RunningManRankPoolDT;
                    if (null == tPoolDt) continue;
                    tPoolDt.UpdateRankAndStar(serverData.page * RANK_LIST_NUM_PRE_PAGE + i + 1, tNode.hisStars);
                }
                else
                {
                    RunningManRankPoolDT tPoolDt = new RunningManRankPoolDT(serverData.page * RANK_LIST_NUM_PRE_PAGE + i + 1, tNode.userId, tNode.hisStars);
                    rankList.Add(tPoolDt);
                }
                
            }
        }
    }

    /// <summary>
    /// 排行榜（自己）
    /// </summary>
    /// <param name="value"></param>
    private void f_SC_RunningManRankHandleSelf(object value)
    {
        CMsg_SC_RunningManRankMySelf serverData = (CMsg_SC_RunningManRankMySelf)value;
        m_iMyHistoryStarRank = serverData.myRank;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value1">起始章节 单前的通关条件</param>
    /// <param name="value2">单前章节的通关关卡 [0,3]</param>
    /// <param name="num"></param>
    /// <param name="dataArr"></param>
    private void f_SC_RunningManSweepRetHandle(int value1, int value2, int num, ArrayList dataArr)
    {
        sweepChapBoxList.Clear();
        sweepTollgateList.Clear();
        int tChapId = value1;
        int tt = GameParamConst.RMTollgateNumPreChap - value2;
        sweepTotalItem = new RunningManSweepResult(EM_RunningManSweepResultType.Total);
        for (int i = 0; i < dataArr.Count; i++)
        {
            CMsg_SC_RunningManSweepRetNode tNode = (CMsg_SC_RunningManSweepRetNode)dataArr[i];
            if (i < tt)
            {
                int tIdx = value2 + i;
                f_AddSweepRetTollgateItem(tChapId, tIdx, tNode.uAwardMoneyRate, tNode.uAwardPrestRate);
            }
            else
            {
                int tIdx = (i - tt) % GameParamConst.RMTollgateNumPreChap;
                if (tIdx == 0)
                {
                    f_AddSweepRetChapBoxItem(tChapId);
                    tChapId++;
                }
                f_AddSweepRetTollgateItem(tChapId, tIdx, tNode.uAwardMoneyRate, tNode.uAwardPrestRate);
            }
        }
        f_AddSweepRetChapBoxItem(tChapId);
        isSweeping = false;
    }

    private void f_AddSweepRetChapBoxItem(int tChapId)
    {
        RunningManPoolDT tPoolDt = (RunningManPoolDT)f_GetForId(tChapId);
        int tStarNum = 0;
        for (int j = 0; j < tPoolDt.m_TollgatePoolDTs.Length; j++)
        {
            tStarNum += tPoolDt.m_TollgatePoolDTs[j].m_iResult;
        }
        RunningManSweepResult tSweepChapBoxItem = new RunningManSweepResult(EM_RunningManSweepResultType.ChapBox);
        if (tStarNum >= 9)
            tSweepChapBoxItem.f_UpdateChapBoxData(tPoolDt.m_ChapterTemplate.iBox9);
        else if (tStarNum >= 6)
            tSweepChapBoxItem.f_UpdateChapBoxData(tPoolDt.m_ChapterTemplate.iBox6);
        else if (tStarNum >= 3)
            tSweepChapBoxItem.f_UpdateChapBoxData(tPoolDt.m_ChapterTemplate.iBox3);
        sweepChapBoxList.Add(tSweepChapBoxItem);
        sweepTotalItem.f_AddChapBoxAward(tSweepChapBoxItem.m_ChapBoxAward);
    }

    private void f_AddSweepRetTollgateItem(int chapId, int tollgateIdx, ushort moneyRate, ushort prestigeRate)
    {
        RunningManPoolDT tPoolDt = (RunningManPoolDT)f_GetForId(chapId);
        RunningManTollgateDT tTollgateDt = tPoolDt.m_TollgatePoolDTs[tollgateIdx].m_TollgateTemplate;
        int[] moneyArr = ccMath.f_String2ArrayInt(tTollgateDt.szMoneys, ";");
        int[] prestigeArr = ccMath.f_String2ArrayInt(tTollgateDt.szPrests, ";");
        int tMoney = moneyArr[GameParamConst.RMModeNumPreTollgate - 1] * moneyRate / 100;
        int tPrestige = prestigeArr[GameParamConst.RMModeNumPreTollgate - 1] * prestigeRate / 100;
        string tMoneyDesc = f_GetCritDesc(moneyRate);
        string tPrestigeDesc = f_GetCritDesc(prestigeRate);
        RunningManSweepResult tSweepTollgateItem = new RunningManSweepResult(EM_RunningManSweepResultType.Tollgate);
        tSweepTollgateItem.f_UpdateTollgateData(tTollgateDt.iId, tTollgateDt.szName, tMoney, tPrestige, tMoneyDesc, tPrestigeDesc);
        sweepTollgateList.Add(tSweepTollgateItem);
        sweepTotalItem.f_AddTollgateAward(tSweepTollgateItem.m_iMoney, tSweepTollgateItem.m_iPrestige);
    }

    #endregion

    #region 发送协议

    /// <summary>
    /// 过关斩将初始化
    /// </summary>
    public void f_RunningManInit(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RunningManInit, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RunningManInit, bBuf);
    }

    /// <summary>
    /// 过关斩将挑战
    /// </summary>
    /// <param name="chapId">章节Id</param>
    /// <param name="idx">[1,3]</param>
    /// <param name="star">目标难度</param>
    public void f_RunningManChallenge(ushort chapId, byte idx, byte star, RunningManTollgatePoolDT extendInfo, SocketCallbackDT tSocketCallbackDT)
    {
        //更新战斗数据
        StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_RunningMan, chapId, extendInfo.m_iTollgateId, extendInfo.m_TollgateTemplate.iSceneId);
        challengeFinish = false;
        //发送战斗数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RunningManChallenge, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(chapId);
        tCreateSocketBuf.f_Add(idx);
        tCreateSocketBuf.f_Add(star);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RunningManChallenge, bBuf);
    }

    /// <summary>
    /// 章节宝箱
    /// </summary>
    /// <param name="chapId">章节Id</param>
    public void f_RunningManChapBox(ushort chapId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RunningManChapBox, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(chapId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RunningManChapBox, bBuf);
    }

    /// <summary>
    /// 兑换Buff
    /// </summary>
    /// <param name="chapId">目标章节</param>
    /// <param name="idx">buff索引[1,3]</param>
    public void f_RunningManBuff(ushort chapId, byte idx, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RunningManBuff, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(chapId);
        tCreateSocketBuf.f_Add(idx);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RunningManBuff, bBuf);
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void f_RunningManReset(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RunningManReset, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RunningManReset, bBuf);
        f_Reset();
    }

    /// <summary>
    /// 三星扫荡   //未实现
    /// </summary>
    public void f_RunningManSweep(SocketCallbackDT tSocketCallbackDT)
    {
        if (isSweeping)
        {
UITool.Ui_Trip("Đang xử lý");
            return;
        }
        isSweeping = true;
        sweepBuffList.Clear();
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RunningManSweep, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RunningManSweep, bBuf);

    }

    /// <summary>
    /// 历史最高星数排行榜  (游服发送，关系服接受)
    /// </summary>
    /// <param name="page">[0,4]</param>
    public void f_RunningManRank(int page, SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RunningManRank, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(page);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RunningManRank, bBuf);
    }

    /// <summary>
    /// 精英关卡挑战
    /// </summary>
    /// <param name="tollgateId">发送给服务器的 关卡Id</param>
    /// <param name="sceneId">客户端本地传递参数</param>
    public void f_RunningManElite(ushort tollgateId, int sceneId, SocketCallbackDT tSocketCallbackDT)
    {
        //更新战斗数据
        StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_RunningManElite, tollgateId, tollgateId, sceneId);
        eliteChallengeFinish = false;
        //发送协议
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RunningManElite, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tollgateId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RunningManElite, bBuf);
    }

    /// <summary>
    /// 精英关卡购买次数
    /// </summary>
    public void f_RunningManEliteTimes(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RunningManEliteTimes, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RunningManEliteTimes, bBuf);
    }

    #endregion

    #region 过关斩将相关变量
    /// <summary>
    /// 是否已经向服务器请求初始化
    /// </summary>
    private bool initForServer = false;

    /// <summary>
    /// 是否已经结算了
    /// </summary>
    private bool challengeFinish = false;
    private CMsg_SC_RunningManRet challengeFinishRet;

    private int historyChapter = 0;
    private int history3StarsChapter = 0;
    private int resetTimes = 0;

    private int curPassChapter = 0;
    private int chapterMaxId = 0;
    private bool isLose = false;

    public bool m_bChallengeFinish
    {
        get
        {
            return challengeFinish;
        }
    }

    public CMsg_SC_RunningManRet m_ChallengeFinishRet
    {
        get
        {
            return challengeFinishRet;
        }
    }

    /// <summary>
    /// 历史最高章节
    /// </summary>
    public int m_iHistoryChapter
    {
        get
        {
            return historyChapter;
        }
    }

    /// <summary>
    /// 历史最高连续3星章节
    /// </summary>
    public int m_iHistory3StarsChapter
    {
        get
        {
            return history3StarsChapter;
        }
    }

    /// <summary>
    /// 今日重置次数
    /// </summary>
    public int m_iResetTimes
    {
        get
        {
            return resetTimes;
        }
    }

    /// <summary>
    /// 重置次数
    /// </summary>
    public int m_iResetTimesLimit
    {
        get
        {
            return Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_RunningManResetTimes);
        }
    }

    public int m_iCurPassChapter
    {
        get
        {
            return curPassChapter;
        }
    }

    public int m_iChapterMaxId
    {
        get
        {
            return chapterMaxId;
        }
    }

    /// <summary>
    /// 是否已经失败了
    /// </summary>
    public bool m_bIsLose
    {
        get
        {
            return isLose;
        }
    }

    #endregion

    #region 星星相关

    private int historyStarNum = 0;

    private int curStarNum = 0;

    private int usedStarNum = 0;

    /// <summary>
    /// 历史最高星星数
    /// </summary>
    public int m_iHistoryStarNum
    {
        get
        {
            return historyStarNum;
        }
    }
    /// <summary>
    /// 单前的星星数
    /// </summary>
    public int m_iCurStarNum
    {
        get
        {
            return curStarNum;
        }
    }
    /// <summary>
    /// 已使用的星星数
    /// </summary>
    public int m_iUsedStarNum
    {
        get
        {
            return usedStarNum;
        }
    }

    /// <summary>
    /// 本轮剩余的星星数
    /// </summary>
    public int m_iLeftStarNum
    {
        get
        {
            return curStarNum - usedStarNum;
        }
    }

    private int m_iMyHistoryStarRank; //我的最高星数排行
    #endregion

    #region PropertyBuff

    private List<RunningManBuffProperty> buffPropertyList;

    /// <summary>
    /// 过关斩将的Buff属性
    /// </summary>
    public List<RunningManBuffProperty> m_BuffPropertyList
    {
        get
        {
            return buffPropertyList;
        }
    }

    public void f_AddBuff(int buffIdx, int buffId)
    {
        RunningManBuffDT buffDt = (RunningManBuffDT)glo_Main.GetInstance().m_SC_Pool.m_RunningManBuffSC.f_GetSC(buffId);
        if (buffDt == null)
        {
MessageBox.ASSERT("Added buff does not exist，BuffId：" + buffId);
            return;
        }
        //处理扫荡的Buff
        if (isSweeping)
        {
            RunningManSweepResult tBuffResult = new RunningManSweepResult(EM_RunningManSweepResultType.Buff);
            if (buffIdx == 1)
                tBuffResult.f_UpdateBuffData(buffIdx, buffDt.iAttrId, buffDt.iValue1);
            else if (buffIdx == 2)
                tBuffResult.f_UpdateBuffData(buffIdx, buffDt.iAttrId, buffDt.iValue2);
            else if (buffIdx == 3)
                tBuffResult.f_UpdateBuffData(buffIdx, buffDt.iAttrId, buffDt.iValue3);
            sweepBuffList.Add(tBuffResult);
        }
        //处理Buff
        for (int i = 0; i < buffPropertyList.Count; i++)
        {
            if (buffPropertyList[i].m_iPropertyType == buffDt.iAttrId)
            {
                if (buffIdx == 1)
                {
                    buffPropertyList[i].f_AddProperty(buffDt.iValue1);
                }
                else if (buffIdx == 2)
                {
                    buffPropertyList[i].f_AddProperty(buffDt.iValue2);
                }
                else if (buffIdx == 3)
                {
                    buffPropertyList[i].f_AddProperty(buffDt.iValue3);
                }
                return;
            }
        }
        RunningManBuffProperty tItem = new RunningManBuffProperty();
        if (buffIdx == 1)
        {
            tItem.f_UpdateProperty(buffDt.iAttrId, buffDt.iValue1);
        }
        else if (buffIdx == 2)
        {
            tItem.f_UpdateProperty(buffDt.iAttrId, buffDt.iValue2);
        }
        else if (buffIdx == 3)
        {
            tItem.f_UpdateProperty(buffDt.iAttrId, buffDt.iValue3);
        }
        buffPropertyList.Add(tItem);
    }

    #endregion

    #region 排行榜

    private List<BasePoolDT<long>> rankList = new List<BasePoolDT<long>>();
    public List<BasePoolDT<long>> m_RankList
    {
        get
        {
            return rankList;
        }
    }
    private const int RANK_LIST_TIME_DIS = 30;
    private const int RANK_LIST_NUM_PRE_PAGE = 10;
    private const int RANK_LIST_PAGE_MAX = 5;

    private int rankListTime = 0;
    private bool rankListWait = false;
    private int rankListPageIdx = 0;

    public void f_ExecuteAferRankList(bool first, ccCallback callbackRankList)
    {
        if (rankListWait && rankList.Count > 0)
        {
MessageBox.DEBUG("Waiting for a response from the server");
            return;
        }

        ccCallback callback_RunningManRank = (object result) =>
        {
            if (rankListWait)
            {
                rankListPageIdx++;
            }
            rankListWait = false;
            if (callbackRankList != null)
                callbackRankList(result);
        };


        if (first)
        {
            int tNow = GameSocket.GetInstance().f_GetServerTime();
            if (tNow - rankListTime > RANK_LIST_TIME_DIS)
            {
                rankListTime = tNow;
                rankListWait = true;
                rankList.Clear();
                rankListPageIdx = 0;
                SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
                socketCallbackDt.m_ccCallbackSuc = callback_RunningManRank;
                socketCallbackDt.m_ccCallbackFail = callback_RunningManRank;
                f_RunningManRank(rankListPageIdx, socketCallbackDt);
            }
            else
            {
                if (callbackRankList != null)
                    callbackRankList(eMsgOperateResult.OR_Succeed);
            }
        }
        else
        {
            if (rankList.Count == rankListPageIdx * RANK_LIST_NUM_PRE_PAGE && rankListPageIdx < RANK_LIST_PAGE_MAX)
            {
                rankListWait = true;
                SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
                socketCallbackDt.m_ccCallbackSuc = callback_RunningManRank;
                socketCallbackDt.m_ccCallbackFail = callback_RunningManRank;
                f_RunningManRank(rankListPageIdx, socketCallbackDt);
            }
            else
            {
                if (callbackRankList != null)
                    callbackRankList(eMsgOperateResult.OR_Succeed);
            }
        }
    }

    /// <summary>
    /// 获取我的排行榜信息
    /// </summary>
    /// <returns></returns>
    public RunningManRankPoolDT f_GetMyRankData() {
        for (int i = 0; i < rankList.Count; i++) {
            RunningManRankPoolDT dt = (RunningManRankPoolDT)rankList[i];
            if (dt.m_iPlayerId == Data_Pool.m_UserData.m_iUserId)
                return dt;
        }

        //未上榜的数据取不到，构造一个
        return new RunningManRankPoolDT(m_iMyHistoryStarRank, Data_Pool.m_UserData.m_iUserId, historyStarNum);
    }
    #endregion

    #region 精英关卡

    private List<BasePoolDT<long>> eliteList;

    private int eliteFirstProg = 0;
    private int eliteTimes = 0;
    private int eliteBuyTimes = 0;
    private int eliteTollgateMax = 0;

    public int m_iEliteFirstProg
    {
        get
        {
            return eliteFirstProg;
        }
    }

    public int m_iEliteTimes
    {
        get
        {
            return eliteTimes;
        }
    }

    public int m_iEliteBuyTimes
    {
        get
        {
            return eliteBuyTimes;
        }
    }

    public int m_iEliteTollgateMax
    {
        get
        {
            return eliteTollgateMax;
        }
    }

    private bool eliteChallengeFinish = false;
    private CMsg_SC_RunningManEliteRet eliteChallengeFinishRet;

    public bool m_bEliteChallengeFinish
    {
        get
        {
            return eliteChallengeFinish;
        }
    }

    public CMsg_SC_RunningManEliteRet m_EliteChallengeFinishRet
    {
        get
        {
            return eliteChallengeFinishRet;
        }
    }

    public List<BasePoolDT<long>> m_EliteList
    {
        get
        {
            return eliteList;
        }
    }

    public RunningManElitePoolDT f_GetCurElitePoolDt()
    {
        int idx = 0;
        idx = UnityEngine.Mathf.Max(idx, Data_Pool.m_RunningManPool.historyChapter);
        idx = UnityEngine.Mathf.Min(idx, Data_Pool.m_RunningManPool.eliteFirstProg + 1);
        return (RunningManElitePoolDT)eliteList.Find(delegate (BasePoolDT<long> dt)
        {
            return dt.iId == idx;
        });
    }

    public RunningManElitePoolDT f_GetEliteData(long id)
    {
        return (RunningManElitePoolDT)eliteList.Find(delegate (BasePoolDT<long> tItem)
        {
            return tItem.iId == id;
        });
    }

    #endregion

    #region 扫荡相关

    private List<RunningManSweepResult> sweepTollgateList = new List<RunningManSweepResult>();
    private List<RunningManSweepResult> sweepBuffList = new List<RunningManSweepResult>();
    private List<RunningManSweepResult> sweepChapBoxList = new List<RunningManSweepResult>();
    private RunningManSweepResult sweepTotalItem;

    private bool isSweeping = false;

    public List<RunningManSweepResult> m_SweepTollgateList
    {
        get
        {
            return sweepTollgateList;
        }
    }
    public List<RunningManSweepResult> m_SweepBuffList
    {
        get
        {
            return sweepBuffList;
        }
    }
    public List<RunningManSweepResult> m_SweepChapBoxList
    {
        get
        {
            return sweepChapBoxList;
        }
    }
    public RunningManSweepResult m_SweepTotalItem
    {
        get
        {
            return sweepTotalItem;
        }
    }

    #endregion


    #region 对外接口
    public void f_CheckEliTimesLeft()
    {
        if (historyChapter > 0 && UITool.f_GetIsOpensystem(EM_NeedLevel.RunningManLvel))
        {
            int eliteLeftTimes = m_iEliteBuyTimes + GameParamConst.RMEliteTimesLimit - m_iEliteTimes;
            if (eliteLeftTimes > 0)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RunningManEliteLeftTimes);
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.RunningManEliteLeftTimes);
            }
            else
            {
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RunningManEliteLeftTimes);
            }
        }
    }
    /// <summary>
    /// 在
    /// </summary>
    /// <param name="executeHandle"></param>
    public void f_ExecuteAfterRunningManInit(ccCallback executeHandle)
    {
        if (initForServer)
        {
            executeHandle(eMsgOperateResult.OR_Succeed);
            return;
        }
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = executeHandle;
        socketCallbackDt.m_ccCallbackFail = executeHandle;
        f_RunningManInit(socketCallbackDt);
        initForServer = true;
    }

    #endregion

}


/// <summary>
/// 过关斩将Buff属性结构体
/// </summary>
public class RunningManBuffProperty
{
    public int m_iPropertyType
    {
        get;
        private set;
    }

    public int m_iPropertyValue
    {
        get;
        private set;
    }

    public void f_UpdateProperty(int propertyType, int propertyValue)
    {
        m_iPropertyType = propertyType;
        m_iPropertyValue = propertyValue;
    }

    public void f_AddProperty(int propertyValue)
    {
        m_iPropertyValue += propertyValue;
    }
}
