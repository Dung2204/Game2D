using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;

public class TurntableBoxInfo
{

    public TurntableBoxInfo(int idx)
    {
        mIdx = idx;
    }

    public void f_UpdateInfo(int boxId, int boxScore)
    {
        mBoxId = boxId;
        if (mBoxId == 0)
            mBoxState = EM_BoxGetState.Invalid;
        mBoxScore = boxScore;
        //Debug.LogError("mBoxId:" + mBoxId+">>>>:"+ mBoxState);
    }

    public void f_UpdateInfo(EM_BoxGetState boxState)
    {
        mBoxState = boxState;
    }

    public EM_BoxGetState mBoxState
    {
        get;
        private set;
    }
    public int mBoxScore
    {
        get;
        private set;
    }

    public int mBoxId
    {
        get;
        private set;
    }

    public int mIdx
    {
        get;
        private set;
    }
}

public class TurntablePool : BasePool
{
    //当前宝箱数据
    public TurntableBoxDT mCurBoxDT
    {
        get;
        private set;
    }
    public TurntableBoxInfo[] mBoxInfo;
    public int m_CurSyceeRemain = 0;//当前剩余元宝
    public int m_CurRemainTime = 0;//当前剩余时间
    public int m_EndTime = 0;
	public int m_StartTime = 0; //TsuCode
    public List<TurntableLotteryInfo> m_RecordList;
    public int m_BoxCount = 0;//宝箱次数
    public List<AwardPoolDT> m_TurntableAwardList = new List<AwardPoolDT>();
    public List<int> m_TempIdList = new List<int>();
    public ccCallback UpdateUI;
    public TurntablePool() : base("TurntablePoolDT", true)
    {
        m_RecordList = new List<TurntableLotteryInfo>();
    }

