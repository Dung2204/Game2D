using ccU3DEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 排行榜类型，不要调整顺序，避免某些数据越界异常！！！
/// </summary>
public enum EM_RankListType
{
    /// <summary>
    /// 战斗力排行榜
    /// </summary>
    Power = 0,
    /// <summary>
    /// 军团排行榜
    /// </summary>
    Legion = 1,
    Level = 2,
    Duplicate = 3,
    Arena = 4,
    RunningMan = 5,
    End = 6,
}

public class RankListPool : BasePool
{
    /// <summary>
    /// 点赞奖励元宝数
    /// </summary>
    public const int PraiseAwardSyceeNum = 5;

    public const int SelfPraiseTimesLimit = 1;

    private int m_SelfPraiseTimes;
    /// <summary>
    /// 个人点赞次数
    /// </summary>
    public int SelfPraiseTimes
    {
        get
        {
            return m_SelfPraiseTimes;
        }
    }

    /// <summary>
    /// 排行榜X个每页
    /// </summary>
    private const int RANK_LIST_NUM_PRE_PAGE = 10;
    /// <summary>
    /// 排行榜页数最大值
    /// </summary>
    private const int RANK_LIST_PAGE_MAX = 5;

    private Dictionary<EM_RankListType, List<BasePoolDT<long>>> m_RankListDic = new Dictionary<EM_RankListType, List<BasePoolDT<long>>>();
    private Dictionary<EM_RankListType, int> m_RankListPageIdxDic = new Dictionary<EM_RankListType, int>();
    private Dictionary<EM_RankListType, int> m_RankListTimeDic = new Dictionary<EM_RankListType, int>();
    private Dictionary<EM_RankListType, BasePoolDT<long>> m_SelfPoolDtDic = new Dictionary<EM_RankListType, BasePoolDT<long>>();
    private bool[] m_RankListWait = new bool[4];

    public RankListPool() : base("PowerRankListPoolDT") //这个Pool 这里传入的结构体无用，个人需要不同类型的Pool
    {
        m_SelfPraiseTimes = 0;
        for (int i = 0; i <= (int)EM_RankListType.End; i++)
        {
            m_RankListDic.Add((EM_RankListType)i, new List<BasePoolDT<long>>());
            m_RankListPageIdxDic.Add((EM_RankListType)i, 0);
            m_RankListTimeDic.Add((EM_RankListType)i, 0);
        }    
        m_SelfPoolDtDic.Add(EM_RankListType.Power, new RankListPoolDT());
        m_SelfPoolDtDic.Add(EM_RankListType.Level, new RankListPoolDT());
        m_SelfPoolDtDic.Add(EM_RankListType.Duplicate, new RankListPoolDT());
        m_SelfPoolDtDic.Add(EM_RankListType.Legion, null);
        m_SelfPoolDtDic.Add(EM_RankListType.End, new BasePoolDT<long>());
    }


    protected override void f_Init()
    {

    }

