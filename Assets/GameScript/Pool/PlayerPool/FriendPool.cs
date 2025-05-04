using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using ccU3DEngine;

public class FriendPool : BasePool
{
    private Dictionary<int, List<BasePoolDT<long>>> mFriendDataDic;
    //是否初始化
    private Dictionary<int, bool> mFriendDataInitDic;
    private Dictionary<int, ccCallback> mFriendDataInitCallbackDic;

    public FriendPool() : base("FriendPoolDT")
    {

    }

    protected override void f_Init()
    {
        mFriendDataDic = new Dictionary<int, List<BasePoolDT<long>>>();
        mFriendDataInitDic = new Dictionary<int, bool>();
        mFriendDataInitCallbackDic = new Dictionary<int, ccCallback>();
        for (int i = 0; i < (int)EM_FriendListType.END; i++)
        {
            List<BasePoolDT<long>> tList = new List<BasePoolDT<long>>();
            mFriendDataDic.Add(i, tList);
            mFriendDataInitDic.Add(i, false);
            mFriendDataInitCallbackDic.Add(i, null);
        }
        _donateVigorInfo = new Dictionary<long, FriendVigorInfo>();
        _otherVigorInfo = new Dictionary<long, FriendVigorInfo>();
        mRecvVigorNum = 0;
        mLastRecvVigorNum = 0;
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
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_THENEXTDAY_UIPROCESS,EM_NextDaySource.FriendPool);
    }

    private void f_Reset()
    {
        //好友列表 重置精力数据
        List<BasePoolDT<long>> tList = f_GetDataListByType(EM_FriendListType.Friend);
        BasePlayerPoolDT tItem;
        for (int i = 0; i < tList.Count; i++)
        {
            tItem = (BasePlayerPoolDT)tList[i];
            tItem.f_ResetVigorData();
        }
        //领取精力列表 重置数据
        tList = f_GetDataListByType(EM_FriendListType.Vigor);
        for (int i = 0; i < tList.Count; i++)
        {
            tItem = (BasePlayerPoolDT)tList[i];
            tItem.f_ResetVigorData();
        }
        tList.Clear();
        mLastRecvVigorNum = 0;
        mRecvVigorNum = 0;
        _donateVigorInfo.Clear();
        _otherVigorInfo.Clear();
    }

    #endregion
    

    protected override void RegSocketMessage()
    {
        //好友列表初始化
        RC_InitFriend tRC_InitFriend = new RC_InitFriend();
        ChatSocket.GetInstance().f_RegMessage_Int1((int)ChatSocketCommand.RC_InitFriend, tRC_InitFriend, Callback_SocketData_Update);
        //移除好友
        stPoolDelData tRC_RemovFriend = new stPoolDelData();
        ChatSocket.GetInstance().f_RegMessage((int)ChatSocketCommand.RC_RemoveFriend, tRC_RemovFriend, f_Callback_RemoveFriend);
        //初始化好友申请列表
        stPoolDelData tRC_InitFriendApply = new stPoolDelData();
        ChatSocket.GetInstance().f_RegMessage_Int1((int)ChatSocketCommand.RC_InitFriendApply, tRC_InitFriendApply, Callback_SocketData_ApplyUpdate);
        //推荐好友列表
        CMsg_RTC_RecomFriend tMsg_RTC_RecomFriend = new CMsg_RTC_RecomFriend();
        ChatSocket.GetInstance().f_RegMessage((int)ChatSocketCommand.RC_RecomFriend, tMsg_RTC_RecomFriend,f_Callback_RecomFriend);
        //好友申请回应
        CMsg_RTC_ReplyFriend tMsg_RTC_ReplyFriend = new CMsg_RTC_ReplyFriend();
        ChatSocket.GetInstance().f_RegMessage((int)ChatSocketCommand.CR_ReplyFriend, tMsg_RTC_ReplyFriend, f_Callback_ReplyFriend);
        //黑名单回应
        stPoolDelData tRC_InitBlacklist = new stPoolDelData();
        ChatSocket.GetInstance().f_RegMessage_Int1((int)ChatSocketCommand.RC_InitBlack, tRC_InitBlacklist, Callback_SocketData_BlacklistUpdate);
        //精力数据
        CMsg_RTC_VigorHistory tRC_VigorHistory = new CMsg_RTC_VigorHistory();
        ChatSocket.GetInstance().f_RegMessage_Int1((int)ChatSocketCommand.RC_VigorHistory, tRC_VigorHistory, Callback_SocketData_VigorUpdate);
        //玩家关系信息
        CMsg_RTC_RelationUser tRC_RelationUser = new CMsg_RTC_RelationUser();
        ChatSocket.GetInstance().f_RegMessage((int)ChatSocketCommand.RC_RelationUser, tRC_RelationUser, f_Callback_RelationUser);
        
    }

    /// <summary>
    /// 好友申请回应
    /// </summary>
    private void f_Callback_ReplyFriend(object result)
    {
        CMsg_RTC_ReplyFriend tServerData = (CMsg_RTC_ReplyFriend)result;
MessageBox.DEBUG(string.Format("Response from request to add friends,userId:{0} result:{1}",tServerData.userId,tServerData.uAccept));
    }

    private void f_Callback_RelationUser(object result)
    {
        CMsg_RTC_RelationUser tServerData = (CMsg_RTC_RelationUser)result;
        mLastRecvVigorNum = mRecvVigorNum;
        mRecvVigorNum = tServerData.vigorTimes;
    }


    #region 好友列表相关

    //好友列表处理
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        RC_InitFriend tServerData = (RC_InitFriend)Obj;
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.userId);
        bool bAddGeneralPool = false;
        if (tPoolDataDT == null)
        {
            tPoolDataDT = new BasePlayerPoolDT();
            bAddGeneralPool = true;
        }
        tPoolDataDT.iId = tServerData.userId;
        tPoolDataDT.f_SaveBase(tServerData.m_UserView.m_szName, tServerData.m_UserView.uSex, 
            tServerData.m_UserView.uFrameId, tServerData.m_UserView.uVipLv, tServerData.m_UserView.uTitleId);
        tPoolDataDT.f_SaveExtrend(tServerData.m_UserExtra.szLegionName, tServerData.m_UserExtra.iLv,
            tServerData.m_UserExtra.iBattlePower,tServerData.m_UserExtra.offlineTime);
        tPoolDataDT.f_UpdateDungeonStar(tServerData.m_UserExtra.iDungeonStars);
        //检查更新精力数据
        f_CheckVigorInfoInAddFriend(tPoolDataDT);

        f_AddDataListByType(EM_FriendListType.Friend, tPoolDataDT);
        f_SortDataListByType(EM_FriendListType.Friend);
        if (bAddGeneralPool)
        {
            Data_Pool.m_GeneralPlayerPool.f_AddPlayer(tPoolDataDT);
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE,EM_FriendListType.Friend);
    }
    //好友列表处理
    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        RC_InitFriend tServerData = (RC_InitFriend)Obj;
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.userId);
        if (tPoolDataDT != null)
        {
            tPoolDataDT.f_SaveBase(tServerData.m_UserView.m_szName, tServerData.m_UserView.uSex, tServerData.m_UserView.uFrameId, 
                tServerData.m_UserView.uVipLv, tServerData.m_UserView.uTitleId);
            tPoolDataDT.f_SaveExtrend(tServerData.m_UserExtra.szLegionName, tServerData.m_UserExtra.iLv, 
                tServerData.m_UserExtra.iBattlePower, tServerData.m_UserExtra.offlineTime);
            tPoolDataDT.f_UpdateDungeonStar(tServerData.m_UserExtra.iDungeonStars);
            f_SortDataListByType(EM_FriendListType.Friend);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Friend);
        }
        else
        {
MessageBox.ASSERT("ID does not exist");
        }
    }

    //移除好友
    private void f_Callback_RemoveFriend(object result)
    {
        stPoolDelData tServerData = (stPoolDelData)result;
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.iId);
        if (tPoolDataDT != null)
        {
            //移除好友删除精力数据
            f_CheckVigorInfoInRemoveFriend(tPoolDataDT);
            f_RemoveDataListByType(EM_FriendListType.Friend, tServerData.iId);
            f_SortDataListByType(EM_FriendListType.Friend);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Friend);
        }
        else
        {
MessageBox.ASSERT("ID does not exist");
        }
    }

    #endregion

    #region 好友申请列表相关

    private void Callback_SocketData_ApplyUpdate(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            if (iData1 == (int)eUpdateNodeType.node_add)
            {
                f_Socket_ApplyAddData(tData, true);
            }
            else if (iData1 == (int)eUpdateNodeType.node_update)
            {
MessageBox.ASSERT("No update for the request list");
            }
            else if (iData1 == (int)eUpdateNodeType.node_default)
            {
                f_Socket_ApplyAddData(tData, false);
            }
            else if (iData1 == (int)eUpdateNodeType.node_delete)
            {
                f_Socket_ApplyRemoveData(tData);
            }
        }
    }

    private void f_Socket_ApplyAddData(SockBaseDT obj, bool bNew)
    {
        stPoolDelData tServerData = (stPoolDelData)obj;
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.iId);
        if (tPoolDataDT != null)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.FriendApply);
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FriendApply);
            f_AddDataListByType(EM_FriendListType.Apply, tPoolDataDT);
            f_SortDataListByType(EM_FriendListType.Apply);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Apply);
        }
        else
        {
MessageBox.ASSERT("ID does not exist");
        }
    }

    private void f_Socket_ApplyRemoveData(SockBaseDT obj)
    {
        stPoolDelData tServerData = (stPoolDelData)obj;
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.iId);
        if (tPoolDataDT != null)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.FriendApply);
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FriendApply);
            f_RemoveDataListByType(EM_FriendListType.Apply, tPoolDataDT.iId);
            f_SortDataListByType(EM_FriendListType.Apply);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Apply);
        }
        else
        {
MessageBox.ASSERT("ID does not exist");
        }
    }

    #endregion

    #region 黑名单相关

    private void Callback_SocketData_BlacklistUpdate(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            if (iData1 == (int)eUpdateNodeType.node_add)
            {
                f_Socket_BlacklistAddData(tData, true);
            }
            else if (iData1 == (int)eUpdateNodeType.node_update)
            {
MessageBox.ASSERT("No blacklist update");
            }
            else if (iData1 == (int)eUpdateNodeType.node_default)
            {
                f_Socket_BlacklistAddData(tData, false);
            }
            else if (iData1 == (int)eUpdateNodeType.node_delete)
            {
                f_Socket_BlacklistRemoveData(tData);
            }
        }
    }

    private void f_Socket_BlacklistAddData(SockBaseDT obj, bool bNew)
    {
        stPoolDelData tServerData = (stPoolDelData)obj;
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.iId);
        if (tPoolDataDT != null)
        {
            f_AddDataListByType(EM_FriendListType.Blacklist, tPoolDataDT);
            f_SortDataListByType(EM_FriendListType.Blacklist);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Blacklist);
        }
        else
        {
MessageBox.ASSERT("ID does not exist");
        }
    }

    private void f_Socket_BlacklistRemoveData(SockBaseDT obj)
    {
        stPoolDelData tServerData = (stPoolDelData)obj;
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.iId);
        if (tPoolDataDT != null)
        {
            f_RemoveDataListByType(EM_FriendListType.Blacklist, tPoolDataDT.iId);
            f_SortDataListByType(EM_FriendListType.Blacklist);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Blacklist);
        }
        else
        {
MessageBox.ASSERT("ID does not exist");
        }
    }

    #endregion

    #region 红点相关
    public void f_CheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.FriendVigor);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.FriendApply);
        if (mFriendDataDic.ContainsKey((int)EM_FriendListType.Vigor))
        {
            List<BasePoolDT<long>> listPoolDT = mFriendDataDic[(int)EM_FriendListType.Vigor];
            for (int i = 0; i < listPoolDT.Count; i++)
            {
                BasePlayerPoolDT basePlayerPoolDT = listPoolDT[i] as BasePlayerPoolDT;
                if (basePlayerPoolDT != null && basePlayerPoolDT.mCanRecvVigor)
                {
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.FriendVigor);
                }
            }
        }
        if (mFriendDataDic.ContainsKey((int)EM_FriendListType.Apply))
        {
            List<BasePoolDT<long>> listPoolDT = mFriendDataDic[(int)EM_FriendListType.Apply];
            for (int i = 0; i < listPoolDT.Count; i++)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.FriendApply);
            }
        }
    }
    #endregion
    #region 精力相关

    /// <summary>
    /// 已领取领取精力的次数
    /// </summary>
    public int mRecvVigorNum
    {
        get;
        private set;
    }

    /// <summary>
    /// 上次领取精力次数
    /// </summary>
    public int mLastRecvVigorNum
    {
        get;
        private set;
    }

    /// <summary>
    /// 是否领取次数已满
    /// </summary>
    public bool f_CheckRecvVigorIsFull()
    {
        return mRecvVigorNum >= GameParamConst.RecvVigorMaxNum;
    }

    private void Callback_SocketData_VigorUpdate(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            if (iData1 == (int)eUpdateNodeType.node_add)
            {
                f_Socket_VigorAddData(tData, true);
            }
            else if (iData1 == (int)eUpdateNodeType.node_update)
            {   
                f_Socket_VigorUpdateData(tData);
            }
            else if (iData1 == (int)eUpdateNodeType.node_default)
            {
                f_Socket_VigorAddData(tData, false);
            }
            else if (iData1 == (int)eUpdateNodeType.node_delete)
            {
MessageBox.ASSERT("No update operation for getting stamina");
            }
        }
    }

    private void f_Socket_VigorAddData(SockBaseDT obj, bool bNew)
    {
        CMsg_RTC_VigorHistory tServerData = (CMsg_RTC_VigorHistory)obj;
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.friendId);
        if (tPoolDataDT == null)
        {
            //保存删除好友的精力数据
            if (tServerData.uMask == (byte)EM_VigorFlag.Donate)
                f_SaveDonateVigorInfo(tServerData);
            else
                f_SaveOtherVigorInfo(tServerData);
            return;
        }
        if (tServerData.uMask == (byte)EM_VigorFlag.Donate)
        {
            tPoolDataDT.f_UpdateVigorByServer(tServerData.uMask,tServerData.uTime);
            f_SortDataListByType(EM_FriendListType.Friend);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Vigor);
        }
        else
        {
            tPoolDataDT.f_UpdateVigorByServer(tServerData.uMask,tServerData.uTime);
            if (tServerData.uMask == (byte)EM_VigorFlag.CanGet)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.FriendVigor);
                Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FriendVigor);
                f_AddDataListByType(EM_FriendListType.Vigor, tPoolDataDT);
                f_SortDataListByType(EM_FriendListType.Vigor);
                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Vigor);
            }
            else if (tServerData.uMask == (byte)EM_VigorFlag.AlreadyGet)
            {
MessageBox.DEBUG(string.Format("Received energy from {0}",tPoolDataDT.m_szName));
            }
            else
