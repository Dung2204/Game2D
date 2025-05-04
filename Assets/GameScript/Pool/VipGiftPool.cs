using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;
using System.Collections.Generic;
/// <summary>
/// vip礼包
/// </summary>
public class VipGiftPool : BasePool
{
    public bool mIsGetToday = false;//当天是否已领取
    int mlastInitTime = 0;
    #region Pool数据管理
    public VipGiftPool() : base("VipGiftPoolDT", true)
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        InitData();
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    public void InitData()
    {
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_VipGiftSC.f_GetAll().Count; i++)
        {
            VipGiftPoolDT poolDT = new VipGiftPoolDT();
            poolDT.mVipGiftDT = glo_Main.GetInstance().m_SC_Pool.m_VipGiftSC.f_GetAll()[i] as VipGiftDT;
            poolDT.iId = poolDT.mVipGiftDT.iId;
            f_Save(poolDT);
        }
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void RegSocketMessage()
    {
        SC_VipGiftInfo scVipGiftInfo = new SC_VipGiftInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_VipGiftInfo, scVipGiftInfo, OnInfoCallback);
    }
    /// <summary>
    /// 回调
    /// </summary>
    private void OnInfoCallback(object value)
    {
        SC_VipGiftInfo scVipGiftInfo = (SC_VipGiftInfo)value;
        //mIsGetToday = scVipGiftInfo.iGet == 1 ? true : false;
        VipGiftPoolDT vipDT;
        for (int i = 0; i < f_GetAll().Count; i++)
        {
            vipDT = f_GetAll()[i] as VipGiftPoolDT;
            vipDT.mToDayGet = BitTool.BitTest(scVipGiftInfo.bGetFlag, (ushort)(i + 1));
        }
        mlastInitTime = GameSocket.GetInstance().f_GetServerTime();
        CheckRedPoint();
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    /// <summary>
    /// 检查红点
    /// </summary>
    public void CheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.VipGift);
        int vipExp = (int)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip);
        int tVipLv = 0;
        int tNeedExp = 0;
        UITool.f_GetVipLvAndNeedExp(vipExp, ref tVipLv, ref tNeedExp);
        List<BasePoolDT<long>> listData = f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            if (tVipLv == (listData[i] as VipGiftPoolDT).mVipGiftDT.iId)
            {
                //if (!mIsGetToday)
                //{
                //    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.VipGift);
                //    ActivityPage.SortAct(EM_ActivityType.VipGift);
                //}
                if (!(listData[i] as VipGiftPoolDT).mToDayGet)
                {
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.VipGift);
                    ActivityPage.SortAct(EM_ActivityType.VipGift);
                }
            }
        }
    }
    #endregion

    #region 外部接口
    /// <summary>
    /// 请求领取信息
    /// </summary>
    /// <param name="tSocketCallbackDT">信息回调</param>
    public void f_RequestInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (CommonTools.f_CheckSameDay(GameSocket.GetInstance().f_GetServerTime(), mlastInitTime))
        {
            CheckRedPoint();
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(null);
            return;
        }
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_VipGiftInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_VipGiftInfo, bBuf);
    }
    /// <summary>
    /// 请求领取
    /// </summary>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_GetAward(byte VipLv,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_VipGift, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        ////向服务器提交数据
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(VipLv);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_VipGift, bBuf);
    }
    #endregion
}
