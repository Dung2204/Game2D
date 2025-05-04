using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;

public class WeekFundPool: BasePool
{
    private int m_CurWeekFundTypeId = 0;//类型ID
    public bool m_IsBought; //是否已购买
    public int m_AwardFlag;        //领奖标志,按位与
    private bool mIsInitForSer = false;//防止重复请求
    public WeekFundPool() : base("WeekFundPoolDT", true)
    {

    }
    //初始化数据
    protected override void f_Init()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_WeekFundSC.f_GetAll();
        for (int i =0; i< listDT.Count;i++)
        {
            WeekFundDT dt = listDT[i] as WeekFundDT;
            WeekFundPoolDT poolDT = new WeekFundPoolDT();
            poolDT.iId = dt.iId;
            poolDT.WeekFundData = dt;
            f_Save(poolDT);
        }
    }
    //注册消息
    protected override void RegSocketMessage()
    {
        SC_WeekFundInfo scWeekFundInfo = new SC_WeekFundInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_WeekFundInfo, scWeekFundInfo, CallbackWeekFundInfo);

        SC_WeekFundSign scWeekFundSign = new SC_WeekFundSign();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_WeekFundSign, scWeekFundSign, CallbackWeekFundSign);
    }

    /// <summary>
    /// 信息回调
    /// </summary>
    /// <param name="data"></param>
    private void CallbackWeekFundInfo(object data)
    {
        SC_WeekFundInfo scWeekFundData = (SC_WeekFundInfo)data;
        m_CurWeekFundTypeId = scWeekFundData.iId;
        WeekFundPoolDT weekFundPoolDT = (WeekFundPoolDT)f_GetForId((long)scWeekFundData.iId);
        m_IsBought = scWeekFundData.bFinish == 1;
        m_AwardFlag = scWeekFundData.awardFlag;
        mIsInitForSer = true;
        f_CheckWeekFundRedPoint();
    }

    /// <summary>
    /// 领取周基金回调
    /// </summary>
    /// <param name="data"></param>
    private void CallbackWeekFundSign(object data)
    {
        SC_WeekFundSign scWeekFundData = (SC_WeekFundSign)data;
        m_AwardFlag = scWeekFundData.awardFlag;
        f_CheckWeekFundRedPoint();
    }

    public WeekFundPoolDT f_GetCurWeekFundPoolDt()
    {
        return (WeekFundPoolDT)f_GetForId(m_CurWeekFundTypeId);
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }

    //查询周基金
    public void f_QueryWeekFundInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_WeekFundInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_WeekFundInfo, bBuf);
    }

    //领取周基金奖励
    private SocketCallbackDT tGetWeekFundAwardSocketCallbackDT;
    public void f_GetWeekFundAward(byte uDay, SocketCallbackDT tSocketCallbackDT)
    {
        tGetWeekFundAwardSocketCallbackDT = tSocketCallbackDT;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_WeekFundSign, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(uDay);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_WeekFundSign, bBuf);
    }

    //获取当前处于活动地第几天,0表示第一天，-1表示未在活动时间内
    public int f_GetCurDay()
    {        
        //如果购买了，则多3天领奖时间
        int totalDay = m_IsBought ? 10 : 7;
        int nowTime = GameSocket.GetInstance().f_GetServerTime();

        //第一条固定绑定开服时间，开服1-7天开启
        if (m_CurWeekFundTypeId == 1) {
            int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
            int openServerDay = ccMath.f_GetTotalDaysByTime(nowTime) - ccMath.f_GetTotalDaysByTime(openServerTime);
            if (openServerDay >= 0 && openServerDay < totalDay)
            {
                return openServerDay;
            }
            return -1;
        }

        //判断周基金其余类型周期活动内是否开起
        WeekFundPoolDT poolDT = (WeekFundPoolDT)f_GetForId(m_CurWeekFundTypeId);
        if (poolDT == null)
        {
            return -1;
        }
        string stStartTime = poolDT.WeekFundData.iActivityBegin.ToString();
        string stEendTime = m_IsBought ? poolDT.WeekFundData.iAwardEnd.ToString() :  poolDT.WeekFundData.iActivityEnd.ToString();
        DateTime startTime = GetTimeByTimeStr(poolDT.WeekFundData.iActivityBegin.ToString());        
        if (CommonTools.f_CheckTime(stStartTime, stEendTime))
        {
            return DateTime.Now.Subtract(startTime).Days;
        }
        return -1;
    }

    private DateTime GetTimeByTimeStr(string StrTime)
    {
        int bYear = int.Parse(StrTime.Substring(0, 4));
        int bMonth = int.Parse(StrTime.Substring(4, 2));
        int bDay = int.Parse(StrTime.Substring(6, 2));
        DateTime time = new DateTime(bYear, bMonth, bDay, 0,
            0, 0);
        return time;
    }

    //检测红点
    public void f_CheckWeekFundRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.WeekFund);
        if (f_GetCurDay() < 0 || !m_IsBought)
        {
            return;
        }
        int curDay = f_GetCurDay();
        for (int i = 0; i <= curDay; i++) {
            if (!BitTool.BitTest(m_AwardFlag, (ushort)(i + 1)))
            {
                ActivityPage.SortAct(EM_ActivityType.WeekFund);
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.WeekFund);
                Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.WeekFund);
                return;
            }
        }
     }
}