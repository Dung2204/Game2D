using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using System.Collections;

public class SevenActivityTaskPool : BasePool
{
    /// <summary>
    /// 第四页的半价
    /// </summary>
    public Dictionary<int, BasePoolDT<long>> HalfDiscountPool = new Dictionary<int, BasePoolDT<long>>();
    /// <summary>
    /// 任务对应的完成参数
    /// </summary>
    public Dictionary<EM_eSevenDay, int[]> m_taskInfo = new Dictionary<EM_eSevenDay, int[]>();
    private Dictionary<EM_eSevenDay, List<BasePoolDT<long>>> _SevebTaskInfo = new Dictionary<EM_eSevenDay, List<BasePoolDT<long>>>();
    private int[] _NeedLoad;   //需要本地记录的类型

    public int OpenSeverTime = 0;
    public SevenActivityTaskPool() : base("SevenActivityTaskPoolDT", true)
    {
        HalfDiscountPool = new Dictionary<int, BasePoolDT<long>>();
        m_taskInfo = new Dictionary<EM_eSevenDay, int[]>();    //不需要初始化   
        _SevebTaskInfo = new Dictionary<EM_eSevenDay, List<BasePoolDT<long>>>();     //碎片整理
        _NeedLoad = new int[18] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 36, 37, 43, 45, 64 };
        //七日活动初始化
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_SevenActivityTaskSC.f_GetAll().Count; i++)
        {
            SevenActivityTaskPoolDT tSevenDayPoolDT = new SevenActivityTaskPoolDT();
            SevenActivityTaskDT ttttt = glo_Main.GetInstance().m_SC_Pool.m_SevenActivityTaskSC.f_GetAll()[i] as SevenActivityTaskDT;
            tSevenDayPoolDT.iId = ttttt.iId;
            tSevenDayPoolDT.IDayNum = ttttt.iDayNum;
            tSevenDayPoolDT.ITempleteId = ttttt.iId;
            tSevenDayPoolDT.IPageNum = ttttt.iPage;
            tSevenDayPoolDT.m_result = new int[2];
            tSevenDayPoolDT.m_SevenActivityTaskDT = ttttt;

            f_Save(tSevenDayPoolDT);
            if (_SevebTaskInfo.ContainsKey((EM_eSevenDay)ttttt.itype))
                _SevebTaskInfo[(EM_eSevenDay)ttttt.itype].Add(tSevenDayPoolDT);
            else
            {
                _SevebTaskInfo.Add((EM_eSevenDay)ttttt.itype, new List<BasePoolDT<long>>());
                _SevebTaskInfo[(EM_eSevenDay)ttttt.itype].Add(tSevenDayPoolDT);
            }
        }
        //七日活动半价初始化
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_HalfDiscountSC.f_GetAll().Count; i++)
        {
            HalfDiscountPoolDT ttttt = new HalfDiscountPoolDT();
            HalfDiscountDT ttttttttttttt = glo_Main.GetInstance().m_SC_Pool.m_HalfDiscountSC.f_GetAll()[i] as HalfDiscountDT;
            ttttt._ITempleteId = ttttttttttttt.iId;
            ttttt.iId = ttttttttttttt.iId;
            ttttt.m_HalfDiscountDT = ttttttttttttt;
            HalfDiscountPool.Add(ttttttttttttt.iId, ttttt);
        }

    }
    #region Pool数据管理
    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        f_Socket_UpdateData(Obj);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        taskGain tTaskGain = (taskGain)Obj;

        SevenActivityTaskPoolDT FindSevenPoolDT = f_GetForId((long)tTaskGain.taskID) as SevenActivityTaskPoolDT;
        if (FindSevenPoolDT == null)
        {
            MessageBox.ASSERT("SevenActivityTask PoolDT null");
        }
        FindSevenPoolDT.isGain = tTaskGain.isGain;
        FindSevenPoolDT.isFinsh = tTaskGain.isFinsh;
        MessageBox.DEBUG(string.Format("SevenActivityTask PoolDT{0} đã hoàn thành {1}   đã nhận {2}   ", FindSevenPoolDT.iId, FindSevenPoolDT.isFinsh, FindSevenPoolDT.isGain));
        //已完成的                             未领取的
        if (FindSevenPoolDT.isFinsh == 1 && FindSevenPoolDT.isGain != 1)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayTask);
        }

    }

    protected override void RegSocketMessage()
    {
        taskGain tTaskGain = new taskGain();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_SevenDayTaskAward, tTaskGain, Callback_SocketData_Update);


        taskInfo tTaskInfo = new taskInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_SevenDayTaskInfo, tTaskInfo, SevenDayTaskInfo);

        HalfDiscInfo tHlfDiscInfo = new HalfDiscInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_SevenDayTaskHalfDiscountInfo, tHlfDiscInfo, SevenHalfInfo);

        //开服时间
        OpenSeverTime tOpenTime = new OpenSeverTime();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_OpenServTime, tOpenTime, SaveOpenSeverTime);
    }


    private void SevenDayTaskInfo(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (taskInfo item in aData)
        {
            if (m_taskInfo.ContainsKey((EM_eSevenDay)item.taskEnum))
                m_taskInfo[(EM_eSevenDay)item.taskEnum][0] = item.value;
            else
            {
                int[] tbyte = new int[2] { item.value, 0 };
                m_taskInfo.Add((EM_eSevenDay)item.taskEnum, tbyte);
            }
            //服务器处理
            f_UpdateSevenDataInfo((EM_eSevenDay)item.taskEnum);
        }
    }


    public void _LoadDate()
    {
        //本地处理
        for (int i = 0; i < _NeedLoad.Length; i++)
            f_UpdateSevenDataInfo((EM_eSevenDay)_NeedLoad[i], true);
    }
    //private void _LoadData()
    //{

    //    taskInfo[(EM_eSevenDay)3][0] = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
    //    taskInfo[(EM_eSevenDay)4][0] = Data_Pool.m_TeamPool.f_GetTotalBattlePower();
    //    taskInfo[(EM_eSevenDay)5][0] = Data_Pool.m_DungeonPool.m_DungeonMainMax;
    //    taskInfo[(EM_eSevenDay)6][0] = Data_Pool.m_TeamPool.f_GetAll().Count;

    //    taskInfo[(EM_eSevenDay)7][0] = Data_Pool.m_TeamPool.f_GetAll().Count;
    //    //taskInfo[(EM_eSevenDay)7][1] = Data_Pool.m_TeamPool.f_GetTeamImporent();

    //    taskInfo[(EM_eSevenDay)45][0] = Data_Pool.m_RechargePool.f_GetDayRechageMoneyMax(ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime()).Day);

    //    taskInfo[(EM_eSevenDay)7][0] = Data_Pool.m_TeamPool.f_GetAll().Count;
    //    taskInfo[(EM_eSevenDay)7][1] = Data_Pool.m_TeamPool.f_GetTeamImporent(5);

    //    taskInfo[(EM_eSevenDay)9][0] = Data_Pool.m_TeamPool.f_GetEquipMinLevel(1);
    //    taskInfo[(EM_eSevenDay)9][1] = Data_Pool.m_TeamPool.f_GetAllEquipNum();

    //    taskInfo[(EM_eSevenDay)10][0] = Data_Pool.m_TeamPool.f_GetEquipMinLevel(2);
    //    taskInfo[(EM_eSevenDay)10][1] = Data_Pool.m_TeamPool.f_GetAllEquipNum();

    //    taskInfo[(EM_eSevenDay)11][0] = Data_Pool.m_TeamPool.f_GetTreasureMinLevel(1);
    //    taskInfo[(EM_eSevenDay)11][1] = Data_Pool.m_TeamPool.f_GetAllTreasureNum();

    //    taskInfo[(EM_eSevenDay)12][0] = Data_Pool.m_TeamPool.f_GetTreasureMinLevel(2);
    //    taskInfo[(EM_eSevenDay)12][1] = Data_Pool.m_TeamPool.f_GetAllTreasureNum();

    //    //taskInfo[(EM_eSevenDay)40][0]=(byte)Data_Pool.m_RebelArmyPool    28
    //    taskInfo[(EM_eSevenDay)43][0] = Data_Pool.m_TeamPool.f_GetTeamTreasureMaxLevel();
    //    taskInfo[(EM_eSevenDay)64][0] = Data_Pool.m_RechargePool.f_GetMonthCard();
    //}

    private void SevenHalfInfo(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (HalfDiscInfo item in aData)
        {
            if (HalfDiscountPool.ContainsKey(item.DayIdx))
            {
                (HalfDiscountPool[item.DayIdx] as HalfDiscountPoolDT).BuyTime = item.BuyTime;
            }
        }
    }

    private void SaveOpenSeverTime(object obj)
    {
        OpenSeverTime = ((OpenSeverTime)obj).value1;
        if (_OpenTimeUpdate != null)
        {
            _OpenTimeUpdate(obj);
        }
    }
    #endregion
    ccCallback _OpenTimeUpdate;
    ccCallback _UpdateUnHold;
    /// <summary>
    /// 获取七日奖励
    /// </summary>
    /// <param name="Tempid"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_GetSevenAward(int Tempid, SocketCallbackDT tSocketCallbackDT)
    {
        Data_Pool.m_AwardPool.m_GetLoginAward.Clear();

        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SevenDayTaskAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(Tempid);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SevenDayTaskAward, bBuf);
    }
    /// <summary>
    /// 奖励达成
    /// </summary>
    private void f_GetSevenDayAchieve(int Tempid)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SevenDayAchieve, TaskAchieve, TaskAchieve);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(Tempid);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SevenDayAchieve, bBuf);
    }
    /// <summary>
    /// 七日完成参数主入口
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_GetTaskInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SevenDayTaskInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(1);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SevenDayTaskInfo, bBuf);
    }
    public void f_GetHalfInfo(byte DayNum, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SevenDayTaskHalfDiscount, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(DayNum);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SevenDayTaskHalfDiscount, bBuf);
    }

    public void f_HalfInfo(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SevenDayTaskHalfDiscountInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(1);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SevenDayTaskHalfDiscountInfo, bBuf);
    }

    public void f_GetOpenTime(ccCallback Time, SocketCallbackDT tSocketCallbackDT)
    {
        _OpenTimeUpdate = Time;
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_OpenServTime, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(1);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_OpenServTime, bBuf);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    private void _LoadData()
    {
        for (int i = 0; i < _NeedLoad.Length; i++)
        {
            //UpdateTask((EM_eSevenDay)_NeedLoad[i]);
        }
    }




    int CreateDayNum = 0;
    bool isReddot;
    public void f_UpdateSevenDataInfo(EM_eSevenDay _EmSevenDay, bool isLoad = false)
    {
        if (!_SevebTaskInfo.ContainsKey(_EmSevenDay))
        {
MessageBox.DEBUG("SevenActivityTaskInfo does not contain a Key" + _EmSevenDay.ToString());
            return;
        }
        isReddot = false;
        SevenActivityTaskPoolDT tSevenPoolDt;
        CreateDayNum = ((GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_SevenActivityTaskPool.OpenSeverTime) / 86400) + 1;
        for (int i = 0; i < _SevebTaskInfo[_EmSevenDay].Count; i++)
        {
            tSevenPoolDt = _SevebTaskInfo[_EmSevenDay][i] as SevenActivityTaskPoolDT;


            //大于当天的不考虑
            if (tSevenPoolDt.m_SevenActivityTaskDT.iDayNum > CreateDayNum)
                break;
            if (CreateDayNum > 7)
                break;
            //已完成的不考虑                     每日
            if (tSevenPoolDt.isGain == 1)
                continue;



            if (isLoad)
            {
                UpdateTask(_EmSevenDay, tSevenPoolDt);//, tSevenPoolDt.m_SevenActivityTaskDT.iCondition2,out tSevenPoolDt.m_result);
            }
            else
            {
                tSevenPoolDt.m_result = m_taskInfo[(EM_eSevenDay)tSevenPoolDt.m_SevenActivityTaskDT.itype];    //服务器的
            }

            //if (tSevenPoolDt.m_SevenActivityTaskDT.iDayNum == CreateDayNum && _EmSevenDay == EM_eSevenDay.eSevenDay_Login)
            if (tSevenPoolDt.m_SevenActivityTaskDT.iDayNum <= CreateDayNum && _EmSevenDay == EM_eSevenDay.eSevenDay_Login) //Tsucode - cho phép nhận quà các ngày đăng nhập trước
            {
                UpdateTaskFinsh(tSevenPoolDt, true);
            }
            else if (_EmSevenDay == EM_eSevenDay.eSevenDay_MonthCard)   //月卡的单独处理
            {
                if (tSevenPoolDt.m_SevenActivityTaskDT.iCondition1 == tSevenPoolDt.m_result[0] && tSevenPoolDt.m_SevenActivityTaskDT.iId == 1102)
                {
                    UpdateTaskFinsh(tSevenPoolDt, true);
                }
                else if (tSevenPoolDt.m_SevenActivityTaskDT.iCondition1 == tSevenPoolDt.m_result[1] && tSevenPoolDt.m_SevenActivityTaskDT.iId == 1103)
                {
                    UpdateTaskFinsh(tSevenPoolDt, true);
                }
            }
            else if (_EmSevenDay == EM_eSevenDay.eSevenDay_Arena_Rank_X || _EmSevenDay == EM_eSevenDay.eSevenDay_Through_Rank_X)    //竞技场的单独处理   排行
            {
                if (tSevenPoolDt.m_result[0] <= tSevenPoolDt.m_SevenActivityTaskDT.iCondition1)
                    UpdateTaskFinsh(tSevenPoolDt, true);
            }
            else
            {
                if (tSevenPoolDt.m_result[0] >= tSevenPoolDt.m_SevenActivityTaskDT.iCondition1 &&
                              tSevenPoolDt.m_SevenActivityTaskDT.iCondition2 == 0)
                    UpdateTaskFinsh(tSevenPoolDt, true);
                else if (tSevenPoolDt.m_result[1] >= tSevenPoolDt.m_SevenActivityTaskDT.iCondition2 &&
                         tSevenPoolDt.m_SevenActivityTaskDT.iCondition2 != 0)
                    UpdateTaskFinsh(tSevenPoolDt, true);
                //tSevenPoolDt.isFinsh = 1;
                else
                    UpdateTaskFinsh(tSevenPoolDt, false);
                //tSevenPoolDt.isFinsh = 0;
            }
            if (tSevenPoolDt.isFinsh == 1 && tSevenPoolDt.isGain != 1 && !isReddot)
            {
                isReddot = true;   //请求一次红点就不需要在此请求
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayTask);
            }

        }
    }

    void UpdateTaskFinsh(SevenActivityTaskPoolDT TaskPoolDT, bool isFinsh)
    {
        if (TaskPoolDT.isFinsh != 1)
        {
            if (isFinsh)
            {
                //MessageBox.DEBUG(string.Format("")):
                f_GetSevenDayAchieve(TaskPoolDT.ITempleteId);
                TaskPoolDT.isFinsh = 1;
            }
            else
            {
                TaskPoolDT.isFinsh = 0;
            }
        }
    }

    void UpdateTask(EM_eSevenDay ttt, SevenActivityTaskPoolDT tSevenActivityTaskPoolDT)//int iContion1, int iContion2, out int[] Icontion)
    {
        if (!m_taskInfo.ContainsKey(ttt))
            m_taskInfo.Add(ttt, new int[2]);

        int iContion1 = tSevenActivityTaskPoolDT.m_SevenActivityTaskDT.iCondition1;
        int iContion2 = tSevenActivityTaskPoolDT.m_SevenActivityTaskDT.iCondition2;
        switch (ttt)
        {
            //case EM_eSevenDay.eSevenDay_Login:
            //    tSevenActivityTaskPoolDT.m_result[0] = 1;
            //    break;
            case EM_eSevenDay.eSevenDay_Days_Recharge_X:
                //TsuComment //tSevenActivityTaskPoolDT.m_result[0] = Data_Pool.m_RechargePool.f_GetAllRechageMoney();
                tSevenActivityTaskPoolDT.m_result[0] = Data_Pool.m_RechargePool.f_GetAllRechageMoney_TsuFunc(); //TsuCode
                break;
            case EM_eSevenDay.eSevenDay_Lv_X:
                tSevenActivityTaskPoolDT.m_result[0] = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
                break;
            case EM_eSevenDay.eSevenDay_FightPower_X:
                tSevenActivityTaskPoolDT.m_result[0] = Data_Pool.m_TeamPool.f_GetTotalBattlePower();
                break;
            case EM_eSevenDay.eSevenDay_MainPve_X:
                tSevenActivityTaskPoolDT.m_result[0] = Data_Pool.m_DungeonPool.m_DungeonMainMax;
                break;
            case EM_eSevenDay.eSevenDay_Battle_X_Card:
                tSevenActivityTaskPoolDT.m_result[0] = Data_Pool.m_TeamPool.f_GetAll().Count;
                break;
            case EM_eSevenDay.eSevenDay_Battle_X_Card_Q_X:
                tSevenActivityTaskPoolDT.m_result[1] = Data_Pool.m_TeamPool.f_GetTeamImporent(iContion1);
                break;
            case EM_eSevenDay.eSevenDay_Battle_X_CardWear_Q_X:
                tSevenActivityTaskPoolDT.m_result[1] = Data_Pool.m_TeamPool.f_GetTeamEquipImporentNum(iContion1);
                break;
            case EM_eSevenDay.eSevenDay_Battle_X_WearEquip_X:
                tSevenActivityTaskPoolDT.m_result[1] = Data_Pool.m_TeamPool.f_GetTeamEquipInten(iContion1, 1);
                break;
            case EM_eSevenDay.eSevenDay_Battle_X_WearRefine_X:
                tSevenActivityTaskPoolDT.m_result[1] = Data_Pool.m_TeamPool.f_GetTeamEquipInten(iContion1, 0);
                break;
            case EM_eSevenDay.eSevenDay_Battle_X_MagicStren_X:
                tSevenActivityTaskPoolDT.m_result[1] = Data_Pool.m_TeamPool.f_GetTeamTreasureIntenNum(iContion1, 1);
                break;
            case EM_eSevenDay.eSevenDay_Battle_X_MagicRefine_X:
                tSevenActivityTaskPoolDT.m_result[1] = Data_Pool.m_TeamPool.f_GetTeamTreasureIntenNum(iContion1, 0);
                break;
            case EM_eSevenDay.eSevenDay_WearEquip_Refine_X:
                tSevenActivityTaskPoolDT.m_result[0] = Data_Pool.m_TeamPool.f_GetTeamEquipMaxRefine();
                break;
            case EM_eSevenDay.eSevenDay_LifStar_X:
                tSevenActivityTaskPoolDT.m_result[1] = Data_Pool.m_TeamPool.f_GetTeamCardTypeLv(iContion1, 1);
                break;
            case EM_eSevenDay.eSevenDay_LifStar_Max:
                tSevenActivityTaskPoolDT.m_result[0] = Data_Pool.m_TeamPool.f_GetTeamCardMax(1);
                break;
            case EM_eSevenDay.eSevenDay_Magic_Refine_Max:
                tSevenActivityTaskPoolDT.m_result[0] = Data_Pool.m_TeamPool.f_GetTeamTreasureMaxLevel();
                break;
            case EM_eSevenDay.eSevenDay_Recharge_X:
                //tSevenActivityTaskPoolDT.m_result[0] =
                //    Data_Pool.m_RechargePool.f_GetDayRechageMoneyMax(ccMath.time_t2DateTime(OpenSeverTime + (tSevenActivityTaskPoolDT.m_SevenActivityTaskDT.iDayNum - 1) * 86400));
                tSevenActivityTaskPoolDT.m_result[0] =
                    Data_Pool.m_RechargePool.f_GetDayRechageMoneyMax_TsuFunc(ccMath.time_t2DateTime(OpenSeverTime + (tSevenActivityTaskPoolDT.m_SevenActivityTaskDT.iDayNum - 1) * 86400)); //TsuCode
                break;
            case EM_eSevenDay.eSevenDay_MonthCard:
                tSevenActivityTaskPoolDT.m_result[0] = Data_Pool.m_RechargePool.f_GetMonthCard(1) ? 1 : 0;
                tSevenActivityTaskPoolDT.m_result[1] = Data_Pool.m_RechargePool.f_GetMonthCard(2) ? 1 : 0;
                break;
            default:
MessageBox.DEBUG("Type not handled by client" + ttt.ToString());
                break;
        }
        ////m_taskInfo[ttt] = Icontion;
    }


    #region   返回

    void TaskAchieve(object obj)
    {
        if ((int)obj == (int)eMsgOperateResult.OR_Succeed)
        {
            if (_UpdateUnHold != null)
            {
                _UpdateUnHold(obj);
            }
MessageBox.DEBUG("SevenActivity updated successfully");
        }
        else
        {
MessageBox.DEBUG("SevenActivity update failed");
        }
    }

    void UpdateLegion(object obj)
    {
        if ((int)obj == (int)eMsgOperateResult.OR_Succeed)
        {
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_Legion_Make][0] = LegionMain.GetInstance().m_LegionInfor.m_iLegionId > 0 ? 1 : 0;
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_Legion_Publicity_H][0] = LegionMain.GetInstance().m_LegionInfor.m_iSacrificeType == 3 ? 1 : 0;
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_Legion_Publicity_L][0] = LegionMain.GetInstance().m_LegionInfor.m_iSacrificeType == 1 ? 1 : 0;
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_Legion_Publicity_M][0] = LegionMain.GetInstance().m_LegionInfor.m_iSacrificeType == 2 ? 1 : 0;
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_Legion_Make);
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_Legion_Publicity_H);
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_Legion_Publicity_L);
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_Legion_Publicity_M);
        }
        else
        {
MessageBox.DEBUG("SevenActivity query failed");
        }
    }

    void UpdateRunningMan(object obj)
    {
        if ((int)obj == (int)eMsgOperateResult.OR_Succeed)
        {
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_Through_Star_X][0] = Data_Pool.m_RunningManPool.m_iHistoryStarNum;
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_Through_Star_X);
        }
        else
        {
MessageBox.DEBUG("SevenActivity, RunningMan query failed");
        }
    }
    void UpdateShopLottery(object obj)
    {
        if ((int)obj == (int)eMsgOperateResult.OR_Succeed)
        {
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_FightGeneral_Draw_X][0] = (Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.NorAd) as ShopLotteryPoolDT).totalTimes;
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_GodGeneral_Draw_X][0] = (Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd) as ShopLotteryPoolDT).totalTimes;
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_FightGeneral_Draw_X);
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_GodGeneral_Draw_X);
        }
        else
        {
MessageBox.DEBUG("SevenActivity, ShopLottery query failed");
        }
    }

    void UpdateDmgMax(object obj)
    {
        Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_Rebels_Dam_Max][0] = Data_Pool.m_RebelArmyPool.tRebelInit.maxDmg * 10000;
        _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_Rebels_Dam_Max);
    }

    void UpdateOpenServ(object obj)
    {
        if ((int)obj == (int)eMsgOperateResult.OR_Succeed)
        {
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_AllServ_Welfare_X][0] = Data_Pool.m_OpenServFundPool.m_buyFundCount;
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_AllServ_Welfare_X);
        }
        else
