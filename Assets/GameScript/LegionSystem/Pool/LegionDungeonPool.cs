using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LegionDungeonPool : BasePool
{
    #region 公共属性

    /// <summary>
    /// 副本次数
    /// </summary>
    private int _dungeonTimes;
    public int m_iDungeonTimes
    {
        get
        {
            return _dungeonTimes;
        }
    }

    /// <summary>
    /// 副本挑战额外次数
    /// </summary>
    private int _dungeonExtraTimes;

    public int f_GetDungeonExtraTimes(DateTime now)
    {
        int span = (now.Hour <= LegionConst.LEGION_DUNGEON_END_TIME? now.Hour:LegionConst.LEGION_DUNGEON_END_TIME) - LegionConst.LEGION_DUNGEON_BEGIN_TIME;
        if (span <= 0)
        {
            return _dungeonExtraTimes;
        }
        _dungeonExtraTimes =  span / LegionConst.LEGION_DUNGEON_TIMES_RECOVER_TIME;
        return _dungeonExtraTimes;
    }


    /// <summary>
    /// 副本挑战次数限制（未加上时间恢复的次数）
    /// </summary>
    public int m_iDungeonTimesLimit
    {
        get
        {
            return LegionConst.LEGION_DUNGEON_INIT_TIMES_LIMIT + _dungeonBuyTimes;
        }
    }

    /// <summary>
    /// 副本剩余次数（未加上时间恢复的次数）
    /// </summary>
    public int m_iDungeonLeftTimes
    {
        get
        {
            return m_iDungeonTimesLimit - m_iDungeonTimes;
        }
    }
    
    private int _dungeonBuyTimes;
    /// <summary>
    /// 副本已经购买次数
    /// </summary>
    public int m_iDungeonBuyTimes
    {
        get
        {
            return _dungeonBuyTimes;
        }
    }

    /// <summary>
    /// 副本购买次数限制
    /// </summary>
    public int m_iDungeonBuyTimesLimit
    {
        get
        {
            return Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_LegionDungeonBuyTimes);
        }
    }

    /// <summary>
    /// 购买剩余次数
    /// </summary>
    public int m_iDungeonBuyLeftTimes
    {
        get
        {
            return m_iDungeonBuyTimesLimit - m_iDungeonBuyTimes;
        }
    }

    private byte _curChapId;

    public byte m_iCurDungeonChapId
    {
        get
        {
            return _curChapId;
        }
    }

    private byte _dungeonPassChap;
    /// <summary>
    /// 军团副本最高通关章节 
    /// </summary>
    public byte m_iDungeonPassChap
    {
        get
        {
            return _dungeonPassChap;
        }
    }

    private byte _resetFlag;
    /// <summary>
    /// 军团副本重置类型
    /// </summary>
    public byte m_ResetFlag
    {
        get
        {
            return _resetFlag;
        }
    }
    /// <summary>
    /// 章节的最大值
    /// </summary>
    public byte m_iChapterIdMax = 0;
    //重置章节Id
    private byte _resetChapterId;
    /// <summary>
    /// 重置章节的Id
    /// </summary>
    public byte m_iResetChapterId
    {
        get
        {
            return _resetChapterId;
        }
    }
    

    /// <summary>
    /// 章节宝箱相关
    /// </summary>
    private byte _awardChapter;
    /// <summary>
    /// 已经领取奖励的章节
    /// </summary>
    public byte m_iAwardChapter
    {
        get
        {
            return _awardChapter;
        }
    }

    private List<byte> _chapterFinishTodayList = new List<byte>();

    #endregion

    public LegionDungeonPool() : base("LegionDungeonPoolDT", false)
    {

    }

    protected override void f_Init()
    {
        _dungeonExtraTimes = 0;
        _resetFlag = 0;
        _dungeonPassChap = 0;
        _curChapId = 0;
        _chapterFinishTodayList.Clear();
        List<NBaseSCDT> tInitList = glo_Main.GetInstance().m_SC_Pool.m_LegionChapterSC.f_GetAll();
        for (int i = 0; i < tInitList.Count; i++)
        {
            LegionChapterDT tNode = (LegionChapterDT)tInitList[i];
            LegionDungeonPoolDT poolDt = new LegionDungeonPoolDT(tNode);
            f_Save(poolDt);
            m_iChapterIdMax = (byte)tNode.iId;
        }
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, f_UpdateDataByNextDay);
    }

    private void f_UpdateDataByNextDay(object value)
    {
        _dungeonExtraTimes = 0;
        _dungeonTimes = 0;
        _chapterFinishTodayList.Clear();
        //跨天UI事件  通知关心此消息的UI更新
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_THENEXTDAY_UIPROCESS, EM_NextDaySource.LegionDungeonPool);
    }

    protected override void RegSocketMessage()
    {
        CMsg_GTC_LegionDungeonInitChapter tsInitChapter = new CMsg_GTC_LegionDungeonInitChapter();
        GameSocket.GetInstance().f_RegMessage((int)LegionSocketCmd.SC_LegionDungeonInitChapter, tsInitChapter, f_GTC_InitChapterHandle);

        CMsg_ByteNode tsInitFiniChapterNode = new CMsg_ByteNode();
        GameSocket.GetInstance().f_RegMessage_Int3((int)LegionSocketCmd.SC_LegionDungeonInitFiniChapter, tsInitFiniChapterNode, f_GTC_InitFiniChapterHandle);

        CMsg_AwardInfoNode tsInitTollgateNode = new CMsg_AwardInfoNode();
        GameSocket.GetInstance().f_RegMessage_Int1((int)LegionSocketCmd.SC_LegionDungeonInitTollgate, tsInitTollgateNode, f_GTC_InitTollgateHandle);

        CMsg_GTC_LegionDungeonChallengeRet tsChallengeRet = new CMsg_GTC_LegionDungeonChallengeRet();
        GameSocket.GetInstance().f_RegMessage((int)LegionSocketCmd.SC_LegionDungeonChallengeRet, tsChallengeRet, f_GTC_ChallengeRetHandle);

        CMsg_GTC_LegionDungeonTimes tsDungeonTimes = new CMsg_GTC_LegionDungeonTimes();
        GameSocket.GetInstance().f_RegMessage((int)LegionSocketCmd.SC_LegionDungeonTimes, tsDungeonTimes, f_GTC_DungeonTimesHandle);
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        
    }

    #region 协议处理函数

    private void f_GTC_InitChapterHandle(object value)
    {
        CMsg_GTC_LegionDungeonInitChapter tServerData = (CMsg_GTC_LegionDungeonInitChapter)value;
        //处理
        LegionDungeonPoolDT poolDt =  (LegionDungeonPoolDT)f_GetForId(tServerData.chapId);
        if (poolDt == null)
        {
MessageBox.ASSERT("LegionDungeonInitChapter does not exist ,Id:" + tServerData.chapId);
            return;
        }
        for (int i = 0; i < tServerData.hpKillerNode.Length; i++)
        {
            poolDt.f_UpdateTollgateInfo(i,tServerData.hpKillerNode[i].hp, tServerData.hpKillerNode[i].killer);
        } 
    }

    private void f_GTC_InitFiniChapterHandle(int iData1, int iData2,int iData3 ,int iNum, ArrayList aData)
    {
        _curChapId = (byte)iData1;
        _dungeonPassChap = (byte)iData2;
        _resetFlag = (byte)iData3;
        //设置重置章节Id
        if (_dungeonPassChap <= 0 || _dungeonPassChap >= m_iChapterIdMax)
        {
            _resetChapterId = _curChapId;
        }
        if (_resetFlag == 0)
        {
            int chapterLimit = LegionMain.GetInstance().m_LegionInfor.mLevelTemplate.iDungeonChapter;
            if (_dungeonPassChap + 1 > chapterLimit)
                _resetChapterId = _dungeonPassChap; 
            else
                _resetChapterId = (byte)(_dungeonPassChap + 1);
        }
        else
            _resetChapterId =  _dungeonPassChap;
        _chapterFinishTodayList.Clear();
        for (int i = 0; i < aData.Count; i++)
        {
            CMsg_ByteNode tNode = (CMsg_ByteNode)aData[i];
            _chapterFinishTodayList.Add(tNode.value1);
        }
        //章节奖励红点
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LegionChapetAward);
        if (_awardChapter < _dungeonPassChap)
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.LegionChapetAward);
    }

    private void f_GTC_InitTollgateHandle(int iData1, int iData2, int iNum, ArrayList aData)
    {
        int chapId_Camp = iData1;
        if (!_tollgateAwardDic.ContainsKey(chapId_Camp))
        {
            f_InitTollgateAwardData(chapId_Camp);
        }
        List<BasePoolDT<long>> tAwardList = _tollgateAwardDic[chapId_Camp];
        LegionTollgateAwardPoolDT tPoolDt;
        LegionTollgateBoxAward tAwardTemplate = null;
        for (int i = 0; i < aData.Count; i++)
        {
            CMsg_AwardInfoNode tNode = (CMsg_AwardInfoNode)aData[i];
            if (tNode.idx <= 0 && tNode.idx > tAwardList.Count)
            {
MessageBox.ASSERT("Bonus legion, Idx out of range");
                continue;
            }
            tPoolDt = (LegionTollgateAwardPoolDT)tAwardList[tNode.idx-1];
            if (tNode.awardIdx <= 0 && tNode.awardIdx > _tollgateBoxAwardDic[chapId_Camp].Count)
            {
MessageBox.ASSERT("Bonus Legion, AwardIdx out of range");
                continue;
            }
            tAwardTemplate = _tollgateBoxAwardDic[chapId_Camp][tNode.awardIdx -1];
            tPoolDt.f_UpdateData(tNode.userId, tNode.awardIdx, tAwardTemplate);
            LegionTollgateAwardCountPoolDT tAwardCountPoolDt;
            tAwardCountPoolDt = (LegionTollgateAwardCountPoolDT)_tollgateAwardCountDic[chapId_Camp][tNode.awardIdx-1];
            tAwardCountPoolDt.f_AlreadyGetCountAdd();
            if (tNode.userId == Data_Pool.m_UserData.m_iUserId)
                _tollgateAwardSlefCanGetDic[chapId_Camp] = false;
        }
    }

    private bool _challengeFinish = false;
    public bool m_bChallengeFinish
    {
        get
        {
            return _challengeFinish;
        }
    }
    public CMsg_GTC_LegionDungeonChallengeRet m_sChallengeRet
    {
        private set;
        get;
    }

    private void f_GTC_ChallengeRetHandle(object value)
    {
        CMsg_GTC_LegionDungeonChallengeRet ret = (CMsg_GTC_LegionDungeonChallengeRet)value;
        _challengeFinish = true;
        m_sChallengeRet = ret;
    }

    private void f_GTC_DungeonTimesHandle(object value)
    {
        CMsg_GTC_LegionDungeonTimes tServerData = (CMsg_GTC_LegionDungeonTimes)value;
        _dungeonTimes = tServerData.uTimes;
        _dungeonBuyTimes = tServerData.uBuyTimes;
    }

    #endregion

    #region 发送协议

    /// <summary>
    /// 初始化章节
    /// </summary>
    public void f_InitChapter(byte chapId,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionDungeonInitChapter, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(chapId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionDungeonInitChapter, bBuf);
    }

    /// <summary>
    /// 初始化已完成章节数据
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_InitFiniChapter(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionDungeonInitFiniChapter, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionDungeonInitFiniChapter, bBuf);
    }

    /// <summary>
    /// 初始化关卡奖励信息
    /// </summary>
    public void f_InitTollgate(bool isValid ,byte chapter,byte camp,SocketCallbackDT socketCallbackDt)
    {    
        //重置关卡奖励消息
        int chapId_Camp = f_ProcessChapIdAndCamp(chapter, camp);
        f_ResetTollgateAwardData(chapId_Camp);
        if (!isValid)
        {
            socketCallbackDt.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionDungeonInitTollgate, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(chapId_Camp);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionDungeonInitTollgate, bBuf);
    }

    /// <summary>
    /// 挑战关卡
    /// </summary>
    public void f_Challenge(byte camp,int sceneId,SocketCallbackDT socketCallbackDt)
    {
        //更新战斗数据
        StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_LegionDungeon, m_iCurDungeonChapId, camp, sceneId);
        _challengeFinish = false;
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionDungeonChallenge, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(camp);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionDungeonChallenge, bBuf);
    }

    /// <summary>
    /// 关卡奖励
    /// </summary>
    /// <param name="chapId">章节ID</param>
    /// <param name="camp">阵营 【1，4】</param>
    /// <param name="boxIdx">宝箱Idx 【1，公会最大人数】</param>
    /// <param name="socketCallbackDt"></param>
    public void f_TollgateAward(byte chapId,byte camp,byte boxIdx,SocketCallbackDT socketCallbackDt)
    {
        int chapId_Camp = f_ProcessChapIdAndCamp(chapId, camp);
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionDungeonTollgateAward, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(chapId_Camp);
        tCreateSocketBuf.f_Add(boxIdx);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionDungeonTollgateAward, bBuf);
    }

    /// <summary>
    /// 章节奖励
    /// </summary>
    public void f_ChapterAward(byte chanpId,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionDungeonChapterAward, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(chanpId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionDungeonChapterAward, bBuf);
    }

    /// <summary>
    /// 重置副本
    /// </summary>
    /// <param name="isBack">0：默认，!0：回退 </param>
    /// <param name="socketCallbackDt"></param>
    public void f_ResetForServer(byte isBack,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionDungeonReset, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(isBack);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionDungeonReset, bBuf);
    }


    /// <summary>
    /// 购买副本挑战次数
    /// </summary>
    /// <param name="times"></param>
    /// <param name="socketCallbackDt"></param>
    public void f_BuyDungeonTimes(byte times, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionDungeonTimes, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(times);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionDungeonTimes, bBuf);
    }

    #endregion

    #region 对外接口

    public bool f_IsInOpenTime(bool showTip)
    {
        DateTime tNow = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        if (tNow.Hour >= LegionConst.LEGION_DUNGEON_BEGIN_TIME && tNow.Hour < LegionConst.LEGION_DUNGEON_END_TIME)
        {
            return true;
        }
        else
        {
            if (showTip)
            {
UITool.Ui_Trip(string.Format("{0}:00-{1}:00 mỗi ngày có thể khiêu chiến", LegionConst.LEGION_DUNGEON_BEGIN_TIME,LegionConst.LEGION_DUNGEON_END_TIME));
            }
            return false;
        }
    }

    

    public bool f_IsFinisChapterToday(byte chapId)
    {
        return _chapterFinishTodayList.Contains(chapId);
    }

    public void f_ExecuteAfterInitFiniChapterAndInitCurChapter(ccCallback callbackHandle)
    {
        m_Callback_AfterInitFiniChapterAndInitCurChapter = callbackHandle;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_InitFiniChapter;
        socketCallbackDt.m_ccCallbackFail = f_Callback_InitFiniChapter;
        f_InitFiniChapter(socketCallbackDt);
    }

    public void f_UpdateChapterAward(byte awardChapter)
    {
        _awardChapter = awardChapter;
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LegionChapetAward);
        if(_awardChapter < _dungeonPassChap)
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.LegionChapetAward);
    }

    #endregion

    /// <summary>
    /// 处理章节Id和阵型
    /// </summary>
    /// <param name="chapId"></param>
    /// <param name="camp"></param>
    /// <returns></returns>
    private int f_ProcessChapIdAndCamp(byte chapId, byte camp)
    {
        return chapId * 10 + camp;
    }

    private void f_ProcessChapIdAndCamp(int chapId_Camp, ref byte chapId, ref byte camp)
    {
        chapId = (byte)(chapId_Camp / 10);
        camp = (byte)(chapId_Camp % 10);
    }


    private ccCallback m_Callback_AfterInitFiniChapterAndInitCurChapter;
    private void f_Callback_InitFiniChapter(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_InitCurChapter;
            socketCallbackDt.m_ccCallbackFail = f_Callback_InitCurChapter;
            f_InitChapter(m_iCurDungeonChapId,socketCallbackDt);
        }
        else
        {
MessageBox.ASSERT("Initialization of chapter data failed ,code:" + result);
        }
    }

    private void f_Callback_InitCurChapter(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            if (m_Callback_AfterInitFiniChapterAndInitCurChapter != null)
                m_Callback_AfterInitFiniChapterAndInitCurChapter(eMsgOperateResult.OR_Succeed);
        }
        else
        {
MessageBox.ASSERT("Initialization of current chapter data failed,code:" + result);
        }
    }

    #region 关卡奖励相关

    /// <summary>
    /// 关卡奖励字典
    /// </summary>
    private Dictionary<int,List<BasePoolDT<long>>> _tollgateAwardDic = new Dictionary<int,List<BasePoolDT<long>>>();

    /// <summary>
    /// 关卡奖励数量统计字典
    /// </summary>
    private Dictionary<int, List<BasePoolDT<long>>> _tollgateAwardCountDic = new Dictionary<int, List<BasePoolDT<long>>>();

    /// <summary>
    /// 关卡奖励配置文件数据
    /// </summary>
    private Dictionary<int, List<LegionTollgateBoxAward>> _tollgateBoxAwardDic = new Dictionary<int, List<LegionTollgateBoxAward>>();

    private Dictionary<int, bool> _tollgateAwardSlefCanGetDic = new Dictionary<int, bool>();

    private void f_InitTollgateAwardData(int chapId_Camp)
    {
        _tollgateAwardDic.Add(chapId_Camp, new List<BasePoolDT<long>>());
        _tollgateAwardCountDic.Add(chapId_Camp,new List<BasePoolDT<long>>());
        _tollgateBoxAwardDic.Add(chapId_Camp,new List<LegionTollgateBoxAward>());
        _tollgateAwardSlefCanGetDic.Add(chapId_Camp, true);
        int tollgateBoxCount = 0;
        byte tChapterId = 0;
        byte tCamp = 0;
        f_ProcessChapIdAndCamp(chapId_Camp, ref tChapterId, ref tCamp);
        LegionDungeonPoolDT tDungeonDt = (LegionDungeonPoolDT)f_GetForId(tChapterId);
        int tTollgateId = tDungeonDt.f_GetTollgatePoolDtByIdx(tCamp - 1).mTollgateTemplate.iId;
        LegionTollgateBoxDT tBoxDt = (LegionTollgateBoxDT)glo_Main.GetInstance().m_SC_Pool.m_LegionTollgateBoxSC.f_GetSC(tTollgateId);
        if (tBoxDt != null)
        {
            tollgateBoxCount = tBoxDt.iGenCount1 + tBoxDt.iGenCount2 + tBoxDt.iGenCount3 + tBoxDt.iGenCount4 + tBoxDt.iGenCount5 + tBoxDt.iGenCount6;
            _tollgateBoxAwardDic[chapId_Camp].Add(new LegionTollgateBoxAward(tBoxDt.iAwardType1,tBoxDt.iAwardId1,tBoxDt.iAwardNum1,tBoxDt.iGenCount1));
            _tollgateBoxAwardDic[chapId_Camp].Add(new LegionTollgateBoxAward(tBoxDt.iAwardType2, tBoxDt.iAwardId2, tBoxDt.iAwardNum2, tBoxDt.iGenCount2));
            _tollgateBoxAwardDic[chapId_Camp].Add(new LegionTollgateBoxAward(tBoxDt.iAwardType3, tBoxDt.iAwardId3, tBoxDt.iAwardNum3, tBoxDt.iGenCount3));
            _tollgateBoxAwardDic[chapId_Camp].Add(new LegionTollgateBoxAward(tBoxDt.iAwardType4, tBoxDt.iAwardId4, tBoxDt.iAwardNum4, tBoxDt.iGenCount4));
            _tollgateBoxAwardDic[chapId_Camp].Add(new LegionTollgateBoxAward(tBoxDt.iAwardType5, tBoxDt.iAwardId5, tBoxDt.iAwardNum5, tBoxDt.iGenCount5));
            _tollgateBoxAwardDic[chapId_Camp].Add(new LegionTollgateBoxAward(tBoxDt.iAwardType6, tBoxDt.iAwardId6, tBoxDt.iAwardNum6, tBoxDt.iGenCount6));
            LegionTollgateAwardPoolDT tAwardPoolDt;
            for (int i = 0; i < tollgateBoxCount; i++)
            {
                tAwardPoolDt = new LegionTollgateAwardPoolDT(chapId_Camp, i + 1);
                _tollgateAwardDic[chapId_Camp].Add(tAwardPoolDt);
            }
            LegionTollgateAwardCountPoolDT tAwardCountAwardPoolDt;
            for (int i = 0; i < _tollgateBoxAwardDic[chapId_Camp].Count; i++)
            {
                if (_tollgateBoxAwardDic[chapId_Camp][i].m_iAwardType == 0)
                    continue;
                tAwardCountAwardPoolDt = new LegionTollgateAwardCountPoolDT(chapId_Camp, i + 1, _tollgateBoxAwardDic[chapId_Camp][i]);
                _tollgateAwardCountDic[chapId_Camp].Add(tAwardCountAwardPoolDt);
            }
        }
        else
        {
MessageBox.ASSERT("Treatment of corps duplicate bonus does not exist,tollgateId:" + tTollgateId);
        }
    }

    public void f_ResetTollgateAwardData(int chapId_Camp)
    {
        if (!_tollgateAwardDic.ContainsKey(chapId_Camp))
            f_InitTollgateAwardData(chapId_Camp);
        LegionTollgateAwardPoolDT tAwardPoolDt;
        for (int i = 0,max = _tollgateAwardDic[chapId_Camp].Count; i < max; i++)
        {
            tAwardPoolDt = (LegionTollgateAwardPoolDT)_tollgateAwardDic[chapId_Camp][i];
            tAwardPoolDt.f_ResetData();
        }
        LegionTollgateAwardCountPoolDT tAwardCountPoolDt;
        for (int i = 0,max = _tollgateAwardCountDic[chapId_Camp].Count; i < max; i++)
        {
            tAwardCountPoolDt = (LegionTollgateAwardCountPoolDT)_tollgateAwardCountDic[chapId_Camp][i];
            tAwardCountPoolDt.f_AlreadyGetCountInit();
        }
        _tollgateAwardSlefCanGetDic[chapId_Camp] = true;
    }

    public List<BasePoolDT<long>> f_GetTollgateAwardList(byte chapterId, byte camp)
    {
        int chapId_Camp = f_ProcessChapIdAndCamp(chapterId, camp);
        if (_tollgateAwardDic.ContainsKey(chapId_Camp))
            return _tollgateAwardDic[chapId_Camp];
        else
            return new List<BasePoolDT<long>>(); 
    }
    public List<BasePoolDT<long>> f_GetTollgateAwardCountList(byte chapterId, byte camp)
    {
        int chapId_Camp = f_ProcessChapIdAndCamp(chapterId, camp);
        if (_tollgateAwardCountDic.ContainsKey(chapId_Camp))
            return _tollgateAwardCountDic[chapId_Camp];
        else
            return new List<BasePoolDT<long>>();
    }

    public bool f_GetTollgateAwardCanGet(byte chapterId, byte camp)
    {
        int chapId_Camp = f_ProcessChapIdAndCamp(chapterId, camp);
        if (_tollgateAwardSlefCanGetDic.ContainsKey(chapId_Camp))
            return _tollgateAwardSlefCanGetDic[chapId_Camp];
        else
            return false;
    }

    #endregion
}

public class LegionTollgateBoxAward
{
    public LegionTollgateBoxAward(int awardType,int awardId,int awardCount,int genCount)
    {
        m_iAwardType = awardType;
        m_iAwardId = awardId;
        m_iAwardCount = awardCount;
        m_iGenCount = genCount;
    }

    public int m_iAwardType
    {
        get;
        private set;
    }

    public int m_iAwardId
    {
        get;
        private set;
    }

    public int m_iAwardCount
    {
        get;
        private set;
    }

    public int m_iGenCount
    {
        get;
        private set;
    }
}
