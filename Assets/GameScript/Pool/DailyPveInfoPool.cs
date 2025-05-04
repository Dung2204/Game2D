using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
/// <summary>
/// 每日副本
/// </summary>
public class DailyPveInfoPool : BasePool
{
    public bool isFinish = false;
    private int pveId = 0;
    public int Star = 0;
    public List<AwardPoolDT> listAwardData;
    private bool isInitInfoSer = false;
    public DailyPveInfoPool() : base("DailyPveInfoPoolDT", true)
    {

    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_DailyPveInfoSC.f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            DailyPveInfoPoolDT pooldt = new DailyPveInfoPoolDT();
            DailyPveInfoDT dt = listData[i] as DailyPveInfoDT;
            pooldt.iId = dt.iId;
            pooldt.TodayPassTimes = 0;
            pooldt.m_DailyPveInfoDT = dt;
            f_Save(pooldt);
        }
    }

    protected override void RegSocketMessage()
    {
        SC_DailyPveFight scDailyPveFight = new SC_DailyPveFight();
        //结算监听
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_DailyPveFight, scDailyPveFight, OnResultCallback);
        //查询次数
        SC_DailyPveFightInfo scDailyPveFightInfo = new SC_DailyPveFightInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_DailyPveFightInfo, scDailyPveFightInfo, Callback_SocketData_Update);
    }
    private void OnResultCallback(object result)
    {
        isFinish = true;
        SC_DailyPveFight scDailyPveFight = (SC_DailyPveFight)result;
        Star = scDailyPveFight.uStars;
        if (Star > 0)
        {
            DailyPveGateDT dailyPveGateDT = glo_Main.GetInstance().m_SC_Pool.m_DailyPveGateSC.f_GetSC(pveId) as DailyPveGateDT;
            (f_GetForId(dailyPveGateDT.iType) as DailyPveInfoPoolDT).TodayPassTimes++;
        }
        Debug.Log("Star:" + Star);
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_DailyPveFightInfo scDailyPveFightInfo = (SC_DailyPveFightInfo)Obj;
        DailyPveInfoPoolDT dailyPveInfoPoolDT = (DailyPveInfoPoolDT)f_GetForId(scDailyPveFightInfo.id);
        dailyPveInfoPoolDT.TodayPassTimes = 1;
        CheckRedPoint();
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_DailyPveFightInfo scDailyPveFightInfo = (SC_DailyPveFightInfo)Obj;
        DailyPveInfoPoolDT dailyPveInfoPoolDT = (DailyPveInfoPoolDT)f_GetForId(scDailyPveFightInfo.id);
        dailyPveInfoPoolDT.TodayPassTimes = 1;
        CheckRedPoint();
    }
    private void CheckRedPoint()
    {
        string temp;
        List<BasePoolDT<long>> listdata = f_GetAll();
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        for (int i = 0; i < listdata.Count; i++)
        {
            DailyPveInfoPoolDT pooldt = (DailyPveInfoPoolDT)listdata[i];
            bool isOpen = f_CheckTime(pooldt.m_DailyPveInfoDT, GameSocket.GetInstance().f_GetServerTime(), out temp);
            //开启，且未攻打，且等级足够
            if (isOpen && pooldt.TodayPassTimes == 0)
            {
                List<NBaseSCDT> listDailyPve = glo_Main.GetInstance().m_SC_Pool.m_DailyPveGateSC.f_GetAll();
                for (int a = 0; a < listDailyPve.Count; a++)
                {
                    DailyPveGateDT DailyPveGateDT = listDailyPve[a] as DailyPveGateDT;
                    if (DailyPveGateDT.iType == pooldt.m_DailyPveInfoDT.iId && playerLevel >= DailyPveGateDT.iLevelLimit1)
                    {
                        Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.DailyPve);
                        return;
                    }
                }
            }
        }
    }
    #region 外部接口
    /// <summary>
    /// 请求pve信息
    /// </summary>
    public void f_DailyPveInfo(SocketCallbackDT socketCallbackDT)
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.DailyPve);
        CheckRedPoint();
        if (isInitInfoSer)
        {
            if(socketCallbackDT != null)
                socketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        isInitInfoSer = true;
        if (socketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DailyPveFightInfo, socketCallbackDT.m_ccCallbackSuc, socketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DailyPveFightInfo, bBuf);
    }
    /// <summary>
    /// 请求挑战某个关卡
    /// </summary>
    /// <param name="tollgateId">关卡Id</param>
    public void f_Challenge(int id,int type, List<AwardPoolDT> listAwardData, SocketCallbackDT socketCallbackDt)
    {
        ////DungeonTollgatePoolDT tDungeonTollgatePoolDT = (Data_Pool.m_DungeonPool.f_GetForId(id) as DungeonPoolDT).mTollgateList[0];
        ////StaticValue.m_CurDungeonTollgatePoolDT = tDungeonTollgatePoolDT;
        ////StaticValue.m_CurDugeonType = (EM_Fight_Enum)tDungeonTollgatePoolDT.mChapterType;
        ////Data_Pool.m_DialogPool.f_UpdateCurDialogData(StaticValue.m_CurDungeonTollgatePoolDT.mTollgateId);
        ////GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DungeonChallenge, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        ////CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        ////tCreateSocketBuf.f_Add(tDungeonTollgatePoolDT.mTollgateId);
        ////byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ////GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DungeonChallenge, bBuf);
        //_bDungeonFinish = false;

        pveId = id;
        this.listAwardData = listAwardData;
        MessageBox.DEBUG("DailyPve Level:" + id);
        //更新战斗数据
        StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_DailyPve, type, id, 0);
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DailyPveFight, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DailyPveFight, bBuf);
        isFinish = false;
    }
    /// <summary>
    /// 判断今天时间是否满足
    /// </summary>
    public bool f_CheckTime(DailyPveInfoDT dt, int timeNow, out string openStr)
    {
        DateTime dateTime = ccMath.time_t2DateTime(timeNow);
        DayOfWeek day = dateTime.DayOfWeek;
        openStr = "";
        bool isOpen = false;//是否开放
        for (int i = 1; i <= 7; i++)
        {
            //星期天放到最后
            int j = i % 7;
            DayOfWeek dayOfWeek = (DayOfWeek)j;
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    if (dt.iOpen0 == 1)
                    {
                        isOpen = day == dayOfWeek ? true : isOpen;
                        openStr += "CN";
                    }
                    break;
                case DayOfWeek.Monday:
                    if (dt.iOpen1 == 1)
                    {
                        isOpen = day == dayOfWeek ? true : isOpen;
                        openStr += "2,";
                    }
                    break;
                case DayOfWeek.Tuesday:
                    if (dt.iOpen2 == 1)
                    {
                        isOpen = day == dayOfWeek ? true : isOpen;
                        openStr += "3,";
                    }
                    break;
                case DayOfWeek.Wednesday:
                    if (dt.iOpen3 == 1)
                    {
                        isOpen = day == dayOfWeek ? true : isOpen;
                        openStr += "4,";
                    }
                    break;
                case DayOfWeek.Thursday:
                    if (dt.iOpen4 == 1)
                    {
                        isOpen = day == dayOfWeek ? true : isOpen;
                        openStr += "5,";
                    }
                    break;
                case DayOfWeek.Friday:
                    if (dt.iOpen5 == 1)
                    {
                        isOpen = day == dayOfWeek ? true : isOpen;
                        openStr += "6,";
                    }
                    break;
                case DayOfWeek.Saturday:
                    if (dt.iOpen6 == 1)
                    {
                        isOpen = day == dayOfWeek ? true : isOpen;
                        openStr += "7,";
                    }
                    break;
            }
        }
openStr += " mở";
        return isOpen;
    }
    #endregion
}
