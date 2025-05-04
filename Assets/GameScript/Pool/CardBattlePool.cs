using ccU3DEngine;
using System.Collections.Generic;
using System;

public class CardBattlePool : BasePool
{
    public enum EM_CardBattleState
    {
        Invalid = 0, //无效的，未开启
        InApply = 1, //在报名期间
        BetweenApplyBattle = 2, //在报名和战斗期间
        InBattle = 3, //在战斗期间
    }

    /// <summary>
    /// 可用斗将数量
    /// </summary>
    public const int CardBattleUseableNum = 20;
	GameParamDT CardBattleOpen;

    private int m_LeftRefreshTeamTimes;
    /// <summary>
    /// 剩余刷新队伍
    /// </summary>
    public int LeftRefreshTeamTimes
    {
        get
        {
            return m_LeftRefreshTeamTimes;
        }
    }

    public readonly int RefreshTeamTimeCD;
    //请求刷新队伍时间
    private int m_RefreshTeamTime;
    public int RefreshTeamTime
    {
        get
        {
            return m_RefreshTeamTime;
        }
    }

    public readonly int SetDefClothTimeCD;
    private int m_SetDefClothTime;
    //设置防守阵容时间
    public int SetDefClothTime
    {
        get
        {
            return m_SetDefClothTime;
        }
    }

    //开启时间相关配置
    private readonly int m_OpenDayLimit; //开服多少天后才能开启
    private readonly DayOfWeek[] m_OpenDayOfWeeks; //星期几开启
    private readonly int[] m_ApplyTime;  //报名时间： [开启时间,持续时间(单位：分钟)]
    private readonly int[] m_BattleTime; //战斗时间：[开启时间，持续时间(单位：分钟)]
    
    private int m_CurNowTotalDays = -1;
    private int m_ApplyBeginTime = 0;
    private int m_ApplyEndTime = 0;
    private int m_BattleBeginTime = 0;
    private int m_BattleEndTime = 0;

    /// <summary>
    /// 战斗时间
    /// </summary>
    public int[] BattleTime
    {
        get
        {
            return m_BattleTime;
        }
    }

    /// <summary>
    /// 战斗结束时间戳
    /// </summary>
    public int BattleEndTime
    {
        get
        {
            return m_BattleEndTime;
        }
    }

    //布阵
    private List<CardBattleClothPoolDT> m_AtkClothList;
    private List<CardBattleClothPoolDT> m_DefClothList;
    /// <summary>
    /// 攻击布阵
    /// </summary>
    public List<CardBattleClothPoolDT> AtkClothList
    {
        get
        {
            return m_AtkClothList;
        }
    }
    /// <summary>
    /// 防守布阵
    /// </summary>
    public List<CardBattleClothPoolDT> DefClothList
    {
        get
        {
            return m_DefClothList;
        }
    }

    //对手列表
    private List<BasePoolDT<long>> m_EnemyList;
    /// <summary>
    /// 对手列表
    /// </summary>
    public List<BasePoolDT<long>> EnemyList
    {
        get
        {
            return m_EnemyList;
        }
    }

    public readonly int RefreshEnemyTimeCD;
    //请求刷新对手时间
    private int m_RefreshEnemyTime;
    public int RefreshEnemyTime
    {
        get
        {
            return m_RefreshEnemyTime;
        }
    } 

    private int m_LeftChallengeTimes;
    /// <summary>
    /// 剩余挑战次数
    /// </summary>
    public int LeftChallengeTimes
    {
        get
        {
            return m_LeftChallengeTimes;
        }
    }

    private int m_WinTimes;
    /// <summary>
    /// 胜利次数
    /// </summary>
    public int WinTimes
    {
        get
        {
            return m_WinTimes;
        }
    }

    private int m_LoseTimes;
    /// <summary>
    /// 失败次数
    /// </summary>
    public int LoseTimes
    {
        get
        {
            return m_LoseTimes;
        }
    }

    public bool IsFinish
    {
        private set;
        get;
    }

    public int Result
    {
        private set;
        get;
    }

