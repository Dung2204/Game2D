using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class FirstRechargePool : BasePool
{
    private bool mIsInitForSer = false;//防止重复请求
    private long timeAllReceived = 0;
    //TsuCode - FirstRechargeNew - NapDau
    private List<long> day = new List<long>();
    public long getDay(int index)
    {
        if (day.Count != 0)
        {
            if (index >= day.Count) MessageBox.ASSERT("TsuLog Day FirstRecharge out of range");
            return day[index];
        }
        return 0;

    }
    //-------------------------------------
    public FirstRechargePool() : base("FirstRechargePoolDT", true)
    {

    }

    protected override void f_Init()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_FirstRechargeSC.f_GetAll();
        for (int i = 0; i < listDT.Count; i++)
        {
            FirstRechargeDT dt = listDT[i] as FirstRechargeDT;
            FirstRechargePoolDT poolDT = new FirstRechargePoolDT();
            poolDT.iId = dt.iId;
            poolDT.mGetTimes = 0;
            poolDT.m_FirstRechargeDT = dt;
            f_Save(poolDT);
        }
    }
    protected override void RegSocketMessage()
    {
        SC_FirstRechargeInfo scFirstRechargeInfo = new SC_FirstRechargeInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_FirstRechargeInfo, scFirstRechargeInfo, Callback_Info);
    }
    /// <summary>
    /// 信息回调
    /// </summary>
    protected void Callback_Info(int iData1, int iData2, int iNum, ArrayList aData)
    {
        day.Clear();
        foreach (SockBaseDT tData in aData)
        {
            SC_FirstRechargeInfo scFirstRechargeInfo = (SC_FirstRechargeInfo)tData;
            //Tsucode - FirstRechargeNew - NapDau
            if (scFirstRechargeInfo.day != null)
                day.Add(scFirstRechargeInfo.day);
            
            FirstRechargePoolDT poolDT = f_GetForId(scFirstRechargeInfo.mId) as FirstRechargePoolDT;
            if (poolDT != null)
            {
              
                poolDT.mGetTimes = 1;
                //TsuCode - FirstRechargeNew - NapDau
                poolDT.received_1 = scFirstRechargeInfo.received_1;
                poolDT.received_2 = scFirstRechargeInfo.received_2;
                poolDT.received_3 = scFirstRechargeInfo.received_3;
                timeAllReceived = scFirstRechargeInfo.timeAllReceived;
                //
            }
        }
        //mIsInitForSer = true;
        f_CheckRedPoint();
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #region 检查红点
    public void f_CheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.FirstRecharge);
        if (!f_CheckIsOpen())
        {
            return;
        }
        if (CheckFirstRecharge())
        {
            // ActivityPage.SortAct(EM_ActivityType.FirstRecharge);
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.FirstRecharge);
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FirstRecharge);
        }
    }

    private bool CheckFirstRecharge()
    {
        if (getDay(0) != 0)
        {
            if (CheckRecharge(0))
            {
                return true;
            }
        }
        if (getDay(1) != 0)
        {
            if (CheckRecharge(1))
            {
                return true;
            }
        }
        if (getDay(2) != 0)
        {
            if (CheckRecharge(2))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckRecharge(int charge)
    {
        FirstRechargePoolDT poolDT = f_GetAll()[charge] as FirstRechargePoolDT;
        int dayRecharge = CommonTools.f_GetZeroTimestamp(getDay(charge)); ;
        int dayNow = GameSocket.GetInstance().f_GetServerTime();

        int plusday = (dayNow - dayRecharge)/86400;
        switch (plusday)
        {
            case 0:
                if (poolDT.received_1 == 0)
                {
                    return true;
                }
                break;
            case 1:
                if (poolDT.received_1 == 0 || poolDT.received_2 == 0)
                {
                    return true;
                }
                break;
            case 2:
                if (poolDT.received_1 == 0 || poolDT.received_2 == 0 || poolDT.received_3 == 0)
                {
                    return true;
                }
                break;
            default:
                if (poolDT.received_1 == 0 || poolDT.received_2 == 0 || poolDT.received_3 == 0)
                {
                    return true;
                }
                break;
        }
        return false;
    }
    /// <summary>
    /// 检查活动是否开启
    /// </summary>
    public bool f_CheckIsOpen()
    {
        //if (!CommonTools.f_GetGameParamOpen(EM_GameParamType.V_ActivityOpen))
        //{
        //    return false;
        //}


        //List<BasePoolDT<long>> listPoolDT = f_GetAll();
        //for (int i = 0; i < listPoolDT.Count; i++)
        //{
        //    FirstRechargePoolDT poolDT = listPoolDT[i] as FirstRechargePoolDT;
        //    //if (poolDT.mGetTimes <= 0)
        //    if (poolDT.received != 3)
        //    {
        //        return true;
        //    }
        //}
        //return false;
        MessageBox.ASSERT("Time All Received " + timeAllReceived);
        if (timeAllReceived == 0) return true;
        long nowTime = GameSocket.GetInstance().f_GetServerTime();
        if (((nowTime - timeAllReceived) / 86400) >= 1) return false;

        return true;
    }
    #endregion
    #region 外部接口
    /// <summary>
    /// 查询信息
    /// </summary>
    public void f_QueryInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (mIsInitForSer)
        {
            f_CheckRedPoint();
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FirstRechargeInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FirstRechargeInfo, bBuf);
    }
    /// <summary>
    /// 领取
    /// </summary>
    public void f_GetAward(int id, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FirstRecharge, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FirstRecharge, bBuf);
    }

    //TsuCode - FirstRechargeNew - NapDau
    public void f_GetAward_Tsu(int id, int dayId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FirstRecharge, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        tCreateSocketBuf.f_Add(dayId); //tsuCode - new
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FirstRecharge, bBuf);
    }
    //-----------------------------------
    #endregion
}
