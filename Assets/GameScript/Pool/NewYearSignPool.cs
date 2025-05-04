using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;
using System.Collections.Generic;
/// <summary>
/// 签到
/// </summary>
public class NewYearSignPool : BasePool
{
    public int signedCount;//已经签到过的数量
    public int lastSignTime;//上一次签到时间
    private int lastInfoTime;
    #region Pool数据管理
    public NewYearSignPool() : base("", true)
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        signedCount = 0;
        lastSignTime = 0;
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void RegSocketMessage()
    {
        SC_NewYearSignInfo scNewYearSignInfo = new SC_NewYearSignInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_NewYearSignInfo, scNewYearSignInfo, OnNewYearSignInfoCallback);
    }
    /// <summary>
    /// 返回是否签到消息
    /// </summary>
    /// <param name="data"></param>
    private void OnNewYearSignInfoCallback(object data)
    {
        SC_NewYearSignInfo scNewYearSignInfo = (SC_NewYearSignInfo)data;
        signedCount = scNewYearSignInfo.eSignedDays;
        lastSignTime = scNewYearSignInfo.lastSignedTime;
        lastInfoTime = GameSocket.GetInstance().f_GetServerTime();
        CheckRedPoint();
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    public bool isSeeGrandSignPage = false;//是否看了签到页面（看一次消失红点）
    /// <summary>
    /// 检查红点
    /// </summary>
    public void CheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.NewYearSign);

        GameParamDT gameParamDT = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearSign) as GameParamDT);
        if (!CommonTools.f_CheckTime(gameParamDT.iParam1.ToString(), gameParamDT.iParam2.ToString()))//活动未开启
        {
            return;
        }

        GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
        int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (userLevel < gameParamDTOpenLevel.iParam1)
        {
            return;
        }
        if (glo_Main.GetInstance().m_SC_Pool.m_SignedSC.f_GetAll().Count > signedCount)
        {
            int rechageCount = 30;
            List<NBaseSCDT> listSignSC = glo_Main.GetInstance().m_SC_Pool.m_NewYearSignSC.f_GetAll();
            for (int i = 0; i < listSignSC.Count; i++)
            {
                if (signedCount == i)
                {
                    NewYearSignDT signedDT = listSignSC[i] as NewYearSignDT;
                    rechageCount = signedDT.iRechargeNum;
                }
            }
            if (Data_Pool.m_RechargePool.f_GetAllRechageMoneyToday() >= rechageCount)
            {
                if (!CommonTools.f_CheckSameDay(GameSocket.GetInstance().f_GetServerTime(), lastSignTime))
                {
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.NewYearSign);
                    NewYearActPage.SortAct(EM_NewYearActType.NewYearSign);
                }
            }
            else
            {
                if (!isSeeGrandSignPage)
                {
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.NewYearSign);
                    NewYearActPage.SortAct(EM_NewYearActType.NewYearSign);
                }
            }
        }
    }
    #endregion

    #region 外部接口
    /// <summary>
    /// 请求信息
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    /// <returns></returns>
    public void f_RequestSignInfo(SocketCallbackDT tSocketCallbackDT)
    {
        CheckRedPoint();
        if (CommonTools.f_CheckSameDay(lastInfoTime, GameSocket.GetInstance().f_GetServerTime()))
        {
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
        }
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearSignInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        ////向服务器提交数据
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearSignInfo, bBuf);
    }
    /// <summary>
    /// 请求签到
    /// </summary>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_GetSign(SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearSign, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearSign, bBuf);
    }
    #endregion
}
