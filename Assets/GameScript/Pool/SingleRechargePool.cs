using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 单充福利
/// </summary>
public class SingleRechargePool : BasePool
{
    private int lastInfoTime = 0;
    private bool mIsInitForSer = false;//防止重复请求
    public SingleRechargePool() : base("SingleRechargePoolDT", true)
    {
    }

    protected override void f_Init()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_NewYearSingleRechargeAwardSC.f_GetAll();
        for (int i = 0; i < listDT.Count; i++)
        {
            NewYearSingleRechargeAwardDT dt = listDT[i] as NewYearSingleRechargeAwardDT;
            SingleRechargePoolDT poolDT = new SingleRechargePoolDT();
            poolDT.iId = dt.iId;
            poolDT.mGetTime = 0;
            poolDT.m_NewYearSingleRechargeAwardDT = dt;
            f_Save(poolDT);
        }
    }
    protected override void RegSocketMessage()
    {
        SC_NewYearRechargeAwardInfo scNewYearRechargeAwardInfo = new SC_NewYearRechargeAwardInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_NewYearRechargeAwardInfo, scNewYearRechargeAwardInfo, Callback_RecordData_Update);
    }
    /// <summary>
    /// 信息回调
    /// </summary>
    protected void Callback_RecordData_Update(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            SC_NewYearRechargeAwardInfo scNewYearRechargeAwardInfo = (SC_NewYearRechargeAwardInfo)tData;
            if (scNewYearRechargeAwardInfo.type == 1)
            {
                SingleRechargePoolDT poolDT = f_GetForId(scNewYearRechargeAwardInfo.id) as SingleRechargePoolDT;
                poolDT.mGetTime = 1;
            }
        }
        lastInfoTime = GameSocket.GetInstance().f_GetServerTime();
        f_CheckRedPoint();
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #region 检查红点
    /// <summary>
    /// 红点检查
    /// </summary>
    public void f_CheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SingleRecharge);

        GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
        int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (userLevel < gameParamDTOpenLevel.iParam1)
        {
            return;
        }
        List<BasePoolDT<long>> listPoolDT = f_GetAll();
        for (int i = 0; i < listPoolDT.Count; i++)
        {
            SingleRechargePoolDT poolDT = listPoolDT[i] as SingleRechargePoolDT;
            int timeBegin = poolDT.m_NewYearSingleRechargeAwardDT.iTimeBeg;
            int timeEnd = poolDT.m_NewYearSingleRechargeAwardDT.iTimeEnd;
            //if (poolDT.mGetTime <= 0 && CommonTools.f_CheckActIsOpenForOpenSeverTime(timeBegin, timeEnd))//CommonTools.f_CheckTime(timeBegin.ToString(), timeEnd.ToString()))
            if (poolDT.mGetTime <= 0 && CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(timeBegin, timeEnd)) //TsuCode
            {
                //long timeStart1 = CommonTools.f_GetActEndTimeForOpenSeverTime(timeBegin); //ccMath.DateTime2time_t(CommonTools.f_GetDateTimeByTimeStr(timeBegin.ToString()));
                //long timeEnd1 = CommonTools.f_GetActStarTimeForOpenSeverTime(timeEnd); //ccMath.DateTime2time_t(CommonTools.f_GetDateTimeByTimeStr(timeEnd.ToString()));
                //TsuCode
                long timeStart1 = CommonTools.f_GetActEndTimeForOpenSeverTime_Tsu(timeBegin); 
                long timeEnd1 = CommonTools.f_GetActStarTimeForOpenSeverTime_Tsu(timeEnd);
                //
                //TsuComment //bool isCanGet = Data_Pool.m_RechargePool.f_GetIsSingleRecharge(poolDT.m_NewYearSingleRechargeAwardDT.iCondition, timeStart1, timeEnd1);
                bool isCanGet = Data_Pool.m_RechargePool.f_GetIsSingleRecharge_TsuFunc(poolDT.m_NewYearSingleRechargeAwardDT.iCondition, timeStart1, timeEnd1); //TsuCode 
                if (isCanGet)
                {
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SingleRecharge);
                    NewYearActPage.SortAct(EM_NewYearActType.SingleRecharge);
                    return;
                }
            }
        }
    }
    #endregion
    #region 外部接口
    /// <summary>
    /// 查询信息
    /// </summary>
    public void f_QueryInfo(SocketCallbackDT tSocketCallbackDT)
    {
        f_CheckRedPoint();
        if (!CommonTools.f_CheckSameDay(lastInfoTime, GameSocket.GetInstance().f_GetServerTime()))
        {
            mIsInitForSer = false;
        }
        if (mIsInitForSer)
        {
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        mIsInitForSer = true;
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearRechargeAwardInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);

        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearRechargeAwardInfo, bBuf);
    }
    /// <summary>
    /// 领取
    /// </summary>
    public void f_GetAward(int id, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearSingleRechargeAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearSingleRechargeAward, bBuf);
    }
    #endregion
}
