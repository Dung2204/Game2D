using ccU3DEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

public class EventTimePool : BasePool
{
    private List<NBaseSCDT> _EventTimeSCList;
    public Dictionary<int, List<NBaseSCDT>> _EventTimeDict = new Dictionary<int, List<NBaseSCDT>>();
    public Dictionary<int, bool> First_Req = new Dictionary<int, bool>();
    public EventTimePool():base("EventTimePoolDT")
    {
        


    }

    protected override void f_Init()
    {
       
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    protected override void RegSocketMessage()
    {
        EventTimeInfo EventTimeInfo = new EventTimeInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_EventTimeInfo, EventTimeInfo, f_EventTimeInfo);

        OpenSeverTime tOpenTime = new OpenSeverTime();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_OpenServTime, tOpenTime, InitEventTimeSCList);

        SC_RankingBattlePassUser tRC_RankingPowerUser = new SC_RankingBattlePassUser();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_RankingBattlePassList, tRC_RankingPowerUser, f_Callback_RankingPowerList);

        SC_RankingBattlePassMyRank tRC_SC_RankingPowerMyRank = new SC_RankingBattlePassMyRank();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_RankingBattlePassMyRank, tRC_SC_RankingPowerMyRank, f_Callback_RankingPowerMyRank);
    }

    public int OpenSeverTime = 0;
    public void InitEventTimeSCList(object obj)
    {

        OpenSeverTime = ((OpenSeverTime)obj).value1;

        int day = (GameSocket.GetInstance().f_GetServerTime() - OpenSeverTime) / (3600 * 24) + 1;
        _EventTimeSCList = new List<NBaseSCDT>();
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_EventTimeSC.f_GetAll().Count; i++)
        {
            EventTimeDT ttttttttttttt = glo_Main.GetInstance().m_SC_Pool.m_EventTimeSC.f_GetAll()[i] as EventTimeDT;
            switch (ttttttttttttt.iType)
            {
                case 0: break;
                case 1:
                    _EventTimeSCList.Add(ttttttttttttt);
                    break;
                case 2:
                    if (ttttttttttttt.iOpenTime <= day && day <= ttttttttttttt.iCloseTime)
                    {
                        _EventTimeSCList.Add(ttttttttttttt);
                    }
                    break;
                case 3:
                    bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime(ttttttttttttt.iOpenTime, ttttttttttttt.iCloseTime);
                    if (isOpen)
                    {
                        _EventTimeSCList.Add(ttttttttttttt);
                    }
                    break;
                case 4:
                    int createRoleTime = CommonTools.f_GetDayCreatAcc();
                    bool isOpen4 = createRoleTime >= ttttttttttttt.iOpenTime && createRoleTime <= ttttttttttttt.iEndTime;
                    if (isOpen4)
                    {
                        _EventTimeSCList.Add(ttttttttttttt);
                    }
                    break;
                case 5:
                    //mo theo pool tra ve
                    List<NBaseSCDT> temp_LevelGiftDTs = glo_Main.GetInstance().m_SC_Pool.m_LevelGiftSC.f_GetSCByEventTimeId(ttttttttttttt.iId);
                    for (int j = 0; j < temp_LevelGiftDTs.Count; j++)
                    {
                        LevelGiftDT levelGiftDT = (LevelGiftDT)temp_LevelGiftDTs[j];
                        EventTimePoolDT eventTimePoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(levelGiftDT.iEventTime, levelGiftDT.iId);
                        if (eventTimePoolDT == null) continue;
                        bool isOpen5 = GameSocket.GetInstance().f_GetServerTime() <= eventTimePoolDT.idata2 && eventTimePoolDT.idata3 < levelGiftDT.iMaxNum;
                        if (isOpen5)
                        {
                            if (!_EventTimeSCList.Any(o => o.iId == ttttttttttttt.iId))
                            {
                                _EventTimeSCList.Add(ttttttttttttt);
                                break;
                            };
                        }
                    }
                    break;
                default:

                    break;
            }
            AddListChildEvent(ttttttttttttt);
        }
    }

    public List<NBaseSCDT> GetEventTimeSCList()
    {      
        return _EventTimeSCList;
    }

    private void AddListChildEvent(EventTimeDT ttttttttttttt)
    {
        switch (ttttttttttttt.szNameConst)
        {
            case "OnlineVoteAppPage":
                List<NBaseSCDT> voteAppDTs = glo_Main.GetInstance().m_SC_Pool.m_VoteAppSC.f_GetSCByEventTimeId(ttttttttttttt.iId);

                if (_EventTimeDict.ContainsKey(ttttttttttttt.iId))
                {
                    _EventTimeDict[ttttttttttttt.iId] = voteAppDTs;
                }
                else
                {
                    _EventTimeDict.Add(ttttttttttttt.iId, voteAppDTs);
                }
                for (int i = 0; i < voteAppDTs.Count; i++)
                {
                    int key = InitKey(ttttttttttttt.iId, voteAppDTs[i].iId);
                    EventTimePoolDT tPoolDataDT = (EventTimePoolDT)f_GetForId(key);

                    if (tPoolDataDT == null)
                    {
                        tPoolDataDT = new EventTimePoolDT();
                        tPoolDataDT.iId = key;
                        tPoolDataDT.IEventTimeId = ttttttttttttt.iId;
                        tPoolDataDT.IEventId = voteAppDTs[i].iId;

                        f_Save(tPoolDataDT);
                    }
                    else
                    {

                    }
                }
                break;
            case "RankingPowerPage":
                List<NBaseSCDT> RankingPowerAwardDTs = glo_Main.GetInstance().m_SC_Pool.m_RankingPowerAwardSC.f_GetSCByEventTimeId(ttttttttttttt.iId);
                if (_EventTimeDict.ContainsKey(ttttttttttttt.iId))
                {
                    _EventTimeDict[ttttttttttttt.iId] = RankingPowerAwardDTs;
                }
                else
                {
                    _EventTimeDict.Add(ttttttttttttt.iId, RankingPowerAwardDTs);
                }
                for (int i = 0; i < RankingPowerAwardDTs.Count; i++)
                {
                    int key = InitKey(ttttttttttttt.iId, RankingPowerAwardDTs[i].iId);
                    EventTimePoolDT tPoolDataDT = (EventTimePoolDT)f_GetForId(key);

                    if (tPoolDataDT == null)
                    {
                        tPoolDataDT = new EventTimePoolDT();
                        tPoolDataDT.iId = key;
                        tPoolDataDT.IEventTimeId = ttttttttttttt.iId;
                        tPoolDataDT.IEventId = RankingPowerAwardDTs[i].iId;

                        f_Save(tPoolDataDT);
                    }
                    else
                    {

                    }
                }
                break;
            case "TripleMoneyPage":
                List<NBaseSCDT> TripleMoneyDTs = glo_Main.GetInstance().m_SC_Pool.m_TripleMoneySC.f_GetSCByEventTimeId(ttttttttttttt.iId);
                if (_EventTimeDict.ContainsKey(ttttttttttttt.iId))
                {
                    _EventTimeDict[ttttttttttttt.iId] = TripleMoneyDTs;
                }
                else
                {
                    _EventTimeDict.Add(ttttttttttttt.iId, TripleMoneyDTs);
                }
                for (int i = 0; i < TripleMoneyDTs.Count; i++)
                {
                    int key = InitKey(ttttttttttttt.iId, TripleMoneyDTs[i].iId);
                    EventTimePoolDT tPoolDataDT = (EventTimePoolDT)f_GetForId(key);

                    if (tPoolDataDT == null)
                    {
                        tPoolDataDT = new EventTimePoolDT();
                        tPoolDataDT.iId = key;
                        tPoolDataDT.IEventTimeId = ttttttttttttt.iId;
                        tPoolDataDT.IEventId = TripleMoneyDTs[i].iId;

                        f_Save(tPoolDataDT);
                    }
                    else
                    {

                    }
                }
                break;
            case "LevelGiftPage":
                List<NBaseSCDT> LevelGiftDTs = glo_Main.GetInstance().m_SC_Pool.m_LevelGiftSC.f_GetSCByEventTimeId(ttttttttttttt.iId);
                if (_EventTimeDict.ContainsKey(ttttttttttttt.iId))
                {
                    _EventTimeDict[ttttttttttttt.iId] = LevelGiftDTs;
                }
                else
                {
                    _EventTimeDict.Add(ttttttttttttt.iId, LevelGiftDTs);
                }
                for (int i = 0; i < LevelGiftDTs.Count; i++)
                {
                    int key = InitKey(ttttttttttttt.iId, LevelGiftDTs[i].iId);
                    EventTimePoolDT tPoolDataDT = (EventTimePoolDT)f_GetForId(key);

                    if (tPoolDataDT == null)
                    {
                        tPoolDataDT = new EventTimePoolDT();
                        tPoolDataDT.iId = key;
                        tPoolDataDT.IEventTimeId = ttttttttttttt.iId;
                        tPoolDataDT.IEventId = LevelGiftDTs[i].iId;

                        f_Save(tPoolDataDT);
                    }
                    else
                    {

                    }
                }
                break;
            case "BattlePassPage":
                List<NBaseSCDT> BattlePassTaskDTs = glo_Main.GetInstance().m_SC_Pool.m_BattlePassTaskSC.f_GetAll();
                if (_EventTimeDict.ContainsKey(ttttttttttttt.iId))
                {
                    _EventTimeDict[ttttttttttttt.iId] = BattlePassTaskDTs;
                }
                else
                {
                    _EventTimeDict.Add(ttttttttttttt.iId, BattlePassTaskDTs);
                }
                for (int i = 0; i < BattlePassTaskDTs.Count; i++)
                {
                    int key = InitKey(ttttttttttttt.iId, BattlePassTaskDTs[i].iId);
                    EventTimePoolDT tPoolDataDT = (EventTimePoolDT)f_GetForId(key);

                    if (tPoolDataDT == null)
                    {
                        tPoolDataDT = new EventTimePoolDT();
                        tPoolDataDT.iId = key;
                        tPoolDataDT.IEventTimeId = ttttttttttttt.iId;
                        tPoolDataDT.IEventId = BattlePassTaskDTs[i].iId;

                        f_Save(tPoolDataDT);
                    }
                    else
                    {

                    }
                }
                break;
            case "RankingGodEquipPage":
                List<NBaseSCDT> RankingGodEquipAwardDTs = glo_Main.GetInstance().m_SC_Pool.m_RankingPowerAwardSC.f_GetSCByEventTimeId(ttttttttttttt.iId);
                if (_EventTimeDict.ContainsKey(ttttttttttttt.iId))
                {
                    _EventTimeDict[ttttttttttttt.iId] = RankingGodEquipAwardDTs;
                }
                else
                {
                    _EventTimeDict.Add(ttttttttttttt.iId, RankingGodEquipAwardDTs);
                }
                for (int i = 0; i < RankingGodEquipAwardDTs.Count; i++)
                {
                    int key = InitKey(ttttttttttttt.iId, RankingGodEquipAwardDTs[i].iId);
                    EventTimePoolDT tPoolDataDT = (EventTimePoolDT)f_GetForId(key);

                    if (tPoolDataDT == null)
                    {
                        tPoolDataDT = new EventTimePoolDT();
                        tPoolDataDT.iId = key;
                        tPoolDataDT.IEventTimeId = ttttttttttttt.iId;
                        tPoolDataDT.IEventId = RankingGodEquipAwardDTs[i].iId;

                        f_Save(tPoolDataDT);
                    }
                    else
                    {

                    }
                }
                break;
            case "RankingTariningPage":
                List<NBaseSCDT> RankingTariningAwardDTs = glo_Main.GetInstance().m_SC_Pool.m_RankingPowerAwardSC.f_GetSCByEventTimeId(ttttttttttttt.iId);
                if (_EventTimeDict.ContainsKey(ttttttttttttt.iId))
                {
                    _EventTimeDict[ttttttttttttt.iId] = RankingTariningAwardDTs;
                }
                else
                {
                    _EventTimeDict.Add(ttttttttttttt.iId, RankingTariningAwardDTs);
                }
                for (int i = 0; i < RankingTariningAwardDTs.Count; i++)
                {
                    int key = InitKey(ttttttttttttt.iId, RankingTariningAwardDTs[i].iId);
                    EventTimePoolDT tPoolDataDT = (EventTimePoolDT)f_GetForId(key);

                    if (tPoolDataDT == null)
                    {
                        tPoolDataDT = new EventTimePoolDT();
                        tPoolDataDT.iId = key;
                        tPoolDataDT.IEventTimeId = ttttttttttttt.iId;
                        tPoolDataDT.IEventId = RankingTariningAwardDTs[i].iId;

                        f_Save(tPoolDataDT);
                    }
                    else
                    {

                    }
                }
                break;
            case "LotteryLimitEventPage":
                List<NBaseSCDT> LotteryLimitEvents = glo_Main.GetInstance().m_SC_Pool.m_LotteryLimitEventSC.f_GetSCByEventTimeId(ttttttttttttt.iId);
                if (_EventTimeDict.ContainsKey(ttttttttttttt.iId))
                {
                    _EventTimeDict[ttttttttttttt.iId] = LotteryLimitEvents;
                }
                else
                {
                    _EventTimeDict.Add(ttttttttttttt.iId, LotteryLimitEvents);
                }
                for (int i = 0; i < LotteryLimitEvents.Count; i++)
                {
                    int key = InitKey(ttttttttttttt.iId, LotteryLimitEvents[i].iId);
                    EventTimePoolDT tPoolDataDT = (EventTimePoolDT)f_GetForId(key);

                    if (tPoolDataDT == null)
                    {
                        tPoolDataDT = new EventTimePoolDT();
                        tPoolDataDT.iId = key;
                        tPoolDataDT.IEventTimeId = ttttttttttttt.iId;
                        tPoolDataDT.IEventId = LotteryLimitEvents[i].iId;

                        f_Save(tPoolDataDT);
                    }
                    else
                    {

                    }
                }
                break;
        }
    }

    public void f_GetAward(int iIdEventTime, int iIdVoteApp, SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetVoteAppAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iIdEventTime);
        tCreateSocketBuf.f_Add(iIdVoteApp);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetVoteAppAward, bBuf);
    }

    private void f_EventTimeInfo(object obj)
    {
        MessageBox.ASSERT("SIGN POOL f_EventTimeInfo");
        EventTimeInfo EventTimeInfo = (EventTimeInfo)obj;
        int key = InitKey(EventTimeInfo.ieventtime, EventTimeInfo.ievent);

        EventTimePoolDT tPoolDataDT = (EventTimePoolDT)f_GetForId(key);
        if(tPoolDataDT == null)
        {
            tPoolDataDT = new EventTimePoolDT();
            tPoolDataDT.iId = key;
            tPoolDataDT.IEventTimeId = EventTimeInfo.ieventtime;
            tPoolDataDT.IEventId = EventTimeInfo.ievent;

            tPoolDataDT.idata1 = EventTimeInfo.idata1;
            tPoolDataDT.idata2 = EventTimeInfo.idata2;
            tPoolDataDT.idata3 = EventTimeInfo.idata3;
            tPoolDataDT.idata4 = EventTimeInfo.idata4;
            f_Save(tPoolDataDT);
        }
        else
        {
            tPoolDataDT.idata1 = EventTimeInfo.idata1;
            tPoolDataDT.idata2 = EventTimeInfo.idata2;
            tPoolDataDT.idata3 = EventTimeInfo.idata3;
            tPoolDataDT.idata4 = EventTimeInfo.idata4;
        }
        f_CheckEventAcctive(tPoolDataDT);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_VOTE_APP_AWARD);
        if(f_CheckOpenEventTime(EventTimeInfo.ieventtime))
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_EVENT_TIME, EventTimeInfo.ieventtime);

    }

    private void f_CheckEventAcctive(EventTimePoolDT eventTimePoolDT)
    {
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_EventTimeSC.f_GetAll().Count; i++)
        {
            EventTimeDT ttttttttttttt = glo_Main.GetInstance().m_SC_Pool.m_EventTimeSC.f_GetAll()[i] as EventTimeDT;
            switch (ttttttttttttt.szNameConst)
            {
                case "LevelGiftPage":
                    if(ttttttttttttt.iId == eventTimePoolDT.IEventTimeId)
                    {

                        LevelGiftDT LevelGiftDT = (LevelGiftDT)glo_Main.GetInstance().m_SC_Pool.m_LevelGiftSC.f_GetSC(eventTimePoolDT.IEventId);
                        bool isOpen5 = GameSocket.GetInstance().f_GetServerTime() <= eventTimePoolDT.idata2 && eventTimePoolDT.idata3 < LevelGiftDT.iMaxNum;
                        if (isOpen5)
                        {
                            if (!_EventTimeSCList.Any(o => o.iId == ttttttttttttt.iId))
                            {
                                _EventTimeSCList.Add(ttttttttttttt);
                                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_OPEN_EVENT_TIME, eventTimePoolDT.IEventTimeId);
                            };
                            UITool.CreatLocalDataLevelGift(LevelGiftDT.iId, true);
                        }
                        else
                        {
                            UITool.CreatLocalDataLevelGift(LevelGiftDT.iId, false);
                        }
                    }                   
                    break;
                default:

                    break;
            }
        }
    }

    public bool f_CheckOpenEventTime(int IEventTimeId, int lev = 1)
    {
        List<NBaseSCDT> nBaseSCDTs;
        if (_EventTimeDict.ContainsKey(IEventTimeId))
        {
            if( Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < lev)
            {
                return false;
            }

            nBaseSCDTs = _EventTimeDict[IEventTimeId];
            bool check = true;
            for (int i = 0; i < nBaseSCDTs.Count; i++)
            {
                int key = InitKey(IEventTimeId, nBaseSCDTs[i].iId);
                EventTimePoolDT tPoolDataDT = (EventTimePoolDT)f_GetForId(key);
                if (tPoolDataDT == null)
                {
                    return true;                  
                }
                else
                {
                    EventTimeDT EventTimeDT = (EventTimeDT)glo_Main.GetInstance().m_SC_Pool.m_EventTimeSC.f_GetSC(IEventTimeId);
                    if (EventTimeDT.szNameConst == "OnlineVoteAppPage")
                    {
                        check = tPoolDataDT.idata1 > 0;
                    }
                    else if (EventTimeDT.szNameConst == "TripleMoneyPage")
                    {
                        TripleMoneyDT TripleMoneyDT = (TripleMoneyDT)glo_Main.GetInstance().m_SC_Pool.m_TripleMoneySC.f_GetSC(nBaseSCDTs[i].iId);

                        if (tPoolDataDT.idata1 < TripleMoneyDT.iMaxNum)
                        {
                            check = false;
                        }
                    }
                    else if (EventTimeDT.szNameConst == "RankingPowerPage" || EventTimeDT.szNameConst == "RankingGodEquipPage" || EventTimeDT.szNameConst == "RankingTariningPage")
                    {
                        check = false;
                    }
                    else if (EventTimeDT.szNameConst == "LevelGiftPage")
                    {
                        LevelGiftDT LevelGiftDT = (LevelGiftDT)glo_Main.GetInstance().m_SC_Pool.m_LevelGiftSC.f_GetSC(nBaseSCDTs[i].iId);
                        bool isOpen4 = GameSocket.GetInstance().f_GetServerTime() <= tPoolDataDT.idata2 && tPoolDataDT.idata3 < LevelGiftDT.iMaxNum;

                        if (isOpen4)
                        {
                            return check = false;
                        }
                    }
                    else if (EventTimeDT.szNameConst == "BattlePassPage")
                    {
                        check = false;
                    }
                    else if (EventTimeDT.szNameConst == "LotteryLimitEventPage")
                    {
                        bool isOpen5 = GameSocket.GetInstance().f_GetServerTime() >= tPoolDataDT.idata1 && GameSocket.GetInstance().f_GetServerTime() < tPoolDataDT.idata2;

                        if (isOpen5)
                        {
                            return check = false;
                        }
                    }
                }
            }
            return check;
        }
        return true;
    }

    private int InitKey(int idA, int idB)
    {
        string key = idA + "" + idB;
        return int.Parse(key);
    }

    public void f_TripleMoneyPool(int iIdEventTime, SocketCallbackDT tSocketCallbackDT)
    {
        if (First_Req.ContainsKey(iIdEventTime))
        {
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        First_Req[iIdEventTime] = true;

        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetTripleMoney, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iIdEventTime);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetTripleMoney, bBuf);
    }

    public void f_LevelGiftPool(int iIdEventTime, SocketCallbackDT tSocketCallbackDT)
    {
        if (First_Req.ContainsKey(iIdEventTime))
        {
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        First_Req[iIdEventTime] = true;

        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetLevelGift, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iIdEventTime);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetLevelGift, bBuf);
    }

    public BasePoolDT<long> f_EventTimePoolDT(int iIdEventTime, int iId)
    {
        int key = InitKey(iIdEventTime, iId);
        return f_GetForId(key);
    }

    public bool CheckReddotBattlePassTask(int iIdEventTime)
    {
        List<NBaseSCDT> temp_BattlePassDTs = glo_Main.GetInstance().m_SC_Pool.m_BattlePassTaskSC.f_GetAll();
        for (int i = 0; i < temp_BattlePassDTs.Count; i++)
        {
            BattlePassTaskDT node = (BattlePassTaskDT)temp_BattlePassDTs[i];
            EventTimePoolDT eventTimePoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(iIdEventTime, node.iId);
            int numiConditionParam = node.iConditionParam1;
            switch (eventTimePoolDT.idata2)
            {
                case 0:
                    numiConditionParam = node.iConditionParam1;
                    break;
                case 1:
                    numiConditionParam = node.iConditionParam2;
                    break;
                case 2:
                    numiConditionParam = node.iConditionParam3;
                    break;
                case 3:
                    numiConditionParam = node.iConditionParam4;
                    break;
                case 4:
                    numiConditionParam = node.iConditionParam5;
                    break;
                case 5:
                    numiConditionParam = node.iConditionParam6;
                    break;
                case 6:
                    numiConditionParam = node.iConditionParam7;
                    break;
                case 7:
                    numiConditionParam = node.iConditionParam7;
                    break;
            }
            if(eventTimePoolDT.idata1 >= numiConditionParam && eventTimePoolDT.idata2 < 7)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckReddotBattlePassAward(int iIdEventTime, int Idinfo)
    {
        List<NBaseSCDT> temp_BattlePassAwardDTs = glo_Main.GetInstance().m_SC_Pool.m_BattlePassAwardSC.f_GetAll();
        EventTimePoolDT eventTimeInfoPoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(iIdEventTime, Idinfo);
        if(eventTimeInfoPoolDT == null || temp_BattlePassAwardDTs == null)
        {
            return false;
        }
        for (int i = 0; i < temp_BattlePassAwardDTs.Count; i++)
        {
            BattlePassAwardDT battlePassAwardDT = (BattlePassAwardDT)temp_BattlePassAwardDTs[i];
            if (battlePassAwardDT.iLevel > eventTimeInfoPoolDT.idata2 && battlePassAwardDT.iScore <= eventTimeInfoPoolDT.idata1)
            {
                return true;
            }
            if (eventTimeInfoPoolDT.idata3 > 0 && battlePassAwardDT.iLevel > eventTimeInfoPoolDT.idata4 && battlePassAwardDT.iScore <= eventTimeInfoPoolDT.idata1)
            {
                return true;
            }
        }
        return false;
    }

    public void f_GetBattlePassTaskAward(int iIdEventTime,int iIdVoteApp, SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetBattlePassTaskAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iIdEventTime);
        tCreateSocketBuf.f_Add(iIdVoteApp);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetBattlePassTaskAward, bBuf);
    }

    public void f_BattlePassInfoPool(int iIdEventTime, SocketCallbackDT tSocketCallbackDT)
    {
        if (First_Req.ContainsKey(iIdEventTime))
        {
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        First_Req[iIdEventTime] = true;

        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetBattlePassInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iIdEventTime);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetBattlePassInfo, bBuf);
    }

    public void f_GetBattlePassAward(int iIdEventTime, SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetBattlePassAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iIdEventTime);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetBattlePassAward, bBuf);
    }

    private const int RANK_LIST_PAGE_MAX = 50;
    Dictionary<int, List<BasePoolDT<long>>> m_RankList = new Dictionary<int, List<BasePoolDT<long>>>();
    Dictionary<int, QueryBattlePassRank> m_QueryBattleRank = new Dictionary<int, QueryBattlePassRank>();

    public void f_GetBattlePassRankList(int iIdEventTime, SocketCallbackDT tSocketCallbackDT)
    {

        if (m_QueryBattleRank.ContainsKey(iIdEventTime))
        {
            if(GameSocket.GetInstance().f_GetServerTime() >= m_QueryBattleRank[iIdEventTime].times + 1800)
            {
                if (m_RankList.ContainsKey(iIdEventTime))
                {
                    m_RankList[iIdEventTime].Clear();
                }
            }else if (m_RankList.ContainsKey(iIdEventTime) && m_RankList[iIdEventTime].Count >= RANK_LIST_PAGE_MAX 
                   || m_RankList.ContainsKey(iIdEventTime) && m_RankList[iIdEventTime].Count <= m_QueryBattleRank[iIdEventTime].count)
            {
                if (tSocketCallbackDT != null)
                    tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
                return;
            }
        }

        int count = 0;
        if (m_RankList.ContainsKey(iIdEventTime))
        {
            count = m_RankList[iIdEventTime].Count;
        }
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RankingBattlePassList, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();

        QueryBattlePassRank queryBattlePassRank = new QueryBattlePassRank();
        queryBattlePassRank.count = count;
        queryBattlePassRank.times = GameSocket.GetInstance().f_GetServerTime();
        m_QueryBattleRank[iIdEventTime] = queryBattlePassRank;
        if (count < RANK_LIST_PAGE_MAX)
        {         
            tCreateSocketBuf.f_Add(count);
            tCreateSocketBuf.f_Add(iIdEventTime);
            byte[] bBuf = tCreateSocketBuf.f_GetBuf();
            GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RankingBattlePassList, bBuf);
        }
    }

    private void f_Callback_RankingPowerList(int iData1, int iData2, int iNum, ArrayList aData)
    {
        List<BasePoolDT<long>> RankList = new List<BasePoolDT<long>>();
        if (m_RankList.ContainsKey(iData1))
        {
            RankList = m_RankList[iData1];
        }
        for (int i = 0; i < aData.Count; i++)
        {
            SC_RankingBattlePassUser tUpdateNode = (SC_RankingBattlePassUser)aData[i];

            RankingPowerAwardPoolDT lvRankItem = new RankingPowerAwardPoolDT();
            lvRankItem.f_UpdateInfo(tUpdateNode.rank, tUpdateNode.ift, tUpdateNode.szRoleName);
            RankList.Add(lvRankItem);
        }
        m_RankList[iData1] = RankList;
    }

    Dictionary<int, SC_RankingBattlePassMyRank> RankingBattlePassMyRank = new Dictionary<int, SC_RankingBattlePassMyRank>();
    private void f_Callback_RankingPowerMyRank(object aData)
    {
        SC_RankingBattlePassMyRank tRC_SC_RankingPowerMyRank = (SC_RankingBattlePassMyRank)aData;
        RankingBattlePassMyRank[tRC_SC_RankingPowerMyRank.eventtimeId] = tRC_SC_RankingPowerMyRank;
    }

    public List<BasePoolDT<long>> f_GetRankList(int iIdEventTime)
    {
        return m_RankList[iIdEventTime];
    }

    public int f_GetMyRank(int iIdEventTime)
    {
        if (RankingBattlePassMyRank.ContainsKey(iIdEventTime))
        {
            return RankingBattlePassMyRank[iIdEventTime].rank;
        }
        return -1;
    }

    public void ClearListRank(int iIdEventTime)
    {
        if (m_RankList.ContainsKey(iIdEventTime))
        {
            m_RankList[iIdEventTime].Clear();
        }
    }

    public static bool ReddotAwardBattlePass()
    {

        return false;
    }

}

public class QueryBattlePassRank
{
    public int count;
    public int times;
}
