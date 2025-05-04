using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;
using System.Collections.Generic;
/// <summary>
/// 红包任务pool
/// </summary>
public class RedPacketTaskPool : BasePool
{
    private bool mIsInitForSer = false;//防止重复请求
    public int mRecruitGiftTaskID = 12;//招募有礼任务ID
    public RedPacketTaskPool() : base("RedPacketTaskPoolDT", true)
    {
    }

    protected override void f_Init()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_RedPacketTaskSC.f_GetAll();
        for (int i = 0; i < listDT.Count; i++)
        {
            RedPacketTaskDT dt = listDT[i] as RedPacketTaskDT;
            RedPacketTaskPoolDT poolDT = new RedPacketTaskPoolDT();
            poolDT.iId = listDT[i].iId;
            poolDT.mProgress = 0;
            poolDT.mHasGetCount = 0;
            poolDT.mRedPacketTaskDT = dt;
            f_Save(poolDT);
        }
    }

    protected override void RegSocketMessage()
    {
        SC_RedPacketTaskInfo scRedPacketTaskInfo = new SC_RedPacketTaskInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_RedPacketTaskState, scRedPacketTaskInfo, Callback_RedPacketTask_Update);
        SC_RedPacketTaskTypeUpdate scRedPacketTaskInfoUpdate = new SC_RedPacketTaskTypeUpdate();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_RedPacketTaskInfo, scRedPacketTaskInfoUpdate, Callback_TaskType_Update);
    }
    /// <summary>
    /// 回调
    /// </summary>
    protected void Callback_RedPacketTask_Update(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            SC_RedPacketTaskInfo scRedPacketTaskInfo = (SC_RedPacketTaskInfo)tData;
            RedPacketTaskPoolDT poolDT = f_GetForId(scRedPacketTaskInfo.id) as RedPacketTaskPoolDT;
            poolDT.mHasGetCount = scRedPacketTaskInfo.iGetCount;
        }
        f_CheckRedPoint();
    }

    /// <summary>
    /// 任务类型进度值更新
    /// </summary>
    protected void Callback_TaskType_Update(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            SC_RedPacketTaskTypeUpdate scRedPacketTaskInfoUpdate = (SC_RedPacketTaskTypeUpdate)tData;
            List<BasePoolDT<long>> listPoolDT = f_GetAll();
            for (int i = 0; i < listPoolDT.Count; i++)
            {
                RedPacketTaskPoolDT poolDT = listPoolDT[i] as RedPacketTaskPoolDT;
                if(poolDT.mRedPacketTaskDT.iTaskType == scRedPacketTaskInfoUpdate.iTaskType)
                    poolDT.mProgress = scRedPacketTaskInfoUpdate.iProgress;
            }
        }
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
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RedPacketTask);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RecruitGift);
        
        GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
        int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (userLevel < gameParamDTOpenLevel.iParam1)
        {
            return;
        }
        List<BasePoolDT<long>> listPoolDT = f_GetAll();
        for (int i = 0; i < listPoolDT.Count; i++)
        {
            RedPacketTaskPoolDT poolDT = listPoolDT[i] as RedPacketTaskPoolDT;
            if (poolDT.mHasGetCount <= 0)
            {
                if (poolDT.mProgress >= poolDT.mRedPacketTaskDT.iConditonParam)//达到领取条件
                {
                    //if (!CommonTools.f_CheckTime(poolDT.mRedPacketTaskDT.iTimeBegin.ToString(), poolDT.mRedPacketTaskDT.iTimeEnd.ToString()))
                    if (!CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(poolDT.mRedPacketTaskDT.iTimeBegin, poolDT.mRedPacketTaskDT.iTimeEnd)) //TsuCde
                    {
                        continue;
                    }
                     if (poolDT.mRedPacketTaskDT.iTaskType == 12)//招募有礼
                    {
                        Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.RecruitGift);
                        NewYearActPage.SortAct(EM_NewYearActType.RecruitGift);
                    }
                    else//红包任务
                    {
                        Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.RedPacketTask);
                        NewYearActPage.SortAct(EM_NewYearActType.RedPacketTask);
                    }
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
        if (mIsInitForSer)
        {
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        mIsInitForSer = true;
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RedPacketTaskInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);

        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RedPacketTaskInfo, bBuf);
    }
    /// <summary>
    /// 领取
    /// </summary>
    public void f_GetAward(int id, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_RedPacketTask, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RedPacketTask, bBuf);
    }
    #endregion
}
