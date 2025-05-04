using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;

public class SkyFortunePool : BasePool
{
    private int lastSignInfoTime = 0;
    public int mTimes;//已经祈福次数

    public short m_GetSyceeTime;//获取到的元宝倍数
    public List<string> listRecordFortune = new List<string>();//历史记录
    public bool isInitServ = false;//每次登陆只查询一次

    public int mOpenSevDay;//开服天数
    public SkyFortunePool() : base("", true)
    {

    }
    protected override void f_Init()
    {

    }

    protected override void RegSocketMessage()
    {
        SC_DescendFortuneInfo scDescendFortuneInfo = new SC_DescendFortuneInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_DescendFortuneInfo, scDescendFortuneInfo, CallbackInfo);
        SC_DescendFortune scDescendFortune = new SC_DescendFortune();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_DescendFortune, scDescendFortune, CallbackFortune);
        SC_DescendForuneRecord scDescendForuneRecord = new SC_DescendForuneRecord();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_DescendForuneRecord, scDescendForuneRecord, Callback_RecordData_Update);
    }
    /// <summary>
    /// 查询信息服务器回调
    /// </summary>
    /// <param name="data"></param>
    private void CallbackInfo(object data)
    {
        SC_DescendFortuneInfo scDescendFortuneInfo = (SC_DescendFortuneInfo)data;
        mTimes = scDescendFortuneInfo.num;
        mOpenSevDay = scDescendFortuneInfo.openServerDay;
        CheckRedPoint();
    }
    /// <summary>
    /// 祈福服务器回调
    /// </summary>
    /// <param name="data"></param>
    private void CallbackFortune(object data)
    {
        SC_DescendFortune scDescendFortune = (SC_DescendFortune)data;
        m_GetSyceeTime = scDescendFortune.times;
    }
    /// <summary>
    /// 抽奖记录回调
    /// </summary>
    protected void Callback_RecordData_Update(int iData1, int iData2, int iNum, ArrayList aData)
    {
        listRecordFortune.Clear();

        foreach (SockBaseDT tData in aData)
        {
            SC_DescendForuneRecord scDescendForuneRecord = (SC_DescendForuneRecord)tData;
listRecordFortune.Add("Chúc mừng [6FFF48FF][" + scDescendForuneRecord.szRoleName + "][FFFFFFFF] đã nhận x[F44B6AFF]"
+ (scDescendForuneRecord.iTimes * 1.0f / 100) + "[FFFFFFFF] KNB！");

        }
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #region 检查红点
    public void CheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SkyFortune);
        if (mOpenSevDay != 2 && mOpenSevDay != 8 && mOpenSevDay != 15 && mOpenSevDay != 30 && mOpenSevDay != 45 && mOpenSevDay != 60)
        {
            if (Data_Pool.m_SkyFortunePool.isInitServ)
                return;
        }
        int vipLevel = UITool.f_GetNowVipLv();
        int times = Data_Pool.m_SkyFortunePool.mTimes;//已经祈福的次数
        int CanFortuneTimes = 0;//当前vip可祈福总次数
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_DescendFortuneSC.f_GetAll();
        int curIndex = 0;
        for (int i = 0; i < listData.Count; i++)
        {
            DescendFortuneDT dt = listData[i] as DescendFortuneDT;
            if (i == times)
                curIndex = dt.iId;
            if (dt.iVip <= vipLevel)
                CanFortuneTimes++;
        }
        if (times < CanFortuneTimes)//可祈福
        {
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SkyFortune);
            ActivityPage.SortAct(EM_ActivityType.SkyFortune);
        }
    }
    #endregion
    #region 外部接口
    /// <summary>
    /// 查询信息
    /// </summary>
    public void f_QueryInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (!CommonTools.f_CheckSameDay(lastSignInfoTime, GameSocket.GetInstance().f_GetServerTime()))
        {
            isInitServ = false;
        }
        if (isInitServ)
        {
            CheckRedPoint();
            return;
        }
        isInitServ = true;
        lastSignInfoTime = GameSocket.GetInstance().f_GetServerTime();
        if(tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DescendFortuneInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);

        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DescendFortuneInfo, bBuf);
    }
    /// <summary>
    /// 查询抽奖记录
    /// </summary>
    public void f_QueryRecord(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DescendForuneRecord, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DescendForuneRecord, bBuf);
    }
    /// <summary>
    /// 祈福
    /// </summary>
    public void f_Fortune(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DescendFortune, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DescendFortune, bBuf);
    }
    #endregion
}
