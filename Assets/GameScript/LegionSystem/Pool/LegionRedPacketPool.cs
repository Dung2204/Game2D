using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
public class LegionRedPacketDTSub : BasePoolDT<long>
{
    public int m_PlayerId;//玩家id
    public int m_PlayerName;//玩家名称
    public string m_LegionName;//军团名字
    public int m_LegionLevel;//军团等级
    public byte m_IconId;         // 图标id
    public byte m_FrameId;        // 边框id
    public int m_SycceCount;//发出或抢到元宝数量
    public int Rank;//排名
}
/// <summary>
/// 军团红包pool
/// </summary>
public class LegionRedPacketPool : BasePool {
    private ccCallback OnGetSyceeCallback;
    public int m_curSendPacketTimes = 0;//当前发红包次数
    public int m_DaySendPacketTimes = 5;//每日可以限发红包次数
    public int m_MySendSyceeCount = 0;//我发放的元宝数量
    
    public int m_TodayLeftRecvTimes //今日剩余红包领取次数
    {
        private set;
        get;
    }

    public Dictionary<EM_RedPacket, int> m_dicRedPacketToSycee = new Dictionary<EM_RedPacket, int>();//红包价格和元宝对应关系
    public Dictionary<EM_RedPacket, int> m_dicRedPacketToCount = new Dictionary<EM_RedPacket, int>();//红包价格和其个数对应关系
    /// <summary>
    /// 财神榜
    /// </summary>
    public List<BasePoolDT<long>> listBlissRank = new List<BasePoolDT<long>>();
    #region Pool数据管理
    /// <summary>
    /// 构造
    /// </summary>
    public LegionRedPacketPool() : base("LegionRedPacketPoolDT", true)
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        m_dicRedPacketToSycee.Add(EM_RedPacket.Packet200, 200);
        m_dicRedPacketToSycee.Add(EM_RedPacket.Packet500, 500);
        m_dicRedPacketToSycee.Add(EM_RedPacket.Packet1000, 1000);

