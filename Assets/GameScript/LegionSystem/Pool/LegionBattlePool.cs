using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LegionBattlePool : BasePool
{
    private int signupTime = 0;
    /// <summary>
    /// 军团战报名时间
    /// </summary>
    public int m_iSignupTime
    {
        get
        {
            return signupTime;
        }
    }

    private int beginTime = 0;
    /// <summary>
    /// 军团战开始时间
    /// </summary>
    public int m_iBeginTime
    {
        get
        {
            return beginTime;
        }
    }

    private int endTime = 0;
    /// <summary>
    /// 结束时间
    /// </summary>
    public int m_iEndTime
    {
        get
        {
            return endTime;
        }
    }

    private EM_LegionBattleState state;

    /// <summary>
    /// 军团战状态 （服务器同步的状态）
    /// </summary>
    public EM_LegionBattleState m_State
    {
        get
        {
            return state;
        }
    }


    #region 军团战次数相关

    private int times;
    /// <summary>
    /// 军团战挑战次数
    /// </summary>
    public int m_iTimes
    {
        get {
            return times;
        }
    }

    /// <summary>
    /// 军团挑战次数限制（初始的）
    /// </summary>
    public int m_iTimesLimit
    {
        get {
            return 2;
        }
    }

    /// <summary>
    /// 军团战购买次数限制(暂时没用)
    /// </summary>
    public int m_iBuyTimesLimit
    {
        get {
            return 1;
        }
    }

    /// <summary>
    /// 军团战购买次数(暂时没用)
    /// </summary>
    public int m_iBuyTimes
    {
        get {
            return 0;
        }
    }

    #endregion

    private LegionBattlePoolDT selfPoolDt;
    public LegionBattlePoolDT m_SelfPoolDt
    {
        get
        {
            return selfPoolDt;
        }
    }

    private LegionBattlePoolDT enemyPoolDt;
    public LegionBattlePoolDT m_EnemyPoolDt
    {
        get
        {
            return enemyPoolDt;
        }
    }

    private bool isFinished = false;
    public bool m_bIsFinished
    {
        get
        {
            return isFinished;
        }
    }

    private int challengeRet = 0;
    public int m_iChallengeRet
    {
        get
        {
            return challengeRet;
        }
    }


    public LegionBattlePool() : base("LegionPlayerPoolDT", true)
    {
        
    }

    protected override void f_Init()
    {
        selfPoolDt = new LegionBattlePoolDT(true);
        enemyPoolDt = new LegionBattlePoolDT(false);
        tableList = new List<BasePoolDT<long>>();
    }

    protected override void RegSocketMessage()
    {
        //军团战初始化
        CMsg_SC_LegionBattleInit tSC_BattleInit = new CMsg_SC_LegionBattleInit();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_LegionBattleInit, tSC_BattleInit, f_OnLegionBattleInit);

        //军团战信息预览
        CMsg_SC_LegionBattleInfo tSC_BattleInfo = new CMsg_SC_LegionBattleInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_LegionBattleView, tSC_BattleInfo, f_OnLegionBattleView);

        //军团战防守列表
        CMsg_SC_LegionBattleDefenceNode tSC_DefencdNode = new CMsg_SC_LegionBattleDefenceNode();
        ChatSocket.GetInstance().f_RegMessage_Int2((int)SocketCommand.SC_LegionBattleDefenceList, tSC_DefencdNode, f_OnLegionBattleDefenceList);

        //军团战挑战结果
        CMsg_ByteNode tSC_ChallengeRet = new CMsg_ByteNode();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_LegionBattleChallengeRet, tSC_ChallengeRet, f_OnLegionBattleChallengeRet);

        //军团战列表
        CMsg_SC_LegionBattleTabeNode tSC_TabeNode = new CMsg_SC_LegionBattleTabeNode();
        GameSocket.GetInstance().f_RegMessage_Int2((int)SocketCommand.SC_LegionBattleTable, tSC_TabeNode, f_OnLegionBattleTable);
    }

    #region S2C G2C 协议

    /// <summary>
    /// 军团战初始化
    /// </summary>
    /// <param name="value">CMsg_SC_LegionBattleInit</param>
    private void f_OnLegionBattleInit(object value)
    {
        CMsg_SC_LegionBattleInit tServerInfo = (CMsg_SC_LegionBattleInit)value;
        state = (EM_LegionBattleState)tServerInfo.state;
        signupTime = tServerInfo.lastApplyTime;
        times = tServerInfo.myChallengeTimes; 
    }

    /// <summary>
    /// 军团战信息预览
    /// </summary>
    /// <param name="value">CMsg_SC_LegionBattleInfo</param>
    private void f_OnLegionBattleView(object value)
    {
        CMsg_SC_LegionBattleInfo tServerInfo = (CMsg_SC_LegionBattleInfo)value;
        selfPoolDt.isNowWin = tServerInfo.isSelfNowWin;
        selfPoolDt.f_UpdateLegionBaseInfo(LegionMain.GetInstance().m_LegionInfor.m_iLegionId, LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().LegionName);
        for (int i = 0; i < tServerInfo.selfBattleInfo.Length; i++)
        {
            selfPoolDt.f_UpdateGateNode((EM_LegionGate)i + 1, tServerInfo.selfBattleInfo[i].memberNum, tServerInfo.selfBattleInfo[i].starNum);
        }
        enemyPoolDt.f_UpdateLegionBaseInfo(tServerInfo.enemyLegionId, tServerInfo.szEnemyLegionName);
        for (int i = 0; i < tServerInfo.enemyBattleInfo.Length; i++)
        {
            enemyPoolDt.f_UpdateGateNode((EM_LegionGate)i + 1, tServerInfo.enemyBattleInfo[i].memberNum, tServerInfo.enemyBattleInfo[i].starNum);
        }
    }

    private void f_OnLegionBattleDefenceList(int flag, int gate, int iNum, ArrayList aData)
    {
        if (flag == 0)
        {
            for (int i = 0; i < aData.Count; i++)
            {
                CMsg_SC_LegionBattleDefenceNode tNode = (CMsg_SC_LegionBattleDefenceNode)aData[i];
                selfPoolDt.f_UpdateGateNodeDefenerNode((EM_LegionGate)gate, i, tNode.userId, tNode.uStar);
            }
        }
        else
        {
            for (int i = 0; i < aData.Count; i++)
            {
                CMsg_SC_LegionBattleDefenceNode tNode = (CMsg_SC_LegionBattleDefenceNode)aData[i];
                enemyPoolDt.f_UpdateGateNodeDefenerNode((EM_LegionGate)gate, i, tNode.userId, tNode.uStar);
            }
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_BATTLE_DEFENCE_LIST_UPDATE);
        
    }

    /// <summary>
    /// 军团战挑战结果
    /// </summary>
    /// <param name="value"> CMsg_ByteNode</param>
    private void f_OnLegionBattleChallengeRet(object value)
    {
        CMsg_ByteNode tServerInfo = (CMsg_ByteNode)value;
        isFinished = true;
        challengeRet = tServerInfo.value1;
    }

    private List<BasePoolDT<long>> tableList;
    public List<BasePoolDT<long>> m_TableList
    {
        get
        {
            return tableList;
        }
    }

    private int period;
    private int listTime;
    /// <summary>
    /// 军团战期数
    /// </summary>
    public int m_iPeriod
    {
        get
        {
            return period;
        }
    }
    /// <summary>
    /// 军团战对阵表 时间
    /// </summary>
    public int m_iListTime
    {
        get
        {
            return listTime;
        }
    }
    

    private void f_OnLegionBattleTable(int uPeriod, int beginTime, int num, ArrayList aData)
    {
        period = uPeriod;
        listTime = beginTime;
        for (int i = 0; i < aData.Count; i++)
        {
            CMsg_SC_LegionBattleTabeNode tNode = (CMsg_SC_LegionBattleTabeNode)aData[i];
            LegionBattleTableNode tAddData = new LegionBattleTableNode(tableList.Count,(EM_LegionTableRet)tNode.ret, tNode.szEnemyLegionNameA, tNode.szEnemyLegionNameB);
            tableList.Add(tAddData);
        }
    }

    #endregion


    #region C2S C2G 协议

    /// <summary>
    /// 请求军团战初始化
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionBattleInit(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_LegionBattleInit, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_LegionBattleInit, bBuf);
    }

    /// <summary>
    /// 请求军团战报名
    /// </summary>
    /// <param name="socketCallbackDT"></param>
    public void f_LegionBattleApply(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_LegionBattleApply, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_LegionBattleApply, bBuf);
    }

    /// <summary>
    /// 军团战防守列表
    /// </summary>
    /// <param name="flag">0己方，1敌方</param>
    /// <param name="gate">EM_LegionGate[1,4]</param>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionBattleDefenceList(int flag,int gate,SocketCallbackDT socketCallbackDt)
    {
        //游戏服请求，关系服返回操作结果
        ChatSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_LegionBattleDefenceList, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(flag);
        tCreateSocketBuf.f_Add(gate);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_LegionBattleDefenceList, bBuf);
    }

    /// <summary>
    /// 军团战挑战
    /// </summary>
    /// <param name="gate">EM_LegionGate[1,4]</param>
    /// <param name="idx">[0,DefenderMaxNum]</param>
    /// <param name="starNum">[1,3]</param>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionBattleChallenge(int gate,byte idx,byte starNum,SocketCallbackDT socketCallbackDt)
    {
        isFinished = false;
        StaticValue.m_CurBattleConfig.f_UpdateInfo(EM_Fight_Enum.eFight_LegionBattle, gate, 0, 0);
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_LegionBattleChallenge, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(gate);
        tCreateSocketBuf.f_Add(idx);
        tCreateSocketBuf.f_Add(starNum);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_LegionBattleChallenge, bBuf);
    }

    //请求cd 30秒
    private const int TableTimeLimit = 30;
    private int tableTime = 0;
    public void f_LegionBattleTable(SocketCallbackDT socketCallbackDt)
    {
        int tNow = GameSocket.GetInstance().f_GetServerTime();
        if (tNow - tableTime <= TableTimeLimit)
        {
            if (socketCallbackDt != null && socketCallbackDt.m_ccCallbackSuc != null)
            {
                socketCallbackDt.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
                return;
            }
        }
        tableTime = tNow;
        tableList.Clear();
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_LegionBattleTable, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_LegionBattleTable, bBuf);
    }

    /// <summary>
    /// 购买军团战挑战次数 (未实现)
    /// </summary>
    /// <param name="times">购买次数</param>
    /// <param name="socketCallbackDT"></param>
    public void f_BuyBattleTimes(byte times, SocketCallbackDT socketCallbackDT)
    {
        //
        socketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
    }

    #endregion


    #region 对外接口

    /// <summary>
    /// 军团报名限制（未包含权限）
    /// </summary>
    /// <param name="legionLv">军团等级</param>
    /// <param name="memberNum">军团成员限制</param>
    /// <returns>true:满足报名条件,false:不满足报名条件</returns>
    public bool f_IsEnableSignUp(int legionLv,int memberNum)
    {
		GameParamDT LegionParam = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(97) as GameParamDT);
		int legionLvLimit = LegionParam.iParam1;
        // if (legionLv >= LegionConst.LEGION_BATTLE_SIGNUP_LV_LIMIT
            // && memberNum >= LegionConst.LEGION_BATTLE_SIGNUP_MEMBERNUM_LIMIT)
            // return true;
        // else
            // return false;
		MessageBox.ASSERT("Legion LV LIMIT: " + legionLvLimit);
		if (legionLv >= legionLvLimit
            && memberNum >= LegionConst.LEGION_BATTLE_SIGNUP_MEMBERNUM_LIMIT)
            return true;
        else
            return false;
    }
    ///////////////////////TsuComment///////////////////
    /// <summary>
    /// 设置军团战时间
    /// </summary>
    /// <param name="serverTime"></param>
    //public void f_SetBattleTime(int serverTime)
    //{
    //    DateTime tNow = ccMath.time_t2DateTime(serverTime);
    //    for (int i = 0; i < LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS.Length; i++)
    //    {
    //        if (LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[i] > (int)tNow.DayOfWeek)
    //        {
    //            DateTime tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day).AddDays(LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[i] - (int)tNow.DayOfWeek);
    //            beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR));
    //            endTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_END_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_END_Minute));
    //            break;
    //        }
    //        else if (LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[i] == (int)tNow.DayOfWeek)
    //        {
    //            DateTime tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day);
    //            beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR));
    //            endTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_END_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_END_Minute));
    //            //设置下一个军团战时间
    //            if (
    //                serverTime > endTime //过了军团战时间
    //                || (signupTime == 0 && serverTime >= beginTime && serverTime <= endTime) //未报名而且已经到了军团战时间
    //                || signupTime > beginTime  //报名时间超过了军团战开始时间
    //                )
    //            {
    //                int tNextIdx = i + 1;
    //                if (tNextIdx >= LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS.Length)
    //                {
    //                    tNextIdx = 0;
    //                    tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day).AddDays(7 - (int)tNow.DayOfWeek + LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[tNextIdx]);
    //                }
    //                else
    //                {
    //                    tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day).AddDays(LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[tNextIdx] - (int)tNow.DayOfWeek);
    //                }
    //                beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR));
    //                endTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_END_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_END_Minute));
    //            }
    //            break;
    //        }
    //        else if (i == LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS.Length - 1 && LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[i] < (int)tNow.DayOfWeek)
    //        {
    //            DateTime tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day).AddDays(7 - (int)tNow.DayOfWeek + LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[0]);
    //            beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR));
    //            endTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_END_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_END_Minute));
    //        }
    //    }
    //}
    ///////////////////////TsuComment///////////////////
    //TsuCode
    public void f_SetBattleTime(int serverTime)
    {
        DateTime tNow = ccMath.time_t2DateTime(serverTime);
        for (int i = 0; i < LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS.Length; i++)
        {
            if (LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[i] > (int)tNow.DayOfWeek)
            {
                DateTime tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day).AddDays(LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[i] - (int)tNow.DayOfWeek);
                //beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR));
                //TsuCode
                beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_BEGIN_Minute));
                //
                endTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_END_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_END_Minute));
                break;
            }
            else if (LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[i] == (int)tNow.DayOfWeek)
            {
                DateTime tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day);
                //beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR));
                //TsuCode
                beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_BEGIN_Minute));
                //
                endTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_END_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_END_Minute));
                //设置下一个军团战时间
                if (

                    serverTime > endTime //过了军团战时间
                    || (signupTime == 0 && serverTime >= beginTime && serverTime <= endTime) //未报名而且已经到了军团战时间
                    || signupTime > beginTime  //报名时间超过了军团战开始时间
                    )
                {
                    int tNextIdx = i + 1;
                    if (tNextIdx >= LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS.Length)
                    {
                        tNextIdx = 0;
                        tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day).AddDays(7 - (int)tNow.DayOfWeek + LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[tNextIdx]);
                        //TsuCode
                        //tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day).AddDays(7 - (int)tNow.DayOfWeek + LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[tNextIdx]-1);
                        //
                    }
                    else
                    {
                        tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day).AddDays(LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[tNextIdx] - (int)tNow.DayOfWeek);
                        //TsuCOde
                        //tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day).AddDays(0);
                        //
                    }
                    //beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR));
                    //TsuCode
                    beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_BEGIN_Minute));
                    //
                    endTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_END_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_END_Minute));

                }
                break;
            }
            else if (i == LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS.Length - 1 && LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[i] < (int)tNow.DayOfWeek)
            {
                DateTime tBeginDateTime = new DateTime(tNow.Year, tNow.Month, tNow.Day).AddDays(7 - (int)tNow.DayOfWeek + LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[0]);
                //beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR));
                //TsuCode
                beginTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_BEGIN_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_BEGIN_Minute));
                //
                endTime = (int)ccMath.DateTime2time_t(tBeginDateTime.AddHours(LegionConst.LEGION_BATTLE_END_HOUR).AddMinutes(LegionConst.LEGION_BATTLE_END_Minute));
            }
        }
    }



    /// <summary>
    /// 根据星数返回奖励
    /// </summary>
    /// <param name="starNum">0：失败</param>
    /// <returns></returns>
    public int f_GetAwardByStar(int starNum)
    {
        int tAwardId = 0;
        if (starNum >= 0 && starNum < LegionConst.LEGION_BATTLE_AWARD_STAR.Length)
        {
            tAwardId = LegionConst.LEGION_BATTLE_AWARD_STAR[starNum];
        }
        return tAwardId;
    }

    /// <summary>
    /// 是否站军团战期间
    /// </summary>
    /// <returns></returns>
    public bool f_IsInBattleTime()
    {
        int curServerTime = GameSocket.GetInstance().f_GetServerTime();
        f_SetBattleTime(curServerTime);
        return curServerTime >= LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime && curServerTime <= LegionMain.GetInstance().m_LegionBattlePool.m_iEndTime;
    }


    #endregion

    #region Invaild(无效的)

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }

    #endregion

}