MessageBox.ASSERT("Spirit data does not exist in uMask:" + tServerData.uMask);
        }
    }

    private void f_Socket_VigorUpdateData(SockBaseDT obj)
    {
        CMsg_RTC_VigorHistory tServerData = (CMsg_RTC_VigorHistory)obj;
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.friendId);
        if (tPoolDataDT == null)
        {
MessageBox.ASSERT("ID does not exist");
            return;
        }
        tPoolDataDT.f_UpdateVigorByServer(tServerData.uMask,tServerData.uTime);
        if (tServerData.uMask == (byte)EM_VigorFlag.AlreadyGet)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.FriendVigor); 
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FriendVigor);
            f_RemoveDataListByType(EM_FriendListType.Vigor, tPoolDataDT.iId);
            f_SortDataListByType(EM_FriendListType.Vigor);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Vigor);
        }
    }

    private Dictionary<long, FriendVigorInfo> _donateVigorInfo;
    private Dictionary<long, FriendVigorInfo> _otherVigorInfo;

    private void f_SaveDonateVigorInfo(CMsg_RTC_VigorHistory serverData)
    {
        if (_donateVigorInfo.ContainsKey(serverData.friendId))
        {
            _donateVigorInfo[serverData.friendId].f_UpdateInfo(serverData.uMask, serverData.uTime);
        }
        else
        {
            FriendVigorInfo tInfo = new FriendVigorInfo(serverData.friendId);
            tInfo.f_UpdateInfo(serverData.uMask, serverData.uTime);
            _donateVigorInfo.Add(tInfo.m_FriendId,tInfo);
        }
    }

    private void f_SaveOtherVigorInfo(CMsg_RTC_VigorHistory serverData)
    {
        if (_otherVigorInfo.ContainsKey(serverData.friendId))
        {
            _otherVigorInfo[serverData.friendId].f_UpdateInfo(serverData.uMask, serverData.uTime);
        }
        else
        {
            FriendVigorInfo tInfo = new FriendVigorInfo(serverData.friendId);
            tInfo.f_UpdateInfo(serverData.uMask, serverData.uTime);
            _otherVigorInfo.Add(tInfo.m_FriendId, tInfo);
        }
    }

    /// <summary>
    /// 添加好友时检查是否存在精力数据
    /// </summary>
    /// <param name="poolDt"></param>
    private void f_CheckVigorInfoInAddFriend(BasePlayerPoolDT poolDt)
    {
        if (_donateVigorInfo.ContainsKey(poolDt.iId))
        {
            poolDt.f_UpdateVigorByServer(_donateVigorInfo[poolDt.iId].m_Mask, _donateVigorInfo[poolDt.iId].m_Time);
        }
        if (_otherVigorInfo.ContainsKey(poolDt.iId))
        {
            poolDt.f_UpdateVigorByServer(_otherVigorInfo[poolDt.iId].m_Mask, _otherVigorInfo[poolDt.iId].m_Time);
            if (_otherVigorInfo[poolDt.iId].m_Mask == (byte)EM_VigorFlag.CanGet)
            {
                f_AddDataListByType(EM_FriendListType.Vigor, poolDt);
                f_SortDataListByType(EM_FriendListType.Vigor);
                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Vigor);
            }
        }
        else
        {
            //如果之前已存在精力数据而且是可领取状态就添加的精力列表
            if (poolDt.mCanRecvVigor == true)
            {
                f_AddDataListByType(EM_FriendListType.Vigor, poolDt);
                f_SortDataListByType(EM_FriendListType.Vigor);
                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Vigor);
            }
        }
    }

    /// <summary>
    /// 移除好友时 移除Vior数据列表
    /// </summary>
    /// <param name="poolDt"></param>
    private void f_CheckVigorInfoInRemoveFriend(BasePlayerPoolDT poolDt)
    {
        f_RemoveDataListByType(EM_FriendListType.Vigor, poolDt.iId);
        f_SortDataListByType(EM_FriendListType.Vigor);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Vigor);
    }

    #endregion


    /// <summary>
    /// 根据类型添加数据
    /// </summary>
    private void f_AddDataListByType(EM_FriendListType type,BasePlayerPoolDT dt)
    {
        if (mFriendDataDic.ContainsKey((int)type))
        {
            mFriendDataDic[(int)type].Add(dt);
        }
        else
MessageBox.ASSERT("FriendPool Gets data from non-existent type" + type.ToString());
    }

    /// <summary>
    /// 根据类型移除数据
    /// </summary>
    private void f_RemoveDataListByType(EM_FriendListType type,long playerId)
    {
        if (mFriendDataDic.ContainsKey((int)type))
        {
            int removNum = mFriendDataDic[(int)type].RemoveAll(delegate(BasePoolDT<long> item){ return item.iId == playerId; });
MessageBox.DEBUG("The number you deleted：" + removNum);
        }
        else
MessageBox.ASSERT("FriendPool Gets data from non-existent type" + type.ToString());
    }
    /// <summary>
    /// 排序规则 
    /// 好友排序：
    /// 未赠送精力的在线玩家>未赠送精力的离线玩家>已赠送精力的玩家
    /// 同类型的对比战斗力，战斗力高的排上面
    /// 
    /// 其他排序：
    /// 按照玩家的战斗力排序，战斗力高的排上面
    /// </summary>
    private void f_SortDataListByType(EM_FriendListType type)
    {
        if (type == EM_FriendListType.Friend)
        {
            f_GetDataListByType(type).Sort(delegate (BasePoolDT<long> node1, BasePoolDT<long> node2)
            {
                BasePlayerPoolDT item1 = (BasePlayerPoolDT)node1;
                BasePlayerPoolDT item2 = (BasePlayerPoolDT)node2;
                if (item1.mCanDonateVigor && !item2.mCanDonateVigor)
                    return -1;
                else if (!item1.mCanDonateVigor && item2.mCanDonateVigor)
                    return 1;
                else
                {
                    if (item1.m_iOfflineTime == 0 && item2.m_iOfflineTime > 0)
                        return -1;
                    else if (item1.m_iOfflineTime > 0 && item2.m_iOfflineTime == 0)
                        return 1;
                    else
                    {
                        if (item1.m_iBattlePower > item2.m_iBattlePower)
                            return -1;
                        else if (item1.m_iBattlePower < item2.m_iBattlePower)
                            return 1;
                    }
                }
                return 0;
            });
        }
        else
        {
            f_GetDataListByType(type).Sort(delegate (BasePoolDT<long> node1, BasePoolDT<long> node2)
            {
                BasePlayerPoolDT item1 = (BasePlayerPoolDT)node1;
                BasePlayerPoolDT item2 = (BasePlayerPoolDT)node2;
                if (item1.m_iBattlePower > item2.m_iBattlePower)
                    return -1;
                else if (item1.m_iBattlePower < item2.m_iBattlePower)
                    return 1;
                return 0;
            });
        }
    }

    #region 初始化相关

    /// <summary>
    /// 执行初始化后执行Callback
    /// </summary>
    /// <param name="type"></param>
    /// <param name="callbackHandle"></param>
    public void f_ExecuteAfterInit(EM_FriendListType type, ccCallback callbackHandle)
    {
        int tKey = (int)type;
        if (mFriendDataInitDic[tKey])
        {
            if (callbackHandle != null)
                callbackHandle(eMsgOperateResult.OR_Succeed);
        }
        else
        {
            mFriendDataInitCallbackDic[tKey] = callbackHandle;
            if (type == EM_FriendListType.Friend)
            {
                SocketCallbackDT tCallbackDt = new SocketCallbackDT();
                tCallbackDt.m_ccCallbackSuc = f_Callback_InitFriendResult;
                tCallbackDt.m_ccCallbackFail = f_Callback_InitFriendResult;
                f_InitFriend(tCallbackDt);
            }
            else if (type == EM_FriendListType.Apply)
            {
                SocketCallbackDT tCallbackDt = new SocketCallbackDT();
                tCallbackDt.m_ccCallbackSuc = f_Callback_InitFriendApplyResult;
                tCallbackDt.m_ccCallbackFail = f_Callback_InitFriendApplyResult;
                f_InitFriendApply(tCallbackDt);
            }
            else if (type == EM_FriendListType.Blacklist)
            {
                SocketCallbackDT tCallbackDt = new SocketCallbackDT();
                tCallbackDt.m_ccCallbackSuc = f_Callback_InitBlacklistResult;
                tCallbackDt.m_ccCallbackFail = f_Callback_InitBlacklistResult;
                f_InitBlack(tCallbackDt);
            }
            else if (type == EM_FriendListType.Vigor)
            {
                SocketCallbackDT tCallbackDt = new SocketCallbackDT();
                tCallbackDt.m_ccCallbackSuc = f_Callback_InitVigorResult;
                tCallbackDt.m_ccCallbackFail = f_Callback_InitVigorResult;
                f_InitVigor(tCallbackDt);
            }
            else
            {
MessageBox.ASSERT("Friendly Pool initialized, but no handler：" + type.ToString() + ", please add handler");
            }
        }
    }

    private void f_Callback_InitFriendResult(object result)
    {
        int tKey = (int)EM_FriendListType.Friend;
        mFriendDataInitDic[tKey] = true;
        if (mFriendDataInitCallbackDic[tKey] != null)
            mFriendDataInitCallbackDic[tKey](result);
    }
    
    private void f_Callback_InitFriendApplyResult(object result)
    {
        int tKey = (int)EM_FriendListType.Apply;
        mFriendDataInitDic[tKey] = true;
        if (mFriendDataInitCallbackDic[tKey] != null)
            mFriendDataInitCallbackDic[tKey](result);
    }

    private void f_Callback_InitBlacklistResult(object result)
    {
        int tKey = (int)EM_FriendListType.Blacklist;
        mFriendDataInitDic[tKey] = true;
        if (mFriendDataInitCallbackDic[tKey] != null)
            mFriendDataInitCallbackDic[tKey](result);
    }

    private void f_Callback_InitVigorResult(object result)
    {
        int tKey = (int)EM_FriendListType.Vigor;
        mFriendDataInitDic[tKey] = true;
        if (mFriendDataInitCallbackDic[tKey] != null)
            mFriendDataInitCallbackDic[tKey](result);
    }

    #endregion

    #region 对外接口
    
    /// <summary>
    /// 玩家已拥有的好友数
    /// </summary>
    public int mHaveFriendNum
    {
        get
        {
            return mFriendDataDic[(int)EM_FriendListType.Friend].Count;
        }
    }

    /// <summary>
    /// 检查好友是否已满
    /// </summary>
    /// <returns></returns>
    public bool f_CheckIsFull()
    {
        return mHaveFriendNum >= GameParamConst.FriendMaxNum;
    }

    /// <summary>
    /// 检查是否是好友
    /// </summary>
    /// <returns>true：是好友  false：不是好友</returns>
    public bool f_CheckIsFriend(long playerId)
    {
        BasePoolDT<long> tItem = mFriendDataDic[(int)EM_FriendListType.Friend].Find(
            delegate (BasePoolDT<long> item) { return item.iId == playerId; }
            );
        return tItem != null;
    }

    /// <summary>
    /// 检查是否是好友
    /// </summary>
    public bool f_CheckIsFriend(string playerName)
    {
        BasePoolDT<long> tItem = mFriendDataDic[(int)EM_FriendListType.Friend].Find(
            delegate (BasePoolDT<long> item) {
                BasePlayerPoolDT tInfo = (BasePlayerPoolDT)item;
                return tInfo.m_szName == playerName;
            }
            );
        return tItem != null;
    }

    /// <summary>
    /// 检查是否在申请队列中
    /// </summary>
    public bool f_CheckIsInApplyList(long playerId)
    {
        BasePoolDT<long> tItem = mFriendDataDic[(int)EM_FriendListType.Apply].Find(
            delegate (BasePoolDT<long> item) { return item.iId == playerId; }
            );
        return tItem != null;
    }

    /// <summary>
    /// 检查是否在申请队列中
    /// </summary>
    public bool f_CheckIsInApplyList(string playerName)
    {
        BasePoolDT<long> tItem = mFriendDataDic[(int)EM_FriendListType.Apply].Find(
            delegate (BasePoolDT<long> item)
            {
                BasePlayerPoolDT tInfo = (BasePlayerPoolDT)item;
                return tInfo.m_szName == playerName;
            }
            );
        return tItem != null;
    }

    /// <summary>
    /// 检查是否已在黑名单
    /// </summary>
    public bool f_CheckIsInBlackList(long playerId)
    {
        BasePoolDT<long> tItem = mFriendDataDic[(int)EM_FriendListType.Blacklist].Find(
            delegate (BasePoolDT<long> item) { return item.iId == playerId; }
            );
        return tItem != null;
    }

    /// <summary>
    /// 检查是否已在黑名单
    /// </summary>
    public bool f_CheckIsInBlackList(string playerName)
    {
        BasePoolDT<long> tItem = mFriendDataDic[(int)EM_FriendListType.Blacklist].Find(
            delegate (BasePoolDT<long> item)
            {
                BasePlayerPoolDT tInfo = (BasePlayerPoolDT)item;
                return tInfo.m_szName == playerName;
            }
            );
        return tItem != null;
    }

    /// <summary>
    /// 根据类型获取数据列表
    /// </summary>
    public List<BasePoolDT<long>> f_GetDataListByType(EM_FriendListType type)
    {
        if (mFriendDataDic.ContainsKey((int)type))
        {
            return mFriendDataDic[(int)type];
        }
        else
        {
MessageBox.ASSERT("FriendPool retrieves data from non-existent type" + type.ToString());
            return new List<BasePoolDT<long>>();
        }
    }

    #endregion

    #region 发送协议

    /// <summary>
    /// 初始化好友列表
    /// </summary>
    public void f_InitFriend(SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_InitFriend, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_InitFriend, bBuf);
    }

    /// <summary>
    /// 初始化好友申请列表
    /// </summary>
    public void f_InitFriendApply(SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_InitFriendApply, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_InitFriendApply, bBuf);
    }

    /// <summary>
    /// 请求推荐好友
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_RecomFriend(SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_RecomFriend, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_RecomFriend, bBuf);
    }

    /// <summary>
    /// 请求申请加好友
    /// </summary>
    public void f_ApplyFriend(long playerId,SocketCallbackDT tSocketCallbackDT)
    {
        f_UpdateRefreshRecommonedList(playerId);
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_ApplyFriend, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(playerId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_ApplyFriend, bBuf);
    }

    /// <summary>
    /// 通过名字请求申请加好友
    /// </summary>
    public void f_ApplyFriendByName(string playerName,SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_ApplyFriendByName, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(playerName,28);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_ApplyFriendByName, bBuf);
    }

    /// <summary>
    /// 好友申请回应
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="result">0=拒绝</param>
    public void f_ReplyFriend(long playerId,byte accept,SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_ReplyFriend, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(playerId);
        tCreateSocketBuf.f_Add(accept);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_ReplyFriend, bBuf);
    }

    /// <summary>
    /// 请求移除好友
    /// </summary>
    public void f_RemoveFriend(long playerId,SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_RemoveFriend, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(playerId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_RemoveFriend, bBuf);
    }

    /// <summary>
    /// 请求初始化精力数据
    /// </summary>
    public void f_InitVigor(SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_VigorHistory, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_VigorHistory, bBuf);
    }

    /// <summary>
    /// 请求赠送精力
    /// </summary>
    public void f_SendVigor(long playerId,SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_SendVigor, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(playerId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_SendVigor, bBuf);
    }
    
    /// <summary>
    /// 请求领取精力   playerId = 0 表示一键领取
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_RecvVigor(long playerId,SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_RecvVigor, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(playerId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_RecvVigor, bBuf);
    }

    /// <summary>
    /// 请求初始化黑名单
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_InitBlack(SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_InitBlack, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_InitBlack, bBuf);
    }

    /// <summary>
    /// 请求拉黑
    /// </summary>
    public void f_Blacklist(long playerId, SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_Black, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(playerId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_Black, bBuf);
    }

    /// <summary>
    /// 请求移除黑名单
    /// </summary>
    public void f_Disblacklist(long playerId, SocketCallbackDT tSocketCallbackDT)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)ChatSocketCommand.CR_Disblack, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(playerId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_Disblack, bBuf);
    }

    #endregion


    #region 推荐好友相关

    /// <summary>
    /// 推荐好友数据返回
    /// </summary>
    private void f_Callback_RecomFriend(object result)
    {
        int tKey = (int)EM_FriendListType.Recommend;
        mFriendDataDic[tKey].Clear();
        CMsg_RTC_RecomFriend tServerDT = (CMsg_RTC_RecomFriend)result;
        for (int i = 0; i < tServerDT.recomUserId.Length; i++)
        {
            if (tServerDT.recomUserId[i] != 0)
            {
                //好友，申请列表和黑名单 全部过滤
                if (f_CheckIsFriend(tServerDT.recomUserId[i]))
                    continue;
                else if (f_CheckIsInBlackList(tServerDT.recomUserId[i]))
                    continue;
                else if (f_CheckIsInApplyList(tServerDT.recomUserId[i]))
                    continue;
                BasePoolDT<long> tPoolDT = Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerDT.recomUserId[i]);
                if (tPoolDT != null)
                    mFriendDataDic[tKey].Add(tPoolDT);
                else
MessageBox.ASSERT("ID not found:"+ tServerDT.recomUserId[i]);
            }
            else
MessageBox.ASSERT("ID not found:" + tServerDT.recomUserId[i]);
        }
    }

    //推荐列表刷新时间戳
    public int mRefreshRecommendTime
    {
        private set;
        get;
    }
    private ccCallback _callback_RefreshRecommend;

    /// <summary>
    /// 刷新推荐好友列表
    /// </summary>
    /// <param name="finishHandle"></param>
    /// <param name="force">是否强制更新</param>
    public void f_RefreshRecommendData(ccCallback finishHandle, bool force)
    {
        int tNow = GameSocket.GetInstance().f_GetServerTime();
        if (tNow - mRefreshRecommendTime >= GameParamConst.RecommendRefreshInterval)
        {
            if (force)
            {
                _callback_RefreshRecommend = finishHandle;
                SocketCallbackDT tCallBackDT = new SocketCallbackDT();
                tCallBackDT.m_ccCallbackSuc = f_RefreshRecommonedSuc;
                tCallBackDT.m_ccCallbackFail = f_RefreshRecommonedFail;
                f_RecomFriend(tCallBackDT);
            }
            else
            {
                if (f_GetDataListByType(EM_FriendListType.Recommend).Count > 0)
                {
                    if (finishHandle != null)
                        finishHandle(eMsgOperateResult.OR_Succeed);
                }
                else
                {
                    _callback_RefreshRecommend = finishHandle;
                    SocketCallbackDT tCallBackDT = new SocketCallbackDT();
                    tCallBackDT.m_ccCallbackSuc = f_RefreshRecommonedSuc;
                    tCallBackDT.m_ccCallbackFail = f_RefreshRecommonedFail;
                    f_RecomFriend(tCallBackDT);
                }
            }
        }
        else
        {
            if (finishHandle != null)
                finishHandle(eMsgOperateResult.OR_Succeed);
        }
    }

    private void f_RefreshRecommonedSuc(object result)
    {
        mRefreshRecommendTime = GameSocket.GetInstance().f_GetServerTime();
        if (_callback_RefreshRecommend != null)
            _callback_RefreshRecommend(result);
    }

    private void f_RefreshRecommonedFail(object result)
    {
MessageBox.ASSERT("Nomination failed，code:" + result);
        mRefreshRecommendTime = GameSocket.GetInstance().f_GetServerTime();
        if (_callback_RefreshRecommend != null)
            _callback_RefreshRecommend(result);
    }

    /// <summary>
    /// 添加好友时触发，刷新推荐列表
    /// </summary>
    /// <param name="playerId"></param>
    private void f_UpdateRefreshRecommonedList(long playerId)
    {
        int removeNum = f_GetDataListByType(EM_FriendListType.Recommend).RemoveAll(delegate (BasePoolDT<long> item) 
        { return item.iId == playerId; });
        if (removeNum > 0)
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_FRIENDDATA_UPDATE, EM_FriendListType.Recommend);
    }

    #endregion

    public void f_CheckFriendRedDot()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.FriendApply);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.FriendVigor);
        List<BasePoolDT<long>> tApplyList = f_GetDataListByType(EM_FriendListType.Apply);
        List<BasePoolDT<long>> tVigorList = f_GetDataListByType(EM_FriendListType.Vigor);
        for (int i = 0; i < tApplyList.Count; i++)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.FriendApply);
        }
        for (int i = 0; i < tVigorList.Count; i++)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.FriendVigor);
        }
    }

}

public class FriendVigorInfo
{
    public FriendVigorInfo(long friendId)
    {
        m_FriendId = friendId;
    }

    public void f_UpdateInfo(byte mask,int time)
    {
        m_Mask = mask;
        m_Time = time;
    }

    public long m_FriendId
    {
        get;
        private set;
    }

    public byte m_Mask
    {
        get;
        private set;
    }

    public int m_Time
    {
        get;
        private set;
    }
}