    public CardBattlePool():base("CardBattleTeamPoolDT")
    {
		//My Code
		CardBattleOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(96) as GameParamDT);
		//
        m_IsInitInfo = false;
        m_RefreshTeamTime = 0;
        RefreshTeamTimeCD = 30;
        m_SetDefClothTime = 0;
        SetDefClothTimeCD = 10;
        m_RefreshEnemyTime = 0;
        RefreshEnemyTimeCD = 10;
        m_LeftChallengeTimes = 0;
        m_WinTimes = 0;
        m_LoseTimes = 0;
        m_OpenDayLimit = CardBattleOpen.iParam2 * 1;
        //配置文件初始 参数
        GameParamDT openDayOfWeekDT = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.CardBattleOpenDayOfWeek);
        if (openDayOfWeekDT != null)
        {
            if (openDayOfWeekDT.iParam4 >= 0)
                m_OpenDayOfWeeks = new DayOfWeek[] { (DayOfWeek)openDayOfWeekDT.iParam1, (DayOfWeek)openDayOfWeekDT.iParam2, (DayOfWeek)openDayOfWeekDT.iParam3, (DayOfWeek)openDayOfWeekDT.iParam4 };
            else if (openDayOfWeekDT.iParam3 >= 0)
                m_OpenDayOfWeeks = new DayOfWeek[] { (DayOfWeek)openDayOfWeekDT.iParam1, (DayOfWeek)openDayOfWeekDT.iParam2, (DayOfWeek)openDayOfWeekDT.iParam3 };
            else if (openDayOfWeekDT.iParam2 >= 0)
                m_OpenDayOfWeeks = new DayOfWeek[] { (DayOfWeek)openDayOfWeekDT.iParam1, (DayOfWeek)openDayOfWeekDT.iParam2 };
            else if (openDayOfWeekDT.iParam1 >= 0)
                m_OpenDayOfWeeks = new DayOfWeek[] { (DayOfWeek)openDayOfWeekDT.iParam1 };
            else
                m_OpenDayOfWeeks = new DayOfWeek[] { };
        }
        else
        {
            m_OpenDayOfWeeks = new DayOfWeek[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };
        }
        //
        GameParamDT applyTimeDT = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.CardBattleApplyTime);
        if (applyTimeDT != null)
            m_ApplyTime = new int[3] { applyTimeDT.iParam1, applyTimeDT.iParam2, applyTimeDT.iParam3 };
        else
            m_ApplyTime = new int[3] { 9, 0, 535 };
        GameParamDT battleTimeDT = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.CardBattleBattleTime);
        if(battleTimeDT != null)
            m_BattleTime = new int[3] { battleTimeDT.iParam1, battleTimeDT.iParam2, battleTimeDT.iParam3 };
        else
            m_BattleTime = new int[3] { 18,0, 360 };

        m_AtkClothList = new List<CardBattleClothPoolDT>();
        m_DefClothList = new List<CardBattleClothPoolDT>();
        for (int i = 0; i < (int)EM_CloseArrayPos.eCloseArray_PosSix + 1; i++)
        {
            m_AtkClothList.Add(new CardBattleClothPoolDT(i, 0));
            m_DefClothList.Add(new CardBattleClothPoolDT(i, 0));
        }
        m_EnemyList = new List<BasePoolDT<long>>();
    }

    protected override void f_Init()
    {
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, f_UpdateDataByNextDay); 
    }
    
    private void f_UpdateDataByNextDay(object value)
    {
        m_WinTimes = 0;
        m_LoseTimes = 0;
        m_IsInitInfo = false;
    }

    protected override void RegSocketMessage()
    {
        CMsg_SC_CrossCardBattleInfor tSC_CrossCardBattleInfor = new CMsg_SC_CrossCardBattleInfor();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossCardBattleInfor, tSC_CrossCardBattleInfor, f_SC_CrossCardBattleInfoHandle);

        CMsg_SC_CrossCardBattleRandCard tSC_CrossCardBattleRandCard = new CMsg_SC_CrossCardBattleRandCard();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossCardBattleRandCard, tSC_CrossCardBattleRandCard, f_SC_CrossCardBattleRandCardHandle);

        CMsg_SC_CrossCardBattlePlayer tSC_CrossCardBattlePlayer = new CMsg_SC_CrossCardBattlePlayer();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossCardBattlePlayer, tSC_CrossCardBattlePlayer, f_SC_CrossCardBattlePlayerHandle);

        CMsg_SC_CrossCardBattleFormation tSC_CrossCardBattleFormation = new CMsg_SC_CrossCardBattleFormation();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossCardBattleFormation, tSC_CrossCardBattleFormation, f_SC_CrossCardBattleFormationHandle);

        CMsg_SC_CrossCardBattleResult tSC_CrossCardBattleResult = new CMsg_SC_CrossCardBattleResult();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossCardBattleResult, tSC_CrossCardBattleResult, f_SC_CrossCardBattleResultHandle);
    }

    #region 接受处理协议

    private void f_SC_CrossCardBattleInfoHandle(object value)
    {
        m_IsInitInfo = true;
        CMsg_SC_CrossCardBattleInfor serverData = (CMsg_SC_CrossCardBattleInfor)value;
        m_LeftChallengeTimes = serverData.iBattleTimes;
        m_LeftRefreshTeamTimes = serverData.iReflashTimes;
        m_WinTimes = serverData.iWinningTimes;
        m_LoseTimes = serverData.iLostingTimes;
    }

    private void f_SC_CrossCardBattleRandCardHandle(object value)
    {
        CMsg_SC_CrossCardBattleRandCard serverData = (CMsg_SC_CrossCardBattleRandCard)value;
        m_RefreshTeamTime = GameSocket.GetInstance().f_GetServerTime();
        f_Clear(); 
        for (int i = 0; i < serverData.aRandCardTempId.Length; i++)
        {
            f_Save(new CardBattleTeamPoolDT(i, serverData.aRandCardTempId[i])); 
        }
    }

    private void f_SC_CrossCardBattlePlayerHandle(object value)
    {
        m_RefreshEnemyTime = GameSocket.GetInstance().f_GetServerTime();
        CMsg_SC_CrossCardBattlePlayer serverData = (CMsg_SC_CrossCardBattlePlayer)value;
        for (int i = 0; i < serverData.aPlayer.Length; i++)
        {
            //if (serverData.aPlayer[i].userId == 0) continue;
            int[] clothArr = new int[serverData.aPlayer[i].aFormation.Length];
            for (int j = 0; j < clothArr.Length; j++)
            {
                clothArr[j] = serverData.aPlayer[i].aFormation[j];
            }           
            m_EnemyList.Add(new CardBattleEnemyPoolDT(serverData.aPlayer[i].userId, serverData.aPlayer[i].iServerId, serverData.aPlayer[i].szRoleName, clothArr));
        }
    }

    private void f_SC_CrossCardBattleFormationHandle(object value)
    {
        CMsg_SC_CrossCardBattleFormation serverData = (CMsg_SC_CrossCardBattleFormation)value;
        m_AtkClothList.Clear();
        m_DefClothList.Clear();
        for (int i = 0; i < serverData.aCardTempId.Length; i++)
        {
            m_AtkClothList.Add(new CardBattleClothPoolDT(i, serverData.aCardTempId[i]));
            m_DefClothList.Add(new CardBattleClothPoolDT(i, serverData.aCardTempId[i]));
        }
        m_SetDefClothTime = GameSocket.GetInstance().f_GetServerTime();
    }

    private void f_SC_CrossCardBattleResultHandle(object value)
    {
        CMsg_SC_CrossCardBattleResult serverData = (CMsg_SC_CrossCardBattleResult)value;
        IsFinish = true;
        Result = serverData.iResult;
        if (m_ChallengeSocketCallbackDt != null &&
            m_ChallengeSocketCallbackDt.m_ccCallbackSuc != null &&
            m_ChallengeSocketCallbackDt.m_ccCallbackFail != null)
        {
            if (serverData.iOperateResult == (int)eMsgOperateResult.OR_Succeed)
            {
                //更新战斗数据
                StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_CardBattle, 0, 0, 0);
                m_ChallengeSocketCallbackDt.m_ccCallbackSuc(serverData.iOperateResult);
            }
            else
            {
                m_ChallengeSocketCallbackDt.m_ccCallbackFail(serverData.iOperateResult);
            }
            m_ChallengeSocketCallbackDt = null;
        }
    }

    #endregion

    #region 发送协议

    private bool m_IsInitInfo = false;
    public void f_CardBattleInit(SocketCallbackDT socketCallbackDt)
    {
        if (m_IsInitInfo && socketCallbackDt != null && socketCallbackDt.m_ccCallbackSuc != null)
        {
            socketCallbackDt.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossCardBattleInfor, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(0);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossCardBattleInfor, bBuf);
    }

    public void f_RefreshCardTeam(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossCardRandCard, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(0);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossCardRandCard, bBuf); 
    }

    /// <summary>
    /// 设置攻击阵容 未实现
    /// </summary>
    /// <param name="clothArray"></param>
    /// <param name="socketCallbackDt"></param>
    public void f_SetAtkClothArray(int[] clothArray, SocketCallbackDT socketCallbackDt)
    {
        for (int i = 0; i < clothArray.Length; i++)
        {
            if (i < m_AtkClothList.Count)
            {
                m_AtkClothList[i].f_UpdateCard(clothArray[i]);
            }
        }
        ccTimeEvent.GetInstance().f_RegEvent(1.0f, false, eMsgOperateResult.OR_Succeed, socketCallbackDt.m_ccCallbackSuc);
    }

    public void f_SetDefClothArray(int[] clothArray, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossCardSaveFormation, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        for (int i = 0; i < clothArray.Length; i++)
        {
            tCreateSocketBuf.f_Add(clothArray[i]);
        }
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossCardSaveFormation, bBuf);
    }

    public void f_RefreshEnemyList(SocketCallbackDT socketCallbackDt)
    {
        m_EnemyList.Clear();
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossCardRandUser, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(0);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossCardRandUser, bBuf); 
    }

    private SocketCallbackDT m_ChallengeSocketCallbackDt;
    public void f_ChallengeEnemy(long id, SocketCallbackDT socketCallbackDt)
    {
        IsFinish = false;
        m_ChallengeSocketCallbackDt = socketCallbackDt;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossCardBattle, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossCardBattle, bBuf);
    }

    #endregion


    #region 对外接口

    /// <summary>
    /// 获取斗将活动的状态
    /// </summary>
    /// <returns></returns>
    public EM_CardBattleState f_GetState()
    {
        int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        int nowTime = GameSocket.GetInstance().f_GetServerTime();
        int nowTotalDays = ccMath.f_GetTotalDaysByTime(nowTime);
        if (m_CurNowTotalDays != nowTotalDays)
        {
            //重新计算当天的开始结束时间
            m_ApplyBeginTime = nowTime - ccMath.f_GetSecondOfDayByTime(nowTime) + m_ApplyTime[0] * 3600 + m_ApplyTime[1] * 60;
            m_ApplyEndTime = m_ApplyBeginTime + m_ApplyTime[2] * 60;
            m_BattleBeginTime = nowTime - ccMath.f_GetSecondOfDayByTime(nowTime) + m_BattleTime[0] * 3600 + m_BattleTime[1] * 60;
            m_BattleEndTime = m_BattleBeginTime + m_BattleTime[2] * 60;
            m_CurNowTotalDays = nowTotalDays;
        }
        //开服了多少天
        int openServerDay =  ccMath.f_GetTotalDaysByTime(nowTime) - ccMath.f_GetTotalDaysByTime(openServerTime);
        DayOfWeek dayOfWeek = ccMath.f_GetDayOfWeekByTime(nowTime);
        if (openServerDay < m_OpenDayLimit)
        {
            return EM_CardBattleState.Invalid;
        }
        for (int i = 0; i < m_OpenDayOfWeeks.Length; i++)
        {
            if(dayOfWeek == m_OpenDayOfWeeks[i])
            {
                if (nowTime >= m_ApplyBeginTime && nowTime <= m_ApplyEndTime)
                {
                    return EM_CardBattleState.InApply;
                }
                else if (nowTime > m_ApplyEndTime && nowTime < m_BattleBeginTime)
                {
                    return EM_CardBattleState.BetweenApplyBattle;
                }
                else if (nowTime >= m_BattleBeginTime && nowTime <= m_BattleEndTime)
                {
                    return EM_CardBattleState.InBattle;
                }
                else
                {
                    return EM_CardBattleState.Invalid;
                }
            }
        }
        return EM_CardBattleState.Invalid;
    }


    #endregion

    #region 无用
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        throw new NotImplementedException();
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        throw new NotImplementedException();
    }
    #endregion
}
