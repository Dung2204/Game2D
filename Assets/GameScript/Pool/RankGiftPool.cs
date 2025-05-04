using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;

public class RankGiftPool : BasePool {
    /// <summary>
    /// 构造
    /// </summary>
    public RankGiftPool() : base("RankGiftPoolDT",true)
    {
        
    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        Init();
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void Init()
    {
        List<NBaseSCDT> listRankGiftSC = glo_Main.GetInstance().m_SC_Pool.m_RankGiftSC.f_GetAll();
        for(int i = 0; i < listRankGiftSC.Count; i++)
        {
            RankGiftDT rankGiftDT = listRankGiftSC[i] as RankGiftDT;
            RankGiftPoolDT rankGiftPoolDT = new RankGiftPoolDT();
            rankGiftPoolDT.iId = rankGiftDT.iId;
            rankGiftPoolDT.m_buyTimes = 0;
            rankGiftPoolDT.m_RankGiftDT = rankGiftDT;
            f_Save(rankGiftPoolDT);
        }
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void RegSocketMessage()
    {
        SC_RankInfo scRankInfo = new SC_RankInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_RankAllow, scRankInfo, Callback_RankAllow);
        SC_GiftInfo scGiftInfo = new SC_GiftInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_GiftBuy, scGiftInfo, Callback_GiftBuy_Update);
    }
    protected void Callback_RankAllow(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            OnRankGiftCallback(tData);
        }
    }

    /// <summary>
    /// 等级礼包数据回调
    /// </summary>
    /// <param name="date"></param>
    private void OnRankGiftCallback(object data)
    {
MessageBox.DEBUG("Gift package received");
        SC_RankInfo scRankInfo = (SC_RankInfo)data;
        List<BasePoolDT<long>> listAllData = f_GetAll();
        for (int i = 0; i < listAllData.Count; i++)
        {
            RankGiftPoolDT poolDT = listAllData[i] as RankGiftPoolDT;
            if (poolDT.m_RankGiftDT.iOpenLevel == scRankInfo.Rank)
            {
                poolDT.m_levelTime = scRankInfo.TimeStamp;
            }
        }
        f_CheckRedPoint();
    }
    /// <summary>
    /// 购买信息
    /// </summary>
    protected void Callback_GiftBuy_Update(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            f_Socket_UpdateGiftBuy(tData);
        }
        f_CheckRedPoint();
    }
    /// <summary>
    /// 更新礼包购买数量
    /// </summary>
    /// <param name="Obj"></param>
    protected void f_Socket_UpdateGiftBuy(SockBaseDT Obj)
    {
        SC_GiftInfo scGiftInfo = (SC_GiftInfo)Obj;
        RankGiftPoolDT rankGiftPoolDT = (RankGiftPoolDT)f_GetForId(scGiftInfo.GiftId);
        rankGiftPoolDT.m_buyTimes = scGiftInfo.Num;
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #region 外部接口
    /// <summary>
    /// 根据其等级获取其升级时间
    /// </summary>
    /// <param name="level">等级</param>
    /// <returns></returns>
    private int getLevelUpTime(int level)
    {
        List<BasePoolDT<long>> listRankGiftPoolDT = f_GetAll();
        for (int i = 0; i < listRankGiftPoolDT.Count; i++)
        {
            RankGiftPoolDT dt = listRankGiftPoolDT[i] as RankGiftPoolDT;
            if (dt.m_RankGiftDT.iOpenLevel == level)
            {
                return dt.m_levelTime;
            }
        }
        return 0;
    }
    /// <summary>
    /// 检查红点，有没有可购买的等级礼包
    /// </summary>
    private void f_CheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RankGiftBuy);
        int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        List<NBaseSCDT> listLevelData = glo_Main.GetInstance().m_SC_Pool.m_RankGiftSC.f_GetAll();
        for (int index = 0; index < listLevelData.Count; index++)
        {
            int level = (listLevelData[index] as RankGiftDT).iOpenLevel;
            int serverTime = GameSocket.GetInstance().f_GetServerTime();
            int levelUpTime = getLevelUpTime(level);
            int timeLeft = (24 * 60 * 60) - (serverTime - levelUpTime);
            RankGiftPoolDT poolDT = f_GetForId(listLevelData[index].iId) as RankGiftPoolDT;
            bool hasBuyTimeLeft = poolDT.m_buyTimes < poolDT.m_RankGiftDT.iBuyTime;
            if (userLevel >= level && hasBuyTimeLeft && timeLeft > 0)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RankGiftBuy);
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.RankGiftBuy);
                break;
            }
        }
    }
    /// <summary>
    /// 查询等级礼包
    /// </summary>
    public void f_RequestRankGiftInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if(tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_QueryRankGfit, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QueryRankGfit, bBuf);
    }
    /// <summary>
    /// 购买等级礼包
    /// </summary>
    public void f_BuyRankGift(int giftId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RankGift, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(giftId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RankGift, bBuf);
    }
    #endregion

    public bool f_CheckGiftIsOpen() {
        bool isOpen = false;
        int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        List<NBaseSCDT> listLevelData = glo_Main.GetInstance().m_SC_Pool.m_RankGiftSC.f_GetAll();
        for (int index = 0; index < listLevelData.Count; index++)
        {
            int level = (listLevelData[index] as RankGiftDT).iOpenLevel;
            int serverTime = GameSocket.GetInstance().f_GetServerTime();
            int levelUpTime = getLevelUpTime(level);
            int timeLeft = (24 * 60 * 60) - (serverTime - levelUpTime);
            RankGiftPoolDT poolDT = f_GetForId(listLevelData[index].iId) as RankGiftPoolDT;
            bool hasBuyTimeLeft = poolDT.m_buyTimes < poolDT.m_RankGiftDT.iBuyTime;
            if (userLevel >= level && hasBuyTimeLeft && timeLeft > 0)
            {
                isOpen = true;
                break;
            }
        }

        return isOpen;
    }
}