    protected override void f_Init()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_TurntableLotterySC.f_GetAll();
        for (int i = 0; i < listDT.Count; i++)
        {
            TurntableLotteryDT dt = listDT[i] as TurntableLotteryDT;
            TurntablePoolDT poolDT = new TurntablePoolDT();
            poolDT.iId = dt.iId;
            poolDT.m_TurntableData = dt;
            f_Save(poolDT);
        }
        mBoxInfo = new TurntableBoxInfo[6];
        for (int i = 0; i < mBoxInfo.Length; i++)
        {
            mBoxInfo[i] = new TurntableBoxInfo(i);
            mBoxInfo[i].f_UpdateInfo(0, 0);
        }
        f_UpdateCurTaskBoxDT();
    }

    //根据宝箱表id初始宝箱数据
    private void f_UpdateCurTaskBoxDT()
    {
        List<NBaseSCDT> tmp = glo_Main.GetInstance().m_SC_Pool.m_TurntableBoxSC.f_GetAll();
        TurntableBoxDT tmpItem;
        for (int i = 0; i < tmp.Count; i++)
        {
            tmpItem = (TurntableBoxDT)tmp[i];
            mCurBoxDT = tmpItem;
            mBoxInfo[i].f_UpdateInfo(mCurBoxDT.iBoxId, mCurBoxDT.iCount);
            f_UpdateBoxInfo();
        }
    }

    //更新宝箱状态
    private void f_UpdateBoxInfo()
    {
        for (int i = 0; i < mBoxInfo.Length; i++)
        {
            mBoxInfo[i].f_UpdateInfo(EM_BoxGetState.Lock);
        }
    }


    public EM_BoxGetState f_GetBoxStateByIdx(int idx)
    {
        if (idx < 0 || idx >= mBoxInfo.Length)
        {
            return EM_BoxGetState.Invalid;
        }
        else
        {
            return mBoxInfo[idx].mBoxState;
        }
    }

    public string f_GetBoxExtraInfo(int idx)
    {
        if (idx < 0 || idx >= mBoxInfo.Length)
            return string.Empty;
        else
return string.Format("{0} times", mBoxInfo[idx].mBoxScore);
    }

    public TurntableBoxInfo f_GetBoxInfo(int idx)
    {
        if (idx < 0 || idx >= mBoxInfo.Length)
            return null;
        else
            return mBoxInfo[idx];
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }



    protected override void RegSocketMessage()
    {
        //记录榜单
        TurntableLotteryInfo scTurntableInfo = new TurntableLotteryInfo();
        GameSocket.GetInstance().f_RegMessage_Int2((int)SocketCommand.SC_TurntableInfo, scTurntableInfo, CallbackscTurntableInfo);

        //宝箱返还
        SC_TurntableBoxInfo scTurntableBoxInfo = new SC_TurntableBoxInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_TurntableBoxInfo, scTurntableBoxInfo, CallbackscTurntableBox);

        //抽奖返还
        TurntableLottery scTurntableLottery = new TurntableLottery();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_TurntableLottery, scTurntableLottery, CallbackscTurntableLottery);

        SC_TurntableRemainSycee tSC_TurntableRemainSycee = new SC_TurntableRemainSycee();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_TurntableRemainSycee, tSC_TurntableRemainSycee, UpdateSycee);
    }

    //记录榜单回调
    private void CallbackscTurntableInfo(int iData1, int iData2,int iNum, ArrayList aData)
    {
        m_CurSyceeRemain = iData2;
        //m_CurRemainTime = iData3;
        if (iData1 == 0)
            m_RecordList.Clear();
        foreach (SockBaseDT tData in aData)
        {
            TurntableLotteryInfo turntableInfo = (TurntableLotteryInfo)tData;
            m_RecordList.Insert(0, turntableInfo);
            //m_RecordList.Add(turntableInfo);    
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_TurntablePage_UpdateRecord);
    }
    //抽奖榜单回调
    private void CallbackscTurntableLottery(int iData1, int iData2, int iNum, ArrayList aData)
    {
        m_TurntableAwardList.Clear();
        m_TempIdList.Clear();
        foreach (TurntableLottery item in aData)
        {
            m_TempIdList.Add(item.tempId);
            TurntableLotteryDT data = (TurntableLotteryDT)glo_Main.GetInstance().m_SC_Pool.m_TurntableLotterySC.f_GetSC(item.tempId);
            AwardPoolDT tSC_CenterAwardNode = new AwardPoolDT();
            if (data.iAwardType != 1)
            {

                tSC_CenterAwardNode.f_UpdateByInfo((byte)data.iAwardType, data.iAwardId, data.iAwardNum);
                m_TurntableAwardList.Add(tSC_CenterAwardNode);
            }
            else
            {
                tSC_CenterAwardNode.f_UpdateByInfo((byte)data.iAwardType, data.iAwardId, item.sycee);
                m_TurntableAwardList.Add(tSC_CenterAwardNode);
            }
        }
    }

    //宝箱状态
    private void CallbackscTurntableBox(object data)
    {
        SC_TurntableBoxInfo boxData = (SC_TurntableBoxInfo)data;
        m_BoxCount = boxData.uLotteryTime;
        for (int i = 0; i < boxData.uState.Length; i++)// 0  没有领取  1 已领取   2 可领取
        {
            if (boxData.uState[i] == 0)// 没有领取
            {
                mBoxInfo[i].f_UpdateInfo(EM_BoxGetState.Lock);
            }
            else if (boxData.uState[i] == 1) // 1 已领取
            {
                mBoxInfo[i].f_UpdateInfo(EM_BoxGetState.AlreadyGet);
            }
            else if (boxData.uState[i] == 2) // 2 可领取
            {
                mBoxInfo[i].f_UpdateInfo(EM_BoxGetState.CanGet);
            }
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_TurntablePage_UpdateBoxInfo);
    }

    private void UpdateSycee(object obj)
    {
        SC_TurntableRemainSycee tSC_TurntableRemainSycee = (SC_TurntableRemainSycee)obj;
        m_CurSyceeRemain = tSC_TurntableRemainSycee.uRemainSycee;
        if (UpdateUI != null)
        {
            UpdateUI(obj);
        }
    }
    //查询基本信息
    public void f_QueryTurntableInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TurntableInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TurntableInfo, bBuf);
    }

    //查询宝箱基本信息
    public void f_QueryTurntableBoxInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TurntableBoxInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TurntableBoxInfo, bBuf);
        f_UpdateCurTaskBoxDT();
    }

    //请求抽奖
    public void f_QueryLotteryDraw(byte buyCount, SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TurntableLottery, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(buyCount);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TurntableLottery, bBuf);
    }

    //请求领取宝箱
    public void f_QueryTurntableBox(int id, SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TurntableBox, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TurntableBox, bBuf);
    }

    public void f_OneKeyGetBox(SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TurntableBoxAll, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(0);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TurntableBoxAll, bBuf);
    }

    public bool f_GetIsOpen()
    {
        List<NBaseSCDT> list = glo_Main.GetInstance().m_SC_Pool.m_TurntableTimeSC.f_GetAll();
        TurntableTimeDT tTurntableTimeDT;
        for (int i = 0; i < list.Count; i++)
        {
            tTurntableTimeDT = list[i] as TurntableTimeDT;
            bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime(tTurntableTimeDT.iBeginTime, tTurntableTimeDT.iEndTime);
            //MessageBox.DEBUG(string.Format("开启时间{0}  结束时间{1} 当前日期{2} 开服日期{3}", tTurntableTimeDT.iBeginTime, tTurntableTimeDT.iEndTime,GameSocket.GetInstance().f_GetServerTime(),Data_Pool.m_SevenActivityTaskPool.OpenSeverTime));
            if (isOpen) {
                //TsuCode
                m_EndTime = tTurntableTimeDT.iEndTime; //TsuCode
                m_StartTime = tTurntableTimeDT.iBeginTime;
                //
                //m_EndTime = CommonTools.f_GetActEndTimeForOpenSeverTime(tTurntableTimeDT.iEndTime);
                return true;
            }
        }
        return false;
    }
}
