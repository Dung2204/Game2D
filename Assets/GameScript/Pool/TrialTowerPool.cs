using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class TrialTowerPool : BasePool
{
    public int Max;

    public bool isEnter;
    public bool oneEnter;
    public eTrialTowerState NowState;

    public bool isOpen
    {
        get
        {


            GameParamDT tGameParamDT = UITool.f_GetGameParamDT((int)EM_GameParamType.TrialTower);
            //if (tGameParamDT.iParam1 != 1)
            //{
            //    return false;
            //}

            long OpenTime = ccMath.DateTime2time_t(CommonTools.f_GetDateTimeByTimeStr(tGameParamDT.iParam2.ToString()));
            //OpenTime += 3 * 86400;
            //if (OpenTime>GameSocket.GetInstance().f_GetServerTime()) {
            //    return true;
            //}
            int Time = GameSocket.GetInstance().f_GetServerTime() - (int)OpenTime;
            if (Time < 0)
                return false;
            int Day = Time / 86400;
            int isOpenDay = Day % (tGameParamDT.iParam3 + tGameParamDT.iParam4);
            if (isOpenDay < tGameParamDT.iParam3)
                return true;
            return false;
        }
    }


    public bool isFinsh;

    public int NowTower;

    public bool FinshRet;

    public int mNowEndTime;
    public int mNowStartTime;

    private bool OneCountTime;

    public bool isFirst;
    public TrialTowerPool() : base("TrialTowerPoolDT", false)
    {

        TrialTowerDT tt;
        List<NBaseSCDT> list = glo_Main.GetInstance().m_SC_Pool.m_TrialTowerSC.f_GetAll();
        for (int i = 0; i < list.Count; i++)
        {
            TrialTowerPoolDT t = new TrialTowerPoolDT();
            tt = list[i] as TrialTowerDT;
            t.mPass = false;
            t.iId = tt.iId;
            t.mTempId = tt.iId;
            f_Save(t);
        }
        oneEnter = true;
        f_GetAll().Reverse();
        OneCountTime = true;
    }

    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
    }

    protected override void RegSocketMessage()
    {
        SC_TowerEnter tSC_TowerEnter = new SC_TowerEnter();
        SC_TowerChallengeRet tSC_TowerChallengeRet = new SC_TowerChallengeRet();
        SC_TowerInit tSC_TowerInit = new SC_TowerInit();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_TowerEnter, tSC_TowerEnter, UpdateItem);
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_TowerChallengeRet, tSC_TowerChallengeRet, UpdateNowRet);
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_TowerInit, tSC_TowerInit, UpdateEnter);
    }

    private void UpdateItem(object obj)
    {
        SC_TowerEnter tSC_TowerEnter = (SC_TowerEnter)obj;
        Max = tSC_TowerEnter.uHighestTower;

        if (tSC_TowerEnter.uState == 1)
        {
            NowTower = tSC_TowerEnter.uCurTower + 1;
            NowState = eTrialTowerState.NoEnter;
        }
        else
        {
            NowTower = tSC_TowerEnter.uCurTower;
            NowState = eTrialTowerState.Fail;
        }
        if (NowTower > f_GetAll().Count)
        {
            NowTower = f_GetAll().Count;
            NowState = eTrialTowerState.Suc;
        }
        isEnter = true;
    }

    private void UpdateNowRet(object obj)
    {
        isFinsh = true;
        SC_TowerChallengeRet tSC_TowerChallengeRet = (SC_TowerChallengeRet)obj;
        FinshRet = tSC_TowerChallengeRet.uWin == 1;
        if (FinshRet)
        {
            NowTower += 1;
            if (NowTower > f_GetAll().Count)
            {
                NowState = eTrialTowerState.Suc;
                NowTower = f_GetAll().Count;
            }
            else
                NowState = eTrialTowerState.NoEnter;

            if (Max < NowTower)
            {
                Max = NowTower;
            }
        }
        else
        {
            NowState = eTrialTowerState.Fail;
        }

    }

    private void UpdateEnter(object obj)
    {
        SC_TowerInit tSC_TowerInit = (SC_TowerInit)obj;

        isEnter = tSC_TowerInit.uEnter == 1;
        isFirst = tSC_TowerInit.uFirst == 1;
    }

    public void CountEndTime()
    {
        if (OneCountTime)
        {
            OneCountTime = false;
        }
        else
        {
            return;
        }

        GameParamDT tGameParamDT = UITool.f_GetGameParamDT((int)EM_GameParamType.SevenStar);
        int OpenTime = (int)ccMath.DateTime2time_t(CommonTools.f_GetDateTimeByTimeStr(tGameParamDT.iParam2.ToString()));
        int intervalTime = GameSocket.GetInstance().f_GetServerTime() - OpenTime;
        if (intervalTime < 0 || tGameParamDT.iParam1 == 0)
        {
            mNowEndTime = 0;
            return;
        }
        int Day = (intervalTime / 86400) % (tGameParamDT.iParam3 + tGameParamDT.iParam4);
        if (Day <= tGameParamDT.iParam3)
        {
            //DateTime NowTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime() - Day * 86400);
            //DateTime EndTime = new DateTime(NowTime.Year, NowTime.Month, NowTime.Day + 3, 0, 0, 0);
            mNowEndTime = GameSocket.GetInstance().f_GetServerTime() - Day * 86400 + tGameParamDT.iParam3 * 86400;
            //CommonTools.f_GetActEndTimeForOpenSeverTime(tGameParamDT.iParam3);//(int)ccMath.DateTime2time_t(EndTime);
        }
        else
        {
            mNowEndTime = 0;
        }

        //if (!CommonTools.f_CheckOpenServerDay(tGameParamDT.iParam2))
        //{
        //    mNowEndTime = 0;
        //    return;
        //}
        ////int OpenTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + tGameParamDT.iParam2 * 86400;
        ////
        //int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        //int nowTime = GameSocket.GetInstance().f_GetServerTime();
        //int startTime = openServerTime + tGameParamDT.iParam2 * 86400;
        //
        //int nowDay = ccMath.f_GetTotalDaysByTime(nowTime) - ccMath.f_GetTotalDaysByTime(openServerTime) + 1;
        //int intervalDay = nowDay - tGameParamDT.iParam2;
        //
        //int Day = intervalDay % (tGameParamDT.iParam3 + tGameParamDT.iParam4);
        //if (Day <= tGameParamDT.iParam3)
        //{
        //
        //    mNowEndTime = Data_Pool.m_EventTimePool.OpenSeverTime + tGameParamDT.iParam3 * 86400 - GameSocket.GetInstance().f_GetServerTime();
        //    //mNowEndTime = GameSocket.GetInstance().f_GetServerTime() - Day * 86400 + tGameParamDT.iParam3 * 86400;
        //    mNowEndTime = startTime + tGameParamDT.iParam3 * 86400 - GameSocket.GetInstance().f_GetServerTime();
        //
        //}
        //else
        //{
        //    mNowEndTime = 0;
        //}

        //int startTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + tGameParamDT.iParam2 * 86400;
        //int openTime = tGameParamDT.iParam3 * 86400;
        //int difTime = tGameParamDT.iParam4 * 86400;
        //int nowTime = GameSocket.GetInstance().f_GetServerTime();
        //if(!CommonTools.f_CheckOpenServerDay(ccMath.f_GetTotalDaysByTime(startTime)- ccMath.f_GetTotalDaysByTime(Data_Pool.m_SevenActivityTaskPool.OpenSeverTime)))
        //{
        //    mNowEndTime = 0;
        //    return;
        //}
        //int tmpTime = nowTime - startTime;
        //int checkTime = tmpTime % (openTime + difTime);
        //
        //mNowStartTime = nowTime - checkTime;
        //mNowEndTime = mNowStartTime + openTime - 86400;

        //int day = (GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_SevenActivityTaskPool.OpenSeverTime) / (3600 * 24) + 1;
        //int dayEvent = day % (tGameParamDT.iParam3 + tGameParamDT.iParam4);
        //if (dayEvent == 0)
        //{
        //    mNowEndTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + day * 86400;// hom nay la ngay end
        //}
        //else if (dayEvent >= tGameParamDT.iParam2)
        //{
        //    int inEvent = dayEvent - tGameParamDT.iParam2 + 1;//+1 day vi ngay bat dau la ngay 1 ko phai ngay 0
        //    mNowEndTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + (day + tGameParamDT.iParam3 - inEvent) * 86400;
        //}
        //else
        //{
        //    mNowEndTime = 0;
        //    return;
        //}

    }

    public void f_Challeng(SocketCallbackDT socket)
    {
        if (socket != null)
        {

        }
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TowerChallenge, socket.m_ccCallbackSuc, socket.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TowerChallenge, bBuf);
    }

    public void f_Sweep(SocketCallbackDT socket)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TowerSweep, socket.m_ccCallbackSuc, socket.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TowerSweep, bBuf);
    }

    public void f_Enter(SocketCallbackDT socket)
    {
        if (!oneEnter)
        {
            return;
        }
        //oneEnter = false;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TowerEnter, socket.m_ccCallbackSuc, socket.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TowerEnter, bBuf);
    }

    public void f_Reset(SocketCallbackDT socket)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TowerReset, socket.m_ccCallbackSuc, socket.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TowerReset, bBuf);
    }

    public bool f_GetIsOpen()
    {
        bool isopen = false;
        GameParamDT tGameParamDT = UITool.f_GetGameParamDT((int)EM_GameParamType.TrialTower);
        if (tGameParamDT.iParam1 == 1)
        {
            isopen = true;

        }
        return isopen;
    }
}
