using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 商店抽牌pool
/// </summary>
public class ShopLotteryPool : BasePool
{
    private bool isInitServer = false;
    public ShopLotteryPool() : base("ShopLotteryPoolDT", true)
    {

    }
    /// <summary>
    /// 检测红点（战将和神将是否免费）
    /// </summary>
    public void CheckRedDot()
    {
        ShopLotteryPoolDT lotteryPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.NorAd) as ShopLotteryPoolDT;
        int secondLeft = lotteryPoolDT.shopLotteryDT.iFreeCD * 60 - (GameSocket.GetInstance().f_GetServerTime() - lotteryPoolDT.lastFreeTime);
        bool isFree = secondLeft < 0 ? true : false;
        if (isFree)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.NorAdmiralFree);
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.NorAdmiralFree);
        }
        else
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.NorAdmiralFree);

        ShopLotteryPoolDT lotteryPoolDT2 = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd) as ShopLotteryPoolDT;
        int secondLeft2 = lotteryPoolDT2.shopLotteryDT.iFreeCD * 60 - (GameSocket.GetInstance().f_GetServerTime() - lotteryPoolDT2.lastFreeTime);
        bool isFree2 = secondLeft2 < 0 ? true : false;
        if (isFree2)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.GenAdmiralFree);
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.GenAdmiralFree);
        }
        else
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.GenAdmiralFree);
    }
    #region Pool数据管理
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        InitLottery();
    }
    /// <summary>
    /// 初始化商城抽牌数据
    /// </summary>
    private void InitLottery()
    {
        List<NBaseSCDT> allShopLottery = glo_Main.GetInstance().m_SC_Pool.m_ShopLotterySC.f_GetAll();
        for (int i = 0; i < allShopLottery.Count; i++)
        {
            ShopLotteryDT dt = allShopLottery[i] as ShopLotteryDT;
            if (dt != null)
            {
                ShopLotteryPoolDT poolDt = new ShopLotteryPoolDT();
                poolDt.iId = dt.iId;
                poolDt.totalTimes = 0;
                poolDt.lastFreeTime = 0;
                poolDt.shopLotteryDT = dt;
                f_Save(poolDt);
            }
        }
    }
    protected override void RegSocketMessage()
    {
        SC_ShopLotteryInfo tSC_ShopLotteryInfo = new SC_ShopLotteryInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_ShopLotteryInfo, tSC_ShopLotteryInfo, Callback_SocketData_Update);

        SC_Award pAward = new SC_Award();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_ShopLotteryGetAward, pAward, OnGetAwardCallBack);
    }
    private void OnGetAwardCallBack(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    {
        List<AwardPoolDT> tAwardPoolList = new List<AwardPoolDT>();
        foreach (SC_Award item in aData)
        {
            AwardPoolDT taward = new AwardPoolDT();
            taward.f_UpdateByInfo(item.resourceType, item.resourceId, item.resourceNum);
            tAwardPoolList.Add(taward);
        }
        if (tAwardPoolList.Count > 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tAwardPoolList });
            tAwardPoolList.Clear();
        }
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        f_Socket_UpdateData(Obj);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_ShopLotteryInfo tServerData = (SC_ShopLotteryInfo)Obj;
        List<BasePoolDT<long>> AllListPoosDt = f_GetAll();
        bool isContain = false;
        for (int i = 0; i < AllListPoosDt.Count; i++)
        {
            ShopLotteryPoolDT data = AllListPoosDt[i] as ShopLotteryPoolDT;
            if (data.iId == tServerData.lotteryId)
            {
                isContain = true;
                data.totalTimes = tServerData.totalTimes;
                data.lastFreeTime = tServerData.lastFreeTime;
                data.totalHalfTimes = tServerData.totalHalfTimes;

                data.tempTotalTimes = tServerData.tempTotalTimes;
                data.award = tServerData.award;
                data.itemId = tServerData.itemId;
                data.iLock = tServerData.iLock;
                break;
            }
        }
        CheckRedDot();
        if (!isContain)
        {
Debug.LogError("ShopLotteryPool no data found");
        }
    }
    //1.抽牌2.合成3.宝箱
    #endregion
    #region 以下为外部调用接口
    /// <summary>
    /// 请求抽牌商店列表
    /// </summary>
    /// <param name="tSocketCallbackDT">请求回调</param>
    public void f_GetLotteryShopInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (isInitServer)
        {
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            CheckRedDot();
            return;
        }
        isInitServer = true;
        ////向服务器提交数据
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopLotteryInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopLotteryInfo, bBuf);
    }
    public EM_RecruitType lotteryId;
    /// <summary>
    /// 请求抽牌消息
    /// </summary>
    /// <param name="lotteryId">抽牌id</param>
    /// <param name="buyPara">购买模式  1=单次类型1 2=单次类型2 3=十连类型1 4=十连类型2</param>
    /// <param name="tSocketCallbackDT">购买回调</param>
    public void f_Buy(bool isFree,EM_RecruitType lotteryId, BuyMode buyPara, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        this.lotteryId = lotteryId;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopLotteryBuy, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((int)lotteryId);
        tCreateSocketBuf.f_Add((byte)(GetTypeByBuyMode(buyPara)));
        tCreateSocketBuf.f_Add((byte)(isFree ? 1 : 0));
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopLotteryBuy, bBuf);
    }
    public void f_CampBuy(EM_RecruitType lotteryId, BuyMode buyPara, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        this.lotteryId = lotteryId;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopLotteryCampBuy, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((int)lotteryId);
        tCreateSocketBuf.f_Add((byte)(GetTypeByBuyMode(buyPara)));
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopLotteryCampBuy, bBuf);
    }
    public void f_Choose(EM_RecruitType lotteryId, int itemId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopLotteryChoose, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((int)lotteryId);
        tCreateSocketBuf.f_Add(itemId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopLotteryChoose, bBuf);
    }
    public void f_GetAward(EM_RecruitType lotteryId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopLotteryGetAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((int)lotteryId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopLotteryGetAward, bBuf);
    }
    public void f_LotteryLimit(int iIdEventTime, int iIdLottery,int iType, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_LotteryLimit_Draw, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((int)iIdEventTime);
        tCreateSocketBuf.f_Add((int)iIdLottery);
        tCreateSocketBuf.f_Add((int)iType);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_LotteryLimit_Draw, bBuf);
    }
    /// <summary>
    /// 返回购买模式  1=单次类型1 2=单次类型2 3=十连类型1 4=十连类型2</param>
    /// </summary>
    /// <returns></returns>
    private int GetTypeByBuyMode(BuyMode buyMode)
    {
        switch (buyMode.em_buyMode)
        {
            case EM_BuyMode.BuyOne:
                if (buyMode.em_UserType == EM_UseType.Good)
                    return 1;
                else
                    return 2;
            case EM_BuyMode.BuyTen:
                if (buyMode.em_UserType == EM_UseType.Good)
                    return 3;
                else
                    return 4;
            default:
Debug.LogError("Error, undefined purchase mode！");
                return 1;
        }
    }
    public enum EM_UseType
    {
        Good,//使用道具购买
        Sycee,//使用元宝购买
    }
    /// <summary>
    /// 购买模式
    /// </summary>
    public enum EM_BuyMode
    {
        BuyOne,//抽一次
        BuyTen,//十连抽
    }
    public class BuyMode
    {
        public EM_UseType em_UserType;//消耗类型
        public EM_BuyMode em_buyMode;//购买模式
    }
    #endregion
}