MessageBox.DEBUG("SevenActivity, OpenServFund query failed");
    }

    void UpdateActMonthCard(object obj)
    {
        if ((int)obj == (int)eMsgOperateResult.OR_Succeed)
        {
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_MonthCard][0] =
                  Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy25 ? 25 : 0;
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_MonthCard][0] +=
                 Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy50 ? 50 : 0;
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_MonthCard);
        }
        else
        {
MessageBox.DEBUG("SevenActivity, MonthCard query failed");
        }
    }

    void UpdateDungeon(object obj)
    {
        if ((int)obj == (int)eMsgOperateResult.OR_Succeed)
        {
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_MainPve_Star_X][0] = Data_Pool.m_DungeonPool.f_GetMaxStraNum((int)EM_Fight_Enum.eFight_DungeonMain);
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_ElitePve_Clearance_X][0] = Data_Pool.m_DungeonPool.m_DungeonEliteMax;
            Data_Pool.m_SevenActivityTaskPool.m_taskInfo[EM_eSevenDay.eSevenDay_ElitePve_Star_X][0] = Data_Pool.m_DungeonPool.f_GetMaxStraNum((int)EM_Fight_Enum.eFight_DungeonElite);
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_MainPve_Star_X);
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_ElitePve_Clearance_X);
            _SendTaskFinsh((int)EM_eSevenDay.eSevenDay_ElitePve_Star_X);
        }
        else
        {
MessageBox.DEBUG("SevenActivity, Dungeon query failed");
        }
    }

    void _SendTaskFinsh(int type)
    {
        SevenActivityTaskPoolDT tSevenPoolDt;
        for (int i = 0; i < f_GetAllForData3(type).Count; i++)
        {
            tSevenPoolDt = f_GetAllForData3(type)[i] as SevenActivityTaskPoolDT;
            if (tSevenPoolDt.isFinsh == 1)
                continue;
            if (CreateDayNum < tSevenPoolDt.m_SevenActivityTaskDT.iDayNum)
                break;


            if (m_taskInfo[(EM_eSevenDay)tSevenPoolDt.m_SevenActivityTaskDT.itype][0] >= tSevenPoolDt.m_SevenActivityTaskDT.iCondition1 &&
                           tSevenPoolDt.m_SevenActivityTaskDT.iCondition2 == 0)
            {
                MessageBox.DEBUG(m_taskInfo[(EM_eSevenDay)tSevenPoolDt.m_SevenActivityTaskDT.itype][0] + "       " + tSevenPoolDt.m_SevenActivityTaskDT.iCondition1);
                f_GetSevenDayAchieve(tSevenPoolDt.ITempleteId);
            }
            else if (m_taskInfo[(EM_eSevenDay)tSevenPoolDt.m_SevenActivityTaskDT.itype][1] >= tSevenPoolDt.m_SevenActivityTaskDT.iCondition2 &&
                     tSevenPoolDt.m_SevenActivityTaskDT.iCondition2 != 0)
            {
                f_GetSevenDayAchieve(tSevenPoolDt.ITempleteId);
            }

        }
    }

    void _SendTaskFinshRecharge()
    {
        SevenActivityTaskPoolDT tSevenPoolDt;
        for (int i = 0; i < f_GetAllForData3(45).Count; i++)
        {
            tSevenPoolDt = f_GetAllForData3(45)[i] as SevenActivityTaskPoolDT;
            if (tSevenPoolDt.m_SevenActivityTaskDT.iDayNum == CreateDayNum)
            {
                if (m_taskInfo[(EM_eSevenDay)tSevenPoolDt.m_SevenActivityTaskDT.itype][0] >= tSevenPoolDt.m_SevenActivityTaskDT.iCondition1)
                    f_GetSevenDayAchieve(tSevenPoolDt.ITempleteId);
            }

        }


    }
    #endregion



}

