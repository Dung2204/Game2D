using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
/// <summary>
/// 在线奖励页面
/// </summary>
public class OnlineAwardPool : BasePool
{
    /// <summary>
    /// 在线时长(秒)
    /// </summary>
    public int m_timeSecondToday;
    /// <summary>
    /// 当前初始化的日期
    /// </summary>
    private int mCurrentInitDayIndex = -1;
    public OnlineAwardPool() : base("OnlineAwardPoolDT", true)
    {

    }
    protected override void f_Init()
    {
        
    }

    protected override void RegSocketMessage()
    {
        SC_OpenServOnlineAwardInfo scOpenServOnlineAwardInfo = new SC_OpenServOnlineAwardInfo();
        GameSocket.GetInstance().f_RegMessage_Int2((int)SocketCommand.SC_OpenServOnlineAwardInfo, scOpenServOnlineAwardInfo, Callback_OnlineInfo);
    }
    private void InitData(int dayIndex)
    {
        //同一天不需要重复初始化，否则会重置。
        if (dayIndex == mCurrentInitDayIndex)
            return;
        f_Clear();
        mCurrentInitDayIndex = dayIndex;
        List<NBaseSCDT> listOnlineAwardDT = glo_Main.GetInstance().m_SC_Pool.m_OnlineAwardSC.f_GetAll();
        //初始化数据
        for (int i = 0; i < listOnlineAwardDT.Count; i++)
        {
            OnlineAwardDT onlineAwardDT = listOnlineAwardDT[i] as OnlineAwardDT;
            bool isOpen = f_CheckTime(onlineAwardDT, dayIndex);
            if (isOpen)
            {
                OnlineAwardPoolDT onlineAwardPoolDT = new OnlineAwardPoolDT();
                onlineAwardPoolDT.iId = onlineAwardDT.iId;
                onlineAwardPoolDT.mTime = onlineAwardDT.iTime;
                onlineAwardPoolDT.m_isGet = false;
                onlineAwardPoolDT.m_OnlineAwardDT = onlineAwardDT;
                f_Save(onlineAwardPoolDT);
            }
        }
    }
    protected void Callback_OnlineInfo(int iData1, int iData2, int iNum, ArrayList aData)
    {
        int dayIndex = iData1;
        m_timeSecondToday = iData2;
        InitData(dayIndex);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.OnlineAward);
        foreach (SockBaseDT tData in aData)
        {
            SC_OpenServOnlineAwardInfo scOpenServOnlineAwardInfo = (SC_OpenServOnlineAwardInfo)tData;
            OnlineAwardPoolDT onlineAwardPoolDT = f_GetForId(scOpenServOnlineAwardInfo.Id) as OnlineAwardPoolDT;
            if (onlineAwardPoolDT == null)
            {
MessageBox.ASSERT("Error in data from server");
                continue;
            }
            else
            {
                onlineAwardPoolDT.m_isGet = scOpenServOnlineAwardInfo.state == 0 ? true : false;
            }
            if ((m_timeSecondToday / 60) >= onlineAwardPoolDT.mTime && !onlineAwardPoolDT.m_isGet)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.OnlineAward);
                ActivityPage.SortAct(EM_ActivityType.OnlineAward);
            }
        }
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #region 外部接口
    /// <summary>
    /// 查询在线奖励信息
    /// </summary>
    public void f_QueryInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if(tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_OpenServOnlineAwardInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_OpenServOnlineAwardInfo, bBuf);
    }
    /// <summary>
    /// 领取奖励
    /// </summary>
    public void f_GetAward(int id,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_OpenServOnlineAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_OpenServOnlineAward, bBuf);
    }
    /// <summary>
    /// 判断今天时间是否满足
    /// </summary>
    public bool f_CheckTime(OnlineAwardDT dt, int dayOfWeek)
    {
        if (dayOfWeek == 0 || dayOfWeek == 7)//兼容0和7，都表示星期天
        {
            return dt.iWeek == 0 || dt.iWeek == 7;
        }
        if (dt.iWeek == dayOfWeek)
            return true;
        return false;
    }
    #endregion
}