    protected override void RegSocketMessage()
    {
        //军团排行的军团详细信息
        RC_LegionPowerInfo tsc_LegionPowerInfo = new RC_LegionPowerInfo();
        ChatSocket.GetInstance().f_RegMessage((int)SocketCommand.RC_LegionPowerInfo, tsc_LegionPowerInfo, f_GTC_LegionPowerInfo_Handle);

        //玩家战力排行榜
        RC_RankerUserPowerNode tsc_UserPowerNode = new RC_RankerUserPowerNode();
        ChatSocket.GetInstance().f_RegMessage_Int3((int)SocketCommand.RC_RankerUserPower, tsc_UserPowerNode, f_GTC_RankerUserPower_Handle);

        //玩家战力排行榜（自己信息）
        RC_RankerUserPowerSelfNode tsc_UserPowerSelfNode = new RC_RankerUserPowerSelfNode();
        ChatSocket.GetInstance().f_RegMessage((int)SocketCommand.RC_RankerUserPowerSelf, tsc_UserPowerSelfNode, f_GTC_RankerUserPowerSelf_Handle);

        //军团战力排行
        RC_RankerLegionPowerNode tsc_LegionPowerNode = new RC_RankerLegionPowerNode();
        ChatSocket.GetInstance().f_RegMessage_Int2((int)SocketCommand.RC_RankerLegionPower, tsc_LegionPowerNode, f_GTC_RankerLegionPower_Handle);

        //军团战力排行（自己信息）
        RC_RankerLegionPowerSelf tsc_LegionPowerSelfNode = new RC_RankerLegionPowerSelf();
        ChatSocket.GetInstance().f_RegMessage((int)SocketCommand.RC_RankerLegionPowerSelf, tsc_LegionPowerSelfNode, f_GTC_RankerLegionPowerSelf_Handle);

        //玩家副本排行榜
        RC_RankerUserDungeonStarNode tsc_UserDungeonStarNode = new RC_RankerUserDungeonStarNode();
        ChatSocket.GetInstance().f_RegMessage_Int3((int)SocketCommand.RC_RankerUserDungeonStar, tsc_UserDungeonStarNode, f_GTC_RankerUserDungeonStar_Handle);

        //玩家副本排行榜（自己信息）
        RC_RankerUserDungeonStarSelfNode tsc_UserDungeonStarSelfNode = new RC_RankerUserDungeonStarSelfNode();
        ChatSocket.GetInstance().f_RegMessage((int)SocketCommand.RC_RankerUserDungeonStarSelf, tsc_UserDungeonStarSelfNode, f_GTC_RankerUserDungeonStarSelf_Handle);

        //玩家等级排行榜
        RC_RankerUserLevelNode tsc_UserLevelNode = new RC_RankerUserLevelNode();
        ChatSocket.GetInstance().f_RegMessage_Int3((int)SocketCommand.RC_RankerUserLevel, tsc_UserLevelNode, f_GTC_RankerUserLevel_Handle);

        //玩家等级排行榜（自己信息）
        RC_RankerUserLevelSelfNode tsc_UserLevelSelfNode = new RC_RankerUserLevelSelfNode();
        ChatSocket.GetInstance().f_RegMessage((int)SocketCommand.RC_RankerUserLevelSelf, tsc_UserLevelSelfNode, f_GTC_RankerUserLevelSelf_Handle);
    }

    #region 协议处理

    private void f_GTC_LegionPowerInfo_Handle(object value)
    {
        RC_LegionPowerInfo tServerData = (RC_LegionPowerInfo)value;
        f_SaveLegionPowerInfo(tServerData);
    }