        m_dicRedPacketToCount.Add(EM_RedPacket.Packet200, 10);
        m_dicRedPacketToCount.Add(EM_RedPacket.Packet500, 15);
        m_dicRedPacketToCount.Add(EM_RedPacket.Packet1000, 20);
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void RegSocketMessage()
    {
        SC_LegionRedPacketInfo scLegionRedPacketInfo = new SC_LegionRedPacketInfo();//请求红包列表信息
        GameSocket.GetInstance().f_RegMessage_Int2((int)LegionSocketCmd.SC_LegionRedPacketInfo, scLegionRedPacketInfo, Callback_RedPacketInfo);
        SC_LegionAwardRedPacket scLegionAwardRedPacket = new SC_LegionAwardRedPacket();//发红包回调（暂时没用到）
        GameSocket.GetInstance().f_RegMessage((int)LegionSocketCmd.SC_LegionAwardRedPacket, scLegionAwardRedPacket, OnSendRedPacketCallback);
        SC_LegionReceiveRedPacket scLegionReceiveRedPacket = new SC_LegionReceiveRedPacket();//收红包回调
        GameSocket.GetInstance().f_RegMessage((int)LegionSocketCmd.SC_LegionReceiveRedPacket, scLegionReceiveRedPacket, OnReceiveRedPacketCallback);
        SC_LegionRedPacketRank scLegionRedPacketRank = new SC_LegionRedPacketRank();//红包排行
        GameSocket.GetInstance().f_RegMessage_Int0((int)LegionSocketCmd.RC_LegionRankList, scLegionRedPacketRank, Callback_Rank);
    }
    /// <summary>
    /// 红包排行榜信息
    /// </summary>
    protected void Callback_Rank(int iData1, int iData2, int iNum, ArrayList aData)
    {
        listBlissRank.Clear();
        m_MySendSyceeCount = 0;
        foreach (SockBaseDT tData in aData)
        {
            SC_LegionRedPacketRank item = (SC_LegionRedPacketRank)tData;
            if (item.userId == Data_Pool.m_UserData.m_iUserId)
            {
                BasePlayerPoolDT tSelfPlayer = Data_Pool.m_GeneralPlayerPool.f_GetForId(item.userId) as BasePlayerPoolDT;
                LegionTool.f_SelfPlayerInfoDispose(ref tSelfPlayer);
                m_MySendSyceeCount = item.monCount;
            }
            LegionRedPacketRankPoolDT poolDT = new LegionRedPacketRankPoolDT();
            poolDT.iId = item.userId;
            poolDT.m_BasePlayerPoolDT = Data_Pool.m_GeneralPlayerPool.f_GetForId(item.userId) as BasePlayerPoolDT;
            poolDT.m_SyceeCount = item.monCount;
            listBlissRank.Add(poolDT);
        }
        listBlissRank.Sort(delegate (BasePoolDT<long> node1, BasePoolDT<long> node2)
        {
            LegionRedPacketRankPoolDT item1 = (LegionRedPacketRankPoolDT)node1;
            LegionRedPacketRankPoolDT item2 = (LegionRedPacketRankPoolDT)node2;
            if (item1.m_SyceeCount < item2.m_SyceeCount)
                return 1;
            else if (item1.m_SyceeCount > item2.m_SyceeCount)
                return -1;
            return item2.iId.CompareTo(item1.iId);
        });
        for (int i = 0; i < listBlissRank.Count; i++)
        {
            LegionRedPacketRankPoolDT tNode = (LegionRedPacketRankPoolDT)listBlissRank[i];
            tNode.m_Rank = i + 1;
        }
    }
    /// <summary>
    /// 发红包回调
    /// </summary>
    private void OnSendRedPacketCallback(object obj)
    {
        SC_LegionAwardRedPacket scLegionAwardRedPacket = (SC_LegionAwardRedPacket)obj;
MessageBox.DEBUG("Callback send lucky money"+ (EM_RedPacket)scLegionAwardRedPacket.money);
    }
    /// <summary>
    /// 收红包回调
    /// </summary>
    private void OnReceiveRedPacketCallback(object obj)
    {
        SC_LegionReceiveRedPacket scLegionReceiveRedPacket = (SC_LegionReceiveRedPacket)obj;
        if (OnGetSyceeCallback != null)
        {
            OnGetSyceeCallback((int)scLegionReceiveRedPacket.getSycee);
            OnGetSyceeCallback = null;
        }
MessageBox.DEBUG("Callback receives lucky money" + scLegionReceiveRedPacket.id + ":" +scLegionReceiveRedPacket.getSycee);
    }
    /// <summary>
    /// 请求红包信息回调
    /// </summary>
    protected void Callback_RedPacketInfo(int iData1, int iData2, int iNum, ArrayList aData)
    {
        m_curSendPacketTimes = iData1;//该值为今天发红包次数
        m_TodayLeftRecvTimes = iData2; //该值为今日剩余红包领取次数
        foreach (SockBaseDT tData in aData)
        {
            UpdateRedPacketInfo(tData);
        }
        f_SortData();
    }
    /// <summary>
    /// 增加/更新可以领取的红包列表信息
    /// </summary>
    /// <param name="Obj"></param>
    private void UpdateRedPacketInfo(SockBaseDT Obj)
    {
        SC_LegionRedPacketInfo scLegionRedPacketInfo = (SC_LegionRedPacketInfo)Obj;
        LegionRedPacketPoolDT dt = f_GetForId(scLegionRedPacketInfo.id) as LegionRedPacketPoolDT;
        if (scLegionRedPacketInfo.userId == Data_Pool.m_UserData.m_iUserId)
        {
            BasePlayerPoolDT tSelfPlayer = Data_Pool.m_GeneralPlayerPool.f_GetForId(scLegionRedPacketInfo.userId) as BasePlayerPoolDT;
            LegionTool.f_SelfPlayerInfoDispose(ref tSelfPlayer);
        }
        if (dt == null)
        {
            LegionRedPacketPoolDT newDT = new LegionRedPacketPoolDT();
            newDT.iId = scLegionRedPacketInfo.id;
            newDT.em_redPacket = (EM_RedPacket)scLegionRedPacketInfo.type;
            newDT.m_PlayerId = scLegionRedPacketInfo.userId; 
            newDT.m_BasePlayerPoolDT = Data_Pool.m_GeneralPlayerPool.f_GetForId(newDT.m_PlayerId) as BasePlayerPoolDT;
            newDT.m_CurCount = scLegionRedPacketInfo.proc;
            newDT.m_CurSycee = scLegionRedPacketInfo.hasGet;
            newDT.m_MyIsGet = scLegionRedPacketInfo.isGet == 1 ? true : false;
            f_Save(newDT);
            //红点
            if (m_TodayLeftRecvTimes > 0 
                && newDT.m_CurSycee < m_dicRedPacketToSycee[newDT.em_redPacket]
                && newDT.m_CurCount < m_dicRedPacketToCount[newDT.em_redPacket]
                && !newDT.m_MyIsGet)
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.LegionRedPacket);
        }
        else
        {
            dt.em_redPacket = (EM_RedPacket)scLegionRedPacketInfo.type;
            dt.m_PlayerId = scLegionRedPacketInfo.userId;
            dt.m_BasePlayerPoolDT = Data_Pool.m_GeneralPlayerPool.f_GetForId(dt.m_PlayerId) as BasePlayerPoolDT;
            dt.m_CurCount = scLegionRedPacketInfo.proc;
            dt.m_CurSycee = scLegionRedPacketInfo.hasGet;
            dt.m_MyIsGet = scLegionRedPacketInfo.isGet == 1 ? true : false;
            //红点
            if (m_TodayLeftRecvTimes > 0 
                && dt.m_CurSycee < m_dicRedPacketToSycee[dt.em_redPacket]
                && dt.m_CurCount < m_dicRedPacketToCount[dt.em_redPacket]
                && !dt.m_MyIsGet)
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.LegionRedPacket);
            
        }
    }


    /// <summary>
    /// 红包数据排序 未领取 > 未领取可领取元宝为0 > 未领取可领取元宝为0 > iId
    /// </summary>
    private void f_SortData()
    {
        f_GetAll().Sort((BasePoolDT<long> dt1,BasePoolDT<long> dt2) =>
        {
            LegionRedPacketPoolDT packet1 = (LegionRedPacketPoolDT)dt1;
            LegionRedPacketPoolDT packet2 = (LegionRedPacketPoolDT)dt2;
            if (packet1.m_MyIsGet && !packet2.m_MyIsGet)
                return 1;
            else if (!packet1.m_MyIsGet && packet2.m_MyIsGet)
                return -1;
            if (packet1.m_CurSycee < m_dicRedPacketToSycee[packet1.em_redPacket] && packet2.m_CurSycee >= m_dicRedPacketToSycee[packet2.em_redPacket])
                return -1;
            if (packet1.m_CurSycee >= m_dicRedPacketToSycee[packet1.em_redPacket] && packet2.m_CurSycee < m_dicRedPacketToSycee[packet2.em_redPacket])
                return 1;
            if (packet1.m_CurCount < m_dicRedPacketToCount[packet1.em_redPacket] && packet2.m_CurCount >= m_dicRedPacketToCount[packet2.em_redPacket])
                return -1;
            if (packet1.m_CurCount >= m_dicRedPacketToCount[packet1.em_redPacket] && packet2.m_CurCount < m_dicRedPacketToCount[packet2.em_redPacket])
                return 1;
            return packet1.iId.CompareTo(packet2.iId);
        });
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }
    /// <summary>
    /// 更新数据
    /// </summary>
    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #endregion
    #region 通讯接口
    /// <summary>
    /// 请求红包列表信息
    /// </summary>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_GetRedPacketInfo(SocketCallbackDT tSocketCallbackDT)
    {
        f_Clear();
        //红点
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LegionRedPacket);

        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionRedPacketInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionRedPacketInfo, bBuf);
    }
    /// <summary>
    /// 请求发红包
    /// </summary>
    /// <param name="packetType">红包类型</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_SendRedPacket(EM_RedPacket packetType, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionAwardRedPacket, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)packetType);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionAwardRedPacket, bBuf);
    }
    /// <summary>
    /// 请求收红包
    /// </summary>
    /// <param name="packetId">红包id</param>
    /// <param name="tSocketCallbackDT">回调</param>
    /// <param name="OnGetSyceeCallback">领取得到元宝多少回调</param>
    public void f_ReceiveRedPacket(long packetId, SocketCallbackDT tSocketCallbackDT,ccCallback OnGetSyceeCallback)
    {
        this.OnGetSyceeCallback = OnGetSyceeCallback;
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionReceiveRedPacket, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(packetId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionReceiveRedPacket, bBuf);
MessageBox.DEBUG("Get lucky money" + packetId);
    }
    /// <summary>
    /// 请求排行信息
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_GetRankInfo(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionRankList, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionRankList, bBuf);
    }
    #endregion

    /// <summary>
    /// 检查红点
    /// </summary>
    public void f_CheckReddot()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LegionRedPacket);
        if (m_TodayLeftRecvTimes <= 0)
            return;
        List<BasePoolDT<long>> tList = f_GetAll();
        for (int i = 0; i < tList.Count; i++)
        {
            LegionRedPacketPoolDT tNode = (LegionRedPacketPoolDT)tList[i];
            //红点
            if (tNode.m_CurSycee < m_dicRedPacketToSycee[tNode.em_redPacket]
                && tNode.m_CurCount < m_dicRedPacketToCount[tNode.em_redPacket]
                && !tNode.m_MyIsGet)
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.LegionRedPacket);
        }
    }
}
/// <summary>
/// 红包类型
/// </summary>
public enum EM_RedPacket
{
    Packet200 = 1,
    Packet500 = 2,
    Packet1000 = 3,
}
