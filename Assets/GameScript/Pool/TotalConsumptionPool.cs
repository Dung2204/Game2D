using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using System.Collections;

public class TotalConsumptionPool : BasePool
{

    private bool mIsInitForSer = false;//防止重复请求
    public int mSendTime = 0;
    public TotalConsumptionPool() : base("TotalConsumotionPoolDT", false)
    {

    }

    protected override void f_Init()
    {
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_NewYearSyceeConsumeSC.f_GetAll().Count; i++)
        {
            TotalConsumptionPoolDT tTotalConsumptionPoolDT = new TotalConsumptionPoolDT();
            NewYearSyceeConsumeDT tNewYearSyceeConsumeDT = glo_Main.GetInstance().m_SC_Pool.m_NewYearSyceeConsumeSC.f_GetAll()[i] as NewYearSyceeConsumeDT;
            tTotalConsumptionPoolDT.iId = tNewYearSyceeConsumeDT.iId;
            tTotalConsumptionPoolDT.m_NewYearSyceeConsume = tNewYearSyceeConsumeDT;
            f_Save(tTotalConsumptionPoolDT);
        }
    }
    protected override void RegSocketMessage()
    {
        SC_TotalConsumotion tSC_TotalConsumotion = new SC_TotalConsumotion();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_NewYearSyceeConsumeInfo, tSC_TotalConsumotion, Callback_SC_TotalConsumotion);
    }
    protected void Callback_SC_TotalConsumotion(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            SC_TotalConsumotion tSC_TotalConsumotion = (SC_TotalConsumotion)tData;
            TotalConsumptionPoolDT tNewYearSyceeConsumeDT = f_GetForId(tSC_TotalConsumotion.m_DTId) as TotalConsumptionPoolDT;
            if (tNewYearSyceeConsumeDT == null)
            {
MessageBox.ASSERT("Cumulative spending, error ID" + tSC_TotalConsumotion.m_DTId);
                continue;
            }
            tNewYearSyceeConsumeDT.m_bIsGetAward = true;
        }
MessageBox.ASSERT("Delay time:"+(GameSocket.GetInstance().f_GetServerTime()- mSendTime));
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
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TotalConst);
        GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
        int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (userLevel < gameParamDTOpenLevel.iParam1)
        {
            return;
        }
        List<BasePoolDT<long>> listPoolDT = f_GetAll();
        for (int i = 0; i < listPoolDT.Count; i++)
        {
            TotalConsumptionPoolDT poolDT = listPoolDT[i] as TotalConsumptionPoolDT;
            //if (!poolDT.m_bIsGetAward && CommonTools.f_CheckActIsOpenForOpenSeverTime(poolDT.m_NewYearSyceeConsume.iTimeBegin, poolDT.m_NewYearSyceeConsume.iTimeEnd))//CommonTools.f_CheckTime(poolDT.m_NewYearSyceeConsume.iTimeBegin.ToString(),poolDT.m_NewYearSyceeConsume.iTimeEnd.ToString()))
            if (!poolDT.m_bIsGetAward && CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(poolDT.m_NewYearSyceeConsume.iTimeBegin, poolDT.m_NewYearSyceeConsume.iTimeEnd)) //TsuCode
            {
                //int _count = Data_Pool.m_HistoryConstPool.f_GetAppointTimeRanges(CommonTools.f_GetActStarTimeForOpenSeverTime(poolDT.m_NewYearSyceeConsume.iTimeBegin),
                //                                                                    CommonTools.f_GetActEndTimeForOpenSeverTime(poolDT.m_NewYearSyceeConsume.iTimeEnd));
                /*(ccMath.f_Data2Int(poolDT.m_NewYearSyceeConsume.iTimeBegin),
                                    ccMath.f_Data2Int(poolDT.m_NewYearSyceeConsume.iTimeEnd));**/
                //TsuCode
                int _count = Data_Pool.m_HistoryConstPool.f_GetAppointTimeRanges(CommonTools.f_GetActStarTimeForOpenSeverTime_Tsu(poolDT.m_NewYearSyceeConsume.iTimeBegin),
                                                                                   CommonTools.f_GetActEndTimeForOpenSeverTime_Tsu(poolDT.m_NewYearSyceeConsume.iTimeEnd));
                //
                if (_count >= poolDT.m_NewYearSyceeConsume.iCondition)
                {
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TotalConst);
                    NewYearActPage.SortAct(EM_NewYearActType.TotalConst);
                    return;
                }
            }
        }
    }
    #endregion


    #region 外部接口
    public void f_TotalConsumpInfo(SocketCallbackDT tSocketCallbackDT)
    {
        f_CheckRedPoint();
        if (mIsInitForSer)
        {
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        mIsInitForSer = true;
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearSyceeConsumeInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tBuf = new CreateSocketBuf();
        byte[] tArr = tBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearSyceeConsumeInfo, tArr);
        mSendTime = GameSocket.GetInstance().f_GetServerTime();
    }
    public void f_GetAward(int id, SocketCallbackDT tSocketCallbackDT)
    {

        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearSyceeConsume, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tBuf = new CreateSocketBuf();
        tBuf.f_Add(id);
        byte[] tArr = tBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearSyceeConsume, tArr);
    }

    #endregion
}

