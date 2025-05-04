using ccU3DEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PatrolPool : BasePool
{
    private PatrolPoolDT selfPatrolDt;
    public PatrolPoolDT m_SelfPatrolDt
    {
        get
        {
            return selfPatrolDt;
        }
    }


    private int pacifyTimes;
    public int m_iPacifyTimes
    {
        get
        {
            return pacifyTimes;
        }
    }

    /// <summary>
    /// 镇压暴动限制
    /// </summary>
    public int m_iPacifyTimesLimit
    {
        get
        {
            return 20;
        }
    }

    /// <summary>
    /// 战斗是否完成
    /// </summary>
    public bool m_bIsFinish
    {
        private set;
        get;
    }

    /// <summary>
    /// 战斗结算结果
    /// </summary>
    public int m_iIsWin
    {
        private set;
        get;
    }

    public PatrolPool() : base("PatrolPoolDT", false)
    {
        
    }

    protected override void f_Init()
    {
        pacifyTimes = 0;
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, f_UpdateDataByNextDay);
    }

    //跨天重置
    private void f_UpdateDataByNextDay(object result)
    {
        pacifyTimes = 0; 
    }

    protected override void RegSocketMessage()
    {
        CMsg_SC_PatrolInitNode tSC_InitNode = new CMsg_SC_PatrolInitNode();
        GameSocket.GetInstance().f_RegMessage_Int2((int)SocketCommand.SC_PatrolInit, tSC_InitNode, f_GTC_PatrolInit_Handle);

        CMsg_SC_PatrolChallengeRet  tSC_ChallengeRet = new CMsg_SC_PatrolChallengeRet();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_PatrolChallengeRet, tSC_ChallengeRet, f_GTC_ChallengeRet_Handle);

        CMsg_SC_PatrolPacify tSC_PatrolPacify = new CMsg_SC_PatrolPacify();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_PatrolPacify, tSC_PatrolPacify, f_GTC_PatrolPacify_Handle);

        CMsg_SC_PatrolEventInfoNode tSC_EventInfoNode = new CMsg_SC_PatrolEventInfoNode();
        GameSocket.GetInstance().f_RegMessage_Int3((int)SocketCommand.SC_PatrolEvent, tSC_EventInfoNode, f_GTC_PatrolEventInfo_Handle);

        //一键镇压返回的协议 SocketCommand.SC_PatrolPacifyOnekey

        //好友领地 （请求通过关系服发送） 游服接受
        CMsg_RC_FriendPatrolNode tRC_FriendPatrolNode = new CMsg_RC_FriendPatrolNode();
        GameSocket.GetInstance().f_RegMessage_Int2((int)ChatSocketCommand.RC_FriendsPatrol, tRC_FriendPatrolNode, f_GTC_FriendPatrol_Handle);
    }

    #region 接受协议

    private void f_GTC_PatrolInit_Handle(int value1,int value2,int num,ArrayList aData)
    {
        long userId = CommonTools.f_TwoInt2Long((uint)value1, (uint)value2);
        //CMsg_SC_PatrolInitNode serverNodeData;
        //自己
        if (userId == Data_Pool.m_UserData.m_iUserId)
        {
            if (selfPatrolDt == null)
            {
                selfPatrolDt = new PatrolPoolDT(userId);
                //初始化技能
                landSkills = new PatrolSkillNode[selfPatrolDt.m_PatrolLands.Length];
                for (int i = 0; i < landSkills.Length; i++)
                {
                    int lvMax = f_GetSkillLvMax(selfPatrolDt.m_PatrolLands[i].m_iTemplateId);
                    landSkills[i] = new PatrolSkillNode(selfPatrolDt.m_PatrolLands[i].m_Template,lvMax);
                }               
            }    
            for (int i = 0; i < aData.Count; i++)
            {
                CMsg_SC_PatrolInitNode serverData = (CMsg_SC_PatrolInitNode)aData[i];
                selfPatrolDt.f_UpdateLandInfo(serverData.landId, serverData.lv, serverData.totalHours, serverData.cardId,serverData.patrolId,serverData.beginTime, serverData.isRiot,serverData.isAward);
                PatrolLandNode tNode = selfPatrolDt.f_GetPatrolLandNode(serverData.landId);
                f_UpdateSkillNode(serverData.landId,tNode.m_State, tNode.m_iLv, tNode.m_iTotalHours);
            }
        }
        //其他人
        else
        {
            PatrolPoolDT tNode = (PatrolPoolDT)f_GetForId(userId);
            if (tNode == null)
            {
                tNode = new PatrolPoolDT(userId);
                f_Save(tNode);
            }
            for (int i = 0; i < aData.Count; i++)
            {
                CMsg_SC_PatrolInitNode serverData = (CMsg_SC_PatrolInitNode)aData[i];
                tNode.f_UpdateLandInfo(serverData.landId, serverData.lv, serverData.totalHours, serverData.cardId, serverData.patrolId, serverData.beginTime, serverData.isRiot,serverData.isAward);
            }
            
        }
    }

    private void f_GTC_ChallengeRet_Handle(object value)
    {
        CMsg_SC_PatrolChallengeRet serverData = (CMsg_SC_PatrolChallengeRet)value;
        m_bIsFinish = true;
        m_iIsWin = serverData.isWin;
    }

    private void f_GTC_PatrolPacify_Handle(object value)
    {
        CMsg_SC_PatrolPacify serverData = (CMsg_SC_PatrolPacify)value;
        pacifyTimes = serverData.todayTimes;
    }

    public void f_GTC_PatrolEventInfo_Handle(int value1,int value2,int value3,int num,ArrayList aData)
    {
        long userId = CommonTools.f_TwoInt2Long((uint)value1, (uint)value2);
        int landId = value3;
        if (userId == Data_Pool.m_UserData.m_iUserId)
        {
            if (selfPatrolDt == null)
MessageBox.ASSERT("An error occurred with the Patrol event message");
            for (int i = 0; i < aData.Count; i++)
            {
                CMsg_SC_PatrolEventInfoNode serverData = (CMsg_SC_PatrolEventInfoNode)aData[i];
                selfPatrolDt.f_UpdateLandEventInfo(landId,serverData); 
            }
        }
        else
        {
            PatrolPoolDT tNode = (PatrolPoolDT)f_GetForId(userId);
            if (tNode == null)
MessageBox.ASSERT("An error occurred with the Patrol event message");
            for (int i = 0; i < aData.Count; i++)
            {
                CMsg_SC_PatrolEventInfoNode serverData = (CMsg_SC_PatrolEventInfoNode)aData[i];
                tNode.f_UpdateLandEventInfo(landId, serverData);
            } 
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_PATROLEVENT_UPDATE);
    }

    public void f_GTC_FriendPatrol_Handle(int value1, int value2, int num, ArrayList aData)
    {
        friendInfoRequestTime = GameSocket.GetInstance().f_GetServerTime();
        //处理好友巡逻信息
        for (int i = 0; i < aData.Count; i++)
        {
            CMsg_RC_FriendPatrolNode serverData = (CMsg_RC_FriendPatrolNode)aData[i];
            PatrolFriendInfoPoolDT tInfo = new PatrolFriendInfoPoolDT(serverData.userId,serverData.landNum,serverData.patrolNum,serverData.riotNum);
            friendInfoList.Add(tInfo);
        }
        friendInfoList.Sort(delegate (BasePoolDT<long> node1, BasePoolDT<long> node2)
        {
            PatrolFriendInfoPoolDT tItem1 = (PatrolFriendInfoPoolDT)node1;
            PatrolFriendInfoPoolDT tItem2 = (PatrolFriendInfoPoolDT)node2;
            int riotNum = tItem2.m_iRiotingNum.CompareTo(tItem1.m_iRiotingNum);
            if (riotNum != 0)
                return riotNum;
            if (tItem1.m_PlayerInfo.m_iOfflineTime == 0 && tItem2.m_PlayerInfo.m_iOfflineTime != 0)
                return -1;
            else if (tItem1.m_PlayerInfo.m_iOfflineTime != 0 && tItem2.m_PlayerInfo.m_iOfflineTime == 0)
                return 1;
            return tItem2.m_PlayerInfo.m_iOfflineTime.CompareTo(tItem1.m_PlayerInfo.m_iOfflineTime);
        }
        );
    }

    #endregion

    #region 发送协议

    /// <summary>
    /// 初始化巡逻数据
    /// </summary>
    /// <param name="userId">玩家Id(如果是自己就填0)</param>
    /// <param name="socketCallbackDt"></param>
    public void f_PatrolInit(long userId,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PatrolInit, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(userId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_PatrolInit, bBuf);
    }

    public void f_PatrolChallenge(int landId,SocketCallbackDT socketCallbackDt)
    {
        //更新战斗数据
        StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_Patrol, 0, landId, 0);

        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PatrolChallenge, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(landId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_PatrolChallenge, bBuf);
    }

    public void f_PatrolEvent(long friendId,int landId,byte eventNum,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PatrolEvent, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(friendId);
        tCreateSocketBuf.f_Add(landId);
        tCreateSocketBuf.f_Add(eventNum);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_PatrolEvent, bBuf);
    }

    public void f_PatrolBegin(int landId,int cardId,int patrolId,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PatrolBegin, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(landId);
        tCreateSocketBuf.f_Add(cardId);
        tCreateSocketBuf.f_Add(patrolId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_PatrolBegin, bBuf);
    }

    public void f_PatrolPacify(long friendId,int landId,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PatrolPacify, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(friendId);
        tCreateSocketBuf.f_Add(landId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_PatrolPacify, bBuf);
    }

    public void f_PatrolUpgrade(int landId,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PatrolUpgrade, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(landId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_PatrolUpgrade, bBuf);
    }

    public void f_PatrolEventEx(long friendId,byte[] eventNums, SocketCallbackDT socketCallbackDt)
    {
        if (eventNums.Length != 6)
MessageBox.ASSERT("Quick patrol parameter error");
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PatrolEventEx, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(friendId);
        for (int i = 0; i < eventNums.Length; i++)
        {
            tCreateSocketBuf.f_Add(eventNums[i]);
        }
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_PatrolEventEx, bBuf);
    }

    /// <summary>
    /// 一键镇压暴动(未做)
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_PatrolPacifyOneKey(SocketCallbackDT socketCallbackDt)
    {
        //比较特殊  关系服发送
        ChatSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PatrolPacifyOnekey, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(SocketCommand.CS_PatrolPacifyOnekey, bBuf);
    }

    /// <summary>
    /// 领取奖励
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_PatrolAward(int landId,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PatrolAward, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(landId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_PatrolAward, bBuf);
    }

    //关系服发送，游服返回
    public void f_PatrolFriendInfo(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_FriendsPatrol, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_FriendsPatrol, bBuf);
    }

    #endregion

    public PatrolLandSkillDT f_GetLandSkillTemplate(int landId, int lv)
    {
        return (PatrolLandSkillDT)glo_Main.GetInstance().m_SC_Pool.m_PatrolLandSkillSC.f_GetAll().Find(delegate (NBaseSCDT node)
        {
            PatrolLandSkillDT tItem = (PatrolLandSkillDT)node;
            return tItem.iLandId == landId && tItem.iLv == lv;
        });
    }

    /// <summary>
    /// 根据类型获得巡逻类型相关数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<NBaseSCDT> f_GetPatrolTypeByType(int type)
    {
        return glo_Main.GetInstance().m_SC_Pool.m_PatrolTypeSC.f_GetAll().FindAll(delegate (NBaseSCDT node)
        {
            PatrolTypeDT tItem = (PatrolTypeDT)node;
            return tItem.iType == type;
        });
    }

    public PatrolTypeDT f_GetPatrolTypeByTypeAndTime(int type, int time)
    {
        return  (PatrolTypeDT)glo_Main.GetInstance().m_SC_Pool.m_PatrolTypeSC.f_GetAll().Find(delegate (NBaseSCDT node)
        {
            PatrolTypeDT tItem = (PatrolTypeDT)node;
            return tItem.iType == type && tItem.iTime == time;
        });
    }

    private int landTotalNum = 0;
    /// <summary>
    /// 领地数目
    /// </summary>
    public int m_iLandTotalNum
    {
        get
        {
            return landTotalNum;
        }
    }

    /// <summary>
    /// 刷新领地总数
    /// </summary>
    /// <param name="landTotalNum"></param>
    public void f_UpdateLandTotalNum(int landTotalNum)
    {
        if (this.landTotalNum == 0)
            this.landTotalNum = landTotalNum;
    }

    public void f_RequestEventByServer(long friendId,int landId,byte eventNum,ccCallback callbackHandle)
    {
        if (friendId == Data_Pool.m_UserData.m_iUserId)
            friendId = 0;
        int now = GameSocket.GetInstance().f_GetServerTime();
        if (!eventSendTime.ContainsKey(friendId))
        {
            eventSendTime.Add(friendId, new Dictionary<int, int>());
            eventSendTime[friendId].Add(landId, 0);
        }
        if (!eventSendTime[friendId].ContainsKey(landId))
            eventSendTime[friendId].Add(landId, 0); 
        if (now - eventSendTime[friendId][landId] <= 1)
        {
MessageBox.ASSERT("Unable to request information of the same player and consecutive event");
            if(callbackHandle != null)
                callbackHandle(eMsgOperateResult.OR_Succeed);
            return;
        } 
        m_PatrolEventHandle = callbackHandle;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolEvent;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolEvent;
        f_PatrolEvent(friendId, landId,eventNum, socketCallbackDt);
    }

    private Dictionary<long, Dictionary<int,int>> eventSendTime = new Dictionary<long, Dictionary<int,int>>();
    private ccCallback m_PatrolEventHandle;

    static int recvIdx = 0;
    private void f_Callback_PatrolEvent(object result)
    {
        int now = GameSocket.GetInstance().f_GetServerTime();
        if (m_PatrolEventHandle != null)
            m_PatrolEventHandle(result);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
MessageBox.ASSERT("The patrol event query failed");
    }

    #region LandSkill

    private PatrolSkillNode[] landSkills;
    public PatrolSkillNode[] m_LandSkills
    {
        get
        {
            return landSkills;
        }
    }

    public int f_GetLandSkillLv(int landId)
    {
        for (int i = 0; i < landSkills.Length; i++)
        {
            if (landSkills[i].m_LandTemplate.iId == landId)
            {
                return landSkills[i].m_iLv;
            }
        }
        return 0;
    }

    private int f_GetSkillLvMax(int landId)
    {
        int lvMax = 0;
        List<NBaseSCDT> tSkillList = glo_Main.GetInstance().m_SC_Pool.m_PatrolLandSkillSC.f_GetAll().FindAll(delegate (NBaseSCDT tNode)
        {
            PatrolLandSkillDT tItem = (PatrolLandSkillDT)tNode;
            return tItem.iLandId == landId;
        }
        );
        lvMax = landSkills.Length > 0 ? ((PatrolLandSkillDT)tSkillList[landSkills.Length - 1]).iLv : 0;
        return lvMax;
    }

    private void f_UpdateSkillNode(int landId,EM_PatrolState landState,int landLv, int landTime)
    {
        for (int i = 0; i < landSkills.Length; i++)
        {
            if (landSkills[i].m_iLandId == landId)
                landSkills[i].f_UpdateByInfo(landState,landLv, landTime);
        }
    }

    #endregion

    #region 好友巡逻信息

    private const int FriendInfoRequestTimeCD = 8;
    private int friendInfoRequestTime = 0;
    private List<BasePoolDT<long>> friendInfoList = new List<BasePoolDT<long>>();
    private ccCallback callback_FriendInfo;
    public List<BasePoolDT<long>> m_FriendInfoList
    {
        get
        {
            return friendInfoList;
        }
    }

    public void f_RequestPatrolFriendInfo(ccCallback callbackHandle)
    {
        int tNow = GameSocket.GetInstance().f_GetServerTime();
        if (tNow - friendInfoRequestTime > FriendInfoRequestTimeCD)
        {
            callback_FriendInfo = callbackHandle;
            friendInfoList.Clear();
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolFriendInfo;
            socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolFriendInfo;
            f_PatrolFriendInfo(socketCallbackDt);
        }
        else
        {
            callbackHandle(eMsgOperateResult.OR_Succeed);
        }
    }

    private void f_Callback_PatrolFriendInfo(object result)
    {
        if (callback_FriendInfo != null)
            callback_FriendInfo(result);
    }

    #endregion

    #region 红点

    private bool isAlreadyInitReddotData = false;
    public void f_InitReddotData(ccCallback callback_InitReddotData)
    {
        if (isAlreadyInitReddotData)
        {
            callback_InitReddotData(eMsgOperateResult.OR_Succeed);
            return;
        }
        else if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.PatrolLevel))
        {
            callback_InitReddotData(eMsgOperateResult.OR_Succeed);
            return;
        }
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = callback_InitReddotData;
        socketCallbackDt.m_ccCallbackFail = callback_InitReddotData;
        f_PatrolInit(0, socketCallbackDt);
    }

    public void f_CheckPatrolReddot()
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.PatrolLevel))
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.PatrolGetAward);
            return;
        }
        else if (selfPatrolDt == null)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.PatrolGetAward);
            return;
        }
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.PatrolGetAward);
        for (int i = 0; i < selfPatrolDt.m_PatrolLands.Length; i++)
        {
            if (selfPatrolDt.m_PatrolLands[i].m_iEndTime != 0 && GameSocket.GetInstance().f_GetServerTime() > selfPatrolDt.m_PatrolLands[i].m_iEndTime)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.PatrolGetAward);
                break;
            }
            else if (selfPatrolDt.m_PatrolLands[i].m_State == EM_PatrolState.GetAward)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.PatrolGetAward);
                break;
            }
        }
    }



    #endregion

    #region Disable

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