    private void f_GTC_RankerUserPower_Handle(int iData1, int iData2, int iData3, int iNum, ArrayList aData)
    {
        List<BasePoolDT<long>> tList = f_GetRankList(EM_RankListType.Power);
        for (int i = 0; i < aData.Count; i++)
        {
            RC_RankerUserPowerNode tUpdateNode = (RC_RankerUserPowerNode)aData[i];
            if (tUpdateNode.userId == Data_Pool.m_UserData.m_iUserId)
            {
                //更新自己的数据
                RankListPoolDT tItem = (RankListPoolDT)f_GetSelfPoolDt(EM_RankListType.Power);
                if (tItem != null)
                {
                    tItem.f_UpdateInfo(tUpdateNode.rank, tItem.PlayerInfo);
                    tItem.f_UpdatePraiseInfo(tUpdateNode.iPraiseNum);
                }
                else
MessageBox.ASSERT("Not received combat rating data from server");
            }

            //更新数据，，存在更新，否则添加
            RankListPoolDT lvRankItem = tList.Find((BasePoolDT<long> item) => { return item.iId == tUpdateNode.userId; }) as RankListPoolDT;
            if (null == lvRankItem)
            {
                lvRankItem = new RankListPoolDT();
                BasePlayerPoolDT tPlayerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tUpdateNode.userId);
                if (tPlayerInfo == null)
                {
MessageBox.ASSERT("Not received combat rating data from server");
                    continue;
                }
                lvRankItem.f_UpdateInfo(tUpdateNode.rank, tPlayerInfo);
                lvRankItem.f_UpdatePraiseInfo(tUpdateNode.iPraiseNum);
                tList.Add(lvRankItem);
            }
            else
            {
                lvRankItem.f_UpdateInfo(tUpdateNode.rank, lvRankItem.PlayerInfo);
                lvRankItem.f_UpdatePraiseInfo(tUpdateNode.iPraiseNum);
            }
        }
    }

    private void f_GTC_RankerUserPowerSelf_Handle(object value)
    {
        RC_RankerUserPowerSelfNode tServerData = (RC_RankerUserPowerSelfNode)value;
        BasePlayerPoolDT tPlayerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(Data_Pool.m_UserData.m_iUserId);
        LegionTool.f_SelfPlayerInfoDispose(ref tPlayerInfo);
        tPlayerInfo.f_SaveExtrend(tServerData.szLegionName, tPlayerInfo.m_iLv, (int)tServerData.iPower, 0);
        RankListPoolDT tSelfItem = (RankListPoolDT)m_SelfPoolDtDic[EM_RankListType.Power];
        tSelfItem.f_UpdateInfo(tServerData.iRank, tPlayerInfo);
        tSelfItem.f_UpdatePraiseInfo(tServerData.iPraiseNum1);
        m_SelfPraiseTimes = tServerData.iPraiseNum0;
    }

    private void f_GTC_RankerLegionPower_Handle(int iData1, int iData2, int iNum, ArrayList aData)
    {
        List<BasePoolDT<long>> tList = f_GetRankList(EM_RankListType.Legion);
        for (int i = 0; i < aData.Count; i++)
        {
            RC_RankerLegionPowerNode tRankNode =  (RC_RankerLegionPowerNode)aData[i];
            int rankIndex = tList.FindIndex((BasePoolDT<long> item) => { return item.iId == tRankNode.legionId; });
            if (rankIndex >= 0)
            {
                RankListLegionPoolDT rankItem = tList[rankIndex] as RankListLegionPoolDT;
                if (null == rankItem) continue;
                rankItem.f_UpdateRank(tRankNode.rank);
            }
            else
            {
                RankListLegionPoolDT tLegionNode = (RankListLegionPoolDT)f_GetLegionPowerInfo(tRankNode.legionId);
                if (tLegionNode == null)
                {
MessageBox.ASSERT("The corps rank data received from the server does not exist");
                    continue;
                }  
                tLegionNode.f_UpdateRank(tRankNode.rank);
                tList.Add(tLegionNode);
            }
        }
    }

    private void f_GTC_RankerLegionPowerSelf_Handle(object value)
    {
        RC_RankerLegionPowerSelf tServerData = (RC_RankerLegionPowerSelf)value;
        if (tServerData.legionId == 0)
        {
            m_SelfPoolDtDic[EM_RankListType.Legion] = null;
        }
        else
        {
            LegionInfoPoolDT myLegionInfo = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion();
            RankListLegionPoolDT tLegionNode = new RankListLegionPoolDT();
            tLegionNode.f_UpdateRank(tServerData.rank);
            tLegionNode.f_UpdateByInfo(myLegionInfo, tServerData.iPower);
            m_SelfPoolDtDic[EM_RankListType.Legion] = tLegionNode;
        }
    }


    private void f_GTC_RankerUserDungeonStar_Handle(int iData1, int iData2, int iData3, int iNum, ArrayList aData)
    {
        List<BasePoolDT<long>> tList = f_GetRankList(EM_RankListType.Duplicate);
        for (int i = 0; i < aData.Count; i++)
        {
            RC_RankerUserDungeonStarNode tUpdateNode = (RC_RankerUserDungeonStarNode)aData[i];
            if (tUpdateNode.userId == Data_Pool.m_UserData.m_iUserId)
            {
                //更新自己的数据
                RankListPoolDT tItem = (RankListPoolDT)f_GetSelfPoolDt(EM_RankListType.Duplicate);
                if (tItem != null)
                {
                    tItem.f_UpdateInfo(tUpdateNode.rank, tItem.PlayerInfo);
                    tItem.f_UpdateCurChapterId(tUpdateNode.iCurChapterId);
                }
                else
MessageBox.ASSERT("Not received combat rating data from server");
            }

            //更新数据，，存在更新，否则添加
            RankListPoolDT dungeonRankItem = tList.Find((BasePoolDT<long> item) => { return item.iId == tUpdateNode.userId; }) as RankListPoolDT;
            if (null == dungeonRankItem)
            {
                dungeonRankItem = new RankListPoolDT();
                BasePlayerPoolDT tPlayerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tUpdateNode.userId);
                if (tPlayerInfo == null)
                {
MessageBox.ASSERT("Not received combat rating data from server");
                    continue;
                }
                dungeonRankItem.f_UpdateInfo(tUpdateNode.rank, tPlayerInfo);
                dungeonRankItem.f_UpdateCurChapterId(tUpdateNode.iCurChapterId);
                tList.Add(dungeonRankItem);
            }
            else
            {
                dungeonRankItem.f_UpdateInfo(tUpdateNode.rank, dungeonRankItem.PlayerInfo);
                dungeonRankItem.f_UpdateCurChapterId(tUpdateNode.iCurChapterId);
            }
        }
    }

    private void f_GTC_RankerUserDungeonStarSelf_Handle(object value)
    {
        RC_RankerUserDungeonStarSelfNode tServerData = (RC_RankerUserDungeonStarSelfNode)value;
        BasePlayerPoolDT tPlayerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(Data_Pool.m_UserData.m_iUserId);
        LegionTool.f_SelfPlayerInfoDispose(ref tPlayerInfo);
        tPlayerInfo.f_UpdateDungeonStar(tServerData.iStars);
        RankListPoolDT tSelfItem = (RankListPoolDT)m_SelfPoolDtDic[EM_RankListType.Duplicate];
        tSelfItem.f_UpdateInfo(tServerData.iRank, tPlayerInfo);
        tSelfItem.f_UpdateCurChapterId(tServerData.iCurChapterId);
    }

    private void f_GTC_RankerUserLevel_Handle(int iData1, int iData2, int iData3, int iNum, ArrayList aData)
    {
        List<BasePoolDT<long>> tList = f_GetRankList(EM_RankListType.Level);
        for (int i = 0; i < aData.Count; i++)
        {
            RC_RankerUserLevelNode tUpdateNode = (RC_RankerUserLevelNode)aData[i];
            if (tUpdateNode.userId == Data_Pool.m_UserData.m_iUserId)
            {
                //更新自己的数据
                RankListPoolDT tItem = (RankListPoolDT)f_GetSelfPoolDt(EM_RankListType.Level);
                if (tItem != null)
                {
                    tItem.f_UpdateInfo(tUpdateNode.rank, tItem.PlayerInfo);
                }
                else
MessageBox.ASSERT("Not received combat rating data from server");
            }

            //更新数据，，存在更新，否则添加
            RankListPoolDT lvRankItem = tList.Find((BasePoolDT<long> item) => { return item.iId == tUpdateNode.userId; }) as RankListPoolDT;
            if (null == lvRankItem)
            {
                lvRankItem = new RankListPoolDT();
                BasePlayerPoolDT tPlayerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tUpdateNode.userId);
                if (tPlayerInfo == null)
                {
MessageBox.ASSERT("Not received combat rating data from server");
                    continue;
                }
                lvRankItem.f_UpdateInfo(tUpdateNode.rank, tPlayerInfo);
                tList.Add(lvRankItem);
            }
            else
            {
                lvRankItem.f_UpdateInfo(tUpdateNode.rank, lvRankItem.PlayerInfo);
            }
        }
    }

    private void f_GTC_RankerUserLevelSelf_Handle(object value)
    {
        RC_RankerUserLevelSelfNode tServerData = (RC_RankerUserLevelSelfNode)value;
        RankListPoolDT tSelfItem = (RankListPoolDT)m_SelfPoolDtDic[EM_RankListType.Level];
        BasePlayerPoolDT tPlayerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(Data_Pool.m_UserData.m_iUserId);
        LegionTool.f_SelfPlayerInfoDispose(ref tPlayerInfo);
        tSelfItem.f_UpdateInfo(tServerData.iRank, tPlayerInfo);
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

    #region 发送协议

    public void f_RankList(byte rankListType, byte pageIdx, SocketCallbackDT socketCallbackDt)
    {
        if (rankListType > (byte)EM_RankListType.Duplicate)
        {
            MessageBox.ASSERT("Request no process ranklist type:" + rankListType);
            if (socketCallbackDt != null && socketCallbackDt.m_ccCallbackSuc != null)
            {
                socketCallbackDt.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            }
            return;
        }

        int[] messageId = new int[4];
        messageId[(int)EM_RankListType.Power] = (int)SocketCommand.CS_RankerUserPower;
        messageId[(int)EM_RankListType.Legion] = (int)SocketCommand.CS_RankerLegionPower;
        messageId[(int)EM_RankListType.Level] = (int)SocketCommand.CS_RankerUserLevel;
        messageId[(int)EM_RankListType.Duplicate] = (int)SocketCommand.CS_RankerUserDungeonStars;
        ChatSocket.GetInstance().f_RegCommandReturn(messageId[rankListType], socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(pageIdx);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf((SocketCommand)messageId[rankListType], bBuf);
    }

    public void f_PowerPraise(long userId, SocketCallbackDT socketCallbackDt)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_UserPowerPraise, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(userId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_UserPowerPraise, bBuf);
    }
    
    #endregion

    public List<BasePoolDT<long>> f_GetRankList(EM_RankListType rankListType)
    {
        if (m_RankListDic.ContainsKey(rankListType))
        {
            return m_RankListDic[rankListType];
        }
        else
        {
            //不存在，返回空列表
            return m_RankListDic[EM_RankListType.End];
        }
    }

    public BasePoolDT<long> f_GetSelfPoolDt(EM_RankListType rankListType)
    {
        if (m_SelfPoolDtDic.ContainsKey(rankListType))
        {
            return m_SelfPoolDtDic[rankListType];
        }
        else
        {
            return null;
        }
    }

    public void f_ExecuteAfterRankList(EM_RankListType rankListType, bool first, ccCallback callbackRankList)
    {
        //判断是否要等待
        int nRankListType = (int)rankListType;
        if (nRankListType >= m_RankListWait.Length) return;
        if (m_RankListWait[nRankListType] && m_RankListDic[rankListType].Count > 0)
        {
MessageBox.DEBUG("Waiting for a response from the server");
            return;
        }

        //服务器回调处理
        ccCallback callback_RankList = (object result) =>
         {
            if (m_RankListWait[nRankListType])
            {
                m_RankListPageIdxDic[rankListType]++;
            }
            m_RankListWait[nRankListType] = false;
            if (callbackRankList != null)
                 callbackRankList(result);
        };

        int tNow = GameSocket.GetInstance().f_GetServerTime();
        int tLast = m_RankListTimeDic[rankListType];
        bool needUpdate = tNow / 3600 != tLast / 3600;
        if (needUpdate)
        {
            m_RankListTimeDic[rankListType] = tNow;
            m_RankListPageIdxDic[rankListType] = 0;
            m_RankListDic[rankListType].Clear();
            m_RankListWait[nRankListType] = true;
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = callback_RankList;
            socketCallbackDt.m_ccCallbackFail = callback_RankList;
            f_RankList((byte)rankListType, (byte)m_RankListPageIdxDic[rankListType], socketCallbackDt);
        }
        else
        {
            if (first)
            {
                if (callbackRankList != null)
                    callbackRankList(eMsgOperateResult.OR_Succeed);
            }
            else
            {
                if (m_RankListDic[rankListType].Count == m_RankListPageIdxDic[rankListType] * RANK_LIST_NUM_PRE_PAGE && m_RankListPageIdxDic[rankListType] < RANK_LIST_PAGE_MAX)
                {
                    m_RankListWait[nRankListType] = true;
                    SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
                    socketCallbackDt.m_ccCallbackSuc = callback_RankList;
                    socketCallbackDt.m_ccCallbackFail = callback_RankList;
                    f_RankList((byte)rankListType, (byte)m_RankListPageIdxDic[rankListType], socketCallbackDt);
                }
                else
                {
                    if (callbackRankList != null)
                        callbackRankList(eMsgOperateResult.OR_Succeed);
                }
            }
        }
    }
    #region 军团战力排行 的军团详细信息

    private BetterList<BasePoolDT<long>> m_LegionPowerInfoList = new BetterList<BasePoolDT<long>>();

    private void f_SaveLegionPowerInfo(RC_LegionPowerInfo serverData)
    {
        RankListLegionPoolDT tItem = (RankListLegionPoolDT)f_GetLegionPowerInfo(serverData.legionId);
        if (tItem == null)
        {
            tItem = new RankListLegionPoolDT();
            tItem.iId = serverData.legionId;
            LegionInfoPoolDT tLegionInfo = new LegionInfoPoolDT();
            tLegionInfo.iId = serverData.legionId;
            tItem.f_UpdateByInfo(tLegionInfo, serverData.legionPower);
            m_LegionPowerInfoList.Add(tItem);
        }
        tItem.LegionInfo.f_UpdateProperty((int)EM_LegionProperty.Lv, serverData.lv);
        tItem.LegionInfo.f_UpdateProperty((int)EM_LegionProperty.Icon, serverData.iconId);
        tItem.LegionInfo.f_UpdateProperty((int)EM_LegionProperty.Name, serverData.szLegionName);
        tItem.f_UpdateChiefInfo((int)serverData.chiefSex, (int)serverData.chiefLv, (int)serverData.chiefPower, 
            serverData.chiefName, serverData.chiefId, serverData.uChiefFrameId, serverData.uChiefVipLv);
    }

    private BasePoolDT<long> f_GetLegionPowerInfo(long legionId)
    {
        for (int i = 0; i < m_LegionPowerInfoList.size; i++)
        {
            if (m_LegionPowerInfoList[i].iId == legionId)
                return m_LegionPowerInfoList[i];
        }
        return null;
    }

    #endregion

}
